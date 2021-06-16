using System;
using System.Collections.Generic;
using System.Linq;

namespace FireSharp.Core
{
    public class QueryBuilder
    {
        private readonly string initialQuery;
        private const string FormatParam = "format";
        private const string ShallowParam = "shallow";
        private const string OrderByParam = "orderBy";
        private const string StartAtParam = "startAt";
        private const string EndAtParam = "endAt";
        private const string EqualToParam = "equalTo";
        private const string FormatVal = "export";
        private const string LimitToFirstParam = "limitToFirst";
        private const string LimitToLastParam = "limitToLast";
        private const string PrintParam = "print";

        static Dictionary<string, object> _query = new Dictionary<string, object>();

        private QueryBuilder(string initialQuery = "")
        {
            this.initialQuery = initialQuery;
            _query = new Dictionary<string, object>();
        }

        public static QueryBuilder New(string initialQuery = "")
        {
            return new QueryBuilder(initialQuery);
        }
        public QueryBuilder StartAt(string value)
        {
            return AddToQueryDictionary(StartAtParam, value);
        }

        public QueryBuilder StartAt(long value)
        {
            return AddToQueryDictionary(StartAtParam, value);
        }

        public QueryBuilder EndAt(string value)
        {
            return AddToQueryDictionary(EndAtParam, value);
        }

        public QueryBuilder EndAt(long value)
        {
            return AddToQueryDictionary(EndAtParam, value);
        }

        public QueryBuilder EqualTo(string value)
        {
            return AddToQueryDictionary(EqualToParam, value);
        }

        public QueryBuilder OrderBy(string value)
        {
            return AddToQueryDictionary(OrderByParam, value);
        }

        public QueryBuilder LimitToFirst(int value)
        {
            return AddToQueryDictionary(LimitToFirstParam, value > 0 ? value.ToString() : string.Empty, skipEncoding: true);
        }

        public QueryBuilder LimitToLast(int value)
        {
            return AddToQueryDictionary(LimitToLastParam, value > 0 ? value.ToString() : string.Empty, skipEncoding: true);
        }



        public QueryBuilder Shallow(bool value)
        {
            return AddToQueryDictionary(ShallowParam, value ? "true" : string.Empty, skipEncoding: true);
        }

        public QueryBuilder Print(string value)
        {
            return AddToQueryDictionary(PrintParam, value, skipEncoding: true);
        }

        public QueryBuilder IncludePriority(bool value)
        {
            return AddToQueryDictionary(FormatParam, value ? FormatVal : string.Empty, skipEncoding: true);
        }

        private QueryBuilder AddToQueryDictionary(string parameterName, string value, bool skipEncoding = false)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _query.Add(parameterName, skipEncoding ? value : EscapeString(value));
            }
            else
            {
                _query.Remove(StartAtParam);
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
            if (!_query.Any() && !string.IsNullOrEmpty(initialQuery)) return initialQuery;

            return !string.IsNullOrEmpty(initialQuery)
                ? $"{initialQuery}&{string.Join("&", _query.Select(pair => $"{pair.Key}={pair.Value}").ToArray())}"
                : string.Join("&", _query.Select(pair => $"{pair.Key}={pair.Value}").ToArray());
        }
    }
}
