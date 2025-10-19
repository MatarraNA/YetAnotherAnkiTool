using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YetAnotherAnkiTool.Core.Config
{
    public static class Config
    {
        private static string CONFIG_PATH => Path.Combine(Environment.CurrentDirectory, "Settings.json");
        private static JsonConfig? _config = null;
        public static JsonConfig Configuration
        {
            get
            {
                if (_config != null) return _config;

                try
                {
                    if (File.Exists(CONFIG_PATH))
                    {
                        var json = File.ReadAllText(CONFIG_PATH);
                        _config = JsonSerializer.Deserialize<JsonConfig>(json);
                        if (_config != null) return _config;
                    }
                }
                catch(Exception ex)
                {
                    // deserialization failed. throw exception and terminate
                    MessageBox.Show($"Error: Failed to deserialize configuration file. There is an error in the json formatting.\n\n{ex.Message}");
                    Environment.Exit(-1);
                }

                // Create default config and persist it
                _config = new JsonConfig();

                var jsonOut = JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(CONFIG_PATH, jsonOut);

                return _config;
            }
        }

        public static void SaveConfig()
        {
            if (_config == null) return;

            var json = JsonSerializer.Serialize(_config, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(CONFIG_PATH, json);
        }


        public class JsonConfig
        {
            public string AnkiWordFieldName { get; set; } = "Expression";
            public string AnkiImgFieldName { get; set; } = "Picture";
            public string AnkiSoundFieldName { get; set; } = "Audio";
            public string AnkiMediaFolderPath { get; set; } = "C:\\Users\\...\\AppData\\Roaming\\Anki2\\User 1\\collection.media";
            public string AnkiPort { get; set; } = "8765";
            public string AnkiAddress { get; set; } = "127.0.0.1";
            public float AnkiAudioOutputGain { get; set; } = 1.0f;
            public int AnkiImgWidthOverride { get; set; } = 0;
            public int AnkiImgHeightOverride { get; set; } = 400;
        }
    }
}
