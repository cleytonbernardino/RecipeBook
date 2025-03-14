using Sqids;

namespace CommonTestUtilities.IdEncription
{
    public class IdEncripterBuilder
    {
        public static SqidsEncoder<long> Build()
        {
            return new SqidsEncoder<long>(new()
            {
                MinLength = 3,
                Alphabet = "aJVu3P4s0foxAqivmTdrGO1ynS6eMtRLwEFzZkDgCNcj2IHpK7l5bXYWhUBQ89"
            });
        }
    }
}
