using System;

namespace NR_MainWindow.Devises
{
  public class CPU : IDescription
  {
    #region Fields 
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public double Income { get; set; }
    public double Expence { get; set; }
    #endregion
    public CPU(string line)
    {
      Init(line);
    }
    public CPU()
    {
    }
    public void Init(string line)
    {
      string[] lines = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
      for (int i = 0; i < lines.Length; i++)
      {
        if (i < 2)
          Manufacturer += lines[i] + " ";
        else if (i < lines.Length - 2)
          Model += lines[i] + " ";
        else if (i == lines.Length - 2)
          Expence = double.Parse(lines[i].Replace('.', ','));
        else
          Income = double.Parse(lines[i].Replace('.', ','));
      }
    }
    public override string ToString()
    {
      return $"Комлектующий - Процессор\nПроизводитель - {Manufacturer}\nМодель - {Model}";
    }
  }
}
