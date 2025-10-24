import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getUsers, deleteUser } from "../services/api";

export default function Admin() {
  const [users, setUsers] = useState([]);
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  const token = localStorage.getItem("accessToken");
  const user = JSON.parse(localStorage.getItem("user"));
  const rawRole = user?.role;

  // ✅ Chuẩn hóa role
  const role =
    typeof rawRole === "object"
      ? rawRole.name
      : typeof rawRole === "string"
      ? rawRole.toLowerCase()
      : "";

  // ✅ Kiểm tra quyền truy cập
  useEffect(() => {
    if (!token || (role !== "admin" && role !== "moderator")) {
      alert("🚫 Bạn không có quyền truy cập trang quản trị!");
      navigate("/profile");
    }
  }, [token, role, navigate]);

  // ✅ Lấy danh sách người dùng
  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const res = await getUsers();
        setUsers(res.data);
      } catch (err) {
        console.error("❌ Lỗi khi tải danh sách:", err);
        setMessage("❌ Không thể tải danh sách người dùng!");
      }
    };
    if (token) fetchUsers();
  }, [token]);

  // ✅ Xóa người dùng
  const handleDelete = async (id) => {
    if (role !== "admin") {
      alert("⚠️ Chỉ Admin mới có quyền xóa!");
      return;
    }
    if (!window.confirm("Bạn có chắc muốn xóa người dùng này?")) return;

    try {
      const res = await deleteUser(id);
      setUsers(users.filter((u) => u._id !== id));
      setMessage(res.data.message || "✅ Xóa người dùng thành công!");
    } catch (err) {
      console.error("❌ Lỗi khi xóa:", err);
      setMessage("❌ Lỗi khi xóa người dùng!");
    }
  };

  return (
    <div
      style={{
        maxWidth: "900px",
        margin: "50px auto",
        backgroundColor: "#f0f4ff",
        padding: "30px",
        borderRadius: "15px",
        boxShadow: "0 0 15px rgba(0,0,0,0.1)",
        fontFamily: "Arial, sans-serif",
      }}
    >
      <h2
        style={{
          color: "#2b3a67",
          textAlign: "center",
          marginBottom: "25px",
        }}
      >
        📋 Trang Quản Trị ({role === "admin" ? "Quản trị viên" : "Điều hành viên"})
      </h2>

      {message && (
        <p
          style={{
            textAlign: "center",
            color: message.includes("✅") ? "green" : "red",
            fontWeight: "bold",
            marginBottom: "20px",
          }}
        >
          {message}
        </p>
      )}

      <div style={{ overflowX: "auto" }}>
        <table
          style={{
            width: "100%",
            borderCollapse: "collapse",
            borderRadius: "10px",
            overflow: "hidden",
          }}
        >
          <thead style={{ backgroundColor: "#3b82f6", color: "white" }}>
            <tr>
              <th style={thStyle}>Tên</th>
              <th style={thStyle}>Email</th>
              <th style={thStyle}>Vai trò</th>
              {role === "admin" && <th style={thStyle}>Hành động</th>}
            </tr>
          </thead>
          <tbody>
            {users.length ? (
              users.map((u, i) => (
                <tr
                  key={u._id}
                  style={{
                    backgroundColor: i % 2 === 0 ? "#ffffff" : "#eef2ff",
                    textAlign: "center",
                    transition: "0.2s",
                  }}
                  onMouseOver={(e) =>
                    (e.currentTarget.style.backgroundColor = "#dbeafe")
                  }
                  onMouseOut={(e) =>
                    (e.currentTarget.style.backgroundColor =
                      i % 2 === 0 ? "#ffffff" : "#eef2ff")
                  }
                >
                  <td style={tdStyle}>{u.name}</td>
                  <td style={tdStyle}>{u.email}</td>
                  <td style={tdStyle}>
                    {typeof u.role === "object"
                      ? u.role?.name
                      : u.role || "Không có vai trò"}
                  </td>
                  {role === "admin" && (
                    <td style={tdStyle}>
                      <button
                        onClick={() => handleDelete(u._id)}
                        style={deleteBtn}
                      >
                        🗑 Xóa
                      </button>
                    </td>
                  )}
                </tr>
              ))
            ) : (
              <tr>
                <td
                  colSpan={role === "admin" ? 4 : 3}
                  style={{ textAlign: "center", padding: "20px" }}
                >
                  Không có người dùng nào.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}

// 🎨 CSS style inline
const thStyle = {
  padding: "12px",
  fontSize: "16px",
  fontWeight: "600",
  borderBottom: "2px solid #2563eb",
};

const tdStyle = {
  padding: "10px",
  borderBottom: "1px solid #ddd",
  fontSize: "15px",
};

const deleteBtn = {
  backgroundColor: "#ef4444",
  color: "white",
  border: "none",
  padding: "6px 12px",
  borderRadius: "6px",
  cursor: "pointer",
  fontSize: "14px",
  transition: "0.2s",
};
