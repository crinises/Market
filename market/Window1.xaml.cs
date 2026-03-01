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

namespace market
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void btnIntrare_Click(object sender, RoutedEventArgs e)
        {
            if (rbComanda.IsChecked == true)
            {
                OrderWindow orderWin = new OrderWindow();
                orderWin.Show();
                this.Close();          
            }
            else if (rbProduse.IsChecked == true)
            {
                ProductsWindow productsWin = new ProductsWindow();
                productsWin.Show();
                this.Close();          
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
