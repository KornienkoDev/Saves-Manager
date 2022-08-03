using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SavesManager
{
    public partial class addGame : Window
    {

        readonly string game_in_list = Properties.Resources.game_in_list;
        readonly string images_type = Properties.Resources.images_type;
        readonly string image_not_found = Properties.Resources.image_not_found;
        readonly string enter_game_data = Properties.Resources.enter_game_data;


        Game newGame;
        string image_path;
        public addGame()
        {
            InitializeComponent();
            image_path = "index-of-assetsimg.png";  
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (gameName_UI.Text != string.Empty & gamePath_UI.Text != string.Empty) 
            {
                if (Game.games.Any(x => x.gameName == gameName_UI.Text))
                {
                    Info_UI.Text = game_in_list;
                    return;
                }
                else
                {
                    newGame = new Game(gameName_UI.Text, gamePath_UI.Text, image_path, getCurrentDatetime());

                    Game.games.Add(newGame);

                    MainWindow mw = Application.Current.MainWindow as MainWindow;
                    mw.GamesList_UI.Items.Refresh();
                    mw.SaveToDisk(mw.dataStorage, Game.games);

                    Close();

                }
            }
            else
            {
                Info_UI.Text = enter_game_data;
            }
 
        }

        public static string getCurrentDatetime()
        {
            DateTime dt = DateTime.Now;
            return dt.ToString();
        }

        private void gameImage_UI_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.Filters.Add(new CommonFileDialogFilter(images_type, "*.png"));
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    gameImage_UI.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    image_path = openFileDialog.FileName;
                }
                else Info_UI.Text = image_not_found;
            }
                
        }

        private void AddGame_Choose_Path_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog();
            fileDialog.IsFolderPicker = true;
            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                gamePath_UI.Text = fileDialog.FileName;
            }
        }
    }
}
