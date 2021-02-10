using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MinecraftLauncher.Core.Standard.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MinecraftLauncher.Core.Standard.Service.Local
{
    public class MinecraftLocator
    {
        public string Location { get; private set; }

        public MinecraftLocator(string location)
        {
            if (location != null)
                this.Location = location;
            else throw new ArgumentNullException("location is null");
        }

        public List<string> GetVersions()
        {
            var list = new List<string>();

            Directory.GetDirectories($"{Location}\\versions").ToList().ForEach(x => 
            {
                string FolderName = new DirectoryInfo(x).Name;
                string FileName = $"{x}\\{FolderName}.json";

                if (File.Exists(FileName))
                    try
                    {
                        var Entity = JsonConvert.DeserializeObject<MinecraftEntity>(File.ReadAllText(FileName));

                        if (Entity.Id == FolderName)
                            list.Add(x);
                    }
                    catch { }
            });

            return list;
        }

        public MinecraftEntity GetMinecraftEntity(string id)
        {
            foreach (string path in this.GetVersions())
                if (path.EndsWith(id))
                {
                    string FolderName = new DirectoryInfo(path).Name;
                    string FileName = $"{path}\\{FolderName}.json";

                    return JsonConvert.DeserializeObject<MinecraftEntity>(File.ReadAllText(FileName));
                }

            return null;
        }
    }
}
