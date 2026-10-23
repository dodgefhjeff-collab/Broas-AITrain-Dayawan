using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WaterStation.Client
{
    public static class ConfigureManager
    {
        private const string ConfigurePath = "appsettings.json";
        private static Dictionary<string, string> AppSettings = new();
        private static bool IsInit = false;
        private static void Init()
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurePath);
            using (StreamReader reader = new (jsonPath, Encoding.UTF8))
            {
                string jsonData = reader.ReadToEnd();
                if (!string.IsNullOrEmpty(jsonData))
                    AppSettings = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);
            }
            IsInit = true;
        }
        public static string GetAppSetting(string key)
        {
            if (!IsInit)
                Init();
            if (AppSettings.ContainsKey(key))
                return AppSettings[key];
            return "";
        }
    }
}
