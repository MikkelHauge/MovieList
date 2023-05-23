using BusinessLogicLayer.Model;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MovieList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UserListBox.DisplayMemberPath = "Name";


            MovieListBox.DisplayMemberPath = "TitleAndYear";

            MovieListBoxAll.DisplayMemberPath = "TitleAndYear";

            using (var context = new Context())
            {
                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);


                var users = userRepo.GetAll().Include(u => u.Movies);

                foreach (var u in users)
                {
                    UserListBox.Items.Add(u);
                }
                UserListBox.SelectedIndex = 0;

                var movies = movieRepo.GetAll().Include(u => u.Users);

                foreach (var m in movies)
                {
                    MovieListBoxAll.Items.Add(m);
                }
                MovieListBoxAll.SelectedIndex = 0;
            }
        }

        public void RefreshData()
        {
            using (var context = new Context())
            {
                UserListBox.ItemsSource = null;
                UserListBox.Items.Clear();

                MovieListBox.ItemsSource = null;
                MovieListBox.Items.Clear();

                MovieListBoxAll.ItemsSource = null;
                MovieListBoxAll.Items.Clear();

                var userRepo = new UserRepository(context);
                var movieRepo = new MovieRepository(context);
                var users = userRepo.GetAll().OrderByDescending(u => u.UserId).ToList();

                UserListBox.ItemsSource = users;
                if (users.Count > 0)
                {
                    UserListBox.SelectedIndex = 0;
                }

                var selectedUser = UserListBox.SelectedItem as User;
                if (selectedUser != null)
                {
                    var userMovies = selectedUser.Movies;
                    MovieListBox.Items.Clear();
                    MovieListBox.ItemsSource = userMovies.OrderByDescending(m => m.MovieId).ToList();
                    if (userMovies.Count > 0)
                    {
                        MovieListBox.SelectedIndex = 0;
                    }
                }

                var allMovies = movieRepo.GetAll().OrderByDescending (m => m.MovieId).ToList();
                MovieListBoxAll.ItemsSource = allMovies;

            }
        }

        private void UserListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MovieListBox.ItemsSource = null;
            MovieListBox.Items.Clear();

            var selectedUser = (User)UserListBox.SelectedItem;
            if (selectedUser != null)
            {
                lblUserID.Content = "User ID: " + selectedUser.UserId.ToString();

                if (selectedUser.Movies.Count > 0)
                {
                    foreach (var movie in selectedUser.Movies)
                    {
                        if (movie != null)
                        {
                            MovieListBox.Items.Add(movie);

                            labelMovieTitel.Content = movie.Title + "       ID: " + movie.MovieId.ToString();
                        }
                    }
                    if (selectedUser.Movies.Count == 1)
                    {
                        lblUsersMovieList.Content = selectedUser.Name + "\nhas " + (selectedUser.Movies.Count + 1) + " movie on their list."; 
                        Errorlabel.Content = "Putting " + selectedUser.Movies.Count + " movie into MovieListBox.";
                    }
                    else
                    {
                        lblUsersMovieList.Content = selectedUser.Name + "\nhas " + (selectedUser.Movies.Count + 1) + " movies on their list.";

                        Errorlabel.Content = "Putting " + selectedUser.Movies.Count + " movies into MovieListBox.";
                    }

                }
                else
                {

                    Errorlabel.Content = "No movies found for the selected user.";
                }

            }

        }

        private void MovieListBox_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMovie = (Movie)MovieListBox.SelectedItem;
            if (selectedMovie != null)
            {
                using (var context = new Context())
                {
                    var movieRepo = new MovieRepository(context);
                    labelMovieTitel.Content = selectedMovie.Title + "       ID: " + selectedMovie.MovieId.ToString();
                    LabelWatchCount.Content = movieRepo.GetNumberOfUsersWithMovie(selectedMovie.MovieId) + " user(s) wants to see this movie.";

                    lblMovieTitel.Content = selectedMovie.Title;
                    lblMovieRelease.Content = selectedMovie.ReleaseYear.ToString();
                    lblMovieDescription.Text = selectedMovie.Description;
                }
            }
        }

        private void MovieListBoxAll_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedMovie = (Movie)MovieListBoxAll.SelectedItem;
            if (selectedMovie != null)
            {
                using (var context = new Context())
                {
                    var movieRepo = new MovieRepository(context);
                    labelMovieTitel.Content = selectedMovie.Title + "       ID: " + selectedMovie.MovieId.ToString();
                    LabelWatchCount.Content = movieRepo.GetNumberOfUsersWithMovie(selectedMovie.MovieId) + " user(s) wants to see this movie.";

                    lblMovieTitel.Content = selectedMovie.Title;
                    lblMovieRelease.Content = selectedMovie.ReleaseYear.ToString();
                    lblMovieDescription.Text = selectedMovie.Description;
                }
            }
        }

        private void ButtonCreateUser_click(object sender, RoutedEventArgs e)
        {
            var createUserWindow = new CreateUser(this);

            createUserWindow.ShowDialog();
        }

        private void ButtonEditUser_click(object sender, RoutedEventArgs e)
        {
            User selectedUser = (User)UserListBox.SelectedItem;
            if (selectedUser == null)
            {
                System.Windows.MessageBox.Show("Please select a user to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var editUserWindow = new EditUserWindow(this, selectedUser);

            editUserWindow.ShowDialog();
        }

        private void ButtonDeleteUser_click(object sender, RoutedEventArgs e)
        {

            User selectedUser = (User)UserListBox.SelectedItem;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete " + selectedUser.Name + "?", "Delete User", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (selectedUser != null && result == MessageBoxResult.OK)
            {
                // slet brugeren
                using (var context = new Context())
                {
                    var userRepo = new UserRepository(context);


                    userRepo.Delete(selectedUser);
                    context.SaveChanges();
                    Errorlabel.Content = selectedUser.Name + " has been deleted forever.";

                    RefreshData();
                }
            }
            else
            {
                // Cancel
                Errorlabel.Content = "Delete User cancelled.";

            }
        }

        private void ButtonCreateMovie_click(object sender, RoutedEventArgs e)
        {
            var createUserWindow = new CreateMovieWindow(this);

            createUserWindow.ShowDialog();
        }

        private void ButtonEditMovie_click(object sender, RoutedEventArgs e)
        {
            Movie selectedMovie = (Movie)MovieListBoxAll.SelectedItem;
            if (selectedMovie == null)
            {
                System.Windows.MessageBox.Show("Please select a movie to edit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var editMovieWindow = new EditMovie(this, selectedMovie);

            editMovieWindow.ShowDialog();
        }

        private void ButtonDeleteMovie_click(object sender, RoutedEventArgs e)
        {

            Movie selectedMovie = (Movie)MovieListBoxAll.SelectedItem;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete " + selectedMovie.Title + "?", "Delete User", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (selectedMovie != null && result == MessageBoxResult.OK)
            {
                // slet brugeren
                using (var context = new Context())
                {
                    var movieRepo = new MovieRepository(context);


                    movieRepo.Delete(selectedMovie);
                    context.SaveChanges();
                    Errorlabel.Content = selectedMovie.Title + " has been deleted forever.";

                    RefreshData();
                }
            }
            else
            {
                // Cancel
                Errorlabel.Content = "Delete Movie cancelled.";

            }
        }
    }
}
