using System;

namespace Json.Comparer.Tests.TestObjects
{
    internal class SimpleObject
    {
        public string Id { get; set; }

        public string StringProperty { get; set; } = "Value";

        public int IntProperty { get; set; } = 100;

        public DateTime DateTimeProperty { get; set; } = new DateTime(2000, 1, 1);

        public int? NullableIntProperty { get; set; } = null;
    }
}