using App.Data.Entities;
using Microsoft.EntityFrameworkCore;
using SimoshStore;

namespace SimoshStoreAPI;

public class BlogService : IBlogService
{
    private readonly IDataRepository _dataRepository;
    public BlogService(IDataRepository dataRepository)
    {
        _dataRepository = dataRepository;
    }
    public async Task<IEnumerable<BlogEntity>> GetBlogsAsync()
    {
        var blogs = await _dataRepository.GetAll<BlogEntity>()
                                        .Include(B => B.User)
                                        .Include(B => B.Comments)
                                        .Include(B => B.BlogCategories)
                                        .ToListAsync();
        if(blogs is null)
        {
            throw new NullReferenceException("There is no blog.");
        }
        return blogs;
    }
    public async Task<BlogEntity> GetBlogByIdAsync(int id)
    {
        var blogs = await GetBlogsAsync();
        var blog = blogs.FirstOrDefault(B => B.Id==id);
        if(blog == null)
        {
            throw new NullReferenceException($"There is no blog with that {id}");
        }
        return blog;
    }
    public async Task<IServiceResult> UpdateBlogAsync(int id, BlogDTO dto)
    {
        var updatedBlog = await GetBlogByIdAsync(id);
        if(updatedBlog is null)
        {
            return new ServiceResult(false,"There is no blog with that id.");
        }
        updatedBlog.Title = dto.Title;
        updatedBlog.Content = dto.Content;
        var user = await _dataRepository.GetByIdAsync<UserEntity>(dto.userId);
        if(user is null)
        {
            return new ServiceResult(false,"There is no user with that ID");
        }
        updatedBlog.UserId = dto.userId;
        updatedBlog.ImageUrl = dto.ImageUrl;
        await _dataRepository.UpdateAsync(updatedBlog);
        return new ServiceResult(true,"Blog updated successfully.");
    }
    public async Task<IServiceResult> CreateBlogAsync(BlogDTO dto)
    {
        var blog = MappingHelper.MappingBlogEntity(dto);
        if(blog.Title==""||blog.Content=="")
        {
            return new ServiceResult(false,"Title and content must be filled.");
        }
        await _dataRepository.AddAsync(blog);
        return new ServiceResult(true,"Blog added successfully");
    }
    public async Task<IServiceResult> DeleteBlogAsync(int id)
    {
        var blog = await GetBlogByIdAsync(id);
        if(blog is null)
        {
            return new ServiceResult(false,"There is no blog with that ID.");
        }
        await _dataRepository.DeleteAsync<BlogEntity>(blog.Id);
        return new ServiceResult(true,"Blog deleted successfully.");
    }
}
