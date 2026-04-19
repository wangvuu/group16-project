using Dapper;
using Npgsql;
using duanso1.Model;

namespace duanso1.Repositories
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private NpgsqlConnection GetConnection()
            => new NpgsqlConnection(_connectionString);

        // ── USERS ─────────────────────────────────────────────────────

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using var conn = GetConnection();
            return await conn.QueryAsync<User>(
                "SELECT * FROM users ORDER BY id");
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(
                @"SELECT id, username, email,
                         password_hash AS PasswordHash,
                         role, created_at AS CreatedAt
                  FROM users WHERE id = @Id",
                new { Id = id });
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(
                @"SELECT id, username, email,
                         password_hash AS PasswordHash,
                         role, created_at AS CreatedAt
                  FROM users
                  WHERE username = @Username",
                new { Username = username });
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(
                @"SELECT id, username, email,
                         password_hash AS PasswordHash,
                         role, created_at AS CreatedAt
                  FROM users WHERE email = @Email",
                new { Email = email });
        }

        public async Task<int> CreateAsync(User user)
        {
            using var conn = GetConnection();
            var sql = @"INSERT INTO users (username, email, password_hash, role, created_at)
                        VALUES (@Username, @Email, @PasswordHash, @Role, NOW())
                        RETURNING id";
            return await conn.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            using var conn = GetConnection();
            var sql = @"UPDATE users
                        SET username = @Username, email = @Email, role = @Role
                        WHERE id = @Id";
            var rows = await conn.ExecuteAsync(sql, user);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var conn = GetConnection();
            var rows = await conn.ExecuteAsync(
                "DELETE FROM users WHERE id = @Id", new { Id = id });
            return rows > 0;
        }

        // ── REFRESH TOKENS ────────────────────────────────────────────

        /// <summary>Lưu refresh token mới vào DB</summary>
        public async Task SaveRefreshTokenAsync(int userId, string token, DateTime expiresAt)
        {
            using var conn = GetConnection();
            var sql = @"INSERT INTO refresh_tokens (user_id, token, expires_at, created_at, is_revoked)
                        VALUES (@UserId, @Token, @ExpiresAt, NOW(), false)";
            await conn.ExecuteAsync(sql, new { UserId = userId, Token = token, ExpiresAt = expiresAt });
        }

        /// <summary>Lấy refresh token từ DB (để kiểm tra hợp lệ)</summary>
        public async Task<RefreshTokenRecord?> GetRefreshTokenAsync(string token)
        {
            using var conn = GetConnection();
            return await conn.QueryFirstOrDefaultAsync<RefreshTokenRecord>(
                @"SELECT id, user_id AS UserId, token,
                         expires_at  AS ExpiresAt,
                         created_at  AS CreatedAt,
                         is_revoked  AS IsRevoked
                  FROM refresh_tokens
                  WHERE token = @Token",
                new { Token = token });
        }

        /// <summary>Thu hồi (vô hiệu hóa) refresh token</summary>
        public async Task RevokeRefreshTokenAsync(string token)
        {
            using var conn = GetConnection();
            await conn.ExecuteAsync(
                "UPDATE refresh_tokens SET is_revoked = true WHERE token = @Token",
                new { Token = token });
        }

        /// <summary>Xoá toàn bộ refresh token hết hạn (dọn dẹp định kỳ)</summary>
        public async Task CleanExpiredTokensAsync()
        {
            using var conn = GetConnection();
            await conn.ExecuteAsync(
                "DELETE FROM refresh_tokens WHERE expires_at < NOW()");
        }
    }

    // ── Model cho refresh_tokens table ───────────────────────────────
    public class RefreshTokenRecord
    {
        public int      Id        { get; set; }
        public int      UserId    { get; set; }
        public string   Token     { get; set; } = "";
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool     IsRevoked { get; set; }
    }
}