// TaskManager.Infrastructure/Repositories/TaskRepository.cs
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Entities.Shared;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;
    public TaskRepository(AppDbContext db) => _db = db;

    public Task<TaskItem?> GetAsync(Guid id, Guid userId) =>
        _db.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

    public async Task<PagedResult<TaskItem>> ListAsync(TaskQuery q, Guid userId)
    {
        var query = _db.Tasks.Where(t => t.UserId == userId);

        if (q.Status.HasValue)
            query = query.Where(t => t.Status == q.Status);

        if (q.Priority.HasValue)
            query = query.Where(t => t.Priority == q.Priority);

        if (!string.IsNullOrWhiteSpace(q.Search))
            query = query.Where(t => t.Title.Contains(q.Search));

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(t => t.UpdatedAt)
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .ToListAsync();

        return new PagedResult<TaskItem>
        {
            Items = items,
            Total = total,
            Page = q.Page,
            PageSize = q.PageSize
        };
    }


    public async Task AddAsync(TaskItem task) { _db.Tasks.Add(task); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(TaskItem task) { _db.Tasks.Update(task); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(TaskItem task) { _db.Tasks.Remove(task); await _db.SaveChangesAsync(); }
}
