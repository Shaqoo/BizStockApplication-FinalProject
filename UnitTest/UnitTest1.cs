namespace UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Assert.All(new[] { 1, 2, 3 }, i => Assert.True(i > 0));

            Assert.Contains(1, new[] { 1, 2, 3 });

            Assert.Collection(new List<int> { 1, 2, 3, 4, 5 },
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(4, item),
                item => Assert.Equal(5, item)
            );
        }
        /* "SGkT8n0pwfIY",
  "jk3f9QHQXbkI",
  "15jV89SOomDj",
  "ID4R2ach1pwv",
  "XKfsgxV7s7a0",
  "fBYOuTrpIWic",
  "kobhYg9LbW4s",
  "TlR9zzdGySle",
  "SRL7GXBRI6Mg",
  "jUHnnCBBF9C4"*/
    }
}