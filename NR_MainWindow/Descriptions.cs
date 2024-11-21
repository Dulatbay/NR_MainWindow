using System;
using System.Collections;
using System.Collections.Generic;

namespace NR_MainWindow
{
  public class Descriptions : ICollection<IDescription>
  {
    public class DesriptionEnumerator : IEnumerator<IDescription>
    {
      private Descriptions _collection; // коллекция
      private int curIndex; // текущий индекс
      private IDescription? curDesription; // текущее дискрипция

      public DesriptionEnumerator(Descriptions desriptions)
      {
        _collection = desriptions;
        curIndex = -1;
        curDesription = default(IDescription);
      }
      public IDescription Current { get { return curDesription; } }

      object IEnumerator.Current => Current;

      public void Dispose()
      {
        GC.SuppressFinalize(this);
      }

      public bool MoveNext()
      {
        if (++curIndex >= _collection.Count)
        {
          return false;
        }
        curDesription = _collection[curIndex];
        return true;
      }

      public void Reset() => curIndex = -1;

    }

    private List<IDescription> _descriptions = new List<IDescription>();
    public int Count => _descriptions.Count;

    public bool IsReadOnly => false;
    public IDescription this[int index]
    {
      get { return _descriptions[index]; }
      set { _descriptions[index] = value; }
    }
    public void Add(IDescription item)
    {
      _descriptions.Add(item);
    }

    public int GetCopyCount(IDescription description)
    {
      int cnt = 0;
      foreach (var desc in _descriptions)
      {
        if (desc.GetLine() == description.GetLine())
          cnt++;
      }
      return cnt;
    }

    public void Clear()
    {
      _descriptions.Clear();
    }

    public IDescription? GetDescription(string manu, string model)
    {
      if(model == null) 
        return null;

      foreach (var item in _descriptions)
      {
        if (item.Model.Trim() == model.Trim() && item.Manufacturer.Trim() == manu.Trim())
        {
          return item;
        }
      }
      return null;
    }

    public bool Contains(IDescription item)
    {
      foreach (var desc in _descriptions)
      {
        if (desc.GetLine() == item.GetLine())
          return true;
      }
      return false;
    }

    public void CopyTo(IDescription[] array, int arrayIndex)
    {
      _descriptions.CopyTo(array, arrayIndex);
    }

    public IEnumerator<IDescription> GetEnumerator() => new DesriptionEnumerator(this);

    public bool Remove(IDescription item)
    {
      int i = 0;
      foreach (var desc in _descriptions)
      {
        if (desc.GetLine() == item.GetLine())
        {
          _descriptions.RemoveAt(i);
          return true;
        }
        i++;
      }
      return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => new DesriptionEnumerator(this);

    public List<string> GetManufacturers()
    {
      List<string> manufacturers = new List<string>();
      foreach (var desc in _descriptions)
      {
        if (!manufacturers.Contains(desc.Manufacturer)) { manufacturers.Add(desc.Manufacturer); }
      }
      return manufacturers;
    }

    public List<string> GetModels(string manufacturers)
    {
      List<string> models = new List<string>();
      foreach (var desc in _descriptions)
      {
        if (desc.Manufacturer == manufacturers) { models.Add(desc.Model); }
      }
      return models;
    }

  }
}
