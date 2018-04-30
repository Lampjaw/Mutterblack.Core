using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Mutterblack.Data
{
    public class MutterblackContext : DbContext
    {
        public MutterblackContext(DbContextOptions<MutterblackContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ApplyConfigurations(builder);
            ConvertToConvention(builder);
        }

        private static void ApplyConfigurations(ModelBuilder builder)
        {
            var applyGenericMethod = typeof(ModelBuilder).GetMethod("ApplyConfiguration", BindingFlags.Instance | BindingFlags.Public);
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters))
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                        applyConcreteMethod.Invoke(builder, new object[] { Activator.CreateInstance(type) });
                        break;
                    }
                }
            }
        }

        private static void ConvertToConvention(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.Relational().TableName = ToSnakeCase(entity.Relational().TableName);

                // Replace column names
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = ToSnakeCase(property.Name);
                }

                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = ToSnakeCase(key.Relational().Name);
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = ToSnakeCase(key.Relational().Name);
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = ToSnakeCase(index.Relational().Name);
                }
            }
        }

        private static string ToSnakeCase(string input)
        {
            var result = Regex.Replace(input, ".[A-Z]", m => m.Value[0] + "_" + m.Value[1]);

            return result.ToLower();
        }
    }
}
