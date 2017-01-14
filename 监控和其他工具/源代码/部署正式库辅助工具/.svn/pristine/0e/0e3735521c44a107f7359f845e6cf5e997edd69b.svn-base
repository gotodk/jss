using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;


namespace 部署正式库辅助工具
{

    /*
     调用方法
            string logo = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\tupianshuiyin.png";
            string oldtu = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\yuantu.jpg";

            //缩放图标并添加水印(由于是全屏水印，因此图片是根据水印大小自动缩放的)
            WaterImageManage WIM = new WaterImageManage();
            Image newtu =  WIM.OnlyPingTai(logo, oldtu);
 
            //保存图片看看效果
            newtu.Save(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\生成预览.jpg");
            newtu.Dispose();
      
     */

    /// <summary>   
    /// 图片位置   
    /// </summary>   
    public enum ImagePosition
    {
        LeftTop,        //左上   
        LeftBottom,    //左下   
        RightTop,       //右上   
        RigthBottom,  //右下   
        TopMiddle,     //顶部居中   
        BottomMiddle, //底部居中   
        Center           //中心   
    }

    /// <summary>   
    /// 水印图片的操作管理 Design by Gary Gong From Demetersoft.com   
    /// </summary>   
    public class WaterImageManage
    {
        /// <summary>   
        /// 生成一个新的水印图片制作实例   
        /// </summary>   
        public WaterImageManage()
        {
            //   
            // TODO: Add constructor logic here   
            //   
        }

        /// <summary>
        /// 平台专用，原图按照水印的大小，按原图比例进行缩放后添加水印，水印半透明，使用一般质量
        /// </summary>
        /// <param name="logo"></param>
        /// <param name="oldtu"></param>
        /// <returns></returns>
        public Image OnlyPingTai(string logo, string oldtu)
        {
            WaterImageManage WIM = new WaterImageManage();

            Image logo_img = ReadToImage(logo);
            Image oldtu_img = WIM.MakeThumbnail(oldtu, logo_img.Width, logo_img.Height, "A");
            Image newtu = WIM.DrawImage(oldtu_img, logo_img, 0.5f, ImagePosition.Center);
            return KiSaveAsJPEG((Bitmap)newtu, 90);
        }


        private Image ReadToImage(string ImgFile)
        {
            FileStream fs = new FileStream(ImgFile, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();
            MemoryStream ms = new MemoryStream(bytes);

            return System.Drawing.Image.FromStream(ms);
        }




        /**/
        /// <summary>
        /// 保存JPG时用
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns>得到指定mimeType的ImageCodecInfo</returns>
        private ImageCodecInfo GetCodecInfo(string mimeType)
        {
            ImageCodecInfo[] CodecInfo = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo ici in CodecInfo)
            {
                if (ici.MimeType == mimeType) return ici;
            }
            return null;
        }

        /**/
        /// <summary>
        /// 保存为JPEG格式，支持压缩质量选项
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="FileName"></param>
        /// <param name="Qty"></param>
        /// <returns></returns>
        public Image KiSaveAsJPEG(Bitmap bmp,int Qty)
        {
            try
            {
                EncoderParameter p;
                EncoderParameters ps;

                ps = new EncoderParameters(1);

                p = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Qty);
                ps.Param[0] = p;

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, GetCodecInfo("image/jpeg"), ps);

                return System.Drawing.Image.FromStream(ms);
            }
            catch
            {
                return null;
            }

        }

        ///<summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public Image MakeThumbnail(string originalImagePath, int width, int height, string mode)
        {
            try
            {
                System.Drawing.Image originalImage = ReadToImage(originalImagePath);

                int towidth = width;
                int toheight = height;

                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;

                switch (mode)
                {
                    case "HW"://指定高宽缩放（可能变形）                
                        break;
                    case "W"://指定宽，高按比例                    
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case "H"://指定高，宽按比例
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case "A":
                        if (originalImage.Width / originalImage.Height >= width / height)
                        {
                            if (originalImage.Width > width)
                            {
                                towidth = width;
                                toheight = (originalImage.Height * width) / originalImage.Width;
                            }
                            else
                            {
                                towidth = originalImage.Width;
                                toheight = originalImage.Height;
                            }
                        }
                        else
                        {
                            if (originalImage.Height > height)
                            {
                                toheight = height;
                                towidth = (originalImage.Width * height) / originalImage.Height;
                            }
                            else
                            {
                                towidth = originalImage.Width;
                                toheight = originalImage.Height;
                            }
                        }
                        break;
                    case "Cut"://指定高宽裁减（不变形）                
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        break;
                }

                //新建一个bmp图片
                System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

                //新建一个画板
                Graphics g = System.Drawing.Graphics.FromImage(bitmap);

                //设置高质量插值法
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                 new Rectangle(x, y, ow, oh),
                 GraphicsUnit.Pixel);


                originalImage.Dispose();

                g.Dispose();

                return bitmap;
            }
            catch (System.Exception e)
            {
                throw e;
            }

        }

        /// <summary>   
        /// 添加图片水印   
        /// </summary>   
        /// <param name="sourcePicture">源图片文件名</param>   
        /// <param name="waterImage">水印图片文件名</param>   
        /// <param name="alpha">透明度(0.1-1.0数值越小透明度越高)</param>   
        /// <param name="position">位置</param>   
        /// <param name="PicturePath" >图片的路径</param>   
        /// <returns>返回生成于指定文件夹下的水印文件名</returns>   
        public Image DrawImage(Image imgPhoto,
                                          Image imgWatermark,
                                          float alpha,
                                          ImagePosition position)
        {
            ////   
            //// 判断参数是否有效   
            ////   
            //if (sourcePicture == string.Empty || waterImage == string.Empty || alpha == 0.0 || PicturePath == string.Empty)
            //{
            //    return sourcePicture;
            //}

            //   
            // 源图片，水印图片全路径   
            //   
            //string sourcePictureName = PicturePath + sourcePicture;
            //string waterPictureName = PicturePath + waterImage;
            //string fileSourceExtension = System.IO.Path.GetExtension(sourcePictureName).ToLower();
            //string fileWaterExtension = System.IO.Path.GetExtension(waterPictureName).ToLower();
            //   
            // 判断文件是否存在,以及类型是否正确   
            //   
            //if (System.IO.File.Exists(sourcePictureName) == false ||
            //    System.IO.File.Exists(waterPictureName) == false || (
            //    fileSourceExtension != ".gif" &&
            //    fileSourceExtension != ".jpg" &&
            //    fileSourceExtension != ".png") || (
            //    fileWaterExtension != ".gif" &&
            //    fileWaterExtension != ".jpg" &&
            //    fileWaterExtension != ".png")
            //    )
            //{
            //    return sourcePicture;
            //}

            //   
            // 目标图片名称及全路径   
            //   
            //string targetImage = sourcePictureName.Replace(System.IO.Path.GetExtension(sourcePictureName), "") + "_1101.jpg";

            //   
            // 将需要加上水印的图片装载到Image对象中   
            //   
            //Image imgPhoto = Image.FromFile(sourcePictureName);
            //   
            // 确定其长宽   
            //   
            int phWidth = imgPhoto.Width;
            int phHeight = imgPhoto.Height;

            //   
            // 封装 GDI+ 位图，此位图由图形图像及其属性的像素数据组成。   
            //   
            Bitmap bmPhoto = new Bitmap(phWidth, phHeight, PixelFormat.Format24bppRgb);

            //   
            // 设定分辨率   
            //    
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //   
            // 定义一个绘图画面用来装载位图   
            //   
            Graphics grPhoto = Graphics.FromImage(bmPhoto);

            //   
            //同样，由于水印是图片，我们也需要定义一个Image来装载它   
            //   
            //Image imgWatermark = new Bitmap(waterPictureName);

            //   
            // 获取水印图片的高度和宽度   
            //   
            int wmWidth = imgWatermark.Width;
            int wmHeight = imgWatermark.Height;

            //SmoothingMode：指定是否将平滑处理（消除锯齿）应用于直线、曲线和已填充区域的边缘。   
            // 成员名称   说明    
            // AntiAlias      指定消除锯齿的呈现。     
            // Default        指定不消除锯齿。     
            // HighQuality  指定高质量、低速度呈现。     
            // HighSpeed   指定高速度、低质量呈现。     
            // Invalid        指定一个无效模式。     
            // None          指定不消除锯齿。    
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;

            //   
            // 第一次描绘，将我们的底图描绘在绘图画面上   
            //   
            grPhoto.DrawImage(imgPhoto,
                                        new Rectangle(0, 0, phWidth, phHeight),
                                        0,
                                        0,
                                        phWidth,
                                        phHeight,
                                        GraphicsUnit.Pixel);

            //   
            // 与底图一样，我们需要一个位图来装载水印图片。并设定其分辨率   
            //   
            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            //   
            // 继续，将水印图片装载到一个绘图画面grWatermark   
            //   
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //   
            //ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。   
            //          
            ImageAttributes imageAttributes = new ImageAttributes();

            //   
            //Colormap: 定义转换颜色的映射   
            //   
            ColorMap colorMap = new ColorMap();

            //   
            //我的水印图被定义成拥有绿色背景色的图片被替换成透明   
            //   
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {    
           new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f}, // red红色   
           new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f}, //green绿色   
           new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f}, //blue蓝色          
           new float[] {0.0f,  0.0f,  0.0f,  alpha, 0.0f}, //透明度        
           new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};//   

            //  ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。   
            //  ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。   
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);


            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
             ColorAdjustType.Bitmap);

            //   
            //上面设置完颜色，下面开始设置位置   
            //   
            int xPosOfWm;
            int yPosOfWm;

            switch (position)
            {
                case ImagePosition.BottomMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.Center:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = (phHeight - wmHeight) / 2;
                    break;
                case ImagePosition.LeftBottom:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.LeftTop:
                    xPosOfWm = 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RightTop:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = 10;
                    break;
                case ImagePosition.RigthBottom:
                    xPosOfWm = phWidth - wmWidth - 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
                case ImagePosition.TopMiddle:
                    xPosOfWm = (phWidth - wmWidth) / 2;
                    yPosOfWm = 10;
                    break;
                default:
                    xPosOfWm = 10;
                    yPosOfWm = phHeight - wmHeight - 10;
                    break;
            }

            //   
            // 第二次绘图，把水印印上去   
            //   
            grWatermark.DrawImage(imgWatermark,
             new Rectangle(xPosOfWm,
                                 yPosOfWm,
                                 wmWidth,
                                 wmHeight),
                                 0,
                                 0,
                                 wmWidth,
                                 wmHeight,
                                 GraphicsUnit.Pixel,
                                 imageAttributes);


            imgPhoto = bmWatermark;
            grPhoto.Dispose();
            grWatermark.Dispose();

            //   
            // 保存文件到服务器的文件夹里面   
            //   
            //imgPhoto.Save(targetImage, ImageFormat.Jpeg);
            //imgPhoto.Dispose();
            imgWatermark.Dispose();
            return imgPhoto;
        }


    }

    /// <summary>   
    /// 装载水印图片的相关信息   
    /// </summary>   
    public class WaterImage
    {
        public WaterImage()
        {

        }

        private string m_sourcePicture;
        /// <summary>   
        /// 源图片地址名字(带后缀)   
        /// </summary>   
        public string SourcePicture
        {
            get { return m_sourcePicture; }
            set { m_sourcePicture = value; }
        }

        private string m_waterImager;
        /// <summary>   
        /// 水印图片名字(带后缀)   
        /// </summary>   
        public string WaterPicture
        {
            get { return m_waterImager; }
            set { m_waterImager = value; }
        }

        private float m_alpha;
        /// <summary>   
        /// 水印图片文字的透明度   
        /// </summary>   
        public float Alpha
        {
            get { return m_alpha; }
            set { m_alpha = value; }
        }

        private ImagePosition m_postition;
        /// <summary>   
        /// 水印图片或文字在图片中的位置   
        /// </summary>   
        public ImagePosition Position
        {
            get { return m_postition; }
            set { m_postition = value; }
        }

        private string m_words;
        /// <summary>   
        /// 水印文字的内容   
        /// </summary>   
        public string Words
        {
            get { return m_words; }
            set { m_words = value; }
        }

    }

}