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
      {/* Khi user đã đăng nhập */}
      {user ? (
        <>
          {/* Link cơ bản */}
          <Link to="/profile">Hồ sơ</Link>
          <Link to="/upload-avatar">Upload Avatar</Link>

          {/* Moderator */}
          {user.role === "moderator" && (
            <Link to="/moderator">Quản lý bài viết</Link>
          )}

          {/* Admin */}
          {user.role === "admin" && <Link to="/admin">Quản trị hệ thống</Link>}

          {/* Hiển thị tên & vai trò */}
          <span style={{ marginLeft: "20px", color: "gray" }}>
            👤 {user.name} ({user.role})
          </span>

          {/* Nút đăng xuất */}
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
        // Khi chưa đăng nhập
        <>
          <Link to="/login">Đăng nhập</Link>
          <Link to="/signup">Đăng ký</Link>
        </>
      )}
    </nav>
  );
}
