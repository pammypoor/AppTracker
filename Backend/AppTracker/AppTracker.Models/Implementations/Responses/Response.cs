using AppTracker.Models.Contracts;

namespace AppTracker.Models.Implementations.Responses
{
    public class Response<T>: IResponse<T>, IEquatable<Response<T>>
    {
        public string ErrorMessage { get; set; }
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccess { get; set; }

        public Response(string ErrorMessage, T? Data, int StatusCode, bool IsSuccess)
        {
            this.ErrorMessage = ErrorMessage;
            this.Data = Data;
            this.StatusCode = StatusCode;
            this.IsSuccess = IsSuccess;
        }

        public bool Equals(Response<T>? obj)
        {
            if (obj != null)
            {
                return StatusCode.Equals(obj.StatusCode);
            }
            return false;
        }
    }
}
