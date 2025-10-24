import Log from "../models/Log.js";

export const getAllLogs = async (req, res) => {
  try {
    const logs = await Log.find()
      .populate("userId", "name email role")
      .sort({ createdAt: -1 });

    res.json(logs);
  } catch (err) {
    console.error("❌ Lỗi khi lấy logs:", err);
    res.status(500).json({ message: "Lỗi server khi lấy logs" });
  }
};

// 🧩 Lấy logs kèm thông tin người dùng (frontend AdminLogs.jsx dùng)
export const getLogsWithUser = async (req, res) => {
  try {
    const logs = await Log.find()
      .populate("userId", "email role")
      .sort({ createdAt: -1 });
    res.status(200).json(logs);
  } catch (error) {
    console.error("❌ getLogsWithUser error:", error);
    res.status(500).json({ message: "Lỗi khi lấy logs" });
  }
};
