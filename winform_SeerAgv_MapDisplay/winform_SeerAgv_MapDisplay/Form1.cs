using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace winform_SeerAgv_MapDisplay
{
    
    public partial class Form1 : Form
    {

        //class define***************************************************
        private TcpClient tcpClient_connectAgv_port19204=new TcpClient ();


        //user define*************************************************
        private bool isConnect = false;
        private Color btnRegionColor;
        private ThreadSignal ths_receiveMes;
        private CMDMaster cmdStatu = CMDMaster.empty;

        //Tcp Command**************************************************
        //5A 01 00 01 00 00 00 1C 07 D2 00 00 00 00 00 00
        //5a 01 00 01 00 00 00 00 03 ec 00 00 00 00 00 00
        byte[] getMap={0x0F,0xAB};
        byte[] getLaser = { 0x03,0xF1};
        byte[] getPosition = { 0x03, 0xEC };

        
        //***********************************************************
        private byte[] getTelegramHead(byte[] cmd)
        {
            if(cmd.Length!=2)
                return new byte [0];

            byte[] ret={0x5A, 0x01, 00, 01, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00, 00};
            ret[8] = cmd[0];
            ret[9]=cmd[1];

            return ret;
        }

        private void RotateFormCenter1(PictureBox pb, float angle)
        {
            Image img = imageList1.Images[0];
            int newWidth = Math.Max(img.Height, img.Width);
            Bitmap bmp = new Bitmap(newWidth, newWidth);
            Graphics g = Graphics.FromImage(bmp);
            Matrix x = new Matrix();
            PointF point = new PointF(img.Width / 2f, img.Height / 2f);
            x.RotateAt(angle, point);
            g.Transform = x;
            g.DrawImage(img, 0, 0);
            g.Dispose();
            img = bmp;
            pb.Image = img;
        }

        private void RotateFormCenter(PictureBox pb, float angle)
        {
            Graphics graphics = pb.CreateGraphics();
            graphics.Clear(pb.BackColor);
            //装入图片
            Bitmap image = new Bitmap(imageList1.Images[0]);
            //获取当前窗口的中心点
            Rectangle rect = new Rectangle(0, 0, pb.Width, pb.Height);
            PointF center = new PointF(rect.Width / 2, rect.Height / 2);
            float offsetX = 0;
            float offsetY = 0;
            offsetX = center.X - image.Width / 2;
            offsetY = center.Y - image.Height / 2;
            //构造图片显示区域:让图片的中心点与窗口的中心点一致
            RectangleF picRect = new RectangleF(offsetX, offsetY, image.Width, image.Height);
            PointF Pcenter = new PointF(picRect.X + picRect.Width / 2,
                picRect.Y + picRect.Height / 2);
            // 绘图平面以图片的中心点旋转
            graphics.TranslateTransform(Pcenter.X, Pcenter.Y);
            graphics.RotateTransform(angle);
            //恢复绘图平面在水平和垂直方向的平移
            graphics.TranslateTransform(-Pcenter.X, -Pcenter.Y);
            //绘制图片
            graphics.DrawImage(image, picRect);
        }

        public Bitmap Rotate(Bitmap b, int angle)
        {
            angle = angle % 360;

            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            //原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));

            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            Graphics g = Graphics.FromImage(dsImage);

            g.InterpolationMode = InterpolationMode.Bilinear;

            g.SmoothingMode = SmoothingMode.HighQuality;

            //计算偏移量
            Point Offset = new Point((W - w) / 2, (H - h) / 2);

            //构造图像显示区域：让图像的中心与窗口的中心点一致
            Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(360 - angle);

            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(b, rect);

            //重至绘图的所有变换
            g.ResetTransform();

            g.Save();
            g.Dispose();
            return dsImage;
        }

        public static Bitmap KiRotate(Bitmap bmp, float angle, Color bkColor)
        {
            int w = bmp.Width + 2;
            int h = bmp.Height + 2;

            PixelFormat pf;

            if (bkColor == Color.Transparent)
            {
                pf = PixelFormat.Format32bppArgb;
            }
            else
            {
                pf = bmp.PixelFormat;
            }

            Bitmap tmp = new Bitmap(w, h, pf);
            Graphics g = Graphics.FromImage(tmp);
            g.Clear(bkColor);
            g.DrawImageUnscaled(bmp, 1, 1);
            g.Dispose();

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, w, h));
            Matrix mtrx = new Matrix();
            mtrx.Rotate(angle);
            RectangleF rct = path.GetBounds(mtrx);

            Bitmap dst = new Bitmap((int)rct.Width, (int)rct.Height, pf);
            g = Graphics.FromImage(dst);
            g.Clear(bkColor);
            g.TranslateTransform(-rct.X, -rct.Y);
            g.RotateTransform(angle);
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.DrawImageUnscaled(tmp, 0, 0);
            g.Dispose();

            tmp.Dispose();

            return dst;
        }

        public Form1()
        {
            InitializeComponent();
            btnRegionColor = buttonskin_connect.BaseColor;
        }
        
        private void buttonskin_connect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isConnect)
                {
                    IPAddress ip = IPAddress.Parse(textskin_ip.Text);
                    int port = int.Parse(textSkin_port.Text);
                    tcpClient_connectAgv_port19204 = new TcpClient();
                    IAsyncResult IR = tcpClient_connectAgv_port19204.BeginConnect(ip, port, null, null);
                    IR.AsyncWaitHandle.WaitOne(150, false);
                    if (IR.IsCompleted)
                    {
                        buttonskin_connect.BaseColor = Color.Green;
                        buttonskin_connect.Text = "Disconnect";
                        isConnect = true;
                        if (ths_receiveMes == null)
                        {
                            ths_receiveMes = new ThreadSignal();
                            ths_receiveMes.thread = new Thread(func_receiceMes);
                            ths_receiveMes.SetBack();
                            ths_receiveMes.Start();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Connect Fail");
                    }
                    tcpClient_connectAgv_port19204.EndConnect(IR);
                }
                else
                {
                    buttonskin_connect.BaseColor = btnRegionColor;
                    ths_receiveMes.Abort();
                    ths_receiveMes = null;
                    tcpClient_connectAgv_port19204.Close();
                    buttonskin_connect.Text = "Connect";
                    isConnect = false;
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void func_receiceMes(object obj)
        {
            try
            {
                Action<byte[]> acdelegate = (x) =>
                    {
                        textBox_display.Clear();
                        byte[] buff=new byte [x.Length-16];
                        System.Buffer.BlockCopy(x,16,buff,0,buff.Length);
                        string str = BitConverter.ToString(buff).Replace("-", "-");
                        str = Encoding.ASCII.GetString(buff);
                        textBox_display.Text=str;
                        if (cmdStatu == CMDMaster.getPosition)
                        {
                            agvPositon strJsonObj = JsonConvert.DeserializeObject<agvPositon>(str);
                            MessageBox.Show("angle:[" + strJsonObj.angle.ToString() + "] current_station: [" + strJsonObj.current_station + " ]x/y [" + strJsonObj.x.ToString() + "/" + strJsonObj.y.ToString() + "]");
                        }
                    };
                while (tcpClient_connectAgv_port19204.Connected)
                {
                    Thread.Sleep(10);
                    if (ths_receiveMes.StopThread)
                    { return; }
                    if (tcpClient_connectAgv_port19204.Available > 10)
                    {
                        NetworkStream ns = tcpClient_connectAgv_port19204.GetStream();
                        byte[] retBuffer = new byte[tcpClient_connectAgv_port19204.Available];
                        int retNum = ns.Read(retBuffer, 0, retBuffer.Length);
                        textBox_display.Invoke(acdelegate, retBuffer);
 
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        private void btnskin_getMap_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient_connectAgv_port19204.Connected)
                {
                    NetworkStream ns_send = tcpClient_connectAgv_port19204.GetStream();
                    byte[] sendByte = getTelegramHead(getLaser);
                    cmdStatu = CMDMaster.getLaser;
                    ns_send.Write(sendByte, 0, 16); 
                }
            }
            catch (Exception ex)
            { }
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient_connectAgv_port19204.Connected)
                {
                    NetworkStream ns_send = tcpClient_connectAgv_port19204.GetStream();
                    byte[] sendByte = getTelegramHead(getPosition);
                    cmdStatu = CMDMaster.getPosition;
                    ns_send.Write(sendByte, 0, 16); 
                }
            }
            catch (Exception ex)
            { }
        }

        private void buttonskin_getMap_Click(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient_connectAgv_port19204.Connected)
                {
                    NetworkStream ns_send = tcpClient_connectAgv_port19204.GetStream();
                    byte[] sendByte = getTelegramHead(getMap);
                    cmdStatu = CMDMaster.getLaser;
                    ns_send.Write(sendByte, 0, 16);
                }
            }
            catch (Exception ex)
            { }
        }

        private void skinButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
#if false
                Bitmap a = new Bitmap(imageList1.Images[0]);//得到图片框中的图片
                pictureBox2.Image = KiRotate(a, Convert.ToInt32(skinTextBox1.Text),Color.Blue);
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                //pictureBox1.Location =  .Location;
                pictureBox2.Refresh();//最后刷新图片框
#else
                RotateFormCenter1(pictureBox2, float.Parse(skinTextBox1.Text));
                pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox2.BackgroundImageLayout = ImageLayout.Stretch;
                pictureBox2.Refresh();
#endif
            }
            catch { }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            paintPoint(299, Color.Blue);
        }

        private void paintPoint(int num, Color color)
        {
            Graphics g = panel_back.CreateGraphics();
            Pen p = new Pen(color, 2);
           // g.DrawLine(p, 100, 300, 200, 400);
            for (int i = 0; i < num; i++)
            {
                g.FillEllipse(Brushes.Red, 200+i*4, 400, 2, 2);
            }
            g.Dispose();
        }

        private void panel_back_Paint(object sender, PaintEventArgs e)
        {
            //paintPoint(1, Color.Blue);
        }
    }

    [Serializable]
    public class agvPositon
    {
        public float angle = 0;
        public float confidence = 0;
        public string current_station = "";
        public float x = 0;
        public float y = 0;
    }

    public enum CMDMaster
    {
        empty,
        getPosition,
        getLaser,
        getBattery
    }

}
