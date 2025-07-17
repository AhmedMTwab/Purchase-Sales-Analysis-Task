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
        public Task<bool> AddSale(SaleAddDTO newSale)
        {
            Sale sale = new Sale() { productName = newSale.productName,quantity = newSale.quantity, price = newSale.price };

            var added = _saleRepo.AddSale(sale);
            return added;
        }
    }
}
