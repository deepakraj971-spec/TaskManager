// TaskManager.Application/Interfaces/ITaskRepository.cs
using TaskManager.Domain.Entities;
using TaskManager.Domain.Entities.Shared;

namespace TaskManager.Application.Interfaces;

public class TaskQuery
{
    public Status? Status { get; set; }
    public Priority? Priority { get; set; }
    public string? Search { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public interface ITaskRepository
{
    Task<TaskItem?> GetAsync(Guid id, Guid userId);
    Task<PagedResult<TaskItem>> ListAsync(TaskQuery query, Guid userId);
    Task AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
}
