using System.Collections.Generic;


namespace SavesManager
{
    public class Game
    {
        public string gameName { get; set; }
        public string gamePath { get; set; }
        public string gameImage { get; set; } = "index-of-assetsimg.png";

        public string dateTime { get; set; } = "01.01.1970 00:00:00";

        public static List<Game> games = new List<Game>();

        public Game(string gameName, string gamePath, string gameImage, string dateTime)
        {
            this.gameName = gameName;
            this.gamePath = gamePath;
            this.dateTime = dateTime;
            

            if (System.IO.File.Exists(gameImage))
            {
                this.gameImage = gameImage;
            }

        }

    }
}
