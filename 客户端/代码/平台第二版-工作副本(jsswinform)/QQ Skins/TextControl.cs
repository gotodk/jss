using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Com.Seezt.Skins
{
    public partial class TextControl : UserControl
    {
        private Graphics g = null;

        private ArrayList text = new ArrayList();
        public TextControl()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
        }

        private void TextPanel_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            for (int i = 0; i < Texts.Count; i++)
            {
                g.DrawString(Texts[i].ToString(), new Font("宋体", 9F), Brushes.Black, 10, i*20);
            }
        }

        [Description("文本"), Category("Appearance")]
        public ArrayList Texts
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                this.Invalidate();
            }
        }

        public void AppendText(string text) 
        {
            Texts.Add(text);
            this.Invalidate();
        }
    }
}
