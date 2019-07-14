namespace KeepFitStore.Models.ViewModels.Address
{
    public class CreateAddressViewModel
    {
        public int Id { get; set; }

        public int StreetNumber { get; set; }

        public string StreetName { get; set; }

        public int CityId { get; set; }

        public string CityName { get; set; }

        public string CityPostCode { get; set; }
    }
}