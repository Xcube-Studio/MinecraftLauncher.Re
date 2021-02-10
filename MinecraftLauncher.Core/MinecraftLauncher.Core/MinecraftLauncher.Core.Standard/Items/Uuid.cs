using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace MinecraftLauncher.Core.Standard.Items
{
    public class Uuid
    {
        public string Content { get; set; }

        public Guid Guid { get { return Guid.Parse(this.Content); } }

        public static Uuid Parse(string text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.Default.GetBytes(text);
                return Uuid.Parse(new Guid(md5.ComputeHash(bytes)));
            }
        }

        public static Uuid Parse(Guid guid) => 
            new Uuid()
            {
                Content = guid.ToString("N")
            };

        public static implicit operator string(Uuid uuid) => uuid.Content;

        public static implicit operator Uuid(string value) => new Uuid() { Content = value };

    }
}
