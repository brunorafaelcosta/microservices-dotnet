using Microsoft.EntityFrameworkCore;
using Transversal.Common.InversionOfControl;
using Transversal.Common.InversionOfControl.Registrar;
using Transversal.Data.EFCore;
using Transversal.Data.EFCore.DbContext;
using Transversal.Data.EFCore.DbSeeder;

namespace Services.Resources.API.Core.Data
{
    [IoCRegistrarDependency(typeof(TransversalDataEFCoreIoCRegistrar))]
    public class DataIoCRegistrar : IIoCRegistrar
    {
        public void Run(IIoCManager ioCManager)
        {
            ioCManager.RegisterGeneric(typeof(IDbContextOptionsResolver<>), typeof(DbContextOptionsResolver<>));

            ioCManager.Register<DbContextOptions<DefaultDbContext>>((ioCResolver) =>
            {
                var defaultDbContextOptionsResolver = ioCResolver.Resolve<IDbContextOptionsResolver<DefaultDbContext>>();
                return defaultDbContextOptionsResolver.GetDbContextOptions();
            });
            ioCManager.Register<DefaultDbContext>();
            ioCManager.Register<IEfCoreDbSeeder<DefaultDbContext>, DefaultDbSeeder>();
            
            #region Repositories
            ioCManager.Register<Domain.IResourceGroupRepository, Repositories.ResourceGroupRepository>();
            ioCManager.Register<Domain.IResourceRepository, Repositories.ResourceRepository>();
            #endregion Repositories
        }
    }
}
