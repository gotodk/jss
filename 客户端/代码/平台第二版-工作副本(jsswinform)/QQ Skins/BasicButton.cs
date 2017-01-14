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
    [DefaultEvent("Click")]
    public partial class BasicButton : UserControl
    {
        private Graphics g = null;
        private Bitmap Bmp = null;
        private string m_buttonText = "Button";
        public BasicButton()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            InitializeComponent();
            Bmp = ResClass.GetResObj("btn_normal");
        }

        [Description("文本"), Category("Appearance")]
        public string Texts
        {
            get
            {
                return m_buttonText;
            }
            set
            {
                m_buttonText = value;
                this.Invalidate();
            }
        }

        private PointF GetPointF(string buttonText)
        {
            float x, y;
            switch (buttonText.Length)
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
            return new PointF(x,y);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawImage(Bmp, new Rectangle(0, 0, this.Width, this.Height), 0, 0, Bmp.Width, Bmp.Height, GraphicsUnit.Pixel);
            PointF point = GetPointF(this.m_buttonText);
            if (this.Enabled)
                g.DrawString(this.Texts, new Font("宋体", 9F), Brushes.Black, point);
            else
                g.DrawString(this.Texts, new Font("宋体", 9F), Brushes.Gray, point);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            Bmp = ResClass.GetResObj("btn_down");
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            OnMouseLeave(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Bmp = ResClass.GetResObj("btn_highlight");
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if(Focused)
                Bmp = ResClass.GetResObj("btn_focus");
            else
                Bmp = ResClass.GetResObj("btn_normal");
            this.Invalidate();
        }

        protected override void OnEnter(EventArgs e)
        {
            Bmp = ResClass.GetResObj("btn_focus");
            this.Invalidate();
        }

        protected override void OnLeave(EventArgs e)
        {
            Bmp = ResClass.GetResObj("btn_normal");
            this.Invalidate();
        }
    }
}
