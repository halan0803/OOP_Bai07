/********************************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
********************************/

using System;
namespace TeachingDeviceManagement;
{
    public class ProjectorRemote : TeachingDevice
    {
        public int BatteryLevel { get; }

        public ProjectorRemote(string id, string name, string storageLocation, int batteryLevel, DeviceStatus status = DeviceStatus.Available)
            : base(id, name, storageLocation, status)
        {
            // Mức pin từ 0 đến 100
            if (batteryLevel < 0 || batteryLevel > 100)
                throw new ArgumentOutOfRangeException(nameof(batteryLevel), "Pin phải nằm trong khoảng 0 đến 100.");

            BatteryLevel = batteryLevel;
        }

        // Chỉ sẵn sàng cấp phát khi trạng thái là Available và mức pin từ 20% trở lên
        public override bool IsReadyForUse()
        {
            return Status == DeviceStatus.Available && BatteryLevel >= 20;
        }

        // Điểm ưu tiên mặc định bằng mức pin
        public override double GetDefaultPriorityScore()
        {
            return BatteryLevel;
        }
    }
}