using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Parse;

namespace NR_MainWindow
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    User User { get; set; }

    Device device = Device.CPU;
    public MainWindow()
    {
      InitializeComponent();
      
    
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      LOG.StartMethodTrace("Window_Loaded()");
      DataBase.InitDataBase();
      User = User.GetInstance();
      InitComboBoxManufacturer();
      if (!File.Exists("user.txt"))
        new Task(InitPopups).Start();
      InitExpanderCard();
      LOG.EndMethodTrace("Window_Loaded()");
    
    }
    private void SetResult()
    {
      LOG.StartMethodTrace("SetResult()");
      var description = DataBase.GetDescription(((string)cbManufacturer.SelectedItem), (string)cbModel.SelectedItem, device);

      if (description == null) return;

      var expence = Math.Round((
        (description.Expence * numericUpDown.Val)
        / Global.ValueExpence)
        * timeSpan.TotalHours, 1);
      tbTotalExpence.Text = expence.ToString();


      var income = Math.Round((
        ((description.Income * numericUpDown.Val))
        * timeSpan.TotalHours) / Global.ValueIncome, 1);
      tbtotaIncome.Text = income.ToString();

      InitUserTB();
      LOG.EndMethodTrace("SetResult()");
    }


    private void InitUserTB()
    {
      LOG.StartMethodTrace("SetResult()");
      double summExpence = 0;
      double summIncome = 0;
      tbTotalUserExpence.Text = "0";
      tbTotalUserIncome.Text = "0";

      void AddSumm(ICollection<IDescription> descriptions)
      {
        if (descriptions.Count != 0)
        {
          foreach (var i in descriptions)
          {
            summIncome += i.Income;
            summExpence += i.Expence;
          }
        }
      }

      AddSumm(User.CPUs);
      AddSumm(User.HDDs);
      AddSumm(User.PowerBlocks);
      AddSumm(User.RAMs);
      AddSumm(User.VideoCards);
      tbTotalUserExpence.Text = Math.Round((summExpence * timeSpan.TotalHours) / Global.ValueExpence, 1).ToString();
      tbTotalUserIncome.Text = Math.Round((summIncome * timeSpan.TotalHours) / Global.ValueIncome, 1).ToString();
      LOG.EndMethodTrace("SetResult()");
    }
    #region DefaultEvents
    private void HideButton_Click(object sender, RoutedEventArgs e)
    {
      if (this.WindowState == WindowState.Normal || this.WindowState == WindowState.Minimized)
        this.WindowState = WindowState.Minimized;
      else
        this.WindowState = WindowState.Normal;
      LOG.ValueChanged("Размер окна изменен");
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
      User.Write();
      LOG.ValueChanged("Закрытие окна...");
      Close();
    }
    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

    #endregion

    #region EventsComboBoxes
    private void InitComboBox(List<string> items, ComboBox comboBox)
    {
      LOG.StartMethodTrace("InitComboBox()");
      try
      {
        comboBox?.Items.Clear();
        foreach (var item in items)
        {
          comboBox?.Items.Add(new ComboBoxItem().Content = item);
        }
        if (comboBox != null) comboBox.SelectedIndex = 0;
        LOG.EndMethodTrace("InitComboBox()");
      }
      catch (Exception ex)
      {
        LOG.WrongMethodTrace("InitComboBox()");
        LOG.Error(ex.Message);
      }

    }
    private void InitComboBoxManufacturer()
    {
      try
      {
        LOG.StartMethodTrace("InitComboBoxManufacturer()");
        switch (device)
        {
          case Device.CPU:
            InitComboBox(DataBase.CPUs.GetManufacturers(), cbManufacturer);
            break;
          case Device.HDD:
            InitComboBox(DataBase.HDDs.GetManufacturers(), cbManufacturer);
            break;
          case Device.VideoCard:
            InitComboBox(DataBase.VideoCards.GetManufacturers(), cbManufacturer);
            break;
          case Device.PowerBlock:
            InitComboBox(DataBase.PowerBlocks.GetManufacturers(), cbManufacturer);
            break;
          case Device.RAM:
            InitComboBox(DataBase.RAMs.GetManufacturers(), cbManufacturer);
            break;
        }
        if (cbManufacturer is not null) cbManufacturer.SelectedIndex = 0;
        LOG.EndMethodTrace("InitComboBoxManufacturer()");
      }
      catch (Exception ex)
      {
        LOG.WrongMethodTrace("InitComboBoxManufacturer()");
        LOG.Error(ex.Message);
      }

    }

    private void cbDevice_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      try
      {
        LOG.StartMethodTrace("cbDevice_SelectionChanged()");
        var comboBoxItem = ((ComboBoxItem)cbDevice.SelectedItem);
        string? str = comboBoxItem.Content?.ToString();
        switch (str)
        {
          case "CPU":
            device = Device.CPU;
            break;
          case "HDD":
            device = Device.HDD;
            break;
          case "VideoCard":
            device = Device.VideoCard;
            break;
          case "PowerBlock":
            device = Device.PowerBlock;
            break;
          case "RAM":
            device = Device.RAM;
            break;
          default:
            device = Device.CPU;
            break;
        }
        InitComboBoxManufacturer();
        LOG.EndMethodTrace("cbDevice_SelectionChanged()");
      }
      catch (Exception ex)
      {
        LOG.WrongMethodTrace("cbDevice_SelectionChanged()");
        LOG.Error(ex.Message);
      }

    }

    private void cbManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LOG.StartMethodTrace("cbManufacturer_SelectionChanged()");
      if (cbManufacturer.SelectedIndex != -1)
        try
        {
          string str = (string)cbManufacturer.SelectedItem;
          switch (device)
          {
            case Device.CPU:
              InitComboBox(DataBase.CPUs.GetModels(str), cbModel);
              break;
            case Device.HDD:
              InitComboBox(DataBase.HDDs.GetModels(str), cbModel);
              break;
            case Device.VideoCard:
              InitComboBox(DataBase.VideoCards.GetModels(str), cbModel);
              break;
            case Device.PowerBlock:
              InitComboBox(DataBase.PowerBlocks.GetModels(str), cbModel);
              break;
            case Device.RAM:
              InitComboBox(DataBase.RAMs.GetModels(str), cbModel);
              break;
            default:
              break;
          }
        }
        catch (Exception ex)
        {
          LOG.WrongMethodTrace("cbManufacturer_SelectionChanged()");
          LOG.Error(ex.Message);
          return;
        }
      LOG.EndMethodTrace("cbManufacturer_SelectionChanged()");
    }

    private void cbModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      LOG.StartMethodTrace("cbModel_SelectionChanged()");
      try
      {
        SetResult();
        LOG.EndMethodTrace("cbModel_SelectionChanged()");
      }
      catch (Exception ex)
      {
        LOG.WrongMethodTrace("cbModel_SelectionChanged()");
        LOG.Error(ex.Message);
      }
      finally
      {
        numericUpDown.Val = 1;
      }
    }
    #endregion

    #region PeriodButtonsEvent

    // текущий период времени
    // 0 = день
    // 1 = неделя
    // 2 = месяц
    // 3 = год
    private int _currentPeriodIndex = 0;
    TimeSpan timeSpan = TimeSpan.FromDays(1);
    private int GetSelectIndex(object s)
    {
      var tempGrid = s as Grid;
      if (tempGrid != null)
      {
        return Grid.GetColumn(tempGrid);
      }
      var temp = s as TextBlock;
      return Grid.GetColumn(temp);
    }

    private void PeriodTextBlock_MouseLeave(object sender, MouseEventArgs e)
    {
      Grid.SetColumn(borderPeriodIndex, _currentPeriodIndex);
    }

    private void PeriodTextBlock_MouseEnter(object sender, MouseEventArgs e)
    {
      Grid.SetColumn(borderPeriodIndex, GetSelectIndex(sender));
    }
    private void PeriodTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
      _currentPeriodIndex = GetSelectIndex(sender);
      switch (_currentPeriodIndex)
      {
        case 0:
          timeSpan = TimeSpan.FromDays(1);
          break;
        case 1:
          timeSpan = TimeSpan.FromDays(7);
          break;
        case 2:
          timeSpan = TimeSpan.FromDays(31);
          break;
        case 3:
          timeSpan = TimeSpan.FromDays(365);
          break;
        default:
          break;
      }
      Grid.SetColumn(borderPeriodIndex, _currentPeriodIndex);
      SetResult();
      LOG.ValueChanged("Период показа времени изменен");
    }


    private void numericUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
    {
      try
      {
        SetResult();
      }
      catch (Exception ex)
      {
        LOG.Error(ex.Message);
      }

    }
    #endregion

    #region ToolsButtonEvents
    private void AddButton_Click(object sender, RoutedEventArgs e)
    {


      LOG.StartMethodTrace("AddButton_Click()");
      var description = DataBase.GetDescription((string)cbManufacturer.SelectedItem, (string)cbModel.SelectedItem, device);// получаем из базы модель передавая параметры
      LOG.ValueChanged("Данные из базы получены");
      // добавляем в user и сохраняем
      for (int i = 0; i < numericUpDown.Val; i++)
      {
        User.Add(device, description);
      }
      User.Write();

      InitExpanderCard();
      // возвращаем обычные настройки
      numericUpDown.Val = 1;
      // Настраиваем под валюту
      SetResult();
      LOG.EndMethodTrace("AddButton_Click()");
    }




    private void CreateBorderDescription(IDescription description, int num)
    {
      LOG.StartMethodTrace("CreateBorderDescription()");
      TextBox textBlock = new TextBox();
      textBlock.BorderThickness = new Thickness(0);
      textBlock.IsReadOnly = true;
      textBlock.FontSize = 13;
      textBlock.TextWrapping = TextWrapping.Wrap;
      textBlock.Text = description.ToString() + "\nКоличество - " + num.ToString() + " штук";
      textBlock.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock.Margin = new Thickness(10, 0, 0, 0);


      Button button = new Button();
      button.Width = 50;
      button.Height = 50;
      button.Background = new SolidColorBrush(Colors.Transparent);
      button.Content = "X";
      button.Click += DeleteButton_Click;
      button.HorizontalAlignment = HorizontalAlignment.Right;
      button.Margin = new Thickness(0, 0, 10, 0);

      Grid grid = new Grid();
      grid.Children.Add(textBlock);
      grid.Children.Add(button);

      Border border = new Border();
      border.CornerRadius = new CornerRadius(10);
      border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#867BFF"));
      border.Margin = new Thickness(0, 10, 0, 10);
      border.Child = grid;
      spBox.Children.Add(border);
      LOG.EndMethodTrace("CreateBorderDescription()");
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
      Grid grid = (Grid)((Button)e.OriginalSource).Parent;
      TextBox textBox = (TextBox)grid.Children[0];
      (IDescription description, Device dev) = ParseValueInTextBox(textBox);
      User.Delete(dev, description).ToString();
      InitExpanderCard();
    }

    // Из textbox возвращаю данные(для удаления)
    private (IDescription, Device) ParseValueInTextBox(TextBox textBox)
    {
      string GetNeedValue(string line)
      {
        string res = "";
        var firstLine = line.Split();
        for (int i = 2; i < firstLine.Length; i++)
        {
          res += firstLine[i] + " ";
        }
        return res.Trim();
      }


      var lines = textBox.Text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
      // парс комлектующего
      string sDevise = GetNeedValue(lines[0]);
      Device dev = Global.GetDeviceByString(sDevise);
      string manc = GetNeedValue(lines[1]);
      string model = GetNeedValue(lines[2]);
      var result = DataBase.GetDescription(manc, model, dev);
      return (result, dev);
    }


    /*
     * Так как данные в базе данных не соотвествуют тому, что лежить на php файле
     * я предпочел сделать для каждого комплетующего отдельный запрос на поиск
     * Некоторые девайсы могут не находиться, парсер требует дороботок
     */
    private void AboutButton_Click(object sender, RoutedEventArgs e)
    {
      // come back later
      string search = "";
      switch (device)
      {
        case Device.CPU:
          string manc = ((string)cbManufacturer.SelectedItem);
          manc = manc.Trim();
          if (manc.Split()[0] == "Intel")
            manc = manc.Replace("CPU", "Core");
          else
            manc = manc.Replace("CPU", "");
          search = (manc.Trim() + " " + ((string)cbModel.SelectedItem).Trim().Replace('-', ' '));
          break;
        case Device.HDD:
          search = ((string)cbModel.SelectedItem + (string)cbManufacturer.SelectedItem).Trim();
          break;
        case Device.VideoCard:
          if ((string)cbModel.SelectedItem != "AMD")
            search = (string)cbModel.SelectedItem + " " + (string)cbManufacturer.SelectedItem;
          else search = "";
          break;
        case Device.PowerBlock:
          search = (string)cbModel.SelectedItem + " " + (string)cbManufacturer.SelectedItem;
          break;
        case Device.RAM:
          search = (string)cbModel.SelectedItem + " " + (string)cbManufacturer.SelectedItem;
          break;
      }
      Thread t = new Thread(() =>
      {
        bool isFinish = false;
        new Task(() =>
        {
          int i = 0;
          while (true)
          {
            string temp = "Подробнее";
            if (i++ == 0)
              temp = "Подробнее.";
            else if (i == 1)
              temp = "Подробнее..";
            else if (i == 2)
              temp = "Подробнее...";
            else i = 0;
            Thread.Sleep(500);
            Dispatcher.Invoke(() =>
            {
              if (isFinish)
              {
                return;
              }
              btAbout.Content = temp;
            });
          }
        }).Start();
        var res = Request.Get(search, device.ToString());
        isFinish = true;
        Dispatcher.Invoke(() =>
        {
          new About(res).ShowDialog();
          btAbout.Content = "Подробнее";
        });

      });
      t.Start();
      LOG.ValueChanged($"Поток для поиска в файле - {t?.ManagedThreadId}");
      LOG.ValueChanged($"Новый поток для поиска в файле завершился - {t?.ManagedThreadId}");
    }


    #endregion

    #region Popup
    private void ShowPopup(Control uIElement, string message, out string predBursh)
    {
      string? temp = "";
      Dispatcher.Invoke(() =>
      {
        mainPopup.Placement = PlacementMode.Left;
        mainPopup.PlacementTarget = uIElement;
        temp = uIElement?.Background?.ToString();
        uIElement.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#29EE78"));
        tbPopup.Text = message;
        mainPopup.IsOpen = true;
      });
      predBursh = temp;
    }

    private void ShowPopup(Panel uIElement, string message, out string predBursh)
    {
      string temp = "";
      Dispatcher.Invoke(() =>
      {
        mainPopup.Placement = PlacementMode.Left;
        mainPopup.PlacementTarget = uIElement;
        temp = uIElement.Background.ToString();
        uIElement.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#29EE78"));
        tbPopup.Text = message;
        mainPopup.IsOpen = true;
      });
      predBursh = temp;
    }
    private void ShowPopup(Border uIElement, string message, out string predBursh)
    {
      string temp = "";
      Dispatcher.Invoke(() =>
      {
        mainPopup.Placement = PlacementMode.Left;
        mainPopup.PlacementTarget = uIElement;
        temp = uIElement.Background.ToString();
        uIElement.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#29EE78"));
        tbPopup.Text = message;
        mainPopup.IsOpen = true;
      });
      predBursh = temp;
    }
    private void InitPopups()
    {
      LOG.StartMethodTrace("InitPopups()");
      string predBursh = string.Empty;
      ShowPopup(btValute, "Здесь вы можете менять валюту", out predBursh);
      ShowPopup(predBursh);

      ShowPopup(gridTotalResult, "Здесь отображается общий баланс", out predBursh);
      ShowPopup(predBursh);

      ShowPopup(spComboxses, "Здесь вы можете выбирать комплектующий с нужным производителем и моделью", out predBursh);
      ShowPopup(predBursh);

      ShowPopup(numericUpDown, "Здесь вы можете изменять количество нужного комлектуюущего", out predBursh);
      ShowPopup(predBursh);

      ShowPopup(borderPeriodIndex, "Здесь вы выбираете период времени для подсчета расхода/дохода", out predBursh);
      ShowPopup(predBursh);

      ShowPopup(btAbout, "Найти информацию подробнее о комплектующем", out predBursh);
      ShowPopup(predBursh);

      ShowPopup(btAdd, "Добавить комплектующий в корзину", out predBursh);
      ShowPopup(predBursh);

      ShowPopup(exBox, "Здесь хранятся ваши добавленные товары", out predBursh);
      ShowPopup(predBursh);
      LOG.EndMethodTrace("InitPopups()");
      File.Create("user.txt");
    }
    private void ShowPopup(string brush)
    {
      bool isOpen = true;
      DateTime dateTime = DateTime.Now;
      while (isOpen)
      {
        if (DateTime.Now.Subtract(dateTime).TotalSeconds > 5)
        {
          Dispatcher.Invoke(() => { mainPopup.IsOpen = false; });
          break;
        }
        Thread.Sleep(500);
        Dispatcher.Invoke(() => { isOpen = mainPopup.IsOpen; });
      }
      Dispatcher.Invoke(() =>
      {
        try
        {
          if (mainPopup.PlacementTarget as Panel != null)
            (mainPopup.PlacementTarget as Panel).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(brush));
          else if (mainPopup.PlacementTarget as Control != null)
          {
            if (brush == null)
            {
              (mainPopup.PlacementTarget as Control).Background = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
              (mainPopup.PlacementTarget as Control).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(brush));
            }
          }
          else (mainPopup.PlacementTarget as Border).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(brush));
        }
        catch (Exception)
        {

        }
      });
    }
    #endregion

    #region ResizeWindow
    bool ResizeInProcess = false;
    private void Resize_Init(object sender, MouseButtonEventArgs e)
    {
      Rectangle? senderRect = sender as Rectangle;
      if (senderRect != null)
      {
        ResizeInProcess = true;
        senderRect.CaptureMouse();
      }
    }

    private void Resize_End(object sender, MouseButtonEventArgs e)
    {
      Rectangle? senderRect = sender as Rectangle;
      if (senderRect != null)
      {
        ResizeInProcess = false; ;
        senderRect.ReleaseMouseCapture();
      }
    }

    private void Resizeing_Form(object sender, MouseEventArgs e)
    {
      if (ResizeInProcess)
      {
        Rectangle? senderRect = sender as Rectangle;
        Window? mainWindow = senderRect.Tag as Window;
        if (senderRect != null)
        {
          double width = e.GetPosition(mainWindow).X;
          double height = e.GetPosition(mainWindow).Y;
          senderRect.CaptureMouse();
          if (senderRect.Name.ToLower().Contains("right"))
          {
            width += 5;
            if (width > 0)
              mainWindow.Width = width;
          }
          if (senderRect.Name.ToLower().Contains("left"))
          {
            File.AppendAllText("ss.txt", Width.ToString() + " | " + width.ToString() + System.Environment.NewLine);
            width -= 5;
            if (Width > this.MinWidth)
              mainWindow.Left += width;
            width = mainWindow.Width - width;
            if (width > 0)
              mainWindow.Width = width;
          }
          if (senderRect.Name.ToLower().Contains("bottom"))
          {
            height += 5;
            if (height > 0)
              mainWindow.Height = height;
          }
          if (senderRect.Name.ToLower().Contains("top"))
          {
            height -= 5;
            if (Height > this.MinHeight)
              mainWindow.Top += height;
            height = mainWindow.Height - height;
            if (height > 0)
            {
              mainWindow.Height = height;
            }
          }
        }
      }
    }





    #endregion

    private void InitExpanderCard()
    {
      spBox.Children.Clear();
      void init(Descriptions descriptions)
      {
        Descriptions uniq = new Descriptions();
        foreach (var i in descriptions)
        {
          if (!uniq.Contains(i))
          {
            CreateBorderDescription(i, descriptions.GetCopyCount(i));
            uniq.Add(i);
          }
        }
      }
      init(User.HDDs);
      init(User.CPUs);
      init(User.PowerBlocks);
      init(User.RAMs);
      init(User.VideoCards);
      LOG.ValueChanged("Корзина проинициализирована");
      SetResult();
    }

    private void ValuteChangeButton_Click(object sender, RoutedEventArgs e)
    {
      if (Global.valuteType != (Global.Valute)2)
        Global.valuteType++;
      else Global.valuteType = 0;

      switch (Global.valuteType)
      {
        case Global.Valute.USD:
          btValute.Content = "$";
          tbValute.Text = tbValute.Text.Replace("₽", "$");
          MaterialDesignThemes.Wpf.TextFieldAssist.SetSuffixText(tbtotaIncome, "$");
          break;
        case Global.Valute.TG:
          btValute.Content = "₸";
          tbValute.Text = tbValute.Text.Replace("$", "₸");
          MaterialDesignThemes.Wpf.TextFieldAssist.SetSuffixText(tbtotaIncome, "₸");
          break;
        case Global.Valute.R:
          btValute.Content = "₽";
          tbValute.Text = tbValute.Text.Replace("₸", "₽");
          MaterialDesignThemes.Wpf.TextFieldAssist.SetSuffixText(tbtotaIncome, "₽");
          break;
      }
      LOG.ValueChanged("Валюта изменена");
      SetResult();
    }
  }
}
