using System;
using System.Linq.Expressions;

namespace Transversal.Common.Projection
{
    public abstract class Projectable<TEntity, TResult>
        where TResult: class, new()
    {
        public static Expression<Func<TEntity, TResult>> Projection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        [ReplaceWithExpression(nameof(Projection))]
        public static TResult FromEntity(TEntity entity) => Projection.Compile().Invoke(entity);
    }
}
