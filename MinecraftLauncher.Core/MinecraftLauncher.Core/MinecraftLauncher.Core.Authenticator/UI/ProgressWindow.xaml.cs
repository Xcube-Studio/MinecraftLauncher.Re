using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;
using MinecraftLauncher.Core.Standard.Service.Network;

namespace MinecraftLauncher.Core.Authenticator.UI
{
    /// <summary>
    /// ProgressWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressWindow : Window
    {

        public ProgressWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string code = await WebBrowser.GetCodeAsync();

            this.info.Text = "Getting authorization token...";

            string oauthjson = await Http.PostAsync(
                    url: "https://login.live.com/oauth20_token.srf",
                    content: $"client_id=00000000402b5328&code={code}&grant_type=authorization_code&redirect_uri=https://login.live.com/oauth20_desktop.srf&scope=service::user.auth.xboxlive.com::MBI_SSL",
                    contentType: "application/x-www-form-urlencoded");

            string access_token = JObject.Parse(oauthjson)["access_token"].ToString();
            Console.WriteLine(access_token);

            this.info.Text = "Authenticating with XBL...";

            string xboxlive = await Http.PostAsync(
                url: "https://user.auth.xboxlive.com/user/authenticate",
                content: "{\"Properties\":{\"AuthMethod\":\"RPS\",\"SiteName\":\"user.auth.xboxlive.com\",\"RpsTicket\":\"d=<access_token>\"},\"RelyingParty\":\"http://auth.xboxlive.com\",\"TokenType\":\"JWT\"}".Replace("<access_token>", access_token),
                contentType: "application/json");
            Console.WriteLine(xboxlive);

            this.info.Text = "Authenticating with XSTS...";

            string Token = JObject.Parse(xboxlive)["Token"].ToString();
            Console.WriteLine(Token);
            string xboxliveToken = await Http.PostAsync(
                url: "https://xsts.auth.xboxlive.com/xsts/authorize",
                content: "{\"Properties\":{\"SandboxId\":\"RETAIL\",\"UserTokens\":[\"xbl_token\"]},\"RelyingParty\":\"rp://api.minecraftservices.com/\",\"TokenType\":\"JWT\"}".Replace("xbl_token", Token),
                contentType: "application/json");

            this.info.Text = "Authenticating with minecraft...";

            JObject xsts = JObject.Parse(xboxliveToken);
            string uhs = xsts["DisplayClaims"]["xui"][0]["uhs"].ToString();
            string xsts_token = xsts["Token"].ToString();
            string minecraft = await Http.PostAsync(
                url: "https://api.minecraftservices.com/authentication/login_with_xbox",
                content: "{\"identityToken\":\"XBL3.0 x=<uhs>;<xsts_token>\"}".Replace("<uhs>", uhs).Replace("<xsts_token>", xsts_token),
                contentType: "application/json");

            this.info.Text = "Getting the minecraft profile...";

            access_token = JObject.Parse(minecraft)["access_token"].ToString();
            string profile = await Http.GetAsync(
                url: "https://api.minecraftservices.com/minecraft/profile",
                authorization: new string[] { "Bearer", access_token });
            Console.WriteLine(profile);

            this.info.Text = "Done..";
            this.progress.Value = 100;
            this.progress.IsIndeterminate = false;
            button.Content = "Done";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public static Task Load()
        {
            Thread thread = new Thread(() =>
            {
                new ProgressWindow().ShowDialog();
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            return Task.Run(() =>
            {
                thread.Join();
            });
        }
    }
}
