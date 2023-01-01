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
        public static Int32[] MSCoord(Int32 val, Int32 handlength, Int32 width, Int32 height)
        {
            var x = width / 2;
            var y = height / 2;
            Int32[] coord = new Int32[2];
            val *= 6;

            if (val >= 0 && val <= 180)
            {
                coord[0] = x + (Int32)(handlength * Math.Sin(Math.PI * val / 180));
                coord[1] = y - (Int32)(handlength * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = x - (Int32)(handlength * -Math.Sin(Math.PI * val / 180));
                coord[1] = y - (Int32)(handlength * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }

        public static Int32[] HrCoord(Int32 hval, Int32 mval, Int32 handlength, Int32 width, Int32 height)
        {
            var x = width / 2;
            var y = height / 2;
            Int32[] coord = new Int32[2];

            var val = (Int32)(hval * 30 + mval * 0.5);

            if (val >= 0 && val <= 180)
            {
                coord[0] = x + (Int32)(handlength * Math.Sin(Math.PI * val / 180));
                coord[1] = y - (Int32)(handlength * Math.Cos(Math.PI * val / 180));
            }
            else
            {
                coord[0] = x - (Int32)(handlength * -Math.Sin(Math.PI * val / 180));
                coord[1] = y - (Int32)(handlength * Math.Cos(Math.PI * val / 180));
            }
            return coord;
        }
    }
}
