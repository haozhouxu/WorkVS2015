#define DEBUG
using System;
using System.Diagnostics;

namespace ConsoleApplication_Attribute
{
    public class Myclass
    {
        [Conditional("DEBUG")]
        public static void Message(string msg)
        {
            Console.WriteLine(msg);
        }
    }

    class Program
    {
        static void function1()
        {
            Myclass.Message("In Function 1.");
            function2();
        }

        private static void function2()
        {
            Myclass.Message("In Function 2.");
        }

        public static void Main(string[] args)
        {
            Myclass.Message("In Main Function.");
            function1();
            Console.ReadKey();
        }
    }
}
