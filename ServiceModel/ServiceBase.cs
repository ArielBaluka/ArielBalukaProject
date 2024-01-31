using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ServiceModel
{
    public class ServiceBase : IServiceBase
    {
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
            return res;
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
        {
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
        {
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

        public GameList GetNextmonthGames()
        {
            GameList list = PremierLeagueData.ReadGameSchedule();
            return list;
        }

        public void InsertNewGames()
        {
            GameList list = GetNextmonthGames();
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

            GameList games =new GameList(db.SelectAll().FindAll(g=>g.AWAYSCORE==-1));
            foreach (Game game in games)
            {
                Game current = list.Find(g => g.Date == game.Date && g.HOMETEAM.ID== game.HOMETEAM.ID && g.AWAYTEAM.ID == game.AWAYTEAM.ID);
                if (current != null)
                {
                    game.AWAYSCORE = current.AWAYSCORE;
                    game.HOMESCORE = current.HOMESCORE;
                    db.Update(game);
                }
            }
        }

        public int CalculateUserPoint(User user)
        {
            GuessDB guessDB = new GuessDB();
            GameDB gameDB = new GameDB();
            GuessList user_guesses = guessDB.GetUserGuesses(user);
            GameList games = gameDB.SelectAll();
            Game game = null;
            int score = 0;
            // the first id of a game in my gameDB is 3
            foreach (Guess guess in user_guesses)
            {
                game = games[guess.GAME.ID - 3];

                if (CheckGuess(guess, game))
                {
                    score += 25;
                }
            }
            return score;
        }

        private bool CheckGuess(Guess guess, Game game)
        {
            if (game.HOMESCORE == game.AWAYSCORE && guess.ISDRAW)
                return true;

            if(game.HOMESCORE > game.AWAYSCORE && guess.TEAMGUESSED.ID == game.HOMETEAM.ID)
                return true;

            if (game.AWAYSCORE > game.HOMESCORE && guess.TEAMGUESSED.ID == game.AWAYTEAM.ID)
                return true;

            return false;
        }
    }
}
