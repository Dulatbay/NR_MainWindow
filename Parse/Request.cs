using System;
using System.IO;
using System.Net;
using System.Text;

namespace Parse
{
  public static class Request
  {
    private static string URL = "https://shop.kz/bitrix/catalog_export/yandex.php";

    static Categories categories;
    static string END = "210";
    public static Product Get(string name, string category)
    {
      switch (category)
      {
        case "CPU":
          categories = Categories.CPU;
          break;
        case "RAM":
          categories = Categories.RAM;
          break;
        case "VideoCards":
          categories = Categories.VideoCards;
          break;
        case "HDD":
          categories = Categories.HDD;
          break;
        case "PowerBlock":
          categories = Categories.PowerBlock;
          break;
      }
      File.AppendAllText("loggg.txt", categories.ToString());
      System.Text.Encoding.RegisterProvider(
    System.Text.CodePagesEncodingProvider.Instance);
      Encoding srcEncoding = Encoding.GetEncoding(1251);
      HttpWebRequest request = WebRequest.Create(URL) as HttpWebRequest;
      if (request != null)
      {
        request.Method = "GET";
        HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        StreamReader reader = new StreamReader(response.GetResponseStream(), srcEncoding);
        {
          string line;
          Product about = new Product();
          bool isNew = false;
          bool isExit = false;
          string search = name;
          string id = ((int)categories).ToString();
          File.Delete("file1.txt");
          File.Delete("file.txt");
          File.AppendAllText("res.txt", "name - " + search + Environment.NewLine);
          while ((line = reader.ReadLine()) != null)
          {
            string[] val = line.Split(new char[] { '<', '>', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (val.Length == 0)
            {
              continue;
            }
            if (line.Contains("<offer"))
            {
              about = new Product();
              isNew = true;
            }
            if (isNew)
            {

              if (val[0] == "url")
              {
                about.Url = val[1];
              }
              if (val[0] == ("price"))
              {
                about.Price = val[1];

              }
              if (val[0] == ("name"))
              {
                if (val[1].Contains(search))
                {
                  about.Name = val[1];
                  isExit = true;
                }
                else
                {
                  isNew = false;
                  continue;
                }
              }
              if (val[0] == ("description"))
              {
                about.Description = val[1];
                File.AppendAllText("file1.txt", "это description\n");
                if (isExit)
                {
                  File.AppendAllText("res.txt", "name - " + about.Name + Environment.NewLine);
                  File.AppendAllText("res.txt", "description - " + about.Description + Environment.NewLine);
                  File.AppendAllText("res.txt", "url - " + about.Url + Environment.NewLine);
                  return about;
                }
                else isNew = false;
              }
              if (val[0] == ("categoryId"))
              {
                if (val[1] == END) return null;
                if (val[1] != id)
                {
                  isNew = false;
                  continue;
                }
              }
            }
          }
        }
      }
      return null;
    }
  }
}
