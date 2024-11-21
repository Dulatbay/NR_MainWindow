using NR_MainWindow.Devises;
using System;
using System.IO;

namespace NR_MainWindow
{
  /*
   * Класс медиатор
   * Для работы с сохранениями, записями в файл
   * Для сохранения путей к файлам
   * Для конверта валют
   */
  public static class Global
  {
    public enum Valute
    {
      USD,
      TG,
      R
    }

    /*
     * Так как в базе данных информация указана таким образом -
     * 1) Модель комплектующего 
     * 2) Расход за час в киловаттах (6, 52 тенге/кВт.ч.)
     * 3) Доход за час в долларах 
     * Мне следует сделать конвертер для расхода и дохода отдельно
     * так как в базе информация указана таким образом (тенге, доллар),
     * а как по мне, переписывать базу данных, плохая практика.
     */
    public static Valute valuteType = Valute.USD;
    
    //  делитель (для конверта расхода)
    public static double ValueExpence 
    { get {
        switch (valuteType)
        {
          case Valute.USD:
            return 509.18;
          case Valute.TG:
            return 1;
          case Valute.R:
            return 4.87;
        }
        return 0;
      } 
    }
    
    //  делитель (для конверта дохода)
    public static double ValueIncome
    {
      get
      {
        switch (valuteType)
        {
          case Valute.USD:
            return 1;
          case Valute.TG:
            return 0.00195;
          case Valute.R:
            return 0.00949;
        }
        return 0;
      }
    }

    #region Path
    public static readonly string PrevixUrl = "DB\\";
    public static readonly string UrlCpu = "БД процессоры.txt";
    public static readonly string UrlRam = "БД ОЗУ.txt";
    public static readonly string UrlPowerBlock = "БД Блок питания.txt";
    public static readonly string UrlHDD = "БД жекстий диск.txt";
    public static readonly string UrlVideoCard = "БД видеокарты.txt";
    public static readonly string UrlValute = "Valute.txt";
    #endregion

    // Запись данных в файл
    public static void Write(Descriptions descriptions, string path)
    {
      try
      {
        using (var file = new StreamWriter(path))
        {
          foreach (var description in descriptions)
          {
            file.WriteLine(description.GetLine());
          }
        }
        LOG.ValueChanged("Запись данных в файл прошла успешно");
      }
      catch (Exception ex)
      {
        LOG.Error(ex.Message);
      }

    }

    // Инициализация данных из файла
    public static void Save(Descriptions descriptions, string path, Device device)
    {
      try
      {
       
        using (var file = new StreamReader(path))
        {
          string? line = "";
          while ((line = file.ReadLine()) != null)
          {
            IDescription l = GetDescription(device);
            l.Init(line);
            descriptions.Add(l);
          }
        }
        LOG.ValueChanged("Инициализация данных из файла прошла успешно");
      }
      catch (Exception ex)
      {
        LOG.Error(ex.Message);
      }

    }

    // вернуть экземплер в соотвествии с типом устройства
    public static IDescription GetDescription(Device device)
    {
      switch (device)
      {
        case Device.CPU: return new CPU();
        case Device.RAM: return new RAM();
        case Device.PowerBlock: return new PowerBlock();
        case Device.VideoCard: return new VideoCard();
        case Device.HDD: return new HDD();
        default: return null;
      }
    }
    public static Device GetDeviceByString(string sDevise)
    {
      switch (sDevise)
      {
        case "Жесткий диск":
          return Device.HDD;
        case "ОЗУ":
          return Device.RAM;
        case "Видеокарта":
          return Device.VideoCard;
        case "Процессор":
          return Device.CPU;
        case "Блок питания":
          return Device.PowerBlock;
      }
      throw new ArgumentException();
    }

  }
}
