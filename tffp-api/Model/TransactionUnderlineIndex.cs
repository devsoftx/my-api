using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tffp_api.Model
{
    public class TransactionUnderlineIndex
    {
        public static int AMOUNT = 1;
        public static int EXPORTER = 2;
        public static int EXPORTERADDRESS = 3;
        public static int EXPORTERCOUNTRY = 4;
        public static int IMPORTER = 5;
        public static int IMPORTERADDRESS = 6;
        public static int IMPORTERCOUNTRY = 7;
        public static int COUNTRYOFSHIPMENT = 8;
        public static int COUNTRYOFSHIPMENTREGION = 9;
        public static int COUNTRYOFIMPORT = 10;
        public static int DESTINATIONOFGOODSREGION = 11;
        public static int GOODS = 12;
        public static int INTRAREGIONALTRADE = 13;
        public static int SECTOR = 14;
        public static int SHIPMENTDATE = 15;
        public static int FINANCINGDATE = 16;
        public static int FINANCINGMATURITYDATE = 17;
    }
}
