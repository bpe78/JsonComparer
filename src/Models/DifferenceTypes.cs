using System;

namespace Models
{
    public enum DifferenceTypes
    {
        None = 0,
        Identical,
        Values,
        OnlyLeftHasProperty,
        OnlyRightHasProperty,
        ValueAndObject,
        ValueAndArray,
        ObjectAndValue,
        ObjectAndArray,
        ArrayAndValue,
        ArrayAndObject,
    }
}
