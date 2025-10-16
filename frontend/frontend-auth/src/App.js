import React from "react";
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import Signup from "./pages/Signup";
import Login from "./pages/Login";
import Profile from "./pages/Profile";
import ForgotPassword from "./pages/ForgotPassword";
import ResetPassword from "./pages/ResetPassword";
import Admin from "./pages/Admin";

export default function App() {
  return (
    <Router>
      <nav style={{ padding: 10, borderBottom: "1px solid #ccc" }}>
        <Link to="/signup">Đăng ký</Link> |{" "}
        <Link to="/login">Đăng nhập</Link> |{" "}
        <Link to="/profile">Hồ sơ cá nhân</Link> |{" "}
        <Link to="/admin">Quản trị</Link>
      </nav>

      <Routes>
        <Route path="/signup" element={<Signup />} />
        <Route path="/login" element={<Login />} />
        <Route path="/profile" element={<Profile />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />
        <Route path="/reset-password/:token" element={<ResetPassword />} />
        <Route path="/admin" element={<Admin />} />
      </Routes>
    </Router>
  );
}