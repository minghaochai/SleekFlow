using SleekFlow.Domain.Entities;

namespace SleekFlow.Infrastructure.Repositories
{
    public class ToDoRepository : BaseRepository<ToDo>
    {
        public ToDoRepository(ISleekFlowDbContext context)
            : base(context)
        {
        }
    }
}
