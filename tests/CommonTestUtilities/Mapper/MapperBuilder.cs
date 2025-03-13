using AutoMapper;
using RecipeBook.Application.Services.AutoMapper;
using Sqids;

namespace CommonTestUtilities.Mapper
{
    public class MapperBuilder
    {
        public static IMapper Build()
        {
            SqidsEncoder<long> sqids = new(new()
            {
                MinLength = 3,
                Alphabet = "aJVu3P4s0foxAqivmTdrGO1ynS6eMtRLwEFzZkDgCNcj2IHpK7l5bXYWhUBQ89"
            });

            return new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping(sqids));
            }).CreateMapper();
        }
    }
}
