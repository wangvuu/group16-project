import React from "react";

export default function ProfilePage() {
  const user = JSON.parse(localStorage.getItem("user"));
  
  return (
    <div style={{ padding: "20px" }}>
      <h2>👤 Hồ sơ người dùng</h2>
      {user ? (
        <div>
          <p><strong>Tên:</strong> {user.name}</p>
          <p><strong>Email:</strong> {user.email}</p>
          <p><strong>Vai trò:</strong> {user.role}</p>
        </div>
      ) : (
        <p>Không có thông tin người dùng.</p>
      )}
    </div>
  );
}
