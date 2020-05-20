using System.Collections.Generic;

namespace Transversal.Web.API.Models.Response
{
    public class PaginatedResponseModel<TResponseModel> : ListResponseModel<TResponseModel>,
        IPaginatedResponseModel<TResponseModel>
        where TResponseModel : IResponseModel
    {
        public virtual int PageIndex { get; set; }

        public virtual int PageSize { get; set; }

        public virtual long Total { get; set; }

        public PaginatedResponseModel(IList<TResponseModel> list)
            : base(list)
        {
        }
    }
}
