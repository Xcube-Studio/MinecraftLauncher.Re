using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using MinecraftLauncher.Core.Standard.Service.Network;
using MinecraftLauncher.Core.Standard.Enums;

namespace MinecraftLauncher.Core.Standard.Items
{
    public class SkinData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("state")]
        public SkinState SkinState { get; set; }

        [JsonProperty("variant")]
        public SkinVariant SkinVariant { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; } // 昵称

        public Task SaveAsync(bool withDialog = true,string path = null)
        {
            return Task.Run(async() =>
            {
                Thread thread = new Thread(() =>
                {
                    if (withDialog)
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.FileName = "skin.png";
                        saveFileDialog.Title = "Save Skin";
                        saveFileDialog.AutoUpgradeEnabled = true;

                        saveFileDialog.ShowDialog();
                        saveFileDialog.FileName = path;
                    }
                    else
                    {
                        if (path == null)
                            throw new Exception("value'path' can't be null");
                    }
                });
                thread.SetApartmentState(ApartmentState.STA);

                thread.Start();
                thread.Join();

                using (FileStream fileStream = new FileStream(path, FileMode.Create)) 
                {
                    byte[] bytes = new byte[1024];

                    using (Stream stream = await Http.GetStreamAsync(this.Url))
                    {
                        bytes = new byte[stream.Length];
                        stream.Read(bytes, 0, bytes.Length);
                    }

                    fileStream.Write(bytes, 0, bytes.Length);
                    fileStream.Flush();
                    fileStream.Close();
                }
            });
        }
    }
}
