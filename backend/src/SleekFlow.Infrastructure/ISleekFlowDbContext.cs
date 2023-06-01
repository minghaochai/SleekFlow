namespace SleekFlow.Infrastructure
{
    public interface ISleekFlowDbContext : IDisposable
    {
        SleekFlowDbContext Instance { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
