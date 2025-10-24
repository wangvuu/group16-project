import React, { useState } from "react";
import { signup } from "../services/api";

export default function Signup() {
  const [form, setForm] = useState({
    name: "",
    email: "",
    password: "",
    role: "user",
  });
  const [message, setMessage] = useState("");

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const res = await signup(form);
      setMessage(res.data.message || "✅ Đăng ký thành công!");
    } catch (err) {
      setMessage(err.response?.data?.message || "❌ Lỗi đăng ký!");
    }
  };

  return (
    <div
      style={{
        maxWidth: "400px",
        margin: "100px auto",
        padding: "30px",
        borderRadius: "12px",
        backgroundColor: "#f0f4ff",
        boxShadow: "0 0 10px rgba(0,0,0,0.1)",
        textAlign: "center",
      }}
    >
      <h2 style={{ marginBottom: "20px", color: "#2b3a67" }}>
        Đăng ký tài khoản
      </h2>
      <form onSubmit={handleSubmit} style={{ display: "flex", flexDirection: "column", gap: "12px" }}>
        <input
          name="name"
          placeholder="Tên"
          onChange={handleChange}
          required
          style={{
            padding: "10px",
            borderRadius: "8px",
            border: "1px solid #ccc",
            fontSize: "16px",
          }}
        />
        <input
          name="email"
          placeholder="Email"
          onChange={handleChange}
          required
          style={{
            padding: "10px",
            borderRadius: "8px",
            border: "1px solid #ccc",
            fontSize: "16px",
          }}
        />
        <input
          type="password"
          name="password"
          placeholder="Mật khẩu"
          onChange={handleChange}
          required
          style={{
            padding: "10px",
            borderRadius: "8px",
            border: "1px solid #ccc",
            fontSize: "16px",
          }}
        />

        <select
          name="role"
          value={form.role}
          onChange={handleChange}
          style={{
            padding: "10px",
            borderRadius: "8px",
            border: "1px solid #ccc",
            fontSize: "16px",
          }}
        >
          <option value="user">Người dùng</option>
          <option value="moderator">Điều hành viên</option>
          <option value="admin">Quản trị viên</option>
        </select>

        <button
          type="submit"
          style={{
            padding: "12px",
            borderRadius: "8px",
            border: "none",
            backgroundColor: "#3b82f6",
            color: "white",
            fontSize: "16px",
            cursor: "pointer",
          }}
        >
          Đăng ký
        </button>
      </form>

      <p style={{ marginTop: "15px", color: "#444" }}>{message}</p>
    </div>
  );
}
