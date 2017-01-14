using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;

namespace Com.Seezt.Skins
{
    public partial class LogonButton : UserControl
    {
        private Graphics g = null;
        private Bitmap Bmp = null;

        public LogonButton()
        {
            Bmp = ResClass.GetResObj("LoginPanel_LoginButton_Focus");
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            this.BackColor = Color.FromArgb(214, 238, 255);
        }

        [Description("文本"), Category("Appearance")]
        public string Texts
        {
            get
            {
                return Text;
            }
            set
            {
                Text = value;
                this.Invalidate();
            }
        }

        private PointF GetPointF(string text)
        {
            float x, y;
            switch (text.Length)
            {
                case 1:
                    x = (float)(this.Width - 12.5 * 1) / 2;
                    y = 4;
                    break;
                case 2:
                    x = (float)(this.Width - 12.5 * 2) / 2;
                    y = 4;
                    break;
                case 3:
                    x = (float)(this.Width - 12.5 * 3) / 2;
                    y = 4;
                    break;
                case 4:
                    x = (float)(this.Width - 12.5 * 4) / 2;
                    y = 4;
                    break;
                case 5:
                    x = (float)(this.Width - 12.3 * 5) / 2;
                    y = 4;
                    break;
                case 6:
                    x = (float)(this.Width - 12.3 * 6) / 2;
                    y = 4;
                    break;
                default:
                    x = (float)(this.Width - 12.3 * 4) / 2;
                    y = 4;
                    break;
            }
            return new PointF(x, y);
        }

        private void LoginButton_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawImage(Bmp, new Rectangle(0, 0, this.Width, this.Height), 0, 0, Bmp.Width, Bmp.Height, GraphicsUnit.Pixel);
            PointF point = GetPointF(this.Text);
            if (this.Enabled)
                g.DrawString(this.Texts, new Font("宋体", 9F), Brushes.Black, point);
            else
                g.DrawString(this.Texts, new Font("宋体", 9F), Brushes.Gray, point);
        }

        private void LoginButton_MouseDown(object sender, MouseEventArgs e)
        {
            Bmp = ResClass.GetResObj("LoginPanel_LoginButton_pushedAction_sb1_background_T0");
            this.Invalidate();
        }

        private void LoginButton_MouseEnter(object sender, EventArgs e)
        {
            Bmp = ResClass.GetResObj("LoginPanel_LoginButton_background_foreground");
            this.Invalidate();
        }

        private void LoginButton_MouseLeave(object sender, EventArgs e)
        {
            Bmp = ResClass.GetResObj("LoginPanel_LoginButton_Focus");
            this.Invalidate();
        }

        private void LoginButton_MouseUp(object sender, MouseEventArgs e)
        {
            Bmp = ResClass.GetResObj("LoginPanel_LoginButton_Focus");
            this.Invalidate();
        }
    }
}
