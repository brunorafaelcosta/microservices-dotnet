namespace Transversal.Application.Dto.Request
{
    /// <summary>
    /// Specifies the direction of a property sort operation in a <see cref="IListRequestDto{TResponseDto}"/>.
    /// </summary>
    public static class ListRequestDtoSortDirection
    {
        /// <summary>
        ///  Sorts the property in ascending order.
        /// </summary>
        public const string Ascending = "asc";

        /// <summary>
        /// Sorts the property in descending order.
        /// </summary>
        public const string Descending = "desc";
    }
}
