using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MResultPattern
{

    /// <summary>
    /// Veri içeren API yanıtları için Result sınıfı
    /// </summary>
    public sealed class Result<T>
    {
        [JsonPropertyName("data")]
        public T? Data { get; private set; }

        [JsonPropertyName("Errors")]
        public List<string>? Errors { get; private set; }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; private set; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;

        private Result()
        {
        }

        /// <summary>
        /// Başarılı sonuç oluşturur
        /// </summary>
        public static Result<T> Success(T data, int statusCode = (int)HttpStatusCode.OK)
        {
            return new Result<T>
            {
                Data = data,
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Hata listesiyle başarısız sonuç oluşturur
        /// </summary>
        public static Result<T> Failure(int statusCode, List<string> errorMessages)
        {
            return new Result<T>
            {
                StatusCode = statusCode,
                Errors = errorMessages
            };
        }

        /// <summary>
        /// Tek hata mesajıyla başarısız sonuç oluşturur
        /// </summary>
        public static Result<T> Failure(int statusCode, string errorMessage)
        {
            return new Result<T>
            {
                StatusCode = statusCode,
                Errors = new List<string> { errorMessage }
            };
        }

        /// <summary>
        /// 500 hata koduyla başarısız sonuç oluşturur
        /// </summary>
        public static Result<T> Failure(string errorMessage)
        {
            return Failure((int)HttpStatusCode.InternalServerError, errorMessage);
        }

        /// <summary>
        /// JSON formatına dönüştürür
        /// </summary>
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }
    }

    /// <summary>
    /// Veri içermeyen API yanıtları için Result sınıfı
    /// </summary>
    public class Result
    {
        [JsonPropertyName("Errors")]
        public List<string>? Errors { get; private set; }

        [JsonPropertyName("statusCode")]
        public int StatusCode { get; private set; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;

        private Result() { }

        /// <summary>
        /// Başarılı sonuç oluşturur
        /// </summary>
        public static Result Success(int statusCode = (int)HttpStatusCode.OK)
        {
            return new Result
            {
                StatusCode = statusCode
            };
        }

        /// <summary>
        /// Hata listesiyle başarısız sonuç oluşturur
        /// </summary>
        public static Result Failure(int statusCode, List<string> errorMessages)
        {
            return new Result
            {
                StatusCode = statusCode,
                Errors = errorMessages
            };
        }

        /// <summary>
        /// Tek hata mesajıyla başarısız sonuç oluşturur
        /// </summary>
        public static Result Failure(int statusCode, string errorMessage)
        {
            return new Result
            {
                StatusCode = statusCode,
                Errors = [errorMessage]
            };
        }

        /// <summary>
        /// 500 hata koduyla başarısız sonuç oluşturur
        /// </summary>
        public static Result Failure(string errorMessage)
        {
            return Failure((int)HttpStatusCode.InternalServerError, errorMessage);
        }

        /// <summary>
        /// JSON formatına dönüştürür
        /// </summary>
        public override string ToString()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, options);
        }
    }

    /// <summary>
    /// Result sınıfları için ek metotlar
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Başarılı durumda işlem yapar
        /// </summary>
        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess && result.Data != null)
            {
                action(result.Data);
            }
            return result;
        }

        /// <summary>
        /// Başarısız durumda işlem yapar
        /// </summary>
        public static Result<T> OnFailure<T>(this Result<T> result, Action<List<string>> action)
        {
            if (!result.IsSuccess && result.Errors != null)
            {
                action(result.Errors);
            }
            return result;
        }
    }
}
