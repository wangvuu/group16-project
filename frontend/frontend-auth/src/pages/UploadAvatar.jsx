import React, { useState } from "react";
import { uploadAvatar } from "../services/api";
import { useNavigate } from "react-router-dom";

export default function UploadAvatar() {
  const [file, setFile] = useState(null);
  const [preview, setPreview] = useState("");
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  const handleChange = (e) => {
    const selected = e.target.files[0];
    if (selected) {
      setFile(selected);
      setPreview(URL.createObjectURL(selected)); // hiển thị ảnh xem trước
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!file) {
      setMessage("❌ Vui lòng chọn ảnh trước!");
      return;
    }

    const formData = new FormData();
    formData.append("avatar", file);

    try {
      const res = await uploadAvatar(formData);
      setMessage("✅ Ảnh đại diện đã được cập nhật!");
      console.log("Cloudinary URL:", res.data.avatarUrl);

      // ✅ Cập nhật user trong localStorage
      const user = JSON.parse(localStorage.getItem("user"));
      user.avatar = res.data.avatarUrl;
      localStorage.setItem("user", JSON.stringify(user));

      // Quay lại trang hồ sơ sau 2s
      setTimeout(() => navigate("/profile"), 2000);
    } catch (err) {
      console.error("❌ Lỗi upload:", err);
      setMessage("❌ Upload thất bại, vui lòng thử lại!");
    }
  };

  return (
    <div
      style={{
        textAlign: "center",
        marginTop: 50,
        border: "1px solid #ddd",
        borderRadius: "10px",
        padding: "20px",
        width: "350px",
        margin: "40px auto",
      }}
    >
      <h2>📤 Tải lên ảnh đại diện</h2>

      <form onSubmit={handleSubmit}>
        {preview && (
          <img
            src={preview}
            alt="Xem trước"
            width="120"
            height="120"
            style={{
              borderRadius: "50%",
              marginBottom: "10px",
              objectFit: "cover",
            }}
          />
        )}

        <input
          type="file"
          accept="image/*"
          onChange={handleChange}
          style={{ marginBottom: "10px" }}
        />
        <br />

        <button
          type="submit"
          style={{
            backgroundColor: "#4CAF50",
            color: "white",
            padding: "8px 16px",
            border: "none",
            borderRadius: "5px",
            cursor: "pointer",
          }}
        >
          Upload
        </button>
      </form>

      {message && (
        <p
          style={{
            marginTop: 10,
            color: message.includes("✅") ? "green" : "red",
          }}
        >
          {message}
        </p>
      )}
    </div>
  );
}
