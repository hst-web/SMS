/*----------------------------------------------------------------

// 文件名：ImagHelper.cs
// 功能描述： 图片帮助类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace HST.Utillity
{
    /// <summary>
    /// 水印位置
    /// </summary>
    public enum ImagePosition
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 1,
        /// <summary>
        /// 左上
        /// </summary>
        LeftTop = 2,
        /// <summary>
        /// 左下
        /// </summary>
        LeftBottom = 3,
        /// <summary>
        /// 右上
        /// </summary>
        RightTop = 4,
        /// <summary>
        /// 右下
        /// </summary>
        RigthBottom = 5,
        //TopMiddle,     //顶部居中
        //BottomMiddle, //底部居中
        //Center           //中心
    }

    public enum ThumbnailModel
    {
        /// <summary>
        /// 指定高宽缩放（可能变形）
        /// </summary>
        HW,
        /// <summary>
        /// 指定宽，高按比例
        /// </summary>
        W,
        /// <summary>
        /// 指定高，宽按比例
        /// </summary>
        H,
        /// <summary>
        /// 指定高宽裁减（不变形）
        /// </summary>
        Cut,
        /// <summary>
        /// 不超出尺寸，比它小就不截了，不留白，大就缩小到最佳尺寸，主要为手机用
        /// </summary>
        Fit
    }

    /// <summary>
    /// 图片 帮助类。
    /// </summary>
    public class ImagHelper
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ImagHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        ///  生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>	
        /// <param name="isaddwatermark">是否添加水印</param>	
        /// <param name="quality">图片品质</param>	
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ThumbnailModel mode = ThumbnailModel.Cut, bool isaddwatermark = false, int quality = 100)
        {
            MakeThumbnail(originalImagePath, thumbnailPath, width, height, mode, isaddwatermark, ImagePosition.Default, null, quality);
        }

        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>	
        /// <param name="isaddwatermark">是否添加水印</param>	
        /// <param name="quality">图片品质</param>	
        /// <param name="imagePosition">水印位置</param>	
        /// <param name="waterImage">水印图片名称</param>	
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, ThumbnailModel mode, bool isaddwatermark, ImagePosition imagePosition, string waterImage = null, int quality = 100)
        {
            Image originalImage = Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case ThumbnailModel.HW://指定高宽缩放（可能变形）				
                    break;
                case ThumbnailModel.W://指定宽，高按比例					
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case ThumbnailModel.H://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case ThumbnailModel.Cut://指定高宽裁减（不变形）				
                    if (originalImage.Width >= towidth && originalImage.Height >= toheight)
                    {
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
                    }
                    else
                    {
                        x = (originalImage.Width - towidth) / 2;
                        y = (originalImage.Height - toheight) / 2;
                        ow = towidth;
                        oh = toheight;
                    }
                    break;
                case ThumbnailModel.Fit://不超出尺寸，比它小就不截了，不留白，大就缩小到最佳尺寸，主要为手机用
                    if (originalImage.Width > towidth && originalImage.Height > toheight)
                    {
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                            toheight = originalImage.Height * width / originalImage.Width;
                        else
                            towidth = originalImage.Width * height / originalImage.Height;
                    }
                    else if (originalImage.Width > towidth)
                    {
                        toheight = originalImage.Height * width / originalImage.Width;
                    }
                    else if (originalImage.Height > toheight)
                    {
                        towidth = originalImage.Width * height / originalImage.Height;
                    }
                    else
                    {
                        towidth = originalImage.Width;
                        toheight = originalImage.Height;
                        ow = towidth;
                        oh = toheight;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(Color.White);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                new Rectangle(x, y, ow, oh),
                GraphicsUnit.Pixel);

            //加图片水印
            if (isaddwatermark)
            {
                if (string.IsNullOrEmpty(waterImage))
                    waterImage = "watermarker.png";
                Image copyImage = System.Drawing.Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, waterImage));
                //g.DrawImage(copyImage, new Rectangle(bitmap.Width-copyImage.Width, bitmap.Height-copyImage.Height, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
                int xPosOfWm;
                int yPosOfWm;
                int wmHeight = copyImage.Height;
                int wmWidth = copyImage.Width;
                int phHeight = toheight;
                int phWidth = towidth;
                switch (imagePosition)
                {
                    case ImagePosition.LeftBottom:
                        xPosOfWm = 70;
                        yPosOfWm = phHeight - wmHeight - 70;
                        break;
                    case ImagePosition.LeftTop:
                        xPosOfWm = 70;
                        yPosOfWm = 0 - 70;
                        break;
                    case ImagePosition.RightTop:
                        xPosOfWm = phWidth - wmWidth - 70;
                        yPosOfWm = 0 - 70;
                        break;
                    case ImagePosition.RigthBottom:
                        xPosOfWm = phWidth - wmWidth - 70;
                        yPosOfWm = phHeight - wmHeight - 70;
                        break;
                    default:
                        xPosOfWm = 10;
                        yPosOfWm = 0;
                        break;
                }
                g.DrawImage(copyImage, new Rectangle(xPosOfWm, yPosOfWm, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel);
            }

            // 以下代码为保存图片时,设置压缩质量
            EncoderParameters encoderParams = new EncoderParameters();
            long[] qualityArray = new long[1];
            qualityArray[0] = quality;
            EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qualityArray);
            encoderParams.Param[0] = encoderParam;
            //获得包含有关内置图像编码解码器的信息的ImageCodecInfo 对象.
            ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo jpegICI = null;
            for (int i = 0; i < arrayICI.Length; i++)
            {
                if (arrayICI[i].FormatDescription.Equals("JPEG"))
                {
                    jpegICI = arrayICI[i];
                    //设置JPEG编码
                    break;
                }
            }

            try
            {
                if (jpegICI != null)
                {
                    bitmap.Save(thumbnailPath, jpegICI, encoderParams);
                }
                else
                {
                    //以jpg格式保存缩略图
                    bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="sFile">图片原地址</param>
        /// <param name="dFile">图片目标地址</param>
        /// <param name="size">文件大小</param>
        /// <returns></returns>
        public static bool CompressImage(string sFile, string dFile, int size = 500)
        {
            FileInfo fInfo = new FileInfo(sFile);

            //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
            if (fInfo.Length < size * 1024)
            {
                fInfo.CopyTo(dFile);
                return true;
            }

            System.Drawing.Image iSource = System.Drawing.Image.FromFile(sFile);
            return CompressImage(iSource, dFile);
        }


        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="sFileStream">图片内存流</param>
        /// <param name="dFile">压缩后保存图片地址</param>
        /// <param name="size">压缩后图片的最大大小</param>
        /// <returns></returns>
        public static bool CompressImage(Stream sFileStream, string dFile, int size = 500)
        {
            System.Drawing.Image iSource = System.Drawing.Image.FromStream(sFileStream);

            //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
            if (sFileStream.Length <= size * 1024)
            {
                try
                {
                    iSource.Save(dFile);
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    iSource.Dispose();
                }

            }

            return CompressImage(iSource, dFile);
        }


        /// <summary>
        /// 无损压缩图片
        /// </summary>
        /// <param name="iSource">图片信息</param>
        /// <param name="dFile">压缩后保存图片地址</param>
        /// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
        /// <returns></returns>
        private static bool CompressImage(Image iSource, string dFile, int flag = 100)
        {
            ImageFormat tFormat = iSource.RawFormat;

            int dHeight = iSource.Height;
            int dWidth = iSource.Width;
            int sW = 0, sH = 0;
            //按比例缩放
            Size tem_size = new Size(iSource.Width, iSource.Height);
            if (tem_size.Width > dHeight || tem_size.Width > dWidth)
            {
                if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
                {
                    sW = dWidth;
                    sH = (dWidth * tem_size.Height) / tem_size.Width;
                }
                else
                {
                    sH = dHeight;
                    sW = (tem_size.Width * dHeight) / tem_size.Height;
                }
            }
            else
            {
                sW = tem_size.Width;
                sH = tem_size.Height;
            }

            Bitmap ob = new Bitmap(dWidth, dHeight);
            Graphics g = Graphics.FromImage(ob);

            g.Clear(Color.WhiteSmoke);
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

            g.Dispose();

            //以下代码为保存图片时，设置压缩质量
            EncoderParameters ep = new EncoderParameters();
            long[] qy = new long[1];
            qy[0] = flag;//设置压缩的比例1-100
            EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
            ep.Param[0] = eParam;

            try
            {
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                if (jpegICIinfo != null)
                {
                    ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
                }
                else
                {
                    ob.Save(dFile, tFormat);
                }
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                iSource.Dispose();
                ob.Dispose();
            }
        }


    }
}

