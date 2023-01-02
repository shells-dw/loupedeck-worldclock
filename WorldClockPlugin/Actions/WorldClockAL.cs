namespace Loupedeck.WorldClockPlugin
{
    using System;
    using System.Globalization;

    using Loupedeck.WorldClockPlugin.Helpers;

    using NodaTime;
    using NodaTime.Extensions;



    public class WorldClockAL : PluginDynamicCommand
    {
        private WorldClockPlugin _plugin;
        public WorldClockAL()
            : base(displayName: "Time Analog + Location", description: "Shows an analog watchface with location", groupName: "Time") => this.MakeProfileAction("tree");
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
            ZonedDateTime today = clock.GetCurrentZonedDateTime();
            Int32 idx = actionParameter.LastIndexOf("/");
            var secHandLength = 35;
            var minHandLength = 30;
            var hrHandLength = 20;
            Int32[] handCoord = new Int32[2];
            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.Clear(BitmapColor.Black);
                var wx1 = bitmapBuilder.Width * 0.5;
                var wy1 = bitmapBuilder.Width * 0.5;
                bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(EmbeddedResources.FindFile("watchface1.png")));
                handCoord = HelperFunctions.MSCoord(Int32.Parse(today.ToString("ss", CultureInfo.InvariantCulture)), secHandLength, bitmapBuilder.Width, bitmapBuilder.Height);
                bitmapBuilder.DrawLine(handCoord[0], handCoord[1], (Int32)wx1, (Int32)wy1, new BitmapColor(255, 0, 0), 1);
                handCoord = HelperFunctions.MSCoord(Int32.Parse(today.ToString("mm", CultureInfo.InvariantCulture)), minHandLength, bitmapBuilder.Width, bitmapBuilder.Height);
                bitmapBuilder.DrawLine(handCoord[0], handCoord[1], (Int32)wx1, (Int32)wy1, new BitmapColor(120, 120, 120), 2);
                handCoord = HelperFunctions.HrCoord(Int32.Parse(today.ToString("hh", CultureInfo.InvariantCulture)) % 12, Int32.Parse(today.ToString("mm", CultureInfo.InvariantCulture)), hrHandLength, bitmapBuilder.Width, bitmapBuilder.Height);
                bitmapBuilder.DrawLine(handCoord[0], handCoord[1], (Int32)wx1, (Int32)wy1, new BitmapColor(120, 120, 120), 3);
                if (!String.IsNullOrEmpty(actionParameter))
                {
                    var lx1 = bitmapBuilder.Width * 0.1;
                    var lw = bitmapBuilder.Width * 0.8;
                    var ly1 = bitmapBuilder.Height * 0.57;
                    var lh = bitmapBuilder.Height * 0.3;

                    bitmapBuilder.DrawText(actionParameter.Substring(idx + 1).Replace("_", " "), (Int32)lx1, (Int32)ly1, (Int32)lw, (Int32)lh, new BitmapColor(255, 255, 255, 200), imageSize == PluginImageSize.Width90 ? 13 : 9, imageSize == PluginImageSize.Width90 ? 2 : 0, 10);
                }
                return bitmapBuilder.ToImage();
            }
        }
    }
}
