using System.Collections;

namespace WebApi.Test.InlineData
{
    public class CultureInlineDataTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "en" }; // Default
            yield return new object[] { "pt-BR" };
            yield return new object[] { "pt-PT" };
            yield return new object[] { "es" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
