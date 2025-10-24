import { useEffect, useState } from "react";
import { getProfile, updateProfile } from "../services/api";

export default function Profile() {
  const [user, setUser] = useState(null);
  const [form, setForm] = useState({ name: "", email: "" });
  const [message, setMessage] = useState("");

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const res = await getProfile();
        setUser(res.data);
        setForm({ name: res.data.name, email: res.data.email });
        localStorage.setItem("user", JSON.stringify(res.data));
      } catch {
        setMessage("❌ Lỗi tải thông tin người dùng! Hãy đăng nhập lại.");
      }
    };
    fetchProfile();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await updateProfile(form);
      setMessage("✅ Cập nhật thông tin thành công!");
      setUser(res.data);
      localStorage.setItem("user", JSON.stringify(res.data));
    } catch {
      setMessage("❌ Lỗi khi cập nhật thông tin!");
    }
  };

  if (!user)
    return (
      <p style={{ textAlign: "center", marginTop: "40px" }}>
        ⏳ Đang tải thông tin...
      </p>
    );

  return (
    <div
      style={{
        maxWidth: "500px",
        margin: "60px auto",
        backgroundColor: "#f0f4ff",
        padding: "30px",
        borderRadius: "15px",
        boxShadow: "0 0 10px rgba(0,0,0,0.1)",
        textAlign: "center",
        fontFamily: "Arial, sans-serif",
      }}
    >
      <h2 style={{ color: "#2b3a67", marginBottom: "20px" }}>🧍‍♂️ Hồ sơ cá nhân</h2>

      {/* ✅ Avatar */}
      <div style={{ marginBottom: "15px" }}>
        <img
          src={user.avatar || "https://via.placeholder.com/120?text=No+Avatar"}
          alt="Avatar"
          width="120"
          height="120"
          style={{
            borderRadius: "50%",
            objectFit: "cover",
            border: "3px solid #3b82f6",
            boxShadow: "0 0 6px rgba(0,0,0,0.15)",
          }}
        />
        <p style={{ marginTop: "8px" }}>
          <a
            href="/upload-avatar"
            style={{
              color: "#1d4ed8",
              textDecoration: "underline",
              fontSize: "14px",
            }}
          >
            ✏️ Thay đổi ảnh đại diện
          </a>
        </p>
      </div>

      {/* ✅ Thông tin */}
      <div style={{ marginBottom: "20px" }}>
        <h3 style={{ margin: "8px 0", color: "#111827" }}>{user.name}</h3>
        <p style={{ margin: "4px 0", color: "#555" }}>{user.email}</p>
        <p style={{ margin: "4px 0", color: "#333" }}>
          <strong>Vai trò:</strong>{" "}
          {typeof user.role === "object" ? user.role.name : user.role}
        </p>
      </div>

      {/* ✅ Form cập nhật */}
      <form
        onSubmit={handleSubmit}
        style={{
          display: "flex",
          flexDirection: "column",
          gap: "10px",
          alignItems: "center",
        }}
      >
        <input
          name="name"
          value={form.name}
          onChange={(e) => setForm({ ...form, name: e.target.value })}
          placeholder="Tên"
          required
          style={{
            width: "80%",
            padding: "10px",
            borderRadius: "8px",
            border: "1px solid #ccc",
            fontSize: "16px",
          }}
        />
        <input
          name="email"
          value={form.email}
          onChange={(e) => setForm({ ...form, email: e.target.value })}
          placeholder="Email"
          required
          style={{
            width: "80%",
            padding: "10px",
            borderRadius: "8px",
            border: "1px solid #ccc",
            fontSize: "16px",
          }}
        />
        <button
          type="submit"
          style={{
            width: "84%",
            padding: "10px",
            backgroundColor: "#3b82f6",
            color: "white",
            border: "none",
            borderRadius: "8px",
            cursor: "pointer",
            fontSize: "16px",
            transition: "0.2s",
          }}
          onMouseOver={(e) => (e.target.style.backgroundColor = "#2563eb")}
          onMouseOut={(e) => (e.target.style.backgroundColor = "#3b82f6")}
        >
          💾 Cập nhật
        </button>
      </form>

      {/* ✅ Thông báo */}
      {message && (
        <p
          style={{
            marginTop: "15px",
            color: message.includes("✅") ? "green" : "red",
            fontWeight: "bold",
          }}
        >
          {message}
        </p>
      )}
    </div>
  );
}
