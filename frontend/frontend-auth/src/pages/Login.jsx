import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../services/api";

export default function Login() {
  const [form, setForm] = useState({ email: "", password: "" });
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  // 📌 Xử lý thay đổi input
  const handleChange = (e) =>
    setForm({ ...form, [e.target.name]: e.target.value });

  // 📌 Xử lý đăng nhập
  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await login(form);

      // ✅ Lưu token và thông tin user
      localStorage.setItem("accessToken", res.data.accessToken);
      localStorage.setItem("refreshToken", res.data.refreshToken);
      
      localStorage.setItem("user", JSON.stringify(res.data.user));

      setMessage("✅ Đăng nhập thành công!");

      const role = res.data.user.role;

      // ✅ Điều hướng theo vai trò
      setTimeout(() => {
        if (role === "admin") navigate("/admin");
        else if (role === "moderator") navigate("/moderator");
        else navigate("/profile");
      }, 1000);
    } catch (err) {
      console.error("❌ Lỗi đăng nhập:", err);
      setMessage(err.response?.data?.message || "❌ Sai email hoặc mật khẩu!");
    }
  };

  return (
    <div
      className="container"
      style={{
        maxWidth: 400,
        margin: "60px auto",
        textAlign: "center",
        fontFamily: "Arial, sans-serif",
        border: "1px solid #ccc",
        borderRadius: "10px",
        padding: "20px",
      }}
    >
      <h2>🔐 Đăng nhập</h2>
      <form
        onSubmit={handleSubmit}
        style={{ display: "flex", flexDirection: "column", gap: "10px" }}
      >
        <input
          name="email"
          placeholder="Email"
          value={form.email}
          onChange={handleChange}
          required
          style={{ padding: "8px", borderRadius: "5px" }}
        />
        <input
          type="password"
          name="password"
          placeholder="Mật khẩu"
          value={form.password}
          onChange={handleChange}
          required
          style={{ padding: "8px", borderRadius: "5px" }}
        />
        <button
          type="submit"
          style={{
            backgroundColor: "#4CAF50",
            color: "white",
            padding: "8px",
            border: "none",
            borderRadius: "5px",
            cursor: "pointer",
          }}
        >
          Đăng nhập
        </button>
      </form>

      <p style={{ marginTop: "10px" }}>
        <button
          type="button"
          onClick={() => navigate("/forgot-password")}
          style={{
            background: "none",
            border: "none",
            color: "blue",
            textDecoration: "underline",
            cursor: "pointer",
          }}
        >
          Quên mật khẩu?
        </button>
      </p>

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
