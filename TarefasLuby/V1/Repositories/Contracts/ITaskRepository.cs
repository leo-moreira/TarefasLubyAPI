using System.Collections.Generic;
using TarefasLuby.V1.Models;

namespace TarefasLuby.V1.Repositories.Contracts
{
    public interface ITaskRepository
    {
        Task SaveTask(Task task, AppUser user);
        
        List<Task> GetTask(AppUser user);

        Task UpdateTask(Task task, AppUser user);

        void Conclude(int taskId);

        void Remove(int taskId);
    }
}
