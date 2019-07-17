namespace KeepFitStore.Models.InputModels.Orders
{
    using KeepFitStore.Domain.Enums;

    public class CreateOrderProductInputModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int Quantity { get; set; }

        public ProductType ProductType { get; set; }
    }
}