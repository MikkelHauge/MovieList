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
            Listbox_userMovies.ItemsSource = Enumerable.Empty<Movie>();

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


        private void btnAddSelectedMovie_Click(object sender, RoutedEventArgs e)
        {
            if (Listbox_allMovies.SelectedItem != null)
            {
                using(var context = new Context())
                {
                    var userRepo = new UserRepository(context);

                    var selectedMovie = (Movie)Listbox_allMovies.SelectedItem;

                    // Add the movie to the selected user's list of movies
                    selectedUser.Movies.Add(selectedMovie);

                    // Save changes to the database
                    userRepo.Update(selectedUser);
                    context.SaveChanges();

                    // Refresh the movie lists
                    RefreshMovieLists();
                }

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
                    try
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
                    } catch
                    {
                        MessageBox.Show("Brugernavnet er ikke godkendt. Vælg et andet.🤷‍♂️", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        txtbox_newUserName.Text = "";
                    }

                }
            }
        }




        private void DropDown_userMovies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedSortingOption = (string)((ComboBoxItem)DropDown_userMovies.SelectedItem).Content;
            List<Movie> sortedMovies;
            switch (selectedSortingOption)
            {
                case "By Release Year":
                    sortedMovies = SortMoviesByYear();
                    break;

                case "By Title":
                    sortedMovies = SortMoviesByTitle();
                    break;

                case "By MovieID":
                    sortedMovies = SortMoviesByID();
                    break;

                case "By Hype":
                    sortedMovies = SortMoviesByHype();
                    break;

                default:
                    sortedMovies = selectedUser.Movies.ToList();
                    break;
            }

            Listbox_userMovies.ItemsSource = sortedMovies;
        }

        private List<Movie> SortMoviesByYear()
        {
            return selectedUser.Movies.OrderBy(m => m.ReleaseYear).ToList();
        }

        private List<Movie> SortMoviesByTitle()
        {
            return selectedUser.Movies.OrderBy(m => m.Title).ToList();
        }

        private List<Movie> SortMoviesByID()
        {
            return selectedUser.Movies.OrderBy(m => m.MovieId).ToList();
        }

        private List<Movie> SortMoviesByHype()
        {
            return selectedUser.Movies.OrderByDescending(m => m.Users.Count).ToList();
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

