using System;
using System.Linq;
using System.Text;

namespace Backend.Core.Helper
{
    public static class UniqGenerator
    {
        public static string Generate()
        {
            StringBuilder builder = new StringBuilder();
            Enumerable.Range(65, 26).Select(e => ((char)e).ToString())
                                    .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                                    .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                                    .OrderBy(e => Guid.NewGuid())
                                    .Take(16)
                                    .ToList().ForEach(e => builder.Append(e));

            string guid = Guid.NewGuid().ToString();
            string result = builder.ToString();

            return $"{guid}-{result}";
        }

        public static string Generate(int lenght)
        {
            StringBuilder builder = new StringBuilder();
            Enumerable.Range(65, 26)
                                    .Select(e => ((char)e).ToString())
                                    .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                                    .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                                    .OrderBy(e => Guid.NewGuid())
                                    .Take(lenght)
                                    .ToList().ForEach(e => builder.Append(e));

            return builder.ToString();
        }

    }
}
