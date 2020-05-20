using System.Collections.Generic;

namespace Transversal.Web.API.Models.Response
{
    /// <summary>
    /// List response model used by API controller actions.
    /// </summary>
    /// <typeparam name="TResponseModel">Response model type used to return the mapped Data Transfer Objects</typeparam>
    public interface IListResponseModel<TResponseModel> : IReadOnlyList<TResponseModel>
        where TResponseModel : IResponseModel
    {
    }
}
