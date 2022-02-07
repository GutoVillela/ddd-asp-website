﻿using KadoshDomain.Commands;
using KadoshDomain.Entities;
using KadoshDomain.Handlers;
using KadoshDomain.Repositories;
using KadoshDomain.Services.Interfaces;
using KadoshShared.Commands;

namespace KadoshDomain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ICommandResult> CreateCategoryAsync(CreateCategoryCommand command)
        {
            CategoryHandler categoryHandler = new(_categoryRepository);
            return await categoryHandler.HandleAsync(command);
        }

        public async Task<ICommandResult> DeleteCategoryAsync(DeleteCategoryCommand command)
        {
            CategoryHandler categoryHandler = new(_categoryRepository);
            return await categoryHandler.HandleAsync(command);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.ReadAllAsync();
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _categoryRepository.ReadAsync(id);
        }

        public async Task<ICommandResult> UpdateCategoryAsync(UpdateCategoryCommand command)
        {
            CategoryHandler categoryHandler = new(_categoryRepository);
            return await categoryHandler.HandleAsync(command);
        }
    }
}
