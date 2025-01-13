using System;
using System.Linq.Expressions;
using FoodMart.Core.Entities.Base;

namespace FoodMart.Core.Repositories;
public interface IGenericRepository<T> where T : BaseEntity, new()
{
    IQueryable<T> GetAll(params string[] includes);
    Task<T?> GetByIdAysnc(int id);
    IQueryable<T> GetWhere(Expression<Func<T, bool>> expression);
    Task<bool> IsExistAsync(Expression<Func<T, bool>> expression);
    Task<bool> IsExistAsync(int id);
    Task AddAsync(T entity);
    Task<bool> RemoveAsync(int id);
    void Remove(T entity);
    Task<int> SaveAsync();
    string GetCurrentUserId();
    Task<T?> GetCurrentUserAsync(); 
}

