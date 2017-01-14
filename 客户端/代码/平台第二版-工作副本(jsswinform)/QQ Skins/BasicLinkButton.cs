using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Com.Seezt.Skins
{
    public class BasicLinkButton:UserControl
    {
        private Graphics g = null;
        private Bitmap bmp = null;
        private Image icon = null;

        private string text;
        private bool check;

        public BasicLinkButton()
        {
            this.BackColor = Color.Transparent;
            bmp = ResClass.GetResObj("mune_select_bkg");
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        [Description("文本"), Category("Appearance")]
        public string Texts
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        [Description("图标"), Category("Appearance")]
        public Image Icon 
        {
            get 
            {
                return icon;
            }
            set 
            {
                icon = value;
            }
        }

        [Description("选中"), Category("Appearance")]
        public bool CheckEd
        {
            get
            {
                return check;
            }
            set
            {
                check = value;
                this.Invalidate();
            }
        }

        private PointF GetPointF(string text)
        {
            float x = 1;
            float y = 3;
            if (text!=null)
            {
                switch (text.Length)
                {
                    case 1:
                        x = (float)(this.Width - 12.5 * 1) / 2;
                        y = 3;
                        break;
                    case 2:
                        x = (float)(this.Width - 12.5 * 2) / 2;
                        y = 3;
                        break;
                    case 3:
                        x = (float)(this.Width - 12.5 * 3) / 2;
                        y = 3;
                        break;
                    case 4:
                        x = (float)(this.Width - 12.5 * 4) / 2;
                        y = 3;
                        break;
                    case 5:
                        x = (float)(this.Width - 12.3 * 5) / 2;
                        y = 3;
                        break;
                    case 6:
                        x = (float)(this.Width - 12.3 * 6) / 2;
                        y = 3;
                        break;
                    default:
                        x = (float)(this.Width - 12.3 * 4) / 2;
                        y = 3;
                        break;
                }
            }
            return new PointF(x, y);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            PointF point = GetPointF(Texts);
            if (CheckEd)
            {
                g.DrawImage(bmp, new Rectangle(0, 0, 3, this.Height), 0, 0, 3, bmp.Height, GraphicsUnit.Pixel);
                g.DrawImage(bmp, new Rectangle(3, 0, this.Width - 6, this.Height), 3, 0, bmp.Width - 6, bmp.Height, GraphicsUnit.Pixel);
                g.DrawImage(bmp, new Rectangle(this.Width - 3, 0, 3, this.Height), bmp.Width - 3, 0, 3, bmp.Height, GraphicsUnit.Pixel);
                g.DrawString(Texts, new Font("宋体", 9F), Brushes.White, point);
            }
            else
            {
                g.DrawString(Texts, new Font("宋体", 9F), Brushes.DodgerBlue, point);
            }
            if (Icon != null)
                g.DrawImage(icon, new Rectangle(8, 1, 20, 20), 0, 0, 20, 20, GraphicsUnit.Pixel);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor = Cursors.Default;
        }
    }
}
