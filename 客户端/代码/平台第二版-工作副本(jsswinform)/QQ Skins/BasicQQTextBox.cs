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
    public partial class BasicQQTextBox : UserControl
    {
        private Graphics g = null;
        private Bitmap Bmp = null;
        private Color borderColor = Color.FromArgb(84, 165, 213);
        
        public BasicQQTextBox()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            Icon = ResClass.GetResObj("All_MainPanel_MainMenuButton_highlight_on");
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
                this.Invalidate();
            }
        }


        [Description("图标"), Category("Appearance")]
        public Bitmap Icon
        {
            get
            {
                return Bmp;
            }
            set
            {
                Bmp = value;
                this.Invalidate();
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
                if(value)
                    Icon = ResClass.GetResObj("keyboard");
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
                if (value)
                    textBox1.BackColor = Color.Gray;
                else
                    textBox1.BackColor = Color.White;
            }
        }

        public System.Windows.Forms.TextBox textBox
        {
            get
            {
                return textBox1;
            }
            set
            {
                textBox1 = value;
            }
        }

        [Description("右键菜单"), Category("Appearance")]
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return textBox1.ContextMenuStrip;
            }
            set
            {
                textBox1.ContextMenuStrip = value;
            }
        }

        private void TextBox_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawRectangle(new Pen(borderColor, 1F),0,0,this.Width-1,this.Height-1);
            if(Icon!=null)
                g.DrawImage(Icon, new Rectangle(1, 1, Bmp.Width, Bmp.Height), 0, 0, Bmp.Width, Bmp.Height, GraphicsUnit.Pixel);
        }

        private void TextBox_MouseEnter(object sender, EventArgs e)
        {
            g = this.CreateGraphics();
            borderColor = Color.FromArgb(133, 228, 255);
            g.DrawRectangle(new Pen(borderColor, 1F), 0, 0, this.Width - 1, this.Height - 1);
            g.Dispose();
            this.Invalidate();
        }

        private void TextBox_MouseLeave(object sender, EventArgs e)
        {
            borderColor = Color.FromArgb(84, 165, 213);
            this.Invalidate();
        }

        private void TextBox_FontChanged(object sender, EventArgs e)
        {
            textBox1.Font = this.Font;
        }

        private void TextBox_ForeColorChanged(object sender, EventArgs e)
        {
            textBox1.ForeColor = this.ForeColor;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (!textBox1.UseSystemPasswordChar)
            {
                if (textBox1.Text.Equals("<请输入帐号>"))
                {
                    textBox1.Text = "";
                    textBox1.ForeColor = Color.Black;
                }
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (!textBox1.UseSystemPasswordChar)
            {
                if (textBox1.Text.Equals(""))
                {
                    textBox1.Text = "<请输入帐号>";
                    textBox1.ForeColor = Color.DarkGray;
                }
            }
        }
    }
}
