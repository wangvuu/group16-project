using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace duanso11
{
    // ════════════════════════════════════════════════════════════════
    //  TIẾP ĐÓN KHÔNG BH
    // ════════════════════════════════════════════════════════════════
    public class TiepDonKhongBHPanel : Panel
    {
        private TextBox txtMaBN = new(), txtSoHoSo = new(), txtHoTen = new(),
                        txtNgaySinh = new(), txtSDT = new(), txtCCCD = new(),
                        txtNguoiThan = new(), txtSDTNguoiThan = new(), txtDiaChi = new();
        private ComboBox cboGioiTinh = new(), cboQuocTich = new(), cboDanToc = new(),
                         cboNgheNghiep = new(), cboTinhThanh = new();
        private int? _currentBenhNhanId;

        // Tham chiếu nút Lưu để bật/tắt
        private Button _btnLuu = new();

        // ── Trạng thái: true = đang nhập mới (chưa lưu), false = đã lưu / đang xem
        private bool _isNewMode = true;
        private bool _isUpdating = false;

        // ── Cho phép mở khóa form để chỉnh sửa (dùng ở SuaHoSoPanel)
        protected virtual bool AllowEdit => false;

        public TiepDonKhongBHPanel()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);
            BuildUI();
            _ = LoadComboboxesAsync();
        }

        protected virtual void BuildUI()
        {
            var pHeader = UIHelper.MakePageHeader("Tiếp Đón Bệnh Nhân Không Bảo Hiểm", "⚕");
            this.Controls.Add(pHeader);

            var pSearch = UIHelper.MakeCard(new Point(0, 68), new Size(this.Width - 2, 60));
            pSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            var lblS = new Label { Text = "🔍", Location = new Point(14, 18), AutoSize = true, Font = new Font("Segoe UI", 11f) };
            var txtSearch = new TextBox
            {
                Location = new Point(40, 16),
                Size = new Size(400, 28),
                Font = new Font("Segoe UI", 10f),
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Tìm theo tên, CCCD, mã bệnh nhân..."
            };
            var btnSearch = UIHelper.MakePrimaryBtn("Tìm kiếm", new Point(450, 12));
            var btnClear = UIHelper.MakeSecondaryBtn("Xóa form", new Point(555, 12), new Size(80, 34));
            btnSearch.Click += async (s, e) => await SearchAsync(txtSearch.Text);
            btnClear.Click += (s, e) => StartNewMode();
            pSearch.Controls.AddRange(new Control[] { lblS, txtSearch, btnSearch, btnClear });
            this.Controls.Add(pSearch);

            var pInfo = UIHelper.MakeCard(new Point(0, 140), new Size(this.Width - 2, 460));
            pInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            UIHelper.AddSectionLabel(pInfo, "THÔNG TIN BỆNH NHÂN", new Point(14, 10));

            UIHelper.AddFieldPair(pInfo, "Mã bệnh nhân (tự sinh)", "txtMaBN", new Point(14, 44), new Size(200, 28));
            txtMaBN = (TextBox)pInfo.Controls[pInfo.Controls.Count - 1];
            txtMaBN.ReadOnly = true; txtMaBN.BackColor = Color.FromArgb(240, 240, 240);

            UIHelper.AddFieldPair(pInfo, "Số hồ sơ", "txtSoHoSo", new Point(230, 44), new Size(180, 28));
            txtSoHoSo = (TextBox)pInfo.Controls[pInfo.Controls.Count - 1];

            UIHelper.AddFieldPair(pInfo, "Họ và tên *", "txtHoTen", new Point(426, 44), new Size(320, 28));
            txtHoTen = (TextBox)pInfo.Controls[pInfo.Controls.Count - 1];
            txtHoTen.TextChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtHoTen.Text)) return;
                int pos = txtHoTen.SelectionStart;
                string converted = System.Globalization.CultureInfo.CurrentCulture
                    .TextInfo.ToTitleCase(txtHoTen.Text.ToLower());
                if (txtHoTen.Text != converted)
                {
                    txtHoTen.Text = converted;
                    txtHoTen.SelectionStart = Math.Min(pos, txtHoTen.Text.Length);
                }
            };

            UIHelper.AddFieldPair(pInfo, "Ngày sinh (dd/MM/yyyy)", "txtNgaySinh", new Point(14, 110), new Size(180, 28));
            txtNgaySinh = (TextBox)pInfo.Controls[pInfo.Controls.Count - 1];

            cboGioiTinh = UIHelper.AddComboField(pInfo, "Giới tính", new Point(210, 110), new Size(130, 28), new[] { "Nam", "Nữ", "Khác" });

            // ── Số điện thoại: tối đa 11 số nếu bắt đầu "02", còn lại 10 số ──
            UIHelper.AddFieldPair(pInfo, "Số điện thoại", "txtSDT", new Point(356, 110), new Size(180, 28));
            txtSDT = (TextBox)pInfo.Controls[pInfo.Controls.Count - 1];
            txtSDT.KeyPress += (s, e) =>
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back) e.Handled = true;
            };
            txtSDT.TextChanged += (s, e) =>
            {
                int maxLen = txtSDT.Text.StartsWith("02") ? 11 : 10;
                if (txtSDT.Text.Length > maxLen)
                {
                    txtSDT.Text = txtSDT.Text[..maxLen];
                    txtSDT.SelectionStart = maxLen;
                }
            };

            UIHelper.AddFieldPair(pInfo, "Căn cước công dân", "txtCCCD", new Point(552, 110), new Size(190, 28));
            txtCCCD = (TextBox)pInfo.Controls[pInfo.Controls.Count - 1];
            txtCCCD.KeyPress += (s, e) =>
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back) e.Handled = true;
            };
            txtCCCD.TextChanged += (s, e) =>
            {
                if (txtCCCD.Text.Length > 12) { txtCCCD.Text = txtCCCD.Text[..12]; txtCCCD.SelectionStart = 12; }
            };

            // ── Người thân: tự động viết hoa chữ cái đầu ──
            UIHelper.AddFieldPair(pInfo, "Người thân", "txtNguoiThan", new Point(14, 176), new Size(250, 28));
            txtNguoiThan = (TextBox)pInfo.Controls[pInfo.Controls.Count - 1];
            txtNguoiThan.TextChanged += (s, e) =>
            {
                if (string.IsNullOrEmpty(txtNguoiThan.Text)) return;
                int pos = txtNguoiThan.SelectionStart;
                string converted = System.Globalization.CultureInfo.CurrentCulture
                    .TextInfo.ToTitleCase(txtNguoiThan.Text.ToLower());
                if (txtNguoiThan.Text != converted)
                {
                    txtNguoiThan.Text = converted;
                    txtNguoiThan.SelectionStart = Math.Min(pos, txtNguoiThan.Text.Length);
                }
            };

            UIHelper.AddFieldPair(pInfo, "SĐT người thân", "txtSDTNguoiThan", new Point(280, 176), new Size(180, 28));
            txtSDTNguoiThan = (TextBox)pInfo.Controls[pInfo.Controls.Count - 1];
            txtSDTNguoiThan.KeyPress += (s, e) =>
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back) e.Handled = true;
            };
            txtSDTNguoiThan.TextChanged += (s, e) =>
            {
                if (txtSDTNguoiThan.Text.Length > 10) { txtSDTNguoiThan.Text = txtSDTNguoiThan.Text[..10]; txtSDTNguoiThan.SelectionStart = 10; }
            };

            // ── Quốc tịch: DropDown có thể gõ lọc, mặc định "Việt Nam" ──
            var lblQT = new Label
            {
                Text = "Quốc tịch",
                Location = new Point(14, 224),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = UIHelper.LabelFg
            };
            cboQuocTich = new ComboBox
            {
                Location = new Point(14, 242),
                Size = new Size(160, 28),
                Font = new Font("Segoe UI", 10f),
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDown,
                BackColor = Color.FromArgb(250, 252, 255),
                AutoCompleteMode = AutoCompleteMode.None,
                AutoCompleteSource = AutoCompleteSource.None
            };
            pInfo.Controls.Add(lblQT);
            pInfo.Controls.Add(cboQuocTich);

            // ── Dân tộc: DropDown có thể gõ lọc, mặc định "Kinh" ──
            var lblDT = new Label
            {
                Text = "Dân tộc",
                Location = new Point(190, 224),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = UIHelper.LabelFg
            };
            cboDanToc = new ComboBox
            {
                Location = new Point(190, 242),
                Size = new Size(160, 28),
                Font = new Font("Segoe UI", 10f),
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDown,
                BackColor = Color.FromArgb(250, 252, 255),
                AutoCompleteMode = AutoCompleteMode.None,
                AutoCompleteSource = AutoCompleteSource.None
            };
            pInfo.Controls.Add(lblDT);
            pInfo.Controls.Add(cboDanToc);

            // ── Nghề nghiệp: DropDown có thể gõ lọc ──
            var lblNN = new Label
            {
                Text = "Nghề nghiệp",
                Location = new Point(366, 224),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = UIHelper.LabelFg
            };
            cboNgheNghiep = new ComboBox
            {
                Location = new Point(366, 242),
                Size = new Size(200, 28),
                Font = new Font("Segoe UI", 10f),
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDown,
                BackColor = Color.FromArgb(250, 252, 255),
                AutoCompleteMode = AutoCompleteMode.None,
                AutoCompleteSource = AutoCompleteSource.None
            };
            pInfo.Controls.Add(lblNN);
            pInfo.Controls.Add(cboNgheNghiep);

            // ── Tỉnh/Thành phố: DropDown có thể gõ lọc ──
            var lblTT = new Label
            {
                Text = "Tỉnh/Thành phố",
                Location = new Point(582, 224),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = UIHelper.LabelFg
            };
            cboTinhThanh = new ComboBox
            {
                Location = new Point(582, 242),
                Size = new Size(180, 28),
                Font = new Font("Segoe UI", 10f),
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDown,
                BackColor = Color.FromArgb(250, 252, 255),
                AutoCompleteMode = AutoCompleteMode.None,
                AutoCompleteSource = AutoCompleteSource.None
            };
            pInfo.Controls.Add(lblTT);
            pInfo.Controls.Add(cboTinhThanh);

            UIHelper.AddFieldPair(pInfo, "Địa chỉ chi tiết", "txtDiaChi", new Point(14, 308), new Size(748, 28));
            txtDiaChi = (TextBox)pInfo.Controls[pInfo.Controls.Count - 1];

            pInfo.Controls.Add(UIHelper.MakeHorizontalLine(new Point(14, 356), pInfo.Width - 30));

            // ── NÚT: Lưu hồ sơ / Thêm mới / Xóa ──
            _btnLuu = UIHelper.MakePrimaryBtn("💾  Lưu hồ sơ", new Point(14, 370), new Size(130, 38));
            var btnThem = UIHelper.MakeSuccessBtn("➕  Thêm mới", new Point(154, 370), new Size(130, 38));
            var btnXoa = UIHelper.MakeDangerBtn("🗑  Xóa", new Point(294, 370), new Size(100, 38));

            _btnLuu.Click += async (s, e) => await SaveAsync();
            btnThem.Click += (s, e) => StartNewMode();
            btnXoa.Click += async (s, e) => await DeleteAsync();

            pInfo.Controls.AddRange(new Control[] { _btnLuu, btnThem, btnXoa });
            this.Controls.Add(pInfo);

            // Bắt đầu ở chế độ nhập mới
            SetFormEditable(true);
        }

        // ════ HELPERS: khóa / mở form ════

        protected void SetFormEditable(bool editable)
        {
            var inputs = new Control[]
            {
        txtSoHoSo, txtHoTen, txtNgaySinh, txtSDT, txtCCCD,
        txtNguoiThan, txtSDTNguoiThan, txtDiaChi,
        cboGioiTinh, cboQuocTich, cboDanToc, cboNgheNghiep, cboTinhThanh
            };

            foreach (var c in inputs)
            {
                if (c is TextBox tb)
                {
                    tb.ReadOnly = !editable;
                    tb.BackColor = editable ? Color.White : Color.FromArgb(240, 240, 240);
                }
                else if (c is ComboBox cbo)
                {
                    cbo.Enabled = editable;
                }
            }

            // ✅ Số hồ sơ luôn readonly khi đã có hồ sơ (không cho sửa mã hệ thống)
            if (_currentBenhNhanId.HasValue)
            {
                txtSoHoSo.ReadOnly = true;
                txtSoHoSo.BackColor = Color.FromArgb(240, 240, 240);
            }

            _btnLuu.Enabled = editable;
            _btnLuu.BackColor = editable
                ? UIHelper.Primary
                : Color.FromArgb(160, 160, 160);
        }

        protected void StartNewMode()
        {
            _currentBenhNhanId = null;
            _isNewMode = true;
            ClearFields();
            SetFormEditable(true);
        }

        // ════ LOAD COMBOBOXES ════

        private async Task LoadComboboxesAsync()
        {
            try
            {
                var qtList = await ApiService.GetQuocTichAsync();
                var dtList = await ApiService.GetDanTocAsync();
                var nnList = await ApiService.GetNgheNghiepAsync();
                var ttList = await ApiService.GetTinhThanhAsync();

                BindComboSearchable(cboQuocTich, qtList, "Việt Nam");
                BindComboSearchable(cboDanToc, dtList, "Kinh");
                BindComboSearchable(cboNgheNghiep, nnList, "");
                BindComboSearchable(cboTinhThanh, ttList, "");
            }
            catch { }
        }

        // ── Bind cho ComboBox dạng DropDown có gõ lọc ──
        private void BindComboSearchable(ComboBox cbo, List<DanhMucItem> items, string defaultText)
        {
            if (cbo.InvokeRequired) { cbo.Invoke(() => BindComboSearchable(cbo, items, defaultText)); return; }

            // Lưu toàn bộ danh sách vào Tag để lọc
            cbo.Tag = items;

            _isUpdating = true;
            try
            {
                cbo.Items.Clear();
                foreach (var i in items) cbo.Items.Add(i.Ten);
                cbo.SelectedIndex = -1;
                var match = items.Find(i => i.Ten.Equals(defaultText, StringComparison.OrdinalIgnoreCase));
                cbo.Text = match != null ? match.Ten : "";
            }
            finally
            {
                _isUpdating = false;
            }

            // Đăng ký sự kiện lọc
            cbo.TextChanged -= ComboSearchable_TextChanged;
            cbo.TextChanged += ComboSearchable_TextChanged;
        }

        // ── Xử lý gõ phím lọc danh sách + tự động mở dropdown ──
        private void ComboSearchable_TextChanged(object? sender, EventArgs e)
        {
            if (_isUpdating) return;
            if (sender is not ComboBox cbo) return;
            if (cbo.Tag is not List<DanhMucItem> all) return;

            string keyword = cbo.Text;
            var filtered = all.FindAll(i =>
                i.Ten.Contains(keyword, StringComparison.OrdinalIgnoreCase));

            _isUpdating = true;
            try
            {
                cbo.BeginUpdate();
                cbo.Items.Clear();
                foreach (var i in filtered) cbo.Items.Add(i.Ten);
                cbo.EndUpdate();

                cbo.Text = keyword;
                cbo.SelectionStart = keyword.Length;
                cbo.SelectionLength = 0;
            }
            finally
            {
                _isUpdating = false;
            }

            // ✅ Tự động mở dropdown nếu có kết quả — dùng BeginInvoke để tránh re-entrancy
            if (filtered.Count > 0 && !cbo.DroppedDown)
            {
                cbo.BeginInvoke(() => { cbo.DroppedDown = true; });
            }
        }

        // ── Lấy Id từ text hiển thị trong ComboBox dạng DropDown ──
        private int? GetDanhMucIdByText(ComboBox cbo, string text)
        {
            if (cbo.Tag is not List<DanhMucItem> all) return null;
            return all.Find(i => i.Ten.Equals(text?.Trim(), StringComparison.OrdinalIgnoreCase))?.Id;
        }

        // ════ TÌM KIẾM ════

        private async Task SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return;

            // ✅ Đảm bảo combobox đã load xong trước khi fill
            if (cboNgheNghiep.Tag == null)
                await LoadComboboxesAsync();

            var list = await ApiService.SearchBenhNhanAsync(keyword);
            if (list.Count == 0)
            {
                MessageBox.Show("Không tìm thấy bệnh nhân!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _isNewMode = false;
            FillForm(list[0]);
            SetFormEditable(AllowEdit);
        }

        // ════ FILL / BUILD ════

        protected void FillForm(BenhNhan bn)
        {
            _currentBenhNhanId = bn.Id;
            txtMaBN.Text = bn.MaBenhNhan;
            txtSoHoSo.Text = bn.SoHoSo;
            txtHoTen.Text = bn.HoTen;
            txtNgaySinh.Text = bn.NgaySinh?.ToString("dd/MM/yyyy") ?? "";
            if (cboGioiTinh.Items.Contains(bn.GioiTinh)) cboGioiTinh.SelectedItem = bn.GioiTinh;
            txtSDT.Text = bn.SoDienThoai;
            txtCCCD.Text = bn.Cccd;
            txtNguoiThan.Text = bn.NguoiThan;
            txtSDTNguoiThan.Text = bn.SdtNguoiThan;
            txtDiaChi.Text = bn.DiaChi;

            // ── Tất cả 4 combo dạng DropDown → set bằng Text ──
            if (bn.QuocTichId.HasValue && cboQuocTich.Tag is List<DanhMucItem> qtAll)
                cboQuocTich.Text = qtAll.Find(i => i.Id == bn.QuocTichId.Value)?.Ten ?? "";

            if (bn.DanTocId.HasValue && cboDanToc.Tag is List<DanhMucItem> dtAll)
                cboDanToc.Text = dtAll.Find(i => i.Id == bn.DanTocId.Value)?.Ten ?? "";

            if (bn.NgheNghiepId.HasValue && cboNgheNghiep.Tag is List<DanhMucItem> nnAll)
                cboNgheNghiep.Text = nnAll.Find(i => i.Id == bn.NgheNghiepId.Value)?.Ten ?? "";

            if (bn.TinhThanhId.HasValue && cboTinhThanh.Tag is List<DanhMucItem> ttAll)
                cboTinhThanh.Text = ttAll.Find(i => i.Id == bn.TinhThanhId.Value)?.Ten ?? "";
        }

        private BenhNhanRequest BuildRequest() => new()
        {
            SoHoSo = txtSoHoSo.Text.Trim(),
            HoTen = txtHoTen.Text.Trim(),
            NgaySinh = txtNgaySinh.Text.Trim(),
            GioiTinh = cboGioiTinh.SelectedItem?.ToString() ?? "",
            SoDienThoai = txtSDT.Text.Trim(),
            Cccd = txtCCCD.Text.Trim(),
            NguoiThan = txtNguoiThan.Text.Trim(),
            SdtNguoiThan = txtSDTNguoiThan.Text.Trim(),
            DiaChi = txtDiaChi.Text.Trim(),
            // ✅ Tất cả 4 combo đều dùng GetDanhMucIdByText
            QuocTichId = GetDanhMucIdByText(cboQuocTich, cboQuocTich.Text),
            DanTocId = GetDanhMucIdByText(cboDanToc, cboDanToc.Text),
            NgheNghiepId = GetDanhMucIdByText(cboNgheNghiep, cboNgheNghiep.Text),
            TinhThanhId = GetDanhMucIdByText(cboTinhThanh, cboTinhThanh.Text)
        };

        // ════ LƯU ════

        private async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ── Validate SĐT: 11 số nếu bắt đầu "02", còn lại 10 số ──
            if (!string.IsNullOrEmpty(txtSDT.Text))
            {
                bool isBanLine = txtSDT.Text.StartsWith("02");
                int expectedLen = isBanLine ? 11 : 10;
                if (txtSDT.Text.Length != expectedLen)
                {
                    MessageBox.Show(
                        isBanLine
                            ? "Số điện thoại bàn (bắt đầu 02) phải đúng 11 số!"
                            : "Số điện thoại di động phải đúng 10 số!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSDT.Focus(); return;
                }
            }

            if (!string.IsNullOrEmpty(txtCCCD.Text) && txtCCCD.Text.Length != 12)
            {
                MessageBox.Show("Căn cước công dân phải đúng 12 số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCCCD.Focus(); return;
            }

            var req = BuildRequest();

            if (_currentBenhNhanId.HasValue)
            {
                // ✅ Đổi: nhận BenhNhan? thay vì bool
                var updated = await ApiService.UpdateBenhNhanAsync(_currentBenhNhanId.Value, req);
                if (updated != null)
                {
                    txtSoHoSo.Text = updated.SoHoSo;   // ✅ Cập nhật số hồ sơ mới lên form

                    MessageBox.Show("Cập nhật hồ sơ thành công!\n" +
                                    $"Số hồ sơ mới: {updated.SoHoSo}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SetFormEditable(false);
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var result = await ApiService.CreateBenhNhanAsync(req);
                if (result != null)
                {
                    txtMaBN.Text = result.MaBenhNhan;
                    _currentBenhNhanId = result.Id;
                    _isNewMode = false;

                    MessageBox.Show(
                        $"Tạo hồ sơ thành công!\nMã BN: {result.MaBenhNhan}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Khóa form sau khi lưu mới — không cho sửa ở màn Tiếp đón
                    SetFormEditable(false);
                }
                else
                {
                    MessageBox.Show("Tạo hồ sơ thất bại!\nKiểm tra kết nối API backend.",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ════ XÓA ════

        private async Task DeleteAsync()
        {
            if (!_currentBenhNhanId.HasValue)
            {
                MessageBox.Show("Chưa chọn bệnh nhân!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Xác nhận xóa hồ sơ bệnh nhân này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            var ok = await ApiService.DeleteBenhNhanAsync(_currentBenhNhanId.Value);
            if (ok)
            {
                StartNewMode();
                MessageBox.Show("Đã xóa hồ sơ!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ════ CLEAR ════

        private void ClearFields()
        {
            txtMaBN.Text = ""; txtSoHoSo.Text = ""; txtHoTen.Text = "";
            txtNgaySinh.Text = ""; txtSDT.Text = ""; txtCCCD.Text = "";
            txtNguoiThan.Text = ""; txtSDTNguoiThan.Text = ""; txtDiaChi.Text = "";
            cboGioiTinh.SelectedIndex = 0;

            // ✅ Reset cả 4 combo về mặc định và khôi phục toàn bộ danh sách
            ResetSearchableCombo(cboQuocTich, "Việt Nam");
            ResetSearchableCombo(cboDanToc, "Kinh");
            ResetSearchableCombo(cboNgheNghiep, "");
            ResetSearchableCombo(cboTinhThanh, "");
        }

        // Khôi phục toàn bộ list + set về giá trị mặc định
        private void ResetSearchableCombo(ComboBox cbo, string defaultText)
        {
            if (cbo.Tag is not List<DanhMucItem> all) return;
            _isUpdating = true;
            try
            {
                cbo.BeginUpdate();
                cbo.Items.Clear();
                foreach (var i in all) cbo.Items.Add(i.Ten);
                cbo.EndUpdate();
                var match = all.Find(i => i.Ten.Equals(defaultText, StringComparison.OrdinalIgnoreCase));
                cbo.Text = match != null ? match.Ten : "";
            }
            finally
            {
                _isUpdating = false;
            }
        }
    }

    // ════════════════════════════════════════════════════════════════
    //  TIẾP ĐÓN CÓ BH
    // ════════════════════════════════════════════════════════════════
    public class TiepDonCoBHPanel : Panel
    {
        private TextBox txtMaThe = new(), txtHanSD = new(), txtDiaChi = new();
        private ComboBox cboNoiDangKy = new();
        private RadioButton rb80 = new(), rb95 = new(), rb100 = new();
        private int? _benhNhanId;

        public TiepDonCoBHPanel()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);
            BuildUI();
            _ = LoadComboboxesAsync();
        }

        private void BuildUI()
        {
            var pHeader = UIHelper.MakePageHeader("Tiếp Đón Bệnh Nhân Có Bảo Hiểm", "🏥");
            this.Controls.Add(pHeader);

            var pSearch = UIHelper.MakeCard(new Point(0, 68), new Size(this.Width - 2, 60));
            pSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            var lbl = new Label { Text = "🔍", Location = new Point(14, 18), AutoSize = true, Font = new Font("Segoe UI", 11f) };
            var txtSearch = new TextBox { Location = new Point(40, 16), Size = new Size(380, 28), Font = new Font("Segoe UI", 10f), BorderStyle = BorderStyle.FixedSingle, PlaceholderText = "Tìm mã BN hoặc họ tên..." };
            var btnSearch = UIHelper.MakePrimaryBtn("Tìm bệnh nhân", new Point(430, 12), new Size(140, 34));
            btnSearch.Click += async (s, e) => await SearchBenhNhanAsync(txtSearch.Text);
            pSearch.Controls.AddRange(new Control[] { lbl, txtSearch, btnSearch });
            this.Controls.Add(pSearch);

            var pBH = UIHelper.MakeCard(new Point(0, 140), new Size(this.Width - 2, 250));
            pBH.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            UIHelper.AddSectionLabel(pBH, "THÔNG TIN BẢO HIỂM Y TẾ", new Point(14, 10));

            UIHelper.AddFieldPair(pBH, "Mã thẻ BHYT *", "txtMaThe", new Point(14, 44), new Size(280, 28));
            txtMaThe = (TextBox)pBH.Controls[pBH.Controls.Count - 1];
            txtMaThe.CharacterCasing = CharacterCasing.Upper;
            txtMaThe.KeyPress += (s, e) =>
            {
                if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back) e.Handled = true;
            };
            txtMaThe.TextChanged += (s, e) =>
            {
                if (txtMaThe.Text.Length > 16) { txtMaThe.Text = txtMaThe.Text[..16]; txtMaThe.SelectionStart = 16; }
            };

            UIHelper.AddFieldPair(pBH, "Hạn sử dụng (MM/yyyy)", "txtHanSD", new Point(310, 44), new Size(180, 28));
            txtHanSD = (TextBox)pBH.Controls[pBH.Controls.Count - 1];

            cboNoiDangKy = UIHelper.AddComboField(pBH, "Nơi đăng ký thẻ", new Point(506, 44), new Size(256, 28), Array.Empty<string>());

            UIHelper.AddFieldPair(pBH, "Địa chỉ đăng ký", "txtDiaChi", new Point(14, 110), new Size(748, 28));
            txtDiaChi = (TextBox)pBH.Controls[pBH.Controls.Count - 1];

            var lblMuc = new Label { Text = "Mức hưởng:", Location = new Point(14, 158), AutoSize = true, Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(80, 100, 130) };
            rb80 = new RadioButton { Text = "80%", Location = new Point(110, 155), AutoSize = true, Font = new Font("Segoe UI", 9f), Checked = true };
            rb95 = new RadioButton { Text = "95%", Location = new Point(175, 155), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            rb100 = new RadioButton { Text = "100%", Location = new Point(240, 155), AutoSize = true, Font = new Font("Segoe UI", 9f) };
            pBH.Controls.AddRange(new Control[] { lblMuc, rb80, rb95, rb100 });

            pBH.Controls.Add(UIHelper.MakeHorizontalLine(new Point(14, 192), pBH.Width - 30));
            var btnLuu = UIHelper.MakePrimaryBtn("💾  Lưu BHYT", new Point(14, 206), new Size(130, 36));
            btnLuu.Click += async (s, e) => await SaveBHAsync();
            pBH.Controls.Add(btnLuu);
            this.Controls.Add(pBH);
        }

        private async Task LoadComboboxesAsync()
        {
            try
            {
                var items = await ApiService.GetNoiDangKyTheAsync();
                if (cboNoiDangKy.InvokeRequired) cboNoiDangKy.Invoke(() => BindCombo(items));
                else BindCombo(items);
            }
            catch { }
        }

        private void BindCombo(List<DanhMucItem> items)
        {
            cboNoiDangKy.DataSource = new List<DanhMucItem>(items);
            cboNoiDangKy.DisplayMember = "Ten";
            cboNoiDangKy.ValueMember = "Id";
            cboNoiDangKy.SelectedIndex = -1;
        }

        private async Task SearchBenhNhanAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return;
            var list = await ApiService.SearchBenhNhanAsync(keyword);
            if (list.Count == 0)
            {
                MessageBox.Show($"Không tìm thấy bệnh nhân với từ khóa: '{keyword}'",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _benhNhanId = list[0].Id;
            txtMaThe.Text = "";
            txtHanSD.Text = "";
            txtDiaChi.Text = "";
            cboNoiDangKy.SelectedIndex = -1;
            rb80.Checked = true;

            try
            {
                var bh = await ApiService.GetBaoHiemAsync(_benhNhanId.Value);
                if (bh != null)
                {
                    txtMaThe.Text = bh.MaThe;
                    txtHanSD.Text = bh.HanSuDung?.ToString("MM/yyyy") ?? "";
                    txtDiaChi.Text = bh.DiaChiDangKy;
                    if (bh.NoiDangKyId.HasValue) try { cboNoiDangKy.SelectedValue = bh.NoiDangKyId.Value; } catch { }
                    rb80.Checked = bh.MucHuong == 80;
                    rb95.Checked = bh.MucHuong == 95;
                    rb100.Checked = bh.MucHuong == 100;
                }
            }
            catch { }

            MessageBox.Show($"✅ Đã chọn: {list[0].HoTen}  |  {list[0].MaBenhNhan}",
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async Task SaveBHAsync()
        {
            if (!_benhNhanId.HasValue)
            {
                MessageBox.Show("Vui lòng tìm bệnh nhân trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtMaThe.Text))
            {
                MessageBox.Show("Vui lòng nhập mã thẻ BHYT!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaThe.Focus(); return;
            }
            if (txtMaThe.Text.Length < 15 || txtMaThe.Text.Length > 16)
            {
                MessageBox.Show("Mã thẻ BHYT phải từ 15 đến 16 ký tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaThe.Focus(); return;
            }
            if (!char.IsLetter(txtMaThe.Text[0]) || !char.IsLetter(txtMaThe.Text[1]))
            {
                MessageBox.Show("2 ký tự đầu mã thẻ phải là chữ cái (VD: HS, HN, TE...)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaThe.Focus(); return;
            }
            if (txtMaThe.Text[2] < '1' || txtMaThe.Text[2] > '5')
            {
                MessageBox.Show("Ký tự thứ 3 của mã thẻ phải là số từ 1 đến 5!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaThe.Focus(); return;
            }
            if (!string.IsNullOrWhiteSpace(txtHanSD.Text) &&
                !DateTime.TryParseExact(txtHanSD.Text, "MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out _))
            {
                MessageBox.Show("Hạn sử dụng phải đúng định dạng MM/yyyy (VD: 01/2029)!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHanSD.Focus(); return;
            }

            int mucHuong = rb95.Checked ? 95 : rb100.Checked ? 100 : 80;
            var req = new BaoHiemRequest
            {
                BenhNhanId = _benhNhanId.Value,
                MaThe = txtMaThe.Text.Trim(),
                HanSuDung = txtHanSD.Text.Trim(),
                NoiDangKyId = cboNoiDangKy.SelectedValue as int?,
                DiaChiDangKy = txtDiaChi.Text.Trim(),
                MucHuong = mucHuong
            };

            var result = await ApiService.SaveBaoHiemAsync(req);
            if (result != null)
            {
                MessageBox.Show("Lưu BHYT thành công!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // ✅ Hiện lỗi chi tiết từ API để debug
                MessageBox.Show(
                    $"Lưu thất bại!\n\n" +
                    $"BenhNhanId: {req.BenhNhanId}\n" +
                    $"MaThe: {req.MaThe}\n" +
                    $"HanSuDung: {req.HanSuDung}\n" +
                    $"NoiDangKyId: {req.NoiDangKyId?.ToString() ?? "null"}\n" +
                    $"MucHuong: {req.MucHuong}\n\n" +
                    $"Kiểm tra Output window (View → Output) để xem lỗi API.",
                    "Lỗi chi tiết", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    // ════════════════════════════════════════════════════════════════
    //  SỬA HỒ SƠ — kế thừa TiepDonKhongBHPanel, ghi đè AllowEdit = true
    // ════════════════════════════════════════════════════════════════
    public class SuaHoSoPanel : TiepDonKhongBHPanel
    {
        // Ghi đè AllowEdit → true: sau khi tìm kiếm, form sẽ được mở khóa để chỉnh sửa
        protected override bool AllowEdit => true;

        protected override void BuildUI()
        {
            // Gọi base để tạo toàn bộ UI
            base.BuildUI();

            // Đổi tiêu đề header thành "Sửa Thông Tin Hồ Sơ"
            if (this.Controls.Count > 0 && this.Controls[0] is Panel pHeader)
            {
                foreach (Control c in pHeader.Controls)
                {
                    if (c is Label lbl && lbl.Font.Bold && lbl.Font.Size >= 12)
                    {
                        lbl.Text = "Sửa Thông Tin Hồ Sơ Bệnh Nhân";
                        break;
                    }
                }
            }

            // Khởi động ở chế độ khóa — bắt buộc phải tìm kiếm trước khi sửa
            SetFormEditable(false);
        }
    }

    // ════════════════════════════════════════════════════════════════
    //  BỆNH NHÂN PANEL
    // ════════════════════════════════════════════════════════════════
    public class BenhNhanPanel : Panel
    {
        private int? _currentBenhNhanId;
        private int? _currentHoSoId;
        private DataGridView dgvSearch = new(), dgvThuoc = new(), dgvDichVu = new();
        private ComboBox cboThuoc = new(), cboDichVu = new();
        private TextBox txtSLThuoc = new(), txtLieuDung = new(), txtSLDV = new(), txtKetLuan = new();
        private Label lblThuocTong = new(), lblDVTong = new(), lblTongCong = new(), lblSelectedBN = new();

        public BenhNhanPanel()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);
            BuildUI();
        }

        private void BuildUI()
        {
            var pHeader = UIHelper.MakePageHeader("Quản Lý Bệnh Nhân", "👤");
            this.Controls.Add(pHeader);

            var tab = new TabControl
            {
                Location = new Point(0, 68),
                Size = new Size(this.Width - 2, 580),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Font = new Font("Segoe UI", 9.5f)
            };
            tab.TabPages.Add(BuildTimKiemTab());
            tab.TabPages.Add(BuildKeThuocTab());
            tab.TabPages.Add(BuildDichVuTab());
            tab.TabPages.Add(BuildKetThucTab());
            this.Controls.Add(tab);
        }

        private TabPage BuildTimKiemTab()
        {
            var tab = new TabPage("🔍  Tìm kiếm") { BackColor = Color.White, Padding = new Padding(10) };
            var lbl = new Label { Text = "🔍", Location = new Point(14, 22), AutoSize = true, Font = new Font("Segoe UI", 11f) };
            var txtS = new TextBox { Location = new Point(40, 20), Size = new Size(380, 28), Font = new Font("Segoe UI", 10f), PlaceholderText = "Tìm theo tên, CCCD, mã BN..." };
            var btnS = UIHelper.MakePrimaryBtn("Tìm kiếm", new Point(430, 18));

            lblSelectedBN.Location = new Point(14, 58);
            lblSelectedBN.AutoSize = true;
            lblSelectedBN.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            lblSelectedBN.ForeColor = UIHelper.Primary;
            lblSelectedBN.Text = "← Tìm và double-click để chọn bệnh nhân";

            dgvSearch = new DataGridView
            {
                Location = new Point(14, 84),
                Size = new Size(860, 360),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9f),
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvSearch.Columns.Add("cID", "ID"); dgvSearch.Columns["cID"].Visible = false;
            dgvSearch.Columns.Add("cMa", "Mã BN");
            dgvSearch.Columns.Add("cHoTen", "Họ tên");
            dgvSearch.Columns.Add("cNgaySinh", "Ngày sinh");
            dgvSearch.Columns.Add("cSDT", "SĐT");
            dgvSearch.Columns.Add("cCCCD", "CCCD");

            btnS.Click += async (s, e) =>
            {
                var list = await ApiService.SearchBenhNhanAsync(txtS.Text);
                dgvSearch.Rows.Clear();
                foreach (var bn in list)
                    dgvSearch.Rows.Add(bn.Id, bn.MaBenhNhan, bn.HoTen,
                        bn.NgaySinh?.ToString("dd/MM/yyyy"), bn.SoDienThoai, bn.Cccd);
            };

            dgvSearch.CellDoubleClick += async (s, e) =>
            {
                if (e.RowIndex < 0) return;
                _currentBenhNhanId = Convert.ToInt32(dgvSearch.Rows[e.RowIndex].Cells["cID"].Value);
                var hoTen = dgvSearch.Rows[e.RowIndex].Cells["cHoTen"].Value?.ToString();
                var maBN = dgvSearch.Rows[e.RowIndex].Cells["cMa"].Value?.ToString();
                lblSelectedBN.Text = $"⏳ Đang tải hồ sơ: {hoTen}  |  {maBN}";
                lblSelectedBN.ForeColor = Color.DarkOrange;
                try
                {
                    var hoSoList = await ApiService.GetHoSoKhamAsync(_currentBenhNhanId.Value);
                    var active = hoSoList.Find(h => h.TrangThai != "Đã khám");
                    if (active != null)
                        _currentHoSoId = active.Id;
                    else
                    {
                        var newHS = await ApiService.CreateHoSoKhamAsync(_currentBenhNhanId.Value, null);
                        if (newHS == null) throw new Exception("API trả về null sau khi tạo hồ sơ.");
                        _currentHoSoId = newHS.Id;
                    }
                    lblSelectedBN.Text = $"✅ Đã chọn: {hoTen}  |  {maBN}  |  Hồ sơ ID: {_currentHoSoId}";
                    lblSelectedBN.ForeColor = UIHelper.Primary;
                }
                catch (HttpRequestException)
                {
                    _currentBenhNhanId = null; _currentHoSoId = null;
                    lblSelectedBN.Text = "❌ Không kết nối được API backend";
                    lblSelectedBN.ForeColor = Color.Red;
                    MessageBox.Show("Không thể kết nối API backend!", "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (TaskCanceledException)
                {
                    _currentBenhNhanId = null; _currentHoSoId = null;
                    lblSelectedBN.Text = "❌ Kết nối timeout";
                    lblSelectedBN.ForeColor = Color.Red;
                    MessageBox.Show("Kết nối tới backend bị timeout.", "Timeout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    _currentBenhNhanId = null; _currentHoSoId = null;
                    lblSelectedBN.Text = $"❌ Lỗi: {ex.Message}";
                    lblSelectedBN.ForeColor = Color.Red;
                    MessageBox.Show($"Lỗi khi tạo hồ sơ khám:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            tab.Controls.AddRange(new Control[] { lbl, txtS, btnS, lblSelectedBN, dgvSearch });
            return tab;
        }

        private TabPage BuildKeThuocTab()
        {
            var tab = new TabPage("💊  Kê thuốc") { BackColor = Color.White, Padding = new Padding(10) };
            UIHelper.AddSectionLabel(tab, "KÊ THUỐC", new Point(14, 10));

            var lCbo = new Label { Text = "Chọn thuốc:", Location = new Point(14, 30), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = UIHelper.LabelFg };
            cboThuoc = new ComboBox { Location = new Point(14, 48), Size = new Size(300, 28), Font = new Font("Segoe UI", 10f), FlatStyle = FlatStyle.Flat, DropDownStyle = ComboBoxStyle.DropDownList };
            var lSL = new Label { Text = "Số lượng:", Location = new Point(322, 30), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = UIHelper.LabelFg };
            txtSLThuoc = new TextBox { Location = new Point(322, 48), Size = new Size(80, 28), Font = new Font("Segoe UI", 10f), PlaceholderText = "SL", Text = "1" };
            var lLD = new Label { Text = "Liều dùng:", Location = new Point(410, 30), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = UIHelper.LabelFg };
            txtLieuDung = new TextBox { Location = new Point(410, 48), Size = new Size(220, 28), Font = new Font("Segoe UI", 10f), PlaceholderText = "VD: Ngày 2 lần, sáng tối" };
            var btnAdd = UIHelper.MakeSuccessBtn("➕ Thêm", new Point(638, 46), new Size(100, 32));
            var btnXoa = UIHelper.MakeDangerBtn("🗑 Xóa dòng", new Point(14, 432), new Size(110, 32));

            dgvThuoc = new DataGridView { Location = new Point(14, 90), Size = new Size(860, 330), BackgroundColor = Color.White, BorderStyle = BorderStyle.None, Font = new Font("Segoe UI", 9f), RowHeadersVisible = false, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            dgvThuoc.Columns.Add("cID", "ID"); dgvThuoc.Columns["cID"].Visible = false;
            dgvThuoc.Columns.Add("cTen", "Tên thuốc"); dgvThuoc.Columns.Add("cSL", "Số lượng");
            dgvThuoc.Columns.Add("cLD", "Liều dùng"); dgvThuoc.Columns.Add("cDG", "Đơn giá"); dgvThuoc.Columns.Add("cTT", "Thành tiền");

            btnAdd.Click += async (s, e) => await AddThuocAsync();
            btnXoa.Click += async (s, e) => await DeleteThuocAsync();
            tab.VisibleChanged += async (s, e) =>
            {
                if (!tab.Visible) return;
                var thuocList = await ApiService.GetDanhMucThuocAsync();
                cboThuoc.DataSource = new List<DanhMucThuoc>(thuocList);
                cboThuoc.DisplayMember = "Ten"; cboThuoc.ValueMember = "Id";
                if (_currentHoSoId.HasValue) await ReloadThuocAsync();
            };
            tab.Controls.AddRange(new Control[] { lCbo, cboThuoc, lSL, txtSLThuoc, lLD, txtLieuDung, btnAdd, dgvThuoc, btnXoa });
            return tab;
        }

        private TabPage BuildDichVuTab()
        {
            var tab = new TabPage("🏷  Dịch vụ") { BackColor = Color.White, Padding = new Padding(10) };
            UIHelper.AddSectionLabel(tab, "SỬ DỤNG DỊCH VỤ", new Point(14, 10));

            var lCbo = new Label { Text = "Chọn dịch vụ:", Location = new Point(14, 30), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = UIHelper.LabelFg };
            cboDichVu = new ComboBox { Location = new Point(14, 48), Size = new Size(320, 28), Font = new Font("Segoe UI", 10f), FlatStyle = FlatStyle.Flat, DropDownStyle = ComboBoxStyle.DropDownList };
            var lSL = new Label { Text = "Số lượng:", Location = new Point(342, 30), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = UIHelper.LabelFg };
            txtSLDV = new TextBox { Location = new Point(342, 48), Size = new Size(80, 28), Font = new Font("Segoe UI", 10f), Text = "1" };
            var btnAdd = UIHelper.MakeSuccessBtn("➕ Thêm", new Point(430, 46), new Size(100, 32));
            var btnXoa = UIHelper.MakeDangerBtn("🗑 Xóa dòng", new Point(14, 432), new Size(110, 32));

            dgvDichVu = new DataGridView { Location = new Point(14, 90), Size = new Size(860, 330), BackgroundColor = Color.White, BorderStyle = BorderStyle.None, Font = new Font("Segoe UI", 9f), RowHeadersVisible = false, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
            dgvDichVu.Columns.Add("cID", "ID"); dgvDichVu.Columns["cID"].Visible = false;
            dgvDichVu.Columns.Add("cTen", "Dịch vụ"); dgvDichVu.Columns.Add("cSL", "Số lượng");
            dgvDichVu.Columns.Add("cDG", "Đơn giá"); dgvDichVu.Columns.Add("cTT", "Thành tiền"); dgvDichVu.Columns.Add("cTS", "Trạng thái");

            btnAdd.Click += async (s, e) => await AddDichVuAsync();
            btnXoa.Click += async (s, e) => await DeleteDichVuAsync();
            tab.VisibleChanged += async (s, e) =>
            {
                if (!tab.Visible) return;
                var dvList = await ApiService.GetDanhMucDichVuAsync();
                cboDichVu.DataSource = new List<DanhMucDichVu>(dvList);
                cboDichVu.DisplayMember = "Ten"; cboDichVu.ValueMember = "Id";
                if (_currentHoSoId.HasValue) await ReloadDichVuAsync();
            };
            tab.Controls.AddRange(new Control[] { lCbo, cboDichVu, lSL, txtSLDV, btnAdd, dgvDichVu, btnXoa });
            return tab;
        }

        private TabPage BuildKetThucTab()
        {
            var tab = new TabPage("✅  Kết thúc khám") { BackColor = Color.White, Padding = new Padding(10) };
            UIHelper.AddSectionLabel(tab, "KẾT THÚC KHÁM BỆNH", new Point(14, 10));

            var lKL = new Label { Text = "Kết luận / Chẩn đoán cuối:", Location = new Point(14, 30), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = UIHelper.LabelFg };
            txtKetLuan = new TextBox { Location = new Point(14, 48), Size = new Size(760, 70), Font = new Font("Segoe UI", 10f), Multiline = true, PlaceholderText = "Nhập kết luận và hướng điều trị..." };

            var cboHT = UIHelper.AddComboField(tab, "Hình thức kết thúc", new Point(14, 138), new Size(200, 28), new[] { "Ra viện", "Chuyển viện", "Tử vong", "Trốn viện" });
            var lNgay = new Label { Text = "Ngày ra viện:", Location = new Point(230, 118), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = UIHelper.LabelFg };
            var txtNgayRa = new TextBox { Location = new Point(230, 138), Size = new Size(160, 28), Font = new Font("Segoe UI", 10f), Text = DateTime.Today.ToString("dd/MM/yyyy") };
            tab.Controls.Add(lNgay); tab.Controls.Add(txtNgayRa);

            var pTong = new Panel { Location = new Point(14, 186), Size = new Size(420, 120), BackColor = Color.FromArgb(235, 245, 255), BorderStyle = BorderStyle.FixedSingle };
            var lTD = new Label { Text = "TỔNG CHI PHÍ", Location = new Point(12, 10), AutoSize = true, Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = UIHelper.Primary };
            lblThuocTong.Text = "Tiền thuốc:     —"; lblThuocTong.Location = new Point(12, 34); lblThuocTong.AutoSize = true; lblThuocTong.Font = new Font("Segoe UI", 9f);
            lblDVTong.Text = "Tiền dịch vụ:  —"; lblDVTong.Location = new Point(12, 56); lblDVTong.AutoSize = true; lblDVTong.Font = new Font("Segoe UI", 9f);
            lblTongCong.Text = "TỔNG CỘNG:  —"; lblTongCong.Location = new Point(12, 82); lblTongCong.AutoSize = true; lblTongCong.Font = new Font("Segoe UI", 11f, FontStyle.Bold); lblTongCong.ForeColor = Color.DarkRed;
            pTong.Controls.AddRange(new Control[] { lTD, lblThuocTong, lblDVTong, lblTongCong });

            var btnTinhTong = UIHelper.MakeSecondaryBtn("🔄 Tính tổng", new Point(14, 322), new Size(120, 34));
            var btnKT = UIHelper.MakeDangerBtn("✅  Xác nhận kết thúc khám", new Point(144, 322), new Size(210, 40));
            btnTinhTong.Click += async (s, e) => await TinhTongAsync();
            btnKT.Click += async (s, e) =>
            {
                if (!_currentHoSoId.HasValue) { MessageBox.Show("Chưa chọn bệnh nhân!\nVui lòng vào tab Tìm kiếm và double-click chọn bệnh nhân.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                if (MessageBox.Show("Xác nhận kết thúc khám?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                var req = new KetThucKhamRequest { HoSoKhamId = _currentHoSoId.Value, KetLuan = txtKetLuan.Text.Trim(), HinhThucKetThuc = cboHT.SelectedItem?.ToString() ?? "Ra viện", NgayRa = txtNgayRa.Text };
                var ok = await ApiService.KetThucKhamAsync(req);
                if (ok)
                {
                    MessageBox.Show("Kết thúc khám thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _currentHoSoId = null; _currentBenhNhanId = null;
                    lblSelectedBN.Text = "← Tìm và double-click để chọn bệnh nhân mới";
                    lblSelectedBN.ForeColor = UIHelper.Primary;
                }
                else MessageBox.Show("Kết thúc thất bại!\nKiểm tra kết nối API backend.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            tab.Controls.AddRange(new Control[] { lKL, txtKetLuan, pTong, btnTinhTong, btnKT });
            return tab;
        }

        private async Task TinhTongAsync()
        {
            if (!_currentHoSoId.HasValue) { MessageBox.Show("Chưa chọn bệnh nhân!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            var thuocList = await ApiService.GetKeThuocAsync(_currentHoSoId.Value);
            var dvList = await ApiService.GetDichVuAsync(_currentHoSoId.Value);
            decimal tThuoc = 0, tDV = 0;
            foreach (var t in thuocList) tThuoc += t.ThanhTien;
            foreach (var d in dvList) tDV += d.ThanhTien;
            lblThuocTong.Text = $"Tiền thuốc:     {tThuoc:N0} đ";
            lblDVTong.Text = $"Tiền dịch vụ:  {tDV:N0} đ";
            lblTongCong.Text = $"TỔNG CỘNG:  {(tThuoc + tDV):N0} đ";
        }

        private async Task AddThuocAsync()
        {
            if (!_currentHoSoId.HasValue) { MessageBox.Show("Vui lòng vào tab Tìm kiếm và double-click chọn bệnh nhân trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (cboThuoc.SelectedValue is not int thuocId) return;
            if (!int.TryParse(txtSLThuoc.Text, out int sl) || sl <= 0) { MessageBox.Show("Số lượng không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            var thuocList = (List<DanhMucThuoc>)cboThuoc.DataSource;
            var thuoc = thuocList.Find(t => t.Id == thuocId);
            var result = await ApiService.AddKeThuocAsync(new KeThuocRequest { HoSoKhamId = _currentHoSoId.Value, ThuocId = thuocId, SoLuong = sl, LieuDung = txtLieuDung.Text, DonGia = thuoc?.DonGia ?? 0 });
            if (result != null) { await ReloadThuocAsync(); txtSLThuoc.Text = "1"; txtLieuDung.Text = ""; }
        }

        private async Task ReloadThuocAsync()
        {
            if (!_currentHoSoId.HasValue) return;
            var list = await ApiService.GetKeThuocAsync(_currentHoSoId.Value);
            if (dgvThuoc.InvokeRequired) { dgvThuoc.Invoke(() => FillThuocGrid(list)); return; }
            FillThuocGrid(list);
        }

        private void FillThuocGrid(List<KeThuocItem> list)
        {
            dgvThuoc.Rows.Clear();
            foreach (var t in list) dgvThuoc.Rows.Add(t.Id, t.TenThuoc, t.SoLuong, t.LieuDung, $"{t.DonGia:N0}đ", $"{t.ThanhTien:N0}đ");
        }

        private async Task DeleteThuocAsync()
        {
            if (dgvThuoc.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Xóa thuốc này?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            int id = Convert.ToInt32(dgvThuoc.SelectedRows[0].Cells["cID"].Value);
            await ApiService.DeleteKeThuocAsync(id);
            await ReloadThuocAsync();
        }

        private async Task AddDichVuAsync()
        {
            if (!_currentHoSoId.HasValue) { MessageBox.Show("Vui lòng vào tab Tìm kiếm và double-click chọn bệnh nhân trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (cboDichVu.SelectedValue is not int dvId) return;
            if (!int.TryParse(txtSLDV.Text, out int sl) || sl <= 0) sl = 1;
            var dvList = (List<DanhMucDichVu>)cboDichVu.DataSource;
            var dv = dvList.Find(d => d.Id == dvId);
            var result = await ApiService.AddDichVuAsync(new DichVuRequest { HoSoKhamId = _currentHoSoId.Value, DichVuId = dvId, SoLuong = sl, DonGia = dv?.DonGia ?? 0 });
            if (result != null) { await ReloadDichVuAsync(); txtSLDV.Text = "1"; }
        }

        private async Task ReloadDichVuAsync()
        {
            if (!_currentHoSoId.HasValue) return;
            var list = await ApiService.GetDichVuAsync(_currentHoSoId.Value);
            if (dgvDichVu.InvokeRequired) { dgvDichVu.Invoke(() => FillDVGrid(list)); return; }
            FillDVGrid(list);
        }

        private void FillDVGrid(List<DichVuItem> list)
        {
            dgvDichVu.Rows.Clear();
            foreach (var d in list) dgvDichVu.Rows.Add(d.Id, d.TenDichVu, d.SoLuong, $"{d.DonGia:N0}đ", $"{d.ThanhTien:N0}đ", d.TrangThai);
        }

        private async Task DeleteDichVuAsync()
        {
            if (dgvDichVu.SelectedRows.Count == 0) return;
            if (MessageBox.Show("Xóa dịch vụ này?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            int id = Convert.ToInt32(dgvDichVu.SelectedRows[0].Cells["cID"].Value);
            await ApiService.DeleteDichVuAsync(id);
            await ReloadDichVuAsync();
        }
    }

    // ════════════════════════════════════════════════════════════════
    //  NGƯỜI DÙNG
    // ════════════════════════════════════════════════════════════════
    public class NguoiDungPanel : Panel
    {
        private int? _selectedUserId;
        private List<UserInfo> _userList = [];
        private DataGridView dgv = new();
        private TextBox txtID = new(), txtHoTen = new(), txtTenDN = new(),
                        txtEmail = new(), txtMK = new(), txtMKMoi = new();
        private ComboBox cboVaiTro = new();
        private Label lblInfo = new();

        public NguoiDungPanel()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);
            BuildUI();
            _ = LoadAsync();
        }

        private void BuildUI()
        {
            var pHeader = UIHelper.MakePageHeader(AppSession.IsAdmin ? "Quản Lý Người Dùng" : "Thông Tin Tài Khoản", "👥");
            this.Controls.Add(pHeader);
            if (AppSession.IsAdmin) BuildAdminUI();
            else BuildUserUI();
        }

        private void BuildAdminUI()
        {
            var pSearch = UIHelper.MakeCard(new Point(0, 68), new Size(this.Width - 2, 60));
            pSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            var txtSearch = new TextBox { Location = new Point(14, 16), Size = new Size(320, 28), Font = new Font("Segoe UI", 10f), BorderStyle = BorderStyle.FixedSingle, PlaceholderText = "Tìm theo tên đăng nhập, email..." };
            var btnSearch = UIHelper.MakePrimaryBtn("🔍 Tìm", new Point(344, 12), new Size(90, 34));
            var btnRefresh = UIHelper.MakeSecondaryBtn("🔄 Tải lại", new Point(444, 12), new Size(100, 34));
            var btnNew = UIHelper.MakeSuccessBtn("➕ Thêm mới", new Point(554, 12), new Size(110, 34));
            btnSearch.Click += (s, e) => FilterGrid(txtSearch.Text);
            btnRefresh.Click += async (s, e) => await LoadAsync();
            btnNew.Click += (s, e) => ClearForm();
            pSearch.Controls.AddRange(new Control[] { txtSearch, btnSearch, btnRefresh, btnNew });
            this.Controls.Add(pSearch);

            var pGrid = UIHelper.MakeCard(new Point(0, 140), new Size(this.Width - 2, 260));
            pGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            UIHelper.AddSectionLabel(pGrid, "DANH SÁCH TÀI KHOẢN", new Point(14, 10));
            dgv = new DataGridView { Location = new Point(14, 38), Size = new Size(pGrid.Width - 30, 210), Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right, BackgroundColor = Color.White, BorderStyle = BorderStyle.None, Font = new Font("Segoe UI", 9f), RowHeadersVisible = false, AllowUserToAddRows = false, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false };
            dgv.Columns.Add("cID", "ID"); dgv.Columns["cID"].Visible = false;
            dgv.Columns.Add("cTenDN", "Tên đăng nhập"); dgv.Columns.Add("cEmail", "Email"); dgv.Columns.Add("cVaiTro", "Vai trò"); dgv.Columns.Add("cHoTen", "Họ và tên");
            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.SelectedRows.Count == 0) return;
                var row = dgv.SelectedRows[0];
                _selectedUserId = Convert.ToInt32(row.Cells["cID"].Value);
                txtID.Text = _selectedUserId.ToString();
                txtTenDN.Text = row.Cells["cTenDN"].Value?.ToString() ?? "";
                txtEmail.Text = row.Cells["cEmail"].Value?.ToString() ?? "";
                txtHoTen.Text = row.Cells["cHoTen"].Value?.ToString() ?? "";
                cboVaiTro.SelectedItem = row.Cells["cVaiTro"].Value?.ToString() ?? "user";
                txtMK.Clear(); txtMKMoi.Clear(); lblInfo.Text = "";
            };
            pGrid.Controls.Add(dgv);
            this.Controls.Add(pGrid);

            var pEdit = UIHelper.MakeCard(new Point(0, 412), new Size(this.Width - 2, 210));
            pEdit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            UIHelper.AddSectionLabel(pEdit, "THÊM / CHỈNH SỬA TÀI KHOẢN", new Point(14, 10));
            UIHelper.AddFieldPair(pEdit, "ID (tự sinh)", "txtID", new Point(14, 44), new Size(60, 28));
            txtID = (TextBox)pEdit.Controls[pEdit.Controls.Count - 1]; txtID.ReadOnly = true; txtID.BackColor = Color.FromArgb(240, 240, 240);
            UIHelper.AddFieldPair(pEdit, "Tên đăng nhập *", "txtTenDN", new Point(90, 44), new Size(200, 28));
            txtTenDN = (TextBox)pEdit.Controls[pEdit.Controls.Count - 1];
            UIHelper.AddFieldPair(pEdit, "Email", "txtEmail", new Point(306, 44), new Size(240, 28));
            txtEmail = (TextBox)pEdit.Controls[pEdit.Controls.Count - 1];
            UIHelper.AddFieldPair(pEdit, "Họ và tên", "txtHoTen", new Point(562, 44), new Size(200, 28));
            txtHoTen = (TextBox)pEdit.Controls[pEdit.Controls.Count - 1];
            cboVaiTro = UIHelper.AddComboField(pEdit, "Vai trò", new Point(14, 108), new Size(150, 28), new[] { "admin", "user", "doctor", "nurse", "receptionist" });
            UIHelper.AddFieldPair(pEdit, "Mật khẩu mới", "txtMKMoi", new Point(180, 108), new Size(200, 28), "");
            txtMKMoi = (TextBox)pEdit.Controls[pEdit.Controls.Count - 1]; txtMKMoi.UseSystemPasswordChar = true; txtMKMoi.PlaceholderText = "Để trống nếu không đổi";
            var btnLuu = UIHelper.MakePrimaryBtn("💾 Lưu", new Point(14, 158), new Size(100, 34));
            var btnXoa = UIHelper.MakeDangerBtn("🗑 Xóa", new Point(124, 158), new Size(100, 34));
            var btnClear = UIHelper.MakeSecondaryBtn("✖ Bỏ chọn", new Point(234, 158), new Size(110, 34));
            btnLuu.Click += async (s, e) => await AdminSaveAsync();
            btnXoa.Click += async (s, e) => await AdminDeleteAsync();
            btnClear.Click += (s, e) => ClearForm();
            lblInfo.Location = new Point(14, 200); lblInfo.AutoSize = true; lblInfo.Font = new Font("Segoe UI", 9f);
            pEdit.Controls.AddRange(new Control[] { btnLuu, btnXoa, btnClear, lblInfo });
            this.Controls.Add(pEdit);
        }

        private void BuildUserUI()
        {
            var pCard = UIHelper.MakeCard(new Point(0, 68), new Size(600, 280));
            UIHelper.AddSectionLabel(pCard, "THÔNG TIN TÀI KHOẢN CỦA BẠN", new Point(14, 10));
            UIHelper.AddFieldPair(pCard, "Tên đăng nhập", "txtTenDN", new Point(14, 44), new Size(220, 28));
            txtTenDN = (TextBox)pCard.Controls[pCard.Controls.Count - 1]; txtTenDN.ReadOnly = true; txtTenDN.BackColor = Color.FromArgb(240, 240, 240); txtTenDN.Text = AppSession.Username;
            UIHelper.AddFieldPair(pCard, "Email", "txtEmail", new Point(254, 44), new Size(300, 28));
            txtEmail = (TextBox)pCard.Controls[pCard.Controls.Count - 1];
            UIHelper.AddFieldPair(pCard, "Họ và tên", "txtHoTen", new Point(14, 110), new Size(300, 28));
            txtHoTen = (TextBox)pCard.Controls[pCard.Controls.Count - 1];
            pCard.Controls.Add(UIHelper.MakeHorizontalLine(new Point(14, 160), 540));
            var lblMKCu = new Label { Text = "Mật khẩu hiện tại *", Location = new Point(14, 175), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = UIHelper.LabelFg };
            txtMK = new TextBox { Location = new Point(14, 193), Size = new Size(220, 28), Font = new Font("Segoe UI", 10f), UseSystemPasswordChar = true, PlaceholderText = "Nhập mật khẩu hiện tại..." };
            var lblMKMoi = new Label { Text = "Mật khẩu mới", Location = new Point(254, 175), AutoSize = true, Font = new Font("Segoe UI", 8.5f), ForeColor = UIHelper.LabelFg };
            txtMKMoi = new TextBox { Location = new Point(254, 193), Size = new Size(220, 28), Font = new Font("Segoe UI", 10f), UseSystemPasswordChar = true, PlaceholderText = "Để trống nếu không đổi..." };
            var btnLuu = UIHelper.MakePrimaryBtn("💾 Cập nhật thông tin", new Point(14, 232), new Size(180, 36));
            btnLuu.Click += async (s, e) => await UserSaveAsync();
            lblInfo.Location = new Point(14, 240); lblInfo.AutoSize = true; lblInfo.Font = new Font("Segoe UI", 9f);
            pCard.Controls.AddRange(new Control[] { lblMKCu, txtMK, lblMKMoi, txtMKMoi, btnLuu, lblInfo });
            this.Controls.Add(pCard);
        }

        private async Task LoadAsync()
        {
            if (AppSession.IsAdmin) { _userList = await ApiService.GetAllUsersAsync(); FillGrid(_userList); ClearForm(); }
            else { var me = await ApiService.GetUserByIdAsync(AppSession.Id); if (me != null) { txtEmail.Text = me.Email; txtHoTen.Text = me.Username; } }
        }

        private void FillGrid(List<UserInfo> list)
        {
            if (dgv.InvokeRequired) { dgv.Invoke(() => FillGrid(list)); return; }
            dgv.Rows.Clear();
            foreach (var u in list) dgv.Rows.Add(u.Id, u.Username, u.Email, u.Role, "");
        }

        private void FilterGrid(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) { FillGrid(_userList); return; }
            FillGrid(_userList.FindAll(u =>
                u.Username.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                u.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
        }

        private void ClearForm()
        {
            _selectedUserId = null;
            txtID.Text = ""; txtTenDN.Text = ""; txtEmail.Text = ""; txtHoTen.Text = ""; txtMKMoi.Text = "";
            cboVaiTro.SelectedIndex = 1; lblInfo.Text = ""; dgv.ClearSelection();
        }

        private async Task AdminSaveAsync()
        {
            if (string.IsNullOrWhiteSpace(txtTenDN.Text)) { SetInfo("Vui lòng nhập tên đăng nhập!", Color.Red); return; }
            var userPayload = new UserSaveRequest { Id = _selectedUserId ?? 0, Username = txtTenDN.Text.Trim(), Email = txtEmail.Text.Trim(), Role = cboVaiTro.SelectedItem?.ToString() ?? "user", Password = txtMKMoi.Text.Trim() };
            bool ok;
            if (_selectedUserId.HasValue) { ok = await ApiService.UpdateUserAsync(_selectedUserId.Value, userPayload); SetInfo(ok ? "✅ Cập nhật thành công!" : "❌ Cập nhật thất bại!", ok ? Color.Green : Color.Red); }
            else
            {
                if (string.IsNullOrWhiteSpace(txtMKMoi.Text)) { SetInfo("Vui lòng nhập mật khẩu cho tài khoản mới!", Color.Red); return; }
                ok = await ApiService.CreateUserAsync(userPayload);
                SetInfo(ok ? "✅ Tạo tài khoản thành công!" : "❌ Tạo tài khoản thất bại!", ok ? Color.Green : Color.Red);
            }
            if (ok) await LoadAsync();
        }

        private async Task AdminDeleteAsync()
        {
            if (!_selectedUserId.HasValue) { SetInfo("Vui lòng chọn tài khoản cần xóa!", Color.Orange); return; }
            if (_selectedUserId == AppSession.Id) { SetInfo("Không thể xóa tài khoản đang đăng nhập!", Color.Red); return; }
            if (MessageBox.Show($"Xác nhận xóa tài khoản '{txtTenDN.Text}'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            var ok = await ApiService.DeleteUserAsync(_selectedUserId.Value);
            SetInfo(ok ? "✅ Đã xóa tài khoản!" : "❌ Xóa thất bại!", ok ? Color.Green : Color.Red);
            if (ok) await LoadAsync();
        }

        private async Task UserSaveAsync()
        {
            if (string.IsNullOrWhiteSpace(txtMK.Text)) { SetInfo("Vui lòng nhập mật khẩu hiện tại để xác nhận!", Color.Red); return; }
            var payload = new UserSaveRequest { Id = AppSession.Id, Username = AppSession.Username, Email = txtEmail.Text.Trim(), Role = AppSession.Role, Password = txtMKMoi.Text.Trim(), OldPassword = txtMK.Text.Trim() };
            var ok = await ApiService.UpdateUserAsync(AppSession.Id, payload);
            SetInfo(ok ? "✅ Cập nhật thành công!" : "❌ Cập nhật thất bại. Kiểm tra mật khẩu hiện tại!", ok ? Color.Green : Color.Red);
            if (ok) txtMK.Clear();
        }

        private void SetInfo(string msg, Color color)
        {
            if (lblInfo.InvokeRequired) { lblInfo.Invoke(() => SetInfo(msg, color)); return; }
            lblInfo.Text = msg; lblInfo.ForeColor = color;
        }
    }
}