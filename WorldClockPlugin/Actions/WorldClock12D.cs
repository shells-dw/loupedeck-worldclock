namespace Loupedeck.WorldClockPlugin
{
    using System;
    using System.Globalization;
    using System.Threading;

    using Loupedeck.WorldClockPlugin.Helpers;

    using NodaTime;
    using NodaTime.Extensions;



    public class WorldClock12D : PluginDynamicCommand
    {
        private WorldClockPlugin _plugin;
        public WorldClock12D()
            : base(displayName: "Time + Date (12h Format)", description: "Shows time in 12h format", groupName: "Date") => this.MakeProfileAction("tree");
        protected override PluginProfileActionData GetProfileActionData()
        {
            var tree = new PluginProfileActionTree("Select location");
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
                if (!String.IsNullOrEmpty(actionParameter))
                {
                    var x1 = bitmapBuilder.Width * 0.1;
                    var x3 = bitmapBuilder.Width * 0.1;
                    var w = bitmapBuilder.Width * 0.8;
                    var y1 = bitmapBuilder.Height * 0.25;
                    var y2 = bitmapBuilder.Height * 0.4;
                    var y3 = bitmapBuilder.Height * 0.6;
                    var h = bitmapBuilder.Height * 0.3;

                    bitmapBuilder.DrawText(today.ToString("hh:mm", CultureInfo.InvariantCulture), (Int32)x1, (Int32)y1, (Int32)w, (Int32)h, BitmapColor.White, imageSize == PluginImageSize.Width90 ? 33 : 9, imageSize == PluginImageSize.Width90 ? 11 : 8);
                    bitmapBuilder.DrawText(today.ToString("tt", CultureInfo.InvariantCulture), (Int32)x1, (Int32)y2, (Int32)w, (Int32)h, BitmapColor.White, imageSize == PluginImageSize.Width90 ? 12 : 9, imageSize == PluginImageSize.Width90 ? 11 : 8);
                    bitmapBuilder.DrawText(today.LocalDateTime.ToString(currentCulture.DateTimeFormat.ShortDatePattern, currentCulture), (Int32)x3, (Int32)y3, (Int32)w, (Int32)h, BitmapColor.White, imageSize == PluginImageSize.Width90 ? 13 : 9, imageSize == PluginImageSize.Width90 ? 16 : 8, 1);
                }
                return bitmapBuilder.ToImage();
            }
        }
    }
}