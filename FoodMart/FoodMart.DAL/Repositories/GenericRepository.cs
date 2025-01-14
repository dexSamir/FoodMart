using System;
using System.Linq.Expressions;
using System.Security.Claims;
using FoodMart.Core.Entities.Base;
using FoodMart.Core.Repositories;
using FoodMart.DAL.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace FoodMart.DAL.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
{
    readonly AppDbContext _context;
    readonly IHttpContextAccessor _http;
    protected DbSet<T> Table => _context.Set<T>();

    public GenericRepository(AppDbContext context, IHttpContextAccessor http)
    {
        _http = http;
        _context = context;
    }

    public async Task AddAsync(T entity)
        => await Table.AddAsync(entity);


    public IQueryable<T> GetAll(params string[] includes)
    {
        var query = Table.AsQueryable();
        foreach (var include in includes)
            query = query.Include(include);
        return query;
    }

    public async Task<T?> GetByIdAysnc(int id)
        => await Table.FindAsync(id);

    public async Task<T?> GetCurrentUserAsync()
    {
        string userId = GetCurrentUserId();
        if (string.IsNullOrWhiteSpace(userId))
            return null;

        return await Table.FindAsync(userId);
    }

    public string GetCurrentUserId()
        => _http.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression)
        => Table.Where(expression).AsQueryable();

    public Task<bool> IsExistAsync(Expression<Func<T, bool>> expression)
        => Table.AnyAsync(expression);

    public Task<bool> IsExistAsync(int id)
        => Table.AnyAsync(x => x.Id == id);

    public void Remove(T entity)
    {
        Table.Remove(entity);
    }

    public async Task<bool> RemoveAsync(int id)
    {
        int result = await Table.Where(x => x.Id == id).ExecuteDeleteAsync();
        return result > 0; 
    } 
    public async Task<int> SaveAsync()
        => await _context.SaveChangesAsync(); 
}

