using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{
    public class GenericCommandResult
    {
        public GenericCommandResult() { }
        public GenericCommandResult(bool success, string message, object data) 
        {
            Message = message;
            Data = data;
            Success = success;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
