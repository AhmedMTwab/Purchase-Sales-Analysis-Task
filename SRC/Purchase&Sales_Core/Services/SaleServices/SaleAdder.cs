using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Purchase_Sales_Core.DTOs.SaleDTO;
using Purchase_Sales_Core.Services.ProductServices;
using Purchase_Sales_Core.ServicesAbstractions.SaleServicesAbstractions;
using Purchase_Sales_Domain.Models;
using Purchase_Sales_Domain.RepositoryAbstractions;

namespace Purchase_Sales_Core.Services.SaleServices
{
    public class SaleAdder(ISaleRepo _saleRepo) : ISaleAdder
    {
        public async Task<bool> AddPulkOfSales(List<SaleAddDTO> newSales)
        {
            List<Sale> salesToAdd = new List<Sale>();
            foreach(var sale in newSales)
            {
                Sale rowSale = new Sale() { productName = sale.productName, quantity = sale.quantity, price = sale.price };
                salesToAdd.Add(rowSale);

            }
           var added=await _saleRepo.AddPulkOfSale(salesToAdd);
            return added;
        }

        public async Task<bool> AddSale(SaleAddDTO newSale)
        {
            Sale sale = new Sale() { productName = newSale.productName,quantity = newSale.quantity, price = newSale.price };

            var added =await _saleRepo.AddSale(sale);
            return added;
        }
    }
}
