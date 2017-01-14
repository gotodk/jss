using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;
using System.Collections;

namespace Com.Seezt.Skins
{
    public partial class BasicTabControl : UserControl
    {
        private Graphics g = null;
        private Bitmap Bmp = null;
        public BasicTabControl()
        {
            this.SetStyle(ControlStyles.ContainerControl, true);
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            Bmp = ResClass.GetResObj("tabctrl_round_background");
            g.DrawImage(Bmp, new Rectangle(0, 0, this.Width, 24), 0, 0, Bmp.Width-5, Bmp.Height, GraphicsUnit.Pixel);

            if (m_tabBtns != null)
            {
                for (int i = 0; i < m_tabBtns.Length; i++)
                {
                    if (m_tabBtns[i].Checked)
                    {
                        Bmp = ResClass.GetResObj("All_tabbutton_round_pushedPushedBkg");
                        g.DrawImage(Bmp, new Rectangle(i*66, 0, 1, 24), 0, 0, 1, Bmp.Height, GraphicsUnit.Pixel);
                        g.DrawImage(Bmp, new Rectangle(i * 66 + 1, 0, 55 + 11, 24), 1, 0, Bmp.Width - 2, Bmp.Height, GraphicsUnit.Pixel);
                        g.DrawImage(Bmp, new Rectangle(i * 66 + 55 + 12, 0, 1, 24), Bmp.Width - 1, 0, 1, Bmp.Height, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        Bmp = ResClass.GetResObj("tabctrl_round_background");
                        g.DrawImage(Bmp, new Rectangle(i * 66, 0, 55 + 13, 24), 0, 0, 40, 24, GraphicsUnit.Pixel);
                        Bmp = ResClass.GetResObj("tabbutton_round_AdjustColor_seperator");
                        g.DrawImage(Bmp, new Rectangle(i * 66 + 55 + 11, 2, 2, 20), 0, 0, 2, 20, GraphicsUnit.Pixel);
                    }
                }
            }
        }

        private BasicTabBtn[] m_tabBtns;
        [Description("TabBtns"), Category("Appearance")]
        public BasicTabBtn[] TabBtns 
        {
            get 
            {
                return m_tabBtns;
            }
            set 
            {
                m_tabBtns = value;
                this.Invalidate();
            }
        }

        private void AddTabBtn(BasicTabBtn[] tabBtns)
        {
            if (tabBtns != null)
            {
                for (int i = 0; i < tabBtns.Length; i++)
                {
                    tabBtns[i].Name = "tabBtn" + i;
                    if (i == 0)
                        tabBtns[i].left = 0;
                    else
                    {
                        int ll = (((BasicTabBtn)Controls["tabBtn" + (i - 1)]).left + ((BasicTabBtn)Controls["tabBtn" + (i - 1)]).width);
                        tabBtns[i].left = ll;
                    }
                    tabBtns[i].Left = 0;
                    tabBtns[i].Top = 0;
                    tabBtns[i].Width = this.Width;
                    tabBtns[i].Height = this.Height;
                    this.Controls.Add(tabBtns[i]);
                }
            }
        }
    }
}
