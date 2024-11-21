namespace NR_MainWindow
{
  /* 
   * Описывает комплектующий 
   * и на основе этого интерфейса будут создаваться другие устройства
   * Так как в базе данных сохраненные устройства требуют различные алгоритмы для парса
   * Каждый комплетующий должен иметь свою реализацию
   */

  public interface IDescription
  {
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public double Income { get; set; }
    public double Expence { get; set; }
    public string GetLine() => Manufacturer.Trim() + " " + Model.Trim() + "," + Expence.ToString().Replace(',', '.') + "," + Income.ToString().Replace(',', '.');
    public void Init(string line);
  }
}
