using Internative.FoodRecipes.Application.Common.Interfaces;
using Internative.FoodRecipes.Domain.Common;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internative.FoodRecipes.Infrastructure.Persistence
{
    /// <summary>
    /// Represents the entity repository implementation
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        #region Fields

        private readonly IMongoCollection<TEntity> _db;
        private readonly ICurrentUserService _currentUserService;

        #endregion

        #region Ctor

        public Repository(IFoodRecipesDatabaseSettings settings, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            foreach (var property in settings.GetType().GetProperties())
            {
                if (property.PropertyType.Name.Contains(nameof(TEntity)))
                {
                    CollectionName = property.GetValue(settings).ToString();
                    break;
                }
            }

            _db = database.GetCollection<TEntity>(CollectionName); ;
        }

        #endregion

        #region Utilities

        protected virtual async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync)
        {
            return await getAllAsync();
        }

        #endregion

        #region Methods

        public async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            async Task<IList<TEntity>> getAllAsync()
            {
                var query = Collection;
                query = func != null ? func(query) : query;

                return await query.ToListAsync();
            }

            return await GetEntitiesAsync(getAllAsync);
        }

        public async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null)
        {
            async Task<IList<TEntity>> getAllAsync()
            {
                var query = Collection;
                query = func != null ? await func(query) : query;

                return await query.ToListAsync();
            }

            return await GetEntitiesAsync(getAllAsync);
        }

        public async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = Collection;

            query = func != null ? func(query) : query;

            return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
        }

        public async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null, int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = Collection;

            query = func != null ? await func(query) : query;

            return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await (await _db.FindAsync(_ => _.Id.Equals(id))).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(TEntity entity, bool publishEvent = true)
        {
            await _db.InsertOneAsync(entity);

            //if (publishEvent)
            //{
            //    await _mediator.Publish(new EntityInsertedEvent(entity));
            //}
        }

        public async Task InsertAsync(IList<TEntity> entities, bool publishEvent = true)
        {
            foreach (var entity in entities)
            {
                entity.Created = DateTime.Now;
                entity.CreatedBy = _currentUserService.UserId;
            }
            await _db.InsertManyAsync(entities);

            //if (publishEvent)
            //{
            //    foreach (var entity in entities)
            //    {
            //        await _mediator.Publish(new EntityInsertedEvent(entity));
            //    }
            //}
        }

        public async Task UpdateAsync(TEntity entity, bool publishEvent = true)
        {
            entity.LastModified = DateTime.Now;
            entity.LastModifiedBy = _currentUserService.UserId;
            await _db.ReplaceOneAsync(_ => _.Id.Equals(entity.Id), entity);
        }

        public async Task DeleteAsync(TEntity entity, bool publishEvent = true)
        {
            await _db.DeleteOneAsync(_ => _.Id.Equals(entity.Id));
        }

        public async Task DeleteAsync(IList<TEntity> entities, bool publishEvent = true)
        {
            await _db.DeleteManyAsync(_ => entities.Select(_ => _.Id).Contains(_.Id));
        }

        #endregion

        #region Properties

        public virtual IQueryable<TEntity> Collection => _db.Database.GetCollection<TEntity>(CollectionName).AsQueryable();

        public string CollectionName { get; set; }

        #endregion
    }
}
