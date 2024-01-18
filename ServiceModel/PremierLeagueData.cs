using Model;
using System;
using System.Runtime.Remoting.Messaging;
using System.Xml;
using ViewModel;

namespace ServiceModel
{
    public class PremierLeagueData
    {
        public static GameList ParseRSSEPL()
        {
            string xmlPath = @"https://hup2.com/pl/rss/rss_prem.php?leg=1&season=202324";
            GameList list = new GameList();
            GroupDB groupDB = new GroupDB();
            GroupList groups = groupDB.SelectAll();
            try
            {
                XmlTextReader reader = new XmlTextReader(xmlPath);
                reader.WhitespaceHandling = WhitespaceHandling.None;

                Game game = null;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "item":
                                game = new Game(); //יצירת עצם חדש
                                break;
                            case "date":
                                game.Date = DateTime.Parse(reader.ReadString());
                                break;
                            case "hteam":
                                game.HOMETEAM = GetGroup(reader.ReadString(),groups);
                                break;
                            case "hscore":
                                game.HOMESCORE = int.Parse(reader.ReadString());
                                break;
                            case "ateam":
                                game.AWAYTEAM = GetGroup(reader.ReadString(), groups);
                                break;
                            case "ascore":
                                game.AWAYSCORE = int.Parse(reader.ReadString());
                                break;
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == "item") //סיום הגדרות של משחק
                            list.Add(game); //הוספת המשחק לרשימה                      
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            foreach (Game game1 in list)
                Console.WriteLine(game1.ToString());

            return list;
        }

        public static Group GetGroup(string team,GroupList groups)
        {
            team = team == "Man United" ? "Manchester United" : team;
            team = team == "Man City" ? "Manchester City" : team;
            team = team == "West Ham" ? "West Ham United" : team;
            team = team == "Newcastle" ? "Newcastle United" : team;
            team = team == "Tottenham" ? "Tottenham Hotspur" : team;
            team = team == "Wolves" ? "Wolverhampton Wanderers" : team;
            Group group= groups.Find(g => g.GroupName == team);
            if (group == null) 
                group = null;
            return group;
        }
    }
}
