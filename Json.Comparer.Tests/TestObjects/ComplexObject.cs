using System.Collections.Generic;

namespace Json.Comparer.Tests.TestObjects
{
    internal class ComplexObject : SimpleObject
    {
        public ComplexObject(bool includeChildren)
        {
            if (includeChildren)
            {
                Children = new List<ComplexObject>()
                {
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                    new ComplexObject(false),
                };
            }
        }

        public IEnumerable<ComplexObject> Children { get; set; } = new List<ComplexObject>();

        public SimpleObject SingleChildObject { get; set; }
    }
}