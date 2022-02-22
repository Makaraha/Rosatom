using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Rosatom
{
    public class PageReader
    {

        private HtmlDocument pageHtml { get; set; }

        public PageReader(HtmlDocument pageHtml)
        {
            this.pageHtml = pageHtml;
        }

        /// <summary>
        /// Считывает со страницы ссылки на закупки
        /// </summary>
        public List<Procurement.Short> GetProcurementLinks()
        {
            var result = new List<Procurement.Short>();
            int i = 1;

            while (true)
            {
                var procurement = new Procurement.Short();
                // Для перехода к следующей закупки нужно увеличить i на (1 + кол-во лотов)
                var link = LinkReader.GetLinkFromXPath($"/html/body/form/div[4]/div[5]/div[2]/div[3]/table/tbody/tr[{i}]/td[3]/p[2]/a",
                    pageHtml);
                var completionTime = FieldReader.GetTextFromXPath($"/html/body/form/div[4]/div[5]/div[2]/div[3]/table/tbody/tr[{i}]/td[7]/p", 
                    pageHtml);
                var status = FieldReader.GetTextFromXPath($"/html/body/form/div[4]/div[5]/div[2]/div[3]/table/tbody/tr[{i}]/td[8]/p",
                    pageHtml);

                if (link is null)
                    break;

                var lotCount = pageHtml.DocumentNode
                    .SelectSingleNode($"/html/body/form/div[4]/div[5]/div[2]/div[3]/table/tbody/tr[{i}]/td[4]/p[2]")
                    .ChildNodes[1].InnerText;

                procurement.Url = link;
                (procurement.AcceptingApplicationsDeadline, procurement.SummarizingDate)
                    = GetProcurementDates(completionTime);

                i += 1 + int.Parse(lotCount);

                if (status != "Черновик")
                    result.Add(procurement);
            }

            if (result.Count == 0)
                throw new Exception("Ссылок на закупки не найдено");

            return result;
        }

        private (DateTime?, DateTime?) GetProcurementDates(string s)
        {
            if (s is null)
                return (null, null);


            // Вытаскивает даты окончания приёма документов и подведения итогов
            var dataRegex = new Regex(@"\d\d\.\d\d\.\d\d\d\d");
            var dates = dataRegex.Matches(s);

            DateTime? date1 = null;
            DateTime? date2 = null;

            if (dates.Count >= 1)
                date1 = DateTime.ParseExact(dates[0].ToString(), "dd.MM.yyyy", null);

            if (dates.Count >= 2)
                date2 = DateTime.ParseExact(dates[1].ToString(), "dd.MM.yyyy", null);

            return (date1, date2);
        }

        public int GetPagesAmount()
        {
            var itemsString = FieldReader.GetTextFromXPath("/html/body/form/div[4]/div[5]/div[2]/div[2]/div[3]",
                pageHtml);

            itemsString = itemsString.Substring(13);

            int itemsCount = int.Parse(itemsString);

            return itemsCount % 30 == 0 ? itemsCount / 30 : itemsCount / 30 + 1;
        }
    }
}
