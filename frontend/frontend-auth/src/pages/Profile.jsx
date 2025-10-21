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
        const userData = res.data;

        // ✅ Xử lý role linh hoạt: object, string hoặc id
        const userRole =
          typeof userData.role === "object"
            ? userData.role.name
            : userData.role;

        // ✅ Lưu thông tin vào state và localStorage
        setUser(userData);
        setForm({ name: userData.name, email: userData.email });
        localStorage.setItem("user", JSON.stringify(userData));
        localStorage.setItem("role", userRole);
      } catch (err) {
        console.error("❌ Lỗi tải hồ sơ:", err);
        setMessage("❌ Lỗi tải thông tin người dùng! Hãy đăng nhập lại.");
      }
    };
    fetchProfile();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await updateProfile(form);
      const updatedUser = res.data;

      // ✅ Cập nhật role sau khi chỉnh sửa
      const userRole =
        typeof updatedUser.role === "object"
          ? updatedUser.role.name
          : updatedUser.role;

      setUser(updatedUser);
      localStorage.setItem("user", JSON.stringify(updatedUser));
      localStorage.setItem("role", userRole);

      setMessage("✅ Cập nhật thông tin thành công!");
    } catch (err) {
      console.error("❌ Lỗi cập nhật hồ sơ:", err);
      setMessage("❌ Lỗi khi cập nhật thông tin!");
    }
  };

  return (
    <div className="container" style={{ maxWidth: 400, margin: "auto" }}>
      <h2>Hồ sơ cá nhân</h2>

      {user ? (
        <>
          <div style={{ textAlign: "center", marginBottom: 20 }}>
            <strong>{user.name}</strong>
            <p>{user.email}</p>
            <p>
              <b>Vai trò:</b>{" "}
              {typeof user.role === "object"
                ? user.role.name
                : user.role || "Không xác định"}
            </p>
          </div>

          <form onSubmit={handleSubmit}>
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
        </>
      ) : (
        <p>{message || "Đang tải thông tin..."}</p>
      )}

      {message && (
        <p
          style={{
            color: message.includes("✅") ? "green" : "red",
            marginTop: 10,
          }}
        >
          {message}
        </p>
      )}
    </div>
  );
}
