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

        // ✅ Cập nhật localStorage để đồng bộ avatar
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

  if (!user) return <p style={{ textAlign: "center" }}>Đang tải thông tin...</p>;

  return (
    <div className="container" style={{ maxWidth: 500, margin: "40px auto", textAlign: "center" }}>
      <h2>🧍‍♂️ Hồ sơ cá nhân</h2>

      {/* ✅ Ảnh đại diện */}
      <div style={{ marginBottom: "15px" }}>
        <img
          src={user.avatar || "https://via.placeholder.com/120?text=No+Avatar"}
          alt="Avatar"
          width="120"
          height="120"
          style={{
            borderRadius: "50%",
            objectFit: "cover",
            border: "2px solid #ccc",
          }}
        />
        <p>
          <a href="/upload-avatar" style={{ color: "blue", textDecoration: "underline" }}>
            Thay đổi ảnh đại diện
          </a>
        </p>
      </div>

      {/* ✅ Thông tin user */}
      <div style={{ marginBottom: "10px" }}>
        <strong>{user.name}</strong>
        <p>{user.email}</p>
        <p>
          <strong>Vai trò:</strong>{" "}
          {typeof user.role === "object" ? user.role.name : user.role}
        </p>
      </div>

      {/* ✅ Form cập nhật */}
      <form onSubmit={handleSubmit} style={{ display: "flex", gap: "5px", justifyContent: "center" }}>
        <input
          name="name"
          value={form.name}
          onChange={(e) => setForm({ ...form, name: e.target.value })}
          placeholder="Tên"
          required
        />
        <input
          name="email"
          value={form.email}
          onChange={(e) => setForm({ ...form, email: e.target.value })}
          placeholder="Email"
          required
        />
        <button type="submit">Cập nhật</button>
      </form>

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
    </div>
  );
}
