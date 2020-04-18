using System;
using Transversal.Common.Session;
using Transversal.Domain.Uow.Manager;

namespace Transversal.Application
{
    public abstract class ApplicationServiceBase : IApplicationService
    {
        protected ISessionInfo Session { get; private set; }
        protected IUnitOfWorkManager UowManager { get; private set; }

        public ApplicationServiceBase(
            ISessionInfo session,
            IUnitOfWorkManager uowManager
        )
        {
            Session = session ?? throw new ArgumentNullException(nameof(session));
            UowManager = uowManager ?? throw new ArgumentNullException(nameof(uowManager));
        }
    }
}
