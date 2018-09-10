using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace LemonLime.Common
{
    public class Settings
    {
        public static string SettingsFile;

        // The default settings.
        private static Dictionary<string, string> DefaultSettings = new Dictionary<string, string>
        {
        };

        // The current settings.
        private static Dictionary<string, string> Values = new Dictionary<string, string>();

        /// <summary>
        /// Loads the settings from the file
        /// </summary>
        public static void Load()
        {
            if (System.IO.File.Exists(SettingsFile))
            {
                string[] lines = System.IO.File.ReadAllLines(SettingsFile);
                foreach (var line in lines)
                {
                    var arr = line.Split('=');
                    if (arr.Length == 2)
                    {
                        string value = string.Empty;
                        foreach (var str in arr.Skip(1))
                        {
                            value += str + ' ';
                        }
                        // Remove the trailing space
                        value = value.Substring(0, value.Length - 1);
                        // Add the setting
                        Settings.Values.Add(arr[0], value);
                    }
                }
            }
            else
            {
                Values = DefaultSettings;
            }
        }

        /// <summary>
        /// Writes the settings to the file.
        /// </summary>
        public static void Save()
        {
            string str = string.Empty;
            foreach (var setting in Values)
            {
                str += $"{setting.Key}={setting.Value}\n";
            }
            System.IO.File.WriteAllText(SettingsFile, str);
        }

        public static string Get(string key)
        {
            return Values.Keys.Contains(key) ? Values[key] : DefaultSettings[key];
        }

        public static void Set(string key, string value)
        {
            if (Values.Keys.Contains(key))
            {
                Values[key] = value;
            }
            else
            {
                Values.Add(key, value);
            }
        }
    }
}
