namespace Loupedeck.WorldClockPlugin
{
    using System;
    using System.Globalization;
    using System.Threading;

    using Loupedeck.WorldClockPlugin.Helpers;

    using NodaTime;
    using NodaTime.Extensions;



    public class WorldDate : PluginDynamicCommand
    {
        private WorldClockPlugin _plugin;
        public WorldDate()
            : base(displayName: "Date", description: "Display date", groupName: "Date") => this.MakeProfileAction("tree");
        protected override PluginProfileActionData GetProfileActionData()
        {
            var tree = new PluginProfileActionTree("Select Location");
            tree.AddLevel("Zone");
            tree.AddLevel("Location");

            foreach (String zone in Globals.tzNames.AllKeys)
            {
                var node = tree.Root.AddNode(zone);
                foreach (String area in Globals.tzNames[zone].Split(","))
                {
                    node.AddItem($"{zone}/{area}", area.Replace("_", " "));
                }
            }
            return tree;
        }
        protected override Boolean OnLoad()
        {
            this._plugin = base.Plugin as WorldClockPlugin;
            if (this._plugin is null)
            {
                return false;
            }

            this._plugin.Tick += (sender, e) => this.ActionImageChanged("");
            return base.OnLoad();
        }

        protected override void RunCommand(String actionParameter) => this.ActionImageChanged(actionParameter);

        // update command image (nothing to update here per se, but that's called to draw whatever is shown on the Loupedeck)
        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            DateTimeZone zone = DateTimeZoneProviders.Tzdb[actionParameter];
            ZonedClock clock = SystemClock.Instance.InZone(zone);
            LocalDate test = clock.GetCurrentDate();
            ZonedDateTime today = clock.GetCurrentZonedDateTime();
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

            Int32 idx = actionParameter.LastIndexOf("/");
            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.Clear(BitmapColor.Black);
                if (!String.IsNullOrEmpty(actionParameter))
                {
                    var x1 = bitmapBuilder.Width * 0.1;
                    var w = bitmapBuilder.Width * 0.8;
                    var y1 = bitmapBuilder.Height * 0.35;
                    var h = bitmapBuilder.Height * 0.3;

                    bitmapBuilder.DrawText(today.LocalDateTime.ToString(currentCulture.DateTimeFormat.ShortDatePattern, currentCulture), (Int32)x1, (Int32)y1, (Int32)w, (Int32)h, BitmapColor.White, imageSize == PluginImageSize.Width90 ? 15 : 9, imageSize == PluginImageSize.Width90 ? 7 : 5, 10);
                }
                return bitmapBuilder.ToImage();
            }
        }
    }
}
