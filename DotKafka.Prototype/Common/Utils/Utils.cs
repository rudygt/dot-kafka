using System;

namespace DotKafka.Prototype.Common.Utils
{
    public static class Utils
    {
        public static T NotNull<T>(T t)
        {
            if (t == null)
                throw new NullReferenceException();
            else
                return t;
        }
    }
}
