using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Com.Seezt.Skins
{
    public partial class LinkButton : UserControl
    {
        public LinkButton()
        {
            InitializeComponent();
            bmp = ResClass.GetResObj("mune_select_bkg");
            //this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        private Bitmap bmp = null;
            private Image icon = null;
            private bool check;
            private string text;

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
                    if (text!="")
                        labelText.Text = text;
                }
            }

            [Description("选中"), Category("Appearance")]
            public bool Checked
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

            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                if (Checked)
                {
                    g.DrawImage(bmp, new Rectangle(0, 0, 3, this.Height), 0, 0, 3, bmp.Height, GraphicsUnit.Pixel);
                    g.DrawImage(bmp, new Rectangle(3, 0, this.Width - 6, this.Height), 3, 0, bmp.Width - 6, bmp.Height, GraphicsUnit.Pixel);
                    g.DrawImage(bmp, new Rectangle(this.Width - 3, 0, 3, this.Height), bmp.Width - 3, 0, 3, bmp.Height, GraphicsUnit.Pixel);
                    //g.DrawString(Texts, new Font("微软雅黑", 9F), Brushes.White, 5,2);
                }
                else
                {
                    g.DrawString(Texts, new Font("微软雅黑", 9F), Brushes.DodgerBlue, 5, 2);
                }
                if (Icon != null)
                    g.DrawImage(icon, new Rectangle(8, 1, 20, 20), 0, 0, 20, 20, GraphicsUnit.Pixel);
            }

            private void labelText_MouseEnter(object sender, EventArgs e)
            {
                labelText.Cursor = Cursors.Hand;
                labelText.Invalidate();
            }

            private void labelText_MouseLeave(object sender, EventArgs e)
            {
                labelText.Cursor = Cursors.Default;
            }

            private void labelText_Paint(object sender, PaintEventArgs e)
            {
                if (Checked)
                {
                    Graphics g = e.Graphics;
                    g.DrawImage(bmp, new Rectangle(0, 0, 3, this.Height), 0, 0, 3, bmp.Height, GraphicsUnit.Pixel);
                    g.DrawImage(bmp, new Rectangle(3, 0, this.Width - 6, this.Height), 3, 0, bmp.Width - 6, bmp.Height, GraphicsUnit.Pixel);
                    g.DrawImage(bmp, new Rectangle(this.Width - 3, 0, 3, this.Height), bmp.Width - 3, 0, 3, bmp.Height, GraphicsUnit.Pixel);
                }
            }
    }
}
