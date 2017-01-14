using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Com.Seezt.Skins;

namespace Com.Seezt.Skins
{
    public class BasicToolStripSeparator : ToolStripSeparator
    {
        private Graphics g = null;

        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawImage(ResClass.GetResObj("menuItemEx_seperatorDraw"), new Rectangle(28, 2, this.Width-28, 2), 0, 0, 97, 2, GraphicsUnit.Pixel);
        }
    }
}
