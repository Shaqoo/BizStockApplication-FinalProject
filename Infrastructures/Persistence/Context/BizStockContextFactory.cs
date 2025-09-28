using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
                             
namespace Infrastructures.Persistence.Context
{
    public class BizStockContextFactory : IDesignTimeDbContextFactory<BizStockContext>
    {
        public BizStockContext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<BizStockContext>();
            optionBuilder.UseNpgsql(" Host=localhost;Database=BizStock;Username=postgres;Password=Sha@o@;", b =>
    b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            return new BizStockContext(optionBuilder.Options);
        }

    }
}
