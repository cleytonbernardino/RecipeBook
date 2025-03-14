using AutoMapper;
using CommonTestUtilities.IdEncription;
using RecipeBook.Application.Services.AutoMapper;
using Sqids;

namespace CommonTestUtilities.Mapper
{
    public class MapperBuilder
    {
        public static IMapper Build()
        {
            SqidsEncoder<long> IdEncripter = IdEncripterBuilder.Build();

            return new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping(IdEncripter));
            }).CreateMapper();
        }
    }
}
