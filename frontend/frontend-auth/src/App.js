import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Signup from "./pages/Signup";
import Login from "./pages/Login";
import Profile from "./pages/Profile";
import ForgotPassword from "./pages/ForgotPassword";
import ResetPassword from "./pages/ResetPassword";
import Admin from "./pages/Admin";
import ModeratorPage from "./pages/ModeratorPage";
import UploadAvatar from "./pages/UploadAvatar";
import ProtectedRoute from "./components/ProtectedRoute";

export default function App() {
  const user = JSON.parse(localStorage.getItem("user"));

  return (
    <Router>
      {/* 🧭 Thanh điều hướng */}
      <nav style={{ padding: 10, borderBottom: "1px solid #ccc" }}>
        <Link to="/signup">Đăng ký</Link> |{" "}
        <Link to="/login">Đăng nhập</Link> |{" "}
        <Link to="/profile">Hồ sơ cá nhân</Link> |{" "}
        {user?.role === "admin" && <Link to="/admin">Quản trị</Link>} |{" "}
        {user?.role === "moderator" && <Link to="/moderator">Moderator</Link>} |{" "}
        <Link to="/upload-avatar">Upload Avatar</Link>
      </nav>

      {/* 🧩 Định nghĩa route */}
      <Routes>
        {/* Auth */}
        <Route path="/signup" element={<Signup />} />
        <Route path="/login" element={<Login />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
        <Route path="/reset-password/:token" element={<ResetPassword />} />

        {/* Profile (tất cả roles đều được vào sau khi login) */}
        <Route
          path="/profile"
          element={
            <ProtectedRoute allowedRoles={["user", "admin", "moderator"]}>
              <Profile />
            </ProtectedRoute>
          }
        />

        {/* Upload Avatar */}
        <Route
          path="/upload-avatar"
          element={
            <ProtectedRoute allowedRoles={["user", "admin", "moderator"]}>
              <UploadAvatar />
            </ProtectedRoute>
          }
        />

        {/* Moderator */}
        <Route
          path="/moderator"
          element={
            <ProtectedRoute allowedRoles={["moderator", "admin"]}>
              <ModeratorPage />
            </ProtectedRoute>
          }
        />

        {/* Admin */}
        <Route
          path="/admin"
          element={
            <ProtectedRoute allowedRoles={["admin"]}>
              <Admin />
            </ProtectedRoute>
          }
        />
      </Routes>
    </Router>
  );
}
