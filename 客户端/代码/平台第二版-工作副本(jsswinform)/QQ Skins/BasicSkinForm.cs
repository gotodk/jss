using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Com.Seezt.Skins
{
    public partial class BasicSkinForm : Form
    {
        private BasicForm form = null;
        public BasicSkinForm(BasicForm _form)
        {
            form = _form;
            InitializeComponent();
        }

        private void basicPictureBox1_Click(object sender, EventArgs e)
        {
            BasicPictureBox pic = sender as BasicPictureBox;
            switch (pic.Texts)
            {
                case "蓝":
                    trackBar1.Value = -20;
                    trackBar2.Value = 0;
                    trackBar3.Value = 0;
                    break;
                case "黄":
                    trackBar1.Value = -130;
                    trackBar2.Value = -5;
                    trackBar3.Value = 40;
                    break;
                case "橙":
                    trackBar1.Value = -175;
                    trackBar2.Value = 0;
                    trackBar3.Value = 0;
                    break;
                case "绿":
                    trackBar1.Value = -60;
                    trackBar2.Value = -10;
                    trackBar3.Value = 0;
                    break;
                case "黑":
                    trackBar1.Value = 50;
                    trackBar2.Value = -50;
                    trackBar3.Value = -180;
                    break;
                case "紫":
                    trackBar1.Value = 100;
                    trackBar2.Value = -20;
                    trackBar3.Value = 0;
                    break;
                case "红":
                    trackBar1.Value = -185;
                    trackBar2.Value = -5;
                    trackBar3.Value = 10;
                    break;
                case "透明":
                    trackBar1.Value = 0;
                    trackBar2.Value = 0;
                    trackBar3.Value = 0;
                    break;
                case "默认":
                    trackBar1.Value = 0;
                    trackBar2.Value = 0;
                    trackBar3.Value = 0;
                    break;
            }
            trackBar1_Scroll(null, null);
            trackBar2_Scroll(null, null);
            trackBar3_Scroll(null, null);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            form.colorA = trackBar1.Value;
            form.Invalidate();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            form.lightB = trackBar2.Value;
            form.Invalidate();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            form.grayC = trackBar3.Value;
            form.Invalidate();
        }

        private void BasicSkinForm_Deactivate(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
