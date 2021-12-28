using AmoClient;
using System;
using System.IO;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");

            var str = File.ReadAllText(@"C:\RoistatApi\amo.json");

            var leads = AmoParcer.ParceLeads(str);

        }
    }
}