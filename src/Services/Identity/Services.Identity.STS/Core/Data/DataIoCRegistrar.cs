using Microsoft.EntityFrameworkCore;
using Transversal.Common.InversionOfControl;
using Transversal.Common.InversionOfControl.Registrar;
using Transversal.Data.EFCore;
using Transversal.Data.EFCore.DbContext;
using Transversal.Data.EFCore.DbSeeder;

namespace Services.Identity.STS.Core.Data
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
            ioCManager.RegisterIfNot<Domain.Users.IUserPictureRepository, Repositories.UserPictureRepository>();
            ioCManager.RegisterIfNot<Domain.Users.IUserRepository, Repositories.UserRepository>();
            #endregion Repositories
        }
    }
}
