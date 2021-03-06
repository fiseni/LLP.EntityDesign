using LLP.EntityDesign.API.Contracts;
using LLP.EntityDesign.API.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LLP.EntityDesign.API.Services
{
    public class DocumentNoGenerator : IDocumentNoGenerator
    {
        private readonly AppDbContext dbContext;

        public DocumentNoGenerator(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// It generates new order number by incrementing the last number.
        /// It persists leading zeroes, 6 chars.
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetNewOrderNo()
        {
            var lastNo = await dbContext.Orders
                                    .Select(x => x.OrderNo)
                                    .OrderByDescending(x => x.Length)
                                    .ThenByDescending(x => x)
                                    .FirstOrDefaultAsync();

            return (Convert.ToInt32(lastNo) + 1).ToString("D6");
        }
    }
}
