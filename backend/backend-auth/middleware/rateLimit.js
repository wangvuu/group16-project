import rateLimit from "express-rate-limit";

export const loginRateLimiter = rateLimit({
  windowMs: 0 * 60 * 1000, // 15 phút
  max: 5, // chỉ cho phép 5 lần login sai
  message: {
    success: false,
    message: "Quá nhiều lần đăng nhập thất bại. Hãy thử lại sau 15 phút."
  }
});
