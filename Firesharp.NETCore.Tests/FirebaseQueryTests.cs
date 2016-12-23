using Xunit;

namespace FireSharp.NETCore.Tests
{
    public class FirebaseQueryTests
    {
        [Fact]
        public void InitialQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New("test=12&abc=\"xyz\"");
            var queryString = queryBuilder.ToQueryString();
            Assert.Equal(queryString, "test=12&abc=\"xyz\"");
        }

        [Fact]
        public void StartAtQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().StartAt("a");
            var queryString = queryBuilder.ToQueryString();
            Assert.Equal(queryString, "startAt=\"a\"");
        }

        [Fact]
        public void EndAtQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().EndAt("a");
            var queryString = queryBuilder.ToQueryString();
            Assert.Equal(queryString, "endAt=\"a\"");
        }

        [Fact]
        public void StartAtNumberQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().StartAt(1472342400);
            var queryString = queryBuilder.ToQueryString();
            Assert.Equal(queryString, "startAt=1472342400");
        }

        [Fact]
        public void EndAtNumberQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().EndAt(1472342400);
            var queryString = queryBuilder.ToQueryString();
            Assert.Equal(queryString, "endAt=1472342400");
        }

        [Fact]
        public void OrderByQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().OrderBy("test");
            var queryString = queryBuilder.ToQueryString();
            Assert.Equal(queryString, "orderBy=\"test\"");
        }

        [Fact]
        public void LimitToFirstQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().LimitToFirst(8);
            var queryString = queryBuilder.ToQueryString();
            Assert.Equal(queryString, "limitToFirst=8");
        }

        [Fact]
        public void LimitToLastQueryTest()
        {
            QueryBuilder queryBuilder = QueryBuilder.New().LimitToLast(10);
            var queryString = queryBuilder.ToQueryString();
            Assert.Equal(queryString, "limitToLast=10");
        }

    }
}
