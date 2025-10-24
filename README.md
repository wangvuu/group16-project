NHÓM 16

Mô tả dự án:
Dự án xây dựng một ứng dụng web quản lý người dùng nâng cao với các tính năng từ cơ bản đến mở rộng. Ứng dụng giúp người dùng đăng ký, đăng nhập, cập nhật thông tin cá nhân, tải ảnh đại diện, quản lý phân quyền, gửi mail quên mật khẩu và theo dõi lịch sử hoạt động.

Hệ thống được phát triển theo mô hình client–server với:

Backend sử dụng Node.js + Express.

Frontend phát triển bằng React và Redux Toolkit.

Database sử dụng MongoDB Atlas.

Triển khai, quản lý mã nguồn bằng GitHub.

Công nghệ sử dụng:

Backend: Node.js, Express, Mongoose.

Frontend: React, Redux Toolkit, Axios.

Database: MongoDB Atlas.

Công cụ hỗ trợ: Git, GitHub, Postman, VS Code.

Cách chạy dự án:

Backend:

Mở thư mục backend.

Chạy lệnh:

npm install

npm run dev

Tạo file .env và thêm các thông tin:
PORT=5000
MONGO_URI=...
JWT_SECRET=mysecretkey
EMAIL_USER=...
EMAIL_PASS=...
CLOUD_NAME=...
CLOUD_API_KEY=...
CLOUD_API_SECRET=...

Frontend:

Mở thư mục frontend.

Chạy lệnh:

npm install

npm start

Tạo file .env:
REACT_APP_API_URL=http://localhost:5000

Các hoạt động thực hiện:

Hoạt động 1 – Refresh Token & Session Management:
Tạo cơ chế JWT Access Token và Refresh Token giúp duy trì phiên đăng nhập an toàn. API /auth/refresh sinh token mới khi access token hết hạn, lưu refresh token trong MongoDB. Frontend tự động gọi API để làm mới token.
Kết quả: Người dùng vẫn duy trì đăng nhập ngay cả khi access token hết hạn.

Hoạt động 2 – Phân quyền nâng cao (RBAC):
Phân quyền truy cập theo vai trò User, Admin, Moderator. Middleware checkRole() được dùng để kiểm tra quyền trước khi truy cập API. Frontend hiển thị các tính năng tương ứng với từng vai trò.
Kết quả: Admin có thể xem danh sách người dùng và log, user thường không thể truy cập.

Hoạt động 3 – Upload Ảnh Đại Diện (Avatar):
Cho phép người dùng tải ảnh đại diện, resize bằng Sharp và lưu lên Cloudinary. API /users/avatar xử lý upload ảnh và lưu URL vào MongoDB. Frontend hiển thị ảnh sau khi tải thành công.
Kết quả: Ảnh đại diện được lưu thành công lên Cloudinary và hiển thị trên trang cá nhân.

Hoạt động 4 – Quên Mật Khẩu & Reset Mật Khẩu:
Người dùng có thể yêu cầu đặt lại mật khẩu. API /auth/forgot-password gửi email chứa link đặt lại mật khẩu thật qua Gmail SMTP, còn /auth/reset-password/:token giúp người dùng tạo mật khẩu mới.
Kết quả: Email được gửi thành công, người dùng đặt lại mật khẩu mới và đăng nhập lại được.

Hoạt động 5 – Logging & Rate Limiting:
Ghi lại hoạt động người dùng (login, upload, cập nhật…) bằng middleware logActivity. Dữ liệu lưu vào collection logs trong MongoDB. Đồng thời áp dụng giới hạn số lần đăng nhập sai để chống brute-force.
Kết quả: Admin xem được danh sách log, login sai quá nhiều lần sẽ bị tạm chặn.

Hoạt động 6 – Redux & Protected Routes:
Frontend sử dụng Redux Toolkit để quản lý state người dùng. Component ProtectedRoute kiểm tra trạng thái đăng nhập và chặn truy cập khi chưa login. Khi logout, Redux reset state và redirect về trang login.
Kết quả: Giao diện bảo mật hơn, chỉ người dùng đã đăng nhập mới truy cập được các trang riêng.

Hoạt động 7 – Tổng hợp & Merge vào Main:
Tích hợp tất cả các nhánh tính năng (feature/...) vào nhánh main. Test đầy đủ toàn bộ luồng: đăng ký, đăng nhập, refresh token, upload avatar, reset mật khẩu, logging và phân quyền. Cập nhật README.md và chuẩn bị sản phẩm nộp.
Kết quả: Dự án hoàn thiện toàn bộ chức năng, hoạt động ổn định cả frontend và backend.

Kiểm thử chức năng:

Đăng ký người dùng mới: hoạt động tốt.

Đăng nhập và xác thực token: thành công.

Refresh token tự động: hoạt động đúng.

Upload ảnh lên Cloudinary: hoàn tất.

Gửi email quên mật khẩu: nhận được email thật.

Reset mật khẩu: đổi được mật khẩu mới.

Xem logs: chỉ admin có quyền truy cập.

Phân quyền theo vai trò: kiểm tra hợp lệ.

Redux và Protected Routes: redirect và lưu state chính xác.

Triển khai (Deployment):

Backend: Render hoặc Railway.

Frontend: Vercel.

Database: MongoDB Atlas.

Kết quả: Hệ thống hoạt động ổn định online, kết nối giữa frontend, backend và database chính xác.

Cấu trúc thư mục chính:

Thư mục backend gồm:
controllers, middlewares, models, routes, utils, server.js.

Thư mục frontend gồm:
redux, components, pages, App.js, ProtectedRoute.js, package.json.

Thành viên nhóm 16:

SV1 – Nguyễn Quang Vương: Backend Advanced (Auth, Token, Logs, Middleware).
SV2 – Lâm Nhật Hào: Database & Integration (MongoDB, Cloudinary, Nodemailer).
SV3 – Phạm Cao Kiệt (is cao lạc bu ku 😄): Frontend (React, Redux, Protected Routes).

Sản phẩm nộp:

Repo GitHub: https://github.com/wangvuu/group16-project.git

Video demo: Quay đầy đủ flow đăng ký, đăng nhập, upload ảnh, reset mật khẩu, xem log.

Ảnh chụp: Giao diện frontend, trang đăng nhập, upload avatar, xem logs