using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Web;

namespace ConsoleApplication_DeBugInfo
{
    [AttributeUsage(AttributeTargets.Class|
        AttributeTargets.Constructor|
        AttributeTargets.Field|
        AttributeTargets.Method|
        AttributeTargets.Property,
        AllowMultiple = true)]
    public class DeBugInfo : System.Attribute
    {
        private int bugNo;
        private string developer;
        private string lastReview;
        public string message;

        public DeBugInfo(int bg,string dev,string d)
        {
            this.bugNo = bg;
            this.developer = dev;
            this.lastReview = d;
        }

        public int BugNo
        {
            get
            {
                return bugNo;
            }
        }

        public string Developer
        {
            get
            {
                return developer;
            }
        }

        public string LastReview
        {
            get
            {
                return lastReview;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //写了一半，不知道怎么用。特性还不是很熟悉。

            //测试其他东西
            byte[] result = Encoding.Default.GetBytes("000002");
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            Console.WriteLine(output);
            Console.ReadKey();
        }
    }
}
