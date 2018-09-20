using System;
using System.Windows;
using JsonComparer.WPF.Services;

namespace JsonComparer.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel(new UIService());
        }
    }
}
