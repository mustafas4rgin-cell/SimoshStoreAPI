﻿using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace SimoshStoreAPI;

public class CategoryService : ICategoryService
{
    private readonly IDataRepository _Repository;
    public CategoryService(IDataRepository repository)
    {
        _Repository = repository;
    }
    public async Task<IServiceResult> CreateCategoryAsync(CategoryDTO dto)
    {
        var validationResult = Validator.Validate(dto);
        if (!validationResult.Success)
        {
            return validationResult;
        }
        var category = MappingHelper.MappingCategory(dto);
        await _Repository.AddAsync(category);
        return new ServiceResult(true, "Category created successfully");
    }
    public async Task<IEnumerable<CategoryEntity>> GetCategoriesAsync()
    {
        var categories = await _Repository.GetAll<CategoryEntity>().ToListAsync();
        if (categories == null)
        {
            throw new Exception("No categories found");
        }
        return categories;
    }
    public async Task<IServiceResult> DeleteCategoryAsync(int id)
    {
        var category = await _Repository.GetByIdAsync<CategoryEntity>(id);
        if (category is null)
        {
            return new ServiceResult(false, "Category not found");
        }
        var products = _Repository.GetAll<ProductEntity>().Where(x => x.CategoryId == id).ToList();
        if (products.Count > 0)
        {
            return new ServiceResult(false, "Category has products");
        }
        await _Repository.DeleteAsync<CategoryEntity>(id);
        return new ServiceResult(true, "Category deleted successfully");
    }
    public async Task<IServiceResult> UpdateCategoryAsync(CategoryDTO dto, int id)
    {
        var category = await _Repository.GetByIdAsync<CategoryEntity>(id);
        if (category is null)
        {
            return new ServiceResult(false, "Category not found");
        }
        category.Name = dto.Name;
        await _Repository.UpdateAsync(category);
        return new ServiceResult(true, "Category updated successfully");
    }
    public async Task<CategoryEntity> GetCategoryByIdAsync(int id)
    {
        var category = await _Repository.GetByIdAsync<CategoryEntity>(id);
        if (category is null)
        {
            throw new Exception("Category not found");
        }
        return category;
    }
}
