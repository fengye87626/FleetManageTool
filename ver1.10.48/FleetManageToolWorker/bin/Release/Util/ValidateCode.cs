using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace FleetManageTool.Util
{
    //chenyangwen 生成验证码的类
    public class ValidateCode
    {
        //生成随机验证码
        public string CreateValidateCode(int length)
        {
            char[] validateNumberStr = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int seekSeek = unchecked((int)DateTime.Now.Ticks);
            Random seekRand = new Random(seekSeek);
            string validateCode = "";
            for (int i = 0; i < length; ++i)
            {
                int temp = seekRand.Next(10);
                validateCode += validateNumberStr[temp];
            }
            return validateCode;
        }

        //生成验证码图片
        public byte[] CreateValidateImage(string validateCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);
            try
            {
                Random random = new Random();
                Color color = Color.FromArgb(random.Next(255) + 1, random.Next(255) + 1, random.Next(255) + 1);
                g.Clear(Color.White);
                //绘制干扰线
                for (int i = 0; i < 3; ++i)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);

                    g.DrawLine(new Pen(color), x1, y1, x2, y2);
                }
                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                //渐变性画刷，用于绘画文字
                //LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), 
                //    Color.FromArgb(random.Next(255) + 1, random.Next(255) + 1, random.Next(255) + 1), Color.FromArgb(random.Next(255) + 1, random.Next(255) + 1, random.Next(255) + 1), 1.2f, true);
                SolidBrush brush = new SolidBrush(Color.Red);
                g.DrawString(validateCode, font, brush, 3, 2);
                //干扰点
                for (int i = 0; i < 50; i++ )
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //图片的边框线
                color = Color.FromArgb(random.Next(255) + 1, random.Next(255) + 1, random.Next(255) + 1);
                g.DrawRectangle(new Pen(color), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
            return null;
        }
    }
}