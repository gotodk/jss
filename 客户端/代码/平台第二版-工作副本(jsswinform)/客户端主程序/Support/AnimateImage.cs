﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace 客户端主程序.Support
{
    /// <summary>   
    /// 表示一类带动画功能的图像。   
    /// </summary>   
    public class AnimateImage
    {
        Image image;
        FrameDimension frameDimension;
        /// <summary>   
        /// 动画当前帧发生改变时触发。   
        /// </summary>   
        public event EventHandler<EventArgs> OnFrameChanged;

        /// <summary>   
        /// 实例化一个AnimateImage。   
        /// </summary>   
        /// <param name="img">动画图片。</param>   
        public AnimateImage(Image img)
        {
            image = img;

            lock (image)
            {
                mCanAnimate = ImageAnimator.CanAnimate(image);
                if (mCanAnimate)
                {
                    Guid[] guid = image.FrameDimensionsList;
                    frameDimension = new FrameDimension(guid[0]);
                    mFrameCount = image.GetFrameCount(frameDimension);
                }
            }
        }

        bool mCanAnimate;
        int mFrameCount = 1, mCurrentFrame = 0;

        /// <summary>   
        /// 图片。   
        /// </summary>   
        public Image Image
        {
            get { return image; }
        }

        /// <summary>   
        /// 是否动画。   
        /// </summary>   
        public bool CanAnimate
        {
            get { return mCanAnimate; }
        }

        /// <summary>   
        /// 总帧数。   
        /// </summary>   
        public int FrameCount
        {
            get { return mFrameCount; }
        }

        /// <summary>   
        /// 播放的当前帧。   
        /// </summary>   
        public int CurrentFrame
        {
            get { return mCurrentFrame; }
        }

        /// <summary>   
        /// 播放这个动画。   
        /// </summary>   
        public void Play()
        {
            if (mCanAnimate)
            {
                lock (image)
                {
                    ImageAnimator.Animate(image, new EventHandler(FrameChanged));
                }
            }
        }

        /// <summary>   
        /// 停止播放。   
        /// </summary>   
        public void Stop()
        {
            if (mCanAnimate)
            {
                lock (image)
                {
                    ImageAnimator.StopAnimate(image, new EventHandler(FrameChanged));
                }
            }
        }

        /// <summary>   
        /// 重置动画，使之停止在第0帧位置上。   
        /// </summary>   
        public void Reset()
        {
            if (mCanAnimate)
            {
                ImageAnimator.StopAnimate(image, new EventHandler(FrameChanged));
                lock (image)
                {
                    image.SelectActiveFrame(frameDimension, 0);
                    mCurrentFrame = 0;
                }
            }
        }

        private void FrameChanged(object sender, EventArgs e)
        {
            mCurrentFrame = mCurrentFrame + 1 >= mFrameCount ? 0 : mCurrentFrame + 1;
            lock (image)
            {
                image.SelectActiveFrame(frameDimension, mCurrentFrame);
            }
            if (OnFrameChanged != null)
            {
                OnFrameChanged(image, e);
            }
        }




        /*
         * 如何调用。。。
         AnimateImage image;   
  
        public Form1()   
        {   
            InitializeComponent();   
            image = new AnimateImage(Image.FromFile(@"C:\Documents and Settings\Administrator\My Documents\My Pictures\未命名.gif"));   
            image.OnFrameChanged += new EventHandler<EventArgs>(image_OnFrameChanged);   
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);   
        }   
  
        void image_OnFrameChanged(object sender, EventArgs e)   
        {   
            Invalidate();   
        }   
  
        private void Form1_Load(object sender, EventArgs e)   
        {   
            image.Play();   
        }   
  
        private void Form1_Paint(object sender, PaintEventArgs e)   
        {   
            lock (image.Image)   
            {   
                e.Graphics.DrawImage(image.Image, new Point(0, 0));   
            }   
        }   
  
        private void button1_Click(object sender, EventArgs e)   
        {   
            if (button1.Text.Equals("Stop"))   
            {   
                image.Stop();   
                button1.Text = "Play";   
            }   
            else  
            {   
                image.Play();   
                button1.Text = "Stop";   
            }   
            Invalidate();   
        }   
  
        private void button2_Click(object sender, EventArgs e)   
        {   
            image.Reset();   
            button1.Text = "Play";   
            Invalidate();   
        }   
    }   

         * 
         * 
        */



    }

}
