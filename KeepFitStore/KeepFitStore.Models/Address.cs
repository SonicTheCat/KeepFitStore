﻿namespace KeepFitStore.Models
{
    using System.Collections.Generic;

    public class Address
    {
        public int Id { get; set; }

        public int? StreetNumber { get; set; }

        public string StreetName { get; set; }

        public int? BuildingNumebr { get; set; }

        public string RegionName { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public ICollection<KeepFitUser> KeepFitUsers { get; set; }
    }
}