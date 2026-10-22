/********************************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
********************************/

using System;
using System.Collections.Generic;
namespace TeachingDeviceManagement
{
    public class AllocationService<T> where T : TeachingDevice
    {
        private readonly IRepository<T> repository;
        private ISelectionStrategy<T> strategy;

        public AllocationService(IRepository<T> repository, ISelectionStrategy<T> strategy)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        public void ChangeStrategy(ISelectionStrategy<T> newStrategy)
        {
            strategy = newStrategy ?? throw new ArgumentNullException(nameof(newStrategy));
        }

        public T Allocate(Predicate<T>? additionalCondition = null)
        {
            // Lấy các thiết bị từ repository, chỉ giữ thiết bị sẵn sàng
            // và thỏa mãn điều kiện bổ sung nếu được cung cấp
            IReadOnlyList<T> candidates = repository.FindAll(device =>
                device.IsReadyForUse()
                && (additionalCondition == null || additionalCondition(device)));

            // Dùng chiến lược hiện tại để chọn thiết bị
            T selected = strategy.Select(candidates);
            // Gọi Borrow() trên thiết bị được chọn
            selected.Borrow();
            // Trả về thiết bị đã cấp phát
            return selected;
        }
    }
}