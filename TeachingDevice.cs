/********************************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
********************************/

using System;
namespace TeachingDeviceManagement;
{
    public enum DeviceStatus 
    { 
        Available, 
        Borrowed, 
        UnderMaintenance 
    }

    public abstract class TeachingDevice : IIdentifiable
    {
        public string Id { get; }
        public string Name { get; }
        public string StorageLocation { get; }
        public DeviceStatus Status { get; protected set; }

        protected TeachingDevice(string id, string name, string storageLocation, DeviceStatus status = DeviceStatus.Available)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Mã thiết bị không được để trống.", nameof(id));

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Tên thiết bị không được để trống.", nameof(name));

            if (string.IsNullOrWhiteSpace(storageLocation))
                throw new ArgumentException("Vị trí lưu trữ không được để trống.", nameof(storageLocation));

            Id = id.Trim();
            Name = name.Trim();
            StorageLocation = storageLocation.Trim();
            Status = status;
        }

        public abstract bool IsReadyForUse();

        public abstract double GetDefaultPriorityScore();

        public void Borrow()
        {
            if (!IsReadyForUse())
                throw new DeviceNotReadyException(Id, Status);

            Status = DeviceStatus.Borrowed;
        }

        public void Return()
        {
            if (Status != DeviceStatus.Borrowed)
                throw new InvalidOperationException("Chỉ thiết bị đang được mượn mới có thể được trả.");

            Status = DeviceStatus.Available;
        }

        public override string ToString()
        {
            return $"{Id} - {Name} | Vị trí: {StorageLocation} | Trạng thái: {Status}";
        }
    }
}