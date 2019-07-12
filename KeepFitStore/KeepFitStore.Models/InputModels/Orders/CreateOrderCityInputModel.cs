using System;
using System.Collections.Generic;
using System.Text;

namespace KeepFitStore.Models.InputModels.Orders
{
    public class CreateOrderCityInputModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PostCode { get; set; }
    }
}
