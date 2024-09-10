using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Services
{
    public class FavoritePostService : IFavoritePostService
    {
        public FavoritePostService()
        {
            
        }

        public Task<IHomeeResult> Create(FavoritePostRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
