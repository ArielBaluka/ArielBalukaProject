using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceModel
{
    public class ServiceBase : IServiceBase
    {
        public static Dictionary<string, List<string>> groupsInfo = new Dictionary<string, List<string>>();

        static ServiceBase()
        {
            groupsInfo = PremierLeagueData.AdjustDict();
        }

        //public string GetGroupData(Group group)
        //{
        //    List<string> data = groupsInfo[group.GroupName];
        //    string info = "";
        //    foreach(string content in data)
        //    {
        //        info += content + "\n";
        //    }
        //    return info;
        //}

        public GameList GetAllGames()
        {
            GameDB db = new GameDB();
            GameList list = db.SelectAll();
            return list;
        }

        public GroupList GetAllGroups()
        {
            GroupDB db = new GroupDB();
            GroupList list = db.SelectAll();
            return list;
        }

        public GuessList GetAllGuesses()
        {
            GuessDB db = new GuessDB();
            GuessList list = db.SelectAll();
            return list;
        }

        public PlayerList GetAllPlayers()
        {
            PlayerDB db = new PlayerDB();
            PlayerList list = db.SelectAll();
            return list;
        }

        public UserList GetAllUsers()
        {
            UserDB db = new UserDB();
            UserList list = db.SelectAll();
            return list;
        }
        //
        public int InsertGame(Game game)
        {
            GameDB db = new GameDB();
            int res = db.Insert(game);
            return res;
        }

        public int InsertGroup(Group group)
        {
            GroupDB db = new GroupDB();
            int res = db.Insert(group);
            return res;
        }

        public int InsertGuess(Guess guess)
        {
            GuessDB db = new GuessDB();
            int res = db.Insert(guess);
            return res;
        }

        public int InsertPlayer(Player player)
        {
            PlayerDB db = new PlayerDB();
            int res = db.Insert(player);
            return res;
        }

        public int InsertUser(User user)
        {
            UserDB db = new UserDB();
            int res = db.Insert(user);
            return GetAllUsers().Last().ID;
        }
        

        public int UpdateGame(Game game)
        {
            GameDB db = new GameDB();
            int res = db.Update(game);
            return res;
        }

        public int UpdateGroup(Group group)
        {
            GroupDB db = new GroupDB();
            int res = db.Update(group);
            return res;
        }

        public int UpdateGuess(Guess guess)
        {
            GuessDB db = new GuessDB();
            int res = db.Update(guess);
            return res;
        }

        public int UpdatePlayer(Player player)
        {
            PlayerDB db = new PlayerDB();
            int res = db.Update(player);
            return res;
        }

        public int UpdateUser(User user)
        {
            UserDB db = new UserDB();
            int res = db.Update(user);
            return res;
        }


        public int DeleteGame(Game game)
        {// delete from guess
            GuessDB guessDB = new GuessDB();
            guessDB.DeleteByGame(game);
            GameDB db = new GameDB();
            int res = db.Delete(game);
            return res;
        }

        public int DeleteGroup(Group group)
        {
            GroupDB db = new GroupDB();
            int res = db.Delete(group);
            return res;
        }

        public int DeleteGuess(Guess guess)
        {
            GuessDB db = new GuessDB();
            int res = db.Delete(guess);
            return res;
        }

        public int DeletePlayer(Player player)
        {
            PlayerDB db = new PlayerDB();
            int res = db.Delete(player);
            return res;
        }

        public int DeleteUser(User user)
        {// delete from guesses
            GuessDB guessDB = new GuessDB();
            guessDB.DeleteByUser(user);
            UserDB db = new UserDB();
            int res = db.Delete(user);
            return res;
        }
        public User Login(User user)
        {
            UserDB db = new UserDB();
            User LoggedUser = db.Login(user);
            if(LoggedUser != null && LoggedUser.UserName.Equals(user.UserName) 
                && LoggedUser.PassWord.Equals(user.PassWord))
                return LoggedUser;
            return null;
        }

        public GameList GetGameResults()
        {
            GameList list = PremierLeagueData.ParseRSSEPL();
            return list;
        }

        public GameList GetNextmonthGames(int days)
        {
            GameList list = PremierLeagueData.ReadGameSchedule(days);
            return list;
        }

        public void InsertNewGames(int days)
        {
            GameList list = GetNextmonthGames(days);
            GameDB db = new GameDB();
            foreach(Game game in list)
            {
                if(!db.isExist(game))
                {
                    db.Insert(game);
                }
            }
        }

        public void LoadResults()
        {
            GameList list = GetGameResults();
            GameDB db = new GameDB();
            GroupDB Gdb = new GroupDB();
            GroupList groups = Gdb.SelectAll();

            GameList games =new GameList(db.SelectAll().FindAll(g=>g.AWAYSCORE==-1));
            foreach (Game game in games)
            {
                Game current = list.Find(g => g.Date == game.Date && g.HOMETEAM.ID== game.HOMETEAM.ID && g.AWAYTEAM.ID == game.AWAYTEAM.ID);
                if (current != null)
                {
                    game.AWAYSCORE = current.AWAYSCORE;
                    game.HOMESCORE = current.HOMESCORE;
                    db.Update(game);
                    AddGroupPoints(game, groups);
                    Gdb.Update(game.HOMETEAM);
                    Gdb.Update(game.AWAYTEAM);
                }
                else if(game.Date < DateTime.Today)// no result and game score not found
                {
                    GameList gm = new GameList(db.SelectAll().FindAll(g => g.Date > game.Date));//getting all the future games
                    foreach(Game futureGmae in gm)
                    {
                        if (futureGmae.HOMETEAM == game.HOMETEAM && futureGmae.AWAYTEAM == game.AWAYTEAM)
                        {//found the correct delayed game
                            GuessList guesses = GetGuessesByGame(game);
                            foreach (Guess guess in guesses)
                            {
                                guess.GAME = futureGmae;
                                InsertGuess(guess);
                            }
                        }
                    }
                    DeleteGame(game);//remove the delayed game
                }
            }
            if (games[0].HOMETEAM.Points == 0)
            {
                UpdateGroupsFirstTime();
            }

        }

        public void UpdateGroupsFirstTime()
        {
            GameDB db = new GameDB();
            GroupDB Gdb = new GroupDB();
            GroupList groups = Gdb.SelectAll();
            GameList gameLst = new GameList(db.SelectAll().FindAll(g => g.AWAYSCORE != -1));
            foreach (Game g in gameLst)
            {
                AddGroupPoints(g, groups);
            }
            foreach (Group g in groups)
            {
                Gdb.Update(g);
            }
        }

        public void AddGroupPoints(Game game, GroupList groups)
        {
            if(game.HOMESCORE > game.AWAYSCORE)
            {
                game.HOMETEAM.Points += 3;
            }
            else if (game.AWAYSCORE > game.HOMESCORE)
            {
                game.AWAYTEAM.Points += 3;
            }
            else
            {
                game.AWAYTEAM.Points += 1;
                game.HOMETEAM.Points += 1;
            }
            groups[game.HOMETEAM.ID-1].Points += game.HOMETEAM.Points;
            groups[game.AWAYTEAM.ID-1].Points += game.AWAYTEAM.Points;
        }

        public int CalculateUserPoint(User user)
        {
            GuessDB guessDB = new GuessDB();
            GameDB gameDB = new GameDB();
            GuessList user_guesses = guessDB.GetUserGuesses(user);
            int score = 0;
            foreach (Guess guess in user_guesses)
            {
                if (guess.ISCORRECT)
                {
                    score += 25;
                }
            }
            return score;
        }

        private bool CheckGuess(Guess guess, Game game)
        {
            if (game.HOMESCORE != -1 && game.HOMESCORE == game.AWAYSCORE && guess.ISDRAW)
                return true;

            if(game.HOMESCORE > game.AWAYSCORE && guess.TEAMGUESSED.ID == game.HOMETEAM.ID)
                return true;

            if (game.AWAYSCORE > game.HOMESCORE && guess.TEAMGUESSED.ID == game.AWAYTEAM.ID)
                return true;

            return false;
        }

        public void UpdateAllGuesses()
        {
            GuessList guesses = GetAllGuesses();
            GameList games = GetAllGames();
            Game game = null;
            foreach (Guess guess in guesses)
            {
                game = games.FindAll(x => x.ID == guess.GAME.ID)[0];
                if (CheckGuess(guess, game))
                {
                    guess.ISCORRECT = true;
                    UpdateGuess(guess);
                }
            }
        }

        public bool DoesPlayerExist(Player p)
        {
            PlayerDB db = new PlayerDB();
            if(db.IsExist(p))
            {
                return true;
            }
            return false;
        }

        public GroupList GetAllGroupsByPoints()
        {
            GroupDB db = new GroupDB();
            GroupList list = db.SelectAllByPoints();
            return list;
        }

        public PlayerList GetPlayersByGroup(Group group)
        {
            PlayerDB playerDB = new PlayerDB();
            PlayerList players = playerDB.SelectByGroup(group);
            return players;
        }

        public PlayerList GetPlayersByUser(User user)
        {
            PlayerDB playerDB = new PlayerDB();
            PlayerList players = playerDB.SelectByUser(user);
            return players;
        }

        public GuessList GetGuessesByGame(Game game)
        {
            GuessDB guessDB = new GuessDB();
            GuessList guesses = guessDB.SelectByGame(game);
            return guesses;
        }
    }
}
