using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Com.Seezt.Skins;

namespace Com.Seezt.Skins
{
    public partial class BasicContextMenu : ContextMenuStrip
    {
        private Graphics g = null;
        private Bitmap Bmp = null;

        public BasicContextMenu()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            g = e.Graphics;
            Bmp = ResClass.GetResObj("menuEx_background");
            g.DrawImage(Bmp, new Rectangle(0, 0, 28, 5), 0, 0, 28, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(28, 0, this.Width - 33, 5), 29, 0, Bmp.Width - 33, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 0, 5, 5), Bmp.Width - 5, 0, 5, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(0, 5, 28, this.Height - 10), 0, 5, 28, Bmp.Height - 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(28, 5, this.Width - 33, this.Height - 10), 29, 5, Bmp.Width - 33, Bmp.Height - 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 5, 5, this.Height - 10), Bmp.Width - 5, 5, 5, Bmp.Height - 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(0, this.Height - 5, 28, 5), 0, Bmp.Height - 5, 28, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(28, this.Height - 5, this.Width - 33, 5), 29, Bmp.Height - 5, Bmp.Width - 33, 5, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, this.Height - 5, 5, 5), Bmp.Width - 5, Bmp.Height - 5, 5, 5, GraphicsUnit.Pixel);
        }
    }
}
