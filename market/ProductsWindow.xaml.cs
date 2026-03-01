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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;

namespace market
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Price { get; set; }
        public int Stock { get; set; }
        public string Unit { get; set; }
    }

    public partial class ProductsWindow : Window
    {
        private List<Product> _allProducts;

        public ProductsWindow()
        {
            InitializeComponent();
            LoadProducts();
            dgProducts.ItemsSource = _allProducts;
        }

        private void LoadProducts()
        {
            _allProducts = new List<Product>();

            try
            {
                using (var con = DB.GetConnection())
                {
                    con.Open();
                    string sql = @"
                        SELECT p.id, p.nume, c.nume AS categorie,
                               p.pret, p.stoc, p.um
                        FROM prod p
                        INNER JOIN cat c ON p.id_cat = c.id
                        ORDER BY c.nume, p.nume";

                    using (var cmd = new MySqlCommand(sql, con))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _allProducts.Add(new Product
                            {
                                Id = reader.GetInt32("id"),
                                Name = reader.GetString("nume"),
                                Category = reader.GetString("categorie"),
                                Price = reader.GetDecimal("pret").ToString("F2"),
                                Stock = reader.GetInt32("stoc"),
                                Unit = reader.GetString("um")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eroare conexiune DB:\n" + ex.Message,
                    "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuFilter_Click(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            if (mi == null) return;

            string tag = mi.Tag?.ToString() ?? "Toate";
            lblCurrentFilter.Text = tag == "Toate" ? "Toate produsele" : tag;

            dgProducts.ItemsSource = tag == "Toate"
                ? _allProducts
                : _allProducts.Where(p => p.Category == tag).ToList();
        }

        private void MenuDelivery_Click(object sender, RoutedEventArgs e)
        {
            string msg =
                "Conditii de livrare, merge lucrul aici\n\n" +
                "Orar livrări:\n" +
                "     Luni – Vineri:   09:00 – 20:00\n" +
                "Cost livrare:\n" +
                "      Comenzi sub 100 lei  →  30 lei\n" +
                "     Comenzi 100–300 lei  →  20 lei\n" +
                "     Comenzi peste 300 lei →  FREE\n\n" +
               
                "Livram doar in Chisinau";

            MessageBox.Show(msg, "Condiții de livrare",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnOpenOrder_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow().Show();
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