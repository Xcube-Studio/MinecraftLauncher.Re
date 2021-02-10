using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace MinecraftLauncher.Core.Authenticator.UI
{
    public partial class WebBrowser : Form
    {
        public string Code { get; set; }

        public WebBrowser()
        {
            InitializeComponent();
            webBrowser1.Navigate(new Uri("https://login.live.com/oauth20_authorize.srf?client_id=00000000402b5328&response_type=code&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf"));
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Console.WriteLine(e.Url);
            if (e.Url.AbsoluteUri.Contains("https://login.live.com/oauth20_desktop.srf?code="))
            {
                Code = e.Url.AbsoluteUri.Split('=')[1].Split('&')[0];
                Console.WriteLine(Code);
                this.Close();
            }
        }
        
        public static Task<string> GetCodeAsync()
        {
            WebBrowser webBrowser = null;

            Thread thread = new Thread(() =>
            {
                webBrowser = new WebBrowser();
                webBrowser.ShowDialog();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            return Task.Run(() =>
            {
                thread.Join();
                return webBrowser.Code;
            });
        }
    }
}
