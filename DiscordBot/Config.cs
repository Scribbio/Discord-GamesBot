using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Config
    {
        private const string configFolder = "Resource";
        private const string configFile = "config.json";

        public static BotConfig bot;

        /* Why store the token in a seperate file and not hardcode a token in a string?
         * Storing the token in a file is better, as now you can redistribute the software 
         * with an easy way for someone to use their own token.
         * */
        static Config()
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder);

            if(!File.Exists(configFolder + "/" + configFile))
            {
                bot = new BotConfig();
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText(configFolder + "/" + configFile, json);
            }
            else
            {
                string json = File.ReadAllText(configFolder + "/" + configFile);
                bot = JsonConvert.DeserializeObject<BotConfig>(json);

            }

        }

        //These strings, though not instantiated here,
        //are written into the json file created above.
        public struct BotConfig
        {
            public string token;
            public string cmdPrefix;
        }

    }
}
