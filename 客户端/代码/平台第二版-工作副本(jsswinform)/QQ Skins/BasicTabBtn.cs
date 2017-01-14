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
    public partial class BasicTabBtn : UserControl
    {
        private Graphics g = null;
        private Bitmap Bmp = null;

        public BasicTabBtn()
        {
            Bmp = ResClass.GetResObj("tabctrl_round_background");
            InitializeComponent();
        }

        [Description("文本"), Category("Appearance")]
        public string Texts
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
                this.Invalidate();
            }
        }

        private bool m_Checked = false;
        public bool Checked
        {
            get
            {
                return m_Checked;
            }
            set
            {
                m_Checked = value;
                if (m_Checked)
                    this.Show();
                else
                    this.Hide();
                this.Invalidate();
            }
        }

        private int m_left = 0;
        public int left {
            get
            {
                return m_left;
            }
            set
            {
                m_left = value;
                label1.Left = m_left + 7;
                this.Invalidate();
            } 
        }


        public int width
        {
            get
            {
                return label1.Width+13;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            if (Checked)
            {
                Bmp = ResClass.GetResObj("All_tabbutton_round_pushedPushedBkg");
                g.DrawImage(Bmp, new Rectangle(left, 0, 1, 24), 0, 0, 1, Bmp.Height, GraphicsUnit.Pixel);
                g.DrawImage(Bmp, new Rectangle(left+1, 0, label1.Width + 11, 24), 1, 0, Bmp.Width - 2, Bmp.Height, GraphicsUnit.Pixel);
                g.DrawImage(Bmp, new Rectangle(left+label1.Width + 12, 0, 1, 24), Bmp.Width - 1, 0, 1, Bmp.Height, GraphicsUnit.Pixel);
            }
            else
            {
                Bmp = ResClass.GetResObj("tabctrl_round_background");
                g.DrawImage(Bmp, new Rectangle(left, 0, label1.Width + 13, 24), 0, 0, 40, 24, GraphicsUnit.Pixel);
                Bmp = ResClass.GetResObj("tabbutton_round_AdjustColor_seperator");
                g.DrawImage(Bmp, new Rectangle(left+label1.Width + 11, 2, 2, 20), 0, 0, 2, 20, GraphicsUnit.Pixel);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!this.Checked)
            {
                Bmp = ResClass.GetResObj("tabbutton_round_AdjustColor_unpushedHighlightBkg");
                this.Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!this.Checked)
            {
                Bmp = ResClass.GetResObj("tabctrl_round_background");
                this.Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ControlCollection cct = this.Parent.Controls;
                for (int i = 0; i < cct.Count; i++)
                {
                    if (cct[i] is BasicTabBtn && (cct[i] as BasicTabBtn).Checked)
                    {
                        (cct[i] as BasicTabBtn).Checked = false;
                    }
                }
                this.Checked = true;
                this.Invalidate();
            }
        }

        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            OnMouseClick(e);
        }
    }
}
