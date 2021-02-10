using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MinecraftLauncher.Core.Standard.Service.Network
{
    public class Http
    {
        public static HttpClient HttpClient = new HttpClient();

        public static Task<string> PostAsync(string url, string content, string contentType)
        {
            return Task.Run(() =>
            {
                HttpContent httpContent = new StringContent(content);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                using (HttpResponseMessage httpResponseMessage = HttpClient.PostAsync(url, httpContent).GetAwaiter().GetResult())
                    return httpResponseMessage.Content.ReadAsStringAsync();
            });
        }

        public static Task<string> GetAsync(string url, string[] authorization = null)
        {
            return Task.Run(() =>
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(url);
                    httpRequestMessage.Headers.Authorization = authorization != null ? new AuthenticationHeaderValue(authorization[0], authorization[1]) : null;
                    httpRequestMessage.Method = new HttpMethod("GET");

                    using (HttpResponseMessage httpResponseMessage = HttpClient.SendAsync(httpRequestMessage).GetAwaiter().GetResult())
                        return httpResponseMessage.Content.ReadAsStringAsync();
                }
            });
        }

        public static Task<Stream> GetStreamAsync(string url)
        {
            return Task.Run(() =>
            {
                using (HttpRequestMessage httpRequestMessage = new HttpRequestMessage())
                {
                    httpRequestMessage.RequestUri = new Uri(url);
                    httpRequestMessage.Method = new HttpMethod("GET");

                    using (HttpResponseMessage httpResponseMessage = HttpClient.SendAsync(httpRequestMessage).GetAwaiter().GetResult())
                        return httpResponseMessage.Content.ReadAsStreamAsync();
                }
            });
        }
    }
}
