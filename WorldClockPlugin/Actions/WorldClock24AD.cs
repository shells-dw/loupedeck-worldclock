namespace Loupedeck.WorldClockPlugin
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;
    using System.Threading;

    using Loupedeck.WorldClockPlugin.Helpers;
    using Loupedeck.WorldClockPlugin.l10n;

    using NodaTime;
    using NodaTime.Extensions;



    public class WorldClock24AD : PluginDynamicCommand
    {
        private WorldClockPlugin _plugin;
        private L10n _l10n;
        private Dictionary<String, String> l7dValues;
        public WorldClock24AD()
            : base() => this.MakeProfileAction("tree");
        protected override PluginProfileActionData GetProfileActionData()
        {
            var tree = new PluginProfileActionTree("Select location");
            tree.AddLevel(this.l7dValues["zone"]);
            tree.AddLevel(this.l7dValues["location"]);

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
            this._l10n = new L10n(this._plugin);
            this.l7dValues = this._l10n.GetL7dNames("time24AD");
            if (this.l7dValues != null)
            {
                this.DisplayName = this.l7dValues["displayName"];
                this.Description = this.l7dValues["description"];
                this.GroupName = this.l7dValues["groupName"];
            }
            else
            {
                this.DisplayName = "Time Analog + Digital (24h Format)";
                this.GroupName = "Analog";
                this._plugin.Log.Info($"12S : l7dValues was empty or null: DisplayName: {this.l7dValues["displayName"]}, groupName: {this.l7dValues["groupName"]}.");
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
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
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
                    var tx1 = bitmapBuilder.Width * 0.1;
                    var tw = bitmapBuilder.Width * 0.8;
                    var ty1 = bitmapBuilder.Height * 0.23;
                    var th = bitmapBuilder.Height * 0.3;

                    bitmapBuilder.DrawText(today.ToString("HH:mm", currentCulture), (Int32)tx1, (Int32)ty1, (Int32)tw, (Int32)th, BitmapColor.White, imageSize == PluginImageSize.Width90 ? 18 : 9, imageSize == PluginImageSize.Width90 ? 12 : 5);
                }
                return bitmapBuilder.ToImage();
            }
        }
    }
}
