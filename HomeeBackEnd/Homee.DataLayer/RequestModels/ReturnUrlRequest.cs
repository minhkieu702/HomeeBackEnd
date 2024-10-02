using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class PAYOS_RETURN_URLRequest
    {
        public int Code { get; set; }
        public int SubId { get; set; }
        public string PaymentId { get; set; }
        public bool Cancel { get; set; }
        public string Status { get; set; }
    }
}
