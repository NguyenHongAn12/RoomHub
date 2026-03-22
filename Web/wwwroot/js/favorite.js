document.addEventListener("DOMContentLoaded", function () {
    const favoriteButtons = document.querySelectorAll('.btn-toggle-favorite');

    favoriteButtons.forEach(button => {
        button.addEventListener('click', async function (e) {
            e.preventDefault();
            e.stopPropagation(); // Ngăn sự kiện click lan ra thẻ Card

            const roomId = this.getAttribute('data-room-id');
            const icon = this.querySelector('span.material-symbols-outlined');

            // Vô hiệu hóa nút tạm thời để tránh user click liên tục (spam)
            this.style.pointerEvents = 'none';

            try {
                // SỬA LỖI 404 Ở ĐÂY: Dùng '/Favorite/ToggleFavorite' (Không có chữ 's')
                const response = await fetch(`/Favorite/ToggleFavorite?roomId=${roomId}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    }
                    // Không cần body nữa vì đã truyền qua query string
                });

                // Kiểm tra nếu API trả về các lỗi không phải 200 OK (ví dụ: 401, 404, 500)
                if (!response.ok) {
                    if (response.status === 401) {
                        alert('Bạn cần đăng nhập để sử dụng tính năng này!');
                        // Đá người dùng về trang đăng nhập và lưu lại trang hiện tại
                        window.location.href = "/Auth/Login?returnUrl=" + encodeURIComponent(window.location.pathname);
                        return;
                    }
                    throw new Error(`Server báo lỗi: ${response.status}`);
                }

                // Chuyển đổi dữ liệu JSON an toàn
                const data = await response.json();

                if (data.success) {
                    if (data.isFavorite) {
                        // Trạng thái: Đã lưu -> Chuyển tim đỏ
                        icon.style.fontVariationSettings = "'FILL' 1";
                        icon.style.color = "#ef4444";
                    } else {
                        // Trạng thái: Đã bỏ lưu -> Chuyển tim xám rỗng
                        icon.style.fontVariationSettings = "'FILL' 0";
                        icon.style.color = "#94a3b8";

                        // Logic thêm: Nếu đang đứng ở trang "Danh sách yêu thích", ẩn luôn bài đăng đó đi cho đẹp
                        if (window.location.pathname.toLowerCase().includes('/favorite')) {
                            const card = this.closest('.group');
                            if (card) {
                                card.style.transition = "opacity 0.3s ease";
                                card.style.opacity = "0";
                                setTimeout(() => card.remove(), 300); // Đợi hiệu ứng mờ dần rồi xóa hẳn
                            }
                        }
                    }
                } else {
                    alert('Lỗi: ' + data.message);
                }
            } catch (error) {
                console.error("Lỗi khi gọi API yêu thích:", error);
                // Bắt lỗi an toàn, không làm treo các tính năng khác của website
            } finally {
                // Kết thúc quá trình, mở lại nút cho phép click tiếp
                this.style.pointerEvents = 'auto';
            }
        });
    });
});