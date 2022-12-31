namespace Loupedeck.WorldClockPlugin.Helpers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    internal class HelperFunctions
    {
        public static void ReadTzData() => CreateListFromResource("Loupedeck.WorldClockPlugin.Ressources.tzNames");//
        public static void CreateListFromResource(String resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    return;
                }
                Byte[] ba = new Byte[resourceStream.Length];
                resourceStream.Read(ba, 0, ba.Length);
                var byteArray = ba;
                var str = System.Text.Encoding.Default.GetString(byteArray);
                var tzList = str.Split(',').ToList();
                foreach (var tz in tzList)
                {
                    String tzCleaned = tz.Replace("\n", "").Replace("\r", "");
                    Int32 idx = tzCleaned.IndexOf("/");
                        if (idx != -1)
                        {
                            Globals.tzNames.Add(tzCleaned.Substring(0, idx), tzCleaned.Substring(idx + 1));
                        }
                }
            }
        }

    }
}
