using System;

namespace repository;
public interface IRepository<T>
{
    Task<IEnumerable<T>> Get();
    Task<T?> GetById(int id);
    Task Add(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task Save();
}