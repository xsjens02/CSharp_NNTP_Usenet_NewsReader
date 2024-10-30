namespace UsenetProgram.Models
{
    public class Header
    {
        public string ArticleNumber { get; }
        public string Subject { get; }
        public Header(string articleNumber, string subject)
        {
            this.ArticleNumber = articleNumber;
            this.Subject = subject;
        }
    }
}
