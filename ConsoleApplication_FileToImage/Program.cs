using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ConsoleApplication_FileToImage
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] numbers = { 2, 12, 5, 15 };
            //IEnumerable<int> LowNums = from n in numbers
            //                           where n < 10
            //                           select n;
            //var numsMethod = (
            //    from n in numbers
            //    where n < 10
            //    select n).Count();

            //foreach (var x in LowNums)
            //    Console.WriteLine("{0}", x);
            Program test = new Program();
            //test.file16();
            test.a16file();
        }

        public void file16()
        {
            try
            {
                using (FileStream fs = new FileStream("c:\\xhz\\Program.cs", FileMode.Open, FileAccess.Read))
                {
                    //创建一个宽度为1300，高度为704的24位RBG的图片，然后初始化前两行和最后两行为分界线
                    Bitmap bmp = new Bitmap(1300, 604, PixelFormat.Format24bppRgb);
                    int[] height = new int[4] { 0, 1, 602, 603 };
                    foreach (int h in height)
                    {
                        Color SetColor;
                        if (0 == h % 2)
                        {
                            SetColor = Color.Red;
                        }
                        else
                        {
                            SetColor = Color.Black;
                        }

                        for (int w = 0; w < 1300; w++)
                        {
                            bmp.SetPixel(w, h, SetColor);
                        }
                    }


                    BinaryReader br = new BinaryReader(fs);
                    int length = (int)fs.Length; //
                    int i = 1;
                    int x = 0, y = 2;
                    int R = 0, G = 0, B = 0;
                    while (length > 0)
                    {
                        //byte tempByte = br.ReadByte();
                        //string tempStr = Convert.ToString(tempByte, 16);
                        //sw.Write((tempStr.Length > 1 ? "" : "0") + tempStr);
                        //length--;
                        if (1 == i % 3)
                        {
                            R = Convert.ToInt32(br.ReadByte());
                        }
                        else if (2 == i % 3)
                        {
                            G = Convert.ToInt32(br.ReadByte());
                        }
                        else if (0 == i % 3)
                        {
                            B = Convert.ToInt32(br.ReadByte());
                            Color mycolor = Color.FromArgb(R, G, B);
                            bmp.SetPixel(x, y, mycolor);
                            if (1299 == x)
                            {
                                x = 0;
                                y++;
                            }
                            else
                            {
                                x++;
                            }
                        }
                        i++;
                        length--;
                    }
                    fs.Close();
                    br.Close();
                    // sw.Close();
                    bmp.Save("c:\\xhz\\test1.png");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public void a16file()
        {
            //可能是网络问题，导致传输过来的图片有损失。唉唉唉唉，文件大就会，小文件就不会。
            try
            {
                //读取文件
                FileStream fs = new FileStream("c:\\xhz\\file", FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fs);
                //设置文件读取的长度B，因为现在还没自动化，就手动设置
                int filelength = 10928;
                int i = 1;
                int x = 0, y = 0;
                int R = 0, G = 0, B = 0;
                Bitmap bmp = new Bitmap("c:\\xhz\\test2.png");
                //实际图片的大小
                int ReallyWidth = bmp.Width;
                int ReallyHeight = bmp.Height;
                //两个变量为了判断是否是两条线  (可以找到红点)findw = 600findh = 8;
                int findw = 0;
                int findh = 0;
                //实际找到开始的点
                int w = 0;
                int h = 0;
                //用来累计是否长度为1300的，判断是否找到一条红黑相邻的线，如果有一个点上下不为红黑，则设置为0，从来开始计算
                int count = 0;

                //找到读取数据的起始点，就是找到第一条红黑线
                for (; findh < ReallyHeight; findh++)
                {
                    Console.WriteLine(findh);
                    for (findw = 0; findw < ReallyWidth; findw++)
                    {
                        //判断这个点是否是红色的
                        Color FPoint = bmp.GetPixel(findw, findh);
                        Color SPoint = bmp.GetPixel(findw, findh + 1);
                        if ("ffff0000" == FPoint.Name)
                        {
                            //判断高度加1的点是否是黑色的
                            if ("ff000000" == SPoint.Name)
                            {
                                count++;
                                //Console.WriteLine(count);
                                if (1 == count)
                                {
                                    w = findw;
                                    h = findh + 2;
                                }
                                if (1300 == count)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                count = 0;
                                w = 0;
                                h = 0;
                            }
                        }
                        else
                        {
                            count = 0;
                            w = 0;
                            h = 0;
                        }
                    }

                    if (1300 == count)
                    {
                        break;
                    }
                }
                w = 0;
                h = 0;
                for (findh = 0; findh < ReallyHeight; findh++)
                {
                    for (findw = 0; findw < ReallyWidth; findw++)
                    {
                        //判断这个点是否是红色的
                        Color FPoint = bmp.GetPixel(findw, findh);
                        if ("ffff0000" == FPoint.Name)
                        {
                            w = findw;
                            h = findh + 2;
                            break;
                        }
                    }
                    if (w != 0)
                    {
                        break;
                    }
                }

                if ((0 != w) && (0 != h))
                {
                    x = w;
                    y = h;
                    while (filelength > 0)
                    {
                        if (1 == i % 3)
                        {
                            R = bmp.GetPixel(x, y).R;
                            bw.Write(Convert.ToByte(R));
                        }
                        else if (2 == i % 3)
                        {
                            G = bmp.GetPixel(x, y).G;
                            bw.Write(Convert.ToByte(G));
                        }
                        else if (0 == i % 3)
                        {
                            B = bmp.GetPixel(x, y).B;
                            bw.Write(Convert.ToByte(B));
                            if ((w + 1300 - 1) == x)
                            {
                                x = w;
                                y++;
                            }
                            else
                            {
                                x++;
                            }
                        }
                        i++;
                        filelength--;
                    }
                }
                bw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

    }
}
