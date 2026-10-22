/********************************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
********************************/

using System;
namespace TeachingDeviceManagement
{
    public class DuplicateIdException : Exception
    {
        public string DuplicateId { get; }

        public DuplicateIdException(string duplicateId)
            : base($"Mã thiết bị '{duplicateId}' đã tồn tại.")
        {
            DuplicateId = duplicateId;
        }
    }

    public class NoSuitableItemException : Exception
    {
        public NoSuitableItemException()
            : base("Không tìm thấy thiết bị phù hợp.")
        {
        }
    }

    public class DeviceNotReadyException : Exception
    {
        public string DeviceId { get; }
        public DeviceStatus CurrentStatus { get; }

        public DeviceNotReadyException(string deviceId, DeviceStatus currentStatus)
            : base($"Thiết bị '{deviceId}' không đáp ứng điều kiện sẵn sàng (trạng thái hiện tại: {currentStatus}).")
        {
            DeviceId = deviceId;
            CurrentStatus = currentStatus;
        }
    }
}