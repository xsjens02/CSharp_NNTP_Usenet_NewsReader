using System.Collections.ObjectModel;
using System.Diagnostics;
using UsenetProgram.Models;
using UsenetProgram.Services;

namespace UsenetProgram.Applications
{
    public abstract class GetGroups
    {
        public static async Task<List<Group>> ActionAsync()
        {
            List<Group> groupList = new();

            await NetworkManager.Instance.WriteToStreamAsync("LIST");
            string response = await NetworkManager.Instance.ReadFromStreamAsync(true);

            string[] lines = response.Split('\n');

            for (int i = 1; i < lines.Length; i++)
            {
                var line = lines[i];

                string[] parts = line.Split(' ');

                if (parts.Length == 4)
                {
                    string name = parts[0];
                    string finalChar = parts[3];
                    bool postable = finalChar.StartsWith("y");

                    groupList.Add(new Group(name, postable));
                }
            }
            return groupList;
        }
    }
}
