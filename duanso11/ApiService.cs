using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace duanso11
{
    public static class ApiService
    {
        private static readonly HttpClient _http = new()
        {
            BaseAddress = new Uri("http://localhost:5226/"),
            Timeout = TimeSpan.FromSeconds(10)
        };

        private static readonly JsonSerializerOptions _json = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // ── AUTH HEADER ───────────────────────────────────────────────

        /// <summary>Gắn JWT token vào mọi request</summary>
        private static void SetAuthHeader()
        {
            _http.DefaultRequestHeaders.Authorization =
                string.IsNullOrEmpty(AppSession.Token)
                    ? null
                    : new AuthenticationHeaderValue("Bearer", AppSession.Token);
        }

        // ── ĐĂNG NHẬP ────────────────────────────────────────────────

        public static async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/auth/login", new
                {
                    username,
                    password
                });

                var json = await res.Content.ReadAsStringAsync();

                if (res.StatusCode == HttpStatusCode.TooManyRequests)
                    throw new Exception("TOO_MANY_REQUESTS");

                if (!res.IsSuccessStatusCode)
                    throw new Exception("INVALID_CREDENTIALS");

                return JsonSerializer.Deserialize<LoginResponse>(json, _json);
            }
            catch (Exception ex) when (ex.Message is "TOO_MANY_REQUESTS" or "INVALID_CREDENTIALS")
            {
                throw;
            }
            catch
            {
                throw new Exception("CONNECTION_ERROR");
            }
        }

        /// <summary>Làm mới access token bằng refresh token</summary>
        public static async Task<bool> RefreshTokenAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(AppSession.RefreshToken)) return false;

                var res = await _http.PostAsJsonAsync("api/auth/refresh", new
                {
                    refreshToken = AppSession.RefreshToken
                });

                if (!res.IsSuccessStatusCode) return false;

                var json = await res.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TokenResponse>(json, _json);
                if (result is null) return false;

                AppSession.UpdateTokens(result.Token, result.RefreshToken);
                return true;
            }
            catch { return false; }
        }

        /// <summary>Đăng xuất — thu hồi refresh token</summary>
        public static async Task LogoutAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(AppSession.RefreshToken))
                    await _http.PostAsJsonAsync("api/auth/logout", new
                    {
                        refreshToken = AppSession.RefreshToken
                    });
            }
            catch { /* bỏ qua lỗi khi logout */ }
            finally
            {
                AppSession.Clear();
            }
        }

        // ── ĐĂNG KÝ ──────────────────────────────────────────────────

        public static async Task<(bool Success, string Message)> RegisterAsync(
            string username, string email, string password, string role)
        {
            try
            {
                var res = await _http.PostAsJsonAsync("api/auth/register", new
                {
                    username,
                    email,
                    password,
                    role
                });

                var json = await res.Content.ReadAsStringAsync();
                var err = JsonSerializer.Deserialize<ErrorResponse>(json, _json);

                return res.IsSuccessStatusCode
                    ? (true, "Đăng ký thành công!")
                    : (false, err?.Message ?? "Đăng ký thất bại!");
            }
            catch
            {
                return (false, "Không kết nối được server!");
            }
        }

        // ── DANH MỤC ─────────────────────────────────────────────────

        public static async Task<List<DanhMucItem>> GetQuocTichAsync()
            => await GetListAsync<DanhMucItem>("api/danhmuc/quoctich");

        public static async Task<List<DanhMucItem>> GetDanTocAsync()
            => await GetListAsync<DanhMucItem>("api/danhmuc/dantoc");

        public static async Task<List<DanhMucItem>> GetNgheNghiepAsync()
            => await GetListAsync<DanhMucItem>("api/danhmuc/nghenghiep");

        public static async Task<List<DanhMucItem>> GetTinhThanhAsync()
            => await GetListAsync<DanhMucItem>("api/danhmuc/tinhthanh");

        public static async Task<List<DanhMucItem>> GetNoiDangKyTheAsync()
            => await GetListAsync<DanhMucItem>("api/danhmuc/noidangkythe");

        public static async Task<List<DanhMucDichVu>> GetDanhMucDichVuAsync()
            => await GetListAsync<DanhMucDichVu>("api/danhmuc/dichvu");

        public static async Task<List<DanhMucThuoc>> GetDanhMucThuocAsync()
            => await GetListAsync<DanhMucThuoc>("api/danhmuc/thuoc");

        // ── BỆNH NHÂN ────────────────────────────────────────────────

        public static async Task<List<BenhNhan>> SearchBenhNhanAsync(string keyword)
            => await GetListAsync<BenhNhan>(
                $"api/benhnhan/search?keyword={Uri.EscapeDataString(keyword)}");

        public static async Task<BenhNhan?> GetBenhNhanAsync(int id)
            => await GetAsync<BenhNhan>($"api/benhnhan/{id}");

        public static async Task<BenhNhan?> CreateBenhNhanAsync(BenhNhanRequest req)
            => await PostAsync<BenhNhan>("api/benhnhan", req);

        public static async Task<BenhNhan?> UpdateBenhNhanAsync(int id, BenhNhanRequest req)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.PutAsJsonAsync($"api/benhnhan/{id}", req);
                if (res.StatusCode == HttpStatusCode.Unauthorized && await RefreshTokenAsync())
                {
                    SetAuthHeader();
                    res = await _http.PutAsJsonAsync($"api/benhnhan/{id}", req);
                }
                if (!res.IsSuccessStatusCode) return null;
                var json = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BenhNhan>(json, _json);
            }
            catch { return null; }
        }

        public static async Task<bool> DeleteBenhNhanAsync(int id)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.DeleteAsync($"api/benhnhan/{id}");
                if (res.StatusCode == HttpStatusCode.Unauthorized && await RefreshTokenAsync())
                {
                    SetAuthHeader();
                    res = await _http.DeleteAsync($"api/benhnhan/{id}");
                }
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ── BẢO HIỂM Y TẾ ────────────────────────────────────────────

        public static async Task<BaoHiemYTe?> GetBaoHiemAsync(int benhNhanId)
            => await GetAsync<BaoHiemYTe>($"api/baohiem/benhnhan/{benhNhanId}");

        public static async Task<BaoHiemYTe?> SaveBaoHiemAsync(BaoHiemRequest req)
            => await PostAsync<BaoHiemYTe>("api/baohiem", req);

        // ── HỒ SƠ KHÁM ───────────────────────────────────────────────

        public static async Task<List<HoSoKham>> GetHoSoKhamAsync(int benhNhanId)
            => await GetListAsync<HoSoKham>($"api/hosokham/benhnhan/{benhNhanId}");

        public static async Task<HoSoKham?> CreateHoSoKhamAsync(int benhNhanId, int? baoHiemId)
        {
            var res = await PostRawAsync("api/hosokham", new { benhNhanId, baoHiemId });
            if (res is null) return null;
            if (!res.IsSuccessStatusCode)
            {
                var errBody = await res.Content.ReadAsStringAsync();
                throw new Exception($"API lỗi {(int)res.StatusCode}: {errBody}");
            }
            var json = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<HoSoKham>(json, _json);
        }

        public static async Task<bool> KetThucKhamAsync(KetThucKhamRequest req)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.PostAsJsonAsync("api/hosokham/ketthuc", req);
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ── KÊ THUỐC ─────────────────────────────────────────────────

        public static async Task<List<KeThuocItem>> GetKeThuocAsync(int hoSoKhamId)
            => await GetListAsync<KeThuocItem>($"api/kethuoc/{hoSoKhamId}");

        public static async Task<KeThuocItem?> AddKeThuocAsync(KeThuocRequest req)
            => await PostAsync<KeThuocItem>("api/kethuoc", req);

        public static async Task<bool> DeleteKeThuocAsync(int id)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.DeleteAsync($"api/kethuoc/{id}");
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ── DỊCH VỤ ──────────────────────────────────────────────────

        public static async Task<List<DichVuItem>> GetDichVuAsync(int hoSoKhamId)
            => await GetListAsync<DichVuItem>($"api/dichvu/{hoSoKhamId}");

        public static async Task<DichVuItem?> AddDichVuAsync(DichVuRequest req)
            => await PostAsync<DichVuItem>("api/dichvu", req);

        public static async Task<bool> DeleteDichVuAsync(int id)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.DeleteAsync($"api/dichvu/{id}");
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ── NGƯỜI DÙNG ────────────────────────────────────────────────

        public static async Task<List<UserInfo>> GetAllUsersAsync()
            => await GetListAsync<UserInfo>("api/users");

        public static async Task<UserInfo?> GetUserByIdAsync(int id)
            => await GetAsync<UserInfo>($"api/users/{id}");

        public static async Task<bool> CreateUserAsync(UserSaveRequest req)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.PostAsJsonAsync("api/users", req);
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public static async Task<bool> UpdateUserAsync(int id, UserSaveRequest req)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.PutAsJsonAsync($"api/users/{id}", req);
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public static async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.DeleteAsync($"api/users/{id}");
                return res.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ── PRIVATE HELPERS ───────────────────────────────────────────

        private static async Task<List<T>> GetListAsync<T>(string url)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.GetAsync(url);

                // Token hết hạn → tự động làm mới và thử lại
                if (res.StatusCode == HttpStatusCode.Unauthorized && await RefreshTokenAsync())
                {
                    SetAuthHeader();
                    res = await _http.GetAsync(url);
                }

                if (!res.IsSuccessStatusCode) return [];
                var json = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<T>>(json, _json) ?? [];
            }
            catch { return []; }
        }

        private static async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.GetAsync(url);

                if (res.StatusCode == HttpStatusCode.Unauthorized && await RefreshTokenAsync())
                {
                    SetAuthHeader();
                    res = await _http.GetAsync(url);
                }

                if (!res.IsSuccessStatusCode) return default;
                var json = await res.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, _json);
            }
            catch { return default; }
        }

        private static async Task<T?> PostAsync<T>(string url, object body)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.PostAsJsonAsync(url, body);

                if (res.StatusCode == HttpStatusCode.Unauthorized && await RefreshTokenAsync())
                {
                    SetAuthHeader();
                    res = await _http.PostAsJsonAsync(url, body);
                }

                var json = await res.Content.ReadAsStringAsync();
                Console.WriteLine($"[POST] {url} → {(int)res.StatusCode}: {json}");

                // ✅ THÊM TẠM: hiện lỗi ra MessageBox để debug
                if (!res.IsSuccessStatusCode)
                {
                    MessageBox.Show($"API lỗi {(int)res.StatusCode}:\n{json}",
                        "Debug API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return default;
                }

                return JsonSerializer.Deserialize<T>(json, _json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[POST EXCEPTION] {url} → {ex.Message}");
                // ✅ THÊM TẠM:
                MessageBox.Show($"Exception:\n{ex.Message}", "Debug Exception",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return default;
            }
        }

        /// <summary>POST trả về HttpResponseMessage thô (dùng khi cần đọc status code)</summary>
        private static async Task<HttpResponseMessage?> PostRawAsync(string url, object body)
        {
            try
            {
                SetAuthHeader();
                var res = await _http.PostAsJsonAsync(url, body);

                if (res.StatusCode == HttpStatusCode.Unauthorized && await RefreshTokenAsync())
                {
                    SetAuthHeader();
                    res = await _http.PostAsJsonAsync(url, body);
                }

                return res;
            }
            catch { return null; }
        }
    }

    // ── Response / Request models ─────────────────────────────────────

    public class LoginResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }

    public class TokenResponse
    {
        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = "";
    }
}