import express from "express";
import { getAllLogs } from "../controllers/logController.js";
import { verifyToken, isAdmin } from "../middleware/authMiddleware.js";

const router = express.Router();

router.get("/", verifyToken, isAdmin, getAllLogs);

export default router;
