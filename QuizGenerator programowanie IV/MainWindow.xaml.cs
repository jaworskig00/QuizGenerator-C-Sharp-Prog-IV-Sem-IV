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

namespace QuizGenerator_programowanie_IV
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        // NAWIGACJA
        // 1. Menu View
        private void AddQuiz_ButtonClick(object sender, RoutedEventArgs e)
        {
            MenuView.Visibility = Visibility.Collapsed;
            AddView.Visibility = Visibility.Visible;
            ModifyView.Visibility = Visibility.Collapsed;
        }

        private void ModifyQuiz_ButtonClick(object sender, RoutedEventArgs e)
        {
            MenuView.Visibility = Visibility.Collapsed;
            AddView.Visibility = Visibility.Collapsed;
            ModifyView.Visibility = Visibility.Visible;
        }

        private void Exit_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GoToMenu_ButtonClick(object sender, RoutedEventArgs e)
        {
            MenuView.Visibility = Visibility.Visible;
            AddView.Visibility = Visibility.Collapsed;
            ModifyView.Visibility = Visibility.Collapsed;
        }

        // 2. Add Quiz View
    }
}
