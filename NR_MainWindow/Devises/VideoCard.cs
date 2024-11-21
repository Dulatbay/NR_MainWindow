using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NR_MainWindow.Devises
{
  public class VideoCard : IDescription
  {
    #region Fields 
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public double Income { get; set; }
    public double Expence { get; set; }
    #endregion
    public VideoCard(string line)
    {
      Init(line);
    }
    public VideoCard()
    {
    }
    public void Init(string line)
    {
      string[] lines = line.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
      for (int i = 0; i < lines.Length; i++)
      {
        if (i < 1)
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
      return $"Комлектующий - Видеокарта\nПроизводитель - {Manufacturer}\nМодель - {Model}";
    }
  }
}
