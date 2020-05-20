using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Transversal.Web.API.Models.Response
{
    public class ListResponseModel<TResponseModel> : ReadOnlyCollection<TResponseModel>,
        IListResponseModel<TResponseModel>
        where TResponseModel : IResponseModel
    {
        public ListResponseModel(IList<TResponseModel> list)
            : base(list)
        {
        }
    }
}
