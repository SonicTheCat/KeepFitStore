namespace KeepFitStore.WEB.MappingConfiguration
{
    using AutoMapper;

    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.InputModels.Products;
    using KeepFitStore.Models.ViewModels.Products;

    public class KeepFitProfile : Profile
    {
        public KeepFitProfile()
        {
            //Products - input models
            this.CreateMap<CreateProteinProductInputModel, Protein>(); 
            this.CreateMap<CreateCreatineProductInputModel, Creatine>();
            this.CreateMap<CreateVitaminProductInputModel, Vitamin>();
            this.CreateMap<CreateAminoAcidProducInputModel, AminoAcid>();

            //Products - view models
            this.CreateMap<Product, ProductViewModel>();
        }
    }
}