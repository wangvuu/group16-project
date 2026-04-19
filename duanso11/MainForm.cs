using System.Drawing;
using System.Windows.Forms;

namespace duanso11
{
    public partial class MainForm : Form
    {
        private Panel? panelSidebar;
        private Panel? panelContent;
        private Panel? panelHeader;

        private Button? btnDieuTri;
        private Button? btnHeTHong;
        private Button? btnTiepDonKhongBH;
        private Button? btnTiepDonCoBH;
        private Button? btnSuaHoSo;
        private Button? btnBenhNhan;
        private Button? btnNguoiDung;
        private Button? btnCurrentActive;

        public MainForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            this.Text = "Hệ Thống Quản Lý Bệnh Viện";
            this.Size = new Size(1280, 800);
            this.MinimumSize = new Size(1100, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 9f);

            BuildHeader();
            BuildSidebar();
            BuildContentArea();

            ShowTiepDonKhongBH();
        }

        private void BuildHeader()
        {
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 56,
                BackColor = Color.FromArgb(15, 82, 186)
            };

            var lblHospital = new Label
            {
                Text = "🏥  BỆNH VIỆN ĐA KHOA TRUNG ƯƠNG",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 14)
            };

            var lblTime = new Label
            {
                Name = "lblTime",
                Text = DateTime.Now.ToString("HH:mm  |  dd/MM/yyyy"),
                ForeColor = Color.FromArgb(180, 210, 255),
                Font = new Font("Segoe UI", 9f),
                AutoSize = true,
                Location = new Point(1050, 18)
            };

            panelHeader.Controls.Add(lblHospital);
            panelHeader.Controls.Add(lblTime);
            this.Controls.Add(panelHeader);

            var timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += (s, e) => lblTime.Text = DateTime.Now.ToString("HH:mm  |  dd/MM/yyyy");
            timer.Start();
        }

        private void BuildSidebar()
        {
            panelSidebar = new Panel
            {
                Width = 220,
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(21, 41, 82),
                Padding = new Padding(0, 10, 0, 10)
            };

            int y = 20;

            btnDieuTri = CreateMenuGroup("⚕  ĐIỀU TRỊ", y);
            btnDieuTri.Click += (s, e) => ToggleGroup(new[] { btnTiepDonKhongBH!, btnTiepDonCoBH!, btnSuaHoSo!, btnBenhNhan! });
            panelSidebar.Controls.Add(btnDieuTri);
            y += 42;

            btnTiepDonKhongBH = CreateSubMenu("Tiếp đón không BH", y);
            btnTiepDonKhongBH.Click += (s, e) => { SetActive(btnTiepDonKhongBH); ShowTiepDonKhongBH(); };
            panelSidebar.Controls.Add(btnTiepDonKhongBH);
            y += 38;

            btnTiepDonCoBH = CreateSubMenu("Tiếp đón có BH", y);
            btnTiepDonCoBH.Click += (s, e) => { SetActive(btnTiepDonCoBH); ShowTiepDonCoBH(); };
            panelSidebar.Controls.Add(btnTiepDonCoBH);
            y += 38;

            btnSuaHoSo = CreateSubMenu("Sửa thông tin hồ sơ", y);
            btnSuaHoSo.Click += (s, e) => { SetActive(btnSuaHoSo); ShowSuaHoSo(); };
            panelSidebar.Controls.Add(btnSuaHoSo);
            y += 38;

            btnBenhNhan = CreateSubMenu("Bệnh nhân", y);
            btnBenhNhan.Click += (s, e) => { SetActive(btnBenhNhan); ShowBenhNhan(); };
            panelSidebar.Controls.Add(btnBenhNhan);
            y += 50;

            btnHeTHong = CreateMenuGroup("⚙  HỆ THỐNG", y);
            btnHeTHong.Click += (s, e) => ToggleGroup(new[] { btnNguoiDung! });
            panelSidebar.Controls.Add(btnHeTHong);
            y += 42;

            btnNguoiDung = CreateSubMenu("Người dùng", y);
            btnNguoiDung.Click += (s, e) => { SetActive(btnNguoiDung); ShowNguoiDung(); };
            panelSidebar.Controls.Add(btnNguoiDung);

            // ════ NÚT ĐĂNG XUẤT ════
            var btnLogout = new Button
            {
                Text = "🚪  Đăng xuất",
                Size = new Size(220, 42),
                Dock = DockStyle.Bottom,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(255, 180, 180),
                BackColor = Color.FromArgb(120, 30, 30),
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(160, 40, 40);
            btnLogout.Click += BtnLogout_Click;
            panelSidebar.Controls.Add(btnLogout);

            var lblVer = new Label
            {
                Text = "v1.0.0  |  2025",
                ForeColor = Color.FromArgb(80, 100, 140),
                Font = new Font("Segoe UI", 8f),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 30
            };
            panelSidebar.Controls.Add(lblVer);

            this.Controls.Add(panelSidebar);
        }

        // ════ XỬ LÝ ĐĂNG XUẤT ════
        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn đăng xuất?\nTài khoản: {AppSession.Username}",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm != DialogResult.Yes) return;

            
            AppSession.Clear();

       
            var loginForm = new Form1();
            loginForm.Show();

          
            this.Close();
        }

        private static Button CreateMenuGroup(string text, int y)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(0, y),
                Size = new Size(220, 42),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(180, 210, 255),
                BackColor = Color.FromArgb(30, 55, 110),
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(14, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private static Button CreateSubMenu(string text, int y)
        {
            var btn = new Button
            {
                Text = "    • " + text,
                Location = new Point(0, y),
                Size = new Size(220, 38),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FromArgb(140, 170, 220),
                BackColor = Color.FromArgb(21, 41, 82),
                Font = new Font("Segoe UI", 9f),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(35, 65, 120);
            return btn;
        }

        private static void ToggleGroup(Button[] children)
        {
            bool anyVisible = children[0].Visible;
            foreach (var c in children) c.Visible = !anyVisible;
        }

        private void SetActive(Button btn)
        {
            if (btnCurrentActive != null)
            {
                btnCurrentActive.BackColor = Color.FromArgb(21, 41, 82);
                btnCurrentActive.ForeColor = Color.FromArgb(140, 170, 220);
            }
            btn.BackColor = Color.FromArgb(15, 82, 186);
            btn.ForeColor = Color.White;
            btnCurrentActive = btn;
        }

        private void BuildContentArea()
        {
            panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 247, 250),
                Padding = new Padding(20)
            };
            this.Controls.Add(panelContent);
            panelContent.BringToFront();
        }

        private void LoadContent(Control form)
        {
            panelContent!.Controls.Clear();
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Add(form);
        }

        private void ShowTiepDonKhongBH() => LoadContent(new TiepDonKhongBHPanel());
        private void ShowTiepDonCoBH() => LoadContent(new TiepDonCoBHPanel());
        private void ShowSuaHoSo() => LoadContent(new SuaHoSoPanel());
        private void ShowBenhNhan() => LoadContent(new BenhNhanPanel());
        private void ShowNguoiDung() => LoadContent(new NguoiDungPanel());
    }
}