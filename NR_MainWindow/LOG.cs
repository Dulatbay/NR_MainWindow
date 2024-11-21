using NLog;
using System.IO;

namespace NR_MainWindow
{
  /*
   * Класс для логирования
   * Trace - обычные логи, по типу как: X метод начал работу, Y метод завершил работу
   * Debug - более серьезные логи, по типу как: User обновил свои данные
   * Error - исключения и ошибки
   */
  public static class LOG
  {
    private static Logger Logger = LogManager.GetCurrentClassLogger();
    static  LOG() { 
      File.Delete("file.txt");
    }
    public static void StartMethodTrace(string message)
    {
      Logger.Trace(message + " начала работу");
    }
    public static void EndMethodTrace(string message)
    {
      Logger.Trace(message + " завершила работу");
    }
    public static void WrongMethodTrace(string message)
    {
      Logger.Trace(message + " выдала ошибку");
    }

    public static void ValueChanged(string message)
    {
      Logger.Debug(message);
    }
    public static void Error(string message)
    {
      Logger.Error(message);
    }


  }
}
