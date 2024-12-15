namespace NZWalks.API.Utility
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; } = false;
        public object Data { get; set; } = null;
        public string[] Errors { get; set; } = null;

        public void SetResponse(bool isSuccess, object data, string[] errors)
        {
            this.IsSuccess = isSuccess;
            this.Data = data;
            this.Errors = errors;
        }
    }
}
