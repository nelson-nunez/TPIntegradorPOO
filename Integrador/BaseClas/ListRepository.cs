using Integrador.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Integrador.BaseClas
{
    public class ListRepository<T> : IRepository<T>
    {
        public virtual bool CreateItem(List<T> lista, T entity, Expression<Func<T, bool>> filter)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "La entidad no puede ser nula.");

            if (lista.Any(filter.Compile()))
                throw new Exception("El elemento ya existe en la lista.");

            if (lista == null)
                lista = new List<T>();

            lista.Add(entity);
            return true;
        }

        public virtual T ReadItem(List<T> lista, Expression<Func<T, bool>> filter)
        {
            var item = lista.FirstOrDefault(filter.Compile());
            if (item != null)
                return item;
            else
                throw new Exception($"El elemento no existe en la lista.");
        }

        public virtual bool UpdateItem(List<T> lista, T entity, Expression<Func<T, bool>> filter) 
        {
            var itemToUpdate = lista.FirstOrDefault(filter.Compile());
            if (itemToUpdate == null)
                throw new Exception("El elemento a actualizar no existe en la lista.");

            foreach (var property in typeof(T).GetProperties())
            {
                var newValue = property.GetValue(entity);
                if (newValue != null)
                {
                    property.SetValue(itemToUpdate, newValue);
                }
            }

            return true;
        }
            
        public virtual bool DeleteItem(List<T> lista, Expression<Func<T, bool>> filter)
        {
            var item = lista.FirstOrDefault(filter.Compile());
            if (item == null)
              throw new Exception("El elemento no existe en la lista.");

            lista.Remove(item);
            return true;
        }
    }
}
