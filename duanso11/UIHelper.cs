using System;
using System.Drawing;
using System.Windows.Forms;

namespace duanso11
{
    /// <summary>
    /// Shared UI factory helpers — keeps all panels consistent.
    /// </summary>
    public static class UIHelper
    {
        // ── Colour palette ──────────────────────────────────────────
        public static readonly Color Primary = Color.FromArgb(15, 82, 186);
        public static readonly Color Success = Color.FromArgb(22, 163, 74);
        public static readonly Color Danger = Color.FromArgb(220, 38, 38);
        public static readonly Color Secondary = Color.FromArgb(100, 116, 139);
        public static readonly Color CardBg = Color.White;
        public static readonly Color LabelFg = Color.FromArgb(60, 80, 110);
        public static readonly Color SectionFg = Color.FromArgb(15, 82, 186);
        public static readonly Color BorderClr = Color.FromArgb(218, 228, 240);

        // ── Page header ─────────────────────────────────────────────
        public static Panel MakePageHeader(string title, string icon = "")
        {
            var pnl = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(2000, 58),
                BackColor = Color.White
            };

            // Left accent bar
            var accent = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(5, 58),
                BackColor = Primary
            };

            var lbl = new Label
            {
                Text = (icon.Length > 0 ? icon + "  " : "") + title,
                Location = new Point(20, 10),
                AutoSize = true,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 40, 80)
            };

            var sep = new Panel
            {
                Location = new Point(0, 57),
                Size = new Size(2000, 1),
                BackColor = BorderClr
            };

            pnl.Controls.AddRange(new Control[] { accent, lbl, sep });
            return pnl;
        }

        // ── Card (white rounded box) ─────────────────────────────────
        public static Panel MakeCard(Point location, Size size)
        {
            var pnl = new Panel
            {
                Location = location,
                Size = size,
                BackColor = CardBg,
                Padding = new Padding(14, 10, 14, 10)
            };
            pnl.Paint += (s, e) =>
            {
                using var pen = new Pen(BorderClr, 1);
                e.Graphics.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            };
            return pnl;
        }

        // ── Section label ────────────────────────────────────────────
        public static void AddSectionLabel(Control parent, string text, Point loc)
        {
            parent.Controls.Add(new Label
            {
                Text = text,
                Location = loc,
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                ForeColor = SectionFg
            });
        }

        // ── Label + TextBox pair ─────────────────────────────────────
        public static void AddFieldPair(Control parent, string labelText, string name, Point loc, Size size, string placeholder = "")
        {
            var lbl = new Label
            {
                Text = labelText,
                Location = new Point(loc.X, loc.Y - 18),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = LabelFg
            };

            var txt = new TextBox
            {
                Name = name,
                Location = loc,
                Size = size,
                Font = new Font("Segoe UI", 10f),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(250, 252, 255)
            };
            if (placeholder.Length > 0) txt.PlaceholderText = placeholder;

            parent.Controls.Add(lbl);
            parent.Controls.Add(txt);
        }

        // ── Label + ComboBox pair ────────────────────────────────────
        public static ComboBox AddComboField(Control parent, string labelText, Point loc, Size size, string[] items)
        {
            var lbl = new Label
            {
                Text = labelText,
                Location = new Point(loc.X, loc.Y - 18),
                AutoSize = true,
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = LabelFg
            };

            var cb = new ComboBox
            {
                Location = loc,
                Size = size,
                Font = new Font("Segoe UI", 10f),
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(250, 252, 255)
            };
            cb.Items.AddRange(items);
            if (items.Length > 0) cb.SelectedIndex = 0;

            parent.Controls.Add(lbl);
            parent.Controls.Add(cb);
            return cb;
        }

        // ── Horizontal divider ────────────────────────────────────────
        public static Panel MakeHorizontalLine(Point loc, int width)
        {
            return new Panel
            {
                Location = loc,
                Size = new Size(width, 1),
                BackColor = BorderClr
            };
        }

        // ── Button variants ───────────────────────────────────────────
        public static Button MakePrimaryBtn(string text, Point loc, Size? size = null)
            => MakeBtn(text, loc, size ?? new Size(100, 34), Primary, Color.White);

        public static Button MakeSuccessBtn(string text, Point loc, Size? size = null)
            => MakeBtn(text, loc, size ?? new Size(130, 34), Success, Color.White);

        public static Button MakeDangerBtn(string text, Point loc, Size? size = null)
            => MakeBtn(text, loc, size ?? new Size(100, 34), Danger, Color.White);

        public static Button MakeSecondaryBtn(string text, Point loc, Size? size = null)
            => MakeBtn(text, loc, size ?? new Size(80, 34), Secondary, Color.White);

        private static Button MakeBtn(string text, Point loc, Size size, Color bg, Color fg)
        {
            var btn = new Button
            {
                Text = text,
                Location = loc,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                BackColor = bg,
                ForeColor = fg,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 0;

            // Hover effect
            btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Dark(bg, 0.1f);
            btn.MouseLeave += (s, e) => btn.BackColor = bg;
            return btn;
        }
    }
}