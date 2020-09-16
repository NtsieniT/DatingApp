using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpers
{
    /*
     * This class tells automapper about the mappings it needs to support
     */
    public class AutoMapperProfiles : Profile 
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserforListDto>()
                .ForMember(destination => destination.PhotoUrl, option =>
                {
                    option.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => opt.ResolveUsing(d => d.DateOfBirth.CalculateAge()));

            CreateMap<User, UserForDetailedDto>().ForMember(destination => destination.PhotoUrl, option =>
            {
                option.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
            }).ForMember(dest => dest.Age, opt => opt.ResolveUsing(d => d.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();

        }

    }
}
