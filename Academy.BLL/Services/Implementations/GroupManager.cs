using Academy.BLL.DTOs;
using Academy.BLL.Services.Interfaces;
using Academy.DAL.DataContext;
using Academy.DAL.DataContext.Entities;
using Academy.DAL.Repositories.Interfaces;
using AutoMapper;
using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Academy.BLL.Services.Implementations
{
    public class GroupManager : CrudManager<Group, GroupDto, CreateGroupDto, UpdateGroupDto>, IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GroupManager(IGroupRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _groupRepository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupDto>> GetByTeacherUserIdAsync(string userId)
        {
            var groups = await _groupRepository.GetByTeacherUserIdAsync(userId);
            return _mapper.Map<IEnumerable<GroupDto>>(groups);
        }
    }
}
