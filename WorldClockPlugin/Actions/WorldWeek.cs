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
    using NodaTime.Calendars;

    public class WorldWeek : PluginDynamicCommand
    {
        private WorldClockPlugin _plugin;
        private L10n _l10n;
        private Dictionary<String, String> l7dValues;
        public WorldWeek()
            : base() { }
        
        protected override Boolean OnLoad()
        {
            this._plugin = base.Plugin as WorldClockPlugin;
            if (this._plugin is null)
            {
                return false;
            }
            this._l10n = new L10n(this._plugin);
            this.l7dValues = this._l10n.GetL7dNames("week");
            if (this.l7dValues != null)
            {
                this.AddParameter("week", this.l7dValues["displayName"], "");
            }
            else
            {
                this.AddParameter("week", "Week", "Digital");
                this._plugin.Log.Info($"12S : l7dValues was empty or null: DisplayName: {this.l7dValues["displayName"]}, groupName: {this.l7dValues["groupName"]}.");
            }
            this._plugin.Tick += (sender, e) => this.ActionImageChanged("");
            return base.OnLoad();
        }

        protected override void RunCommand(String actionParameter) => this.ActionImageChanged(actionParameter);

        // update command image (nothing to update here per se, but that's called to draw whatever is shown on the Loupedeck)
        protected override BitmapImage GetCommandImage(String actionParameter, PluginImageSize imageSize)
        {
            DateTimeZone zone = DateTimeZoneProviders.Tzdb.GetSystemDefault();
            ZonedClock clock = SystemClock.Instance.InZone(zone);
            ZonedDateTime today = clock.GetCurrentZonedDateTime();
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;

            IWeekYearRule rule = WeekYearRules.Iso;

            Int32 idx = actionParameter.LastIndexOf("/");
            using (var bitmapBuilder = new BitmapBuilder(imageSize))
            {
                bitmapBuilder.Clear(BitmapColor.Black);
                if (!String.IsNullOrEmpty(actionParameter))
                {
                    var x1 = bitmapBuilder.Width * 0.1;
                    var w = bitmapBuilder.Width * 0.8;
                    var y1 = bitmapBuilder.Height * 0.58;
                    var y2 = bitmapBuilder.Height * 0.05;
                    var h = bitmapBuilder.Height * 0.3;

                    bitmapBuilder.DrawText(rule.GetWeekOfWeekYear(today.LocalDateTime.Date).ToString(), (Int32)x1, (Int32)y1, (Int32)w, (Int32)h, BitmapColor.White, imageSize == PluginImageSize.Width90 ? 40 : 9, imageSize == PluginImageSize.Width90 ? 12 : 5);
                    bitmapBuilder.DrawText(this.l7dValues["additionalText"], (Int32)x1, (Int32)y2, (Int32)w, (Int32)h, BitmapColor.White, imageSize == PluginImageSize.Width90 ? 13 : 9, imageSize == PluginImageSize.Width90 ? 7 : 5, 6);
                }
                return bitmapBuilder.ToImage();
            }
        }
    }
}
