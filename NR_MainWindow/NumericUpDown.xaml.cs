using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NR_MainWindow
{
  /// <summary>
  /// Логика взаимодействия для NumericUpDown.xaml
  /// </summary>
  public partial class NumericUpDown : UserControl
  {
    /// <summary>
    /// Initializes a new instance of the NumericUpDownControl.
    /// </summary>
    public NumericUpDown()
    {
      InitializeComponent();
    }

    /// <summary>
    /// Identifies the Value dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(
            "Val", typeof(int), typeof(NumericUpDown),
            new FrameworkPropertyMetadata(MinValue, new PropertyChangedCallback(OnValueChanged),
                                          new CoerceValueCallback(CoerceValue)));

    /// <summary>
    /// Gets or sets the value assigned to the control.
    /// </summary>
    public int Val
    {
      get { return (int)GetValue(ValueProperty); }
      set { SetValue(ValueProperty, value); }
    }

    private static object CoerceValue(DependencyObject element, object value)
    {
      int newValue = (int)value;
      NumericUpDown control = (NumericUpDown)element;
      newValue = Math.Max(MinValue, Math.Min(MaxValue, newValue));
      return newValue;
    }

    private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
      NumericUpDown control = (NumericUpDown)obj;
      RoutedPropertyChangedEventArgs<int> e = new RoutedPropertyChangedEventArgs<int>(
          (int)args.OldValue, (int)args.NewValue, ValueChangedEvent);
      control.OnValueChanged(e);
    }

    /// <summary>
    /// Identifies the ValueChanged routed event.
    /// </summary>
    public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
        "ValueChanged", RoutingStrategy.Bubble,
        typeof(RoutedPropertyChangedEventHandler<int>), typeof(NumericUpDown));

    /// <summary>
    /// Occurs when the Value property changes.
    /// </summary>
    public event RoutedPropertyChangedEventHandler<int> ValueChanged
    {
      add { AddHandler(ValueChangedEvent, value); }
      remove { RemoveHandler(ValueChangedEvent, value); }
    }

    /// <summary>
    /// Raises the ValueChanged event.
    /// </summary>
    /// <param name="args">Arguments associated with the ValueChanged event.</param>
    protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<int> args)
    {
      RaiseEvent(args);
    }



    private const int MinValue = 1, MaxValue = 100;

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
      Val++;
    }

  
  
    private void DownButton_Click(object sender, RoutedEventArgs e)
    {
      Val--;
    }
  }
}
