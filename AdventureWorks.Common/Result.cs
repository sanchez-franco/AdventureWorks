using System.Net;

namespace AdventureWorks.Common
{
    public class Result<T>
    {
        public T Data { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
