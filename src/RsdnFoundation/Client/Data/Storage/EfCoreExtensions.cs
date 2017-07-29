namespace Rsdn.Client.Data.Storage
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    internal static class EfCoreExtensions
    {
        public static void AddOrUpdate<TEntity>(this DbSet<TEntity> set, TEntity entity, Func<TEntity, object> keyof)
            where TEntity : class
        {
            var found = set.Find(keyof(entity));
            if (found != null)
            {
                var entry = set.Remove(found);
                entry.State = EntityState.Detached;
                set.Update(entity);
            }
            else
            {
                set.Add(entity);
            }
        }

        public static void AddOrUpdateRange<TEntity>(this DbSet<TEntity> set, IEnumerable<TEntity> entities, Func<TEntity, object> keyof)
            where TEntity : class
        {
            foreach (var entity in entities)
            {
                set.AddOrUpdate(entity, keyof);
            }
        }

        public static void AddOrUpdateRange(this DbSet<DbRating> set, IEnumerable<DbRating> ratings)
        {
            foreach (var rating in ratings)
            {
                var found = set.Find(rating.PostId, rating.UserId, rating.Value);
                if (found != null)
                {
                    found.PostId = rating.PostId;
                    found.UserId = rating.UserId;
                    found.Value = rating.Value;
                    found.UserFactor = rating.UserFactor;
                    found.ThreadId = rating.ThreadId;
                    found.Rated = rating.Rated;
                }
                else
                {
                    set.Add(rating);
                }
            }
        }
    }
}