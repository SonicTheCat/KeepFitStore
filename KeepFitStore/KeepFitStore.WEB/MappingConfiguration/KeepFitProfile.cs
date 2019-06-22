namespace KeepFitStore.WEB.MappingConfiguration
{
    using AutoMapper;

    using KeepFitStore.Models.Products;
    using Areas.Administrator.Models.InputModels.Products;

    public class KeepFitProfile : Profile
    {
        public KeepFitProfile()
        {
            //Products
            this.CreateMap<CreateProteinProductInputModel, Protein>();
            this.CreateMap<CreateCreatineProductInputModel, Creatine>();
        }
    }
}