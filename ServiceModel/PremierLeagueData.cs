using Model;
using System;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Xml;
using ViewModel;
using Ical.Net;
using Ical.Net.Collections;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Calendar = Ical.Net.Calendar;
using CalendarEvent = Ical.Net.CalendarComponents.CalendarEvent;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Group = Model.Group;

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

        public static Game GenerateGame(CalendarEvent calendarEvent, GroupList groups)
        {
            Game game = new Game();
            game.Date = DateTime.Parse(calendarEvent.Start.AsSystemLocal.ToString("MM/dd/yyyy"));
            game.HOMETEAM = GetGroup(calendarEvent.Summary.Split(new string[] { " v " }, StringSplitOptions.None)[0].Remove(0, 3), groups);
            game.AWAYTEAM = GetGroup(calendarEvent.Summary.Split(new string[] { " v " }, StringSplitOptions.None)[1], groups);
            game.HOMESCORE = game.AWAYSCORE = -1;
            return game;
        }
        public static GameList ReadGameSchedule()
        {
            GroupDB groupDB = new GroupDB();
            GroupList groups = groupDB.SelectAll();
            GameList games = new GameList();
            // Url i get the scheduel from
            string icalUrl = "https://ics.ecal.com/ecal-sub/65ae6c0399a57d00085dfbd2/English%20Premier%20League.ics";

            // Download the .ics file
            string icalContent = DownloadIcalContent(icalUrl);

            if (icalContent != null)
            {
                //loading the information from the downloaded info
                Calendar calendar = Calendar.Load(icalContent);
                // ordering the event by their starting time
                var sortedEvents = calendar.Events.OrderBy(e => e.Start);
                // Get the current time
                DateTime currentLocalTime = DateTime.UtcNow;
                //DateTime nextMonth = currentLocalTime.AddMonths(1);
                DateTime nextTwoWeeks = currentLocalTime.AddDays(14);
                foreach (var calendarEvent in sortedEvents)
                {
                    if (currentLocalTime < calendarEvent.Start.AsSystemLocal && nextTwoWeeks > calendarEvent.Start.AsSystemLocal)
                    {// adding only relevant games - in the future and in less than a month
                        games.Add(GenerateGame(calendarEvent, groups));
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve calendar data.");
            }
            return games;
        }

        static string DownloadIcalContent(string url)
        {
            //creating a http request to get information about the game scheduel
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        return response.Content.ReadAsStringAsync().Result;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to retrieve calendar data. Status code: {response.StatusCode}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return null;
                }
            }
        }

        public static Group GetGroup(string team,GroupList groups)
        {
            team = team == "Brighton & Hove Albion" ? "Brighton" : team;
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
