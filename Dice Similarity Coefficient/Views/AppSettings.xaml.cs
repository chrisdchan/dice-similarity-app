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

namespace Dice_Similarity_Coefficient.Views
{

    public partial class AppSettings : UserControl
    {

        public static Boolean manualSelect = true;
        public static int labelMethod = -1;
        public AppSettings()
        {
            InitializeComponent();
            checkbox.IsChecked = manualSelect;
            labelSelection.SelectedIndex = labelMethod;
        }
        public static Boolean getManualSelect()
        {
            return manualSelect;
        }

        public static int getLabelMethod()
        {
            return labelMethod;
        }
            
        private void manualSelect_changed(object sender, RoutedEventArgs e)
        {
            if(checkbox.IsChecked == null)
            {
                manualSelect = false;
            }
            else
            {
                manualSelect = (Boolean)checkbox.IsChecked;
            }
        }

        private void comboBox_changed(object sender, RoutedEventArgs e)
        {

            labelMethod = labelSelection.SelectedIndex;

        }
    }
}
