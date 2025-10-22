import Log from "../models/Log.js";

export const logActivity = async (req, res, next) => {
  try {
    const userId = req.user ? req.user.id : null; // nếu có user từ token
    await Log.create({
      userId,
      action: `${req.method} ${req.originalUrl}`,
      ip: req.ip,
      userAgent: req.headers["user-agent"]
    });
  } catch (err) {
    console.error("Logging error:", err);
  }
  next();
};
