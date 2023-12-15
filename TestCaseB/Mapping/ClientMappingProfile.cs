using AutoMapper;
using TestCaseB.ApiRequestResponse;
using TestCaseB.Models.ViewModels;

namespace TestCaseB.Mapping
{
    public class ClientMappingProfile : Profile
    {
        public ClientMappingProfile()
        {

            CreateSimpleMapping();
            CreateComplexMapping();
        }

        private void CreateSimpleMapping()
        {
            //handles it by default
        }

        private void CreateComplexMapping()
        {


            CreateMap<UserModel, AuthenticateResponse>()
                .ReverseMap();

        }

    }
}
