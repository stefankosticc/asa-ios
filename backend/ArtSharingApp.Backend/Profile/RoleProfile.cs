using ArtSharingApp.Backend.DTO;
using ArtSharingApp.Backend.Models;

namespace ArtSharingApp.Backend.Profile;

public class RoleProfile : AutoMapper.Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleResponseDTO>();
        CreateMap<RoleRequestDTO, Role>();
    }
}
