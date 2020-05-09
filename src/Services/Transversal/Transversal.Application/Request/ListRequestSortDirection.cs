namespace Transversal.Application.Request
{
    /// <summary>
    /// Specifies the direction of a property sort operation in a <see cref="IListRequest{TRequestDto, TResponseDto}"/>.
    /// </summary>
    public static class ListRequestSortDirection
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
