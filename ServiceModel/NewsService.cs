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
                info += content + "\n";
            }
            return info;
        }

        //public static Dictionary<string, List<string>> AdjustDict()
        //{
        //    Dictionary<string, List<string>> GroupInfo = PremierLeagueData.GenerateGroupsNews();
        //    Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            
        //    foreach (string group in GroupInfo.Keys)
        //    {
        //        dict[GetGroup(group, groups).GroupName] = GroupInfo[group];
        //    }
        //    return dict;
        //}
    }
}
