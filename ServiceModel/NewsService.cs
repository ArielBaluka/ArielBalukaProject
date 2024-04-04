using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using ViewModel;

namespace ServiceModel
{
    public class NewsService : INewsService
    {
        public static Dictionary<string, List<string>> groupsInfo = new Dictionary<string, List<string>>();

        static NewsService()
        {
            groupsInfo = PremierLeagueData.AdjustDict();
        }

        public string GetGroupData(string groupName)
        {
            List<string> data = groupsInfo[groupName];
            string info = "";
            foreach (string content in data)
            {
                info += content + "\n\n";
            }
            return info;
        }
    }
}
