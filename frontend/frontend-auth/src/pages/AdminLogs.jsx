import React, { useEffect, useState } from "react";
import axios from "axios";

export default function AdminLogs() {
  const [logs, setLogs] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchLogs = async () => {
      const token = localStorage.getItem("accessToken");

      try {
        const res = await axios.get(
          `${process.env.REACT_APP_API_URL}/logs/with-user`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        setLogs(res.data);
      } catch (err) {
        console.error("❌ Lỗi khi tải logs:", err);
        setError(err.response?.data?.message || "Không thể tải logs");
      }
    };

    fetchLogs();
  }, []);

  return (
    <div className="max-w-6xl mx-auto p-6 bg-white rounded-2xl shadow-md mt-8">
      <h2 className="text-2xl font-bold mb-6 flex items-center gap-2">
        📜 <span>Nhật ký hoạt động người dùng</span>
      </h2>

      {error && (
        <p className="text-red-600 font-semibold mb-4 bg-red-50 p-3 rounded-lg border border-red-200">
          {error}
        </p>
      )}

      {logs.length === 0 ? (
        <p className="text-gray-500 text-center italic py-6">
          Không có hoạt động nào được ghi lại.
        </p>
      ) : (
        <div className="overflow-x-auto rounded-lg">
          <table className="min-w-full border border-gray-200 text-sm text-gray-800">
            <thead>
              <tr className="bg-gray-100 text-left text-gray-700">
                <th className="py-3 px-4 border-b">👤 Người dùng</th>
                <th className="py-3 px-4 border-b">⚙️ Hành động</th>
                <th className="py-3 px-4 border-b">🕒 Thời gian</th>
              </tr>
            </thead>
            <tbody>
              {logs.map((log, i) => (
                <tr
                  key={i}
                  className={`${
                    i % 2 === 0 ? "bg-white" : "bg-gray-50"
                  } hover:bg-blue-50 transition`}
                >
                  <td className="py-2 px-4 border-b">
                    {log.userId
                      ? `${log.userId.name} (${log.userId.email})`
                      : "👥 Khách (Guest)"}
                  </td>
                  <td className="py-2 px-4 border-b">{log.action}</td>
                  <td className="py-2 px-4 border-b text-gray-600">
                    {new Date(log.createdAt).toLocaleString()}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
