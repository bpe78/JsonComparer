using System;

namespace Models
{
    public class Difference
    {
        public Difference(DifferenceTypes type, string property, JsonObject left, JsonObject right)
        {
            DifferenceType = type;
            PropertyName = property;
            LeftNode = left;
            RightNode = right;
        }

        public DifferenceTypes DifferenceType { get; }
        public string PropertyName { get; }
        public JsonObject LeftNode { get; }
        public JsonObject RightNode { get; }
    }
}
