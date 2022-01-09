using Dice_Similarity_Coefficient.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dice_Similarity_Coefficient.Views
{
    public partial class DiceCoefficient : UserControl
    {

        private List<String> files1 = ImageSelection.getFiles1();
        private List<String> files2 = ImageSelection.getFiles2();

        public static ImageProps img1Prop;
        public static ImageProps img2Prop;

        private UIElement dragObject = null;
        private System.Windows.Point offset;

        public static double orgArea = -1;

        private double zf = 3;

        private double dmx;
        private double dmy;

        private Boolean right = false;
        private Boolean left = false;

        private double opacity = 0.5;

        public DiceCoefficient()
        {
            InitializeComponent();


            canvas.Focus();
            if (files1.Count > 0)
            {
                img1.Source = new BitmapImage(new Uri(files1[0], UriKind.Absolute));
            }
            if (files2.Count > 0)
            {
                img2.Source = new BitmapImage(new Uri(files2[0], UriKind.Absolute));
                alphaSlider.Value = 50;
                img2.Opacity = opacity;
            }

            if (files2.Count > 0 && files1.Count > 0)
            {
                zslider.Maximum = Math.Max(files1.Count, files2.Count) - 1;
            }

            setPosition();
            SavePosition();
        }

        private void SavePosition()
        {

            if (img1Prop == null)
            {
                img1Prop = new ImageProps(img1.Height, img1.Width,
                    Canvas.GetTop(img1), Canvas.GetLeft(img1));
            }
            else
            {
                img1Prop.setDim(img1.Height, img1.Width);
                img1Prop.setPos(Canvas.GetTop(img1), Canvas.GetLeft(img1));

            }

            if(img2Prop == null)
            {

                img2Prop = new ImageProps(img2.Height, img2.Width,
                    Canvas.GetTop(img2), Canvas.GetLeft(img2));
            }
            else
            {
                img2Prop.setDim(img2.Height, img2.Width);
                img2Prop.setPos(Canvas.GetTop(img2), Canvas.GetLeft(img2));
            }

            updateCoords();
        }

        private void setPosition()
        {
            double x1, x2, y1, y2, h1, h2, w1, w2;

            x1 = x2 = y1 = y2 = 50;
            h1 = h2 = 200;
            w1 = w2 = 200;
            

            if (img1Prop != null)
            {
                x1 = img1Prop.x;
                y1 = img1Prop.y;
                w1 = img1Prop.width;
                h1 = img1Prop.height;
            }

            if (img2Prop != null)
            {
                x2 = img2Prop.x;
                y2 = img2Prop.y;
                h2 = img2Prop.height;
                w2 = img2Prop.width;
            }

            Canvas.SetLeft(img1, x1);
            Canvas.SetTop(img1, y1);
            Canvas.SetLeft(img2, x2);
            Canvas.SetTop(img2, y2);

            img1.Width = w1;
            img1.Height = h1;
            img2.Width = w2;
            img2.Height = h2;
        }


        private void calculate(object sender, RoutedEventArgs e)
        {
            int method = MainWindow.getLabelMethod();
            int metric1 = MainWindow.getMetric();

            if(files1 == null || files2 == null || files1.Count == 0 || files2.Count == 0)
            {
                MessageBox.Show("Use the Image Selection button to select images");
            }
            else if(method == 0)
            {
                MessageBox.Show("Select the label colors in Settings");
            }
            else if(metric1 == 0)
            {
                MessageBox.Show("Select a similarity metric");
            }
            else
            {
                double score;
                String msg;
             

                if(metric1 == 1)
                {
                    text1.Text = "Calculating Dice Coefficient ... ";
                    score = Processing.CalculateDice(files1, files2, img1Prop, img2Prop, method);
                    msg = "The Dice Similarity Coefficient is ";
                }
                else if(metric1 == 2)
                {
                    text1.Text = "Calculating Jaccard Index ... ";
                    score = Processing.calculateJaccard(files1, files2, img1Prop, img2Prop, method);
                    msg = "The Jaccard Index is ";
                }
                else
                {
                    score = -1;
                    msg = "Not Implemented: return ";
                }


                String disp = String.Concat(msg, score.ToString("r"));
                text1.Text = disp;

            }
        }
        private void canvas_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            right = true;

            var position = e.GetPosition(sender as IInputElement);

            dmx = position.X - img1Prop.x;
            dmy = position.Y - img1Prop.y;

            dragObject = sender as UIElement;
            offset = e.GetPosition(canvas);
            offset.Y -= Canvas.GetTop(dragObject);
            offset.X -= Canvas.GetLeft(dragObject);
            canvas.CaptureMouse();
        }
        private void img2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Right)
            {
                right = true;
            }
            if(e.ChangedButton == MouseButton.Left)
            {
                left = true;
            }

            dragObject = sender as UIElement;
            offset = e.GetPosition(canvas);
            offset.Y -= Canvas.GetTop(dragObject);
            offset.X -= Canvas.GetLeft(dragObject);
            canvas.CaptureMouse();
        }
        private void canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (dragObject == null)
                return;

            var position = e.GetPosition(sender as IInputElement);

            if(left)
            {
                Canvas.SetLeft(dragObject, position.X - offset.X);
                Canvas.SetTop(dragObject, position.Y - offset.Y);
            }

            if(right)
            {
                double dx = img1Prop.x - img2Prop.x;
                double dy = img1Prop.y - img2Prop.y;

                Canvas.SetLeft(img1, position.X - dmx);
                Canvas.SetTop(img1, position.Y - dmy);

                Canvas.SetLeft(img2, position.X - dmx - dx);
                Canvas.SetTop(img2, position.Y - dmy - dy);
            }


            SavePosition();
        }

        private void canvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            dragObject = null;
            canvas.ReleaseMouseCapture();
            right = false;
            left = false;
        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(orgArea != -1 && orgArea == canvas.ActualWidth * canvas.ActualHeight)
            {
                setPosition();
            }
            else
            {

                double h = canvas.ActualHeight * 0.4;
                double w = canvas.ActualWidth * 0.4;

                orgArea = canvas.ActualWidth * canvas.ActualHeight;

                img1.Height = h;
                img1.Width = w;
                img2.Height = h;
                img2.Width = w;

                double x = (canvas.ActualWidth - w) * 0.5;
                double y = (canvas.ActualHeight - h) * 0.5;

                Canvas.SetLeft(img1, x);
                Canvas.SetTop(img1, y);


            }

            SavePosition();

        }

        private void canvas_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(e.Delta > 0)
            {
                zoom(zf);
            }
            else
            {
                zoom(-zf);
            }
        }
        private void zoom(double r)
        {
            double minHeight = canvas.ActualHeight * 0.1;
            double minWidth = canvas.ActualWidth * 0.1;

            if(img1.Height >= minHeight & img1.Width >= minWidth)
            {

            img1.Height += 2 * r;
            img1.Width += 2 * r;
            img2.Height += 2 * r;
            img2.Width += 2 * r;

            Canvas.SetLeft(img1, img1Prop.x - r);
            Canvas.SetTop(img1, img1Prop.y - r);
            Canvas.SetLeft(img2, img2Prop.x - r);
            Canvas.SetTop(img2, img2Prop.y - r);

            }
            else
            {
                img1.Height = minHeight;
                img2.Width = minWidth;
                img1.Width = minWidth;
                img2.Height = minHeight;
            }
            SavePosition();
        }

        private void updateCoords()
        {
            double x = Canvas.GetLeft(img2);
            double y = Canvas.GetTop(img2);

            double xdis = x + img2.Width / 2 - (img1Prop.x + img1.Width / 2);
            double ydis = img1Prop.y + img1.Height / 2 - (y + img2.Height / 2);
            double percZoom = 100 * img1Prop.height * img1Prop.width / orgArea;

            xdis = Math.Round(xdis, 2);
            ydis = Math.Round(ydis, 2);
            percZoom = Math.Round(percZoom, 2);

            cortext.Text = String.Concat("X: ", xdis.ToString(), 
                "   Y: ", ydis.ToString(), 
                " Zoom: ", percZoom.ToString(),
                "%");
        }

        private void autoCenter(object sender, RoutedEventArgs e)
        {
            center();
        }

        private void center()
        {
            double x2 = img1Prop.x + (img1.Width / 2) - (img2.Width / 2);
            double y2 = img1Prop.y + (img1.Height / 2) - (img2.Height / 2);

            Canvas.SetLeft(img2, x2);
            Canvas.SetTop(img2, y2);
            SavePosition();

        }

        private void alpha_ValueChanged(object sender, RoutedEventArgs e)
        {
            double val = alphaSlider.Value;
            opacity = val / 100;
            opacity = Math.Round(opacity, 2);
            alphaTxt.Text = opacity.ToString();

            img2.Opacity = opacity;

        }

        private void zslider_ValueChanged(object sender, RoutedEventArgs e)
        {
            int slice = (int)Math.Round(zslider.Value);
            zsliderTxt.Text = (slice + 1).ToString();

            if (slice >= 0 && slice < Math.Min(files1.Count, files2.Count))
            {
                img1.Source = new BitmapImage(new Uri(files1[slice], UriKind.Absolute));
                img2.Source = new BitmapImage(new Uri(files2[slice], UriKind.Absolute));

            }
        }

    }
}
