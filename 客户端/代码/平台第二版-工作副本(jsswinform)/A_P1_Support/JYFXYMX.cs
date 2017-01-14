using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace 客户端主程序.Support
{
    /// <summary>
    /// 交易方信用明细
    /// </summary>
    public static class JYFXYMX
    {

        /// <summary>
        /// 合并图片
        /// </summary>
        /// <param name="maps"></param>
        /// <returns></returns>
        public static Bitmap MergerImg(params Image[] maps)
        {
            //创建要显示的图片对象,根据参数的个数设置宽度
            Bitmap backgroudImg = new Bitmap(150, 13);
            try
            {
 
                Graphics g = Graphics.FromImage(backgroudImg);
                //清除画布,背景设置为白色
                g.Clear(System.Drawing.Color.Transparent);
                if (maps != null)
                {
                    if (maps.Length > 0)
                    {
                        int i = maps.Length;

                        ColorMatrix clrMatrix = new ColorMatrix();
                        clrMatrix.Matrix33 = 0.60f;
                        ImageAttributes imgAttributes = new ImageAttributes();
                        imgAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                        for (int j = 0; j < i; j++)
                        {
                            g.DrawImage(maps[j], new Rectangle(j * 14, 0, 14, 13),0,0, maps[j].Width, maps[j].Height, GraphicsUnit.Pixel, imgAttributes);
                        }
                    }
                }

       
                g.Dispose();
          
            }
            catch (Exception ex)
            {
                Support.StringOP.WriteLog("合并图片出错调试：" + ex.ToString());
            }

            return backgroudImg;
        }

        /// <summary>
        /// 用户信用等级图片
        /// </summary>
        /// <param name="userScore"></param>
        /// <returns></returns>
        public static Image[] GetXYImages(double userScore)
        {
            if (userScore >= 1 && userScore < 4)//1-3分  “一心”
            {
                return new Image[] { UserYXImage.XinXing };
            }
            else if (userScore >= 4 && userScore < 7)//4-6分  “二心”
            {
                return new Image[] { UserYXImage.XinXing, UserYXImage.XinXing };
            }
            else if (userScore >= 7 && userScore < 10)//7-9分  “三心”
            {
                return new Image[] { UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing };
            }
            else if (userScore >= 10 && userScore < 13)//10-12分  “四心”
            {
                return new Image[] { UserYXImage.XinXing, UserYXImage.XinXing,UserYXImage.XinXing,UserYXImage.XinXing };
            }
            else if (userScore >= 13 && userScore < 16)//13-15分  “五心”
            {
                return new Image[] { UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing, UserYXImage.XinXing };
            }
            else if (userScore >= 16 && userScore <21)//13-15分  “一钻”
            {
                return new Image[] { UserYXImage.ZuanXing };
            }
            else if (userScore >= 21 && userScore <26)//21-25分  “二钻”
            {
                return new Image[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing };
            }
            else if (userScore >= 26 && userScore <31)//26-30分  “三钻”
            {
                return new Image[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing };
            }
            else if (userScore >= 31 && userScore <36)//31-25分  “四钻”
            {
                return new Image[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing };
            }
            else if (userScore >= 36 && userScore < 41)//36-40分  “五钻”
            {
                return new Image[] { UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing, UserYXImage.ZuanXing };
            }
            else if (userScore >= 41 && userScore < 46)//41-45分  “一皇冠”
            {
                return new Image[] { UserYXImage.HuanGuanXing };
            }
            else if (userScore >= 46 && userScore < 51)//46-50分  “二皇冠”
            {
                return new Image[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
            }
            else if (userScore >= 51 && userScore < 56)//46-50分  “三皇冠”
            {
                return new Image[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
            }
            else if (userScore >= 56 && userScore < 61)//56-60分  “四皇冠”
            {
                return new Image[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
            }
            else if (userScore >= 61 && userScore < 100)//61-99分  “五皇冠”
            {
                return new Image[] { UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing, UserYXImage.HuanGuanXing };
            }
            else if (userScore >= 100) //100分以上 “VIP”
            {
                return new Image[] { UserYXImage.VipXing };

            }
            return null;
        }




    }


    public static class UserYXImage
    {
        static public Hashtable im = new Hashtable();
       
        /// <summary>
        /// 返回“心”形状图片
        /// </summary>
        public static Image XinXing
        {
            get
            {
                if (im.ContainsKey("xinxing"))
                {
                    return (Image)(im["xinxing"]);
                }
                else
                {
                    im["xinxing"] = Image.FromFile(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\xinxing.png");
                    return (Image)(im["xinxing"]);
                }
                
            }
        }

        /// <summary>
        /// 返回“钻”形状图片
        /// </summary>
        public static Image ZuanXing
        {

            get {

                if (im.ContainsKey("zuanxing"))
                {
                    return (Image)(im["zuanxing"]);
                }
                else
                {
                    im["zuanxing"] = Image.FromFile(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\zuanxing.png");
                    return (Image)(im["zuanxing"]);
                }
               
            }
        }

        /// <summary>
        /// 返回“皇冠”形状图片
        /// </summary>
        public static Image HuanGuanXing
        {
            get {

                if (im.ContainsKey("huangguanxing"))
                {
                    return (Image)(im["huangguanxing"]);
                }
                else
                {
                    im["huangguanxing"] = Image.FromFile(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\huangguanxing.png");
                    return (Image)(im["huangguanxing"]);
                }
          
            }
        
        }

        /// <summary>
        /// 返回“VIP”形状图片
        /// </summary>
        public static Image VipXing
        {
            get {

                if (im.ContainsKey("vipxing"))
                {
                    return (Image)(im["vipxing"]);
                }
                else
                {
                    im["vipxing"] = Image.FromFile(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\skin\vipxing.png");
                    return (Image)(im["vipxing"]);
                }
                
            }
        
        }



    }




}
