﻿using AutoMapper;
using Homee.BusinessLayer.Commons;
using Homee.BusinessLayer.IServices;
using Homee.DataLayer.RequestModels;
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
        public Task<IHomeeResult> Create(PostRequest model)
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

        public Task<IHomeeResult> GetByCurrentUser(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IHomeeResult> Update(int id, PostRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
