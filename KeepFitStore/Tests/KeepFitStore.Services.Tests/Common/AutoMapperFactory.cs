namespace KeepFitStore.Services.Tests.Common
{
    using AutoMapper;

    using KeepFitStore.WEB.MappingConfiguration;

    public static class AutoMapperFactory
    {
        public static IMapper Initialize()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new KeepFitProfile());
            });

            return mappingConfig.CreateMapper();
        }
    }
}