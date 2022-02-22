using HtmlAgilityPack;

namespace Rosatom
{
    public class ProcurementReader
    {
        private HtmlDocument procurementHtml { get; set; }
        private long procurementId { get; set; }

        public ProcurementReader(HtmlDocument procurementHtml, long procurementId)
        {
            this.procurementHtml = procurementHtml;
            this.procurementId = procurementId;
        }

        public List<Document> GetDocuments()
        {
            var div = GetDocumentDiv();
            var docs = new List<Document>();

            for(int i = 1; true; i++)
            {
                var documentModel = new Document();

                documentModel.Name = FieldReader.GetTextFromXPath($"/html/body/form/div[4]/div[5]/div[2]/div[5]/div[{div}]/div[2]/table/tbody/tr[{i}]/td[1]/p/a[2]",
                    procurementHtml);

                if (documentModel.Name is null)
                    break;

                documentModel.Url = LinkReader.GetLinkFromXPath($"/html/body/form/div[4]/div[5]/div[2]/div[5]/div[{div}]/div[2]/table/tbody/tr[{i}]/td[1]/p/a[2]", 
                    procurementHtml);

                documentModel.ProcurementId = procurementId;

                docs.Add(documentModel);
            }
            return docs;
        }

        private int GetDocumentDiv()
        {
            for(int i = 1; i <= 10; i++)
            {
                var blockName = FieldReader.GetTextFromXPath($"/html/body/form/div[4]/div[5]/div[2]/div[5]/div[{i}]/div/a[2]",
                    procurementHtml);

                if (blockName == null)
                    break;

                if (blockName == "Документы")
                    return i;
            }
            throw new Exception("Не удалось найти div документов");
        }
    }
}
