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
            string gsdm = "000002";
            InitializeComponent();
            wbSample.Navigate(url + "?stockCode=" + gsdm + "&sign=" + GetMD5(gsdm));
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

        private void txtUrl_KeyUp(object sender, KeyEventArgs e)
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
