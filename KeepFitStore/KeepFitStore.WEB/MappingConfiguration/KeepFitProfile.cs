namespace KeepFitStore.WEB.MappingConfiguration
{
    using System.Linq;

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
    using KeepFitStore.Models.InputModels.Reviews;
    using KeepFitStore.Models.ViewModels.Reviews;
    using KeepFitStore.Models.InputModels.Address;
    using KeepFitStore.Models.ViewModels.Address;
    using KeepFitStore.Models.ViewModels.User;
    using KeepFitStore.Models.InputModels.Orders;
    using KeepFitStore.Models.ViewModels.Orders;
    using KeepFitStore.Helpers;

    public class KeepFitProfile : Profile
    {
        public KeepFitProfile()
        {
            //Products - input models
            this.CreateMap<CreateProteinProductInputModel, Protein>();
            this.CreateMap<CreateCreatineProductInputModel, Creatine>();
            this.CreateMap<CreateVitaminProductInputModel, Vitamin>();
            this.CreateMap<CreateAminoAcidProducInputModel, AminoAcid>();
            this.CreateMap<Protein, EditProteinProductInputModel>();
            this.CreateMap<EditProteinProductInputModel, Protein>();
            this.CreateMap<AminoAcid, EditAminoProductInputModel>();
            this.CreateMap<EditAminoProductInputModel, AminoAcid>();
            this.CreateMap<Creatine, EditCreatineProductInputModel>();
            this.CreateMap<EditCreatineProductInputModel, Creatine>();
            this.CreateMap<Vitamin, EditVitaminProductInputModel>();
            this.CreateMap<EditVitaminProductInputModel, Vitamin>();

            //Products - view models
            this.CreateMap<Product, ProductViewModel>();
            this.CreateMap<Product, IndexProductViewModel>();
            this.CreateMap<Protein, DetailsProteinViewModel>();
            this.CreateMap<Creatine, DetailsCreatineViewModel>();
            this.CreateMap<AminoAcid, DetailsAminoViewModel>();
            this.CreateMap<Vitamin, DetailsVitaminViewModel>();
            this.CreateMap<Product, ProductInBasketViewModel>();
            this.CreateMap<ProductViewModel, ProductInBasketViewModel>();
            this.CreateMap<Protein, ProductViewModel>();
            this.CreateMap<AminoAcid, ProductViewModel>();
            this.CreateMap<Vitamin, ProductViewModel>();
            this.CreateMap<Creatine, ProductViewModel>();

            //Basket 
            this.CreateMap<BasketItem, BasketViewModel>();
            this.CreateMap<BasketItem, EditBasketItemViewModel>()
                   .ForMember(dest => dest.ProductPrice,
                                    opt => opt.MapFrom(src => src.Product.Price));

            //Reviews
            this.CreateMap<CreateReviewInputModel, Review>();
            this.CreateMap<Review, ReviewViewModel>()
                   .ForMember(dest => dest.UserFullName,
                                    opt => opt.MapFrom(src => src.KeepFitUser.FullName));

            //Address
            this.CreateMap<Address, GetAddressViewModel>();
            this.CreateMap<Address, CreateAddressViewModel>();
            this.CreateMap<CreateAddressInputModel, Address>();
            this.CreateMap<CreateAddressInputModel, City>()
                .ForMember(dest => dest.Name,
                                opt => opt.MapFrom(src => src.CityName));

            //Orders
            this.CreateMap<CreateOrderInputModel, Order>()
                .ForMember(x => x.OrderDate, opt => opt.Ignore())
                .ForMember(x => x.Status, opt => opt.Ignore());
            this.CreateMap<KeepFitUser, CreateOrderUserInputModel>();
            this.CreateMap<BasketItem, CreateOrderProductInputModel>()
                .ForMember(dest => dest.ProductType,
                                opt => opt.MapFrom(src => src.Product.ProductType));
            this.CreateMap<BasketItem, ProductOrder>()
                .ForMember(dest => dest.Product,
                                opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.ProductQuantity,
                                opt => opt.MapFrom(src => src.Quantity));
            this.CreateMap<Order, IndexOrdersViewModel>()
                .ForMember(dest => dest.ProductsCount,
                                opt => opt.MapFrom(src => src.Products.Sum(x => x.ProductQuantity)));

            //Orders - Details
            this.CreateMap<Order, DetailsOrdersViewModel>();
            this.CreateMap<KeepFitUser, DetailsOrderUserViewModel>();
            this.CreateMap<Address, DetailsOrdersAddressViewModel>();
            this.CreateMap<ProductOrder, DetailsOrdersProductsViewModel>();

            //Orders - Admin 
            this.CreateMap<Order, AllOrdersViewModel>();

            //User 
            this.CreateMap<KeepFitUser, UpdateUserViewModel>();
        }
    }
}