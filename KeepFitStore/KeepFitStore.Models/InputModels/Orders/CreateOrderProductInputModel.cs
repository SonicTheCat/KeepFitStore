namespace KeepFitStore.Models.InputModels.Orders
{
    using KeepFitStore.Domain.Enums;

    public class CreateOrderProductInputModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public ProductType ProductType { get; set; }
    }
}