namespace KeepFitStore.WEB.MappingConfiguration
{
    using AutoMapper;

    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.InputModels.Products.Aminos;
    using KeepFitStore.Models.InputModels.Products.Creatines;
    using KeepFitStore.Models.InputModels.Products.Proteins;
    using KeepFitStore.Models.InputModels.Products.Vitamins;
    using KeepFitStore.Models.ViewModels.Products;
    using KeepFitStore.Models.ViewModels.Products.Creatines;
    using KeepFitStore.Models.ViewModels.Products.Proteins;
    using KeepFitStore.Models.ViewModels.Products.Vitamins;
    using KeepFitStore.Models.ViewModels.Products.Aminos;
    using KeepFitStore.Domain;
    using KeepFitStore.Models.ViewModels.Basket;

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
            this.CreateMap<Protein, DetailsProteinViewModel>();
            this.CreateMap<Creatine, DetailsCreatineViewModel>();
            this.CreateMap<AminoAcid, DetailsAminoViewModel>();
            this.CreateMap<Vitamin, DetailsVitaminViewModel>();
            this.CreateMap<Product, ProductInBasketViewModel>();
            this.CreateMap<ProductViewModel, ProductInBasketViewModel>();

            //Basket 
            this.CreateMap<BasketItem, BasketViewModel>();
            this.CreateMap<BasketItem, EditBasketItemViewModel>()
                   .ForMember(dest => dest.ProductPrice, 
                                    opt => opt.MapFrom(src => src.Product.Price)); 
        }
    }
}