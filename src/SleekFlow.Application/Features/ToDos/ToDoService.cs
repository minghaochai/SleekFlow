using AutoMapper;
using SleekFlow.Application.Common.Exceptions;
using SleekFlow.Domain.Entities;
using SleekFlow.Domain.Filters;
using SleekFlow.Domain.Interfaces;

namespace SleekFlow.Application.Features.ToDos
{
    public class ToDoService : IToDoService
    {
        private readonly IBaseRepository<ToDo> _toDoRepository;
        private readonly IMapper _mapper;

        public ToDoService(
            IBaseRepository<ToDo> toDoRepository,
            IMapper mapper)
        {
            _toDoRepository = toDoRepository;
            _mapper = mapper;
        }

        public async Task<ToDoResponse> GetToDoById(int id)
        {
            var mapping = await _toDoRepository.GetById(id);
            if (mapping == null)
            {
                throw new NotFoundException("To do not found");
            }

            var result = _mapper.Map<ToDoResponse>(mapping);
            return result;
        }

        public async Task<(IEnumerable<ToDoResponse> Results, long Count)> GetToDoPageList(ToDoPageFilterRequest request)
        {
            var filter = _mapper.Map<ToDoPageFilter>(request);
            var (data, count) = await _toDoRepository.GetPageList(filter);
            var results = _mapper.Map<IEnumerable<ToDoResponse>>(data);
            return (results, count);
        }

        public async Task<ToDoResponse> InsertToDo(string user, ToDoRequest request)
        {
            var toDo = _mapper.Map<ToDo>(request);
            toDo.AddAt = DateTime.UtcNow;
            toDo.AddBy = user;
            toDo.EditAt = null;
            toDo.EditBy = null;
            await _toDoRepository.Insert(toDo);
            return _mapper.Map<ToDoResponse>(toDo);
        }

        public async Task<ToDoResponse> UpdateToDo(string user, int id, ToDoRequest request)
        {
            var data = await _toDoRepository.GetById(id);
            if (data == null)
            {
                throw new NotFoundException($"To do not found");
            }
            var toDo = _mapper.Map<ToDo>(request);
            toDo.EditAt = DateTime.UtcNow;
            toDo.EditBy = user;
            toDo.Id = data.Id;
            toDo.AddAt = data.AddAt;
            toDo.AddBy = data.AddBy;
            await _toDoRepository.Update(toDo);
            return _mapper.Map<ToDoResponse>(toDo);
        }

        public async Task<bool> DeleteToDo(int id)
        {
            var toDo = await _toDoRepository.GetById(id);
            if (toDo == null)
            {
                throw new NotFoundException("To do not found");
            }
            await _toDoRepository.Delete(toDo);
            return true;
        }
    }
}
