using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using MinecraftLauncher.Core.Standard.Items;

namespace MinecraftLauncher.Core.Standard.Service.Local
{
    public class NativesManager
    {
        public List<MinecraftLibrary> MinecraftLibraries { get; private set; }

        public string GameFolder { get; set; }

        public string NativesFolder { get; set; }

        public NativesManager(MinecraftData minecraftData)
        {
            this.MinecraftLibraries = minecraftData.MinecraftLibraries;
            this.GameFolder = minecraftData.LaunchConfiguration.GameFolder;
            this.NativesFolder = minecraftData.LaunchConfiguration.NativesFolder;
        }

        public void Extract()
        {
            var libs = GetNatives(MinecraftLibraries);

            foreach (var lib in libs) 
            {
                string path = $"{GameFolder}\\libraries\\{lib.Path.Replace("/","\\")}";

                using (ZipArchive zipArchive = ZipFile.OpenRead(path))
                    foreach (ZipArchiveEntry entry in zipArchive.Entries)
                        if (entry.FullName.EndsWith(".dll"))
                        {
                            if (!Directory.Exists(NativesFolder))
                                Directory.CreateDirectory(NativesFolder);

                            entry.ExtractToFile($"{NativesFolder}\\{entry.Name}", true);
                        }
            }

        }

        public static List<MinecraftLibrary> GetNatives(List<MinecraftLibrary> list)
        {
            var libraries = new List<MinecraftLibrary>();

            foreach (var minecraftLibrary in list)
                if (minecraftLibrary.Natives)
                    libraries.Add(minecraftLibrary);

            return libraries;
        }
    }
}
