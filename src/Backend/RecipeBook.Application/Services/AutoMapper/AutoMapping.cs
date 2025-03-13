using AutoMapper;
using RecipeBook.Communication.Requests;
using RecipeBook.Communication.Responses;
using RecipeBook.Domain.Entities;
using Sqids;

namespace RecipeBook.Application.Services.AutoMapper;

public class AutoMapping : Profile
{
    private readonly SqidsEncoder<long> _idEncoder;

    public AutoMapping(SqidsEncoder<long> idEncoder)
    {
        _idEncoder = idEncoder;
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());

        CreateMap<RequestRecipeJson, Recipe>()
            .ForMember(dest => dest.Instructions, opt => opt.Ignore())
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct()))
            .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(
                source => source.DishTypes.Select(type => new DishType { Type = (Domain.Enums.DishType)type }).Distinct())
            );
        CreateMap<string, Ingredient>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source));

        CreateMap<Domain.Enums.DishType, DishType>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

        CreateMap<RequestInstructionJson, Instruction>();
    }

    private void DomainToResponse()
    {
        CreateMap<User, ResponseUserProfileJson>();
        CreateMap<Recipe, ResponseRegisteredRecipeJson>()
            .ForMember(dest => dest.Id, conf => conf.MapFrom(source => _idEncoder.Encode(source.ID)));
    }
}

