using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Homee.BusinessLayer.RequestModels;
using Homee.DataLayer;
using Homee.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IMapper mapper, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<IHomeeResult> Delete(int id)
        {
            try
            {
                var result = await GetById(id);
                
                if (result.Status != 0) return result;

                _categoryRepository.Delete(result.Data as Category);
                
                var check = await _categoryRepository.SaveChangesAsync();

                return check <= 0 ? new HomeeResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG) : new HomeeResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
        
        public async Task<IHomeeResult> GetAll()
        {
            try
            {
                var result = _categoryRepository.GetAll();
                return result.Count() <= 0 ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> GetById(int categoryId)
        {
            try
            {
                var result = await _categoryRepository.GetById(categoryId);
                return result == null ? new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> GetByName(string categoryName)
        {
            try
            {
                var result = _categoryRepository.GetAll();
                if (result.Count() == 0)
                {
                    return new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG);
                }
                var cates = result.FirstOrDefault(c => c.CategoryName.ToUpper().Trim().Equals(categoryName.ToUpper().Trim()));
                
                return cates == null ? 
                    new HomeeResult(Const.WARNING_NO_DATA_CODE, Const.WARNING_NO_DATA__MSG) : 
                    new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, result);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Insert(CategoryRequest model)
        {
            try
            {
                var result = await GetByName(model.CategoryName);
                if (result.Status > 0)
                {
                    return new HomeeResult(Const.FAIL_CREATE_CODE, "This category already exist.");
                }
                await _categoryRepository.InsertAsync(_mapper.Map<Category>(model));
                var check = await _categoryRepository.SaveChangesAsync();
                return check <= 0 ? 
                    new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG) : 
                    new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Update(int id, CategoryRequest model)
        {
            try
            {
                var result = _categoryRepository.CanUpdate(id, model.CategoryName);
                if (result == -1)
                {
                    return new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
                }
                var cate = _mapper.Map<Category>(model);
                cate.CategoryId = id;
                await _categoryRepository.UpdateById(cate, id);
                var check = await _categoryRepository.SaveChangesAsync();
                return check > 0 ? 
                    new HomeeResult(Const.SUCCESS_UPDATE_CODE, Const.SUCCESS_UPDATE_MSG) :
                    new HomeeResult(Const.FAIL_UPDATE_CODE, Const.FAIL_UPDATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }
    }
}
