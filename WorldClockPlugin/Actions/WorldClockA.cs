namespace Loupedeck.WorldClockPlugin
{
    using System;
    using System.Globalization;

    using Loupedeck.WorldClockPlugin.Helpers;

    using NodaTime;
    using NodaTime.Extensions;

    // This class implements an example adjustment that counts the rotation ticks of a dial.

    public class WorldClockA : PluginDynamicCommand
    {
        private WorldClockPlugin _plugin;
        public WorldClockA()
            : base(displayName: "Time Analog", description: "Shows an analog watchface with digital time", groupName: "Time") => this.MakeProfileAction("tree");
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
                return false;

            this._plugin.Tick += (sender, e) => this.ActionImageChanged("");
            return base.OnLoad();
        }

        protected override void RunCommand(String actionParameter) => this.ActionImageChanged(actionParameter);

        // update command image (nothing to update here per se, but that's called to draw whatever is shown on the Loupedeck)
        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            if (this.Plugin.PluginStatus.Status.ToString() != "Normal")
            {
                using (var bitmapBuilder = new BitmapBuilder(imageSize))
                {
                    bitmapBuilder.Clear(BitmapColor.Black);
                    bitmapBuilder.DrawText("Error", x: 5, y: 35, width: 70, height: 40, fontSize: 20, color: new BitmapColor(255, 255, 255, 255));
                    return bitmapBuilder.ToImage();
                }
            }
            
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
                var x1 = bitmapBuilder.Width * 0.5;
                var y1 = bitmapBuilder.Width * 0.5;
                bitmapBuilder.SetBackgroundImage(EmbeddedResources.ReadImage(EmbeddedResources.FindFile("watchface1.png")));
                handCoord = HelperFunctions.MSCoord(Int32.Parse(today.ToString("ss", CultureInfo.InvariantCulture)), secHandLength, bitmapBuilder.Width, bitmapBuilder.Height);
                bitmapBuilder.DrawLine(handCoord[0], handCoord[1], (Int32)x1, (Int32)y1, new BitmapColor(255, 0, 0), 1); 
                handCoord = HelperFunctions.MSCoord(Int32.Parse(today.ToString("mm", CultureInfo.InvariantCulture)), minHandLength, bitmapBuilder.Width, bitmapBuilder.Height);
                bitmapBuilder.DrawLine(handCoord[0], handCoord[1], (Int32)x1, (Int32)y1, new BitmapColor(120, 120, 120), 2);
                handCoord = HelperFunctions.HrCoord(Int32.Parse(today.ToString("hh", CultureInfo.InvariantCulture)) % 12, Int32.Parse(today.ToString("mm", CultureInfo.InvariantCulture)), hrHandLength, bitmapBuilder.Width, bitmapBuilder.Height);
                bitmapBuilder.DrawLine(handCoord[0], handCoord[1], (Int32)x1, (Int32)y1, new BitmapColor(120, 120, 120), 3);

                return bitmapBuilder.ToImage();
            }
        }
    }
}
