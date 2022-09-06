using AutoMapper;
using Back_end.DTOs;
using Back_end.Entidades;
using NetTopologySuite.Geometries;

namespace Back_end.Utilities
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile(GeometryFactory geometryFactory)
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<CrearGeneroDTO, Genero>();

            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<CrearActorDTO, Actor>().ForMember( x=> x.Foto, options => options.Ignore());

            CreateMap<Cine, CineDTO>()
                .ForMember(x => x.Latitud, dto => dto.MapFrom(field => field.Ubicacion.Y))
                .ForMember(x => x.Longitud, dto => dto.MapFrom(field => field.Ubicacion.X))
                .ReverseMap();
            CreateMap<CrearCineDTO, Cine>().ForMember(x => x.Ubicacion, x => x.MapFrom(dto =>
                geometryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))
            ));

        }
    }
}
