/********************************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
********************************/

using System;
using System.Collections.Generic;
using System.Linq;
namespace TeachingDeviceManagement
{
    public class InMemoryRepository<T> : IRepository<T> where T : class, IIdentifiable
    {
        private readonly Dictionary<string, T> items = new(StringComparer.OrdinalIgnoreCase);

        public void Add(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (items.ContainsKey(item.Id))
                throw new DuplicateIdException(item.Id);

            items.Add(item.Id, item);
        }

        public bool Remove(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            return items.Remove(id.Trim());
        }

        public T? FindById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            items.TryGetValue(id.Trim(), out T? item);
            return item;
        }

        public IReadOnlyList<T> GetAll()
        {
            return items.Values.ToList().AsReadOnly();
        }

        public IReadOnlyList<T> FindAll(Predicate<T> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            return items.Values.Where(item => condition(item)).ToList().AsReadOnly();
        }
    }
}