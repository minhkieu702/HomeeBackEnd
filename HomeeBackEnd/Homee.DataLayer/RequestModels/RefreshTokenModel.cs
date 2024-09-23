using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.DataLayer.RequestModels
{
    public class RefreshTokenModel
    {
        public int AccountId { get; set; }
        public int Role { get; set; }
        public string RefreshToken { get; set; }
    }
}
