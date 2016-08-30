using Common.Testing.NUnit;
using FluentAssertions;
using NUnit.Framework;

namespace FireSharp.Tests
{
    public class FirebaseQueryTests : TestBase
    {
        [Test]
        public void InitialQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New("test=12&abc=\"xyz\"");
            queryBuilder.ToQueryString().ShouldBeEquivalentTo("test=12&abc=\"xyz\"");
        }

        [Test]
        public void StartAtQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().StartAt("a");
            var queryString = queryBuilder.ToQueryString();
            queryString.ShouldBeEquivalentTo("startAt=\"a\"");
        }
        [Test]
        public void EndAtQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().EndAt("a");
            var queryString = queryBuilder.ToQueryString();
            queryString.ShouldBeEquivalentTo("endAt=\"a\"");
        }

        [Test]
        public void StartAtNumberQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().StartAt(1472342400);
            var queryString = queryBuilder.ToQueryString();
            queryString.ShouldBeEquivalentTo("startAt=1472342400");
        }
        [Test]
        public void EndAtNumberQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().StartAt(1472342400);
            var queryString = queryBuilder.ToQueryString();
            queryString.ShouldBeEquivalentTo("endAt=1472342400");
        }

        [Test]
        public void OrderByQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().OrderBy("test");
            var queryString = queryBuilder.ToQueryString();
            queryString.ShouldBeEquivalentTo("orderBy=\"test\"");
        }
        [Test]
        public void LimitToFirstQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().LimitToFirst(8);
            var queryString = queryBuilder.ToQueryString();
            queryString.ShouldBeEquivalentTo("limitToFirst=8");
        }
        [Test]
        public void LimitToLastQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().LimitToLast(10);
            var queryString = queryBuilder.ToQueryString();
            queryString.ShouldBeEquivalentTo("limitToLast=10");
        }

    }
}
