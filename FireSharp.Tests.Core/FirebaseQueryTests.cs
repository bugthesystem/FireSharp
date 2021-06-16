using FireSharp.Core;
using FireSharp.Tests.Core.Commom;
using FluentAssertions;
using NUnit.Framework;

namespace FireSharp.Tests.Core
{
    [TestFixture]
    public class FirebaseQueryTests : TestBase
    {
        [Test]
        public void InitialQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New("test=12&abc=\"xyz\"");
            queryBuilder.ToQueryString().Should().BeEquivalentTo("test=12&abc=\"xyz\"");
        }

        [Test]
        public void StartAtQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().StartAt("a");
            string queryString = queryBuilder.ToQueryString();
            queryString.Should().BeEquivalentTo("startAt=\"a\"");
        }
        [Test]
        public void EndAtQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().EndAt("a");
            string queryString = queryBuilder.ToQueryString();
            queryString.Should().BeEquivalentTo("endAt=\"a\"");
        }

        [Test]
        public void StartAtNumberQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().StartAt(1472342400);
            string queryString = queryBuilder.ToQueryString();
            queryString.Should().BeEquivalentTo("startAt=1472342400");
        }
        [Test]
        public void EndAtNumberQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().EndAt(1472342400);
            string queryString = queryBuilder.ToQueryString();
            queryString.Should().BeEquivalentTo("endAt=1472342400");
        }

        [Test]
        public void EqualToQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().EqualTo("test");
            string queryString = queryBuilder.ToQueryString();
            queryString.Should().BeEquivalentTo("equalTo=\"test\"");
        }

        [Test]
        public void OrderByQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().OrderBy("test");
            string queryString = queryBuilder.ToQueryString();
            queryString.Should().BeEquivalentTo("orderBy=\"test\"");
        }
        [Test]
        public void LimitToFirstQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().LimitToFirst(8);
            string queryString = queryBuilder.ToQueryString();
            queryString.Should().BeEquivalentTo("limitToFirst=8");
        }
        [Test]
        public void LimitToLastQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().LimitToLast(10);
            string queryString = queryBuilder.ToQueryString();
            queryString.Should().BeEquivalentTo("limitToLast=10");
        }
        [Test]
        public void ShallowParameterTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().Shallow(true);
            string queryString = queryBuilder.ToQueryString();
            queryString.Should().BeEquivalentTo("shallow=true");
        }
        [Test]
        public void PrintParameterTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().Print("silent");
            string queryString = queryBuilder.ToQueryString();
            queryString.Should().BeEquivalentTo("print=silent");
        }

    }
}
