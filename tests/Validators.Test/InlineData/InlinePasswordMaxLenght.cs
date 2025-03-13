using System.Collections;

namespace Validators.Test.InlineData
{
    public class InlinePasswordMaxLenght : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            int minLenght = 6;
            for (int i = (minLenght - 1); i >= 1; i--)
            {
                yield return new object[] { i };
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
