using BusinessLogicLayer.Model;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MovieList
{
    /// <summary>
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {

        private static ComboBoxItem currentSortOption;

        private static User selectedUser;
        private static MainWindow mainWindow;
        private static Context context = new Context();
        private static UserRepository userRepo = new UserRepository(context);
        private static MovieRepository movieRepo = new MovieRepository(context);
        public EditUserWindow(MainWindow mainWindowIncoming, User incomingUser)
        {
            InitializeComponent();

            mainWindow = mainWindowIncoming;
            selectedUser = incomingUser;
            Listbox_allMovies.DisplayMemberPath = "TitleAndYear";
            Listbox_userMovies.DisplayMemberPath = "TitleAndYear";
            DropDown_userMovies.SelectedIndex = 0;
            RefreshMovieLists();
        }

        private void RefreshMovieLists()
        {
            labelEditingUser.Content = selectedUser.Name;
            var userMovieIds = selectedUser.Movies.Select(m => m.MovieId);
            var moviesNotOwned = movieRepo.GetAll().Where(m => !userMovieIds.Contains(m.MovieId));
            Listbox_userMovies.ItemsSource = selectedUser.Movies.ToList();
            Listbox_allMovies.ItemsSource = moviesNotOwned.ToList();
        }


        private void DropDown_userMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedSortingOption = (string)((ComboBoxItem)DropDown_userMovies.SelectedItem).Content;
            Listbox_userMovies.ItemsSource = null;
            Listbox_userMovies.Items.Clear();
            switch (selectedSortingOption)
            {
                case "By Release Year":
                    SortMoviesByYear();
                    break;

                case "By Title":
                    SortMoviesByTitle();
                    break;

                case "By MovieID":
                    SortMoviesByID();
                    break;

                case "By Hype":
                    SortMoviesByHype();
                    break;
            }
        }






        private void btnAddSelectedMovie_Click(object sender, RoutedEventArgs e)
        {
            if (Listbox_allMovies.SelectedItem != null)
            {
                var selectedMovie = (Movie)Listbox_allMovies.SelectedItem;

                // Add the movie to the selected user's list of movies
                selectedUser.Movies.Add(selectedMovie);

                // Save changes to the database
                var userRepo = new UserRepository(context);
                userRepo.Update(selectedUser);
                context.SaveChanges();

                // Refresh the movie lists
                RefreshMovieLists();
            }
        }

        

        private void btnRemoveSelectedMovie_click(object sender, RoutedEventArgs e)
        {
            if (Listbox_userMovies.SelectedItem != null)
            {
                var selectedMovie = (Movie)Listbox_userMovies.SelectedItem;

                // Fjern filmen på selectedUser.movies (De er ikke gemt i databasen endnu!)
                selectedUser.Movies.Remove(selectedMovie);
                context.SaveChanges();

                // Refresh!
                RefreshMovieLists();
            }
        }

        private void btnAcceptUserEdit_click(object sender, RoutedEventArgs e)
        {
            string username = txtbox_newUserName.Text;
            string pattern = @"^[a-zA-Z0-9]+(?:\s[a-zA-Z0-9]+)*$";
            string allMovieTitles = "";
            foreach (var movie in selectedUser.Movies)
            {
                allMovieTitles += movie.Title + "\n";
            }


            // tjek om navne-feltet er tomt (uændret navn)
            if (txtbox_newUserName.Text.Length == 0)
            {
                userRepo.Update(selectedUser);
                context.SaveChanges();
                MessageBox.Show("Alt er godt - " + selectedUser.Name + " er rettet. Beholder samme navn.", "👍", MessageBoxButton.OK, MessageBoxImage.Information);
                int index = mainWindow.UserListBox.Items.IndexOf(selectedUser);
                mainWindow.UserListBox.SelectedIndex = index;
                this.Close();
                mainWindow.RefreshData();

            }
            else if (!Regex.IsMatch(username, pattern))
            {
                MessageBox.Show("Brugernavnet må kun indeholde tal og bogstaver.\nSamt det ene mellemrum.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                // check for dublet!
                var checkNewUserName = userRepo.GetByName(txtbox_newUserName.Text);

                if (checkNewUserName != null)
                {
                    MessageBox.Show("Brugernavnet er allerede i brug. Vælg et andet.🤷‍♂️\nNavne-tjek er ikke case-sensitive. 'Jens' og 'jens' er det samme.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    // Opret User.
                    string oldname = selectedUser.Name;
                    selectedUser.Name = txtbox_newUserName.Text;
                    userRepo.Update(selectedUser);
                    context.SaveChanges();
                    MessageBox.Show("Alt er godt - " + oldname + " blev rettet. og hedder nu " + selectedUser.Name + "!", "👍", MessageBoxButton.OK, MessageBoxImage.Information);
                    int index = mainWindow.UserListBox.Items.IndexOf(selectedUser);
                    mainWindow.UserListBox.SelectedIndex = index;
                    this.Close();
                    mainWindow.RefreshData();
                }
            }
        }




        private void SortMoviesByYear()
        {
            if (DropDown_userMovies.Tag != null && (int)DropDown_userMovies.Tag == 1)
            {
                Listbox_userMovies.Items.SortDescriptions.Clear();
                Listbox_userMovies.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Descending));
                DropDown_userMovies.Tag = 0;
            }
            else
            {
                Listbox_userMovies.Items.SortDescriptions.Clear();
                Listbox_userMovies.Items.SortDescriptions.Add(new SortDescription("Year", ListSortDirection.Ascending));
                DropDown_userMovies.Tag = 1;
            }
        }

        private void SortMoviesByTitle()
        {
            if (DropDown_userMovies.Tag != null && (int)DropDown_userMovies.Tag == 1)
            {
                Listbox_userMovies.Items.SortDescriptions.Clear();
                Listbox_userMovies.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Descending));
                DropDown_userMovies.Tag = 0;
            }
            else
            {
                Listbox_userMovies.Items.SortDescriptions.Clear();
                Listbox_userMovies.Items.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
                DropDown_userMovies.Tag = 1;
            }
        }

        private void SortMoviesByID()
        {
            if (DropDown_userMovies.Tag != null && (int)DropDown_userMovies.Tag == 1)
            {
                Listbox_userMovies.Items.SortDescriptions.Clear();
                Listbox_userMovies.Items.SortDescriptions.Add(new SortDescription("MovieID", ListSortDirection.Descending));
                DropDown_userMovies.Tag = 0;
            }
            else
            {
                Listbox_userMovies.Items.SortDescriptions.Clear();
                Listbox_userMovies.Items.SortDescriptions.Add(new SortDescription("MovieID", ListSortDirection.Ascending));
                DropDown_userMovies.Tag = 1;
            }
        }

        private void SortMoviesByHype()
        {
            if (DropDown_userMovies.Tag != null && (int)DropDown_userMovies.Tag == 1)
            {
                Listbox_userMovies.Items.SortDescriptions.Clear();
                Listbox_userMovies.Items.SortDescriptions.Add(new SortDescription("Users.Count", ListSortDirection.Descending));
                DropDown_userMovies.Tag = 0;
            }
            else
            {
                Listbox_userMovies.Items.SortDescriptions.Clear();
                Listbox_userMovies.Items.SortDescriptions.Add(new SortDescription("Users.Count", ListSortDirection.Ascending));
                DropDown_userMovies.Tag = 1;
            }
        }


        private void ButtonReverseUserMovieList_click(object sender, RoutedEventArgs e)
        {
            var items = Listbox_userMovies.ItemsSource as IEnumerable<Movie>;
            if (items == null)
            {
                return;
            }

            // Create a new collection to hold the reversed items
            var reversedItems = new List<Movie>();

            // Add the items to the new collection in reverse order
            for (int i = items.Count() - 1; i >= 0; i--)
            {
                reversedItems.Add(items.ElementAt(i));
            }

            // Update the ListBox's ItemsSource with the reversed collection
            Listbox_userMovies.ItemsSource = reversedItems;
        }


    }
}

