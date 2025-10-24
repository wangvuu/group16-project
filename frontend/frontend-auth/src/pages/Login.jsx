import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { login } from "../services/api";
import { useDispatch, useSelector } from "react-redux";
import { loginUser } from "../store/authSlice";

export default function Login() {
  const [form, setForm] = useState({ email: "", password: "" });
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  const dispatch = useDispatch();
  const { loading, error } = useSelector((state) => state.auth);

  const handleChange = (e) =>
    setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      dispatch(loginUser(form));
      const res = await login(form);

      localStorage.setItem("accessToken", res.data.accessToken);
      localStorage.setItem("refreshToken", res.data.refreshToken);
      localStorage.setItem("user", JSON.stringify(res.data.user));

      window.dispatchEvent(new Event("userChange"));
      setMessage("✅ Đăng nhập thành công!");

      const role = res.data.user.role;
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
      style={{
        maxWidth: "400px",
        margin: "100px auto",
        padding: "30px",
        borderRadius: "12px",
        backgroundColor: "#f0f4ff",
        boxShadow: "0 0 10px rgba(0,0,0,0.1)",
        textAlign: "center",
        fontFamily: "Arial, sans-serif",
      }}
    >
      <h2 style={{ marginBottom: "20px", color: "#2b3a67" }}>🔐 Đăng nhập</h2>

      <form
        onSubmit={handleSubmit}
        style={{ display: "flex", flexDirection: "column", gap: "12px" }}
      >
        <input
          name="email"
          placeholder="Email"
          value={form.email}
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
          value={form.password}
          onChange={handleChange}
          required
          style={{
            padding: "10px",
            borderRadius: "8px",
            border: "1px solid #ccc",
            fontSize: "16px",
          }}
        />

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
            transition: "0.2s",
          }}
          onMouseOver={(e) => (e.target.style.backgroundColor = "#2563eb")}
          onMouseOut={(e) => (e.target.style.backgroundColor = "#3b82f6")}
          disabled={loading}
        >
          {loading ? "⏳ Đang đăng nhập..." : "Đăng nhập"}
        </button>
      </form>

      <p style={{ marginTop: "10px" }}>
        <button
          type="button"
          onClick={() => navigate("/forgot-password")}
          style={{
            background: "none",
            border: "none",
            color: "#1d4ed8",
            textDecoration: "underline",
            cursor: "pointer",
          }}
        >
          Quên mật khẩu?
        </button>
      </p>

      {(message || error) && (
        <p
          style={{
            marginTop: 15,
            color: message.includes("✅") && !error ? "green" : "red",
            fontWeight: "bold",
          }}
        >
          {message || error}
        </p>
      )}
    </div>
  );
}
