using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Com.Seezt.Skins;

namespace Com.Seezt.Skins
{
    public partial class BasicForm1 : Form
    {
        private Graphics g = null;
        private Bitmap Bmp = null;
        private Image closeBmp = null;
        private Image minBmp = null;
        private Image maxBmp = null;
        private ImageAttributes imageAttr = null;
        private Brush titleColor = Brushes.White;
        private int xwidth = 0;
        public BasicForm1()
        {
            InitializeComponent();
            this.MaximumSize = Screen.PrimaryScreen.WorkingArea.Size;
            closeBmp = ResClass.GetPNG("btn_close_normal");
            minBmp = ResClass.GetPNG("btn_mini_normal");
            maxBmp = ResClass.GetPNG("btn_max_normal");
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBmp = ResClass.GetPNG("btn_restore_normal");
            }
        }

        private void INewForm_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            Bmp = ResClass.GetResObj("All_window_windowBkg");
            imageAttr = new ImageAttributes();
            imageAttr.SetColorKey(Bmp.GetPixel(1, 1), Bmp.GetPixel(1, 1));
            g.DrawImage(Bmp, new Rectangle(0, 0, 5, 31), 0, 0, 5, 31, GraphicsUnit.Pixel, imageAttr);
            g.DrawImage(Bmp, new Rectangle(5, 0, this.Width-10, 31), 5, 0, Bmp.Width-10, 31, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 0, 5, 31), Bmp.Width-5, 0, 5, 31, GraphicsUnit.Pixel, imageAttr);
            
            g.DrawImage(Bmp, new Rectangle(0, 31, 5, 20), 0, 31, 5, 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, 31, this.Width - 10, 20), 5, 31, 5, 20, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 31, 5, 20), Bmp.Width - 5, 31, 5, 20, GraphicsUnit.Pixel);

            g.DrawImage(Bmp, new Rectangle(0, 51, 5, this.Height - 130), 0, 51, 5, 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, 51, this.Width - 10, this.Height - 130), 5, 51, 5, 10, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, 51, 5, this.Height - 130), Bmp.Width - 5, 51, 5, 10, GraphicsUnit.Pixel);

            g.DrawImage(Bmp, new Rectangle(0, this.Height - 79, 5, 45), 0, 61, 5, 45, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(5, this.Height - 79, this.Width - 10, 45), 5, 61, 5, 45, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, this.Height - 79, 5, 45), Bmp.Width - 5, 61, 5, 45, GraphicsUnit.Pixel);

            g.DrawImage(Bmp, new Rectangle(0, this.Height - 34, 5, 34), 0,Bmp.Height - 34, 5, 34, GraphicsUnit.Pixel, imageAttr);
            g.DrawImage(Bmp, new Rectangle(5, this.Height - 34, this.Width-10, 34), 5, Bmp.Height - 34, Bmp.Width-10, 34, GraphicsUnit.Pixel);
            g.DrawImage(Bmp, new Rectangle(this.Width - 5, this.Height - 34, 5, 34), Bmp.Width - 5, Bmp.Height - 34, 5, 34, GraphicsUnit.Pixel, imageAttr);
            g.DrawIcon(this.Icon, new Rectangle(7, 7, 18, 18));
            g.DrawString(this.Text, new Font("宋体", 10F, FontStyle.Bold), titleColor, 26, 5);
        }

        private void INewForm_Resize(object sender, EventArgs e)
        {
            if (this.ControlBox)
            {
                this.ButtonClose.Left = this.Width - ButtonClose.Width - 2;
            }

            if (this.MinimizeBox)
            {
                if (this.MaximizeBox)
                {
                    this.ButtonMax.Left = this.Width - ButtonClose.Width - ButtonMax.Width;
                    this.ButtonMin.Left = this.Width - ButtonClose.Width - ButtonMax.Width - ButtonMin.Width;
                    xwidth = this.Width - ButtonClose.Width - ButtonMax.Width - ButtonMin.Width;
                }
                else
                {
                    this.ButtonMax.Enabled = false;
                    xwidth = this.Width - ButtonClose.Width - ButtonMin.Width;
                    this.ButtonMin.Left = this.Width - ButtonClose.Width - ButtonMin.Width;
                }
            }
            else {

                xwidth = this.Width - ButtonClose.Width;
            }
            this.Invalidate();
        }

        private void ButtonClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.Close();
        }

        private void ButtonClose_MouseDown(object sender, MouseEventArgs e)
        {
            closeBmp = ResClass.GetResObj("btn_close_down");
            ButtonClose.Invalidate();
        }

        private void ButtonClose_MouseUp(object sender, MouseEventArgs e)
        {
            if (!ButtonClose.IsDisposed)
            {
                closeBmp = ResClass.GetResObj("btn_close_normal");
                ButtonClose.Invalidate();
            }
        }

        private void ButtonClose_MouseLeave(object sender, EventArgs e)
        {
            closeBmp = ResClass.GetResObj("btn_close_normal");
            ButtonClose.Invalidate();
        }

        private void ButtonClose_MouseEnter(object sender, EventArgs e)
        {
            closeBmp = ResClass.GetResObj("btn_close_highlight");
            toolTip1.SetToolTip(ButtonClose, "关闭");
            ButtonClose.Invalidate();
        }

        private void ButtonMin_MouseEnter(object sender, EventArgs e)
        {
            minBmp = ResClass.GetResObj("btn_mini_highlight");
            toolTip1.SetToolTip(ButtonMin, "最小化");
            ButtonMin.Invalidate();
        }

        private void ButtonMin_MouseDown(object sender, MouseEventArgs e)
        {
            minBmp = ResClass.GetResObj("btn_mini_down");
            ButtonMin.Invalidate();
        }

        private void ButtonMin_MouseUp(object sender, MouseEventArgs e)
        {
            minBmp = ResClass.GetResObj("btn_close_normal");
            ButtonMin.Invalidate();
        }

        private void ButtonMin_MouseLeave(object sender, EventArgs e)
        {
            minBmp = ResClass.GetResObj("btn_mini_normal");
            ButtonMin.Invalidate();
        }

        private void ButtonMin_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                this.WindowState = FormWindowState.Minimized;
        }

        private void IForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //if (e.Y <= 31 && e.X < xwidth)
                //{
                    System.Windows.Forms.Control.ControlCollection cce = this.Controls;
                    for (int i = 0; i < cce.Count; i++)
                    {
                        if (cce[i] is BasicComboBox && !cce[i].Name.Equals(this.Name))
                            (cce[i] as BasicComboBox).CloseListPanel();
                    }
                    if (e.Clicks >= 2 && this.MaximizeBox)
                    {
                        ButtonMax_MouseClick(null, new MouseEventArgs(MouseButtons.Left, 1, 2, 2, 0));
                    }
                    else
                    {
                        BasicForm.ReleaseCapture();
                        BasicForm.SendMessage(Handle, 0x00A1, 2, 0);
                    }
                //}
            }
        }

        private void ButtonMin_Paint(object sender, PaintEventArgs e)
        {
            if (this.MinimizeBox)
            {
                g = e.Graphics;
                g.DrawImage(minBmp, new Rectangle(0, 0, minBmp.Width, minBmp.Height), 0, 0, minBmp.Width, minBmp.Height, GraphicsUnit.Pixel, imageAttr);
            }
        }

        private void ButtonClose_Paint(object sender, PaintEventArgs e)
        {
            if (this.ControlBox)
            {
                g = e.Graphics;
                g.DrawImage(closeBmp, new Rectangle(0, 0, closeBmp.Width, closeBmp.Height), 0, 0, closeBmp.Width, closeBmp.Height, GraphicsUnit.Pixel, imageAttr);
            }
        }

        private void ButtonMax_MouseDown(object sender, MouseEventArgs e)
        {
            g = ButtonMax.CreateGraphics();
            maxBmp = ResClass.GetResObj("btn_max_down");
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBmp = ResClass.GetResObj("btn_restore_down");
            }
            g.DrawImage(maxBmp, new Rectangle(0, 0, maxBmp.Width, maxBmp.Height), 0, 0, maxBmp.Width, maxBmp.Height, GraphicsUnit.Pixel);
        }

        private void ButtonMax_MouseEnter(object sender, EventArgs e)
        {
            g = ButtonMax.CreateGraphics();
            maxBmp = ResClass.GetResObj("btn_max_highlight");
            toolTip1.SetToolTip(ButtonMax, "最大化");
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBmp = ResClass.GetResObj("btn_restore_highlight");
                toolTip1.SetToolTip(ButtonMax, "还原");
            }
            g.DrawImage(maxBmp, new Rectangle(0, 0, maxBmp.Width, maxBmp.Height), 0, 0, maxBmp.Width, maxBmp.Height, GraphicsUnit.Pixel);
        }

        private void ButtonMax_MouseLeave(object sender, EventArgs e)
        {
            maxBmp = ResClass.GetResObj("btn_max_normal");
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBmp = ResClass.GetResObj("btn_restore_normal");
            }
            ButtonMax.Invalidate();
        }

        private void ButtonMax_MouseUp(object sender, MouseEventArgs e)
        {
            maxBmp = ResClass.GetResObj("btn_max_normal");
            if (this.WindowState == FormWindowState.Maximized)
            {
                maxBmp = ResClass.GetResObj("btn_restore_normal");
            }
            ButtonMax.Invalidate();
        }

        private void ButtonMax_Paint(object sender, PaintEventArgs e)
        {
            if (this.MaximizeBox)
            {
                g = e.Graphics;
                maxBmp = ResClass.GetResObj("btn_max_normal");
                if (this.WindowState == FormWindowState.Maximized)
                {
                    maxBmp = ResClass.GetResObj("btn_restore_normal");
                }
                g.DrawImage(maxBmp, new Rectangle(0, 0, maxBmp.Width, maxBmp.Height), 0, 0, maxBmp.Width, maxBmp.Height, GraphicsUnit.Pixel);
            }
        }

        private void IForm_Load(object sender, EventArgs e)
        {
            if (this.ControlBox)
            {
                this.ButtonClose.Visible = true;
                if (!this.MinimizeBox)
                    this.ButtonMin.Visible = false;
                else
                    this.ButtonMin.Visible = true;
                if (!this.MaximizeBox)
                    this.ButtonMax.Visible = false;
                else
                    this.ButtonMax.Visible = true;
            }
            else
            {
                this.ButtonClose.Visible = false;
                this.ButtonMin.Visible = false;
                this.ButtonMax.Visible = false;
            }
        }

        private void IForm_TextChanged(object sender, EventArgs e)
        {
            this.Invalidate(new Rectangle(26, 7,300,31));
        }

        private void IForm_Activated(object sender, EventArgs e)
        {
            titleColor = Brushes.White;
            this.Invalidate(new Rectangle(26, 7, 300, 31));
        }

        private void IForm_Deactivate(object sender, EventArgs e)
        {
            titleColor = Brushes.LightGray;
            this.Invalidate(new Rectangle(26, 7, 300, 31));
        }

        private void ButtonMax_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.WindowState==FormWindowState.Normal)
                    this.WindowState = FormWindowState.Maximized;
                else
                    this.WindowState = FormWindowState.Normal;
            }
        }
    }
}
