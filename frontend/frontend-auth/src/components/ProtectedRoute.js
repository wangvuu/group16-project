// src/components/ProtectedRoute.jsx
import React from "react";
import { Navigate } from "react-router-dom";

export default function ProtectedRoute({ allowedRoles, children }) {
  const user = JSON.parse(localStorage.getItem("user"));

  // ❌ Nếu chưa đăng nhập
  if (!user) {
    console.warn("⚠️ Chưa có user, chuyển hướng về /login...");
    return <Navigate to="/login" replace />;
  }

  // ❌ Nếu user không có role hoặc role không được phép
  if (!user.role || !allowedRoles.includes(user.role)) {
    alert("Bạn không có quyền truy cập trang này!");
    return <Navigate to="/profile" replace />;
  }

  // ✅ Nếu hợp lệ → hiển thị nội dung
  return children;
}
