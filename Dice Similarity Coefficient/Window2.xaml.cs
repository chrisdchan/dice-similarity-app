using Dice_Similarity_Coefficient.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using Color = System.Drawing.Color;
using Point = System.Windows.Point;

namespace Dice_Similarity_Coefficient
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {

        private List<String> files1 = ImageSelection.getFiles1();
        private List<String> files2 = ImageSelection.getFiles2();
        public Window2()
        {


            InitializeComponent();

            if(files1.Count > 0)
            {
                img1.Source = new BitmapImage(new Uri(files1[0], UriKind.Absolute));
            }
            if(files2.Count > 0)
            {
                img2.Source = new BitmapImage(new Uri(files2[0], UriKind.Absolute));
            }

            Rect_color.Fill = System.Windows.Media.Brushes.SkyBlue;

        }

        //private void img1_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    Point p = e.GetPosition(img1);

        //    int z = 0;
        //    int pixel_X;
        //    int pixel_Y;

        //    Color c = Color.FromArgb(1, 0, 0, 0);

        //    if (!Double.IsNaN(img1.Width) && !Double.IsNaN(img1.Height))
        //    {
        //        pixel_X = (int)Math.Round(folder1[z].Width * p.X / img1.Width);
        //        pixel_Y = (int)Math.Round(p.Y * folder1[z].Height / img2.Height);
                
        //        c = folder1[z].GetPixel(pixel_X, pixel_Y);
        //    }


        //    System.Windows.Media.Color new_c = System.Windows.Media.Color.FromArgb(c.A, c.R, c.B, c.G);

        //    Rect_color.Fill = new SolidColorBrush(new_c);

        //}

        //private void img2_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    Point p = e.GetPosition(img2);

        //    int z = 0;
        //    int pixel_X;
        //    int pixel_Y;

        //    Color c = Color.FromArgb(1, 0, 0, 0);

        //    if(!Double.IsNaN(img2.Height) && !Double.IsNaN(img2.Height))
        //    {
        //        pixel_X = (int)Math.Round(p.X * folder2[z].Width / img2.Width);
        //        pixel_Y = (int)Math.Round(p.Y * folder2[z].Height / img2.Height);

        //        c = folder2[z].GetPixel(pixel_X, pixel_Y);
        //    }

        //    System.Windows.Media.Color new_c = System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);

        //    Rect_color.Fill = new SolidColorBrush(new_c);

        //}

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            img1.Width = ActualHeight * 0.75 - 4;
            img1.Height = ActualWidth * (1.0 / 3.0) - 4;

            img2.Width = ActualHeight * 0.75 - 4;
            img2.Height = ActualWidth * (1.0 / 3.0) - 4;
        }
    }
}
