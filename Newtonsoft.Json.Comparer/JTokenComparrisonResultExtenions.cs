using System;

namespace Newtonsoft.Json.Comparer
{
    public static class JTokenComparrisonResultExtenions
    {
        public static void Match(this JTokenComparrisonResult comparrisonResult,
            Action<JObjectComparrisonResult> objectAction,
            Action<JArrayComparrisonResult> arrayAction,
            Action<JPropertyComparrisonResult> propertyAction,
            Action<JValueComparrisonResult> valueAction)
        {
            switch (comparrisonResult.Type)
            {
                case ComparisonResultType.Object:
                    objectAction(comparrisonResult as JObjectComparrisonResult);
                    break;

                case ComparisonResultType.Array:
                    arrayAction(comparrisonResult as JArrayComparrisonResult);
                    break;

                case ComparisonResultType.Property:
                    propertyAction(comparrisonResult as JPropertyComparrisonResult);
                    break;

                case ComparisonResultType.Value:
                    valueAction(comparrisonResult as JValueComparrisonResult);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(comparrisonResult.Type));
                    break;
            }
        }

        public static TResult Match<TResult>(this JTokenComparrisonResult comparrisonResult,
            Func<JObjectComparrisonResult, TResult> objectAction,
            Func<JArrayComparrisonResult, TResult> arrayAction,
            Func<JPropertyComparrisonResult, TResult> propertyAction,
            Func<JValueComparrisonResult, TResult> valueAction)
        {
            switch (comparrisonResult.Type)
            {
                case ComparisonResultType.Object:
                    return objectAction(comparrisonResult as JObjectComparrisonResult);
                    break;

                case ComparisonResultType.Array:
                    return arrayAction(comparrisonResult as JArrayComparrisonResult);
                    break;

                case ComparisonResultType.Property:
                    return propertyAction(comparrisonResult as JPropertyComparrisonResult);
                    break;

                case ComparisonResultType.Value:
                    return valueAction(comparrisonResult as JValueComparrisonResult);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(comparrisonResult.Type));
                    break;
            }
        }
    }
}