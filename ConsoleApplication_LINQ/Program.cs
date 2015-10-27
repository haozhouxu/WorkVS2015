using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication_LINQ
{
    class Program
    {
        static void Main(string[] args)
        {
            string message = "123";
            List<string> errorList = new List<string>() { "123", "456", "789" };
            if (errorList.Where(x=> message.Contains(x) == true).Count() > 0)
            {
                Console.WriteLine(errorList.Where(x => message.Contains(x) == true).Count());
            }
            Console.ReadKey();
        }
    }
}
