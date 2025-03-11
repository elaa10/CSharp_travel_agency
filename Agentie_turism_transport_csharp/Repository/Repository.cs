namespace DefaultNamespace;

using System;
using System.Collections.Generic;

public interface IRepository<ID, E> where E : Entity<ID>
{
    /// <summary>
    /// Finds an entity by its ID.
    /// </summary>
    /// <param name="id">ID of the entity to be returned (must not be null).</param>
    /// <returns>The entity if found, otherwise null.</returns>
    /// <exception cref="ArgumentException">Thrown if id is null.</exception>
    E? FindOne(ID id);

    /// <summary>
    /// Returns all entities.
    /// </summary>
    /// <returns>An enumerable collection of entities.</returns>
    IEnumerable<E> FindAll();

    /// <summary>
    /// Saves an entity.
    /// </summary>
    /// <param name="entity">Entity to be saved (must not be null).</param>
    /// <returns>Null if the entity was saved, otherwise returns the existing entity.</returns>
    /// <exception cref="ArgumentException">Thrown if entity is null.</exception>
    E? Save(E entity);

    /// <summary>
    /// Removes the entity with the specified ID.
    /// </summary>
    /// <param name="id">ID of the entity to be removed (must not be null).</param>
    /// <returns>Null if there is no entity with the given ID, otherwise returns the removed entity.</returns>
    /// <exception cref="ArgumentException">Thrown if id is null.</exception>
    E? Delete(ID id);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <param name="entity">Entity to update (must not be null).</param>
    /// <returns>Null if the entity was updated, otherwise returns the existing entity.</returns>
    /// <exception cref="ArgumentException">Thrown if entity is null.</exception>
    E? Update(E entity);
}
