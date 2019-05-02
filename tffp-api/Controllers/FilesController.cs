using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using tffp_api.Model;
using tffp_domain;
using tffp_services;

namespace tffp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly ICountryService countryService;

        public FilesController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return this.Ok();
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(TransactionLineResponse), 200)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] FormModel formModel)
        {
            var response = new TransactionLineResponse();
            var formFile = formModel.File;
            if (formFile != null)
            {
                try
                {
                    var filePath = Path.GetTempFileName();
                    if (formFile.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                            response.Lines = ReadFromStream(stream);
                            var query = from line in response.Lines
                                        select new CountryItem
                                        {
                                            RowIndex = line.RowIndex,
                                            CountryName = line.ImporterCountry
                                        };

                            var r = await this.countryService.Validate(query);
                        }
                    }
                }
                catch (Exception e)
                {
                    response.AddError(e.Message);
                }
            }
            else
            {
                response.AddError("File is required");
            }

            return Ok(response);
        }

        private ICollection<TransactionUnderline> ReadFromStream(Stream stream)
        {
            ICollection<TransactionUnderline> lines = new List<TransactionUnderline>();
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                StringBuilder sb = new StringBuilder();
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                int ColCount = worksheet.Dimension.Columns;

                for (int row = 2; row <= rowCount; row++)
                {
                    var transactionLine = new TransactionUnderline
                    {
                        RowIndex = row,
                        Amount = Decimal.Parse(worksheet.Cells[row, TransactionUnderlineIndex.AMOUNT].Value?.ToString()),
                        Exporter = worksheet.Cells[row, TransactionUnderlineIndex.EXPORTER].Value?.ToString(),
                        ExporterAddress = worksheet.Cells[row, TransactionUnderlineIndex.EXPORTERADDRESS].Value?.ToString(),
                        ExporterCountry = worksheet.Cells[row, TransactionUnderlineIndex.EXPORTERCOUNTRY].Value?.ToString(),
                        Importer = worksheet.Cells[row, TransactionUnderlineIndex.IMPORTER].Value?.ToString(),
                        ImporterAddress = worksheet.Cells[row, TransactionUnderlineIndex.IMPORTERADDRESS].Value?.ToString(),
                        ImporterCountry = worksheet.Cells[row, TransactionUnderlineIndex.IMPORTERCOUNTRY].Value?.ToString(),
                        CountryOfShipment = worksheet.Cells[row, TransactionUnderlineIndex.COUNTRYOFSHIPMENT].Value?.ToString(),
                        CountryOfShipmentRegion = worksheet.Cells[row, TransactionUnderlineIndex.COUNTRYOFSHIPMENTREGION].Value?.ToString(),
                        CountryOfImport = worksheet.Cells[row, TransactionUnderlineIndex.COUNTRYOFIMPORT].Value?.ToString(),
                        DestinationOfGoodsRegion = worksheet.Cells[row, TransactionUnderlineIndex.DESTINATIONOFGOODSREGION].Value?.ToString(),
                        Goods = worksheet.Cells[row, TransactionUnderlineIndex.GOODS].Value?.ToString(),
                        IntraRegionalTrade = worksheet.Cells[row, TransactionUnderlineIndex.INTRAREGIONALTRADE].Value?.ToString(),
                        Sector = worksheet.Cells[row, TransactionUnderlineIndex.SECTOR].Value?.ToString(),
                        ShipmentDate = DateTime.Parse(worksheet.Cells[row, TransactionUnderlineIndex.SHIPMENTDATE].Value?.ToString()),
                        FinancingDate = DateTime.Parse(worksheet.Cells[row, TransactionUnderlineIndex.FINANCINGDATE].Value?.ToString()),
                        FinancingMaturityDate = DateTime.Parse(worksheet.Cells[row, TransactionUnderlineIndex.FINANCINGMATURITYDATE].Value?.ToString())
                    };

                    lines.Add(transactionLine);
                }
            }

            return lines;
        }
    }
}
