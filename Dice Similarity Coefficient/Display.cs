using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Dice_Similarity_Coefficient
{
    class Display
    {
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr obj);
        private static IntPtr hBitmap;

        public static BitmapSource formatBitmap(Bitmap b)
        {

            IntPtr hBitmap  = b.GetHbitmap();
            BitmapSource res;
            
            res = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            return res;

        }




    }
}
