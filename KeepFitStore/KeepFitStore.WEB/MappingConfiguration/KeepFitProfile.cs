namespace KeepFitStore.WEB.MappingConfiguration
{
    using AutoMapper;

    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.InputModels.Products;
    
    public class KeepFitProfile : Profile
    {
        public KeepFitProfile()
        {
            //Products
            this.CreateMap<CreateProteinProductInputModel, Protein>(); 
            this.CreateMap<CreateCreatineProductInputModel, Creatine>();
            this.CreateMap<CreateVitaminProductInputModel, Vitamin>();
            this.CreateMap<CreateAminoAcidProducInputModel, AminoAcid>();
        }
    }
}