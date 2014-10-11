using System;
using System.Linq;

namespace Blog.Models
{
    public class JsonReturnObject
    {
        public JsonReturnObject(object data, int status, bool success)
        {
            Data = data;
            Status = status;
            Success = success;
        }

        public object Data { get; set; }
        public int Status { get; set; }
        public bool Success { get; set; }
    }
}