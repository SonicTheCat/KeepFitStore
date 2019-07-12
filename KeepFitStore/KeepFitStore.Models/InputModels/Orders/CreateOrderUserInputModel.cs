using System;
using System.Collections.Generic;
using System.Text;

namespace KeepFitStore.Models.InputModels.Orders
{
    public class CreateOrderUserInputModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}
