import mongoose from "mongoose";

const refreshTokenSchema = new mongoose.Schema(
  {
    user: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User",
      required: true,
    },
    token: {
      type: String,
      required: true,
      // ❌ Bỏ unique để tránh lỗi E11000 duplicate key
      index: true, // ✅ vẫn tạo index để truy vấn nhanh, nhưng không unique
    },
    expiresAt: {
      type: Date,
      required: true,
    },
    revoked: {
      type: Boolean,
      default: false,
    },
    replacedByToken: {
      type: String,
      default: null,
    },
  },
  { timestamps: true }
);

// 🧹 Middleware: xóa token đã hết hạn mỗi khi có truy vấn (tùy chọn)
refreshTokenSchema.pre("find", async function () {
  await mongoose.model("RefreshToken").deleteMany({
    expiresAt: { $lte: new Date() },
  });
});

export default mongoose.model("RefreshToken", refreshTokenSchema);
