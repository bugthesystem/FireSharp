using System;
using System.Collections.Generic;
using System.Linq;

namespace FireSharp.Core
{
    public class QueryBuilder
    {
        private readonly string _initialQuery;
        private string formatParam = "format";
        private string shallowParam = "shallow";
        private string orderByParam = "orderBy";
        private string startAtParam = "startAt";
        private string endAtParam = "endAt";
        private string eqaulToParam = "equalTo";
        private string formatVal = "export";
        private string limitToFirstParam = "limitToFirst";
        private string limitToLastParam = "limitToLast";
        private string printParam = "print";

        static Dictionary<string, object> _query = new Dictionary<string, object>();

        private QueryBuilder(string initialQuery = "")
        {
            _initialQuery = initialQuery;
            _query = new Dictionary<string, object>();
        }

        public static QueryBuilder New(string initialQuery = "")
        {
            return new QueryBuilder(initialQuery);
        }
        public QueryBuilder StartAt(string value)
        {
            return AddToQueryDictionary(startAtParam, value);
        }

        public QueryBuilder StartAt(long value)
        {
            return AddToQueryDictionary(startAtParam, value);
        }

        public QueryBuilder EndAt(string value)
        {
            return AddToQueryDictionary(endAtParam, value);
        }

        public QueryBuilder EndAt(long value)
        {
            return AddToQueryDictionary(endAtParam, value);
        }

        public QueryBuilder EqualTo(string value)
        {
            return AddToQueryDictionary(eqaulToParam, value);
        }

        public QueryBuilder OrderBy(string value)
        {
            return AddToQueryDictionary(orderByParam, value);
        }

        public QueryBuilder LimitToFirst(int value)
        {
            return AddToQueryDictionary(limitToFirstParam, value > 0 ? value.ToString() : string.Empty, skipEncoding: true);
        }

        public QueryBuilder LimitToLast(int value)
        {
            return AddToQueryDictionary(limitToLastParam, value > 0 ? value.ToString() : string.Empty, skipEncoding: true);
        }



        public QueryBuilder Shallow(bool value)
        {
            return AddToQueryDictionary(shallowParam, value ? "true" : string.Empty, skipEncoding: true);
        }

        public QueryBuilder Print(string value)
        {
            return AddToQueryDictionary(printParam, value, skipEncoding: true);
        }

        public QueryBuilder IncludePriority(bool value)
        {
            return AddToQueryDictionary(formatParam, value ? formatVal : string.Empty, skipEncoding: true);
        }

        private QueryBuilder AddToQueryDictionary(string parameterName, string value, bool skipEncoding = false)
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

        private QueryBuilder AddToQueryDictionary(string parameterName, long value)
        {
            _query.Add(parameterName, value);
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
