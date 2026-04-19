using Dapper;
using Npgsql;
using duanso1.Model;

namespace duanso1.Repositories
{
    public class HospitalRepository
    {
        private readonly string _conn;
        public HospitalRepository(IConfiguration cfg)
            => _conn = cfg.GetConnectionString("DefaultConnection")!;

        private NpgsqlConnection Open() => new(_conn);

        // ══════════════════════════════════════════════════════════
        //  HELPER — Parse ngày sinh an toàn, hỗ trợ nhiều định dạng
        // ══════════════════════════════════════════════════════════
        private static DateTime? ParseNgaySinh(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;

            // Thử các định dạng phổ biến từ client WinForms / Postman
            string[] formats =
            {
                "dd/MM/yyyy",   // 01/01/1990
                "d/M/yyyy",     // 1/1/1990
                "yyyy-MM-dd",   // 1990-01-01  (ISO)
                "MM/dd/yyyy",   // 01/01/1990 kiểu Mỹ
                "dd-MM-yyyy",   // 01-01-1990
                "yyyy/MM/dd",   // 1990/01/01
            };

            if (DateTime.TryParseExact(input, formats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out var dt))
                return dt;

            // Fallback TryParse tổng quát
            if (DateTime.TryParse(input, out var dt2))
                return dt2;

            return null;
        }

        // ══════════════════════════════════════════════════════════
        //  DANH MỤC
        // ══════════════════════════════════════════════════════════

        public async Task<IEnumerable<DanhMucItem>> GetQuocTichAsync()
        {
            using var db = Open();
            return await db.QueryAsync<DanhMucItem>("SELECT id, ten FROM dm_quoc_tich ORDER BY id");
        }

        public async Task<IEnumerable<DanhMucItem>> GetDanTocAsync()
        {
            using var db = Open();
            return await db.QueryAsync<DanhMucItem>("SELECT id, ten FROM dm_dan_toc ORDER BY id");
        }

        public async Task<IEnumerable<DanhMucItem>> GetNgheNghiepAsync()
        {
            using var db = Open();
            return await db.QueryAsync<DanhMucItem>("SELECT id, ten FROM dm_nghe_nghiep ORDER BY id");
        }

        public async Task<IEnumerable<DanhMucItem>> GetTinhThanhAsync()
        {
            using var db = Open();
            return await db.QueryAsync<DanhMucItem>("SELECT id, ten FROM dm_tinh_thanh ORDER BY id");
        }

        public async Task<IEnumerable<DanhMucItem>> GetNoiDangKyTheAsync()
        {
            using var db = Open();
            return await db.QueryAsync<DanhMucItem>("SELECT id, ten FROM dm_noi_dang_ky_the ORDER BY id");
        }

        public async Task<IEnumerable<DanhMucDichVu>> GetDanhMucDichVuAsync()
        {
            using var db = Open();
            return await db.QueryAsync<DanhMucDichVu>(
                "SELECT id, ten, don_gia AS DonGia, COALESCE(mo_ta,'') AS MoTa FROM dm_dich_vu WHERE is_active=TRUE ORDER BY ten");
        }

        public async Task<IEnumerable<DanhMucThuoc>> GetDanhMucThuocAsync()
        {
            using var db = Open();
            return await db.QueryAsync<DanhMucThuoc>(
                "SELECT id, ten, don_vi AS DonVi, don_gia AS DonGia FROM dm_thuoc WHERE is_active=TRUE ORDER BY ten");
        }

        // ══════════════════════════════════════════════════════════
        //  BỆNH NHÂN
        // ══════════════════════════════════════════════════════════

        public async Task<IEnumerable<BenhNhan>> GetAllBenhNhanAsync()
        {
            using var db = Open();
            const string sql = @"
                SELECT id,
                       ma_benh_nhan              AS MaBenhNhan,
                       so_ho_so                  AS SoHoSo,
                       ho_ten                    AS HoTen,
                       ngay_sinh::timestamp      AS NgaySinh,
                       gioi_tinh                 AS GioiTinh,
                       so_dien_thoai             AS SoDienThoai,
                       nguoi_than                AS NguoiThan,
                       sdt_nguoi_than            AS SdtNguoiThan,
                       cccd,
                       quoc_tich_id              AS QuocTichId,
                       dan_toc_id                AS DanTocId,
                       nghe_nghiep_id            AS NgheNghiepId,
                       tinh_thanh_id             AS TinhThanhId,
                       dia_chi                   AS DiaChi,
                       created_at                AS CreatedAt,
                       updated_at                AS UpdatedAt
                FROM benh_nhan
                ORDER BY created_at DESC";
            return await db.QueryAsync<BenhNhan>(sql);
        }

        public async Task<IEnumerable<BenhNhan>> SearchBenhNhanAsync(string keyword)
        {
            using var db = Open();
            const string sql = @"
                SELECT id,
                       ma_benh_nhan              AS MaBenhNhan,
                       so_ho_so                  AS SoHoSo,
                       ho_ten                    AS HoTen,
                       ngay_sinh::timestamp      AS NgaySinh,
                       gioi_tinh                 AS GioiTinh,
                       so_dien_thoai             AS SoDienThoai,
                       nguoi_than                AS NguoiThan,
                       sdt_nguoi_than            AS SdtNguoiThan,
                       cccd,
                       quoc_tich_id              AS QuocTichId,
                       dan_toc_id                AS DanTocId,
                       nghe_nghiep_id            AS NgheNghiepId,
                       tinh_thanh_id             AS TinhThanhId,
                       dia_chi                   AS DiaChi,
                       created_at                AS CreatedAt,
                       updated_at                AS UpdatedAt
                FROM benh_nhan
                WHERE ho_ten ILIKE @kw
                   OR ma_benh_nhan ILIKE @kw
                   OR cccd ILIKE @kw
                ORDER BY created_at DESC
                LIMIT 50";
            return await db.QueryAsync<BenhNhan>(sql, new { kw = $"%{keyword}%" });
        }

        public async Task<BenhNhan?> GetBenhNhanByIdAsync(int id)
        {
            using var db = Open();
            return await db.QueryFirstOrDefaultAsync<BenhNhan>(@"
                SELECT id,
                       ma_benh_nhan              AS MaBenhNhan,
                       so_ho_so                  AS SoHoSo,
                       ho_ten                    AS HoTen,
                       ngay_sinh::timestamp      AS NgaySinh,
                       gioi_tinh                 AS GioiTinh,
                       so_dien_thoai             AS SoDienThoai,
                       nguoi_than                AS NguoiThan,
                       sdt_nguoi_than            AS SdtNguoiThan,
                       cccd,
                       quoc_tich_id              AS QuocTichId,
                       dan_toc_id                AS DanTocId,
                       nghe_nghiep_id            AS NgheNghiepId,
                       tinh_thanh_id             AS TinhThanhId,
                       dia_chi                   AS DiaChi,
                       created_at                AS CreatedAt,
                       updated_at                AS UpdatedAt
                FROM benh_nhan WHERE id=@id",
                new { id });
        }

        public async Task<BenhNhan> CreateBenhNhanAsync(BenhNhanRequest req)
        {
            using var db = Open();

            // ✅ Tự phát sinh MaBenhNhan và SoHoSo theo năm + số thứ tự
            var count = await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM benh_nhan");
            var stt = (count + 1).ToString("D4");         // "0005"
            var maBenhNhan = $"BN-{DateTime.Now.Year}-{stt}";   // "BN-2026-0005"
            var soHoSo = $"HS-{DateTime.Now.Year}-{stt}";   // "HS-2026-0005"

            // ✅ Parse ngày sinh hỗ trợ nhiều định dạng (dd/MM/yyyy, yyyy-MM-dd, ...)
            var ngaySinh = ParseNgaySinh(req.NgaySinh);

            const string sql = @"
                INSERT INTO benh_nhan
                    (ma_benh_nhan, so_ho_so, ho_ten, ngay_sinh, gioi_tinh,
                     so_dien_thoai, nguoi_than, sdt_nguoi_than, cccd,
                     quoc_tich_id, dan_toc_id, nghe_nghiep_id, tinh_thanh_id, dia_chi)
                VALUES
                    (@maBenhNhan, @soHoSo, @HoTen, @ngaySinh, @GioiTinh,
                     @SoDienThoai, @NguoiThan, @SdtNguoiThan, @Cccd,
                     @QuocTichId, @DanTocId, @NgheNghiepId, @TinhThanhId, @DiaChi)
                RETURNING id,
                          ma_benh_nhan              AS MaBenhNhan,
                          so_ho_so                  AS SoHoSo,
                          ho_ten                    AS HoTen,
                          ngay_sinh::timestamp      AS NgaySinh,
                          gioi_tinh                 AS GioiTinh,
                          so_dien_thoai             AS SoDienThoai,
                          nguoi_than                AS NguoiThan,
                          sdt_nguoi_than            AS SdtNguoiThan,
                          cccd,
                          quoc_tich_id              AS QuocTichId,
                          dan_toc_id                AS DanTocId,
                          nghe_nghiep_id            AS NgheNghiepId,
                          tinh_thanh_id             AS TinhThanhId,
                          dia_chi                   AS DiaChi,
                          created_at                AS CreatedAt,
                          updated_at                AS UpdatedAt";

            return await db.QuerySingleAsync<BenhNhan>(sql, new
            {
                maBenhNhan,
                soHoSo,
                req.HoTen,
                ngaySinh,
                req.GioiTinh,
                req.SoDienThoai,
                req.NguoiThan,
                req.SdtNguoiThan,
                req.Cccd,
                req.QuocTichId,
                req.DanTocId,
                req.NgheNghiepId,
                req.TinhThanhId,
                req.DiaChi
            });
        }

        public async Task<BenhNhan?> UpdateBenhNhanAsync(int id, BenhNhanRequest req)
        {
            using var db = Open();

            var ngaySinh = ParseNgaySinh(req.NgaySinh);

            // ✅ Tạo SoHoSo mới tăng dần dựa theo MAX hiện tại trong DB
            var maxSoHoSo = await db.ExecuteScalarAsync<string?>(
                "SELECT so_ho_so FROM benh_nhan ORDER BY id DESC LIMIT 1");

            // Lấy số thứ tự lớn nhất từ bảng ho_so_kham để tạo SoHoSo mới
            var countHoSo = await db.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM ho_so_kham WHERE benh_nhan_id = @id", new { id });

            var soHoSoMoi = $"HS-{DateTime.Now.Year}-{id:D4}-{(countHoSo + 1):D2}";
            // Ví dụ: HS-2026-0015-02 (bệnh nhân 15, hồ sơ lần 2)

            // ── 1. Cập nhật thông tin bệnh nhân + ghi SoHoSo mới ──
            var rows = await db.ExecuteAsync(@"
        UPDATE benh_nhan SET
            so_ho_so        = @soHoSoMoi,
            ho_ten          = @HoTen,
            ngay_sinh       = @ngaySinh,
            gioi_tinh       = @GioiTinh,
            so_dien_thoai   = @SoDienThoai,
            nguoi_than      = @NguoiThan,
            sdt_nguoi_than  = @SdtNguoiThan,
            cccd            = @Cccd,
            quoc_tich_id    = @QuocTichId,
            dan_toc_id      = @DanTocId,
            nghe_nghiep_id  = @NgheNghiepId,
            tinh_thanh_id   = @TinhThanhId,
            dia_chi         = @DiaChi,
            updated_at      = NOW()
        WHERE id = @id",
                new
                {
                    id,
                    soHoSoMoi,
                    req.HoTen,
                    ngaySinh,
                    req.GioiTinh,
                    req.SoDienThoai,
                    req.NguoiThan,
                    req.SdtNguoiThan,
                    req.Cccd,
                    req.QuocTichId,
                    req.DanTocId,
                    req.NgheNghiepId,
                    req.TinhThanhId,
                    req.DiaChi
                });

            if (rows == 0) return null;

            // ── 2. INSERT hồ sơ khám mới vào ho_so_kham ──────────────────
            // Lưu lịch sử: mỗi lần sửa = 1 hồ sơ mới với SoHoSo mới
            await db.ExecuteAsync(@"
        INSERT INTO ho_so_kham
            (benh_nhan_id, trang_thai, ngay_vao, chan_doan, ket_luan)
        VALUES
            (@benhNhanId, 'Cập nhật hồ sơ', @ngayVao, @ghiChu, '')",
                new
                {
                    benhNhanId = id,
                    ngayVao = DateTime.Today,
                    ghiChu = $"Cập nhật thông tin - Số hồ sơ: {soHoSoMoi}"
                });

            // ── 3. Trả về bệnh nhân đã cập nhật ──────────────────────────
            return await GetBenhNhanByIdAsync(id);
        }

        public async Task<bool> DeleteBenhNhanAsync(int id)
        {
            using var db = Open();
            var rows = await db.ExecuteAsync("DELETE FROM benh_nhan WHERE id=@id", new { id });
            return rows > 0;
        }

        // ══════════════════════════════════════════════════════════
        //  BẢO HIỂM Y TẾ
        // ══════════════════════════════════════════════════════════

        public async Task<BaoHiemYTe?> GetBaoHiemByBenhNhanAsync(int benhNhanId)
        {
            using var db = Open();
            return await db.QueryFirstOrDefaultAsync<BaoHiemYTe>(@"
                SELECT b.id,
                       b.benh_nhan_id            AS BenhNhanId,
                       b.ma_the                  AS MaThe,
                       b.han_su_dung::timestamp  AS HanSuDung,
                       b.noi_dang_ky_id          AS NoiDangKyId,
                       d.ten                     AS NoiDangKyTen,
                       b.dia_chi_dang_ky         AS DiaChiDangKy,
                       b.muc_huong               AS MucHuong,
                       b.is_active               AS IsActive
                FROM bao_hiem_y_te b
                LEFT JOIN dm_noi_dang_ky_the d ON d.id = b.noi_dang_ky_id
                WHERE b.benh_nhan_id = @benhNhanId AND b.is_active = TRUE
                ORDER BY b.id DESC
                LIMIT 1",
                new { benhNhanId });
        }

        public async Task<BaoHiemYTe> UpsertBaoHiemAsync(BaoHiemRequest req)
        {
            using var db = Open();

            // ✅ Dùng string + ::date trong SQL, tránh hoàn toàn DateOnly/DateTime
            string? hanStr = null;
            if (!string.IsNullOrWhiteSpace(req.HanSuDung))
            {
                if (DateTime.TryParseExact(req.HanSuDung, "MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var dt))
                    hanStr = $"{dt.Year}-{dt.Month:D2}-01";
                else if (DateTime.TryParse(req.HanSuDung, out var dt2))
                    hanStr = dt2.ToString("yyyy-MM-dd");
            }

            await db.ExecuteAsync(
                "UPDATE bao_hiem_y_te SET is_active=FALSE WHERE benh_nhan_id=@id",
                new { id = req.BenhNhanId });

            var newId = await db.ExecuteScalarAsync<int>(@"
        INSERT INTO bao_hiem_y_te
            (benh_nhan_id, ma_the, han_su_dung, noi_dang_ky_id, dia_chi_dang_ky, muc_huong)
        VALUES
            (@BenhNhanId, @MaThe, @hanStr::date, @NoiDangKyId, @DiaChiDangKy, @MucHuong)
        RETURNING id",
                new { req.BenhNhanId, req.MaThe, hanStr, req.NoiDangKyId, req.DiaChiDangKy, req.MucHuong });

            return await db.QuerySingleAsync<BaoHiemYTe>(@"
        SELECT b.id,
               b.benh_nhan_id            AS BenhNhanId,
               b.ma_the                  AS MaThe,
               b.han_su_dung::timestamp  AS HanSuDung,
               b.noi_dang_ky_id          AS NoiDangKyId,
               COALESCE(d.ten,'')        AS NoiDangKyTen,
               b.dia_chi_dang_ky         AS DiaChiDangKy,
               b.muc_huong               AS MucHuong,
               b.is_active               AS IsActive
        FROM bao_hiem_y_te b
        LEFT JOIN dm_noi_dang_ky_the d ON d.id = b.noi_dang_ky_id
        WHERE b.id = @newId",
                new { newId });
        }

        // ══════════════════════════════════════════════════════════
        //  HỒ SƠ KHÁM
        // ══════════════════════════════════════════════════════════

        public async Task<IEnumerable<HoSoKham>> GetHoSoByBenhNhanAsync(int benhNhanId)
        {
            using var db = Open();
            return await db.QueryAsync<HoSoKham>(@"
                SELECT h.id,
                       h.benh_nhan_id                              AS BenhNhanId,
                       b.ho_ten                                    AS HoTenBenhNhan,
                       h.bao_hiem_id                               AS BaoHiemId,
                       h.ngay_vao::timestamp                       AS NgayVao,
                       h.ngay_ra::timestamp                        AS NgayRa,
                       h.chan_doan                                  AS ChanDoan,
                       h.ket_luan                                   AS KetLuan,
                       h.hinh_thuc_ket_thuc                        AS HinhThucKetThuc,
                       h.trang_thai                                 AS TrangThai,
                       h.tong_tien_thuoc                           AS TongTienThuoc,
                       h.tong_tien_dich_vu                         AS TongTienDichVu
                FROM ho_so_kham h
                JOIN benh_nhan b ON b.id = h.benh_nhan_id
                WHERE h.benh_nhan_id = @benhNhanId
                ORDER BY h.ngay_vao DESC",
                new { benhNhanId });
        }

        public async Task<HoSoKham> CreateHoSoKhamAsync(int benhNhanId, int? baoHiemId)
        {
            using var db = Open();

            // ✅ Dùng DateTime thay vì DateOnly
            var ngayVao = DateTime.Today;

            var newId = await db.ExecuteScalarAsync<int>(@"
        INSERT INTO ho_so_kham (benh_nhan_id, bao_hiem_id, trang_thai, ngay_vao)
        VALUES (@benhNhanId, @baoHiemId, 'Chờ khám', @ngayVao)
        RETURNING id",
                new { benhNhanId, baoHiemId, ngayVao });

            return await db.QuerySingleAsync<HoSoKham>(@"
                SELECT h.id,
                       h.benh_nhan_id                              AS BenhNhanId,
                       b.ho_ten                                    AS HoTenBenhNhan,
                       h.bao_hiem_id                               AS BaoHiemId,
                       h.ngay_vao::timestamp                       AS NgayVao,
                       h.ngay_ra::timestamp                        AS NgayRa,
                       COALESCE(h.chan_doan,           '')         AS ChanDoan,
                       COALESCE(h.ket_luan,            '')         AS KetLuan,
                       COALESCE(h.hinh_thuc_ket_thuc, 'Ra viện') AS HinhThucKetThuc,
                       h.trang_thai                                 AS TrangThai,
                       COALESCE(h.tong_tien_thuoc,    0)          AS TongTienThuoc,
                       COALESCE(h.tong_tien_dich_vu,  0)          AS TongTienDichVu
                FROM ho_so_kham h
                JOIN benh_nhan b ON b.id = h.benh_nhan_id
                WHERE h.id = @newId",
                new { newId });
        }

        public async Task<bool> UpdateTrangThaiAsync(int hoSoId, string trangThai, string chanDoan)
        {
            using var db = Open();
            var rows = await db.ExecuteAsync(@"
                UPDATE ho_so_kham
                SET trang_thai = @trangThai,
                    chan_doan  = @chanDoan,
                    updated_at = NOW()
                WHERE id = @hoSoId",
                new { hoSoId, trangThai, chanDoan });
            return rows > 0;
        }

        public async Task<bool> KetThucKhamAsync(KetThucKhamRequest req)
        {
            using var db = Open();
            DateTime? ngayRa = DateTime.TryParse(req.NgayRa, out var dt) ? dt : DateTime.Today;

            var tienThuoc = await db.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(thanh_tien),0) FROM ke_thuoc WHERE ho_so_kham_id=@id",
                new { id = req.HoSoKhamId });
            var tienDV = await db.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(thanh_tien),0) FROM su_dung_dich_vu WHERE ho_so_kham_id=@id",
                new { id = req.HoSoKhamId });

            var rows = await db.ExecuteAsync(@"
                UPDATE ho_so_kham SET
                    ket_luan            = @KetLuan,
                    hinh_thuc_ket_thuc  = @HinhThucKetThuc,
                    ngay_ra             = @ngayRa,
                    trang_thai          = 'Đã khám',
                    tong_tien_thuoc     = @tienThuoc,
                    tong_tien_dich_vu   = @tienDV,
                    updated_at          = NOW()
                WHERE id = @HoSoKhamId",
                new { req.KetLuan, req.HinhThucKetThuc, ngayRa, tienThuoc, tienDV, req.HoSoKhamId });
            return rows > 0;
        }

        // ══════════════════════════════════════════════════════════
        //  KÊ THUỐC
        // ══════════════════════════════════════════════════════════

        public async Task<IEnumerable<KeThuocItem>> GetKeThuocAsync(int hoSoKhamId)
        {
            using var db = Open();
            return await db.QueryAsync<KeThuocItem>(@"
                SELECT k.id,
                       k.ho_so_kham_id AS HoSoKhamId,
                       k.thuoc_id      AS ThuocId,
                       t.ten           AS TenThuoc,
                       k.so_luong      AS SoLuong,
                       k.lieu_dung     AS LieuDung,
                       k.don_gia       AS DonGia,
                       k.thanh_tien    AS ThanhTien
                FROM ke_thuoc k
                JOIN dm_thuoc t ON t.id = k.thuoc_id
                WHERE k.ho_so_kham_id = @hoSoKhamId",
                new { hoSoKhamId });
        }

        public async Task<KeThuocItem> AddKeThuocAsync(KeThuocRequest req)
        {
            using var db = Open();
            var thanhTien = req.DonGia * req.SoLuong;
            return await db.QuerySingleAsync<KeThuocItem>(@"
                INSERT INTO ke_thuoc
                    (ho_so_kham_id, thuoc_id, so_luong, lieu_dung, don_gia, thanh_tien)
                VALUES
                    (@HoSoKhamId, @ThuocId, @SoLuong, @LieuDung, @DonGia, @thanhTien)
                RETURNING id,
                          ho_so_kham_id AS HoSoKhamId,
                          thuoc_id      AS ThuocId,
                          so_luong      AS SoLuong,
                          lieu_dung     AS LieuDung,
                          don_gia       AS DonGia,
                          thanh_tien    AS ThanhTien",
                new { req.HoSoKhamId, req.ThuocId, req.SoLuong, req.LieuDung, req.DonGia, thanhTien });
        }

        public async Task<bool> DeleteKeThuocAsync(int id)
        {
            using var db = Open();
            return await db.ExecuteAsync("DELETE FROM ke_thuoc WHERE id=@id", new { id }) > 0;
        }

        // ══════════════════════════════════════════════════════════
        //  DỊCH VỤ
        // ══════════════════════════════════════════════════════════

        public async Task<IEnumerable<DichVuItem>> GetDichVuAsync(int hoSoKhamId)
        {
            using var db = Open();
            return await db.QueryAsync<DichVuItem>(@"
                SELECT s.id,
                       s.ho_so_kham_id AS HoSoKhamId,
                       s.dich_vu_id    AS DichVuId,
                       d.ten           AS TenDichVu,
                       s.so_luong      AS SoLuong,
                       s.don_gia       AS DonGia,
                       s.thanh_tien    AS ThanhTien,
                       s.trang_thai    AS TrangThai
                FROM su_dung_dich_vu s
                JOIN dm_dich_vu d ON d.id = s.dich_vu_id
                WHERE s.ho_so_kham_id = @hoSoKhamId",
                new { hoSoKhamId });
        }

        public async Task<DichVuItem> AddDichVuAsync(DichVuRequest req)
        {
            using var db = Open();
            var thanhTien = req.DonGia * req.SoLuong;
            return await db.QuerySingleAsync<DichVuItem>(@"
                INSERT INTO su_dung_dich_vu
                    (ho_so_kham_id, dich_vu_id, so_luong, don_gia, thanh_tien, ghi_chu)
                VALUES
                    (@HoSoKhamId, @DichVuId, @SoLuong, @DonGia, @thanhTien, @GhiChu)
                RETURNING id,
                          ho_so_kham_id AS HoSoKhamId,
                          dich_vu_id    AS DichVuId,
                          so_luong      AS SoLuong,
                          don_gia       AS DonGia,
                          thanh_tien    AS ThanhTien,
                          trang_thai    AS TrangThai",
                new { req.HoSoKhamId, req.DichVuId, req.SoLuong, req.DonGia, thanhTien, req.GhiChu });
        }

        public async Task<bool> DeleteDichVuAsync(int id)
        {
            using var db = Open();
            return await db.ExecuteAsync("DELETE FROM su_dung_dich_vu WHERE id=@id", new { id }) > 0;
        }
    }
}