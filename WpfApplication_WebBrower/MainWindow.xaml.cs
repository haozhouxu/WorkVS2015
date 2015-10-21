using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Interop;

namespace WebBrowserTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //http://yuqing.p5w.net/yqjc/views/yqjc/dispatch?stockCode=000002&sign=4dd68a2eb0a79e4a85da2016ef5dc4cd
            string url = "http://yuqing.p5w.net/yqjc/views/yqjc/dispatch";
            string gsdm = "002236";
            InitializeComponent();
            //wbSample.Navigate(url + "?stockCode=" + gsdm + "&sign=" + GetMD5(gsdm));
            //签约的客户：
            //苏宁云商    002024
            //南京银行    601009
            //兴业银行    601166

            //大华股份  002236
            //证通电子   002197
            //陕天然气   002267
            //这三家也是签约用户

            //非签约客户：
            //天玑科技    300245
            //上汽集团    600104
            //上海医药    601607


            wbSample.Navigate("http://wltp.cninfo.com.cn/gddh_vote/cis/logon.do");
            //txtUrl.Focus();
            //System.Windows.Forms.SendKeys.SendWait("{BS}");
            //System.Windows.Input.Key.Back;
            //wbSample.Focus();
            //IntPtr windowHandle = new WindowInteropHelper(this).Handle;

            //wbSample.Dispatcher.BeginInvoke(
            //    new Action(() => { SendKeys.SendWait("{PGDN}"); }),
            //    DispatcherPriority.Input
            //);
            //Keyboard.Press(Key.Enter);
            //System.Windows.Forms.SendKeys.Send("^{-}");

        }

        private string GetMD5(string input)
        {
            string salt = "12345";

            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input + salt));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private void txtUrl_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                if (txtUrl.Text.Contains("http://") || txtUrl.Text.Contains("https://"))
                {
                    wbSample.Navigate(txtUrl.Text);
                }
                else
                {
                    try
                    {
                        wbSample.Navigate("https://" + txtUrl.Text);
                    }
                    catch (Exception)
                    {

                        try
                        {
                            wbSample.Navigate("http://" + txtUrl.Text);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
        }

        private void wbSample_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            txtUrl.Text = e.Uri.OriginalString;
        }

        private void BrowseBack_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((wbSample != null) && (wbSample.CanGoBack));
        }

        private void BrowseBack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbSample.GoBack();
        }

        private void BrowseForward_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ((wbSample != null) && (wbSample.CanGoForward));
        }

        private void BrowseForward_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbSample.GoForward();
        }

        private void GoToPage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void GoToPage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            wbSample.Navigate(txtUrl.Text);
        }
    }
}
