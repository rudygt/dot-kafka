using System;
using System.Linq;

namespace DotKafka.Prototype.Common.Protocol
{
    public class ApiKeysHelper
    {
        private static int _maxApiKey;

        static ApiKeysHelper()
        {
            _maxApiKey = Enum.GetValues(typeof(ApiKeys)).Cast<int>().Max();
        }

        public static int MaxApiKey
        {
            get { return _maxApiKey; }
        }

        public static ApiKeys ForId(int id)
        {
            return (ApiKeys)id;
        }
    }
}