using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LightsOut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int Level { get; set; }

        private List<Level> Levels { get; set; }

        private int Moves { get; set; } = 0;

        private List<int> OnElements { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            using (StreamReader r = new StreamReader("../../../levels/lights-out-levels.json"))
            {
                string json = r.ReadToEnd();
                Levels = JsonConvert.DeserializeObject<List<Level>>(json);
            }
            Level = 0;
            InstantiateLevel(Levels.ElementAt(Level));
        }

        private void Img_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var img = (Image)sender;
            int row = int.Parse(img.Tag.ToString().Split(',')[0]);
            int column = int.Parse(img.Tag.ToString().Split(',')[1]);
            Grid grid = (Grid)Application.Current.MainWindow.FindName("GameGrid");
            var element = grid.Children.Cast<Image>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
            ToggleElement(element);
            UpdateResult(element, row, column);
            if (row > 0)
            {
                element = grid.Children.Cast<Image>().First(e => Grid.GetRow(e) == row - 1 && Grid.GetColumn(e) == column);
                ToggleElement(element);
                UpdateResult(element, row - 1, column);
            }
            if (column > 0)
            {
                element = grid.Children.Cast<Image>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column - 1);
                ToggleElement(element);
                UpdateResult(element, row, column - 1);
            }
            if (row < 3)
            {
                element = grid.Children.Cast<Image>().First(e => Grid.GetRow(e) == row + 1 && Grid.GetColumn(e) == column);
                ToggleElement(element);
                UpdateResult(element, row + 1, column);
            }
            if (column < 3)
            {
                element = grid.Children.Cast<Image>().First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column + 1);
                ToggleElement(element);
                UpdateResult(element, row, column + 1);
            }
            Moves++;
            Label MovesLabel = (Label)Application.Current.MainWindow.FindName("MovesLabel");
            MovesLabel.SetCurrentValue(ContentProperty, Moves.ToString());
        }

        private void ToggleElement(Image image)
        {
            if (((BitmapImage)image.Source).UriSource.AbsoluteUri.Equals("pack://application:,,,/assets/PNG/IMAGE_on.png"))
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/assets/PNG/IMAGE_off.png"));
            }
            else
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/assets/PNG/IMAGE_on.png"));
            }
        }

        private void InstantiateLevel(Level level)
        {
            Grid grid = (Grid)Application.Current.MainWindow.FindName("GameGrid");
            grid.Children.Clear();
            grid.RowDefinitions.Clear();
            grid.ColumnDefinitions.Clear();
            Moves = 0;
            Label MovesLabel = (Label)Application.Current.MainWindow.FindName("MovesLabel");
            MovesLabel.SetCurrentValue(ContentProperty, Moves.ToString());
            OnElements = new List<int>();
            foreach (int element in level.On) OnElements.Add(element);
            Label OnLabel = (Label)Application.Current.MainWindow.FindName("OnLabel");
            OnLabel.SetCurrentValue(ContentProperty, OnElements.Count.ToString());
            int counter = 0;
            grid.Background = new SolidColorBrush(Colors.DarkRed);
            for (int i = 0; i < level.Rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < level.Columns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < level.Rows; i++)
            {
                for (int j = 0; j < level.Columns; j++)
                {
                    Image img = new()
                    {
                        Source = level.On.Contains(counter) ? new BitmapImage(new Uri("pack://application:,,,/assets/PNG/IMAGE_on.png")) : new BitmapImage(new Uri("pack://application:,,,/assets/PNG/IMAGE_off.png")),
                        Width = level.Columns >= 6 ? 40 : 100,
                        Height = level.Rows >= 6 ? 40 : 100
                    };
                    img.SetValue(TagProperty, i.ToString() + ',' + j.ToString());
                    img.PreviewMouseLeftButtonUp += Img_PreviewMouseLeftButtonUp;
                    Grid.SetRow(img, i);
                    Grid.SetColumn(img, j);
                    grid.Children.Add(img);
                    counter++;
                }
            }
        }

        private void UpdateResult(Image image, int row, int column)
        {
            int numElements = (int)Levels.ElementAt(Level).Columns;
            int index = (row) * numElements + column;
            if (((BitmapImage)image.Source).UriSource.AbsoluteUri.Equals("pack://application:,,,/assets/PNG/IMAGE_on.png"))
            {
                if (!OnElements.Contains(index)) OnElements.Add(index);
            }
            else
            {
                if (OnElements.Contains(index)) OnElements.Remove(index);
            }
            Label OnLabel = (Label)Application.Current.MainWindow.FindName("OnLabel");
            OnLabel.SetCurrentValue(ContentProperty, OnElements.Count.ToString());
            CheckGameState();
        }

        private void CheckGameState()
        {
            if (OnElements.Count == (int)Levels.ElementAt(Level).Rows * (int)Levels.ElementAt(Level).Columns)
            {
                WinWindow winPage = new WinWindow(Level, Levels);
                winPage.ShowDialog();
                UpdateLeve();
            }
        }

        private void UpdateLeve()
        {
            Level += 1;
            if (Level >= Levels.Count) Environment.Exit(0);
            else InstantiateLevel(Levels.ElementAt(Level));
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
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximizeClick(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            else Application.Current.MainWindow.WindowState = WindowState.Normal;
        }

        private void ButtonCloseClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {
            InstantiateLevel(Levels.ElementAt(Level));
        }
    }
}
