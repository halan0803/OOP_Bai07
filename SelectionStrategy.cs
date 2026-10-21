/********************************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
********************************/

using System;
using System.Collections.Generic;
namespace TeachingDeviceManagement
{
    // Interface chung cho các chiến lược lựa chọn đối tượng
    public interface ISelectionStrategy<T>
    {
        T Select(IReadOnlyList<T> candidates);
    }

    // Chọn đối tượng có điểm ưu tiên cao nhất
    public class HighestPriorityStrategy<T> : ISelectionStrategy<T>
    {
        private readonly Func<T, double> scoreSelector;

        public HighestPriorityStrategy(Func<T, double> scoreSelector)
        {
            this.scoreSelector = scoreSelector
                ?? throw new ArgumentNullException(nameof(scoreSelector));
        }

        public T Select(IReadOnlyList<T> candidates)
        {
            // Nếu danh sách ứng viên rỗng, chiến lược phát sinh NoSuitableItemException
            if (candidates == null || candidates.Count == 0)
                throw new NoSuitableItemException();

            T selected = candidates[0];
            double highestScore = scoreSelector(selected);

            foreach (T candidate in candidates)
            {
                double score = scoreSelector(candidate);

                if (score > highestScore)
                {
                    highestScore = score;
                    selected = candidate;
                }
            }

            // Chiến lược không thay đổi danh sách đầu vào
            return selected;
        }
    }

    // Chọn phần tử đầu tiên trong danh sách ứng viên
    public class FirstAvailableStrategy<T> : ISelectionStrategy<T>
    {
        public T Select(IReadOnlyList<T> candidates)
        {
            // Nếu danh sách ứng viên rỗng, chiến lược phát sinh NoSuitableItemException
            if (candidates == null || candidates.Count == 0)
                throw new NoSuitableItemException();

            // Chiến lược không thay đổi danh sách đầu vào
            return candidates[0];
        }
    }
}