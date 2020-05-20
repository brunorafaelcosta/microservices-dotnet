namespace Transversal.Web.API.Models.Response
{
    /// <summary>
    /// Paginated response model used by API controller actions.
    /// </summary>
    /// <typeparam name="TResponseModel">Response model type used to return the mapped Data Transfer Objects</typeparam>
    public interface IPaginatedResponseModel<TResponseModel> : IListResponseModel<TResponseModel>
        where TResponseModel : IResponseModel
    {
        /// <summary>
        /// Current page
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// Used page size to limit the fetched objects
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Total number of object
        /// </summary>
        long Total { get; }
    }
}
