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
    public partial class BasicTextBox : UserControl
    {
        private Graphics g = null;
        private Bitmap bmp = null;
        public BasicTextBox()
        {
            bmp = ResClass.GetResObj("frameBorderEffect_normalDraw");
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
        }

        [Description("文字在输入框中的位置"), Category("Appearance")]
        public Point SPLocation
        {
            get
            {
                return textBox1.Location;
            }
            set
            {
                textBox1.Location = value;
            }
        }

        [Description("滚动条"), Category("Appearance")]
        public ScrollBars SB
        {
            get
            {
                return textBox1.ScrollBars;
            }
            set
            {
                textBox1.ScrollBars = value;
            }
        }

        [Description("文本"), Category("Appearance")]
        public string Texts
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        [Description("多行"), Category("Appearance")]
        public bool Multi
        {
            get
            {
                return textBox1.Multiline;
            }
            set
            {
                textBox1.Multiline = value;
                if (value)
                    textBox1.Height = this.Height - 6;
            }
        }

        [Description("只读"), Category("Appearance")]
        public bool ReadOn
        {
            get
            {
                return textBox1.ReadOnly;
            }
            set
            {
                textBox1.ReadOnly = value;
            }
        }

        [Description("密码框"), Category("Appearance")]
        public bool IsPass
        {
            get
            {
                return textBox1.UseSystemPasswordChar;
            }
            set
            {
                textBox1.UseSystemPasswordChar = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawImage(bmp, new Rectangle(0, 0, 4, 4), 0, 0, 4, 4, GraphicsUnit.Pixel);
            g.DrawImage(bmp, new Rectangle(4, 0, this.Width - 8, 4), 4, 0, bmp.Width - 8, 4, GraphicsUnit.Pixel);
            g.DrawImage(bmp, new Rectangle(this.Width - 4, 0, 4, 4), bmp.Width - 4, 0, 4, 4, GraphicsUnit.Pixel);

            g.DrawImage(bmp, new Rectangle(0, 4, 4, this.Height - 6), 0, 4, 4, bmp.Height - 8, GraphicsUnit.Pixel);
            g.DrawImage(bmp, new Rectangle(this.Width - 4, 4, 4, this.Height - 6), bmp.Width - 4, 4, 4, bmp.Height - 6, GraphicsUnit.Pixel);

            g.DrawImage(bmp, new Rectangle(0, this.Height - 2, 2, 2), 0, bmp.Height - 2, 2, 2, GraphicsUnit.Pixel);
            g.DrawImage(bmp, new Rectangle(2, this.Height - 2, this.Width - 2, 2), 2, bmp.Height - 2, bmp.Width - 4, 2, GraphicsUnit.Pixel);
            g.DrawImage(bmp, new Rectangle(this.Width - 2, this.Height - 2, 2, 2), bmp.Width - 2, bmp.Height - 2, 2, 2, GraphicsUnit.Pixel);
        }

        private void textBox1_MouseEnter(object sender, EventArgs e)
        {
            bmp = ResClass.GetResObj("frameBorderEffect_mouseDownDraw");
            this.Invalidate();
        }

        private void textBox1_MouseLeave(object sender, EventArgs e)
        {
            bmp = ResClass.GetResObj("frameBorderEffect_normalDraw");
            this.Invalidate();
        }

        public void AppendText(string ss) 
        {
            textBox1.AppendText(ss);
        }

        private void BasicTextBox_Resize(object sender, EventArgs e)
        {
            if (this.Height > 50)
            {
                Multi = true;
            }
            else
            {
                //this.Height = 25;
                Multi = false;
            }
        }
    }
}
