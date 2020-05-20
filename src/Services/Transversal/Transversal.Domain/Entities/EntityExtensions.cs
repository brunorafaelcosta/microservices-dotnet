using System;
using System.Reflection;
using Transversal.Common.Extensions;
using Transversal.Domain.Entities.Auditing;
using Transversal.Domain.Entities.Tenancy;

namespace Transversal.Domain.Entities
{
    /// <summary>
    /// Extension methods for <see cref="IEntity{TPrimaryKey}"/> objects.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Checks if this entity is null or soft deleted.
        /// </summary>
        public static bool IsNullOrDeleted(this ISoftDelete entity)
        {
            return entity == null || entity.IsDeleted;
        }

        /// <summary>
        /// Undeletes this entity by reseting <see cref="ISoftDelete"/> and <see cref="IDeletionAudited"/> properties.
        /// </summary>
        public static void UnDelete(this ISoftDelete entity)
        {
            entity.IsDeleted = false;

            if (entity is IDeletionAudited)
            {
                var deletionAuditedEntity = entity.As<IDeletionAudited>();
                deletionAuditedEntity.DeletionTime = null;
                deletionAuditedEntity.DeleterUserId = null;
            }
        }

        public static bool EntityIsMayHaveTenant<TEntity, TPrimaryKey>(this TEntity entity)
            where TEntity : IEntity<TPrimaryKey>
        {
            return EntityIsMayHaveTenant(typeof(TEntity), typeof(TPrimaryKey));
        }
        public static bool EntityIsMayHaveTenant(Type entityType, Type primaryKeyType)
        {
            bool isMayHaveTenantType = false;

            if (entityType != null && entityType.GetInterface("IMayHaveTenant`2") != null)
            {
                Type mayHaveTenantType = typeof(IMayHaveTenant<,>).MakeGenericType(entityType, primaryKeyType);
                isMayHaveTenantType = mayHaveTenantType.IsAssignableFrom(entityType);
            }

            return isMayHaveTenantType;
        }

        public static bool TenantIdsAreEqual<TEntityA, TPrimaryKeyA, TEntityB, TPrimaryKeyB>(TEntityA entityA, TEntityB entityB)
            where TEntityA : IEntity<TPrimaryKeyA>
            where TEntityB : IEntity<TPrimaryKeyB>
        {
            bool areEqual = false;

            if (entityA.EntityIsMayHaveTenant<TEntityA, TPrimaryKeyA>()
                && entityB.EntityIsMayHaveTenant<TEntityB, TPrimaryKeyB>())
            {
                areEqual = (bool)typeof(EntityExtensions)
                    .GetMethod(nameof(MayHaveTenantIdsAreEqual), BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(typeof(TEntityA), typeof(TPrimaryKeyA), typeof(TEntityB), typeof(TPrimaryKeyB))
                    .Invoke(null, new object[] { entityA, entityB });
            } 
            
            if (entityA is IMustHaveTenant && entityB is IMustHaveTenant)
            {
                areEqual = areEqual || MustHaveTenantIdsAreEqual((IMustHaveTenant)entityA, (IMustHaveTenant)entityB);
            }

            return areEqual;
        }
        public static bool MayHaveTenantIdsAreEqual<TEntityA, TPrimaryKeyA, TEntityB, TPrimaryKeyB>(TEntityA entityA, TEntityB entityB)
            where TEntityA : class, IEntity<TPrimaryKeyA>, IMayHaveTenant<TEntityA, TPrimaryKeyA>
            where TEntityB : class, IEntity<TPrimaryKeyB>, IMayHaveTenant<TEntityB, TPrimaryKeyB>
        {
            bool areEqual = false;

            if (entityA.TenantId == entityB.TenantId)
            {
                areEqual = true;
            }

            return areEqual;
        }
        public static bool MustHaveTenantIdsAreEqual<TEntityA, TEntityB>(TEntityA entityA, TEntityB entityB)
            where TEntityA : IMustHaveTenant
            where TEntityB : IMustHaveTenant
        {
            bool areEqual = false;

            if (entityA.TenantId == entityB.TenantId)
            {
                areEqual = true;
            }

            return areEqual;
        }
    }
}
