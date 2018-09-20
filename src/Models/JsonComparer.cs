using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public static class JsonComparer
    {
        public static List<Difference> Compare(JsonObject left, JsonObject right)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            switch (left.JsonType)
            {
                case JsonTypes.Value: return CompareValues(left, right);
                case JsonTypes.Object: return CompareObjects(left, right);
                case JsonTypes.Array: return CompareArrays(left, right);
                default: throw new NotImplementedException();
            }
        }

        private static List<Difference> CompareValues(JsonObject left, JsonObject right)
        {
            if (left.JsonType != JsonTypes.Value) throw new InvalidOperationException();

            var differences = new List<Difference>();

            switch (right.JsonType)
            {
                case JsonTypes.Value:
                    {
                        System.Diagnostics.Debug.Assert(left.Fields.Count == 0);
                        System.Diagnostics.Debug.Assert(right.Fields.Count == 0);

                        if ((left.Id == right.Id) && (left.Value.Equals(right.Value)))
                        {
                            // The objects are equal => no differences
                            break;
                        }
                        else if (left.Id == right.Id)
                        {
                            differences.Add(new Difference(DifferenceTypes.Values, left.Id, left, right));
                        }
                        else
                        {
                            differences.Add(new Difference(DifferenceTypes.OnlyLeftHasProperty, left.Id, left, right));
                        }
                    }
                    break;

                case JsonTypes.Object:
                    {
                        differences.Add(new Difference(DifferenceTypes.ValueAndObject, left.Id, left, right));
                    }
                    break;

                case JsonTypes.Array:
                    {
                        differences.Add(new Difference(DifferenceTypes.ValueAndArray, left.Id, left, right));
                    }
                    break;

                default: throw new NotImplementedException();
            }

            return differences;
        }

        private static List<Difference> CompareObjects(JsonObject left, JsonObject right)
        {
            if (left.JsonType != JsonTypes.Object) throw new InvalidOperationException();
            System.Diagnostics.Debug.Assert(left.Value == null);

            var differences = new List<Difference>();

            switch (right.JsonType)
            {
                case JsonTypes.Value:
                    {
                        differences.Add(new Difference(DifferenceTypes.ObjectAndValue, left.Id, left, right));
                    }
                    break;

                case JsonTypes.Array:
                    {
                        System.Diagnostics.Debug.Assert(right.Value == null);

                        differences.Add(new Difference(DifferenceTypes.ObjectAndArray, left.Id, left, right));
                    }
                    break;

                case JsonTypes.Object:
                    {
                        System.Diagnostics.Debug.Assert(right.Value == null);

                        foreach (var child in left.Fields)
                        {
                            //TODO: check here for positional matching fields

                            if (right.ContainsField(child.Id, child.JsonType))
                            {
                                var diffs = Compare(child, right.Fields[child.Id]);
                                if (diffs.Count() > 0)
                                    differences.AddRange(diffs);
                            }
                            else
                            {
                                differences.Add(new Difference(DifferenceTypes.OnlyLeftHasProperty, child.Id, left, right));
                            }
                        }

                        foreach (var child in right.Fields)
                        {
                            if (!left.ContainsField(child.Id))
                            {
                                differences.Add(new Difference(DifferenceTypes.OnlyRightHasProperty, child.Id, left, right));
                            }
                        }
                    }
                    break;

                default: throw new NotImplementedException();
            }

            return differences;
        }

        private static List<Difference> CompareArrays(JsonObject left, JsonObject right)
        {
            if (left.JsonType != JsonTypes.Array) throw new InvalidOperationException();
            System.Diagnostics.Debug.Assert(left.Value == null);

            var differences = new List<Difference>();

            switch (right.JsonType)
            {
                case JsonTypes.Value:
                    {
                        System.Diagnostics.Debug.Assert(right.Value == null);

                        differences.Add(new Difference(DifferenceTypes.ArrayAndValue, left.Id, left, right));
                    }
                    break;

                case JsonTypes.Object:
                    {
                        System.Diagnostics.Debug.Assert(right.Value == null);

                        differences.Add(new Difference(DifferenceTypes.ArrayAndObject, left.Id, left, right));
                    }
                    break;

                case JsonTypes.Array:
                    {
                        foreach (var child in left.Fields)
                        {
                            //TODO: check here for positional matching fields

                            if (right.ContainsField(child.Id, child.JsonType))
                            {
                                var diffs = Compare(child, right.Fields[child.Id]);
                                if (diffs.Count() > 0)
                                    differences.AddRange(diffs);
                            }
                            else
                            {
                                differences.Add(new Difference(DifferenceTypes.OnlyLeftHasProperty, child.Id, left, right));
                            }
                        }

                        foreach (var child in right.Fields)
                        {
                            if (!left.ContainsField(child.Id))
                            {
                                differences.Add(new Difference(DifferenceTypes.OnlyRightHasProperty, child.Id, left, right));
                            }
                        }
                    }
                    break;

                default: throw new NotImplementedException();
            }

            return differences;
        }
    }
}
