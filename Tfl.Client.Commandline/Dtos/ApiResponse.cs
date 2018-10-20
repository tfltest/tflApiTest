namespace Tfl.Client.Commandline.Dtos
{
    public class ApiResponse<T> where T : class
    {
        public ApiResponse(T response, ApiError apiError)
        {
            Response = response;
            ApiError = apiError;
            IsSuccess = response != null;
        }

        public bool IsSuccess { get; private set; }

        public T Response { get; private set; }

        public ApiError ApiError { get; private set; }
    }
}
