using static Mutterblack.Data.DbContextHelper;

namespace Mutterblack.Data
{
    public interface IDbContextHelper
    {
        MutterblackContext Create();
        DbContextFactory GetFactory();
    }
}