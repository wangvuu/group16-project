// src/App.js
import React, { useState, useEffect } from "react";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Link,
  Navigate,
  useLocation,
} from "react-router-dom";
import Signup from "./pages/Signup";
import Login from "./pages/Login";
import Profile from "./pages/Profile";
import ForgotPassword from "./pages/ForgotPassword";
import ResetPassword from "./pages/ResetPassword";
import Admin from "./pages/Admin";
import ModeratorPage from "./pages/ModeratorPage";
import UploadAvatar from "./pages/UploadAvatar";
import ProtectedRoute from "./components/ProtectedRoute";
import AdminLogs from "./pages/AdminLogs";
import { useDispatch } from "react-redux";
import { logout } from "./store/authSlice";

function AppContent() {
  const dispatch = useDispatch();
  const location = useLocation();
  const [user, setUser] = useState(() => {
    const storedUser = localStorage.getItem("user");
    return storedUser ? JSON.parse(storedUser) : null;
  });

  // 🔄 Đồng bộ user khi localStorage thay đổi
  useEffect(() => {
    const syncUser = () => {
      const storedUser = localStorage.getItem("user");
      setUser(storedUser ? JSON.parse(storedUser) : null);
    };
    window.addEventListener("storage", syncUser);
    window.addEventListener("userChange", syncUser);
    return () => {
      window.removeEventListener("storage", syncUser);
      window.removeEventListener("userChange", syncUser);
    };
  }, []);

  const handleLogout = () => {
    dispatch(logout());
    localStorage.removeItem("user");
    localStorage.removeItem("token");
    setUser(null);
    window.location.href = "/login";
  };

  // 🟢 Các trang công khai
  const publicPaths = ["/login", "/signup", "/forgot-password", "/reset-password"];

  // 🧭 Nếu chưa đăng nhập và không ở trang công khai → về login
  if (!user && !publicPaths.includes(location.pathname)) {
    console.warn("⚠️ Chưa đăng nhập, chuyển hướng về /login...");
    return <Navigate to="/login" replace />;
  }

  return (
    <>
      {/* 🔗 Thanh điều hướng */}
      <nav style={{ padding: 10, borderBottom: "1px solid #ccc" }}>
        {!user ? (
          <>
            <Link to="/signup">Đăng ký</Link> | <Link to="/login">Đăng nhập</Link>
          </>
        ) : (
          <>
            <Link to="/profile">Hồ sơ cá nhân</Link> |{" "}
            {user.role === "admin" && <Link to="/admin">Quản trị</Link>} |{" "}
            {user.role === "moderator" && <Link to="/moderator">Moderator</Link>} |{" "}
            <Link to="/upload-avatar">Upload Avatar</Link>
            {user.role === "admin" && (
              <>
                {" | "}
                <Link to="/logs">Xem Logs</Link>
              </>
            )}
            <button
              onClick={handleLogout}
              style={{
                float: "right",
                background: "none",
                border: "none",
                color: "red",
                cursor: "pointer",
              }}
            >
              🚪 Đăng xuất
            </button>
          </>
        )}
      </nav>

      {/* 🛣️ Các route */}
                <Routes>
                  {/* Nếu vào "/" → tự điều hướng */}
                  {/* Khi load "/" thì luôn vào trang đăng nhập nếu chưa có user */}
          <Route
            path="/"
            element={<Navigate to="/login" replace />}
          />

        

        <Route path="/signup" element={<Signup />} />
        <Route path="/login" element={<Login setUser={setUser} />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
        <Route path="/reset-password/:token" element={<ResetPassword />} />

        <Route
          path="/profile"
          element={
            <ProtectedRoute allowedRoles={["user", "admin", "moderator"]}>
              <Profile />
            </ProtectedRoute>
          }
        />
        <Route
          path="/upload-avatar"
          element={
            <ProtectedRoute allowedRoles={["user", "admin", "moderator"]}>
              <UploadAvatar />
            </ProtectedRoute>
          }
        />
        <Route
          path="/moderator"
          element={
            <ProtectedRoute allowedRoles={["moderator", "admin"]}>
              <ModeratorPage />
            </ProtectedRoute>
          }
        />
        <Route
          path="/admin"
          element={
            <ProtectedRoute allowedRoles={["admin"]}>
              <Admin />
            </ProtectedRoute>
          }
        />
        <Route
          path="/logs"
          element={
            <ProtectedRoute allowedRoles={["admin"]}>
              <AdminLogs />
            </ProtectedRoute>
          }
        />
      </Routes>
    </>
  );
}

export default function App() {
  return (
    <Router>
      <AppContent />
    </Router>
  );
}
