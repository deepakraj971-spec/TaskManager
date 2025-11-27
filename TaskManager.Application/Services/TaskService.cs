// TaskManager.Application/Services/TaskService.cs
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Entities.Shared;

namespace TaskManager.Application.Services;

public class TaskService
{
    private readonly ITaskRepository _repo;
    public TaskService(ITaskRepository repo) => _repo = repo;

    public Task<PagedResult<TaskItem>> ListAsync(TaskQuery query, Guid userId) =>
        _repo.ListAsync(query, userId);
    public Task<TaskItem?> GetAsync(Guid id, Guid userId) =>
        _repo.GetAsync(id, userId);

    public async Task<TaskItem> CreateAsync(CreateTaskDto dto, Guid userId)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            Priority = dto.Priority,
            Status = dto.Status,
            UserId = userId
        };
        await _repo.AddAsync(task);
        return task;
    }

    public async Task UpdateAsync(Guid id, UpdateTaskDto dto, Guid userId)
    {
        var task = await _repo.GetAsync(id, userId) ?? throw new KeyNotFoundException();
        task.Title = dto.Title ?? task.Title;
        task.Description = dto.Description ?? task.Description;
        task.DueDate = dto.DueDate ?? task.DueDate;
        task.Priority = dto.Priority ?? task.Priority;
        task.Status = dto.Status ?? task.Status;
        task.UpdatedAt = DateTime.UtcNow;
        await _repo.UpdateAsync(task);
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var task = await _repo.GetAsync(id, userId) ?? throw new KeyNotFoundException();
        await _repo.DeleteAsync(task);
    }
}
