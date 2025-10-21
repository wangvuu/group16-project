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

  // ✅ Chuẩn hóa role (nếu là object, chuỗi hoặc id)
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

  // ✅ Xóa người dùng (chỉ admin)
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
    <div className="container" style={{ padding: "20px" }}>
      <h2>📋 Trang Quản trị ({role})</h2>
      {message && (
        <p
          style={{
            color: message.includes("✅") ? "green" : "red",
            fontWeight: "bold",
          }}
        >
          {message}
        </p>
      )}

      <table
        border="1"
        cellPadding="8"
        style={{ borderCollapse: "collapse", width: "100%" }}
      >
        <thead>
          <tr style={{ backgroundColor: "#f0f0f0" }}>
            <th>Tên</th>
            <th>Email</th>
            <th>Vai trò</th>
            {role === "admin" && <th>Hành động</th>}
          </tr>
        </thead>
        <tbody>
          {users.length ? (
            users.map((u) => (
              <tr key={u._id}>
                <td>{u.name}</td>
                <td>{u.email}</td>
                <td>
                  {typeof u.role === "object"
                    ? u.role?.name
                    : u.role || "Không có vai trò"}
                </td>
                {role === "admin" && (
                  <td>
                    <button
                      onClick={() => handleDelete(u._id)}
                      style={{
                        backgroundColor: "red",
                        color: "white",
                        border: "none",
                        padding: "5px 10px",
                        borderRadius: "5px",
                        cursor: "pointer",
                      }}
                    >
                      Xóa
                    </button>
                  </td>
                )}
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={role === "admin" ? 4 : 3}>
                Không có người dùng nào.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
}
