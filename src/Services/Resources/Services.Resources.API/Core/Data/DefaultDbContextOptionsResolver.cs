﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Reflection;

namespace Services.Resources.API.Core.Data
{
    public class DefaultDbContextOptionsResolver : Transversal.Data.EFCore.DbContext.IDbContextOptionsResolver<DefaultDbContext>
    {
        public DbContextOptions<DefaultDbContext> GetDbContextOptions(string connectionString)
        {
            return GetDbContextOptions(connectionString, null);
        }

        public DbContextOptions<DefaultDbContext> GetDbContextOptions(string connectionString, DbConnection existingConnection)
        {
            var builder = new DbContextOptionsBuilder<DefaultDbContext>();

            builder.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
            });

            return builder.Options;
        }
    }
}