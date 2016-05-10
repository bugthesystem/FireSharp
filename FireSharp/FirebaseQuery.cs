using System;
using System.Collections.Generic;
using System.Linq;

namespace FireSharp
{
    public class FirebaseQuery
    {
        private readonly string _initialQuery;
        private string formatParam = "format";
        private string shallowParam = "shallow";
        private string orderByParam = "orderBy";
        private string startAtParam = "startAt";
        private string endAtParam = "endAt";
        private string formatVal = "export";
        private string limitToFirstParam = "limitToFirst";
        private string limitToLastParam = "limitToLast";

        static Dictionary<string, object> _query = new Dictionary<string, object>();

        private FirebaseQuery(string initialQuery = null)
        {
            _initialQuery = initialQuery;
            _query = new Dictionary<string, object>();
        }

        public static FirebaseQuery New(string initialQuery = null)
        {
            return new FirebaseQuery(initialQuery);
        }
        public FirebaseQuery StartAt(string value)
        {
            return AddToQueryDictionary(startAtParam, value);
        }

        public FirebaseQuery EndAt(string value)
        {
            return AddToQueryDictionary(endAtParam, value);
        }

        public FirebaseQuery OrderBy(string value)
        {
            return AddToQueryDictionary(orderByParam, value);
        }

        public FirebaseQuery LimitToFirst(int value)
        {
            return AddToQueryDictionary(limitToFirstParam, value > 0 ? value.ToString() : string.Empty, skipEncoding: true);
        }

        public FirebaseQuery LimitToLast(int value)
        {
            return AddToQueryDictionary(limitToLastParam, value > 0 ? value.ToString() : string.Empty, skipEncoding: true);
        }



        public FirebaseQuery Shallow(bool value)
        {
            return AddToQueryDictionary(shallowParam, value ? "true" : string.Empty, skipEncoding: true);
        }

        public FirebaseQuery IncludePriority(bool value)
        {
            return AddToQueryDictionary(formatParam, value ? formatVal : string.Empty, skipEncoding: true);
        }

        private FirebaseQuery AddToQueryDictionary(string parameterName, string value, bool skipEncoding = false)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _query.Add(parameterName, skipEncoding ? value : EscapeString(value));
            }
            else
            {
                _query.Remove(startAtParam);
            }

            return this;
        }

        private string EscapeString(string value)
        {
            return $"\"{Uri.EscapeDataString(value).Replace("%20", "+").Trim('\"')}\"";
        }

        public string ToQueryString()
        {
            if (!_query.Any() && !string.IsNullOrEmpty(_initialQuery)) return _initialQuery;

            return !string.IsNullOrEmpty(_initialQuery)
                ? $"{_initialQuery}&{string.Join("&", _query.Select(pair => $"{pair.Key}={pair.Value}").ToArray())}"
                : string.Join("&", _query.Select(pair => $"{pair.Key}={pair.Value}").ToArray());
        }
    }
}
