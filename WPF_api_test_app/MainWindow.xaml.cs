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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using WPF_api_test_app.DTO;

using System.Text.Json;
using BusinessLogicLayer.Model;
using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Net;

namespace WPF_api_test_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // dobelt tjek om porten er korrekt, hvis det ikke fungerer.

        public static string webappurl = "https://localhost:44372/"; // <--- der

        public static List<User> users = new List<User>();
        public static List<Movie> movies = new List<Movie>();
        public MainWindow()
        {
            InitializeComponent();


        }
        private void updateListbox()
        {
            
            listbox_allUsers.ItemsSource = null;
            listbox_allUsers.ItemsSource = users;
            listbox_allUsers.DisplayMemberPath = "nameAndId";

            listbox_allMovies.ItemsSource = null;
            listbox_allMovies.ItemsSource = movies;
            listbox_allMovies.DisplayMemberPath = "TitleAndId";

           
        }


        private void btnGetAllUsers_click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    Task<string> task = client.GetStringAsync(webappurl + "api/UsersAPI/");

                    var taskResult = task.Result;

                    var usersJSON = JsonSerializer.Deserialize<List<UserDTO>>(taskResult);

                    foreach (var newUser in usersJSON)
                    {
                        if (!users.Any(p => p.UserId == newUser.UserId))
                        {
                            User user = new User();
                            user.UserId = newUser.UserId;
                            user.Name = newUser.Name;

                            users.Add(user);
                        }
                    }

                    updateListbox();

                }
            } catch(Exception ex)
            {
                textbox_errormsg.Text = ex.Message;
            }
            
        }

        private void btnGetAllMovies_click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    Task<string> task = client.GetStringAsync(webappurl + "api/MoviesAPI/");

                    var taskResult = task.Result;

                    var movieJSON = JsonSerializer.Deserialize<List<MovieDTO>>(taskResult);

                    foreach (var newMovie in movieJSON)
                    {
                        if (!movies.Any(p => p.MovieId == newMovie.MovieId))
                        {
                            Movie movie = new Movie();
                            movie.MovieId = newMovie.MovieId;
                            movie.Title = newMovie.Title;
                            movie.Description = newMovie.Description;
                            movie.ReleaseYear = newMovie.ReleaseYear;

                            movies.Add(movie);
                        }
                    }

                    updateListbox();

                } 
            } catch (Exception ex)
            {
                textbox_errormsg.Text = ex.Message;
            }
            
        }

        private void btnGetUserWithID_click(object sender, RoutedEventArgs e)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    string userID = textbox_idGetUser.Text;
                    int id;
                    if (int.TryParse(userID, out id))
                    {
                        Task<string> task = client.GetStringAsync(webappurl + "api/UsersAPI/" + id);

                        var taskResult = task.Result;

                        var user = JsonSerializer.Deserialize<UserDTO>(taskResult);
                        textbox_userName.Text = user.Name;
                        lbl_user_id.Content = user.UserId;

                        
                        listbox_users_movieList.ItemsSource = user.MovieIds; 



                        updateListbox();
                        textbox_idGetUser.Text = "";
                    }

                }
            } catch (Exception ex)
            {
                textbox_errormsg.Text = ex.Message;
            }
            
        }

        private void btnGetMovieWithID_click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    string movieID = textbox_idGetMovie.Text;
                    int id;
                    if (int.TryParse(movieID, out id))
                    {
                        Task<string> task = client.GetStringAsync(webappurl + "api/MoviesAPI/" + id);

                        var taskResult = task.Result;
                        textbox_errormsg.Text = taskResult.ToString();
                        var movie = JsonSerializer.Deserialize<MovieDTO>(taskResult);
                        textbox_GetMovie_title.Text = movie.Title;
                        textbox_GetMovie_releaseyear.Text = ""+movie.ReleaseYear;
                        textbox_GetMovie_description.Text = movie.Description;
                        lbl_movie_detail_id.Content = movie.MovieId;
                        textbox_idGetMovie.Text = "";
                        
                    }

                }
            }
            catch (Exception ex)
            {
                textbox_errormsg.Text = ex.Message;
            }
        }

        private async void btnAddMovieToUser_click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    string userIDstring = lbl_user_id.Content.ToString();
                    int UserId;
                    if (int.TryParse(userIDstring, out UserId))
                    {
                        Movie movie = (Movie)listbox_allMovies.SelectedItem;

                        HttpResponseMessage response = await client.PutAsync(webappurl + $"api/UsersAPI/{UserId}/AddMovie/{movie.MovieId}", null);

                        if (response.IsSuccessStatusCode)
                        {
                            textbox_errormsg.Text = "Movie added to user.";
                            UpdateUsersMovieList(UserId);
                        }
                        else
                        {
                            textbox_errormsg.Text = $"Error adding movie to user. Status code: {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                textbox_errormsg.Text = ex.Message;
            }

        }

        private async void btnRemoveMovie_click(object sender, RoutedEventArgs e)
        {
            // "remove" movie from user.
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    string userIDstring = lbl_user_id.Content.ToString();
                    int UserId;
                    if (int.TryParse(userIDstring, out UserId))
                    {
                        var movieid = listbox_users_movieList.SelectedItem;
                        int idofMovie = (int)movieid;

                        HttpResponseMessage response = await client.PutAsync(webappurl + $"api/UsersAPI/{UserId}/RemoveMovie/{idofMovie}", null);

                        if (response.IsSuccessStatusCode)
                        {
                            textbox_errormsg.Text = "Movie removed from user.";
                            UpdateUsersMovieList(UserId);
                        }
                        else
                        {
                            textbox_errormsg.Text = $"Error removing movie from user. Status code: {response.StatusCode}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                textbox_errormsg.Text = ex.Message;
            }
        }

        private async void btnDeleteUser_click(object sender, RoutedEventArgs e)
        {
            try
            {
                User user = (User)listbox_allUsers.SelectedItem;
                textbox_errormsg.Text = user.Name;
                int UserId = user.UserId;
                using (HttpClient client = new HttpClient())
                { // api/UsersAPI/{id}
                    HttpResponseMessage response = await client.DeleteAsync(webappurl + $"api/UsersAPI/{UserId}");
                    if (response.IsSuccessStatusCode)
                    {
                        textbox_errormsg.Text = $"The user {user.Name} has been deleted from the database.";
                        btnGetAllUsers_click(null, null);
                    }
                    else
                    {
                        textbox_errormsg.Text = $"Error deleting user. Status code: {response.StatusCode}";
                    }
                }

            } catch (Exception ex)
            {
                textbox_errormsg.Text = "Error deleting user\n\n" + ex.Message;
            }
        }

        private async void btnDeleteMovie_click(object sender, RoutedEventArgs e)
        {
            try
            {
                Movie movie = (Movie)listbox_allMovies.SelectedItem;
                textbox_errormsg.Text = movie.Title;
                int MovieID = movie.MovieId;
                using (HttpClient client = new HttpClient())
                { // api/UsersAPI/{id}
                    HttpResponseMessage response = await client.DeleteAsync(webappurl + $"api/MoviesAPI/{MovieID}");
                    if (response.IsSuccessStatusCode)
                    {
                        textbox_errormsg.Text = $"The movie {movie.Title} has been deleted from the database.";
                        btnGetAllMovies_click(null, null); // hent alle film (opdater listen)
                    }
                    else
                    {
                        textbox_errormsg.Text = $"Error deleting movie. Status code: {response.StatusCode}";
                    }
                }

            }
            catch (Exception ex)
            {
                textbox_errormsg.Text = "Error deleting movie\n\n" + ex.Message;
            }
        }

        private async void btnCreateNewUser_click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string username = textbox_newUserName.Text;
                    User user = new User();
                    user.Name = username;

                    string jsonData = JsonConvert.SerializeObject(user);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(webappurl + $"api/UsersAPI", content);
                    if (response.IsSuccessStatusCode)
                    {
                        textbox_errormsg.Text = $"The user has been created succesfully!";
                        textbox_newUserName.Text = "";
                        btnGetAllUsers_click(null, null); // opdater listen af users

                    }
                    else
                    {
                        textbox_errormsg.Text = $"Error creating user. Status code: {response.StatusCode} \n" + content.ToString()+ "\n" + jsonData.ToString();

                    }
                }
            } catch (Exception ex)
            {
                textbox_errormsg.Text = ex.Message;
            }

        }

        private async void btnCreateNewMovie_click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string description = textbox_newMovieDescription.Text;
                    string title = textbox_newMovieTitle.Text;
                    string releaseYearText = textbox_newMovieReleaseYear.Text;


                    int releaseYearint;
                    bool isvalidYear = int.TryParse(releaseYearText, out releaseYearint);

                    if(isvalidYear)
                    {

                        Movie movie = new Movie();
                        movie.Title = title;
                        movie.Description = description;
                        movie.ReleaseYear = releaseYearint;
                        string jsonData = JsonConvert.SerializeObject(movie);
                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync(webappurl + $"api/MoviesAPI", content);
                        if (response.IsSuccessStatusCode)
                        {
                            textbox_errormsg.Text = $"The movie has been created succesfully!";
                            textbox_newMovieDescription.Text = "";
                            textbox_newMovieTitle.Text = "";
                            textbox_newMovieReleaseYear.Text = "";
                            btnGetAllMovies_click(null, null); // opdater listen af users

                        }
                        else
                        {
                            textbox_errormsg.Text = $"Error creating movie. Status code: {response.StatusCode} \n" + content.ToString() + "\n" + jsonData.ToString();

                        }
                    } else
                    {
                        textbox_errormsg.Text = "Release Year is incorrect. Has to be a valid number (for example: 2001)";
                    }
                }
            }
            catch (Exception ex)
            {
                textbox_errormsg.Text = ex.Message;
            }
        }

        private async void UpdateUsersMovieList(int UserId)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    HttpResponseMessage response = await client.GetAsync(webappurl + $"api/UsersAPI/{UserId}");
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        UserDTO user = JsonSerializer.Deserialize<UserDTO>(responseContent);

                        if (user != null)
                        {
                            listbox_users_movieList.ItemsSource = user.MovieIds;

                        } else
                        {
                            textbox_errormsg.Text = "User not found";
                        }
                    }
                }
            } catch (Exception ex)
            {
                textbox_errormsg.Text = "error: " + ex.Message;
            }
          
        }

        private async void btnEditUser_click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    int userID;
                    bool isValidId = int.TryParse(lbl_user_id.Content.ToString(), out userID);

                    if (isValidId)
                    {
                        textbox_errormsg.Text = "updating user with id: " + userID;
                        client.DefaultRequestHeaders.Accept.Clear();

                        User userToUpdate = new User();
                        userToUpdate.Name = textbox_userName.Text;

                        string jsonContent = JsonSerializer.Serialize(userToUpdate);
                        HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PutAsync(webappurl + $"api/UsersAPI/{userID}", httpContent);
                        if (response.IsSuccessStatusCode)
                        {
                            textbox_errormsg.Text = "User has been edited.";

                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            textbox_errormsg.Text = "User not found";
                        }
                        else
                        {
                            textbox_errormsg.Text = "Error updating user. Status code: " + response.StatusCode;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                textbox_errormsg.Text = "Error: " + ex.Message;
            }
        }

        private async void btnAcceptEditMovie_click(object sender, RoutedEventArgs e)
        {
            try
            {
                textbox_errormsg.Text = "lol";
                using (HttpClient client = new HttpClient())
                {
                    bool isValidId = int.TryParse(lbl_movie_detail_id.Content.ToString(), out int movieID);
                    bool isValidYear = int.TryParse(textbox_GetMovie_releaseyear.Text, out int movieyear);

                    if (isValidId && isValidYear)
                    {
                        textbox_errormsg.Text = "updating Movie with id: " + movieID;
                        client.DefaultRequestHeaders.Accept.Clear();

                        Movie movieToUpdate = new Movie();
                        movieToUpdate.Title = textbox_GetMovie_title.Text;
                        movieToUpdate.ReleaseYear = movieyear;
                        movieToUpdate.Description = textbox_GetMovie_description.Text;

                        string jsonContent = JsonSerializer.Serialize(movieToUpdate);
                        HttpContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PutAsync(webappurl + $"api/MoviesAPI/{movieID}", httpContent);
                        if (response.IsSuccessStatusCode)
                        {
                            textbox_errormsg.Text = "Movie has been edited.";
                            textbox_GetMovie_title.Text = "";
                            textbox_GetMovie_releaseyear.Text = "";
                            textbox_GetMovie_description.Text = "";
                            btnGetAllMovies_click(null, null);


                        }
                        else if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            textbox_errormsg.Text = "Movie not found";
                        }
                        else
                        {
                            textbox_errormsg.Text = "Error updating Movie. Status code: " + response.StatusCode;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                textbox_errormsg.Text = "Error: " + ex.Message;
            }
        }
    }
}
                   