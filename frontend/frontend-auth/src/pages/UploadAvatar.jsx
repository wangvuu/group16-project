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
      setPreview(URL.createObjectURL(selected));
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

      // ✅ Cập nhật lại localStorage
      const user = JSON.parse(localStorage.getItem("user"));
      user.avatar = res.data.avatarUrl;
      localStorage.setItem("user", JSON.stringify(user));

      setTimeout(() => navigate("/profile"), 2000);
    } catch (err) {
      console.error("❌ Lỗi upload:", err);
      setMessage("❌ Upload thất bại, vui lòng thử lại!");
    }
  };

  return (
    <div
      style={{
        maxWidth: "400px",
        margin: "80px auto",
        backgroundColor: "#f0f4ff",
        padding: "30px",
        borderRadius: "15px",
        boxShadow: "0 0 15px rgba(0,0,0,0.1)",
        textAlign: "center",
        fontFamily: "Arial, sans-serif",
      }}
    >
      <h2 style={{ color: "#2b3a67", marginBottom: "20px" }}>
        📤 Tải lên ảnh đại diện
      </h2>

      <form onSubmit={handleSubmit}>
        {preview ? (
          <img
            src={preview}
            alt="Xem trước"
            width="130"
            height="130"
            style={{
              borderRadius: "50%",
              marginBottom: "15px",
              objectFit: "cover",
              border: "3px solid #3b82f6",
            }}
          />
        ) : (
          <div
            style={{
              width: "130px",
              height: "130px",
              borderRadius: "50%",
              backgroundColor: "#e5e7eb",
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              margin: "0 auto 15px",
              color: "#6b7280",
              fontSize: "14px",
            }}
          >
            Chưa chọn ảnh
          </div>
        )}

        <input
          type="file"
          accept="image/*"
          onChange={handleChange}
          style={{
            marginBottom: "15px",
            display: "block",
            margin: "0 auto 15px",
            cursor: "pointer",
          }}
        />

        <button
          type="submit"
          style={{
            backgroundColor: "#3b82f6",
            color: "white",
            padding: "10px 20px",
            border: "none",
            borderRadius: "8px",
            cursor: "pointer",
            fontSize: "16px",
            transition: "0.2s",
          }}
          onMouseOver={(e) => (e.target.style.backgroundColor = "#2563eb")}
          onMouseOut={(e) => (e.target.style.backgroundColor = "#3b82f6")}
        >
          🚀 Upload
        </button>
      </form>

      {message && (
        <p
          style={{
            marginTop: 15,
            color: message.includes("✅") ? "green" : "red",
            fontWeight: "bold",
          }}
        >
          {message}
        </p>
      )}
    </div>
  );
}
