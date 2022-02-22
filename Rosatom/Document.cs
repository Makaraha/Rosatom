namespace Rosatom
{
    public class Document : IEquatable<Document>
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Url { get; set; }
        public long ProcurementId { get; set; }

        public bool Equals(Document other)
        {
            return Url == other.Url;
        }
    }
}
