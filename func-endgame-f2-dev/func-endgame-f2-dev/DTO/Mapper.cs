using AutoMapper;
using Endgame.Backend.Domain;
using Endgame.DTOs;
using Microsoft.Azure.Cosmos.Spatial;


namespace Endgame.Backend.DTO
{

        public class Mapper
        {
            private static MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LocationDto, Point>().ConvertUsing(l => l == default(LocationDto) ? default(Point) : new Point(l.longitude, l.latitude));
                cfg.CreateMap<Point, LocationDto>().ConvertUsing(p => p == default(Point) ? default(LocationDto) : new LocationDto(p.Position.Longitude, p.Position.Latitude));
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<User, UserDto>();
            });
            public static T Map<T, S>(S dto)
            {
                var mapper = config.CreateMapper();
                return mapper.Map<T>(dto);
            }


        }


}
