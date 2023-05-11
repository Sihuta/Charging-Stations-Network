using ChargingStationsApp.Enums;
using System;

namespace ChargingStationsApp.Models
{
    class SearchOptions
    {
        public SortBy SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string StationName { get; set; }
        public bool Apply { get; set; }
    }
}
