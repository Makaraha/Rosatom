using HtmlAgilityPack;
using System.Net;
using System.Text;

namespace Rosatom
{
    public static class FieldReader
    {
        public static string GetTextFromXPath(string xPath, HtmlDocument doc)
        {
            var node = doc.DocumentNode.SelectSingleNode(xPath);
            return BringToStandart(node?.InnerText);
        }

        internal static string BringToStandart(string str)
        {
            if (str is null)
                return null;

            str = WebUtility.HtmlDecode(str);
            var sb = new StringBuilder();

            str = str.Replace("\n", "").Replace("\t", " ").Replace("\r", " ");
            str = str.Replace("\t", " ");


            for (int i = 0; i < str.Length; i++)
            {
                if ((sb.Length == 0 && str[i] == ' ')
                    || (i < str.Length - 1 && str[i] == ' ' && str[i + 1] == ' ')
                    || (i == str.Length - 1 && str[i] == ' ')
                    || str[i] == 160)
                    continue;
                else
                    sb.Append(str[i]);
            }

            var result = sb.ToString();

            if (result.Length == 0)
                return null;

            return result;
        }
    }
}
