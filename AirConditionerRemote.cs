/********************************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
********************************/

using System;
namespace TeachingDeviceManagement
{
    public class AirConditionerRemote : TeachingDevice
    {
        // Danh sách mã phòng tương thích với điều khiển điều hòa
        public IReadOnlySet<string> CompatibleRooms { get; }

        public AirConditionerRemote(string id, string name, string storageLocation, IEnumerable<string> compatibleRooms, DeviceStatus status = DeviceStatus.Available)
            : base(id, name, storageLocation, status)
        {
            if (compatibleRooms == null)
                throw new ArgumentNullException(nameof(compatibleRooms));

            HashSet<string> rooms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string room in compatibleRooms)
            {
                if (!string.IsNullOrWhiteSpace(room))
                    rooms.Add(room.Trim());
            }

            CompatibleRooms = rooms;
        }

        public bool IsCompatibleWith(string roomId)
        {
            return !string.IsNullOrWhiteSpace(roomId)
                && CompatibleRooms.Contains(roomId.Trim());
        }

        // Chỉ sẵn sàng khi trạng thái là Available
        public override bool IsReadyForUse()
        {
            return Status == DeviceStatus.Available;
        }

        public override double GetDefaultPriorityScore()
        {
            // Điểm ưu tiên mặc định có thể bằng 100 nếu tương thích, ngược lại bằng 0
            // Phòng cụ thể được kiểm tra bằng IsCompatibleWith trong điều kiện cấp phát
            return CompatibleRooms.Count > 0 ? 100 : 0;
        }
    }
}