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

using System.Windows;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace market
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void ChkVizitator_Changed(object sender, RoutedEventArgs e)
        {
            bool vizitator = chkVizitator.IsChecked == true;
            txtUser.IsEnabled = !vizitator;
            txtPass.IsEnabled = !vizitator;
            errBorder.Visibility = Visibility.Collapsed;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (chkVizitator.IsChecked == true)
            {
                new Window1().Show();
                this.Close();
                return;
            }

            

            string user = txtUser.Text.Trim();
            string pass = txtPass.Password.Trim();

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                ShowError("Completează utilizatorul și parola.");
                return;
            }

            try
            {
                using (var con = DB.GetConnection())
                {
                    con.Open();
                    string sql = "SELECT id, nume, prenume, role FROM users " +
                                 "WHERE username = @u AND parola = @p LIMIT 1";

                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@u", user);
                        cmd.Parameters.AddWithValue("@p", pass);

                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                errBorder.Visibility = Visibility.Collapsed;
                                new Window1().Show();
                                this.Close();
                            }
                            else
                            {
                                r.Close();
                                cmd.Parameters.Clear();
                                cmd.CommandText = "SELECT id FROM users WHERE username = @u";
                                cmd.Parameters.AddWithValue("@u", user);
                                var exists = cmd.ExecuteScalar();

                                ShowError(exists != null
                                    ? "Parolă incorectă. Încearcă din nou."
                                    : "Utilizatorul nu există.");
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                ShowError("Eroare conexiune: " + ex.Message);
            }
        }

        private void LnkRegister_Click(object sender, MouseButtonEventArgs e)
        {
            new RegisterWindow().ShowDialog();
        }

        private void ShowError(string msg)
        {
            lblErr.Text = msg;
            errBorder.Visibility = Visibility.Visible;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) DragMove();
        }
    }
}