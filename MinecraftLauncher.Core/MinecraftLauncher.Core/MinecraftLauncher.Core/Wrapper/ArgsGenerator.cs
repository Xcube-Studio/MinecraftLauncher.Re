using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinecraftLauncher.Core.Standard;
using MinecraftLauncher.Core.Standard.Items;
using MinecraftLauncher.Core.Standard.Service.Local;
using Newtonsoft.Json.Linq;

namespace MinecraftLauncher.Core.Wrapper
{
    public class ArgsGenerator : IArgsGenerator
    {
        public ArgsGenerator(MinecraftData minecraftData) => MinecraftData = minecraftData;

        public MinecraftData MinecraftData { get; set; }

        public string BulidArguments() => $"{GetFrontArguments()} {GetBehindArguments()}";

        public string GetFrontArguments()
        {
            var arguments = new ArgumentBuilder();

            arguments.Append(MinecraftData.LaunchConfiguration.FrontArgs, false);
            arguments.Append("-XX:HeapDumpPath=MojangTricksIntelDriversForPerformance_javaw.exe_minecraft.exe.heapdump");
            arguments.Append($"-Djava.library.path={MinecraftData.LaunchConfiguration.NativesFolder}");
            arguments.Append($"-Xmx{MinecraftData.LaunchConfiguration.MaxMemory}M");
            arguments.Append(MinecraftData.LaunchConfiguration.MinMemory != null ? $"-Xmn{MinecraftData.LaunchConfiguration.MinMemory}M" : string.Empty);
            arguments.Append($"-classpath");
            arguments.Append(GetClasspath());

            return arguments.ToString();
        }

        public string GetBehindArguments()
        {
            var arguments = new StringBuilder();
            arguments.Append($"{MinecraftData.MinecraftEntity.MainClass} ");

            string args = MinecraftData.MinecraftEntity.MinecraftArguments != null ?
                MinecraftData.MinecraftEntity.MinecraftArguments : string.Empty;

            if (MinecraftData.EntityType == Standard.Enums.MinecraftEntityType.Modify && MinecraftData.MinecraftInheritEntity.Arguments != null)
                ((JArray)MinecraftData.MinecraftInheritEntity.Arguments["game"]).ToList().ForEach(x => { try { args += $" {(string)x}"; } catch { } });
            if (MinecraftData.MinecraftEntity.Arguments != null)
                ((JArray)MinecraftData.MinecraftEntity.Arguments["game"]).ToList().ForEach(x => { try { args += $" {(string)x}"; } catch { } });

            string assets_index_name = MinecraftData.EntityType == Standard.Enums.MinecraftEntityType.Modify
                ? (string)MinecraftData.MinecraftInheritEntity.AssetIndex["id"] : (string)MinecraftData.MinecraftEntity.AssetIndex["id"];

            arguments.Append(args.Replace("${auth_player_name}", MinecraftData.AuthenticationData.UserName)
                .Replace("${version_name}", MinecraftData.MinecraftEntity.Id)
                .Replace("${game_directory}", $"\"{MinecraftData.LaunchConfiguration.GameFolder}\"")
                .Replace("${assets_root}", $"\"{MinecraftData.LaunchConfiguration.GameFolder}\\assets\"")
                .Replace("${assets_index_name}", assets_index_name)
                .Replace("${auth_uuid}", MinecraftData.AuthenticationData.Uuid)
                .Replace("${auth_access_token}", MinecraftData.AuthenticationData.AccessToken)
                .Replace("${user_type}", MinecraftData.AuthenticationData.UserType.ToString())
                .Replace("${version_type}", MinecraftData.MinecraftEntity.Type)
                .Replace("${user_properties}", "{}"));

            return arguments.ToString();
        }

        public string GetClasspath()
        {
            var arguments = new StringBuilder();

            foreach (var library in MinecraftData.MinecraftLibraries)
                if (library.Natives != true && library.Enable == true)
                    arguments.Append($"{MinecraftData.LaunchConfiguration.GameFolder}\\libraries\\{library.Path.Replace('/', '\\')};");

            arguments.Append($"{MinecraftData.MinecraftMainLibraryPath}");

            return arguments.ToString();
        }

        public class ArgumentBuilder
        {
            public StringBuilder StringBuilder = new StringBuilder();

            public void Append(string value,bool dontUse = true)
            {
                if (value == null || value == string.Empty)
                    return;

                if (value.Contains(" ") && dontUse)
                    value = $"\"{value}\"";
                value = $" {value}";

                StringBuilder.Append(value);
            }

            public new string ToString() => StringBuilder.ToString();
        }
    }
}
