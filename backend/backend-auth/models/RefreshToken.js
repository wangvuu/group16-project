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
      index: true, // để truy vấn nhanh
    },
    expiresAt: {
      type: Date,
      required: true,
      index: { expires: 0 }, // TTL index: tự xóa khi hết hạn
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

// 🔧 Tùy chọn: tạo phương thức tiện dụng
refreshTokenSchema.statics.createToken = async function (userId, token, expiresInDays = 7) {
  const expiresAt = new Date();
  expiresAt.setDate(expiresAt.getDate() + expiresInDays);

  const refreshToken = new this({
    user: userId,
    token,
    expiresAt,
  });

  return await refreshToken.save();
};

export default mongoose.model("RefreshToken", refreshTokenSchema);
