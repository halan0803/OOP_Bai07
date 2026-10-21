/********************************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
********************************/

using System;
namespace TeachingDeviceManagement
{
    public class WirelessMicrophone : TeachingDevice
    {
        public int BatteryLevel { get; }
        public int SignalQuality { get; }

        public WirelessMicrophone(string id, string name, string storageLocation, int batteryLevel, int signalQuality, DeviceStatus status = DeviceStatus.Available)
            : base(id, name, storageLocation, status)
        {
            // Mức pin từ 0 đến 100
            if (batteryLevel < 0 || batteryLevel > 100)
                throw new ArgumentOutOfRangeException(nameof(batteryLevel), "Pin phải nằm trong khoảng 0 đến 100.");
            // Chất lượng tín hiệu từ 0 đến 100
            if (signalQuality < 0 || signalQuality > 100)
                throw new ArgumentOutOfRangeException(nameof(signalQuality), "Chất lượng tín hiệu phải nằm trong khoảng 0 đến 100.");

            BatteryLevel = batteryLevel;
            SignalQuality = signalQuality;
        }

        // Chỉ sẵn sàng khi trạng thái là Available, pin từ 30% trở lên và tín hiệu từ 40% trở lên
        public override bool IsReadyForUse()
        {
            return Status == DeviceStatus.Available
                && BatteryLevel >= 30
                && SignalQuality >= 40;
        }

        // Điểm ưu tiên mặc định bằng trung bình của pin và chất lượng tín hiệu
        public override double GetDefaultPriorityScore()
        {
            return (BatteryLevel + SignalQuality) / 2.0;
        }
    }
}