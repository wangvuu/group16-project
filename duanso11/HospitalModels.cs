namespace duanso11
{
    // ── DANH MỤC ────────────────────────────────────────────────
    public class DanhMucItem
    {
        public int Id { get; set; }
        public string Ten { get; set; } = "";
    }

    public class DanhMucDichVu
    {
        public int Id { get; set; }
        public string Ten { get; set; } = "";
        public decimal DonGia { get; set; }
        public string MoTa { get; set; } = "";
    }

    public class DanhMucThuoc
    {
        public int Id { get; set; }
        public string Ten { get; set; } = "";
        public string DonVi { get; set; } = "viên";
        public decimal DonGia { get; set; }
    }

    // ── BỆNH NHÂN ────────────────────────────────────────────────
    public class BenhNhan
    {
        public int Id { get; set; }
        public string MaBenhNhan { get; set; } = "";
        public string SoHoSo { get; set; } = "";
        public string HoTen { get; set; } = "";
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; } = "";
        public string SoDienThoai { get; set; } = "";
        public string NguoiThan { get; set; } = "";
        public string SdtNguoiThan { get; set; } = "";
        public string Cccd { get; set; } = "";
        public int? QuocTichId { get; set; }
        public int? DanTocId { get; set; }
        public int? NgheNghiepId { get; set; }
        public int? TinhThanhId { get; set; }
        public string DiaChi { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class BenhNhanRequest
    {
        public string MaBenhNhan { get; set; } = "";
        public string SoHoSo { get; set; } = "";
        public string HoTen { get; set; } = "";
        public string NgaySinh { get; set; } = "";
        public string GioiTinh { get; set; } = "";
        public string SoDienThoai { get; set; } = "";
        public string NguoiThan { get; set; } = "";
        public string SdtNguoiThan { get; set; } = "";
        public string Cccd { get; set; } = "";
        public int? QuocTichId { get; set; }
        public int? DanTocId { get; set; }
        public int? NgheNghiepId { get; set; }
        public int? TinhThanhId { get; set; }
        public string DiaChi { get; set; } = "";
    }

    // ── BẢO HIỂM Y TẾ ────────────────────────────────────────────
    public class BaoHiemYTe
    {
        public int Id { get; set; }
        public int BenhNhanId { get; set; }
        public string MaThe { get; set; } = "";
        public DateTime? HanSuDung { get; set; }
        public int? NoiDangKyId { get; set; }
        public string NoiDangKyTen { get; set; } = "";
        public string DiaChiDangKy { get; set; } = "";
        public int MucHuong { get; set; } = 80;
        public bool IsActive { get; set; } = true;
    }

    public class BaoHiemRequest
    {
        public int BenhNhanId { get; set; }
        public string MaThe { get; set; } = "";
        public string HanSuDung { get; set; } = "";
        public int? NoiDangKyId { get; set; }
        public string DiaChiDangKy { get; set; } = "";
        public int MucHuong { get; set; } = 80;
    }

    // ── HỒ SƠ KHÁM ───────────────────────────────────────────────
    // ── HỒ SƠ KHÁM ───────────────────────────────────────────────
    public class HoSoKham
    {
        public int Id { get; set; }
        public int BenhNhanId { get; set; }
        public string HoTenBenhNhan { get; set; } = "";
        public int? BaoHiemId { get; set; }
        public string NgayVao { get; set; } = "";   // ✅ Dùng string để tránh lỗi DateOnly/DateTime
        public string? NgayRa { get; set; }          // ✅ Dùng string
        public string ChanDoan { get; set; } = "";
        public string KetLuan { get; set; } = "";
        public string HinhThucKetThuc { get; set; } = "Ra viện";
        public string TrangThai { get; set; } = "Chờ khám";
        public decimal TongTienThuoc { get; set; }
        public decimal TongTienDichVu { get; set; }
        public decimal TongCong => TongTienThuoc + TongTienDichVu;
    }

    // ── KÊ THUỐC ─────────────────────────────────────────────────
    public class KeThuocItem
    {
        public int Id { get; set; }
        public int HoSoKhamId { get; set; }
        public int ThuocId { get; set; }
        public string TenThuoc { get; set; } = "";
        public int SoLuong { get; set; }
        public string LieuDung { get; set; } = "";
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }

    public class KeThuocRequest
    {
        public int HoSoKhamId { get; set; }
        public int ThuocId { get; set; }
        public int SoLuong { get; set; }
        public string LieuDung { get; set; } = "";
        public decimal DonGia { get; set; }
    }

    // ── DỊCH VỤ ──────────────────────────────────────────────────
    public class DichVuItem
    {
        public int Id { get; set; }
        public int HoSoKhamId { get; set; }
        public int DichVuId { get; set; }
        public string TenDichVu { get; set; } = "";
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string TrangThai { get; set; } = "Chờ xử lý";
    }

    public class DichVuRequest
    {
        public int HoSoKhamId { get; set; }
        public int DichVuId { get; set; }
        public int SoLuong { get; set; } = 1;
        public decimal DonGia { get; set; }
        public string GhiChu { get; set; } = "";
    }

    // ── KẾT THÚC KHÁM ────────────────────────────────────────────
    public class KetThucKhamRequest
    {
        public int HoSoKhamId { get; set; }
        public string KetLuan { get; set; } = "";
        public string HinhThucKetThuc { get; set; } = "Ra viện";
        public string NgayRa { get; set; } = "";
    }
    public class UserSaveRequest
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "user";
        public string Password { get; set; } = "";      // mật khẩu mới
        public string OldPassword { get; set; } = "";   // mật khẩu cũ (để xác thực)
    }
}