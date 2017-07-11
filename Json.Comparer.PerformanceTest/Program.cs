using System;
using Newtonsoft.Json.Linq;

namespace Json.Comparer.PerformanceTest
{
    internal class Program
    {
        private static Random random = new Random(1);

        private static void Main(string[] args)
        {
            try
            {
                var item1 = NewRecursiveClass(0);
                var item2 = NewRecursiveClass(0);
                var jObject1 = JToken.FromObject(item1);
                var jObject2 = JToken.FromObject(item2);
                var comparer = new JTokenComparer();

                for (int i = 0; i < 100000; i++)
                {
                    comparer.CompareTokens("root", jObject1, jObject2);
                }
            }
            catch (Exception e)
            {
                var a = e;
            }
        }

        private static RecursiveClass NewRecursiveClass(int depth)
        {
            var item = new RecursiveClass();
            var childrenCount = random.Next(13 - depth);
            item.Name = random.Next(2) == 0 ? "Name0" : "Name1";

            for (int i = 0; i < childrenCount; i++)
            {
                item.Children.Add(NewRecursiveClass(depth + 1));
            }
            return item;
        }
    }
}