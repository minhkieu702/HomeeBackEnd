using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.Models;
using Homee.DataLayer.RequestModels;
using Homee.DataLayer.ResponseModels;
using Homee.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homee.BusinessLayer.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _repo;

        public PostService(IMapper mapper, IPostRepository postRepository)
        {
            _mapper = mapper;
            _repo = postRepository;
        }
        public async Task<IHomeeResult> Create(PostRequest model)
        {
            try
            {
                var result = _mapper.Map<Post>(model);
                result.IsBlock = false;
                await _repo.InsertAsync(result);
                var check = await _repo.SaveChangesAsync();
                return check >= 0 ? new HomeeResult(Const.SUCCESS_CREATE_CODE, Const.SUCCESS_CREATE_MSG) : new HomeeResult(Const.FAIL_CREATE_CODE, Const.FAIL_CREATE_MSG);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public async Task<IHomeeResult> Delete(int id)
        {
            try
            {
                var result = await _repo.GetById(id);
                if (result == null)
                {
                    return new HomeeResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
                }
                int check = await _repo.DeletePost(id);
                return check >= 0 ? new HomeeResult(Const.SUCCESS_DELETE_CODE, Const.SUCCESS_DELETE_MSG) : new HomeeResult(Const.FAIL_DELETE_CODE, Const.FAIL_DELETE_MSG);
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
                var result = await _repo.GetPosts();
                if (result == null || result.Count() <= 0)
                    return new HomeeResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                var posts = result.ToList().Select(_mapper.Map<PostResponse>);
                return new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, posts);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public Task<IHomeeResult> GetByCurrentUser(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public async Task<IHomeeResult> GetById(int id)
        {
            try
            {
                var result = await _repo.GetPostById(id);
                if (result == null)
                    return new HomeeResult(Const.FAIL_READ_CODE, Const.FAIL_READ_MSG);
                var posts = _mapper.Map<PostResponse>(result);
                return new HomeeResult(Const.SUCCESS_READ_CODE, Const.SUCCESS_READ_MSG, posts);
            }
            catch (Exception ex)
            {
                return new HomeeResult(Const.ERROR_EXCEPTION, ex.Message);
            }
        }

        public Task<IHomeeResult> Update(int id, PostRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
