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

using System.Diagnostics;
using System.IO;

namespace MyLogger
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            try
            {
                this.tb1.Text = "test1";
                this.tb2.Text = "test2";

                int x = 1;
                int y = 0;
                int xy = x / y;

            }
            catch (Exception ex)
            {

                Logger.Log("MainWindow", ex);
            }
        }
    }


    public class MyTraceListener : TraceListener
    {
        public string FilePath { get; private set; }

        public MyTraceListener(string filepath)
        {
            FilePath = filepath;
        }

        public MyTraceListener()
        {
            FilePath = @"Log/1.log";
        }

        public override void Write(string message)
        {
            File.AppendAllText(FilePath, message);
        }

        public override void WriteLine(string message)
        {
            File.AppendAllText(FilePath, Environment.NewLine + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine + message + Environment.NewLine);
        }

        public override void Write(object o, string category)
        {
            string msg = "";

            if (string.IsNullOrWhiteSpace(category) == false) //category参数不为空
            {
                msg = category + Environment.NewLine;
            }

            if (o is Exception) //如果参数o是异常类,输出异常消息+堆栈,否则输出o.ToString()
            {
                var ex = (Exception)o;
                msg += "Message : " + ex.Message + Environment.NewLine;
                msg += "Source : " + ex.Source + Environment.NewLine;
                msg += "StackTrace : " + ex.StackTrace;
            }
            else if (o != null)
            {
                msg = o.ToString();
            }

            WriteLine(msg);
        }
    }

    public static class Logger
    {
        public static void Log(string MethodName, Exception ex)
        {
            Trace.Listeners.Clear();  //清除系统监听器 (就是输出到Console的那个)
            Trace.Listeners.Add(new MyTraceListener()); //添加MyTraceListener实例
            Trace.Write(ex, MethodName);
        }
    }

}
