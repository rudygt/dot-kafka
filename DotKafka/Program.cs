using System;
using DotKafka.Prototype.Common.Protocol;

namespace DotKafka
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Max Value " + ApiKeysHelper.MaxApiKey);

            var test = ApiKeysHelper.ForId(1);

            Console.WriteLine("ApiKeys(1) " + test);

            Console.ReadLine();
        }
    }
}
