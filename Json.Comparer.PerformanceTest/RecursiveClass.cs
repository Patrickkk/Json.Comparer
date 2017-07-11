using System.Collections.Generic;

namespace Json.Comparer.PerformanceTest
{
    public class RecursiveClass
    {
        public string Name { get; set; }

        public List<RecursiveClass> Children { get; set; } = new List<RecursiveClass>();
    }
}