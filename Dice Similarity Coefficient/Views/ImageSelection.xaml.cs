using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using MessageBox = System.Windows.MessageBox;
using Dice_Similarity_Coefficient.Properties;
using System.Text.RegularExpressions;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using Rectangle = System.Drawing.Rectangle;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Dice_Similarity_Coefficient.Views
{
    public partial class ImageSelection : System.Windows.Controls.UserControl
    {

        public static int slice = 0;
        private static int zmax = 0;

        private static List<string> files1 = new List<string>();
        private static List<string> files2 = new List<string>();

        public ImageSelection()
        {
            InitializeComponent();

            if(files1.Count > 0)
            {
                img1.Source = new BitmapImage(new Uri(files1[slice], UriKind.Absolute));
            }
            if(files2.Count > 0)
            {
                img2.Source = new BitmapImage(new Uri(files2[slice], UriKind.Absolute));
                //rotate2.Visibility = Visibility.Visible;
                //mirror.Visibility = Visibility.Visible;
            }

            if(files1.Count > 0 && files2.Count > 0)
            {
                zslider.Visibility = Visibility.Visible;
                sliderTxt.Visibility = Visibility.Visible;
                zmax = Math.Min(files1.Count, files2.Count);
                zslider.Maximum = zmax - 1;

                sliderTxt.Text = slice.ToString();
            }
        }

        public static void setFiles1(List<String> x)
        {
            files1 = x;
        }
        public static void setFiles2(List<String> x)
        {
            files2 = x;
        }

        public static List<String> getFiles1()
        {
            return files1;
        }

        public static List<String> getFiles2()
        {
            return files2;
        }

        private void selectFolder1(object sender, RoutedEventArgs e)
        {

            if(MainWindow.getManualSelected())
            {
                multiSelect(ref files1);
            }
            else
            {
                folderSelect(ref files1);
            }

            if(files1.Count > 0)
            {
                img1.Source = new BitmapImage(new Uri(files1[0], UriKind.Absolute));

            }

            if (files1.Count > 0 && files2.Count > 0)
            {
                zslider.Visibility = Visibility.Visible;
                sliderTxt.Visibility = Visibility.Visible;
                zmax = Math.Min(files1.Count, files2.Count);
                zslider.Maximum = zmax - 1;

                sliderTxt.Text = slice.ToString();
            }

        }
        private void selectFolder2(object sender, RoutedEventArgs e)
        {
            if (MainWindow.getManualSelected())
            {
                multiSelect(ref files2);
            }
            else
            {
                folderSelect(ref files2);
            }
            
            if(files2.Count > 0)
            {
                img2.Source = new BitmapImage(new Uri(files2[slice], UriKind.Absolute));

                //rotate2.Visibility = Visibility.Visible;
                //mirror.Visibility = Visibility.Visible;

            }

            if (files1.Count > 0 && files2.Count > 0)
            {
                zslider.Visibility = Visibility.Visible;
                sliderTxt.Visibility = Visibility.Visible;
                zmax = Math.Min(files1.Count, files2.Count);
                zslider.Maximum = zmax - 1;

                sliderTxt.Text = slice.ToString();

            }
        }
        private void multiSelect(ref List<string> files)
        {
            files.Clear();
            slice = 0;

            OpenFileDialog ofile = new OpenFileDialog();
            ofile.Filter = "Image File (*.bmp,*.jpg)|*.bmp;*.jpg;*.jfif;*.png";
            ofile.Multiselect = true;

            if (ofile.ShowDialog() == true)
            {
                foreach (String file in ofile.FileNames)
                {
                    files.Add(file);
                   
                }
            }
        }

        private void folderSelect(ref List<string> files) 
        {
            files.Clear();
            slice = 0;

            FolderBrowserDialog ofold = new FolderBrowserDialog();
            DialogResult result = ofold.ShowDialog();

            if (result.ToString() != String.Empty)
            {
                String path = ofold.SelectedPath.ToString();
                String rx = @"\.(bmp|jpg|jfif|png)$";

                if(path != String.Empty)
                {
                    int failCount = 0;

                    foreach (string file in Directory.GetFiles(path))
                    {
                        if (Regex.IsMatch(file, rx) && File.Exists(file))
                        {
                                files.Add(file);

                           
                        }
                    }

                    if(failCount > 0)
                    {
                        MessageBox.Show(String.Concat(failCount.ToString() + " images could not be uploaded"));
                    }

                }

            }
        }
        //private void rotate2_Click(object sender, RoutedEventArgs e)
        //{
        //    for(int i = 0; i < folder2.Count; i++)
        //    {
        //        folder2[i].RotateFlip(RotateFlipType.Rotate90FlipNone);
        //    }
        //    img2.Source = new BitmapImage(new Uri(files2[slice], UriKind.Absolute));
        //}

        //private void mirror_Click(object sender, RoutedEventArgs e)
        //{
        //    for (int i = 0; i < folder2.Count; i++)
        //    {
        //        folder2[i].RotateFlip(RotateFlipType.RotateNoneFlipX);
        //    }
        //    img2.Source = new BitmapImage(new Uri(files2[slice], UriKind.Absolute));
        //}

        private void zslider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int val = (int)Math.Round(zslider.Value);
            slice = val;
            sliderTxt.Text = (val + 1).ToString();
            slice = val;

            if(slice >= 0 && slice < Math.Min(files1.Count, files2.Count))
            {
                img1.Source = new BitmapImage(new Uri(files1[slice], UriKind.Absolute));
                img2.Source = new BitmapImage(new Uri(files2[slice], UriKind.Absolute));
            }

        }
    }
}
