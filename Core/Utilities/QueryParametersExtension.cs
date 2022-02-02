using System;

namespace Core.Utilities
{
    public static class QueryParametersExtension
    {
        public static bool HasQuery(this QueryParameters queryParameters)
        {
            return !String.IsNullOrEmpty(queryParameters.Query);
        }
    }
}