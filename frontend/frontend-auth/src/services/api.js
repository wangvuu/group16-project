import axios from "axios";

// ✅ Khởi tạo axios instance
const API = axios.create({
  baseURL: "http://localhost:5000/api",
  headers: { "Content-Type": "application/json" },
});

// 🧩 Gắn accessToken vào mọi request
API.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// 🧩 Tự động refresh token khi hết hạn
API.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      const refreshToken = localStorage.getItem("refreshToken");
      if (!refreshToken) {
        window.location.href = "/login";
        return Promise.reject(error);
      }

      try {
        const { data } = await axios.post(
          "http://localhost:5000/api/auth/refresh",
          { refreshToken }
        );

        // ✅ Cập nhật token mới
        localStorage.setItem("accessToken", data.accessToken);
        API.defaults.headers.common["Authorization"] = `Bearer ${data.accessToken}`;

        // ✅ Thực hiện lại request cũ
        return API(originalRequest);
      } catch (refreshErr) {
        console.error("❌ Refresh token thất bại:", refreshErr);
        localStorage.clear();
        window.location.href = "/login";
      }
    }
    return Promise.reject(error);
  }
);

//
// ===== 🧩 AUTH APIs =====
//
export const signup = (data) => API.post("/auth/signup", data);
export const login = (data) => API.post("/auth/login", data);
export const refreshTokenAPI = (refreshToken) =>
  API.post("/auth/refresh", { refreshToken });
export const logoutAPI = (refreshToken) =>
  API.post("/auth/logout", { refreshToken });

//
// ===== 🧩 PASSWORD APIs =====
//
export const forgotPassword = (data) =>
  API.post("/profile/forgot-password", data);
export const resetPassword = (token, data) =>
  API.post(`/profile/reset-password/${token}`, data);

//
// ===== 🧩 USER APIs =====
//
export const getProfile = () => API.get("/profile");
export const updateProfile = (data) => API.put("/profile", data);

// ✅ Upload Avatar (Hoạt động 3)
export const uploadAvatar = (formData) =>
  API.put("/profile/upload-avatar", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });

export const getUsers = () => API.get("/users");
export const deleteUser = (id) => API.delete(`/users/${id}`);

export default API;
