using System;
using System.IO;
using Newtonsoft.Json;

namespace SlackConsole
{
    public class ConfigReader
    {
        private readonly String ConfigFileName = "config.json";

        public Config ReadOrCreateIfDoesNotExist()
        {
            try
            {
                return ReadConfig();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Please enter API key");
                var apiKey = Console.ReadLine();
                var config = new Config
                {
                    ApiKey = apiKey
                };
                SaveConfig(config);
                return config;
            }
        }

        private Config ReadConfig()
        {
            using (StreamReader r = new StreamReader(ConfigFileName))
            {
                string json = r.ReadToEnd();
                return JsonConvert.DeserializeObject<Config>(json);
            }
        }

        private void SaveConfig(Config config)
        {
            File.WriteAllText(ConfigFileName, JsonConvert.SerializeObject(config));
        }
    }
}