using System;

namespace Json.Comparer
{
    public static class JTokenComparrisonResultExtenions
    {
        /// <summary>
        /// Match different types of comparrisonResults depending on the type.
        /// </summary>
        /// <param name="comparrisonResult"></param>
        /// <param name="objectAction"></param>
        /// <param name="arrayAction"></param>
        /// <param name="propertyAction"></param>
        /// <param name="valueAction"></param>
        public static void Match(this JTokenComparisonResult comparrisonResult,
            Action<JObjectComparisonResult> objectAction,
            Action<JArrayComparrisonResult> arrayAction,
            Action<JPropertyComparisonResult> propertyAction,
            Action<JValueComparrisonResult> valueAction)
        {
            switch (comparrisonResult.Type)
            {
                case ComparedTokenType.Object:
                    objectAction(comparrisonResult as JObjectComparisonResult);
                    break;

                case ComparedTokenType.Array:
                    arrayAction(comparrisonResult as JArrayComparrisonResult);
                    break;

                case ComparedTokenType.Property:
                    propertyAction(comparrisonResult as JPropertyComparisonResult);
                    break;

                case ComparedTokenType.Value:
                    valueAction(comparrisonResult as JValueComparrisonResult);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(comparrisonResult.Type));
            }
        }

        /// <summary>
        /// Match different types of comparrisonResults depending on the type.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="comparrisonResult"></param>
        /// <param name="objectFunction"></param>
        /// <param name="arrayFunction"></param>
        /// <param name="propertyFunction"></param>
        /// <param name="valueFunction"></param>
        /// <returns></returns>
        public static TResult Match<TResult>(this JTokenComparisonResult comparrisonResult,
            Func<JObjectComparisonResult, TResult> objectFunction,
            Func<JArrayComparrisonResult, TResult> arrayFunction,
            Func<JPropertyComparisonResult, TResult> propertyFunction,
            Func<JValueComparrisonResult, TResult> valueFunction)
        {
            switch (comparrisonResult.Type)
            {
                case ComparedTokenType.Object:
                    return objectFunction(comparrisonResult as JObjectComparisonResult);

                case ComparedTokenType.Array:
                    return arrayFunction(comparrisonResult as JArrayComparrisonResult);

                case ComparedTokenType.Property:
                    return propertyFunction(comparrisonResult as JPropertyComparisonResult);

                case ComparedTokenType.Value:
                    return valueFunction(comparrisonResult as JValueComparrisonResult);

                default:
                    throw new ArgumentOutOfRangeException(nameof(comparrisonResult.Type));
            }
        }
    }
}