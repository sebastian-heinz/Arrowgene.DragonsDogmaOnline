using System;

namespace Arrowgene.Ddon.Shared
{
    public static class StringExtension
    {
        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }
    }
}
