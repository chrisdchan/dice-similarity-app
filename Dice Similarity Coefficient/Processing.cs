using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dice_Similarity_Coefficient
{
    class Processing
    {
        private static int aoffsetX;
        private static int aoffsetY;
        private static int boffsetX;
        private static int boffsetY;

        private static int overlapH;
        private static int overlapW;

        public static double CalculateDice(List<String> a, List<String>b, ImageProps ap, ImageProps bp, int method)
        {

            double dc;

            if(method == 1)
            {
                dc = dc_3D(a, b, ap, bp, dc_case1);
            }
            else if(method == 2)
            {
                dc = dc_3D(a, b, ap, bp, dc_case2);
            }
            else
            {
                dc = -1;
            }

            return dc;

        }

        public static double calculateJaccard(List<String> a, List<String> b, ImageProps ap, ImageProps bp, int method)
        {
            double ji;

            if (method == 1)
            {
                ji = ji_3D(a, b, ap, bp, ji_case1);
            }
            else if (method == 2)
            {
                ji = ji_3D(a, b, ap, bp, ji_case2);
            }
            else
            {
                ji = -1;
            }

            return ji;
        }


        private static double dc_3D(List<String> a, List<String> b, ImageProps ap, ImageProps bp, Func<byte[], byte[], int, int, double> dc)
        {
            int z = Math.Min(a.Count, b.Count);
            int count = 0;
            double dcSum = 0;

            for(int i = 0; i < z; i++)
            {

                Bitmap bmpA = new Bitmap(a[i]);
                Rectangle rectA= new Rectangle(0, 0, bmpA.Width, bmpA.Height);
                BitmapData dataA = bmpA.LockBits(rectA, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                int bytesA = dataA.Stride * bmpA.Height;
                byte[] arrA = new byte[bytesA];
  
                Marshal.Copy(dataA.Scan0, arrA, 0, bytesA);


                Bitmap bmpB = new Bitmap(b[i]);
                Rectangle rectB = new Rectangle(0, 0, bmpB.Width, bmpB.Height);
                BitmapData dataB = bmpB.LockBits(rectB, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                int bytesB = dataB.Stride * bmpB.Height;
                byte[] arrB = new byte[bytesB];
                Marshal.Copy(dataB.Scan0, arrB, 0, bytesB);

                initOffsets(bmpA.Width, bmpA.Height, bmpB.Width, bmpB.Height, ap, bp);

                double dice = dc(arrA, arrB, dataA.Stride, dataB.Stride);

                if (!Double.IsNaN(dice))
                {
                    dcSum += dice;
                    count++;
                }

                GC.Collect();

                bmpA.UnlockBits(dataA);
                bmpB.UnlockBits(dataB);
                bmpA.Dispose();
                bmpB.Dispose();
            }

            double res = dcSum / count;
            res = Math.Round(res, 4);
            return res;
        }

        private static double ji_3D(List<String> a, List<String> b, ImageProps ap, ImageProps bp, Func<byte[], byte[], int, int, double> ji)
        {
            int z = Math.Min(a.Count, b.Count);
            int count = 0;
            double jiSum = 0;
            for (int i = 0; i < z; i++)
            {
                Bitmap bmpA = new Bitmap(a[i]);
                Bitmap bmpB = new Bitmap(b[i]);

                Rectangle rectA = new Rectangle(0, 0, bmpA.Width, bmpA.Height);
                Rectangle rectB = new Rectangle(0, 0, bmpB.Width, bmpB.Height);

                BitmapData dataA = bmpA.LockBits(rectA, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                BitmapData dataB = bmpB.LockBits(rectB, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

                int bytesA = dataA.Stride * bmpA.Height;
                int bytesB = dataB.Stride * bmpB.Height;

                byte[] arrA = new byte[bytesA];
                byte[] arrB = new byte[bytesB];

                Marshal.Copy(dataA.Scan0, arrA, 0, bytesA);
                Marshal.Copy(dataB.Scan0, arrB, 0, bytesB);

                initOffsets(bmpA.Width, bmpA.Height, bmpB.Width, bmpB.Height, ap, bp);



                double jaccard = ji(arrA, arrB, dataA.Stride, dataB.Stride);
                
                if(!Double.IsNaN(jaccard))
                {
                    jiSum += jaccard;
                    count++;
                }

                bmpA.UnlockBits(dataA);
                bmpB.UnlockBits(dataB);
                bmpA.Dispose();
                bmpB.Dispose();
            }

            double res = jiSum / count;
            res = Math.Round(res, 4);
            return res;
        }

        private static double dc_case1(byte[] a, byte[] b, int aStride, int bStride)
        {
            //Case: Labels are indicated by color, Background is indicated by black and white

            int labelOverlap = 0;
            int alabelArea = 0;
            int blabelArea = 0;

            for (  int c = 0; c < overlapH; c++)
            {
                for (int r = 0; r < overlapW; r++)
                {
                    int inda = aStride * (c + aoffsetY) + (r + aoffsetX) * 3;
                    int indb = bStride * (c + boffsetY) + (r + boffsetX) * 3;


                    Boolean isA = isColorful(a[inda], a[inda + 1], a[inda + 2]);
                    Boolean isB = isColorful(b[indb], b[indb + 1], b[indb + 2]);

                    if(isA && isB)
                    {
                        labelOverlap++;
                    }
                    if(isA)
                    {
                        alabelArea++;
                    }
                    if(isB)
                    {
                        blabelArea++;
                    }

                }
            }

            //plug into the formula
            double dc = 2.0 * (labelOverlap) / (alabelArea + blabelArea);

            return dc;

        }

        private static double ji_case1(byte[] a, byte[] b, int aStride, int bStride)
        {
            //Case: Labels are indicated by color, Background is indicated by black and white

            int labelOverlap = 0;
            int alabelArea = 0;
            int blabelArea = 0;


            for (int c = 0; c < overlapH; c++)
            {
                for (int r = 0; r < overlapW; r++)
                {
                    int inda = aStride * (c + aoffsetY) + (r + aoffsetX) * 3;
                    int indb = bStride * (c + boffsetY) + (r + boffsetX) * 3;

                    Boolean isA = isColorful(a[inda], a[inda + 1], a[inda + 2]);
                    Boolean isB = isColorful(b[indb], b[indb + 1], b[indb + 2]);

                    if (isA && isB)
                    {
                        labelOverlap++;
                    }
                    if (isA)
                    {
                        alabelArea++;
                    }
                    if (isB)
                    {
                        blabelArea++;
                    }
                }
            }
           
            //plug into the formula
            double dc = labelOverlap / ((double)alabelArea + blabelArea - labelOverlap);
            return dc;
        }


        private static double dc_case2(byte[] a, byte[]b, int aStride, int bStride)
        {
            int labelOverlap = 0;
            int alabelArea = 0;
            int blabelArea = 0;


            for (int c = 0; c < overlapH; c++)
            {
                for (int r = 0; r < overlapW; r++)
                {
                    int inda = aStride * (c + aoffsetY) + (r + aoffsetX) * 3;
                    int indb = bStride * (c + boffsetY) + (r + boffsetX) * 3;

                    Boolean isA = !isWhite(a[inda], a[inda + 1], a[inda + 2]);
                    Boolean isB = !isWhite(b[indb], b[indb + 1], b[indb + 2]);

                    if (isA && isB)
                    {
                        labelOverlap++;
                    }
                    if (isA)
                    {
                        alabelArea++;
                    }
                    if (isB)
                    {
                        blabelArea++;
                    }
                }
            }

            //plug into the formula
            double dc = 2.0 * (labelOverlap) / (alabelArea + blabelArea);
            return dc;

        }

        private static double ji_case2(byte[] a, byte[] b, int aStride, int bStride)
        {
            int labelOverlap = 0;
            int alabelArea = 0;
            int blabelArea = 0;

            for (int c = 0; c < overlapH; c++)
            {
                for (int r = 0; r < overlapW; r++)
                {
                    int inda = aStride * (c + aoffsetY) + (r + aoffsetX) * 3;
                    int indb = bStride * (c + boffsetY) + (r + boffsetX) * 3;

                    Boolean isA = !isWhite(a[inda], a[inda + 1], a[inda + 2]);
                    Boolean isB = !isWhite(b[indb], b[indb + 1], b[indb + 2]);

                    if (isA && isB)
                    {
                        labelOverlap++;
                    }
                    if (isA)
                    {
                        alabelArea++;
                    }
                    if (isB)
                    {
                        blabelArea++;
                    }
                }
            }

            //plug into the formula
            double dc = labelOverlap / ((double)alabelArea + blabelArea - labelOverlap);
            return dc;

        }

        private static void initOffsets(int aWidth, int aHeight, int bWidth, int bHeight, ImageProps ap, ImageProps bp)
        {
            aoffsetX = 0;
            aoffsetY = 0;
            boffsetX = 0;
            boffsetY = 0;

            if (bp.x > ap.x)
            {
                aoffsetX = (int)Math.Round((bp.x - ap.x) * aWidth / ap.width);
                overlapW = Math.Min(aWidth - aoffsetX, bWidth);
            }
            else
            {
                boffsetX = (int)Math.Round((ap.x - bp.x) * bWidth / bp.width);
                overlapW = Math.Min(bWidth - boffsetX, aWidth);
            }

            if (bp.y > ap.y)
            {
                aoffsetY = (int)Math.Round((bp.y - ap.y) * aHeight / ap.height);
                overlapH = Math.Min(aHeight - aoffsetY, bHeight);
            }
            else
            {
                boffsetY = (int)Math.Round((ap.y - bp.y) * bHeight / bp.height);
                overlapH = Math.Min(bHeight - boffsetY, aHeight);
            }
        }

        private static Boolean colorSimilar(Color ca, Color cb)
        {
            int threshold = 60;

            int d1 = Math.Abs(ca.R - cb.R);
            int d3 = Math.Abs(ca.G - ca.G);
            int d2 = Math.Abs(ca.B - cb.B);

            return (d1 + d2 + d3) <= threshold;
                
        }

        private static Boolean isColorful(Color c)
        {
            int maxError = 1000;
            double mean = (c.R + c.G + c.B) / 3.0;
            double e1 = Math.Pow(c.R - mean, 2);
            double e2 = Math.Pow(c.G - mean, 2);
            double e3 = Math.Pow(c.B - mean, 2);

            return (e1 + e2 + e3) >= maxError;
        }

        private static Boolean isColorful(int r, int g, int b)
        {
            int maxError = 1000;

            double mean = (r + g + b) / 3.0;
            double e1 = Math.Pow(r - mean, 2);
            double e2 = Math.Pow(g - mean, 2);
            double e3 = Math.Pow(b - mean, 2);

            return (e1 + e2 + e3) >= maxError;

        }

        private static Boolean isWhite(int r, int g, int b)
        {
            return (r == 255 && g == 255 && b == 255);
        }
    }
}
