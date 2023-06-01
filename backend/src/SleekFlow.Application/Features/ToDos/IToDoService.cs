namespace SleekFlow.Application.Features.ToDos
{
    public interface IToDoService
    {
        Task<ToDoResponse> GetToDoById(int id);

        Task<(IEnumerable<ToDoResponse> Results, long Count)> GetToDoPageList(ToDoPageFilterRequest request);

        Task<ToDoResponse> InsertToDo(string user, ToDoRequest request);

        Task<ToDoResponse> UpdateToDo(string user, int id, ToDoRequest request);

        Task<bool> DeleteToDo(int id);
    }
}
