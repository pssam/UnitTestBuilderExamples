using AutoMapper;

namespace Tests
{
    public static class MapperContext
    {
        public static IMapper Map { get; } =
            new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PostFull, PostView>()
                   .ForMember(dest => dest.Name,
                       opts => opts.MapFrom(src => src.Title));
            }).CreateMapper();

    }
}