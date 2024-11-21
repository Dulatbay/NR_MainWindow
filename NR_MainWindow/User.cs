using NR_MainWindow.Devises;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NR_MainWindow
{
  /* 
   * Класс синглтон
   */
  class User
  {
    private static User? _instance;

    private User() { this.Save(); }

    public static User GetInstance()
    {
      if (_instance == null)
      {
        LOG.ValueChanged("Пользователь создан");
        _instance = new User();
      }
      return _instance;
    }
    public Descriptions CPUs { get; set; } = new Descriptions();
    public Descriptions RAMs { get; set; } = new Descriptions();
    public Descriptions PowerBlocks { get; set; } = new Descriptions();
    public Descriptions VideoCards { get; set; } = new Descriptions();
    public Descriptions HDDs { get; set; } = new Descriptions();


    // Записать данные в файл
    public void Write()
    {
      try
      {
        Global.Write(CPUs, Global.UrlCpu);
        Global.Write(RAMs, Global.UrlRam);
        Global.Write(PowerBlocks, Global.UrlPowerBlock);
        Global.Write(VideoCards, Global.UrlVideoCard);
        Global.Write(HDDs, Global.UrlHDD);
        LOG.ValueChanged("Пользователь успешно записал данные в файл");
      }
      catch (Exception)
      {
        LOG.Error("Ошибка записи данных в файл пользователя");
      }


    }

    // Записать данные из файла
    public void Save()
    {
      try
      {
        Global.Save(CPUs, Global.UrlCpu, Device.CPU);
        Global.Save(RAMs, Global.UrlRam, Device.RAM);
        Global.Save(PowerBlocks, Global.UrlPowerBlock, Device.PowerBlock);
        Global.Save(VideoCards, Global.UrlVideoCard, Device.VideoCard);
        Global.Save(HDDs, Global.UrlHDD, Device.HDD);
        LOG.ValueChanged("Пользователь успешно проинициализировал данные");
      }
      catch (Exception)
      {
        LOG.Error("Ошибка инициализации данных из файла пользователя");
      }

    }


    // Добавить устройство
    public void Add(Device device, IDescription description)
    {
      switch (device)
      {
        case Device.CPU:
          CPUs.Add(description);
          break;
        case Device.HDD:
          HDDs.Add(description);
          break;
        case Device.VideoCard:
          VideoCards.Add(description);
          break;
        case Device.PowerBlock:
          PowerBlocks.Add(description);
          break;
        case Device.RAM:
          RAMs.Add(description);
          break;
      }
      LOG.ValueChanged("Пользователь успешно добавил данные");
    }

    public bool Delete(Device device, IDescription description)
    {
      bool res = false;
      switch (device)
      {
        case Device.CPU:
          res = CPUs.Remove(description);
          break;
        case Device.HDD:
          res = HDDs.Remove(description);
          break;
        case Device.VideoCard:
          res = VideoCards.Remove(description);
          break;
        case Device.PowerBlock:
          res = PowerBlocks.Remove(description);
          break;
        case Device.RAM:
          res = RAMs.Remove(description);
          break;
      }
      if (res)
        LOG.ValueChanged("Пользователь успешно удалил данные");
      else
        LOG.ValueChanged("Неудачная попытка удалить данные пользователя");
        return res;
    }
  }
}
