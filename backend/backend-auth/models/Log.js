import mongoose from "mongoose";

const logSchema = new mongoose.Schema(
  {
    userId: {
      type: mongoose.Schema.Types.ObjectId,
      ref: "User",
      default: null, // có thể là guest
    },
    action: { type: String, required: true },
  },
  { timestamps: true } // tự động có createdAt, updatedAt
);

const Log = mongoose.model("Log", logSchema);
export default Log;
