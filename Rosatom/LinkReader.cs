using HtmlAgilityPack;

namespace Rosatom
{
    internal static class LinkReader
    {
        private static string linkBase { get; set; } = "https://zakupki.rosatom.ru/";

        internal static string GetLinkFromXPath(string xPath, HtmlDocument doc)
        {
            var node = doc.DocumentNode
                .SelectSingleNode(xPath);

            if (node is null)
                return null;

            return linkBase + node.Attributes["href"].Value;
        }

        public static long GetGuidFromUrl(string url)
        {
            return long.Parse(url.Substring(linkBase.Length + 1));
        }
    }
}