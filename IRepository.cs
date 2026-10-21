/********************************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
********************************/

using System;
namespace TeachingDeviceManagement
{
    public interface IRepository<T> where T : class, IIdentifiable
    {
        void Add(T item);
        bool Remove(string id);
        T? FindById(string id);
        IReadOnlyList<T> GetAll();
        IReadOnlyList<T> FindAll(Predicate<T> condition);
    }
}