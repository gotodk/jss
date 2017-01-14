using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Com.Seezt.Skins
{
    public partial class BasicRichTextBox : TextBox
    {
        public BasicRichTextBox()
        {
            InitializeComponent();
            this.Multiline = true;
        }

        private Image _BgImage = null;

        [Description("背景"), Category("Appearance")]
        public Image BgImage
        {
            get
            {
                return _BgImage;
            }
            set
            {
                _BgImage = value;
                this.Invalidate();
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x000f:
                    OnPaint(this.CreateGraphics());
                    m.Result = (IntPtr)1;
                    break;
                case 0x0014:
                    //OnPaint(this.CreateGraphics());
                    //m.Result = (IntPtr)1;
                    break;
            }
            base.WndProc(ref m);
        }

        protected void OnPaint(Graphics e)
        {
            Graphics g = e;
            if (BackgroundImage != null)
            {
                Bitmap Bmp = new Bitmap(BgImage);
                g.Clear(Color.White);
                g.DrawImage(Bmp, new Rectangle(0, 0, this.Width, this.Height), 0, 0, Bmp.Width, Bmp.Height, GraphicsUnit.Pixel);
            }
            g.Dispose();
        }
    }
}
