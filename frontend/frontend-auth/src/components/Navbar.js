import React from "react";
import { Link, useNavigate } from "react-router-dom";

export default function Navbar() {
  const navigate = useNavigate();
  const user = JSON.parse(localStorage.getItem("user"));

  const handleLogout = () => {
    localStorage.clear();
    navigate("/login");
  };

  return (
    <nav
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        gap: "20px",
        backgroundColor: "#f8f9fa",
        padding: "10px 0",
        borderBottom: "1px solid #ddd",
      }}
    >
      {user ? (
        <>
          <Link to="/profile">Hồ sơ</Link>

          {user.role === "moderator" && (
            <Link to="/moderator">Quản lý bài viết</Link>
          )}

          {user.role === "admin" && <Link to="/admin">Quản trị hệ thống</Link>}

          <span style={{ marginLeft: "20px", color: "gray" }}>
            👤 {user.name} ({user.role})
          </span>

          <button
            onClick={handleLogout}
            style={{
              marginLeft: "20px",
              backgroundColor: "red",
              color: "white",
              border: "none",
              borderRadius: "5px",
              padding: "5px 10px",
              cursor: "pointer",
            }}
          >
            Đăng xuất
          </button>
        </>
      ) : (
        <Link to="/login">Đăng nhập</Link>
      )}
    </nav>
  );
}
