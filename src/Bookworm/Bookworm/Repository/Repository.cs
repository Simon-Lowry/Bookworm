using Bookworm.Contracts;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using Bookworm.Data;
using Microsoft.Ajax.Utilities;

namespace Bookworm.Repository
{
    public class Repository<TEntity> : IRepository <TEntity> where TEntity : class
    {
        protected BookwormDbContext DbContext { get; set; }
        private DbSet<TEntity> Entities;
      

        public Repository(BookwormDbContext dbContext)
        {
            DbContext = dbContext;
            Entities = dbContext.Set<TEntity>();
        }


        public virtual TEntity Get(int id) 
        {
            return Entities.Find(id);
        }


        public virtual IEnumerable<TEntity> GetListOf()
        {
            return Entities.ToList();
        }


        public virtual bool Create(TEntity entity)  
        {
            Entities.Add(entity);
            DbContext.SaveChanges();
            return true;
        }


        public virtual void Update(TEntity entity)

        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
          
            Entities.Attach(entity);
            DbEntityEntry dbEntityEntry = this.DbContext.Entry<TEntity>(entity);
            dbEntityEntry.State = EntityState.Modified;
            DbContext.SaveChanges();
        }


        public virtual bool Delete(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = this.DbContext.Entry<TEntity>(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {

                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
               
                Entities.Attach(entity);
                Entities.Remove(entity);
                dbEntityEntry.State = EntityState.Deleted;
            }
            DbContext.SaveChanges();
            return true;
        }


        // not working
        public virtual bool IsPresentInTable(TEntity entity)
        {
            return (DbContext.Set<TEntity>().Attach(entity) != null) ? true : false;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return Entities.AsEnumerable();
        }
    }
}