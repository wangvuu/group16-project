using System.Text.RegularExpressions;

namespace duanso11
{
    public partial class Form1 : Form
    {
        private readonly string _rememberFile = "remember.txt";

        public Form1()
        {
            InitializeComponent();
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) btnLogin_Click(s, e); };
            txtRegConfirm.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) btnRegister_Click(s, e); };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (File.Exists(_rememberFile))
            {
                txtUsername.Text = File.ReadAllText(_rememberFile).Trim();
                chkRemember.Checked = true;
                txtPassword.Focus();
            }
        }

        // ── ĐĂNG NHẬP ────────────────────────────────────────────────
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowMsg(lblLoginMsg, "Vui lòng nhập đầy đủ thông tin!", Color.Orange);
                return;
            }

            btnLogin.Enabled = false;
            btnLogin.Text = "Đang đăng nhập...";
            ShowMsg(lblLoginMsg, "", Color.Gray);

            try
            {
                // ── Gọi API login ──────────────────────────────────────
                var response = await ApiService.LoginAsync(
                    txtUsername.Text.Trim(),
                    txtPassword.Text);

                // ── Lưu token + thông tin vào AppSession ───────────────
                AppSession.Set(response!);

                // ── Ghi nhớ tài khoản ──────────────────────────────────
                if (chkRemember.Checked)
                    File.WriteAllText(_rememberFile, txtUsername.Text.Trim());
                else if (File.Exists(_rememberFile))
                    File.Delete(_rememberFile);

                ShowMsg(lblLoginMsg, "Đăng nhập thành công!", Color.Green);
                await Task.Delay(300);

                // ── Mở MainForm ────────────────────────────────────────
                this.Hide();
                var mainForm = new MainForm();
                mainForm.FormClosed += async (s, args) =>
                {
                    await ApiService.LogoutAsync();   // thu hồi refresh token
                    this.Close();
                };
                mainForm.Show();
            }
            catch (Exception ex)
            {
                var msg = ex.Message switch
                {
                    "TOO_MANY_REQUESTS" => "Quá nhiều lần sai! Thử lại sau 5 phút.",
                    "INVALID_CREDENTIALS" => "Sai tên đăng nhập hoặc mật khẩu!",
                    _ => "Không kết nối được server!"
                };
                ShowMsg(lblLoginMsg, msg,
                    ex.Message == "TOO_MANY_REQUESTS" ? Color.DarkRed : Color.Red);

                txtPassword.Clear();
                txtPassword.Focus();
            }
            finally
            {
                btnLogin.Enabled = true;
                btnLogin.Text = "ĐĂNG NHẬP";
            }
        }

        // ── ĐĂNG KÝ ──────────────────────────────────────────────────
        private async void btnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRegUsername.Text) ||
                string.IsNullOrWhiteSpace(txtRegEmail.Text) ||
                string.IsNullOrWhiteSpace(txtRegPassword.Text) ||
                string.IsNullOrWhiteSpace(txtRegConfirm.Text))
            {
                ShowMsg(lblRegMsg, "Vui lòng nhập đầy đủ thông tin!", Color.Orange);
                return;
            }

            if (!txtRegUsername.Text.Trim().All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                ShowMsg(lblRegMsg, "Tên đăng nhập chỉ chứa chữ, số và dấu _", Color.Red);
                txtRegUsername.Focus();
                return;
            }

            if (txtRegUsername.Text.Trim().Length < 4)
            {
                ShowMsg(lblRegMsg, "Tên đăng nhập phải có ít nhất 4 ký tự!", Color.Red);
                return;
            }

            if (!IsValidEmail(txtRegEmail.Text.Trim()))
            {
                ShowMsg(lblRegMsg, "Email không đúng định dạng!", Color.Red);
                txtRegEmail.Focus();
                return;
            }

            var passError = ValidatePassword(txtRegPassword.Text);
            if (passError != null)
            {
                ShowMsg(lblRegMsg, passError, Color.Red);
                txtRegPassword.Focus();
                return;
            }

            if (txtRegPassword.Text != txtRegConfirm.Text)
            {
                ShowMsg(lblRegMsg, "Mật khẩu xác nhận không khớp!", Color.Red);
                txtRegConfirm.Clear();
                txtRegConfirm.Focus();
                return;
            }

            btnRegister.Enabled = false;
            btnRegister.Text = "Đang tạo tài khoản...";

            try
            {
                var (success, message) = await ApiService.RegisterAsync(
                    txtRegUsername.Text.Trim(),
                    txtRegEmail.Text.Trim(),
                    txtRegPassword.Text,
                    cboRole.SelectedItem?.ToString() ?? "user");

                if (success)
                {
                    ShowMsg(lblRegMsg, "Tạo tài khoản thành công!", Color.Green);
                    MessageBox.Show(
                        $"Tài khoản '{txtRegUsername.Text}' đã được tạo!\nVui lòng đăng nhập.",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    ClearRegisterForm();
                    tabControl.SelectedTab = tabLogin;
                }
                else
                {
                    ShowMsg(lblRegMsg, message, Color.Red);
                }
            }
            catch
            {
                ShowMsg(lblRegMsg, "Không kết nối được server!", Color.Red);
            }
            finally
            {
                btnRegister.Enabled = true;
                btnRegister.Text = "TẠO TÀI KHOẢN";
            }
        }

        // ── HELPERS ──────────────────────────────────────────────────
        private bool IsValidEmail(string email) =>
            Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                          RegexOptions.IgnoreCase);

        private string? ValidatePassword(string password)
        {
            if (password.Length < 6) return "Mật khẩu phải có ít nhất 6 ký tự!";
            if (!password.Any(char.IsUpper)) return "Mật khẩu phải có ít nhất 1 chữ hoa!";
            if (!password.Any(char.IsDigit)) return "Mật khẩu phải có ít nhất 1 chữ số!";
            return null;
        }

        private void ClearRegisterForm()
        {
            txtRegUsername.Clear();
            txtRegEmail.Clear();
            txtRegPassword.Clear();
            txtRegConfirm.Clear();
            cboRole.SelectedIndex = 0;
            lblRegMsg.Text = "";
        }

        private void ShowMsg(Label lbl, string msg, Color color)
        {
            lbl.ForeColor = color;
            lbl.Text = msg;
        }
    }
}