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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace market
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void TxtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9]");
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var errors = new List<string>();

            string nume = txtNume.Text.Trim();
            string prenume = txtPrenume.Text.Trim();
            string username = txtUsername.Text.Trim();
            string parola = txtParola.Password;
            string parolaR = txtParolaRep.Password;
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrEmpty(nume)) errors.Add("Numele este obligatoriu");
            if (string.IsNullOrEmpty(prenume)) errors.Add("Prenumele este obligatoriu");
            if (string.IsNullOrEmpty(username)) errors.Add("Usernameul  este obligatoriu");
            if (string.IsNullOrEmpty(parola)) errors.Add("Parola este obligatoriu");
            if (string.IsNullOrEmpty(parolaR)) errors.Add("Repetati parola");
            if (string.IsNullOrEmpty(email)) errors.Add("Emailul este obligatoriu");
            if (string.IsNullOrEmpty(phone)) errors.Add("Telefonul este obligatoriu");

            if (!string.IsNullOrEmpty(parola) && !string.IsNullOrEmpty(parolaR)
                && parola != parolaR)
                errors.Add("Parolele nu coincid.");

            if (!string.IsNullOrEmpty(email) &&
                !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.Add("Email-ul nu este valid trebuie să conțină @");

            if (!string.IsNullOrEmpty(parola) && parola.Length < 4)
                errors.Add("Parola trebuie să aibă minim 4 caractere");

            if (errors.Any())
            {
                ShowFeedback(string.Join("\n", errors), isError: true);
                return;
            }

            try
            {
                using (var con = DB.GetConnection())
                {
                    con.Open();

                    string chkUser = "SELECT COUNT(*) FROM users WHERE username = @u";
                    using (var cmd = new MySqlCommand(chkUser, con))
                    {
                        cmd.Parameters.AddWithValue("@u", username);
                        long cnt = (long)cmd.ExecuteScalar();
                        if (cnt > 0)
                        {
                            ShowFeedback("Există deja un utilizator cu acest username, incercati altul", isError: true);
                            return;
                        }
                    }

                    string chkEmail = "SELECT COUNT(*) FROM users WHERE email = @e";
                    using (var cmd2 = new MySqlCommand(chkEmail, con))
                    {
                        cmd2.Parameters.AddWithValue("@e", email);
                        long cnt2 = (long)cmd2.ExecuteScalar();
                        if (cnt2 > 0)
                        {
                            ShowFeedback("Există deja un cont cu acest email", isError: true);
                            return;
                        }
                    }

                    string ins = @"INSERT INTO users
                                   (nume, prenume, username, parola, role, email, phone)
                                   VALUES (@n, @p, @u, @pw, 'user', @e, @ph)";

                    using (var cmd3 = new MySqlCommand(ins, con))
                    {
                        cmd3.Parameters.AddWithValue("@n", nume);
                        cmd3.Parameters.AddWithValue("@p", prenume);
                        cmd3.Parameters.AddWithValue("@u", username);
                        cmd3.Parameters.AddWithValue("@pw", parola);
                        cmd3.Parameters.AddWithValue("@e", email);
                        cmd3.Parameters.AddWithValue("@ph", phone);
                        cmd3.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    $"Cont creat cu succes!\n\n" +
                    $"Welcome, {prenume} {nume}!\n" +
                    $"Poți acum să te autentifici cu:\n" +
                    $"   Utilizator: {username}",
                    "Înregistrare reușită ",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                this.Close(); 
            }
            catch (System.Exception ex)
            {
                ShowFeedback("Eroare la înregistrare:\n" + ex.Message, isError: true);
            }
        }

        private void ShowFeedback(string msg, bool isError)
        {
            fbBorder.Visibility = Visibility.Visible;

            if (isError)
            {
                fbBorder.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xF0, 0xF0));
                fbBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x6B, 0x6B));
                fbBorder.BorderThickness = new Thickness(1.5);
                lblFb.Foreground = new SolidColorBrush(Color.FromRgb(0xC0, 0x39, 0x2B));
            }
            else
            {
                fbBorder.Background = new SolidColorBrush(Color.FromRgb(0xD8, 0xF3, 0xDC));
                fbBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0x52, 0xB7, 0x88));
                fbBorder.BorderThickness = new Thickness(1.5);
                lblFb.Foreground = new SolidColorBrush(Color.FromRgb(0x1B, 0x43, 0x32));
            }

            lblFb.Text = msg;
        }

        private void LnkBack_Click(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) DragMove();
        }
    }
}