using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncher.Core.Standard.Items;
using MinecraftLauncher.Core.Standard.Enums;

namespace MinecraftLauncher.Core.Standard.Service.Local
{
    public class MinecraftDataLoader : IDisposable
    {
        public MinecraftLocator MinecraftLocator { get; private set; }

        public AuthenticationData AuthenticationData { get; set; }

        public LaunchConfiguration LaunchConfiguration { get; set; }

        public string Name { get; set; }

        public string JavaPath { get; set; }

        public MinecraftDataLoader(MinecraftLocator minecraftLocator, AuthenticationData authenticationData, string name, LaunchConfiguration launchConfiguration = null)
        {
            this.MinecraftLocator = minecraftLocator;
            this.AuthenticationData = authenticationData;
            this.LaunchConfiguration = launchConfiguration;
            this.Name = name;
        }

        public MinecraftData Load()
        {
            MinecraftEntity minecraftEntity = this.MinecraftLocator.GetMinecraftEntity(Name);

            var data = new MinecraftData()
            {
                MinecraftEntity = minecraftEntity,
                AuthenticationData = this.AuthenticationData,
                MinecraftLibraries = MinecraftLibrary.GetMinecraftLibraries(minecraftEntity, true),
                EntityType = GetEntityType(minecraftEntity)
            };

            if (this.LaunchConfiguration == null)
                this.LaunchConfiguration = new LaunchConfiguration()
                {
                    GameFolder = this.MinecraftLocator.Location,
                    JavaPath = this.JavaPath,
                    MaxMemory = 1024,
                    MinMemory = 1024,
                    FrontArgs = "",
                    Version = this.Name
                };

            if (this.LaunchConfiguration.NativesFolder == null)
                this.LaunchConfiguration.NativesFolder = $"{this.MinecraftLocator.Location}\\versions\\{Name}\\{Name}-natives";

            if (data.EntityType == MinecraftEntityType.Modify)
                data.MinecraftInheritEntity = this.MinecraftLocator.GetMinecraftEntity(data.MinecraftEntity.InheritsFrom);

            if (data.EntityType == MinecraftEntityType.Modify)
                MinecraftLibrary.GetMinecraftLibraries(this.MinecraftLocator.GetMinecraftEntity(data.MinecraftEntity.InheritsFrom), true).ForEach(x => { data.MinecraftLibraries.Add(x); });

            data.MinecraftMainLibraryPath = data.EntityType == MinecraftEntityType.Modify
                ? $"{LaunchConfiguration.GameFolder}\\versions\\{data.MinecraftEntity.InheritsFrom}\\{data.MinecraftEntity.InheritsFrom}.jar"
                : $"{LaunchConfiguration.GameFolder}\\versions\\{Name}\\{Name}.jar";

            data.LaunchConfiguration = this.LaunchConfiguration;

            return data;
        }

        public static MinecraftEntityType GetEntityType(MinecraftEntity minecraftEntity)
        {
            if (minecraftEntity.InheritsFrom != null)
                return MinecraftEntityType.Modify;
            if (minecraftEntity.MainClass == "net.minecraft.client.main.Main")
                return MinecraftEntityType.Vanilla;

            return MinecraftEntityType.Unknown;
        }

        public void Dispose()
        {
            this.MinecraftLocator = null;
            this.LaunchConfiguration = null;
            this.AuthenticationData = null;
            this.JavaPath = null;
            this.Name = null;
        }
    }
}
