using System;

namespace Transversal.Common.Projection
{
	/// <summary>
	/// This will replace the extension method tagged with an expression defined by the linked Method.
	/// <para>Usage: [ReplaceWithExpression(PropertyName = nameof(YourExpression))]</para>
	/// </summary>
	public class ReplaceWithExpressionAttribute : Attribute
	{
		/// <summary>
		/// The name of the property returning an expression which you want to replace this extension with
		/// </summary>
		public string PropertyName { get; set; }

		public ReplaceWithExpressionAttribute(string propertyName)
		{
			PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
		}
	}
}
