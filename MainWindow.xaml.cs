using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace SavesManager
{

    public partial class MainWindow : Window
    {
        public string dataStorage = "storage.dat";

        string[] empty = { };

        string savesPath = @"D:\Saves";

        readonly string no_save_folder = Properties.Resources.no_save_folder;
        readonly string successfully_saved = Properties.Resources.successfully_saved;
        readonly string successfully_restored = Properties.Resources.successfully_restored;
        readonly string choose_game = Properties.Resources.choose_game;
        readonly string filesAreSynced = Properties.Resources.filesAreSynced;
        readonly string empty_game_list = Properties.Resources.empty_game_list;
        readonly string sorted = Properties.Resources.sorted;
        readonly string doYouWantToDelete = Properties.Resources.doYouWantToDelete;
        readonly string gameRemoval = Properties.Resources.gameRemoval;
        readonly string doYouWantToBackup = Properties.Resources.doYouWantToBackup;
        readonly string gamesSaving = Properties.Resources.gamesSaving;
        readonly string doYouWantToRestore = Properties.Resources.doYouWantToRestore;
        readonly string gamesRestoring = Properties.Resources.gamesRestoring;

        bool thereWasNewFile = false;

        string gameDateTime;

        public MainWindow()
        {
            InitializeComponent();

            if (!File.Exists(dataStorage))
            {
                File.WriteAllLines(dataStorage, empty);
            }

            ReadFromDisk(dataStorage, Game.games);
            GamesList_UI.ItemsSource = Game.games;
            SelectedPath_UI.Text = savesPath;

        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            addGame _add_game_window = new addGame()
            {

                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
                Owner = this

            };
            _add_game_window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _add_game_window.ShowDialog();
        }

        private void RemoveGame_Click(object sender, RoutedEventArgs e)
        {
            if (Game.games.Count > 0)
            {
                if (GamesList_UI.SelectedItem != null)
                {

                    MessageBoxResult result = MessageBox.Show(doYouWantToDelete + Game.games[GamesList_UI.SelectedIndex].gameName + "?", gameRemoval, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.OK)
                    {

                        string selectedGamePath = savesPath + @"\" + Game.games[GamesList_UI.SelectedIndex].gameName;

                        if (Directory.Exists(selectedGamePath)) Directory.Delete(selectedGamePath, true);

                        Game.games.RemoveAt(GamesList_UI.SelectedIndex);
                        GamesList_UI.Items.Refresh();
                        SaveToDisk(dataStorage, Game.games);
                    }
                }
                else
                {
                    UpdateInfo(choose_game);
                }
            }
            else
            {
                UpdateInfo(empty_game_list);
            }
        }

        private void SortGames_UI_Click(object sender, RoutedEventArgs e)
        {
            if (Game.games.Count > 0)
            {
                Game.games.Sort((x, y) => x.gameName.CompareTo(y.gameName));
                GamesList_UI.Items.Refresh();
                UpdateInfo(sorted);
            }
            else
            {
                UpdateInfo(empty_game_list);
            }

        }

        private void BackupAll_Click(object sender, RoutedEventArgs e)
        {
            if (Game.games.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show(doYouWantToBackup, gamesSaving, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.OK)
                {
                    foreach (var game in Game.games)
                    {
                        string source, target;

                        source = game.gamePath;
                        target = savesPath + @"\" + game.gameName;

                        if (Directory.Exists(source))
                        {
                            CopyDirectory(source, target, true);

                            if (thereWasNewFile)
                            {
                                game.dateTime = gameDateTime;
                                GamesList_UI.Items.Refresh();
                                UpdateInfo(successfully_saved);
                            }
                            else UpdateInfo(filesAreSynced);
                        }
                        else { UpdateInfo(no_save_folder); break; }
                    }
                }
            }
            else UpdateInfo(empty_game_list);

            thereWasNewFile = false;
        }

        private void RestoreAll_Click(object sender, RoutedEventArgs e)
        {
            if (Game.games.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show(doYouWantToRestore, gamesRestoring, MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.OK)
                {
                    foreach (var game in Game.games)
                    {
                        string source, target;

                        source = savesPath + @"\" + game.gameName;
                        target = game.gamePath;

                        if (Directory.Exists(source))
                        {

                            CopyDirectory(source, target, true);

                            if (thereWasNewFile)
                            {
                                game.dateTime = gameDateTime;
                                GamesList_UI.Items.Refresh();
                                UpdateInfo(successfully_restored);
                            }
                            else UpdateInfo(filesAreSynced);
                        }
                        else { UpdateInfo(no_save_folder); break; }
                    }
                }
            }
            else UpdateInfo(empty_game_list);

            thereWasNewFile = false;
        }

        private void BackupSelectedGame_Click(object sender, RoutedEventArgs e)
        {
            if (GamesList_UI.SelectedItem != null)
            {
                Game selectedGame = Game.games[GamesList_UI.SelectedIndex];

                string source, target;

                source = selectedGame.gamePath;
                target = savesPath + @"\" + selectedGame.gameName;

                if (Directory.Exists(source))
                {
                    CopyDirectory(source, target, true);

                    if (thereWasNewFile)
                    {
                        Game.games[GamesList_UI.SelectedIndex].dateTime = DateTime.Now.ToString();
                        GamesList_UI.Items.Refresh();
                        UpdateInfo(successfully_saved);
                    }

                    else UpdateInfo(filesAreSynced);
                }
                else UpdateInfo(no_save_folder);
            }
            else UpdateInfo(choose_game);

            thereWasNewFile = false;
        }

        private void RestoreSelectedGame_Click(object sender, RoutedEventArgs e)
        {
            if (GamesList_UI.SelectedItem != null)
            {

                Game selectedGame = Game.games[GamesList_UI.SelectedIndex];

                string source, target;

                source = savesPath + @"\" + selectedGame.gameName;
                target = selectedGame.gamePath;

                if (Directory.Exists(source))
                {
                    CopyDirectory(source, target, true);

                    if (thereWasNewFile)
                    {
                        selectedGame.dateTime = DateTime.Now.ToString();
                        GamesList_UI.Items.Refresh();
                        UpdateInfo(successfully_restored);
                    }

                    else UpdateInfo(filesAreSynced);
                }
                else UpdateInfo(no_save_folder);
            }
            else UpdateInfo(choose_game);

            thereWasNewFile = false;
        }

        private void SelectPath_UI_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog fileDialog = new CommonOpenFileDialog();
            fileDialog.IsFolderPicker = true;
            if (fileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                savesPath = fileDialog.FileName;
                SelectedPath_UI.Text = savesPath;
            }

        }

        private void SelectedPath_UI_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            savesPath = SelectedPath_UI.Text;
        }

        private void UpdateInfo(string info)
        {
            SavedText_UI.Text = info;
        }

        private void ReadFromDisk(string filepath, List<Game> _games)
        {

            List<string> _lines = File.ReadAllLines(filepath).ToList();


            foreach (var line in _lines)
            {
                string[] entries = line.Split(',');

                if (entries.Length != 4) { return; }

                else
                {
                    Game newGame = new Game(entries[0], entries[1], entries[2], entries[3]);

                    _games.Add(newGame);
                }
            }
        }

        public void SaveToDisk(string filepath, List<Game> _games)
        {

            File.WriteAllLines(filepath, empty);

            foreach (var game in _games)
            {

                string[] entries = { game.gameName + "," + game.gamePath + "," + game.gameImage + "," + game.dateTime };

                File.AppendAllLines(filepath, entries);
            }
        }

        private void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            var dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destinationDir);

            foreach (FileInfo source_file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, source_file.Name);

                var targetFileLastWriteTime = File.GetLastWriteTime(targetFilePath);

                if (source_file.LastWriteTime > targetFileLastWriteTime)
                {
                    thereWasNewFile = true;
                    gameDateTime = addGame.getCurrentDatetime(); //source_file.LastWriteTime.ToString();
                    source_file.CopyTo(targetFilePath, true);
                }
            }

            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveToDisk(dataStorage, Game.games);
        }

        private void About_UI_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            About _about_game_window = new About()
            {
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
                Owner = this

            };
            _about_game_window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _about_game_window.ShowDialog();
        }

        private void OpenFolderMenu_Click(object sender, RoutedEventArgs e)
        {
            Game selectedGame = Game.games[GamesList_UI.SelectedIndex];
            String selectedGamePath = selectedGame.gamePath;
            if (Directory.Exists(selectedGame.gamePath))
            {
                Process.Start(selectedGamePath);
            }
            else
            {
                UpdateInfo(no_save_folder);
            }

        }

        private void EditRecordMenu_Click(object sender, RoutedEventArgs e)
        {
            EditGame _edit_game_window = new EditGame()
            {
                ResizeMode = ResizeMode.NoResize,
                ShowInTaskbar = false,
                Owner = this

            };
            _edit_game_window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _edit_game_window.ShowDialog();
        }
    }
}