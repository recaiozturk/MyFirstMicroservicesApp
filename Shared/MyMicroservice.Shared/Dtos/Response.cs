using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MyMicroservice.Shared.Dtos
{
    public class Response<T>
    {
        public T Data { get; private set; }

        //json ignore ile cliente status code gösttermeye gerek yok,zaten code olusuyor,sadece biz kullancaz
        [JsonIgnore]
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccesful { get; private set; }

        public List<string> Errors { get; set; }

        //Statşc Factory Methods
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { Data = data, StatusCode = statusCode, IsSuccesful = true };
        }

        public static Response<T> Success(int statusCode)
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccesful = true };
        }

        public static Response<T> Fail(List<string> errors,int statusCode)
        {
            return new Response<T>
            {
                Errors = errors,
                StatusCode = statusCode,
                IsSuccesful = false
            };
        }

        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T>
            {
                Errors = new List<string> { error},
                StatusCode = statusCode,
                IsSuccesful = false
            };
        }
    }


}
