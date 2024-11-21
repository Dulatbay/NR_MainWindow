using Parse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace NR_MainWindow
{
  /// <summary>
  /// Логика взаимодействия для Product.xaml
  /// </summary>
  public partial class About : Window
  {
    string url = "";
    public About(Product about)
    {
      InitializeComponent();
      tbPrice.Text = "Цена - " + about?.Price;
      tbDesc.Text = about?.Description;
      if (about?.Name is null)
      {
        tbName.Text = "НЕ НАЙДЕНО";
        LOG.ValueChanged("Неуспешный поиск");
      }
      else
      {
        tbName.Text = about?.Name;
        LOG.ValueChanged("Успешный поиск");
      }
      if (about?.Url is null)
        url = "https://shop.kz/catalog/komplektuyushchie";
      else
        url = about?.Url;
    }


    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
      Close();
    }

    private void btAbout_Click(object sender, RoutedEventArgs e)
    {

      try
      {
        System.Diagnostics.Process.Start(url);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }



    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
