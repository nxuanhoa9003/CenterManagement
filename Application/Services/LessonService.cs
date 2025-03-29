using Application.InterfacesServices;
using Domain.Entities;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public async Task AddLessonAsync(Lesson lesson)
        {
            await _lessonRepository.AddLessonAsync(lesson);
        }

        public async Task UpdateLessonAsync(Lesson lesson)
        {
            await _lessonRepository.UpdateLessonAsync(lesson);
        }

        public async Task DeleteLessonAsync(Guid Id)
        {
            await _lessonRepository.DeleteLessonAsync(Id);
        }

        public async Task<IEnumerable<Lesson>> GetAllLesson()
        {
            return await _lessonRepository.GetAllLesson();
        }

        public async Task<IEnumerable<Lesson>> GetByNames(string? name)
        {
          return  await _lessonRepository.GetByNames(name);
        }

        public async Task<Lesson?> GetLessonByIdAsync(Guid Id)
        {
            return await _lessonRepository.GetLessonByIdAsync(Id);
        }

        public async Task<bool> LessonExistsAsync(Guid Id)
        {
            return await _lessonRepository.LessonExistsAsync(Id);
        }

        
    }
}
