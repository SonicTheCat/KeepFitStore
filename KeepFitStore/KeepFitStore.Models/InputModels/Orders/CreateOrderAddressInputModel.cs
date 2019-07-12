using System;
using System.Collections.Generic;
using System.Text;

namespace KeepFitStore.Models.InputModels.Orders
{
    public class CreateOrderAddressInputModel
    {
        public int Id { get; set; }

        public int? StreetNumber { get; set; }

        public string StreetName { get; set; }

        public int? BuildingNumebr { get; set; }

        public string RegionName { get; set; }

        public CreateOrderCityInputModel City { get; set; }
    }
}