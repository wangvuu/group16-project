import axios from "axios";

const API = axios.create({
  baseURL: "http://localhost:5000/api",
  headers: { "Content-Type": "application/json" },
});

API.interceptors.request.use((config) => {
  const token = localStorage.getItem("accessToken");
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// ✅ Tự động refresh token khi hết hạn
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

        localStorage.setItem("accessToken", data.accessToken);
        API.defaults.headers.common[
          "Authorization"
        ] = `Bearer ${data.accessToken}`;

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

// ===== API =====
export const signup = (data) => API.post("/auth/signup", data);
export const login = (data) => API.post("/auth/login", data);
export const refreshTokenAPI = (refreshToken) =>
  API.post("/auth/refresh", { refreshToken });
export const logoutAPI = (refreshToken) =>
  API.post("/auth/logout", { refreshToken });

// ✅ Quên mật khẩu / Đặt lại mật khẩu
export const forgotPassword = (data) =>
  API.post("/profile/forgot-password", data);
export const resetPassword = (token, data) =>
  API.post(`/profile/reset-password/${token}`, data);

// ===== User APIs =====
export const getProfile = () => API.get("/profile");
export const updateProfile = (data) => API.put("/profile", data);
export const getUsers = () => API.get("/users");
export const deleteUser = (id) => API.delete(`/users/${id}`);