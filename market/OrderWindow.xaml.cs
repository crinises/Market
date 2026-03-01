using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MySql.Data.MySqlClient;

namespace market
{
    public partial class OrderWindow : Window
    {
        private Dictionary<int, List<(int id, string nume)>> _produse =
            new Dictionary<int, List<(int, string)>>();

        private int _idCatSelectata = -1;

        public OrderWindow()
        {
            InitializeComponent();
            IncarcaDateDinDB();
        }

        private void IncarcaDateDinDB()
        {
            try
            {
                using (var con = DB.GetConnection())
                {
                    con.Open();

                    cmbCategorie.Items.Clear();
                    cmbCategorie.Items.Add(new ComboBoxItem
                    { Content = "Selectați categoria", IsEnabled = false, IsSelected = true });

                    string sqlCat = "SELECT id, nume FROM cat ORDER BY nume";
                    using (var cmd = new MySqlCommand(sqlCat, con))
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            int id = r.GetInt32("id");
                            string nume = r.GetString("nume");
                            var item = new ComboBoxItem { Content = nume, Tag = id };
                            cmbCategorie.Items.Add(item);
                            _produse[id] = new List<(int, string)>();
                        }
                    }

                    //p[entru categorie
                    string sqlProd = "SELECT id, id_cat, nume FROM prod ORDER BY nume";
                    using (var cmd2 = new MySqlCommand(sqlProd, con))
                    using (var r2 = cmd2.ExecuteReader())
                    {
                        while (r2.Read())
                        {
                            int idCat = r2.GetInt32("id_cat");
                            int idProd = r2.GetInt32("id");
                            string numeProd = r2.GetString("nume");
                            if (_produse.ContainsKey(idCat))
                                _produse[idCat].Add((idProd, numeProd));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la încărcarea datelor:\n" + ex.Message,
                    "Eroare DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmbCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCategorie.SelectedItem is ComboBoxItem item && item.IsEnabled)
            {
                _idCatSelectata = (int)item.Tag;
                cmbProdus.Items.Clear();
                cmbProdus.Items.Add(new ComboBoxItem
                { Content = "Selectați produsul", IsEnabled = false, IsSelected = true });

                if (_produse.ContainsKey(_idCatSelectata))
                    foreach (var (id, nume) in _produse[_idCatSelectata])
                        cmbProdus.Items.Add(new ComboBoxItem { Content = nume, Tag = id });

                cmbProdus.IsEnabled = true;
            }
        }

        private void TxtTelefon_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"[0-9]");
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(txtNume.Text))
                errors.Add("Completati campul nume");
            if (string.IsNullOrWhiteSpace(txtPrenume.Text))
                errors.Add("Completati campul prenume!");
            if (string.IsNullOrWhiteSpace(txtTelefon.Text))
                errors.Add("Completati campul cu nr. de telefon");
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
                errors.Add("Completati camul gmail");
            if (string.IsNullOrWhiteSpace(txtCantitate.Text))
                errors.Add("Completati camupl cantitate");

            bool catOk = cmbCategorie.SelectedItem is ComboBoxItem c1 && c1.IsEnabled;
            bool prodOk = cmbProdus.SelectedItem is ComboBoxItem c2 && c2.IsEnabled;
            if (!catOk) errors.Add("Selectați o categorie.");
            if (!prodOk) errors.Add("Selectați un produs.");

            if (!string.IsNullOrWhiteSpace(txtTelefon.Text) &&
                (txtTelefon.Text.Length < 8 || !Regex.IsMatch(txtTelefon.Text, @"^\d+$")))
                errors.Add("nr de contact trebuie sa fie de minim 8 caractere");

            if (!string.IsNullOrWhiteSpace(txtEmail.Text) &&
                !Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                errors.Add("Adresa de email este invalidă");

            if (!string.IsNullOrWhiteSpace(txtCantitate.Text) &&
                (!int.TryParse(txtCantitate.Text, out int qty) || qty <= 0))
                errors.Add("Cantitatea trebuie să fie un număr pozitiv");

            feedbackBorder.Visibility = Visibility.Visible;

            if (errors.Any())
            {
                feedbackBorder.Background = new SolidColorBrush(Color.FromRgb(0xFF, 0xF0, 0xF0));
                feedbackBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x6B, 0x6B));
                feedbackBorder.BorderThickness = new Thickness(1.5);
                lblFeedback.Foreground = new SolidColorBrush(Color.FromRgb(0xC0, 0x39, 0x2B));
                lblFeedback.Text = "Vă rugăm corectați:\n\n" + string.Join("\n", errors);
                return;
            }

            int idProd = (int)((ComboBoxItem)cmbProdus.SelectedItem).Tag;
            int cantitate = int.Parse(txtCantitate.Text);

            try
            {
                using (var con = DB.GetConnection())
                {
                    con.Open();
                    string sql = @"
                        INSERT INTO comenzi (nume, prenume, telefon, email, id_prod, cantitate)
                        VALUES (@n, @p, @t, @e, @ip, @c)";

                    using (var cmd = new MySqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@n", txtNume.Text.Trim());
                        cmd.Parameters.AddWithValue("@p", txtPrenume.Text.Trim());
                        cmd.Parameters.AddWithValue("@t", txtTelefon.Text.Trim());
                        cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@ip", idProd);
                        cmd.Parameters.AddWithValue("@c", cantitate);
                        cmd.ExecuteNonQuery();
                    }
                }

                string numeProd = ((ComboBoxItem)cmbProdus.SelectedItem).Content.ToString();
                string numeCat = ((ComboBoxItem)cmbCategorie.SelectedItem).Content.ToString();

                feedbackBorder.Background = new SolidColorBrush(Color.FromRgb(0xD8, 0xF3, 0xDC));
                feedbackBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(0x52, 0xB7, 0x88));
                feedbackBorder.BorderThickness = new Thickness(1.5);
                lblFeedback.Foreground = new SolidColorBrush(Color.FromRgb(0x1B, 0x43, 0x32));
                lblFeedback.Text =
                    $"Ati realizat cu succes comanda\n\n" +
                    $"{txtNume.Text} {txtPrenume.Text}\n" +
                    $"{txtTelefon.Text}   {txtEmail.Text}\n" +
                    $"{numeProd} ({numeCat})  ×  {cantitate}\n\n" +
                    $"Multumim ";

//resetare
                txtNume.Clear(); txtPrenume.Clear();
                txtTelefon.Clear(); txtEmail.Clear(); txtCantitate.Clear();
                cmbCategorie.SelectedIndex = 0;
                cmbProdus.Items.Clear();
                cmbProdus.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare la salvarea comenzii:\n" + ex.Message,
                    "Eroare la bza datelor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            new ProductsWindow().Show();
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            new Window1().Show();
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) DragMove();
        }
    }
}