namespace KeepFitStore.Services.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Moq;

    using Xunit;

    using KeepFitStore.Data;
    using KeepFitStore.Domain;
    using KeepFitStore.Domain.Enums;
    using KeepFitStore.Domain.Products;
    using KeepFitStore.Models.InputModels.Orders;
    using KeepFitStore.Models.ViewModels.Orders;
    using KeepFitStore.Services.Contracts;
    using KeepFitStore.Services.CustomExceptions;
    using KeepFitStore.Services.Tests.Common;

    public class OrdersServiceTests
    {
        private const int LoopIterations = 5;
        private const string UserOneId = "123";
        private const string UserOneName = "admin@abv.bg";
        private const string UserTwoName = "faketest@abv.bg";
        private const string FullName = "Ivan ivanov";
        private const string Phone = "0821821821";

        private const int ProductId = 100;
        private const decimal Price1 = 120;
        private const string Proteinname = "testov";

        private const int BasketId = 1;

        private const int AddressId = 10;
        private const int CityId = 20;
        private const string StreetName = "Tintqva 19";
        private const int StreetNumber = 777;
        private const string Postcode = "Sq1 4wq";
        private const string CityName = "Sofia";
        private const decimal OrderTotalPrice = 363.0m;
        private const int OrderId = 1;

        private KeepFitDbContext context;
        private IMapper mapper;
        private IOrdersService service;

        [Fact]
        public async Task AddBasketContentToOrder_ShouldWorkCorrect()
        {
            this.Initialize();
            this.SeedProducts();
            this.SeedUsers();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();

            var viewModel = await this.service.AddBasketContentToOrderByUserAsync(UserOneName);
            Assert.NotNull(viewModel.DeliveryAddress);
            Assert.NotNull(viewModel.User);
            Assert.NotEmpty(viewModel.Products);
            Assert.Equal(FullName, viewModel.User.FullName);
            Assert.Equal(Phone, viewModel.User.PhoneNumber);
            Assert.Equal(UserOneId, viewModel.User.Id);
        }

        [Fact]
        public async Task AddBasketContentWithNoProductsInBasket_ShouldThrow()
        {
            this.Initialize();
            this.SeedProducts();
            this.SeedUsers();
            this.SeedAddresses();

            await Assert.ThrowsAsync<ServiceException>(() => this.service.AddBasketContentToOrderByUserAsync(UserOneName));
        }

        [Fact]
        public async Task AddBasketContentToOrderWithNoAddress_ShouldWorkCorrect()
        {
            this.Initialize();
            this.SeedProducts();
            this.SeedUsers();
            this.SeedBasketItemsToUser();

            var viewModel = await this.service.AddBasketContentToOrderByUserAsync(UserOneName);
            Assert.NotNull(viewModel.User);
            Assert.NotEmpty(viewModel.Products);
            Assert.Equal(FullName, viewModel.User.FullName);
            Assert.Equal(Phone, viewModel.User.PhoneNumber);
            Assert.Equal(UserOneId, viewModel.User.Id);

            Assert.Null(viewModel.DeliveryAddress);
        }

        [Fact]
        public async Task AddBasketContentWithInvalidUser_ShouldThrow()
        {
            this.Initialize();
            this.SeedProducts();
            this.SeedUsers();
            this.SeedAddresses();

            await Assert.ThrowsAsync<UserNotFoundException>(() => this.service.AddBasketContentToOrderByUserAsync(UserTwoName));
        }

        [Theory]
        [InlineData("Standart", 0)]
        [InlineData("NextDay", 0)]
        [InlineData("Express", 15)]
        public async Task StartCompletingOrder_ShouldWork(string deliveryType, int deliveryPrice)
        {
            this.Initialize();
            this.SeedProducts();
            this.SeedUsers();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();

            var input = new CreateOrderInputModel()
            {
                PaymentType = PaymentType.Cash,
                DeliveryType = deliveryType,
            };

            var orderId = await this
                .service
                .StartCompletingUserOderAsync(UserOneName, input);

            var order = this.context.Orders.First(x => x.Id == orderId);
            Assert.Equal(OrderTotalPrice + deliveryPrice, order.TotalPrice);
            Assert.Equal(PaymentType.Cash, order.PaymentType);
            Assert.Equal(OrderStatus.NotPayed, order.Status);
            Assert.Equal(FullName, order.KeepFitUser.FullName);
            Assert.Equal(Phone, order.KeepFitUser.PhoneNumber);
            Assert.Equal(UserOneId, order.KeepFitUser.Id);
        }

        [Fact]
        public async Task StartCompletingOrderInvalidProducts_ShouldThrow()
        {
            this.Initialize();
            this.SeedProducts();
            this.SeedUsers();
            this.SeedAddresses();

            var input = new CreateOrderInputModel()
            {
                PaymentType = PaymentType.Cash,
                DeliveryType = DeliveryType.Express.ToString(),
            };

            await Assert.ThrowsAsync<ServiceException>(() => this.service.StartCompletingUserOderAsync(UserOneName, input));
        }

        [Fact]
        public async Task StartCompletingOrderInvalidUsername_ShouldThrow()
        {
            this.Initialize();
            this.SeedProducts();
            this.SeedUsers();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();

            var input = new CreateOrderInputModel()
            {
                PaymentType = PaymentType.Cash,
                DeliveryType = DeliveryType.Express.ToString(),
            };

            await Assert.ThrowsAsync<UserNotFoundException>(() => this.service.StartCompletingUserOderAsync(UserTwoName, input));
        }

        [Fact]
        public async Task CompleteOrder_ShouldComplete()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedOrder();

            var order = this.context.Orders.First(x => x.Id == OrderId);
            Assert.False(order.IsCompleted);
            Assert.Equal(OrderStatus.NotPayed, order.Status);

            await this.service.CompleteOrderAsync(UserOneName, OrderId);

            order = this.context.Orders.First(x => x.Id == OrderId);
            Assert.True(order.IsCompleted);
            Assert.Equal(OrderStatus.Assembling, order.Status);
        }

        [Fact]
        public async Task CopleteOrderWithInvalidOrder_ShouldThrow()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedOrder();

            await Assert.ThrowsAsync<OrderNotFoundException>(() => this.service.CompleteOrderAsync(UserOneName, OrderId + 1));
        }

        [Fact]
        public async Task CompleteOrderWithInvalidUser_ShouldThrow()
        {
            this.Initialize();
            this.SeedProducts();
            this.SeedUsers();
            this.SeedOrder();

            await Assert.ThrowsAsync<UserNotFoundException>(() => this.service.CompleteOrderAsync(UserTwoName, OrderId));
        }

        [Fact]
        public async Task GetOrderById_ShouldWorkCorrect()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProducts();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();
            this.SeedOrders();

            var model = await this.service.GetOrderByIdAsync<CompleteOrderViewModel>(OrderId);

            Assert.Equal(OrderId, model.Id);
            Assert.Equal(UserOneId, model.KeepFitUserId);
            Assert.Equal(FullName, model.KeepFitUserFullName);
            Assert.NotEmpty(model.Products);
        }

        [Fact]
        public async Task GetOrderById_ShouldThrow()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProducts();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();
            this.SeedOrders();

            await Assert.ThrowsAsync<OrderNotFoundException>(() => this.service.GetOrderByIdAsync<CompleteOrderViewModel>(OrderId - 1));
        }

        [Fact]
        public async Task GetAllCompletedOrders_ShouldReturnFullCollection()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProducts();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();
            this.SeedOrders();

            var collection = await this.service.GetAllOrdersAsync<AllOrdersViewModel>();

            Assert.Equal(LoopIterations, collection.Count());
        }

        [Fact]
        public async Task GetAllCompletedOrders_ShouldReturnEmpty()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProducts();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();

            var collection = await this.service.GetAllOrdersAsync<AllOrdersViewModel>();

            Assert.Empty(collection);
        }

        [Fact]
        public async Task GetAllByUser_ShouldReturnFullCollection()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProducts();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();
            this.SeedOrders();

            var collection = await this.service.GetAllOrdersForUserAsync<IndexOrdersViewModel>(UserOneName);

            var expected = 1;
            Assert.Equal(expected, collection.Count());
            Assert.Collection(collection, item => Assert.Equal(OrderId, item.Id));
            Assert.Collection(collection, item => Assert.Equal(0, item.ProductsCount));
        }

        [Fact]
        public async Task GetAllByUser_ShoulThrow()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProducts();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();
            this.SeedOrders();

            await Assert.ThrowsAsync<UserNotFoundException>(() => this.service.GetAllOrdersForUserAsync<IndexOrdersViewModel>(UserTwoName));
        }

        [Fact]
        public async Task GetAllByUserSorted_ShouldReturnFullCollection()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedAddresses();

            var user = this.context.Users.OrderBy(x => x.Id).First();
            var increment = 0;
            for (int i = 0; i < 3; i++)
            {
                user.Orders.Add(new Order()
                {
                    TotalPrice = OrderTotalPrice * increment++
                });
            }
            this.context.SaveChanges();

            var collection = await this.service.GetAllOrdersForUserSortedAsync<IndexOrdersViewModel>(UserOneName, "TotalPrice");

            var expected = 3;
            Assert.Equal(expected, collection.Count());
            var first = collection.First();
            var lst = collection.Last();

            Assert.Equal(0, first.TotalPrice);
            Assert.Equal(OrderTotalPrice * 2, lst.TotalPrice);
        }

        [Fact]
        public async Task GetAllByUserSortedDescending_ShouldReturnSorted()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedAddresses();

            var user = this.context.Users.OrderBy(x => x.Id).First();
            var increment = 0;
            for (int i = 0; i < 3; i++)
            {
                user.Orders.Add(new Order()
                {
                    TotalPrice = OrderTotalPrice * increment++
                });
            }
            this.context.SaveChanges();

            var collection = await this.service.GetAllOrdersForUserSortedAsync<IndexOrdersViewModel>(UserOneName, "TotalPrice Desc");

            var expected = 3;
            Assert.Equal(expected, collection.Count());
            var first = collection.First();
            var last = collection.Last();

            Assert.Equal(0, last.TotalPrice);
            Assert.Equal(OrderTotalPrice * 2, first.TotalPrice);
        }

        [Fact]
        public async Task GetUserOrderByOrderIdAndUsername_ShoulWork()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProducts();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();
            this.SeedOrders();

            var numberToAdd = 1;

            var viewModel = await this.service.GetOrderDetailsForUserAsync<DetailsOrdersViewModel>(UserOneName + numberToAdd, OrderId + numberToAdd);

            Assert.NotNull(viewModel.DeliveryAddress);
            Assert.Equal(AddressId + numberToAdd, viewModel.DeliveryAddress.Id);

            Assert.NotNull(viewModel.KeepFitUser);
            Assert.Equal(FullName + numberToAdd, viewModel.KeepFitUser.FullName);

            Assert.NotNull(viewModel.Products);
            Assert.Single(viewModel.Products);
            Assert.Collection(viewModel.Products, item => Assert.Equal(ProductId + numberToAdd, item.ProductId));
        }

        [Fact]
        public async Task GetUserOrderByOrderIdAndUsername_ShoulThrowWithInvalidOrderId()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProducts();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();
            this.SeedOrders();

            await Assert.ThrowsAsync<OrderNotFoundException>(() => this.service.GetOrderDetailsForUserAsync<DetailsOrdersViewModel>(UserOneName, OrderId + 5));
        }

        [Fact]
        public async Task GetUserOrderByOrderIdAndUsername_ShoulThrowWithMissmatchedUsernames()
        {
            this.Initialize();
            this.SeedUsers();
            this.SeedProducts();
            this.SeedAddresses();
            this.SeedBasketItemsToUser();
            this.SeedOrders();

            await Assert.ThrowsAsync<UserNotAuthorizedException>(() => this.service.GetOrderDetailsForUserAsync<DetailsOrdersViewModel>(UserOneName + 1, OrderId));
        }

        private void SeedOrder()
        {
            this.context.Orders.Add(new Order()
            {
                Id = OrderId,
                IsCompleted = false,
                Status = OrderStatus.NotPayed
            });

            this.context.SaveChanges();
        }

        private void SeedOrders()
        {
            var list = new List<Order>();
            var users = this.context.Users.OrderBy(x => x.Id).ToList();

            for (int i = 0; i < users.Count; i++)
            {
                users[i].Orders.Add(new Order()
                {
                    Id = i == 0 ? OrderId : OrderId + i,
                    IsCompleted = true,
                    Status = OrderStatus.NotPayed,
                    DeliveryAddress = users[i].Address,
                    TotalPrice = OrderTotalPrice * i
                });

                foreach (var order in users[i].Orders)
                {
                    order.Products.Add(new ProductOrder
                    {
                        ProductId = i == 0 ? ProductId : ProductId + i,
                        ProductQuantity = i
                    });
                }
            }

            this.context.SaveChanges();
        }

        private void SeedBasketItemsToUser()
        {
            var list = new List<BasketItem>();
            var users = this.context.Users.ToList();
            var countOfBasketItems = 3;

            for (int i = 0; i < users.Count; i++)
            {
                for (int j = 0; j < countOfBasketItems; j++)
                {
                    var basketItem = new BasketItem()
                    {
                        BasketId = users[i].BasketId,
                        ProductId = ProductId + j
                    };

                    list.Add(basketItem);
                }
            }

            this.context.BasketItems.AddRange(list);
            this.context.SaveChanges();
        }

        private void SeedProducts()
        {
            var list = new List<Product>();

            for (int i = 0; i < LoopIterations; i++)
            {
                Product product = new Protein()
                {
                    Id = i == 0 ? ProductId : ProductId + i,
                    Name = i == 0 ? Proteinname : Proteinname + i,
                    Price = i == 0 ? Price1 : Price1 + i,
                    Type = ProteinType.Bar,
                    ProductType = ProductType.Protein
                };

                list.Add(product);
            }

            this.context.AddRange(list);
            this.context.SaveChanges();
        }

        private void SeedAddresses()
        {
            var users = this.context.Users.ToList();

            for (int i = 0; i < users.Count; i++)
            {
                users[i].Address = new Address()
                {
                    Id = AddressId + i,
                    StreetName = StreetName,
                    StreetNumber = StreetNumber,
                    City = new City()
                    {
                        Id = CityId,
                        Name = CityName,
                        PostCode = Postcode
                    }
                };
            }

            this.context.SaveChanges();
        }

        private void SeedUsers()
        {
            for (int i = 0; i < LoopIterations; i++)
            {
                var user = new KeepFitUser()
                {
                    Id = i == 0 ? UserOneId : UserOneId + i,
                    UserName = i == 0 ? UserOneName : UserOneName + i,
                    PhoneNumber = i == 0 ? Phone : Phone + i,
                    FullName = i == 0 ? FullName : FullName + i,
                    Basket = new Basket()
                    {
                        Id = i == 0 ? BasketId : BasketId + i
                    }
                };

                this.context.Users.Add(user);
            }

            this.context.SaveChanges();
        }

        private void Initialize()
        {
            this.context = KeepFitDbContextInMemoryFactory.Initialize();
            this.mapper = AutoMapperFactory.Initialize();
            var basketService = new Mock<IBasketService>();
            basketService.Setup(x => x.GetBasketTotalPriceAsync(UserOneName))
                         .Returns(Task.FromResult(OrderTotalPrice));

            this.service = new OrdersService(
            context,
            basketService.Object,
            mapper);
        }
    }
}