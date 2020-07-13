using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TarefasLuby.Database;
using TarefasLuby.V1.Models;
using TarefasLuby.V1.Repositories.Contracts;

namespace TarefasLuby.V1.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TarefasLubyContext _context;

        public TaskRepository(TarefasLubyContext context)
        {
            _context = context;
        }

        public void Conclude(int taskId)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.Id == taskId);
            task.Concluded = !task.Concluded;
            _context.SaveChanges();
        }

        public List<Task> GetTask(AppUser user)
        {
            var task = _context.Tasks.Where(x => x.UserId == user.Id);

            return task.ToList();
        }

        public void Remove(int taskId)
        {
            var task = _context.Tasks.AsNoTracking().FirstOrDefault(x => x.Id == taskId);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }

        public Task SaveTask(Task task, AppUser user)
        {
            task.UserId = user.Id;
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return task;
        }

        public Task UpdateTask(Task task, AppUser user)
        {
            task.UserId = user.Id;
            _context.Tasks.Update(task);
            _context.SaveChanges();
            return task;
        }
    }
}
