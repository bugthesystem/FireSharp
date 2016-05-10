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
            FirebaseQuery query = FirebaseQuery.New("test=12&abc=\"xyz\"");
            query.ToQueryString().ShouldBeEquivalentTo("test=12&abc=\"xyz\"");
        }

        [Test]
        public void StartAtQueryTest()
        {
            FirebaseQuery query = FirebaseQuery.New().StartAt("a");
            var queryString = query.ToQueryString();
            queryString.ShouldBeEquivalentTo("startAt=\"a\"");
        }
        [Test]
        public void EndAtQueryTest()
        {
            FirebaseQuery query = FirebaseQuery.New().StartAt("a");
            var queryString = query.ToQueryString();
            queryString.ShouldBeEquivalentTo("startAt=\"a\"");
        }
        [Test]
        public void OrderByQueryTest()
        {
            FirebaseQuery query = FirebaseQuery.New().OrderBy("test");
            var queryString = query.ToQueryString();
            queryString.ShouldBeEquivalentTo("orderBy=\"test\"");
        }
        [Test]
        public void LimitToFirstQueryTest()
        {
            FirebaseQuery query = FirebaseQuery.New().LimitToFirst(8);
            var queryString = query.ToQueryString();
            queryString.ShouldBeEquivalentTo("limitToFirst=8");
        }
        [Test]
        public void LimitToLastQueryTest()
        {
            FirebaseQuery query = FirebaseQuery.New().LimitToLast(10);
            var queryString = query.ToQueryString();
            queryString.ShouldBeEquivalentTo("limitToLast=10");
        }

    }
}
