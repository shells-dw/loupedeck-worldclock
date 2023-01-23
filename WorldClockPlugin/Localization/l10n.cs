namespace Loupedeck.WorldClockPlugin.l10n
{
    using System;
    using System.Collections.Generic;


    internal class L10n
    {
        private readonly String[] SupportedLanguageCodes = { "de", "en", "fr" };
        private readonly WorldClockPlugin _plugin;
        private Dictionary<String, dynamic> actionL10n;

        public L10n(WorldClockPlugin plugin)
        {
            this._plugin = plugin;
            this.ReadL10nFiles();
        }
        private void ReadL10nFiles()
        {
            this.actionL10n = new Dictionary <String, dynamic>();
            foreach (var code in this.SupportedLanguageCodes)
            {
                this.actionL10n[code] = GetL7dData(code);
            }
        }
        private static dynamic GetL7dData(String lc)
        {
            var embededText = EmbeddedResources.ReadTextFile(EmbeddedResources.FindFile("l10n-" + lc + ".json"));
            var result = Loupedeck.JsonHelpers.DeserializeObject<dynamic>(embededText);
            return result;
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
            Dictionary <String, String> result = new Dictionary<String, String>();
            try
            {
                result = new Dictionary<String, String>() {
                            { "displayName", (String)this.actionL10n[LanguageCode][actionId][0]["displayName"] },
                            { "description", (String)this.actionL10n[LanguageCode][actionId][0]["description"] },
                            { "groupName", (String)this.actionL10n[LanguageCode][actionId][0]["groupName"] },
                            { "zone", (String)this.actionL10n[LanguageCode][actionId][0]["zone"] },
                            { "location", (String)this.actionL10n[LanguageCode][actionId][0]["location"] },
                            { "additionalText", (String)this.actionL10n[LanguageCode][actionId][0]["additionalText"] }
                            };
            }
            catch
            {
                result = new Dictionary<String, String>() {
                            { "displayName", (String)this.actionL10n["en"][actionId][0]["displayName"] },
                            { "description", (String)this.actionL10n["en"][actionId][0]["description"] },
                            { "groupName", (String)this.actionL10n["en"][actionId][0]["groupName"] },
                            { "zone", (String)this.actionL10n["en"][actionId][0]["zone"] },
                            { "location", (String)this.actionL10n["en"][actionId][0]["location"] },
                            { "additionalText", (String)this.actionL10n["en"][actionId][0]["additionalText"] }
                            };
            }
 
            return result;
        }
    }
}
