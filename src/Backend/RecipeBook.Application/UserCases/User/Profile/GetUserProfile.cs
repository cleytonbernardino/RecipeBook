using AutoMapper;
using RecipeBook.Communiction.Responses;
using RecipeBook.Domain.Services.LoggedUser;

namespace RecipeBook.Application.UserCases.User.Profile
{
    public class GetUserProfile : IGetUserProfile
    {
        private readonly ILoggedUser _loggedUser;
        private readonly IMapper _mapper;

        public GetUserProfile(ILoggedUser loggedUser, IMapper mapper)
        {
            _loggedUser = loggedUser;
            _mapper = mapper;
        }

        public async Task<ResponseUserProfileJson> Execute()
        {
            Domain.Entities.User user = await _loggedUser.User();
            return _mapper.Map<ResponseUserProfileJson>(user);
        }
    }
}
