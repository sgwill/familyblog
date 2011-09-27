using System;
using System.Linq;

namespace WilliamsonFamily.Models
{
    public interface IDataContext : IDisposable
    {
        /// <summary>
        /// Gets the repository for the given type of entities
        /// </summary>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <returns>The repository of the given type</returns>
        IQueryable<T> Repository<T>() where T : class;

        /// <summary>
        /// Adds a new entity to the repository
        /// </summary>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <param name="item">The entity to add</param>
        void Insert<T>(T item) where T : class;

        /// <summary>
        /// Deletes the specified entity from the repository
        /// </summary>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <param name="item">The entity to delete</param>
        void Delete<T>(T item) where T : class;

        /// <summary>
        /// Commits the changes to the repository
        /// </summary>
        void Commit();
    }
}
