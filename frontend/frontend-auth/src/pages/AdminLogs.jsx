import React, { useEffect, useState } from "react";
import axios from "axios";

const AdminLogs = () => {
  const [logs, setLogs] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchLogs = async () => {
      const token = localStorage.getItem("accessToken");

      try {
        // 🧩 Gọi API logs có populate user
        const res = await axios.get(`${process.env.REACT_APP_API_URL}/logs/with-user`, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setLogs(res.data);
      } catch (err) {
        console.error("❌ Lỗi khi tải logs:", err);
        setError(err.response?.data?.message || "Không thể tải logs");
      }
    };
    fetchLogs();
  }, []);

  return (
    <div className="p-6">
      <h2 className="text-xl font-bold mb-4">📜 User Activity Logs</h2>

      {error && (
        <p style={{ color: "red", fontWeight: "bold", marginBottom: 10 }}>{error}</p>
      )}

      <table className="table-auto w-full border">
        <thead>
          <tr style={{ backgroundColor: "#f2f2f2" }}>
            <th>User</th>
            <th>Action</th>
            <th>Time</th>
          </tr>
        </thead>
        <tbody>
          {logs.map((log, i) => (
            <tr key={i}>
              {/* 🆕 Ưu tiên hiển thị tên + email nếu có */}
              <td>
                {log.userId
                  ? `${log.userId.name} (${log.userId.email})`
                  : "Guest"}
              </td>
              <td>{log.action}</td>
              <td>{new Date(log.createdAt).toLocaleString()}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default AdminLogs;
