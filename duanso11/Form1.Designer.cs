namespace duanso11
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            panelLeft = new Panel();
            lblAppName = new Label();
            lblSub = new Label();
            panelRight = new Panel();
            tabControl = new TabControl();
            tabLogin = new TabPage();
            tabRegister = new TabPage();
            lblLoginTitle = new Label();
            lblLoginDesc = new Label();
            lblUser = new Label();
            txtUsername = new TextBox();
            lblPass = new Label();
            txtPassword = new TextBox();
            chkShowLogin = new CheckBox();
            chkRemember = new CheckBox();
            btnLogin = new Button();
            lblLoginMsg = new Label();
            lblRegTitle = new Label();
            lblRegDesc = new Label();
            lblRegUser = new Label();
            txtRegUsername = new TextBox();
            lblRegEmail = new Label();
            txtRegEmail = new TextBox();
            lblRegPass = new Label();
            txtRegPassword = new TextBox();
            lblRegConfirm = new Label();
            txtRegConfirm = new TextBox();
            lblRegRole = new Label();
            cboRole = new ComboBox();
            chkShowReg = new CheckBox();
            btnRegister = new Button();
            lblRegMsg = new Label();

            panelLeft.SuspendLayout();
            panelRight.SuspendLayout();
            SuspendLayout();

            // ── panelLeft ──
            panelLeft.BackColor = Color.FromArgb(30, 58, 138);
            panelLeft.Controls.Add(lblAppName);
            panelLeft.Controls.Add(lblSub);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Size = new Size(300, 600);

            lblAppName.AutoSize = false;
            lblAppName.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblAppName.ForeColor = Color.White;
            lblAppName.Text = "PHẦN MỀM\nQUẢN LÝ";
            lblAppName.Size = new Size(260, 110);
            lblAppName.Location = new Point(20, 200);

            lblSub.AutoSize = false;
            lblSub.Font = new Font("Segoe UI", 10F);
            lblSub.ForeColor = Color.FromArgb(147, 197, 253);
            lblSub.Text = "Hệ thống quản lý bệnh viện";
            lblSub.Size = new Size(260, 55);
            lblSub.Location = new Point(20, 325);

            // ── panelRight ──
            panelRight.BackColor = Color.White;
            panelRight.Controls.Add(tabControl);
            panelRight.Dock = DockStyle.Fill;

            // ── tabControl ──
            tabControl.Controls.Add(tabLogin);
            tabControl.Controls.Add(tabRegister);
            tabControl.Location = new Point(30, 30);
            tabControl.Size = new Size(480, 530);
            tabControl.Font = new Font("Segoe UI", 11F);
            tabControl.ItemSize = new Size(220, 38);
            tabControl.SizeMode = TabSizeMode.Fixed;

            // ══ Tab Đăng nhập ══
            tabLogin.Text = "  Đăng nhập  ";
            tabLogin.BackColor = Color.White;
            tabLogin.Controls.Add(lblLoginTitle);
            tabLogin.Controls.Add(lblLoginDesc);
            tabLogin.Controls.Add(lblUser);
            tabLogin.Controls.Add(txtUsername);
            tabLogin.Controls.Add(lblPass);
            tabLogin.Controls.Add(txtPassword);
            tabLogin.Controls.Add(chkShowLogin);
            tabLogin.Controls.Add(chkRemember);
            tabLogin.Controls.Add(btnLogin);
            tabLogin.Controls.Add(lblLoginMsg);

            lblLoginTitle.AutoSize = true;
            lblLoginTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblLoginTitle.ForeColor = Color.FromArgb(30, 58, 138);
            lblLoginTitle.Text = "Chào mừng trở lại!";
            lblLoginTitle.Location = new Point(30, 30);

            lblLoginDesc.AutoSize = true;
            lblLoginDesc.Font = new Font("Segoe UI", 10F);
            lblLoginDesc.ForeColor = Color.Gray;
            lblLoginDesc.Text = "Nhập thông tin để đăng nhập";
            lblLoginDesc.Location = new Point(30, 68);

            lblUser.AutoSize = true;
            lblUser.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblUser.ForeColor = Color.FromArgb(55, 65, 81);
            lblUser.Text = "Tên đăng nhập";
            lblUser.Location = new Point(30, 115);

            txtUsername.Font = new Font("Segoe UI", 11F);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Size = new Size(400, 32);
            txtUsername.Location = new Point(30, 138);
            txtUsername.PlaceholderText = "Nhập tên đăng nhập...";

            lblPass.AutoSize = true;
            lblPass.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblPass.ForeColor = Color.FromArgb(55, 65, 81);
            lblPass.Text = "Mật khẩu";
            lblPass.Location = new Point(30, 185);

            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Size = new Size(400, 32);
            txtPassword.Location = new Point(30, 208);
            txtPassword.PlaceholderText = "Nhập mật khẩu...";
            txtPassword.UseSystemPasswordChar = true;

            chkShowLogin.AutoSize = true;
            chkShowLogin.Font = new Font("Segoe UI", 9F);
            chkShowLogin.Text = "Hiển thị mật khẩu";
            chkShowLogin.Location = new Point(30, 248);
            chkShowLogin.CheckedChanged += (s, e) =>
                txtPassword.UseSystemPasswordChar = !chkShowLogin.Checked;

            chkRemember.AutoSize = true;
            chkRemember.Font = new Font("Segoe UI", 9F);
            chkRemember.Text = "Ghi nhớ đăng nhập";
            chkRemember.Location = new Point(220, 248);

            btnLogin.BackColor = Color.FromArgb(30, 58, 138);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Size = new Size(400, 46);
            btnLogin.Location = new Point(30, 280);
            btnLogin.Text = "ĐĂNG NHẬP";
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += btnLogin_Click;

            lblLoginMsg.AutoSize = false;
            lblLoginMsg.Font = new Font("Segoe UI", 10F);
            lblLoginMsg.Size = new Size(400, 30);
            lblLoginMsg.Location = new Point(30, 338);
            lblLoginMsg.TextAlign = ContentAlignment.MiddleCenter;

            // ══ Tab Đăng ký ══
            tabRegister.Text = "  Đăng ký  ";
            tabRegister.BackColor = Color.White;
            tabRegister.Controls.Add(lblRegTitle);
            tabRegister.Controls.Add(lblRegDesc);
            tabRegister.Controls.Add(lblRegUser);
            tabRegister.Controls.Add(txtRegUsername);
            tabRegister.Controls.Add(lblRegEmail);
            tabRegister.Controls.Add(txtRegEmail);
            tabRegister.Controls.Add(lblRegPass);
            tabRegister.Controls.Add(txtRegPassword);
            tabRegister.Controls.Add(lblRegConfirm);
            tabRegister.Controls.Add(txtRegConfirm);
            tabRegister.Controls.Add(lblRegRole);
            tabRegister.Controls.Add(cboRole);
            tabRegister.Controls.Add(chkShowReg);
            tabRegister.Controls.Add(btnRegister);
            tabRegister.Controls.Add(lblRegMsg);

            lblRegTitle.AutoSize = true;
            lblRegTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblRegTitle.ForeColor = Color.FromArgb(30, 58, 138);
            lblRegTitle.Text = "Tạo tài khoản mới";
            lblRegTitle.Location = new Point(30, 20);

            lblRegDesc.AutoSize = true;
            lblRegDesc.Font = new Font("Segoe UI", 10F);
            lblRegDesc.ForeColor = Color.Gray;
            lblRegDesc.Text = "Điền đầy đủ thông tin bên dưới";
            lblRegDesc.Location = new Point(30, 55);

            lblRegUser.AutoSize = true;
            lblRegUser.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRegUser.ForeColor = Color.FromArgb(55, 65, 81);
            lblRegUser.Text = "Tên đăng nhập";
            lblRegUser.Location = new Point(30, 95);

            txtRegUsername.Font = new Font("Segoe UI", 10F);
            txtRegUsername.BorderStyle = BorderStyle.FixedSingle;
            txtRegUsername.Size = new Size(185, 30);
            txtRegUsername.Location = new Point(30, 118);
            txtRegUsername.PlaceholderText = "Tên đăng nhập...";

            lblRegEmail.AutoSize = true;
            lblRegEmail.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRegEmail.ForeColor = Color.FromArgb(55, 65, 81);
            lblRegEmail.Text = "Email";
            lblRegEmail.Location = new Point(230, 95);

            txtRegEmail.Font = new Font("Segoe UI", 10F);
            txtRegEmail.BorderStyle = BorderStyle.FixedSingle;
            txtRegEmail.Size = new Size(185, 30);
            txtRegEmail.Location = new Point(230, 118);
            txtRegEmail.PlaceholderText = "Email...";

            lblRegPass.AutoSize = true;
            lblRegPass.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRegPass.ForeColor = Color.FromArgb(55, 65, 81);
            lblRegPass.Text = "Mật khẩu";
            lblRegPass.Location = new Point(30, 165);

            txtRegPassword.Font = new Font("Segoe UI", 10F);
            txtRegPassword.BorderStyle = BorderStyle.FixedSingle;
            txtRegPassword.Size = new Size(185, 30);
            txtRegPassword.Location = new Point(30, 188);
            txtRegPassword.PlaceholderText = "Mật khẩu...";
            txtRegPassword.UseSystemPasswordChar = true;

            lblRegConfirm.AutoSize = true;
            lblRegConfirm.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRegConfirm.ForeColor = Color.FromArgb(55, 65, 81);
            lblRegConfirm.Text = "Xác nhận mật khẩu";
            lblRegConfirm.Location = new Point(230, 165);

            txtRegConfirm.Font = new Font("Segoe UI", 10F);
            txtRegConfirm.BorderStyle = BorderStyle.FixedSingle;
            txtRegConfirm.Size = new Size(185, 30);
            txtRegConfirm.Location = new Point(230, 188);
            txtRegConfirm.PlaceholderText = "Nhập lại mật khẩu...";
            txtRegConfirm.UseSystemPasswordChar = true;

            lblRegRole.AutoSize = true;
            lblRegRole.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRegRole.ForeColor = Color.FromArgb(55, 65, 81);
            lblRegRole.Text = "Vai trò";
            lblRegRole.Location = new Point(30, 235);

            cboRole.Font = new Font("Segoe UI", 10F);
            cboRole.Size = new Size(185, 30);
            cboRole.Location = new Point(30, 258);
            cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
            cboRole.Items.AddRange(new object[] { "user", "admin" });
            cboRole.SelectedIndex = 0;

            chkShowReg.AutoSize = true;
            chkShowReg.Font = new Font("Segoe UI", 9F);
            chkShowReg.Text = "Hiển thị mật khẩu";
            chkShowReg.Location = new Point(230, 262);
            chkShowReg.CheckedChanged += (s, e) => {
                txtRegPassword.UseSystemPasswordChar = !chkShowReg.Checked;
                txtRegConfirm.UseSystemPasswordChar = !chkShowReg.Checked;
            };

            btnRegister.BackColor = Color.FromArgb(30, 58, 138);
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnRegister.ForeColor = Color.White;
            btnRegister.Size = new Size(400, 46);
            btnRegister.Location = new Point(30, 310);
            btnRegister.Text = "TẠO TÀI KHOẢN";
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.Click += btnRegister_Click;

            lblRegMsg.AutoSize = false;
            lblRegMsg.Font = new Font("Segoe UI", 10F);
            lblRegMsg.Size = new Size(400, 30);
            lblRegMsg.Location = new Point(30, 368);
            lblRegMsg.TextAlign = ContentAlignment.MiddleCenter;

            // ── Form ──
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(860, 600);
            Controls.Add(panelRight);
            Controls.Add(panelLeft);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Phần Mềm Quản Lý";
            BackColor = Color.White;

            panelLeft.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion

        private Panel panelLeft, panelRight;
        private Label lblAppName, lblSub;
        private TabControl tabControl;
        private TabPage tabLogin, tabRegister;
        private Label lblLoginTitle, lblLoginDesc, lblUser, lblPass, lblLoginMsg;
        private TextBox txtUsername, txtPassword;
        private CheckBox chkShowLogin, chkRemember;
        private Button btnLogin;
        private Label lblRegTitle, lblRegDesc, lblRegUser, lblRegEmail;
        private Label lblRegPass, lblRegConfirm, lblRegRole, lblRegMsg;
        private TextBox txtRegUsername, txtRegEmail, txtRegPassword, txtRegConfirm;
        private ComboBox cboRole;
        private CheckBox chkShowReg;
        private Button btnRegister;
    }
}