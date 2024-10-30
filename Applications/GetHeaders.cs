using System.Collections.ObjectModel;
using UsenetProgram.Models;
using UsenetProgram.Services;

namespace UsenetProgram.Applications
{
    public abstract class GetHeaders
    {
        public static async Task<List<Header>> ActionAsync(string groupName)
        {
            List<Header> headerList = new();

            await NetworkManager.Instance.WriteToStreamAsync($"GROUP {groupName}");
            string groupResponse = await NetworkManager.Instance.ReadFromStreamAsync(false);

            string[] groupData = groupResponse.Split(' ');

            if (groupData.Length == 5)
            {
                string first = groupData[2];
                string last = groupData[3];

                await NetworkManager.Instance.WriteToStreamAsync($"XOVER {first}-{last}");
                string xoverResponse = await NetworkManager.Instance.ReadFromStreamAsync(true);

                string[] headers = xoverResponse.Split("\n");
                for (int i = 1; i < headers.Length; i++)
                {
                    string[] headerData = headers[i].Split("\t");

                    if (headerData.Length >= 2)
                    {
                        string articleNumber = headerData[0];
                        string subject = headerData[1];

                        headerList.Add(new Header(articleNumber, subject));
                    }
                }
            }

            return headerList;
        }
    }
}
