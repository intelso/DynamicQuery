using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicQueryTest
{
    public class ExtendedLabel:Label
    {
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Size size = TextRenderer.MeasureText(this.Text, this.Font, new Size(this.Width, 0), TextFormatFlags.WordBreak);
            Size size1 = TextRenderer.MeasureText(this.Text, this.Font, new Size(this.Width, 0), TextFormatFlags.SingleLine);
            int height = Math.Max(size.Height,size1.Height*2);
            //if (this.Height<height)
            //{
                this.Height = height;
            //}
            //if (size.Height > this.Height)
            //{
            //    this.Height = size.Height;
            //}
            //this.Height = size.Height;
        }
    }
}
