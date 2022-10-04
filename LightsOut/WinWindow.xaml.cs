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
using System.Windows.Shapes;

namespace LightsOut
{
    /// <summary>
    /// Interaction logic for WinWindow.xaml
    /// </summary>
    public partial class WinWindow : Window
    {
        private int Level { get; set; }

        private List<Level> Levels { get; set; }

        public WinWindow(int level, List<Level> levels)
        {
            InitializeComponent();
            Level = level;
            Levels = levels;
            Label Massage = (Label)FindName("massageLabel");
            Massage.SetCurrentValue(ContentProperty, "You Win this Battle..");
            if (Levels.Count - 1 == Level)
            {
                Massage.SetCurrentValue(ContentProperty, "Congratulations..");
                Button Next = (Button)FindName("NextButton");
                Next.SetCurrentValue(ContentProperty, "Finish");
            }
        }


        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimizeClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonMaximizeClick(object sender, RoutedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
                WindowState = WindowState.Maximized;
            else WindowState = WindowState.Normal;
        }

        private void ButtonCloseClick(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
