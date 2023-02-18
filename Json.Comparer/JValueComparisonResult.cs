﻿using NullGuard;

namespace Json.Comparer
{
    /// <summary>
    /// The result of a Comparison of JToken.
    /// </summary>
    public class JValueComparisonResult : JTokenComparisonResult
    {
        /// <summary>
        /// The type of JToken compared
        /// </summary>
        public override ComparedTokenType Type { get; } = ComparedTokenType.Value;

        /// <summary>
        /// The value from source1 used for the comparrison
        /// </summary>
        [AllowNull]
        public string Source1Value { get; set; }

        /// <summary>
        /// The value from source2 used for the comparrison
        /// </summary>
        [AllowNull]
        public string Source2Value { get; set; }
    }
}