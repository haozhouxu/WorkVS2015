using System;
using System.IO;

namespace ConsoleApplication_FileStream
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream F = new FileStream("test.dat",FileMode.OpenOrCreate,FileAccess.ReadWrite);
            for (int i = 1; i <= 20; i++)
            {
                F.WriteByte((byte)i);
            }

            F.Position = 0;
            for (int i = 0; i <= 30; i++)
            {
                Console.Write(F.Position + "---");
                Console.Write(F.ReadByte()+" ");
                Console.WriteLine(F.Position + "---");
            }
            F.Close();
            Console.ReadKey();
        }
    }
}
