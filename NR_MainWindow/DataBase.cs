using System.Reflection;

namespace NR_MainWindow
{
  /*
   *  Для взаимодействия с базой данных
   */
  static class DataBase
  {
    static public Descriptions CPUs { get; set; } = new Descriptions();
    static public Descriptions RAMs { get; set; } = new Descriptions();
    static public Descriptions PowerBlocks { get; set; } = new Descriptions();
    static public Descriptions VideoCards { get; set; } = new Descriptions();
    static public Descriptions HDDs { get; set; } = new Descriptions();

    static public void InitDataBase()
    {
      Global.Save(CPUs, Global.PrevixUrl + Global.UrlCpu, Device.CPU);
      Global.Save(RAMs, Global.PrevixUrl + Global.UrlRam, Device.RAM);
      Global.Save(PowerBlocks, Global.PrevixUrl + Global.UrlPowerBlock, Device.PowerBlock);
      Global.Save(VideoCards, Global.PrevixUrl + Global.UrlVideoCard, Device.VideoCard);
      Global.Save(HDDs, Global.PrevixUrl + Global.UrlHDD, Device.HDD);
      LOG.ValueChanged("База проинициализирована");
    }

    // Поиск
    static public IDescription GetDescription(string manufacturer, string model, Device device)
    {
      LOG.ValueChanged("Запрос на базу");
      switch (device)
      {
        case Device.CPU:
          LOG.ValueChanged("Данные получены");
          return CPUs.GetDescription(manufacturer, model);
        case Device.HDD:
          LOG.ValueChanged("Данные получены");
          return HDDs.GetDescription(manufacturer, model);
        case Device.VideoCard:
          LOG.ValueChanged("Данные получены");
          return VideoCards.GetDescription(manufacturer, model);
        case Device.PowerBlock:
          LOG.ValueChanged("Данные получены");
          return PowerBlocks.GetDescription(manufacturer, model);
        case Device.RAM:
          LOG.ValueChanged("Данные получены");
          return RAMs.GetDescription(manufacturer, model);
      }
      LOG.ValueChanged("Неуспешная попытка получиться данные из базы");
      return null;
    }


  }
}
