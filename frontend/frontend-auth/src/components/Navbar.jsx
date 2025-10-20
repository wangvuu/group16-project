import React from "react";
import { Link } from "react-router-dom";

export default function Navbar() {
  const role = localStorage.getItem("role");

  const handleLogout = () => {
    localStorage.clear();
    window.location.href = "/login";
  };

  return (
    <nav
      style={{
        background: "#222",
        color: "#fff",
        padding: "10px",
        display: "flex",
        gap: "15px",
      }}
    >
      <Link to="/profile" style={{ color: "#fff" }}>Profile</Link>
      {(role === "editor" || role === "admin") && (
        <Link to="/upload" style={{ color: "#fff" }}>Upload Avatar</Link>
      )}
      {role === "admin" && (
        <Link to="/admin" style={{ color: "#fff" }}>Quản lý User</Link>
      )}
      <button onClick={handleLogout} style={{ marginLeft: "auto" }}>
        Đăng xuất
      </button>
    </nav>
  );
}
