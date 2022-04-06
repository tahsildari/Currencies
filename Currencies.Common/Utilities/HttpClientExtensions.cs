using Currencies.Models;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace Currencies.Common.Utilities
{
    public static class HttpClientExtensions
    {
        public static async Task<ApiResult<T>> CallApiAsync<T>(this IRestClient client, IRestRequest request)
        {
            try
            {
                var response = await client.ExecuteAsync<T>(request);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return new ApiResult<T>
                        {
                            IsSuccess = true,
                            ErrorCode = null,
                            Result = response.Data
                        };

                    case System.Net.HttpStatusCode.BadRequest:
                    case System.Net.HttpStatusCode.InternalServerError:
                        return new ApiResult<T>
                        {
                            IsSuccess = false,
                            ErrorCode = "500",
                            Result = default
                        };
                }
            }
            catch { }

            return new ApiResult<T>
            {
                IsSuccess = false,
                ErrorCode = "400",
                Result = default
            };
        }

        public static Uri ToUri(this string url)
        {
            try
            {
                return new Uri(url);
            }
            catch (Exception)
            {
            }

            url = url.Replace("\\", "//");
            return new Uri(url);
        }
    }
}