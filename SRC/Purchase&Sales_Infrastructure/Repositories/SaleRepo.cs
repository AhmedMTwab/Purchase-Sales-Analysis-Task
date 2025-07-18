using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Purchase_Sales_Domain.Models;
using Purchase_Sales_Domain.RepositoryAbstractions;
using Purchase_Sales_Infrastructure.Context;

namespace Purchase_Sales_Infrastructure.Repositories
{
    public class SaleRepo(ApplicationDbContext db):ISaleRepo
    {
        public async Task<bool> AddPulkOfSale(List<Sale> newSales)
        {
            await db.BulkInsertAsync(newSales);
            return true;
        }

        public async Task<bool> AddSale(Sale newSale)
        {
            await db.sales.AddAsync(newSale);
            await db.SaveChangesAsync();
            return true;
        }

    }
}
