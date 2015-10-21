using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApplication_WebBrower
{
    /// <summary>
    /// test.xaml 的交互逻辑
    /// </summary>
    public partial class test : Window
    {
        public test()
        {
            InitializeComponent();
        }

        private void SendToUIThread(UIElement element, string text)
        {
            element.Dispatcher.BeginInvoke(
                new Action(() => { SendKeys.Send(text); }),
                DispatcherPriority.Input
            );
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            tb1.Foreground = new SolidColorBrush(Colors.Red);
            tb2.Foreground = new SolidColorBrush(Colors.Blue);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Thread.Sleep(3000);

                SendToUIThread(tb1, "Hello");
                Thread.Sleep(1000);

                SendToUIThread(tb1, " W");
                Thread.Sleep(1000);

                SendToUIThread(tb1, "o");
                Thread.Sleep(1000);

                SendToUIThread(tb1, "r");
                Thread.Sleep(1000);

                SendToUIThread(tb1, "ld!");
                Thread.Sleep(1000);

                SendToUIThread(mainWin, "^c");
                Thread.Sleep(1000);

                SendToUIThread(btn, "{ENTER}");
            });
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control) && e.Key == Key.C)
            {
                tb2.Text = tb1.Text;
            }
        }
    }
}
