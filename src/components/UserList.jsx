import React, { useEffect, useState } from "react";
import axios from "axios";

function UserList() {
  const [users, setUsers] = useState([]);
  const [editingId, setEditingId] = useState(null);
  const [editingData, setEditingData] = useState({ name: "", email: "" });

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const res = await axios.get("http://localhost:5000/users");
      setUsers(res.data);
    } catch (err) {
      console.error(err);
      alert("Lỗi khi lấy danh sách user");
    }
  };

  const handleDelete = async (id) => {
    const ok = window.confirm("Bạn có chắc muốn xóa user này?");
    if (!ok) return;
    try {
      await axios.delete(`http://localhost:5000/users/${id}`);
      setUsers(prev => prev.filter(u => u._id !== id));
    } catch (err) {
      console.error(err);
      alert("Xóa thất bại");
    }
  };

  const startEdit = (user) => {
    setEditingId(user._id);
    setEditingData({ name: user.name, email: user.email });
  };

  const cancelEdit = () => {
    setEditingId(null);
    setEditingData({ name: "", email: "" });
  };

  const saveEdit = async () => {
    const { name, email } = editingData;
    if (!name.trim()) return alert("Name không được để trống");
    if (!/\S+@\S+\.\S+/.test(email)) return alert("Email không hợp lệ");
    try {
      const res = await axios.put(`http://localhost:5000/users/${editingId}`, { name, email });
      setUsers(prev => prev.map(u => (u._id === editingId ? res.data : u)));
      cancelEdit();
    } catch (err) {
      console.error(err);
      alert("Cập nhật thất bại");
    }
  };

  return (
    <div>
      <h2>Danh sách User</h2>
      <ul>
        {users.map(u => (
          <li key={u._id} style={{ marginBottom: 8 }}>
            {editingId === u._id ? (
              <>
                <input
                  value={editingData.name}
                  onChange={(e) => setEditingData({ ...editingData, name: e.target.value })}
                  placeholder="Tên"
                />
                <input
                  value={editingData.email}
                  onChange={(e) => setEditingData({ ...editingData, email: e.target.value })}
                  placeholder="Email"
                />
                <button onClick={saveEdit}>Lưu</button>
                <button onClick={cancelEdit}>Hủy</button>
              </>
            ) : (
              <>
                <span>{u.name} - {u.email}</span>{" "}
                <button onClick={() => startEdit(u)}>Sửa</button>
                <button onClick={() => handleDelete(u._id)}>Xóa</button>
              </>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
}

export default UserList;
