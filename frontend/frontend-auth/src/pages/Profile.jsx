import React, { useEffect, useState } from "react";
import { getProfile, updateProfile, uploadAvatar } from "../services/api";

export default function Profile() {
  const [user, setUser] = useState(null);
  const [form, setForm] = useState({ name: "", email: "" });
  const [file, setFile] = useState(null);
  const [message, setMessage] = useState("");

  // 📌 Lấy thông tin người dùng khi mở trang
  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const res = await getProfile();
        setUser(res.data);
        setForm({ name: res.data.name, email: res.data.email });
      } catch (err) {
        console.error("Lỗi tải thông tin:", err);
        setMessage("⚠️ Token hết hạn, vui lòng đăng nhập lại!");
        setTimeout(() => {
          localStorage.clear();
          window.location.href = "/login";
        }, 1500);
      }
    };
    fetchProfile();
  }, []);

  // 📌 Cập nhật thông tin cá nhân
  const handleUpdate = async (e) => {
    e.preventDefault();
    try {
      const res = await updateProfile(form);
      setMessage(res.data.message || "✅ Cập nhật thành công!");
      const refreshed = await getProfile();
      setUser(refreshed.data);
    } catch (err) {
      console.error(err);
      setMessage("❌ Lỗi cập nhật!");
    }
  };

  // 📌 Upload ảnh đại diện
  const handleUpload = async (e) => {
    e.preventDefault();
    if (!file) return setMessage("⚠️ Vui lòng chọn ảnh!");
    const formData = new FormData();
    formData.append("avatar", file);
    try {
      await uploadAvatar(formData);
      setMessage("✅ Avatar đã được cập nhật!");
      const refreshed = await getProfile();
      setUser(refreshed.data);
    } catch (err) {
      console.error(err);
      setMessage("❌ Lỗi upload ảnh!");
    }
  };

  // 📌 Đăng xuất
  const handleLogout = () => {
    localStorage.clear();
    window.location.href = "/login";
  };

  // 📌 Giao diện
  return (
    <div
      style={{
        maxWidth: 420,
        margin: "40px auto",
        textAlign: "center",
        fontFamily: "Arial, sans-serif",
        border: "1px solid #ccc",
        borderRadius: "10px",
        padding: "20px",
      }}
    >
      <h2>👤 Hồ sơ cá nhân</h2>

      {user ? (
        <>
          {/* Ảnh đại diện */}
          <div style={{ marginBottom: 20 }}>
            <img
              src={user.avatar || "/default-avatar.png"}
              alt="Avatar"
              style={{
                width: "120px",
                height: "120px",
                borderRadius: "50%",
                objectFit: "cover",
                border: "3px solid #ddd",
              }}
            />
          </div>

          {/* Upload avatar */}
          <form onSubmit={handleUpload} style={{ marginBottom: 20 }}>
            <input
              type="file"
              accept="image/*"
              onChange={(e) => setFile(e.target.files[0])}
/>
            <button type="submit" style={{ marginLeft: 10 }}>
              Tải lên Avatar
            </button>
          </form>

          {/* Cập nhật thông tin */}
          <form
            onSubmit={handleUpdate}
            style={{
              display: "flex",
              flexDirection: "column",
              gap: 10,
              alignItems: "center",
            }}
          >
            <input
              name="name"
              value={form.name}
              onChange={(e) => setForm({ ...form, name: e.target.value })}
              placeholder="Họ và tên"
              required
              style={{ width: "80%", padding: "6px" }}
            />
            <input
              name="email"
              value={form.email}
              onChange={(e) => setForm({ ...form, email: e.target.value })}
              placeholder="Email"
              required
              style={{ width: "80%", padding: "6px" }}
            />
            <button type="submit">Cập nhật</button>
          </form>

          {/* Nút đăng xuất */}
          <button
            onClick={handleLogout}
            style={{
              marginTop: 20,
              backgroundColor: "red",
              color: "white",
              border: "none",
              padding: "6px 12px",
              cursor: "pointer",
              borderRadius: 4,
            }}
          >
            Đăng xuất
          </button>

          {/* Thông báo */}
          {message && (
            <p
              style={{
                marginTop: 15,
                color: message.includes("✅") ? "green" : "red",
                fontWeight: "bold",
              }}
            >
              {message}
            </p>
          )}
        </>
      ) : (
        <p>⏳ Đang tải thông tin...</p>
      )}
    </div>
  );
}
