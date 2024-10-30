using System.Collections.ObjectModel;
using UsenetProgram.Services;

namespace UsenetProgram.Applications
{
    public abstract class GetArticleContent
    {
        public static async Task<List<String>> ActionAsync(string articleNumber)
        {
            List<string> articleBodyContent = new();

            await NetworkManager.Instance.WriteToStreamAsync($"ARTICLE {articleNumber}");
            string response = await NetworkManager.Instance.ReadFromStreamAsync(true);

            int contentIndex = response.IndexOf("\r\n\r\n");

            string articleContent = response.Substring(contentIndex);
            string[] lines = articleContent.Split("\n");

            for (int i = 2; i < lines.Length - 2; i++)
                articleBodyContent.Add(lines[i]);

            return articleBodyContent;
        }
    }
}
