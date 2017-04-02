namespace Rsdn.Text
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public static class TypeExtensions
    {
        public static int NthIndexOf(this string str, char ch, int num, int startIndex = 0)
        {
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(num));
            }

            var index = startIndex - 1;

            for (var i = 0; i != num; ++i)
            {
                index = str.IndexOf(ch, index + 1);
                if (index < 0)
                    break;
            }

            return index;
        }
    }
}