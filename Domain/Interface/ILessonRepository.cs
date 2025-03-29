using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface
{
    public interface ILessonRepository : IGenericRepository<Lesson>
    {
        Task<IEnumerable<Lesson>> GetByNames(string? name);
        Task<IEnumerable<Lesson>> GetAllLesson();
        Task<Lesson?> GetLessonByIdAsync(Guid Id);
        Task AddLessonAsync(Lesson lesson);
        Task UpdateLessonAsync(Lesson lesson);
        Task DeleteLessonAsync(Guid Id);
        Task<bool> LessonExistsAsync(Guid Id);
    }
}
