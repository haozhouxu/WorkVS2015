using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApplication_Async
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private async void wait()
        {
            for (int i = 0; i < 10; i++)
            {
                //await;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {


            long length = await AccessWebAsync();

            // 这里可以做一些不依赖回复的操作
            OtherWork();

            this.test.Text += String.Format("\n 回复的字节长度为:  {0}.\r\n", length);
            //txbMainThreadID.Text = Thread.CurrentThread.ManagedThreadId.ToString();
        }

        private async Task<long> AccessWebAsync()
        {
            MemoryStream content = new MemoryStream();

            // 对MSDN发起一个Web请求
            HttpWebRequest webRequest = WebRequest.Create("http://msdn.microsoft.com/zh-cn/") as HttpWebRequest;
            if (webRequest != null)
            {
                // 返回回复结果
                using (WebResponse response = await webRequest.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        await responseStream.CopyToAsync(content);
                    }
                }
            }

            //txbAsynMethodID.Text = Thread.CurrentThread.ManagedThreadId.ToString();
            return content.Length;
        }

        private void OtherWork()
        {
            this.test.Text += "\r\n等待服务器回复中.................\n";
        }
    }
}
