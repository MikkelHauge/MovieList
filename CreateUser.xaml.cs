using BusinessLogicLayer.Model;
using BusinessLogicLayer.Repositories;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text.RegularExpressions;
using System.Windows;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Input;
using Context = DataAccessLayer.Context.Context;

namespace MovieList
{
    /// <summary>
    /// Interaction logic for CreateUser.xaml
    /// </summary>
    public partial class CreateUser : Window
    {
        private static MainWindow mainWindow;
        public static User createdUser;
        private static Context context = new Context();
        private static UserRepository userRepo = new UserRepository(context);

        public CreateUser(MainWindow mainWindowIncoming)
        {

            InitializeComponent();

            mainWindow = mainWindowIncoming;

        }


        private void button_createactualuser_click(object sender, RoutedEventArgs e)
        {
            // tjek om navne-feltet er tomt (uændret navn)
            if (txtbox_newUserName.Text.Length == 0)
            {
                
                MessageBox.Show("Navnet er tomt.\nBrugernavnet må ikke være tomt. Fx. 'Torben Krøjmand'.\nMå kun indeholde tal og bogstaver.\nSamt det ene mellemrum.\"", "👎", MessageBoxButton.OK, MessageBoxImage.Information);
                int index = mainWindow.UserListBox.Items.IndexOf(createdUser);
                mainWindow.UserListBox.SelectedIndex = index;
                this.Close();
                mainWindow.RefreshData();

            }
            else
            {
                // check for dublet!
                var newuser = userRepo.GetByName(txtbox_newUserName.Text);

                if (newuser != null)
                {
                    MessageBox.Show("Brugernavnet er allerede i brug. Vælg et andet.🤷‍♂️\nNavne-tjek er ikke case-sensitive. 'Jens' og 'jens' er det samme.", "👎", MessageBoxButton.OK, MessageBoxImage.Error);

                }
                else
                {
                    try
                    {
                        // Opret User.
                        createdUser = new User();
                        createdUser.Name = txtbox_newUserName.Text;
                        userRepo.Add(createdUser);
                        context.SaveChanges();

                        MessageBox.Show("Brugeren " + createdUser.Name + "blev oprettet.\nHusk at tilføj film til din bruger, inde i 'Edit User'.", "👍", MessageBoxButton.OK, MessageBoxImage.Information);


                        int index = mainWindow.UserListBox.Items.IndexOf(createdUser);
                        mainWindow.UserListBox.SelectedIndex = index;
                        this.Close();
                        mainWindow.RefreshData();
                    } catch {
                        MessageBox.Show("Brugeren " + createdUser.Name + "blev ikke oprettet.\nDer er sket en fejl.", "👎", MessageBoxButton.OK, MessageBoxImage.Error);

                    }



                }
            }
        }
    }
}
