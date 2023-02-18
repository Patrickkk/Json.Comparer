using System;

namespace Json.Comparer
{
    public static class JTokenComparisonResultExtenions
    {
        /// <summary>
        /// Match different types of comparisonResults depending on the type.
        /// </summary>
        /// <param name="comparisonResult"></param>
        /// <param name="objectAction"></param>
        /// <param name="arrayAction"></param>
        /// <param name="propertyAction"></param>
        /// <param name="valueAction"></param>
        public static void Match(this JTokenComparisonResult comparisonResult,
            Action<JObjectComparisonResult> objectAction,
            Action<JArrayComparisonResult> arrayAction,
            Action<JPropertyComparisonResult> propertyAction,
            Action<JValueComparisonResult> valueAction)
        {
            switch (comparisonResult.Type)
            {
                case ComparedTokenType.Object:
                    objectAction(comparisonResult as JObjectComparisonResult);
                    break;

                case ComparedTokenType.Array:
                    arrayAction(comparisonResult as JArrayComparisonResult);
                    break;

                case ComparedTokenType.Property:
                    propertyAction(comparisonResult as JPropertyComparisonResult);
                    break;

                case ComparedTokenType.Value:
                    valueAction(comparisonResult as JValueComparisonResult);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(comparisonResult.Type));
            }
        }

        /// <summary>
        /// Match different types of comparisonResults depending on the type.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="comparisonResult"></param>
        /// <param name="objectFunction"></param>
        /// <param name="arrayFunction"></param>
        /// <param name="propertyFunction"></param>
        /// <param name="valueFunction"></param>
        /// <returns></returns>
        public static TResult Match<TResult>(this JTokenComparisonResult comparisonResult,
            Func<JObjectComparisonResult, TResult> objectFunction,
            Func<JArrayComparisonResult, TResult> arrayFunction,
            Func<JPropertyComparisonResult, TResult> propertyFunction,
            Func<JValueComparisonResult, TResult> valueFunction)
        {
            switch (comparisonResult.Type)
            {
                case ComparedTokenType.Object:
                    return objectFunction(comparisonResult as JObjectComparisonResult);

                case ComparedTokenType.Array:
                    return arrayFunction(comparisonResult as JArrayComparisonResult);

                case ComparedTokenType.Property:
                    return propertyFunction(comparisonResult as JPropertyComparisonResult);

                case ComparedTokenType.Value:
                    return valueFunction(comparisonResult as JValueComparisonResult);

                default:
                    throw new ArgumentOutOfRangeException(nameof(comparisonResult.Type));
            }
        }
    }
}