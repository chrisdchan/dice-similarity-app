using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Drawing.Imaging;
using Dice_Similarity_Coefficient.ViewModels;
using Dice_Similarity_Coefficient.Views;
using System.Collections.Generic;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace Dice_Similarity_Coefficient
{
    
    public partial class MainWindow : Window
    {

        public static Boolean manualSelected;

        public static int method;

        public static int metric;
        public MainWindow()
        {
            InitializeComponent();
            btnClear.Visibility = Visibility.Hidden;
            Calculate_Set.Visibility = Visibility.Collapsed;
            ImageSelection_Set.Visibility = Visibility.Visible;
            manualSelect.IsChecked = manualSelected;
            folderSelect.IsChecked = !manualSelected;

            metricSelect.SelectedIndex = metric;
            
            labelSelection.SelectedIndex = method;

            if (method == 0)
            {
                labelSelection.Foreground = new SolidColorBrush(Color.FromRgb(127, 127, 127));
            }
            else
            {
                labelSelection.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }

            if(metric == 0)
            {
                metricSelect.Foreground = new SolidColorBrush(Color.FromRgb(127, 127, 127));
            }
            else
            {
                metricSelect.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }

            DataContext = new ImageSelectionModel();
        }

        public static Boolean getManualSelected()
        {
            return manualSelected;
        }

        public static int getLabelMethod()
        {
            return method;
        }

        public static int getMetric()
        {
            return metric;
        }

        private void Image_Selection_Click(object sender, RoutedEventArgs e)
        {
            btnClear.Visibility = Visibility.Hidden;
            Calculate_Set.Visibility = Visibility.Collapsed;
            ImageSelection_Set.Visibility = Visibility.Visible;
            DataContext = new ImageSelectionModel();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            btnClear.Visibility = Visibility.Hidden;
            DataContext = new AppSettingsModel();
        }

        private void Dice_Coefficient_Click(object sender, RoutedEventArgs e)
        {
            btnClear.Visibility = Visibility.Visible;
            ImageSelection_Set.Visibility = Visibility.Collapsed;
            Calculate_Set.Visibility = Visibility.Visible;
            DataContext = new DiceCoefficientModel();

        }

        private void clear(object sender, RoutedEventArgs e)
        {
            ImageSelection.setFiles1(new List<String>());
            ImageSelection.setFiles2(new List<String>());
            btnClear.Visibility = Visibility.Hidden;

            Calculate_Set.Visibility = Visibility.Collapsed;
            ImageSelection_Set.Visibility = Visibility.Visible;

            DataContext = new ImageSelectionModel();
        }

        private void manualSelect_Checked(object sender, RoutedEventArgs e)
        {
            manualSelect.IsChecked = true;
            folderSelect.IsChecked = false;

            manualSelected = true;
        }

        private void folderSelect_Checked(object sender, RoutedEventArgs e)
        {
            manualSelect.IsChecked = false;
            folderSelect.IsChecked = true;

            manualSelected = false;
        }
        private void metricSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            metric = metricSelect.SelectedIndex;

            if (metric == 0)
            {
                metricSelect.Foreground = new SolidColorBrush(Color.FromRgb(127, 127, 127));
            }
            else
            {
                metricSelect.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }

        private void labelSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            method = labelSelection.SelectedIndex;

            if(method == 3)
            {
                Window2 win2 = new Window2();
                win2.Show();
            }

            if (method == 0)
            {
                labelSelection.Foreground = new SolidColorBrush(Color.FromRgb(127, 127, 127));
            }
            else
            {
                labelSelection.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }
        }
    }
}
