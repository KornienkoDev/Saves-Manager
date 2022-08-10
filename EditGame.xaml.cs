using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SavesManager
{

    public partial class EditGame : Window
    {

        Game selected_game;
        MainWindow mw;
        String game_image_path;

        public EditGame()
        {
            InitializeComponent();

            mw = Application.Current.MainWindow as MainWindow;

            selected_game = Game.games[mw.GamesList_UI.SelectedIndex];

            if (selected_game.gameImage != "index-of-assetsimg.png")
            {
                Edit_GameImage_UI.Source = new BitmapImage(new Uri(selected_game.gameImage));
            }

            Edit_GameName_UI.Text = selected_game.gameName;
            Edit_GamePath_UI.Text = selected_game.gamePath;

        }

        private void Edit_GameImage_UI_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.Filters.Add(new CommonFileDialogFilter(Properties.Resources.images_type, "*.png"));
            if (openFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    Edit_GameImage_UI.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    game_image_path = openFileDialog.FileName;
                }
                else Edit_Info_UI.Text = Properties.Resources.image_not_found;
            }
        }


        private void Edit_Game_Choose_Path_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog();
            fileDialog.IsFolderPicker = true;
            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Edit_GamePath_UI.Text = fileDialog.FileName;
            }
        }

        private void Edit_Game_Button_Click(object sender, RoutedEventArgs e)
        {

            if (game_image_path != null)
            {
                selected_game.gameImage = game_image_path;
            }


            selected_game.gameName = Edit_GameName_UI.Text;
            selected_game.gamePath = Edit_GamePath_UI.Text;

            mw.GamesList_UI.Items.Refresh();
            mw.SaveToDisk(mw.dataStorage, Game.games);

            Close();


        }


    }
}
