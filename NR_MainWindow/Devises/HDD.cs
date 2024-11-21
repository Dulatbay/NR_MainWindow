using System;
using System.Windows;

namespace NR_MainWindow.Devises
{
  public class HDD : IDescription
  {
    #region Fields 
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public double Income { get; set; }
    public double Expence { get; set; }
    #endregion
    public HDD(string line)
    {
      Init(line);
    }
    public HDD()
    {
    }
    public void Init(string line)
    {
      string[] lines = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
      for (int i = 0; i < lines.Length; i++)
      {
        if (i < 3)
          Model += lines[i] + " ";
        else if (i < lines.Length - 2)
          Manufacturer += lines[i] + " ";
        else if (i == lines.Length - 2)
          Expence = double.Parse(lines[i].Replace('.', ','));
        else
          Income = double.Parse(lines[i].Replace('.', ','));
      }

    }
    public override string ToString()
    {
      return $"Комлектующий - Жесткий диск\nПроизводитель - {Manufacturer}\nМодель - {Model}";
    }
    public string GetLine()
    {
      return Model.Trim() + " " + Manufacturer.Trim() + "," + Expence.ToString().Replace(',', '.') + "," + Income.ToString().Replace(',', '.');
    }

  }
}
