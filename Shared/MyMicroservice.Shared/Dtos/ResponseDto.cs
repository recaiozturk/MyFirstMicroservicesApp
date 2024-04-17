using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MyMicroservice.Shared.Dtos
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }

        //json ignore ile cliente status code gösttermeye gerek yok,zaten code olusuyor,sadece biz kullancaz
        [JsonIgnore]
        public int StatusCode { get; set; }

        [JsonIgnore]
        public bool IsSuccesful { get; set; }
    }
}
