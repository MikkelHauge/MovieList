using BusinessLogicLayer.Model;
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
    /// Interaction logic for CreateMovieWindow.xaml
    /// </summary>
    public partial class CreateMovieWindow : Window
    {
        private static MainWindow mainWindow;
        public static Movie createdMovie;
        public CreateMovieWindow(MainWindow mainWindowIncoming)
        {
            InitializeComponent();

            mainWindow = mainWindowIncoming;
        }
        private void btnCreateMovie_click(object sender, RoutedEventArgs e)
        {
            Movie newMovie = new Movie();
            int releaseYear;
            newMovie.Title = textbox_createmovie_title.Text;
            newMovie.Description = textbox_createmovie_description.Text;
            try
            {
                Int32.TryParse(textbox_createmovie_release.Text, out releaseYear);
                newMovie.ReleaseYear = releaseYear;
            }
            catch (Exception ex)
            {
                // handle error here
                MessageBox.Show(ex.Message);
                return;
            }

            using (var context = new Context())
            {
                var movieRepo = new MovieRepository(context);
                movieRepo.Add(newMovie);

                MessageBox.Show("Movie Created!\n\nTitle: " + newMovie.Title + "\nRelease Year: "+newMovie.ReleaseYear);

                mainWindow.RefreshData(); // refresh the UI to show the new movie in the list
                this.Close();
            }
        }


        private void btnCancelCreateMovie_click(object sender, RoutedEventArgs e)
        {
            // cancel movie creation - close window
        }
    }
}
