using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class ReturnUrlRequest
    {
        public int Code { get; set; }
        public string Id { get; set; }
        public long OrderCode { get; set; }
        public bool Cancel { get; set; }
        public string Status { get; set; }
    }
}
