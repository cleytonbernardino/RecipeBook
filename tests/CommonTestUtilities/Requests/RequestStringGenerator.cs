using Bogus;

namespace CommonTestUtilities.Requests
{
    public class RequestStringGenerator
    {
        public static string Paragraphs(int minCharacters)
        {
            Faker faker = new();

            string longText = faker.Lorem.Paragraphs(7);

            while (longText.Length < minCharacters)
            {
                longText = $"{longText}{faker.Lorem.Paragraph()}";
            }
            return longText;
        }
    }
}
