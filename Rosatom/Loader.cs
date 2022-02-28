using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosatom
{
    public static class Loader
    {
        public static async Task<PageReader> LoadPageAsync(int pageNumber)
        {
            var url = $"https://zakupki.rosatom.ru/Web.aspx?node=currentorders&page={pageNumber}";
            var pageHtml = await LoadHtmlAsync(url);
            return new PageReader(pageHtml);
        }

        public static async Task<ProcurementReader> LoadProcurementAsync(string url)
        {
            var procurementHtml = await LoadHtmlAsync(url);
            var id = LinkReader.GetGuidFromUrl(url);
            return new ProcurementReader(procurementHtml, id);
        }

        public static async Task<HtmlDocument> LoadHtmlAsync(string url)
        {
            var web = new HtmlWeb();
            var repeater = new TryRepeater();

            return await repeater.RepeatUntilSuccessAsync(() => web.Load(url));
        }
    }
}
