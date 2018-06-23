using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Cvb;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Collections;
using Emgu.CV.Util;
using RPoint = System.Drawing.Point;
using System.Text;
namespace KreyGasm
{
    /// <summary>
    /// Сводное описание для Neuron
    /// </summary>
    [WebService(Namespace = "http://microsoft.com/webservices/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class Neuron : System.Web.Services.WebService
    {
        //Создание нейросети с 10 выходными нейронами, которая будет обрабатывать картинку 28x28
        ImageNeuralNetwork net = new ImageNeuralNetwork(10, 28, 28);
        [WebMethod]
        //Метод, который будет распознавать base64 изображение
        public string GetAnswerFromBase64(string text)
        {
            //Загрузка весов
            net.LoadWeights(Server.MapPath("App_Data/Weights"));
            //Парсинг base64 строки
            string[] base_num = text.Split(',');
            //Преобразование base64 в массив байтов, а затем в картинку
            byte[] data = Convert.FromBase64String(base_num[1]);
            using (var ms = new MemoryStream(data))
            {
                Image img = Image.FromStream(ms);
                //Масштабирование картинки
                img = img.ResizeImg(28, 28);
                //Получение и возврат ответа
                return net.GetAnswer(img.ToByte()).ToString();
            }
        }
        [WebMethod]
        public string Recognize_Ball(byte[] image_data_base, string SHsvSettings, string Points)
        {
            byte[] image_data = image_data_base;
            byte[] HsvSettings = new byte[6];
            string[] nums = SHsvSettings.Split(',');
            for (int i = 0; i < 6; i++)
            {
                HsvSettings[i] = Convert.ToByte(nums[i]);
            }
            Bitmap bmp = null;
            using (var ms = new MemoryStream(image_data))
            {
                bmp = new Bitmap(ms);
            }
            Image<Bgr, byte> img = new Image<Bgr, byte>(bmp);
            Image<Hsv, byte> hsv = new Image<Hsv, byte>(bmp);

            img = img.Flip(FlipType.Horizontal);
            CvInvoke.CvtColor(img, hsv, ColorConversion.Bgr2Hsv);

            var mask = hsv.InRange(new Hsv(HsvSettings[0], HsvSettings[1], HsvSettings[2]), new Hsv(HsvSettings[3], HsvSettings[4], HsvSettings[5]));
            var morphed_mask = new Image<Gray, byte>(mask.Width, mask.Height);
            // morphed_mask = mask.Erode(2);
            // morphed_mask = morphed_mask.Dillate(2);
            Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Ellipse, new System.Drawing.Size(5, 5), new RPoint(-1, -1));
            CvInvoke.MorphologyEx(mask, morphed_mask, MorphOp.Open, kernel, new RPoint(-1, -1), 2, new BorderType(), new MCvScalar());

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            Mat hierachy = new Mat();

            CvInvoke.FindContours(morphed_mask.Copy(), contours, hierachy, RetrType.External, ChainApproxMethod.ChainApproxSimple);

            VectorOfPoint maximum = null;
            for (int i = 0; i < contours.Size; i++)
            {
                if (maximum != null)
                {
                    if (contours[i].Size > maximum.Size)
                    {
                        maximum = contours[i];
                    }
                }
                else
                {
                    maximum = contours[i];
                }
            }
            RPoint? pCenter = null;
            if (maximum != null)
            {
                var circle = CvInvoke.MinEnclosingCircle(maximum);
                pCenter = new RPoint((int)circle.Center.X, (int)circle.Center.Y);
                CvInvoke.Circle(img, (RPoint)pCenter, (int)circle.Radius, new MCvScalar(255, 255, 255), 5);
                int speed = -1;
                if (Points != "-1;-1" && pCenter != null)
                {
                    string[] points = Points.Split(';');
                    RPoint p1 = new RPoint(Convert.ToInt32(points[0]), Convert.ToInt32(points[1]));
                    RPoint p2 = (RPoint)pCenter;
                    RPoint ptPoint = new RPoint(p2.X - 20, p2.Y - ((int)circle.Radius + 25));
                    speed = Math.Abs((p1.X + p1.Y) - (p2.X + p2.Y));
                    CvInvoke.PutText(img, speed.ToString(), ptPoint, FontFace.HersheyDuplex, 1, new MCvScalar(255, 255, 255), 2, LineType.AntiAlias);
                }
            }
            using (var ms = new MemoryStream())
            {
                img.Bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                int x = -1, y = -1;
                if (pCenter != null)
                {
                    x = ((RPoint)pCenter).X;
                    y = ((RPoint)pCenter).Y;
                }
                return Convert.ToBase64String(ms.GetBuffer()) + "," + String.Format("{0};{1}", x, y);
            }
        }
    }
}
