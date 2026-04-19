using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using duanso1.Repositories;
using duanso1.Model;

namespace duanso1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserRepository _userRepo;
        private readonly IConfiguration _config;

        // Rate-limit: theo IP
        private static readonly Dictionary<string, (int Count, DateTime LastAttempt)>
            _loginAttempts = new();

        public AuthController(UserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config   = config;
        }

        // ── ĐĂNG NHẬP → trả về JWT ───────────────────────────────────
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // Kiểm tra rate-limit
            if (_loginAttempts.TryGetValue(ip, out var attempt))
            {
                if (attempt.Count >= 5 &&
                    DateTime.Now - attempt.LastAttempt < TimeSpan.FromMinutes(5))
                    return StatusCode(429, new
                    {
                        message = "Quá nhiều lần đăng nhập sai. Thử lại sau 5 phút!"
                    });

                if (DateTime.Now - attempt.LastAttempt >= TimeSpan.FromMinutes(5))
                    _loginAttempts.Remove(ip);
            }

            var user = await _userRepo.GetByUsernameAsync(req.Username);
            if (user is null || user.PasswordHash != req.Password)
            {
                _loginAttempts[ip] = _loginAttempts.TryGetValue(ip, out var a)
                    ? (a.Count + 1, DateTime.Now)
                    : (1, DateTime.Now);

                return Unauthorized(new { message = "Sai tên đăng nhập hoặc mật khẩu" });
            }

            _loginAttempts.Remove(ip);

            // ── Tạo Access Token (JWT) ────────────────────────────────
            var accessToken  = GenerateAccessToken(user);

            // ── Tạo Refresh Token ─────────────────────────────────────
            var refreshToken = Guid.NewGuid().ToString("N");
            var expiresAt    = DateTime.UtcNow.AddDays(7);

            await _userRepo.SaveRefreshTokenAsync(user.Id, refreshToken, expiresAt);

            return Ok(new
            {
                id           = user.Id,
                username     = user.Username,
                email        = user.Email,
                role         = user.Role,
                token        = accessToken,       // JWT ngắn hạn (8h)
                refreshToken = refreshToken       // dài hạn (7 ngày)
            });
        }

        // ── LÀM MỚI TOKEN ────────────────────────────────────────────
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
        {
            var saved = await _userRepo.GetRefreshTokenAsync(req.RefreshToken);
            if (saved is null || saved.IsRevoked || saved.ExpiresAt < DateTime.UtcNow)
                return Unauthorized(new { message = "Refresh token không hợp lệ hoặc đã hết hạn" });

            var user = await _userRepo.GetByIdAsync(saved.UserId);
            if (user is null)
                return Unauthorized(new { message = "Không tìm thấy user" });

            // Thu hồi token cũ, cấp token mới
            await _userRepo.RevokeRefreshTokenAsync(req.RefreshToken);

            var newAccessToken  = GenerateAccessToken(user);
            var newRefreshToken = Guid.NewGuid().ToString("N");
            await _userRepo.SaveRefreshTokenAsync(user.Id, newRefreshToken,
                                                   DateTime.UtcNow.AddDays(7));

            return Ok(new
            {
                token        = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

        // ── ĐĂNG XUẤT ────────────────────────────────────────────────
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshRequest req)
        {
            await _userRepo.RevokeRefreshTokenAsync(req.RefreshToken);
            return Ok(new { message = "Đăng xuất thành công" });
        }

        // ── ĐĂNG KÝ ──────────────────────────────────────────────────
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (await _userRepo.GetByUsernameAsync(req.Username) is not null)
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại!" });

            if (await _userRepo.GetByEmailAsync(req.Email) is not null)
                return BadRequest(new { message = "Email đã được sử dụng!" });

            var user = new User
            {
                Username     = req.Username,
                Email        = req.Email,
                PasswordHash = req.Password,
                Role         = req.Role
            };

            var id = await _userRepo.CreateAsync(user);
            return Ok(new { id, message = "Đăng ký thành công!" });
        }

        // ── PRIVATE: Tạo JWT ─────────────────────────────────────────
        private string GenerateAccessToken(User user)
        {
            var jwtKey  = _config["Jwt:Key"] ?? "PHANMEMQUANLY_SECRET_KEY_MIN32CHARS!";
            var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.Username),
                new Claim(ClaimTypes.Email,          user.Email ?? ""),
                new Claim(ClaimTypes.Role,           user.Role  ?? "user")
            };

            var token = new JwtSecurityToken(
                claims:            claims,
                expires:           DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // ── Request models ────────────────────────────────────────────────
    public class RefreshRequest
    {
        public string RefreshToken { get; set; } = "";
    }
}