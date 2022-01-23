using System;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;

namespace Wolf3D.ReadyPlayerMe.AvatarSDK
{
    public class AvatarUri
    {
        private readonly string[] Extensions = { ".glb", ".gltf" };

        private const string ShortCodeBaseUrl = "https://readyplayer.me/api/avatar/";

        public string Extension { get; private set; }
        public string ModelName { get; private set; }
        public string ModelPath { get; private set; }
        public string AbsoluteUrl { get; private set;}
        public string AbsolutePath {get; private set; }
        public string AbsoluteName { get; private set; }
        public string MetaDataUrl { get; private set; }

        public async Task<AvatarUri> Create(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                url = await GetUrlFromShortCode(url);
            }

            return CreateFromURL(url);
        }

        private AvatarUri CreateFromURL(string url)
        {
            Uri uri = new Uri(url);

            AbsoluteUrl = uri.AbsoluteUri;
            AbsolutePath = uri.AbsolutePath;
            AbsoluteName = Path.GetFileNameWithoutExtension(AbsolutePath);

            Extension = Path.GetExtension(AbsolutePath);
            if (!Extensions.Contains(Extension))
            {
                throw new Exception($"Exceptions.UnsupportedExtensionException: Unsupported model extension { Extension }. Only .gltf and .glb formats are supported.");
            }

            ModelName = AbsolutePath.Split('/').Last();
            ModelPath = $"{ Application.dataPath }/Resources/Avatars/{ ModelName }";

            MetaDataUrl = AbsoluteUrl.Replace(".glb", ".json");

            return this;
        }

        private async Task<string> GetUrlFromShortCode(string shortcode)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ShortCodeBaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();

                response = await client.GetAsync(shortcode);
            }

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Exceptions.ShortCodeNotFound: Avatar at given short code { shortcode } is not found. Please make sure you entered a valid short code. HttpStatusCode: { ((int)response.StatusCode)} - { response.StatusCode }");
            }

            return response.RequestMessage.RequestUri.AbsoluteUri;
        }
    }
}
