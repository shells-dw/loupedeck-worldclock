namespace Loupedeck.WorldClockPlugin.l10n
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal class L10n
    {
        private readonly String[] SupportedLanguageCodes = { "de", "en", "fr" };
        private readonly WorldClockPlugin _plugin;
        private Dictionary<String, JObject> actionL10n;

        public L10n(WorldClockPlugin plugin)
        {
            this._plugin = plugin;
            this.ReadL10nFiles();
        }
        private void ReadL10nFiles()
        {
            this.actionL10n = new Dictionary<String, JObject>();
            foreach (var code in this.SupportedLanguageCodes)
            {
                this.actionL10n[code] = GetL7dData(code);
            }
        }
        private static JObject GetL7dData(String lc)
        {
            var dataStream = EmbeddedResources.GetStream(EmbeddedResources.FindFile("l10n-" + lc + ".json"));
            var result = new JObject();
            if (dataStream.Length > 0)
            {
                var serializer = new JsonSerializer();
                var reader = new StreamReader(dataStream, Encoding.UTF8);
                using (var jtr = new JsonTextReader(reader))
                {
                    result = serializer.Deserialize<JObject>(jtr);
                }
                return result;
            }
            else
            {
                return result;
            }
        }

        public String GetCurrentLanguageCode()
        {
            var CurrentLanguageCode = this._plugin.Localization.LoupedeckLanguage;
            var twoCharacterLanguageCode = CurrentLanguageCode.Substring(0, 2);
            return twoCharacterLanguageCode;
        }

        public Dictionary<String, String> GetL7dNames(String actionId)
        {
            // Get the current language code:
            var LanguageCode = this.GetCurrentLanguageCode();
            Dictionary<String, String> result;
            try
            {
                result = new Dictionary<String, String>() {
                { "displayName", (String)this.actionL10n[LanguageCode][actionId][0]["displayName"] },
                { "description", (String)this.actionL10n[LanguageCode][actionId][0]["displayName"] },
                { "groupName", (String)this.actionL10n[LanguageCode][actionId][0]["groupName"] },
                { "zone", (String)this.actionL10n[LanguageCode][actionId][0]["zone"] },
                { "location", (String)this.actionL10n[LanguageCode][actionId][0]["location"] }
                };
            }
            catch
            {
                result = new Dictionary<String, String>() {
                { "displayName", (String)this.actionL10n["en"][actionId][0]["displayName"] },
                { "description", (String)this.actionL10n["en"][actionId][0]["displayName"] },
                { "groupName", (String)this.actionL10n["en"][actionId][0]["groupName"] },
                { "zone", (String)this.actionL10n["en"][actionId][0]["zone"] },
                { "location", (String)this.actionL10n["en"][actionId][0]["location"] }
                };
            }
            return result;
        }
    }
}
