using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication_Obsolete
{
    class Program
    {
        [Obsolete("Don't use OldMethod, use NewMethod instead",true)]
        static void OldMethod()
        {
            Console.WriteLine("It is the old method");
        }

        static void NewMethod()
        {
            Console.WriteLine("It is the new method");
        }

        static void Main(string[] args)
        {
            OldMethod();
        }
    }
}
