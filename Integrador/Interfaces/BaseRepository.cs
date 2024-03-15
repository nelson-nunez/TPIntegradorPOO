using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Integrador.Interfaces
{
    public interface IRepository<T>
    {
        bool CreateItem(List<T> lista, T entity, Expression<Func<T, bool>> filter);
        T ReadItem(List<T> lista, Expression<Func<T, bool>> filter);
        bool UpdateItem(List<T> lista, T entity, Expression<Func<T, bool>> filter);
        bool DeleteItem(List<T> lista, Expression<Func<T, bool>> filter);
    }
}
