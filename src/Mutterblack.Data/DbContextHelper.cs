using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Mutterblack.Data
{
    public class DbContextHelper : IDbContextHelper
    {
        private readonly DbContextOptions<MutterblackContext> _options;
        private readonly IServiceScopeFactory _scopeFactory;

        public DbContextHelper(DbContextOptions<MutterblackContext> options, IServiceScopeFactory scopeFactory)
        {
            _options = options;
            _scopeFactory = scopeFactory;
        }

        public MutterblackContext Create()
        {
            return new MutterblackContext(_options);
        }

        public DbContextFactory GetFactory()
        {
            return new DbContextFactory(_scopeFactory);
        }

        public class DbContextFactory : IDisposable
        {
            private readonly IServiceScope _scope;
            private readonly MutterblackContext _dbContext;

            public DbContextFactory(IServiceScopeFactory scopeFactory)
            {
                _scope = scopeFactory.CreateScope();
                _dbContext = _scope.ServiceProvider.GetRequiredService<MutterblackContext>();
            }

            public MutterblackContext GetDbContext()
            {
                return _dbContext;
            }

            public void Dispose()
            {
                _scope.Dispose();
            }
        }
    }
}
