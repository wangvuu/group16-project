import axios from "axios";

const API = axios.create({
  baseURL: process.env.REACT_APP_API_URL || "http://localhost:5000/api",
});

// 🧩 Gắn Access Token vào mọi request
API.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("accessToken");
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => Promise.reject(error)
);

// 🧩 Tự động refresh token khi Access Token hết hạn
API.interceptors.response.use(
  (res) => res,
  async (error) => {
    const original = error.config;
    if (error.response?.status === 403 && !original._retry) {
      original._retry = true;
      try {
        const refreshToken = localStorage.getItem("refreshToken");
        const res = await axios.post(
          `${process.env.REACT_APP_API_URL || "http://localhost:5000/api"}/auth/refresh`,
          { refreshToken }
        );
        localStorage.setItem("accessToken", res.data.accessToken);
        original.headers.Authorization = `Bearer ${res.data.accessToken}`;
        return API(original);
      } catch (err) {
        localStorage.clear();
        window.location.href = "/login";
      }
    }
    return Promise.reject(error);
  }
);

// 🧩 AUTH
export const signup = (data) => API.post("/auth/signup", data);
export const login = (data) => API.post("/auth/login", data);
export const refreshToken = (data) => API.post("/auth/refresh", data);
export const logout = (data) => API.post("/auth/logout", data);

// 🧩 PASSWORD
export const forgotPassword = (data) => API.post("/profile/forgot-password", data);
export const resetPassword = (token, data) => API.post(`/profile/reset-password/${token}`, data);

// 🧩 PROFILE
export const getProfile = () => API.get("/profile");
export const updateProfile = (data) => API.put("/profile", data);
export const uploadAvatar = (formData) =>
  API.put("/profile/upload-avatar", formData, {
    headers: { "Content-Type": "multipart/form-data" },
  });

// 🧩 ADMIN
export const getUsers = () => API.get("/users");
export const deleteUser = (id) => API.delete(`/users/${id}`);

export default API;