using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Commons
{
    public interface IHomeeResult
    {
        int Status { get; set; }
        string? Message { get; set; }
        object? Data { get; set; }
    }
    public class HomeeResult : IHomeeResult
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public HomeeResult()
        {
            Status = -1;
            Message = "Action fail";
        }

        public HomeeResult(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public HomeeResult(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
