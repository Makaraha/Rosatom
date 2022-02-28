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

            var repeater = new TryRepeater();
            using (var client = new WebClient())
                await repeater.RepeatForAsync(() => client.DownloadFile(new Uri(uri), Path.Combine(path, fileName)), 5);

            return document;
        }

        public static async Task<string> GetExtensionAsync(string uri)
        {
            using (var client = new HttpClient())
            {
                var repeater = new TryRepeater();
                var response = await await repeater.RepeatForAsync(async () => await client.GetAsync(uri), 5);
                var contentDisp = response.Content.Headers.GetValues("content-disposition");
                var extRegex = new Regex(@"\.[A-Za-z]*$");
                var extension = extRegex.Match(contentDisp.First()).Value;

                if (extension is null || extension == "")
                    throw new Exception($"Empty extnension. Conent-disposition - {contentDisp}");

                return extension[1..].ToLower();
            }
        }
    }
}
