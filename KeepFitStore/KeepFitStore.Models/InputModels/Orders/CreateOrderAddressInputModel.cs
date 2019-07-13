using KeepFitStore.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KeepFitStore.Models.InputModels.Orders
{
    public class CreateOrderAddressInputModel
    {
        public int Id { get; set; }

        [Required]
        [Range(ModelsConstants.StreetNumberMinNumber, ModelsConstants.StreetNumberMaxNumber)]
        [Display(Name = "Ulica")]
        public int? StreetNumber { get; set; }

        [Required]
        [Display(Name = "Ime na Ulica")]
        public string StreetName { get; set; }

        public CreateOrderCityInputModel City { get; set; }
    }
}