using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com.Seezt.Skins;

namespace Com.Seezt.Skins
{
    public partial class BasicControl : UserControl
    {
        private Graphics g = null;
        private Bitmap Bmp = null;
        private Bitmap closeBmp = null;

        public BasicControl()
        {
            InitializeComponent();
            closeBmp = ResClass.GetResObj("Button_colse_normal");
        }

        private string texts;
        public string Texts 
        {
            get 
            {
                return texts;
            }
            set 
            {
                texts = value;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            g = e.Graphics;
            Bmp = ResClass.GetResObj("All_window_windowBkg1");

            g.DrawImage(Bmp, new Rectangle(0, 0, 5, 31), 0, 0, 5, 31, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, 0, this.Width-10, 31), 5, 0, Bmp.Width-10, 31, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 0, 5, 31), Bmp.Width-5, 0, 5, 31, GraphicsUnit.Pixel);
            
            g.DrawImage(Bmp, new Rectangle(0, 31, 5, 20), 0, 31, 5, 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, 31, this.Width - 10, 20), 5, 31, 5, 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 31, 5, 20), Bmp.Width - 5, 31, 5, 20, GraphicsUnit.Pixel);

            g.DrawImage(Bmp, new Rectangle(0, 51, 5, this.Height - 130), 0, 51, 5, 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, 51, this.Width - 10, this.Height - 130), 5, 51, 5, 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 51, 5, this.Height - 130), Bmp.Width - 5, 51, 5, 10, GraphicsUnit.Pixel);

            g.DrawImage(Bmp, new Rectangle(0, this.Height - 79, 5, 45), 0, 61, 5, 45, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, this.Height - 79, this.Width - 10, 45), 5, 61, 5, 45, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, this.Height - 79, 5, 45), Bmp.Width - 5, 61, 5, 45, GraphicsUnit.Pixel);

            g.DrawImage(Bmp, new Rectangle(0, this.Height - 34, 5, 34), 0,Bmp.Height - 34, 5, 34, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, this.Height - 34, this.Width-10, 34), 5, Bmp.Height - 34, Bmp.Width-10, 34, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, this.Height - 34, 5, 34), Bmp.Width - 5, Bmp.Height - 34, 5, 34, GraphicsUnit.Pixel);
            g.DrawString(this.Texts, new Font("宋体", 10F, FontStyle.Bold), Brushes.White, 26, 5);
        }

        private void INewForm_Resize(object sender, EventArgs e)
        {

            this.ButtonClose.Left = this.Width - ButtonClose.Width;
     
            //this.Invalidate();
        }

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.Dispose(true);
        }

        private void ButtonClose_MouseDown(object sender, MouseEventArgs e)
        {
            closeBmp = ResClass.GetResObj("Button_close_pushedBackground");
            ButtonClose.Invalidate();
        }

        private void ButtonClose_MouseUp(object sender, MouseEventArgs e)
        {
            if (!ButtonClose.IsDisposed)
            {
                closeBmp = ResClass.GetResObj("Button_colse_normal");
                ButtonClose.Invalidate();
            }
        }

        private void ButtonClose_MouseLeave(object sender, EventArgs e)
        {
            closeBmp = ResClass.GetResObj("Button_colse_normal");
            ButtonClose.Invalidate();
        }

        private void ButtonClose_MouseEnter(object sender, EventArgs e)
        {
            closeBmp = ResClass.GetResObj("Button_close_highlightBackground");
            toolTip1.SetToolTip(ButtonClose, "关闭");
            ButtonClose.Invalidate();
        }

        private void ButtonClose_Paint(object sender, PaintEventArgs e)
        {
                g = e.Graphics;
                g.DrawImage(closeBmp, new Rectangle(0, 0, closeBmp.Width, closeBmp.Height), 0, 0, closeBmp.Width, closeBmp.Height, GraphicsUnit.Pixel);
        }

        private void IForm_Load(object sender, EventArgs e)
        {

                this.ButtonClose.Visible = true;
          
        }

        private void IForm_TextChanged(object sender, EventArgs e)
        {
            this.Invalidate(new Rectangle(26, 7,300,31));
        }
   }
}
