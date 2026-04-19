namespace duanso11
{
    /// <summary>
    /// Lưu thông tin user đăng nhập hiện tại — dùng chung toàn app
    /// </summary>
    public static class AppSession
    {
        public static int Id { get; set; }
        public static string Username { get; set; } = "";
        public static string Email { get; set; } = "";
        public static string Role { get; set; } = "";
        public static string Token { get; set; } = "";   // JWT access token
        public static string RefreshToken { get; set; } = "";   // Refresh token

        public static bool IsLoggedIn => !string.IsNullOrEmpty(Token);
        public static bool IsAdmin => Role?.ToLower() == "admin";
        public static bool IsUser => Role?.ToLower() == "user";

        public static void Set(LoginResponse response)
        {
            Id = response.Id;
            Username = response.Username;
            Email = response.Email;
            Role = response.Role;
            Token = response.Token;
            RefreshToken = response.RefreshToken;
        }

        public static void UpdateTokens(string newToken, string newRefreshToken)
        {
            Token = newToken;
            RefreshToken = newRefreshToken;
        }

        public static void Clear()
        {
            Id = 0;
            Username = "";
            Email = "";
            Role = "";
            Token = "";
            RefreshToken = "";
        }
    }
}