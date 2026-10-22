/********************************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
********************************/

using System;
using System.Collections.Generic;
namespace TeachingDeviceManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== BẮT ĐẦU KIỂM THỬ HỆ THỐNG QUẢN LÝ THIẾT BỊ GIẢNG DẠY ===\n");

            // Khởi tạo các repository tổng quát
            var projectorRepo = new InMemoryRepository<ProjectorRemote>();
            var acRepo = new InMemoryRepository<AirConditionerRemote>();
            var micRepo = new InMemoryRepository<WirelessMicrophone>();

            // Khởi tạo thiết bị theo yêu cầu 9:
            // 3 máy chiếu, 2 điều hòa, 3 micro
            var p1 = new ProjectorRemote("PRJ01", "ĐK Máy chiếu 1", "Tủ 1", 100);
            var p2 = new ProjectorRemote("PRJ02", "ĐK Máy chiếu 2", "Tủ 1", 15); // Pin < 20 => Không sẵn sàng
            var p3 = new ProjectorRemote("PRJ03", "ĐK Máy chiếu 3", "Tủ 1", 80);

            var ac1 = new AirConditionerRemote("AC01", "ĐK Điều hòa 1", "Tủ 2", new[] { "A203", "A204" });
            var ac2 = new AirConditionerRemote("AC02", "ĐK Điều hòa 2", "Tủ 2", new[] { "B101" });

            var mic1 = new WirelessMicrophone("MIC01", "Micro 1", "Tủ 3", 100, 100);
            var mic2 = new WirelessMicrophone("MIC02", "Micro 2", "Tủ 3", 20, 90); // Pin < 30 => Không sẵn sàng
            var mic3 = new WirelessMicrophone("MIC03", "Micro 3", "Tủ 3", 80, 30); // Tín hiệu < 40 => Không sẵn sàng

            // 1. Thêm thiết bị vào repository
            Console.WriteLine("1. Thêm thiết bị vào repository");
            projectorRepo.Add(p1);
            projectorRepo.Add(p2);
            projectorRepo.Add(p3);
            acRepo.Add(ac1);
            acRepo.Add(ac2);
            micRepo.Add(mic1);
            micRepo.Add(mic2);
            micRepo.Add(mic3);
            Console.WriteLine("=> Đã thêm thành công.\n");

            // 2. Thử thêm hai thiết bị trùng mã
            Console.WriteLine("2. Thử thêm hai thiết bị trùng mã");
            try
            {
                var duplicateProjector = new ProjectorRemote("PRJ01", "ĐK Trùng lặp", "Tủ 1", 50);
                projectorRepo.Add(duplicateProjector);
                Console.WriteLine("=> FAIL: Không phát hiện mã trùng.");
            }
            catch (DuplicateIdException ex)
            {
                Console.WriteLine($"=> PASS: {ex.Message}\n");
            }

            // 3. Tìm thiết bị theo mã
            Console.WriteLine("3. Tìm thiết bị theo mã (Id: AC01)");
            var foundAc = acRepo.FindById("AC01");
            Console.WriteLine($"=> Tìm thấy: {foundAc?.Name} - Hỗ trợ {foundAc?.CompatibleRooms.Count} phòng.\n");

            // 4. Lọc các thiết bị sẵn sàng
            Console.WriteLine("4. Lọc các micro sẵn sàng");
            var readyMics = micRepo.FindAll(m => m.IsReadyForUse());
            Console.WriteLine($"=> Số lượng micro sẵn sàng: {readyMics.Count} (Kỳ vọng: 1 - là MIC01).");
            foreach (var m in readyMics) Console.WriteLine($"   - {m.Id}");
            if (readyMics.Count == 1 && readyMics[0].Id == "MIC01")
                Console.WriteLine("=> PASS: Chỉ MIC01 sẵn sàng.");
            else
                Console.WriteLine("=> FAIL: Kết quả lọc chưa đúng.");
            Console.WriteLine();

            // Khởi tạo dịch vụ
            var highestPriorityStrategy = new HighestPriorityStrategy<ProjectorRemote>(d => d.GetDefaultPriorityScore());
            var projectorService = new AllocationService<ProjectorRemote>(projectorRepo, highestPriorityStrategy);

            // 5. Chọn điều khiển máy chiếu có điểm cao nhất
            Console.WriteLine("5. Chọn điều khiển máy chiếu có điểm cao nhất");
            var selectedProjector = projectorService.Allocate(d => true);
            Console.WriteLine($"=> Đã cấp phát: {selectedProjector.Id} (Mức pin: {selectedProjector.BatteryLevel}%).\n");

            // 6. Chọn điều khiển điều hòa tương thích với một phòng
            Console.WriteLine("6. Chọn điều khiển điều hòa tương thích với phòng A203");
            var acStrategy = new FirstAvailableStrategy<AirConditionerRemote>();
            var acService = new AllocationService<AirConditionerRemote>(acRepo, acStrategy);
            var selectedAc = acService.Allocate(d => d.IsCompatibleWith("A203"));
            Console.WriteLine($"=> Đã cấp phát: {selectedAc.Id}\n");

            // 7. Chọn micro đầu tiên sẵn sàng
            Console.WriteLine("7. Chọn micro đầu tiên sẵn sàng");
            var micStrategy = new FirstAvailableStrategy<WirelessMicrophone>();
            var micService = new AllocationService<WirelessMicrophone>(micRepo, micStrategy);
            var selectedMic = micService.Allocate(d => true);
            Console.WriteLine($"=> Đã cấp phát: {selectedMic.Id}\n");

            // 8. Đổi chiến lược trong thời gian chạy
            Console.WriteLine("8. Đổi chiến lược trong thời gian chạy");

            // Repo riêng để không bị ảnh hưởng bởi trạng thái Borrowed ở các bước trước
            var demoRepo = new InMemoryRepository<ProjectorRemote>();
            demoRepo.Add(new ProjectorRemote("D01", "ĐK Demo 1", "Tủ 1", 30)); // đứng đầu, pin thấp nhưng sẵn sàng
            demoRepo.Add(new ProjectorRemote("D02", "ĐK Demo 2", "Tủ 1", 90)); // pin cao nhất
            demoRepo.Add(new ProjectorRemote("D03", "ĐK Demo 3", "Tủ 1", 60));

            var demoService = new AllocationService<ProjectorRemote>(
                demoRepo,
                new HighestPriorityStrategy<ProjectorRemote>(d => d.GetDefaultPriorityScore()));

            var byPriority = demoService.Allocate();
            Console.WriteLine($"=> Chiến lược HighestPriority chọn: {byPriority.Id}");
            byPriority.Return(); // trả lại để lần chọn sau có cùng tập ứng viên

            demoService.ChangeStrategy(new FirstAvailableStrategy<ProjectorRemote>());
            var byFirst = demoService.Allocate();
            Console.WriteLine($"=> Chiến lược FirstAvailable chọn:  {byFirst.Id}");
            byFirst.Return();

            if (byPriority.Id == "D02" && byFirst.Id == "D01")
                Console.WriteLine("=> PASS: Đổi chiến lược lúc chạy cho kết quả khác nhau.\n");
            else
                Console.WriteLine("=> FAIL: Kết quả không như kỳ vọng.\n");

            // 9. Thử cấp phát khi không có ứng viên phù hợp
            Console.WriteLine("9. Thử cấp phát khi không có ứng viên phù hợp (Phòng C999)");
            try
            {
                acService.Allocate(d => d.IsCompatibleWith("C999"));
                Console.WriteLine("=> FAIL: Không phát sinh ngoại lệ.");
            }
            catch (NoSuitableItemException ex)
            {
                Console.WriteLine($"=> PASS: {ex.Message}\n");
            }

            // 10. Chứng minh thiết bị được chọn chuyển sang Borrowed
            Console.WriteLine("10. Chứng minh thiết bị được chọn chuyển sang Borrowed");
            Console.WriteLine($"=> Trạng thái hiện tại của {selectedProjector.Id}: {selectedProjector.Status}");
            if (selectedProjector.Status == DeviceStatus.Borrowed)
                Console.WriteLine("=> PASS: Thiết bị đã chuyển sang trạng thái Borrowed.\n");
            else
                Console.WriteLine("=> FAIL: Trạng thái thiết bị chưa đúng.\n");

            // 11. Chứng minh repository tổng quát có thể dùng cho ít nhất hai loại thiết bị mà không sao chép mã
            Console.WriteLine("11. Chứng minh tính tổng quát của Generic Repository");
            Console.WriteLine("=> Một hàm PrintRepository<T> và một lớp InMemoryRepository<T> phục vụ cả ba kiểu:");
            PrintRepository(projectorRepo);
            PrintRepository(acRepo);
            PrintRepository(micRepo);
            Console.WriteLine();

            Console.WriteLine("=== HOÀN THÀNH KIỂM THỬ ===");
        }

        static void PrintRepository<T>(IRepository<T> repo) where T : class, IIdentifiable
        {
            Console.WriteLine($"   Repository<{typeof(T).Name}>: {repo.GetAll().Count} đối tượng");
            foreach (T item in repo.GetAll())
                Console.WriteLine($"     - {item}");
        }
    }
}