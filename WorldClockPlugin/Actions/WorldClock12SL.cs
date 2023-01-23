namespace Loupedeck.WorldClockPlugin
{
    using System;
    using System.Globalization;
    using System.Collections.Generic;

    using Loupedeck.WorldClockPlugin.Helpers;
    using Loupedeck.WorldClockPlugin.l10n;

    using NodaTime;
    using NodaTime.Extensions;



    public class WorldClock12SL : PluginDynamicCommand
    {
        private WorldClockPlugin _plugin;
        private L10n _l10n;
        private Dictionary<String, String> l7dValues;
        public WorldClock12SL()
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
            this.l7dValues = this._l10n.GetL7dNames("time12SL");
            if (this.l7dValues != null)
            {
                this.DisplayName = this.l7dValues["displayName"];
                this.Description = this.l7dValues["description"];
                this.GroupName = this.l7dValues["groupName"];
            }
            else
            {
                this.DisplayName = "Time + Location + Seconds (12h Format)";
                this.GroupName = "Digital";
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

            Int32 idx = actionParameter.LastIndexOf("/");
            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.Clear(BitmapColor.Black);
                if (!String.IsNullOrEmpty(actionParameter))
                {
                    var x1 = bitmapBuilder.Width * 0.1;
                    var w = bitmapBuilder.Width * 0.8;
                    var y1 = bitmapBuilder.Height * 0.45;
                    var y2 = bitmapBuilder.Height * 0.65;
                    var y3 = bitmapBuilder.Height * 0.1;
                    var h = bitmapBuilder.Height * 0.3;

                    bitmapBuilder.DrawText(actionParameter.Substring(idx + 1).Replace("_", " "), (Int32)x1, (Int32)y3, (Int32)w, (Int32)h, new BitmapColor(255, 255, 255, 200), imageSize == PluginImageSize.Width90 ? 13 : 9, imageSize == PluginImageSize.Width90 ? 8 : 6, 6);
                    bitmapBuilder.DrawText(today.ToString("hh:mm:ss", CultureInfo.InvariantCulture), (Int32)x1, (Int32)y1, (Int32)w, (Int32)h, BitmapColor.White, imageSize == PluginImageSize.Width90 ? 17 : 9, imageSize == PluginImageSize.Width90 ? 11 : 8);
                    bitmapBuilder.DrawText(today.ToString("tt", CultureInfo.InvariantCulture), (Int32)x1, (Int32)y2, (Int32)w, (Int32)h, BitmapColor.White, imageSize == PluginImageSize.Width90 ? 12 : 9, imageSize == PluginImageSize.Width90 ? 10 : 8, 6);
                }
                return bitmapBuilder.ToImage();
            }
        }
    }
}
