using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dice_Similarity_Coefficient
{
    public class ImageProps
    {
        public double height;  
        public double width;
        public double x;
        public double y;

        public ImageProps(double h, double w, double py, double px)
        {
            height = h;
            width = w;
            x = px;
            y = py;
        }

        public void setPos(double py, double px)
        {
            x = px;
            y = py;
        }

        public void setDim(double h, double w)
        {
            height = h;
            width = w;
        }
    }
}
