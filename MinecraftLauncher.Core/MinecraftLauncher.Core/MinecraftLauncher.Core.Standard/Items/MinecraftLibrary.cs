using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using MinecraftLauncher.Core.Standard.Service.Local;

namespace MinecraftLauncher.Core.Standard.Items
{
    public class MinecraftLibrary
    {
        public string Path { get; set; }

        public string Name { get; set; }

        public string Sha1 { get; set; }

        public bool Enable { get; set; }

        public bool Natives { get; set; }

        public static List<MinecraftLibrary> GetMinecraftLibraries(MinecraftEntity minecraftEntity, bool OnlyEnable = false)
        {
            var libraries = new List<MinecraftLibrary>();

            foreach(JObject jObject in minecraftEntity.Libraries)
            {
                var ml = new MinecraftLibrary();

                ml.Name = (string)jObject["name"];
                ml.Enable = JudgeEnable(jObject);

                if (OnlyEnable == true && ml.Enable == false)
                    continue;

                ml.Natives = jObject.ContainsKey("natives");

                string[] info = GetDownloadInfo(jObject);
                ml.Path = info[0];
                ml.Sha1 = info[1];

                libraries.Add(ml);
            }

            return libraries;
        }

        public static bool JudgeEnable(JObject libObject)
        {
            if (!libObject.ContainsKey("rules"))
                return true;

            var rules = (JArray)libObject["rules"];

            if (rules.Count > 1)
                if ((string)((JObject)rules[1])["os"]["name"] == "windows")
                    return false;
                else return true;
            else
            {
                if ((string)((JObject)rules[0])["os"]["name"] != "windows")
                    return false;
                else return true;
            }
            
        }

        public static string[] GetDownloadInfo(JObject libObject)
        {
            string path, sha1 = null;

            string[] name = libObject["name"].ToString().Split(':');
            bool natives = libObject.ContainsKey("natives");

            if (natives)
            {
                try
                {
                    string native = libObject["natives"]["windows"].ToString().Replace("${arch}", SystemConfiguration.Arch);

                    JObject jObject = (JObject)libObject["downloads"]["classifiers"][native];
                    path = (string)jObject["path"];
                    sha1 = (string)jObject["sha1"];
                }
                catch { path = ""; sha1 = ""; }
            }
            else try
                {
                    JObject jObject = (JObject)libObject["downloads"]["artifact"];
                    path = (string)jObject["path"];
                    sha1 = (string)jObject["sha1"];
                }
                catch {  path = $"{name[0].Replace(".", "/")}/{name[1]}/{name[2]}/{name[1]}-{name[2]}.jar"; }

            return new string[] { path, sha1 };
        }
    }
}
