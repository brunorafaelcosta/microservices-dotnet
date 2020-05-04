using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Transversal.Common.Projection;

namespace Transversal.Data.EFCore.DbContext.LinqExpander
{
	internal class ExpandableVisitor : ExpressionVisitor
	{
		private readonly IQueryProvider _provider;
		private readonly Dictionary<ParameterExpression, Expression> _replacements = new Dictionary<ParameterExpression, Expression>();

		internal ExpandableVisitor(IQueryProvider provider)
		{
			_provider = provider;
		}

		protected override Expression VisitMethodCall(MethodCallExpression node)
		{
			var replaceNodeAttribute = node.Method.GetCustomAttributes(typeof(ReplaceWithExpressionAttribute), false).Cast<ReplaceWithExpressionAttribute>().FirstOrDefault();
			if (replaceNodeAttribute != null && node.Method.IsStatic)
			{
				if (!string.IsNullOrEmpty(replaceNodeAttribute.PropertyName))
				{
					var properties = node.Method.ReturnType.GetRuntimeProperties();
					if (properties.Any(x => x.Name == replaceNodeAttribute.PropertyName))
					{
						var replaceWith = properties.First(x => x.Name == replaceNodeAttribute.PropertyName).GetValue(null);
						if (replaceWith is LambdaExpression)
						{
							RegisterReplacementParameters(node.Arguments.ToArray(), replaceWith as LambdaExpression);
							return Visit((replaceWith as LambdaExpression).Body);
						}
					}
				}
			}
			return base.VisitMethodCall(node);
		}
		protected override Expression VisitParameter(ParameterExpression node)
		{
			Expression replacement;
			if (_replacements.TryGetValue(node, out replacement))
				return Visit(replacement);
			return base.VisitParameter(node);
		}
		private void RegisterReplacementParameters(Expression[] parameterValues, LambdaExpression expressionToVisit)
		{
			if (parameterValues.Length != expressionToVisit.Parameters.Count)
				throw new ArgumentException(string.Format("The parameter values count ({0}) does not match the expression parameter count ({1})", parameterValues.Length, expressionToVisit.Parameters.Count));
			foreach (var x in expressionToVisit.Parameters.Select((p, idx) => new { Index = idx, Parameter = p }))
			{
				if (_replacements.ContainsKey(x.Parameter))
				{
					throw new Exception("Parameter already registered, this shouldn't happen.");
				}
				_replacements.Add(x.Parameter, parameterValues[x.Index]);
			}
		}
	}

}
