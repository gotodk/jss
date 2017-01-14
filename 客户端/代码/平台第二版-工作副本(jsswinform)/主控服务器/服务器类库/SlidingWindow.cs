using System;
using System.Collections.Generic;
using System.Text;

namespace 灵魂裸奔服务器端.服务器类库
{
    /// <summary>
    /// 虚拟滑动窗口
    /// </summary>
    public class SlidingWindow
    {
        /// <summary>
        /// 窗口允许滑动的最大次数,即最大可滑动几次
        /// </summary>
        private int maxSize;
        /// <summary>
        /// 窗口被滑动的大小
        /// </summary>
        private int usedSize = 0;
        /// <summary>
        /// 滑动窗口位置标记
        /// </summary>
        private int windowHead = 0;
        /// <summary>
        /// 窗口开始打开时间
        /// </summary>
        public DateTime beginTime;
        /// <summary>
        /// 窗口最后激活时间
        /// </summary>
        public DateTime lastActiveTime;

        /// <summary>
        /// 构造函数，设置打开和最后激活时间以及最大重发次数
        /// </summary>
        public SlidingWindow()
        {
            beginTime = DateTime.Now;
            lastActiveTime = beginTime;
            this.maxSize = 5;
        }

        /// <summary>
        /// 构造函数重载，设置打开和最后激活时间以及最大重发次数
        /// </summary>
        /// <param name="m"></param>
        public SlidingWindow(int m)
        {
            beginTime = DateTime.Now;
            lastActiveTime = beginTime;
            this.maxSize = m;
        }

        /// <summary>
        /// 窗口向前移动
        /// </summary>
        public void moveAhead()
        {
            this.windowHead++;
            if (this.usedSize < this.maxSize) //累加使用次数
            {
                this.usedSize++;
            }
            this.lastActiveTime = DateTime.Now;
        }

        /// <summary>
        /// 发送完毕后窗口回到原位
        /// </summary>
        public void resetWindowHead()
        {
            this.windowHead = 0;
        }

        /// <summary>
        /// 窗口向后移动
        /// </summary>
        public void takeOne()
        {
            if (this.usedSize > 0) //减少使用次数
            {
                this.usedSize--;
            }
            this.lastActiveTime = DateTime.Now;
        }

        /// <summary>
        /// 得到窗口允许滑动的最大次数
        /// </summary>
        /// <returns></returns>
        public int getMaxSize()
        {
            return this.maxSize;
        }

        /// <summary>
        /// 得到窗口使用次数
        /// </summary>
        /// <returns></returns>
        public int getUsedSize()
        {
            return this.usedSize;
        }

        /// <summary>
        /// 得到窗口位置
        /// </summary>
        /// <returns></returns>
        public int getWindowHead()
        {
            return this.windowHead;
        }
    }
}
