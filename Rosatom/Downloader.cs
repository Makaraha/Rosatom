using System.Net;
using System.Text.RegularExpressions;

namespace Rosatom
{
    public static class Downloader
    {
        public static async Task<Document> DonwloadDocumentAsync(string uri, string extension, string path)
        {
            var documentId = Guid.NewGuid();
            var fileName = $"{documentId}.{extension}";
            var document = new Document();

            document.Name = documentId.ToString();
            document.Extension = extension;
            document.Url = uri;

            using (var client = new WebClient())
                await client.DownloadFileTaskAsync(new Uri(uri), Path.Combine(path, fileName));

            return document;
        }

        public static async Task<string> GetExtensionAsync(string uri)
        {
            using (var client = new HttpClient())
            {
                var message = new HttpRequestMessage();
                message.RequestUri = new Uri(uri);
                var response = await client.SendAsync(message);
                var contentDisp = response.Content.Headers.GetValues("content-disposition");
                var extRegex = new Regex(@"\.[A-Za-z]*");
                var extension = extRegex.Match(contentDisp.First()).Value;

                if (extension is null)
                    throw new Exception($"Empty extnension. Conent-disposition - {contentDisp}");

                return extension[1..].ToLower();
            }
        }
    }
}
