using System;

namespace Newtonsoft.Json.Comparer
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
        public static void Match(this JTokenComparrisonResult comparrisonResult,
            Action<JObjectComparrisonResult> objectAction,
            Action<JArrayComparrisonResult> arrayAction,
            Action<JPropertyComparrisonResult> propertyAction,
            Action<JValueComparrisonResult> valueAction)
        {
            switch (comparrisonResult.Type)
            {
                case ComparedTokenType.Object:
                    objectAction(comparrisonResult as JObjectComparrisonResult);
                    break;

                case ComparedTokenType.Array:
                    arrayAction(comparrisonResult as JArrayComparrisonResult);
                    break;

                case ComparedTokenType.Property:
                    propertyAction(comparrisonResult as JPropertyComparrisonResult);
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
        public static TResult Match<TResult>(this JTokenComparrisonResult comparrisonResult,
            Func<JObjectComparrisonResult, TResult> objectFunction,
            Func<JArrayComparrisonResult, TResult> arrayFunction,
            Func<JPropertyComparrisonResult, TResult> propertyFunction,
            Func<JValueComparrisonResult, TResult> valueFunction)
        {
            switch (comparrisonResult.Type)
            {
                case ComparedTokenType.Object:
                    return objectFunction(comparrisonResult as JObjectComparrisonResult);

                case ComparedTokenType.Array:
                    return arrayFunction(comparrisonResult as JArrayComparrisonResult);

                case ComparedTokenType.Property:
                    return propertyFunction(comparrisonResult as JPropertyComparrisonResult);

                case ComparedTokenType.Value:
                    return valueFunction(comparrisonResult as JValueComparrisonResult);

                default:
                    throw new ArgumentOutOfRangeException(nameof(comparrisonResult.Type));
            }
        }
    }
}