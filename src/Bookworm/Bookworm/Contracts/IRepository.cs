using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookworm.Contracts
{
    public interface IRepository <TEntity>
    {
        bool Create(TEntity entity);
        void Update(TEntity entity);
      //  void Delete(int id);
        bool Delete(TEntity entity);
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        IEnumerable<TEntity> GetListOf();
        bool IsPresentInTable(TEntity entity);
    }
}
