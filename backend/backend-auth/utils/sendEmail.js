import nodemailer from "nodemailer";

const sendEmail = async (to, subject, html) => {
  try {
    console.log("📨 Đang gửi mail tới:", to);

    const transporter = nodemailer.createTransport({
      host: "smtp.gmail.com",
      port: 465,
      secure: true,
      auth: {
        user: process.env.EMAIL_USER,
        pass: process.env.EMAIL_PASS,
      },
      tls: {
        rejectUnauthorized: false, // ⚠️ thêm dòng này
      },
    });

    const info = await transporter.sendMail({
      from: `"Group16 Support" <${process.env.EMAIL_USER}>`,
      to,
      subject,
      html,
    });

    console.log("✅ Email gửi thành công:", info.messageId);
  } catch (err) {
    console.error("❌ Lỗi gửi email:", err);
  }
};

export default sendEmail;