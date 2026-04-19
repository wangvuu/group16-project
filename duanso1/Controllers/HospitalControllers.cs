using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using duanso1.Model;
using duanso1.Repositories;

namespace duanso1.Controllers
{
    // ══════════════════════════════════════════════════════════════
    //  DANH MỤC CONTROLLER  — GET /api/danhmuc/...
    // ══════════════════════════════════════════════════════════════
    [ApiController]
    [Route("api/danhmuc")]
    [Authorize]
    public class DanhMucController : ControllerBase
    {
        private readonly HospitalRepository _repo;
        public DanhMucController(HospitalRepository repo) => _repo = repo;

        [HttpGet("quoctich")]
        public async Task<IActionResult> QuocTich()
            => Ok(await _repo.GetQuocTichAsync());

        [HttpGet("dantoc")]
        public async Task<IActionResult> DanToc()
            => Ok(await _repo.GetDanTocAsync());

        [HttpGet("nghenghiep")]
        public async Task<IActionResult> NgheNghiep()
            => Ok(await _repo.GetNgheNghiepAsync());

        [HttpGet("tinhthanh")]
        public async Task<IActionResult> TinhThanh()
            => Ok(await _repo.GetTinhThanhAsync());

        [HttpGet("noidangkythe")]
        public async Task<IActionResult> NoiDangKyThe()
            => Ok(await _repo.GetNoiDangKyTheAsync());

        [HttpGet("dichvu")]
        public async Task<IActionResult> DichVu()
            => Ok(await _repo.GetDanhMucDichVuAsync());

        [HttpGet("thuoc")]
        public async Task<IActionResult> Thuoc()
            => Ok(await _repo.GetDanhMucThuocAsync());
    }

    // ══════════════════════════════════════════════════════════════
    //  BỆNH NHÂN CONTROLLER  — /api/benhnhan
    // ══════════════════════════════════════════════════════════════
    [ApiController]
    [Route("api/benhnhan")]
    [Authorize]
    public class BenhNhanController : ControllerBase
    {
        private readonly HospitalRepository _repo;
        public BenhNhanController(HospitalRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _repo.GetAllBenhNhanAsync());

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword = "")
            => Ok(await _repo.SearchBenhNhanAsync(keyword));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bn = await _repo.GetBenhNhanByIdAsync(id);
            return bn == null ? NotFound(new { message = "Không tìm thấy bệnh nhân" }) : Ok(bn);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BenhNhanRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.HoTen))
                return BadRequest(new { message = "Họ tên không được để trống" });
            var result = await _repo.CreateBenhNhanAsync(req);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BenhNhanRequest req)
        {
            var updated = await _repo.UpdateBenhNhanAsync(id, req);
            return updated != null
                ? Ok(updated)                                              // ✅ trả về object có SoHoSo mới
                : NotFound(new { message = "Không tìm thấy bệnh nhân" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteBenhNhanAsync(id);
            return ok ? Ok(new { message = "Đã xóa" })
                      : NotFound(new { message = "Không tìm thấy bệnh nhân" });
        }
    }

    // ══════════════════════════════════════════════════════════════
    //  BẢO HIỂM Y TẾ CONTROLLER  — /api/baohiem
    // ══════════════════════════════════════════════════════════════
    [ApiController]
    [Route("api/baohiem")]
    [Authorize]
    public class BaoHiemController : ControllerBase
    {
        private readonly HospitalRepository _repo;
        public BaoHiemController(HospitalRepository repo) => _repo = repo;

        [HttpGet("benhnhan/{benhNhanId:int}")]
        public async Task<IActionResult> GetByBenhNhan(int benhNhanId)
        {
            var bh = await _repo.GetBaoHiemByBenhNhanAsync(benhNhanId);
            return bh == null ? NotFound(new { message = "Chưa có thông tin bảo hiểm" }) : Ok(bh);
        }

        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] BaoHiemRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.MaThe))
                return BadRequest(new { message = "Mã thẻ không được để trống" });
            var result = await _repo.UpsertBaoHiemAsync(req);
            return Ok(result);
        }
    }

    // ══════════════════════════════════════════════════════════════
    //  HỒ SƠ KHÁM CONTROLLER  — /api/hosokham
    // ══════════════════════════════════════════════════════════════
    [ApiController]
    [Route("api/hosokham")]
    [Authorize]
    public class HoSoKhamController : ControllerBase
    {
        private readonly HospitalRepository _repo;
        public HoSoKhamController(HospitalRepository repo) => _repo = repo;

        [HttpGet("benhnhan/{benhNhanId:int}")]
        public async Task<IActionResult> GetByBenhNhan(int benhNhanId)
            => Ok(await _repo.GetHoSoByBenhNhanAsync(benhNhanId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaoHoSoRequest body)
        {
            var result = await _repo.CreateHoSoKhamAsync(body.BenhNhanId, body.BaoHiemId);
            return Ok(result);
        }

        [HttpPut("{id:int}/trangthai")]
        public async Task<IActionResult> UpdateTrangThai(int id, [FromBody] UpdateTrangThaiRequest body)
        {
            var ok = await _repo.UpdateTrangThaiAsync(id, body.TrangThai, body.ChanDoan);
            return ok ? Ok(new { message = "Cập nhật thành công" }) : NotFound();
        }

        [HttpPost("ketthuc")]
        public async Task<IActionResult> KetThuc([FromBody] KetThucKhamRequest req)
        {
            var ok = await _repo.KetThucKhamAsync(req);
            return ok ? Ok(new { message = "Kết thúc khám thành công" }) : BadRequest();
        }
    }

    // ══════════════════════════════════════════════════════════════
    //  KÊ THUỐC CONTROLLER  — /api/kethuoc
    // ══════════════════════════════════════════════════════════════
    [ApiController]
    [Route("api/kethuoc")]
    [Authorize]
    public class KeThuocController : ControllerBase
    {
        private readonly HospitalRepository _repo;
        public KeThuocController(HospitalRepository repo) => _repo = repo;

        [HttpGet("{hoSoKhamId:int}")]
        public async Task<IActionResult> Get(int hoSoKhamId)
            => Ok(await _repo.GetKeThuocAsync(hoSoKhamId));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] KeThuocRequest req)
        {
            if (req.ThuocId <= 0 || req.SoLuong <= 0)
                return BadRequest(new { message = "Thông tin thuốc không hợp lệ" });
            var result = await _repo.AddKeThuocAsync(req);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteKeThuocAsync(id);
            return ok ? Ok(new { message = "Đã xóa" }) : NotFound();
        }
    }

    // ══════════════════════════════════════════════════════════════
    //  DỊCH VỤ CONTROLLER  — /api/dichvu
    // ══════════════════════════════════════════════════════════════
    [ApiController]
    [Route("api/dichvu")]
    [Authorize]
    public class DichVuController : ControllerBase
    {
        private readonly HospitalRepository _repo;
        public DichVuController(HospitalRepository repo) => _repo = repo;

        [HttpGet("{hoSoKhamId:int}")]
        public async Task<IActionResult> Get(int hoSoKhamId)
            => Ok(await _repo.GetDichVuAsync(hoSoKhamId));

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] DichVuRequest req)
        {
            if (req.DichVuId <= 0 || req.SoLuong <= 0)
                return BadRequest(new { message = "Thông tin dịch vụ không hợp lệ" });
            var result = await _repo.AddDichVuAsync(req);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _repo.DeleteDichVuAsync(id);
            return ok ? Ok(new { message = "Đã xóa" }) : NotFound();
        }
    }
}