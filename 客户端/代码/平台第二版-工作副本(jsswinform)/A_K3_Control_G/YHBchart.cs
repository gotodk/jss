using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Collections;

namespace 客户端主程序
{
    public partial class YHBchart : UserControl
    {


        //设置XY轴的刻度文字的字体样式，也用于单位标记的显示
        Font Font_kedu_str = new Font("Tahoma", 9, FontStyle.Regular);


        //XY轴刻度显示的小横杠的长度
        float XY_kedu = 7.0F;

        //XY轴刻度显示文字的颜色，也用于单位标记的显示
        Brush bush_kedu = new SolidBrush(Color.White);

        //XY轴的线条粗细
        float XY_width = 2F;


        //数据连线的颜色
        Pen pen_DB_xian = new Pen(Color.FromArgb(255, 196, 196), 1);

        //数据点空心圆圈的颜色
        Pen pen_DB_dian = new Pen(Color.FromArgb(4, 130, 255), 1);
        //数据点实心圆圈的颜色
        Brush bush_DB_dian = new SolidBrush(Color.White);
        //数据圆点半径(外层空心圆)
        float DB_dian_banjing = 4;
        //数据圆点内部实心圆与外部空心圆的半径差
        float DB_dian_r_cha = 2;

        //容器左边到刻度文字显示之间的留白像素
        float left_liubai = 5F;



        //X轴距离容器右边宽度
        float X_right = 30.0F;
        //Y轴距离容器上边宽度
        float Y_up = 40.0F;
        //Y轴距离容器下边宽度
        float Y_down = 30.0F;



        //背景线之间的间隔
        int bjline_jg = 30;

        //背景线的颜色
        Pen pen_bjline = new Pen(Color.FromArgb(50, 50, 50), 1);


        public YHBchart()
        {
            InitializeComponent();
        }
        Graphics g = null;
        private void PenLine(Graphics g, Color c, Point p1, Point p2)
        {
            g.DrawLine(new Pen(c), p1, p2);
        }
        private void PenLine(Graphics g, Color c, int p1X, int p1Y, int p2X, int p2Y)
        {
            g.DrawLine(new Pen(c), new Point(p1X, p1Y), new Point(p2X, p2Y));
        }

        private void YHBchart_Load(object sender, EventArgs e)
        {



        }

        //圆点图片控件集合
        Hashtable HTyuandian = new Hashtable();

        int DBpointNum = 0; //数据点的数量
        float Y_min = float.MaxValue; //Y轴数据显示最小值
        float Y_max = float.MinValue; //Y轴数据显示最大值
        DataSet dsDB = null; //数据

        /// <summary>
        /// 初始化数据集,用于测试，也是传值的标准。 一个点是一条数据。
        /// </summary>
        /// <returns></returns>
        public DataSet initReturnDataSet()
        {
            DataSet ds = new DataSet();
            DataTable auto1 = new DataTable();
            auto1.TableName = "数据点";
            auto1.Columns.Add("X轴数据", typeof(string));
            auto1.Columns.Add("Y轴数据", typeof(string));
            auto1.Columns.Add("标签附加文字", typeof(string));
            auto1.Columns.Add("其他", typeof(string));
            ds.Tables.Add(auto1);
            DataTable auto2 = new DataTable();
            auto2.TableName = "参数";
            auto2.Columns.Add("X轴数据标题", typeof(string));
            auto2.Columns.Add("X轴数据单位", typeof(string));
            auto2.Columns.Add("Y轴数据标题", typeof(string));
            auto2.Columns.Add("Y轴数据单位", typeof(string));
            auto2.Columns.Add("是否在X轴显示文字", typeof(string));
            ds.Tables.Add(auto2);
            return ds;
        }

        /// <summary>
        /// 开始显示数据
        /// </summary>
        /// <param name="ds">标准格式的数据集</param>
        public void ShowDB(DataSet ds)
        {
            //初始化
            DBpointNum = 0; //数据点的数量
            Y_min = float.MaxValue; //Y轴数据显示最小值
            Y_max = float.MinValue; //Y轴数据显示最大值
            dsDB = null; //数据

            //检查格式是否标准,必须至少含有指定的两个表，指定的表明，指定的列名。其他多余表和列无所谓。
            //并且必须两个指定表各自至少有一行数据。
            if (ds == null)
            {
                dsDB = null;
                Pmain.Refresh();
                return;
            }
            if (!ds.Tables.Contains("数据点") || !ds.Tables.Contains("参数"))
            {
                dsDB = null;
                Pmain.Refresh();
                return;
            }
            if (!ds.Tables["数据点"].Columns.Contains("X轴数据") ||
                !ds.Tables["数据点"].Columns.Contains("Y轴数据") || 
                !ds.Tables["数据点"].Columns.Contains("标签附加文字") || 
                !ds.Tables["数据点"].Columns.Contains("其他") || 
                !ds.Tables["参数"].Columns.Contains("X轴数据标题") || 
                !ds.Tables["参数"].Columns.Contains("X轴数据单位") ||
                !ds.Tables["参数"].Columns.Contains("Y轴数据标题") ||
                !ds.Tables["参数"].Columns.Contains("Y轴数据单位") ||
                !ds.Tables["参数"].Columns.Contains("是否在X轴显示文字"))
            {
                dsDB = null;
                Pmain.Refresh();
                return;
            }
            if (ds.Tables["数据点"].Rows.Count < 1 || ds.Tables["参数"].Rows.Count < 1)
            {
                dsDB = null;
                Pmain.Refresh();
                return;
            }



            //初步处理，得到基础变量
            DBpointNum = ds.Tables["数据点"].Rows.Count;
            init_yuandian(DBpointNum);

            string valEX = @"[0-9]+\.?[0-9]*";//只允许整数或小数的正则表达式
            for (int i = 0; i < DBpointNum; i++)
            {
                string Yshuju = ds.Tables["数据点"].Rows[i]["Y轴数据"].ToString();

                if (Yshuju.Trim() != "" && Regex.IsMatch(Yshuju, valEX))
                {
                    float thisvalue = float.Parse(Yshuju);
                    if (thisvalue <= Y_min)
                    {
                        Y_min = thisvalue;
                    }
                    if (thisvalue >= Y_max)
                    {
                        Y_max = thisvalue;
                    }
                }

            }
            //开始画
            dsDB = ds.Copy();
            Pmain.Refresh();
        }

        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YHBchart));
    /// <summary>
    /// 初始化圆点集合
    /// </summary>
    /// <param name="shuliang"></param>
       private void init_yuandian(int shuliang)
       {

           //清理
           HTyuandian.Clear();
           Pmain.Controls.Clear();
           //画新的圆点
           for(int i = 0 ; i < shuliang; i++)
           {
               PictureBox pb = new PictureBox();
               ((System.ComponentModel.ISupportInitialize)(pb)).BeginInit();
               pb.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
               pb.Location = new System.Drawing.Point(0, 0);
               pb.BorderStyle = System.Windows.Forms.BorderStyle.None;
               pb.Name = "pb_yuandian_" + i.ToString();
               pb.Size = new System.Drawing.Size(13, 13);
               pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
               pb.Cursor = System.Windows.Forms.Cursors.Hand;
               pb.BackColor = Color.Transparent;
               pb.Tag = i.ToString();
               pb.TabIndex = 0;
               pb.TabStop = false;
               pb.Visible = true;
               pb.Click += new System.EventHandler(clickYuanDian);
               pb.MouseEnter += new System.EventHandler(MouseEnterYuanDian);
               pb.MouseLeave += new System.EventHandler(MouseLeaveYuanDian);
               Pmain.Controls.Add(pb);
               ((System.ComponentModel.ISupportInitialize)(pb)).EndInit();
               HTyuandian[pb.Name] = pb;
           }

       }

        //鼠标点击圆点
       private void clickYuanDian(object sender, EventArgs e)
       {
           PictureBox pb = (PictureBox)(sender);
       }
        //鼠标进入圆点
       private void MouseEnterYuanDian(object sender, EventArgs e)
       {
           // 显示标签
           PictureBox pb = (PictureBox)(sender);
           pb.BringToFront();
           int ds_index =  Convert.ToInt32(pb.Tag);
           pb.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
           toolTipYD.SetToolTip(pb, dsDB.Tables["参数"].Rows[0]["X轴数据标题"].ToString() + "：" + dsDB.Tables["数据点"].Rows[ds_index]["X轴数据"].ToString() + dsDB.Tables["参数"].Rows[0]["X轴数据单位"].ToString() + Environment.NewLine + dsDB.Tables["参数"].Rows[0]["Y轴数据标题"].ToString() + "：" + dsDB.Tables["数据点"].Rows[ds_index]["Y轴数据"].ToString() + dsDB.Tables["参数"].Rows[0]["Y轴数据单位"].ToString() + Environment.NewLine + dsDB.Tables["数据点"].Rows[ds_index]["标签附加文字"].ToString());
      
            
       }
        //鼠标离开圆点
       private void MouseLeaveYuanDian(object sender, EventArgs e)
       {
           PictureBox pb = (PictureBox)(sender);
           pb.SendToBack();
           pb.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));

           
       }

      

        private void Pmain_Paint(object sender, PaintEventArgs e)
        {

            try
            {
                //YX轴的线条颜色
                Pen pen_X = new Pen(Color.GreenYellow, XY_width);
                Pen pen_Y = new Pen(Color.White, XY_width);
                //========================================================================================
                //画布
                Graphics g = e.Graphics;

                //设置抗锯齿
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                //设置背景色
                g.Clear(Color.Black);
                //==============================================================================================


                //容器宽度
                int Pwidth = Pmain.Size.Width;
                //容器高度
                int Pheight = Pmain.Size.Height;

                //确定出Y轴最大刻度值的宽度和高度
                SizeF kedu_max_str_sizeF = g.MeasureString(Y_max.ToString("F2"), Font_kedu_str);

                //X轴距离容器左边宽度
                float X_left = 0F;
                if (Y_max == float.MinValue)
                {
                    X_left = left_liubai + XY_kedu;
                }
                else
                {
                    X_left = kedu_max_str_sizeF.Width + left_liubai + XY_kedu;
                }


                //画Y轴正上方的单位标记文字
                if (dsDB != null && dsDB.Tables.Contains("参数") && dsDB.Tables["参数"].Rows[0]["Y轴数据单位"].ToString().Trim() != "")
                {
                    g.DrawString("单位：" + dsDB.Tables["参数"].Rows[0]["Y轴数据单位"].ToString(), Font_kedu_str, bush_kedu, X_left - 14, 10);
                }
   


                //画XY轴
                g.DrawLine(pen_X, X_left, Pheight - Y_down, Pwidth - X_right, Pheight - Y_down);
                g.DrawLine(pen_Y, X_left, Y_up, X_left, Pheight - Y_down);
                //计算画出来的座标中，在容器中的起点坐标，也就是左下角的交叉点坐标。
                float qi_X = X_left;
                float qi_Y = Pheight - Y_down;

                //Y轴从下开始的起点坐标Y值
                float bjline_Y_temp = Pheight - Y_down;
                //Y轴数值差值
                float Y_chazhi = Y_max - Y_min;
                //Y轴高度像素
                float Y_gao = (Pheight - Y_down) - Y_up;
                //X轴宽度像素
                float X_kuan = (Pwidth - X_right) - (X_left + XY_width);

                //Y轴数值差值比例
                float Y_chazhibili = 0F;
                if (Y_max == Y_min)
                {
                    Y_chazhibili = 0F;
                }
                else
                {
                    Y_chazhibili = Y_gao / Y_chazhi;
                }
                //Y轴数值高度比例
                float Y_bili = Y_chazhi / Y_gao;

                //X轴单步偏移像素
                float X_pianyi_step = 0F;
                if (DBpointNum <= 1)
                {
                    X_pianyi_step = 0F;
                }
                else
                {
                    X_pianyi_step = X_kuan / (DBpointNum - 1);
                }


                //Y轴刻度的显示值(0轴上的)
                string this_kedu_str_0 = Y_min.ToString("F2");
                if (dsDB != null && dsDB.Tables.Contains("数据点"))
                {
                    g.DrawString(this_kedu_str_0, Font_kedu_str, bush_kedu, (X_left + XY_width / 2) - kedu_max_str_sizeF.Width - XY_kedu, bjline_Y_temp - kedu_max_str_sizeF.Height / 2);
                }

                //间隔数
                int jgsl = 1;
                while (bjline_Y_temp > Y_up + bjline_jg)
                {
                    //当前刻度
                    bjline_Y_temp = bjline_Y_temp - bjline_jg;
                    //背景线
                    g.DrawLine(pen_bjline, X_left + XY_width / 2, bjline_Y_temp, Pwidth - X_right, bjline_Y_temp);

                    //Y轴刻度
                    g.DrawLine(pen_Y, X_left + XY_width / 2, bjline_Y_temp, X_left + XY_width / 2 - XY_kedu, bjline_Y_temp);

                    //Y轴刻度的显示值
                    string this_kedu_str = "";
                    if (Y_chazhibili == 0F)
                    {
                        this_kedu_str = "";
                    }
                    else
                    {
                        this_kedu_str = (jgsl * bjline_jg * Y_bili).ToString("F2");
                    }
                    if (dsDB != null && dsDB.Tables.Contains("数据点"))
                    {
                        g.DrawString(this_kedu_str, Font_kedu_str, bush_kedu, (X_left + XY_width / 2) - kedu_max_str_sizeF.Width - XY_kedu, bjline_Y_temp - kedu_max_str_sizeF.Height / 2);
                    }
                    jgsl++;

                }



                //画具体的数据对应的折线和数据点
                if (dsDB != null && dsDB.Tables.Contains("数据点"))
                {

                    string valEX = @"[0-9]+\.?[0-9]*";//只允许整数或小数的正则表达式

                    float DB_X_old = float.MinValue; //上一个有效数据点X轴精确位置
                    float DB_Y_old = float.MinValue; //上一个有效数据点Y轴精确位置
                    //开始逐个画点，以及折线
                    for (int p = 0; p < DBpointNum; p++)
                    {
                        //当前行数索引，乘以单步偏移像素 。 得到X轴偏移总量。
                        float X_pianyi = p * X_pianyi_step;

                        float DB_X = qi_X + X_pianyi; //数据点X轴精确位置

                        //开始画点，但不是数字的点，跳过不画
                        string Yshuju = dsDB.Tables["数据点"].Rows[p]["Y轴数据"].ToString();

                        if (Yshuju.Trim() != "" && Regex.IsMatch(Yshuju, valEX))
                        {
                            //原始值，乘以， 差值比例 。 得到Y轴偏移总量。
                            float thisvalue = float.Parse(Yshuju);
                            float Y_pianyi = (thisvalue - Y_min) * Y_chazhibili;

                            //数据点Y轴精确位置
                            float DB_Y = qi_Y - Y_pianyi;

                            //画空心外层圆圈
                            g.DrawEllipse(pen_DB_dian, DB_X - DB_dian_banjing - 1, DB_Y - DB_dian_banjing - 1, DB_dian_banjing * 2 + 1, DB_dian_banjing * 2 + 1);
                            //画实心圈
                            float DB_dian_banjing_2 = DB_dian_banjing - DB_dian_r_cha;
                            g.FillEllipse(bush_DB_dian, DB_X - DB_dian_banjing_2 - 1, DB_Y - DB_dian_banjing_2 - 1, DB_dian_banjing_2 * 2 + 1, DB_dian_banjing_2 * 2 + 1);

                            //放置圆点鼠标响应控件
                            if (HTyuandian != null && HTyuandian.ContainsKey("pb_yuandian_" + p.ToString()))
                            {
                                PictureBox pb = (PictureBox)(HTyuandian["pb_yuandian_" + p.ToString()]);
                               pb.Location = new System.Drawing.Point((int)DB_X - pb.Width / 2, (int)DB_Y - pb.Height / 2);
                            }


                            //画线
                            if (!DB_X_old.Equals(float.MinValue))
                            {
                                g.DrawLine(pen_DB_xian, DB_X_old, DB_Y_old, DB_X, DB_Y);

                            }
                            //记录上一个有效位置
                            DB_X_old = DB_X;
                            DB_Y_old = DB_Y;

                        }

                        //画X轴的刻度
                        if (p != 0)
                        {
                            g.DrawLine(pen_X, DB_X, qi_Y + XY_width / 2, DB_X, qi_Y + XY_width / 2 + XY_kedu);
                        }

                        //画X轴的刻度上的显示文字
                        if (dsDB != null && dsDB.Tables.Contains("数据点") && dsDB.Tables.Contains("参数") && dsDB.Tables["参数"].Rows[0]["是否在X轴显示文字"].ToString() == "是")
                        {
                            string kedu_str_X = dsDB.Tables["数据点"].Rows[p]["X轴数据"].ToString();
                            SizeF kedu_str_sizeF = g.MeasureString(kedu_str_X, Font_kedu_str);
                            g.DrawString(dsDB.Tables["数据点"].Rows[p]["X轴数据"].ToString(), Font_kedu_str, bush_kedu, DB_X - kedu_str_sizeF.Width/2, qi_Y + XY_width / 2 + XY_kedu);
                        }


                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("曲线图意外出错！" + ex.ToString());
            }
            
         

        }

        private void Pmain_Resize(object sender, EventArgs e)
        {

            Pmain.Refresh();
        }

        private void Pmain_MouseMove(object sender, MouseEventArgs e)
        {
            ;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ;
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {

        }
    }
}
