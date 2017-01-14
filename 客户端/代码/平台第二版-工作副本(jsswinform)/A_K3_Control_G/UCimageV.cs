using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ImageViewKJ
{
    public partial class UCimageV : UserControl
    {
        /// <summary>
        /// 移动图片时的鼠标起点坐标
        /// </summary>
        private Point StartP = new Point(0, 0);
        /// <summary>
        /// 在鼠标移动时判断鼠标是否被按下
        /// </summary>
        private bool isMouseDown = false;
        /// <summary>
        /// 在调整控件大小时记录调整前panel的尺寸
        /// </summary>
        private Point panelOldSize = new Point(0, 0);
        /// <summary>
        /// 图片缩放比例的1000倍（提高精度）
        /// </summary>
        private int imgIndexBy1000 = 0;
        /// <summary>
        /// 判断按下了那个按键
        /// 0：没有按键；
        /// 1：按下Ctrl；
        /// 2：按下Shift；
        /// </summary>
        private int keyValue=0;
        /// <summary>
        /// 是否利用鼠标移动图片
        /// </summary>
        private bool isMoveValue = false;
        [Browsable(true), Category("动作")]
        public bool isMove
        {
            get
            {
                return isMoveValue;
            }
            set
            {
                isMoveValue = value;
            }
        }
        /// <summary>
        /// 所要预览的图片
        /// </summary>
        private Image imageResource;
        [Browsable(true), Category("图片")]
        public Image image
        {
            get
            {
                return imageResource;
            }
            set
            {
                imageResource = value;
                try
                {
                    int a = imageResource.Height;
                    getImage(value,"");
                }
                catch (Exception ex)
                { }
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public UCimageV()
        {
            InitializeComponent();
            //初始化界面布局：
            panel1.Size = this.Size;
            hScrollBar1.Maximum = 0;
            hScrollBar1.Visible = false;
            vScrollBar1.Maximum = 0;
            vScrollBar1.Visible = false;
            //记录panel1的Size，imageBox的Size和Location
            panelOldSize.X = panel1.Width;
            panelOldSize.Y = panel1.Height;
        }

        public void Set1bi1()
        {
            ;
        }

        /**************************************************************
         * 自定义的三个函数函数，分别用于：
         * 获取图片：getImage(Image img)；
         * 图片缩放：zoom(Point center, int zoomIndexBy1000)；
         * 图片移动：moveImage(int dx, int dy)。
         **************************************************************/
        /// <summary>
        /// 用于获取和设置控件中的图片，并调整相应的滚动条
        /// </summary>
        /// <param name="img">控件中所要加载的图片</param>
        public void getImage(Image img,string sp)
        {
            if (img == null)
            { return; }
            imageBox.Image = img;
            //设定图片位置
            imageBox.Location = new Point(0, 0);
            //设定图片纵横比
            imgIndexBy1000 = (imageBox.Image.Height * 1000) / imageBox.Image.Width;
            //设定图片初始尺寸
            int defaultH = 0;
            if (image.Height >= this.Height)
            {
                defaultH = this.Height - 20;
            }
            else
            {
                defaultH = image.Height;
            }
            int defaultW = 0;
            if (image.Width >= this.Width)
            {
                defaultW = this.Width - 20;
            }
            else
            {
                defaultW = image.Width;
            }

            if (sp == "1b1")
            {
                defaultH = image.Height;
                defaultW = image.Width;
            }

            if ((double)image.Height / (double)image.Width > (double)panel1.Height / (double)panel1.Width)
            {

                imageBox.Height = defaultH;
                imageBox.Width = (imageBox.Height * 1000) / imgIndexBy1000;
                imageBox.Left = (panel1.Width - imageBox.Width) / 2;
            }
            else
            {
                imageBox.Width = defaultW;
                imageBox.Height = (imageBox.Width * imgIndexBy1000) / 1000;
                imageBox.Top = (panel1.Height - imageBox.Height) / 2;
            }
            //设置属性
            imageResource = img;

            ImageView_Resize(null,null);
        }
        /// <summary>
        /// 图片缩放
        /// </summary>
        /// <param name="center">缩放中心点</param>
        /// <param name="zoomIndexBy1000">缩放倍率的1000倍</param>
        public void zoom(Point center, int zoomIndexBy1000)
        {
            //记录原始的imageBox的Size
            Point oldSize = new Point(imageBox.Width, imageBox.Height);
            //实施放大（以x方向为基准计算得出y方向大小，防止多次运算误差积累使Image和imageBox的尺寸不匹配）
            imageBox.Width = imageBox.Width * zoomIndexBy1000 / 1000;
            imageBox.Height = imageBox.Width * imgIndexBy1000 / 1000;
            //重新定位标定后的imageBox位置
            Point newLoc = new Point(imageBox.Left,imageBox.Top);
            if (imageBox.Width > panel1.Width)
            {
                newLoc.X -= ((imageBox.Width - oldSize.X) * (center.X * 1000 / oldSize.X)) / 1000;
                if (newLoc.X > 0)
                    newLoc.X = 0;
            }
            else
                newLoc.X = (panel1.Width - imageBox.Width) / 2;

            if (imageBox.Height > panel1.Height)
            {
                newLoc.Y -= ((imageBox.Height - oldSize.Y) * (center.Y * 1000 / oldSize.Y)) / 1000;
                if (newLoc.Y > 0)
                    newLoc.Y = 0;
            }
            else
                newLoc.Y = (panel1.Height - imageBox.Height) / 2;

            //重新设定横向滚动条最大值和位置
            if (imageBox.Width - panel1.Width > 0)
            {
                //调整滚动条的显示和属性
                hScrollBar1.Visible = true;
                hScrollBar1.Maximum = imageBox.Width - panel1.Width + 2;
                hScrollBar1.Value = (newLoc.X >= 0 ? 0 : (-newLoc.X > hScrollBar1.Maximum ? hScrollBar1.Maximum : -newLoc.X));
            }
            else
            {
                //把图片定位到该方向的中央
                imageBox.Left = (panel1.Width - imageBox.Width) / 2;
                //调整滚动条的显示和属性
                hScrollBar1.Visible = false;
            }
            //重新设定纵向滚动条最大值和位置
            if (imageBox.Height - panel1.Height > 0)
            {
                //调整滚动条的显示和属性
                vScrollBar1.Visible = true;
                vScrollBar1.Maximum = imageBox.Height - panel1.Height + 2;
                vScrollBar1.Value = (newLoc.Y >= 0 ? 0 : (-newLoc.Y > vScrollBar1.Maximum ? vScrollBar1.Maximum : -newLoc.Y));
            }
            else
            {
                //把图片定位到该方向的中央
                imageBox.Top = (panel1.Height - imageBox.Height) / 2;
                //调整滚动条的显示和属性
                vScrollBar1.Visible = false;
            }
        }
        /// <summary>
        /// 移动图片
        /// </summary>
        /// <param name="x">x方向移动量</param>
        /// <param name="y">y方向移动量</param>
        private void moveImage(int dx, int dy)
        {
            int x = hScrollBar1.Value + dx;
            int y = vScrollBar1.Value + dy;
            if (imageBox.Width > panel1.Width)//只有Image横向比panel大时才可以移动。
            {
                if (x >= 0 && x <= hScrollBar1.Maximum)
                {
                    hScrollBar1.Value = x;
                }
                else
                {
                    hScrollBar1.Value = (x < 0 ? 0 : hScrollBar1.Maximum);
                }
            }

            if (imageBox.Height > panel1.Height)//只有Image纵向比panel大时才可以移动。
            {
                if (y >= 0 && y <= vScrollBar1.Maximum)
                {
                    vScrollBar1.Value = y;
                }
                else
                {
                    vScrollBar1.Value = (y < 0 ? 0 : vScrollBar1.Maximum);
                }
            }
        }
        /**************************************************************
         * 检测图片移动的条件，在符合条件情况下调用移动图片的函数
         **************************************************************/
        private void imageBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove && isMouseDown)
            {
                moveImage(StartP.X - e.X, StartP.Y - e.Y);
            }
        }
        /**************************************************************
         * 记录鼠标按下时鼠标在ImageBox中的坐标，
         * 同时设置鼠标正处于按下状态
         **************************************************************/
        private void imageBox_MouseDown(object sender, MouseEventArgs e)
        {
            StartP = e.Location;
            isMouseDown = true;
        }
        /**************************************************************
         * 当鼠标移入图片区域时，把图片设置为操作焦点；
         * 同时检测当前是否允许用鼠标拖拽来移动图片：
         * 若允许，则将鼠标设置为手型，否则默认鼠标形状
         **************************************************************/
        private void imageBox_MouseEnter(object sender, EventArgs e)
        {
            imageBox.Focus();
            if (isMove && (imageBox.Width > panel1.Width || imageBox.Height > panel1.Height))
                imageBox.Cursor = Cursors.Hand;
            else
                imageBox.Cursor = Cursors.Hand;
        }
        /**************************************************************
         * 将鼠标形状设置为默认的形状
         **************************************************************/
        private void imageBox_MouseLeave(object sender, EventArgs e)
        {
            imageBox.Cursor = Cursors.Hand;
        }
        /**************************************************************
         * 设置鼠标没有处于按下状态
         **************************************************************/
        private void imageBox_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
        /************************************************************
         * 将某键按下的指示清除
         ************************************************************/
        private void imageBox_KeyUp(object sender,KeyEventArgs e)
        {
            keyValue = 0;
        }
        /************************************************************
         * 根据按键设置keyValue的值，在鼠标滚轮的事件里要用到。
         ************************************************************/
        private void imageBox_KeyDown(object sender,KeyEventArgs e)
        {
            if (e.Control)
                keyValue = 1;
            else if (e.Shift)
                keyValue = 2;
            else
                keyValue = 0;
        }
        /************************************************************
         * 滚动鼠标滚轮实现鼠标缩放
         ************************************************************/
        private void imageBox_MouseWheel(object sender, MouseEventArgs e)
        {
            switch (keyValue)
            {
                case 0:
                    if (e.Delta > 0 && imageBox.Width < 10000 && imageBox.Height < 10000)
                    {
                        zoom(e.Location, 1100);
                    }
                    else if (e.Delta < 0 && imageBox.Width > 20 && imageBox.Height > 20)
                    {
                        zoom(e.Location, 900);
                    }
                    break;
                case 2:
                    moveImage(-e.Delta, 0);
                    break;
                default:
                    //moveImage(0, -e.Delta);
                    break;
            }
        }
        /************************************************************
         * 横向滚动条数值（Value）改变时，图片移动
         ************************************************************/
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            imageBox.Left = -hScrollBar1.Value;
        }
        /************************************************************
         * 纵向滚动条数值（Value）改变时，图片移动
         ************************************************************/
        private void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            imageBox.Top = -vScrollBar1.Value;
        }
        /************************************************************
         * 横向滚动条可见性（Visible）改变时，设置panel纵向尺寸
         ************************************************************/
        private void hScrollBar1_VisibleChanged(object sender, EventArgs e)
        {

            if (hScrollBar1.Visible)
                panel1.Height = this.Height - hScrollBar1.Height;
            else
                panel1.Height = this.Height;
        }
        /************************************************************
         * 纵向滚动条可见性（Visible）改变时，设置panel横向尺寸
         ************************************************************/
        private void vScrollBar1_VisibleChanged(object sender, EventArgs e)
        {
            if (vScrollBar1.Visible)
                panel1.Width = this.Width - vScrollBar1.Width;
            else
                panel1.Width = this.Width;
        }
        /************************************************************
         * 控件尺寸改变时图像的显示位置控制
         ************************************************************/
        private void ImageView_Resize(object sender, EventArgs e)
        {
     
            //int xIndex = panel1.Width * 1000 / panelOldSize.X;
            //int yIndex = panel1.Height * 1000 / panelOldSize.Y;

            //设定滚动条(x、y方向分开设置)
            if (imageBox.Width > panel1.Width)
            {
                hScrollBar1.Maximum = imageBox.Width - panel1.Width;
                int newv = -(imageBox.Left + (panel1.Width - panelOldSize.X) / 2);
                if (newv < 0)
                {
                    newv = 0;
                }
                if (newv > hScrollBar1.Maximum)
                {
                    hScrollBar1.Maximum = newv;
                }
                hScrollBar1.Value = newv;
                hScrollBar1.Visible = true;
            }
            else
            {
                hScrollBar1.Value = 0;
                imageBox.Left = (panel1.Width - imageBox.Width) / 2;
                hScrollBar1.Maximum = 0;
                hScrollBar1.Visible = false;
            }
            if (imageBox.Height > panel1.Height)
            {
                vScrollBar1.Maximum = imageBox.Height - panel1.Height;
                int newv = -(imageBox.Top + (panel1.Height - panelOldSize.Y) / 2);
                if (newv < 0)
                {
                    newv = 0;
                }
                if (newv > vScrollBar1.Maximum)
                {
                    vScrollBar1.Maximum = newv;
                }
                vScrollBar1.Value = newv;
                vScrollBar1.Visible = true;
            }
            else
            {
                vScrollBar1.Value = 0;
                imageBox.Top = (panel1.Height - imageBox.Height) / 2;
                vScrollBar1.Maximum = 0;
                vScrollBar1.Visible = false;
            }
            panelOldSize =new Point( panel1.Size);
        }

    }
}
