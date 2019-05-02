using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tffp_api.Model
{
    public class TransactionUnderline
    {
        public int RowIndex { get; set; }
        public decimal? Amount { get; set; }
        public string Exporter { get; set; }
        public string ExporterAddress { get; set; }
        public string ExporterCountry { get; set; }
        public string Importer { get; set; }
        public string ImporterAddress { get; set; }
        public string ImporterCountry { get; set; }
        public string CountryOfShipment { get; set; }
        public string CountryOfShipmentRegion { get; set; }
        public string CountryOfImport { get; set; }
        public string DestinationOfGoodsRegion { get; set; }
        public string Goods { get; set; }
        public string IntraRegionalTrade { get; set; }
        public string Sector { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public DateTime? FinancingDate { get; set; }
        public DateTime? FinancingMaturityDate { get; set; }
    }
}
