using BusinessLogicLayer.Model;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MovieList
{
    /// <summary>
    /// Interaction logic for EditMovie.xaml
    /// </summary>
    public partial class EditMovie : Window
    {
        private static Movie selectedMovie;
        private static MainWindow mainWindow;
        private static Context context = new Context();
        private static UserRepository userRepo = new UserRepository(context);
        private static MovieRepository movieRepo = new MovieRepository(context);
        public EditMovie(MainWindow mainWindowIncoming, Movie selectedMovieIncoming)
        {
            InitializeComponent();
            mainWindow = mainWindowIncoming;
            selectedMovie = selectedMovieIncoming;
            txtbox_editmovie_title.Text = selectedMovie.Title;
            txtbox_editmovie_releaseyear.Text = "" + selectedMovie.ReleaseYear; // det er en int!
            txtbox_editmovie_description.Text = selectedMovie.Description;
            lblMovieTitle.Content = selectedMovie.Title+ "\n" + selectedMovie.ReleaseYear;
        }

        private void btnAcceptMovieEdit_click(object sender, RoutedEventArgs e)
        {
            bool movieEditWentWell = true;
            int year;
            selectedMovie.Title = txtbox_editmovie_title.Text.Trim();
            if(int.TryParse(txtbox_editmovie_releaseyear.Text.Trim(), out year) && year > 1850 && year < 2100)
            {
                selectedMovie.ReleaseYear = year;
                if(selectedMovie.Title == ""){
                    MessageBox.Show("Filmen skal have en titel.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    movieEditWentWell = false;
                } else
                {
                    selectedMovie.Title = txtbox_editmovie_title.Text.Trim();
                    movieEditWentWell = true;
                }
            }
            else
            {
                MessageBox.Show("Release year er ikke et godkendt årstal. Prøv igen.\nFormat: YYYY. Mellem 1851 og 2099\nEksempel: 1993", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                movieEditWentWell = false;
            }
            if (movieEditWentWell)
            {
                try
                {
                    movieRepo.Update(selectedMovie);
                    context.SaveChanges();
                    MessageBox.Show("Filmen blev rettet", "👍", MessageBoxButton.OK, MessageBoxImage.Information);
                    int index = mainWindow.MovieListBoxAll.Items.IndexOf(selectedMovie);
                    mainWindow.MovieListBoxAll.SelectedIndex = index;
                    this.Close();
                    mainWindow.RefreshData();
                } catch (Exception ex)
                {
                    MessageBox.Show("Fejl. Filmen kunne ikke oprettes\n\n"+ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }


        }
    }
}
