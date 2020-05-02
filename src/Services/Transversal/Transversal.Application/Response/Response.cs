namespace Transversal.Application.Response
{
    public class Response<T> : IResponse<T>
    {
        public T Result { get; set; }

        public Response()
        {
        }

        public Response(T result)
        {
            Result = result;
        }
    }
}
