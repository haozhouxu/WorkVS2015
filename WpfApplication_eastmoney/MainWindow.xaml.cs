using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace WpfApplication_eastmoney
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dowmload();
        }

        private void dowmload()
        {
            string date = this.targetdate.Text;
            string sURL;
            int i = 1;
            string url1 = "http://datainterface.eastmoney.com/EM_DataCenter/JS.aspx?type=SR&sty=YJBB&fd=2015-12-31&st=13&sr=-1&ps=50&p=";

            sURL = url1+i;
            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            //WebProxy myProxy = new WebProxy("myproxy", 80);
            //myProxy.BypassProxyOnLocal = true;

            //wrGETURL.Proxy = WebProxy.GetDefaultProxy();

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            string html = string.Empty;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    html += sLine;
            }
            var ss = html.Replace("(","").Replace(")", "").ToArray();
            MessageBox.Show(html);
        }
    }
}
