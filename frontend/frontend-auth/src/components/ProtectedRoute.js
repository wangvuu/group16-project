import React from "react";
import { Navigate } from "react-router-dom";

export default function ProtectedRoute({ allowedRoles, children }) {
  const user = JSON.parse(localStorage.getItem("user"));

  // ⚠️ Nếu chưa đăng nhập
  if (!user) {
    alert("Vui lòng đăng nhập!");
    return <Navigate to="/login" replace />;
  }

  // ⚠️ Nếu không có quyền truy cập
  if (!allowedRoles.includes(user.role)) {
    alert("Bạn không có quyền truy cập trang này!");
    return <Navigate to="/profile" replace />;
  }

  // ✅ Nếu có quyền thì hiển thị nội dung
  return children;
}
