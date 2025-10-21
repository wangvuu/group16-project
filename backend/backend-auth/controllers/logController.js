import Log from "../models/Log.js";

export const getAllLogs = async (req, res) => {
  try {
    const logs = await Log.find().populate("userId", "email role").sort({ createdAt: -1 });
    res.json(logs);
  } catch (err) {
    res.status(500).json({ message: "Lỗi khi lấy log", error: err.message });
  }
};
