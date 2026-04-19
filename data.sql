--
-- PostgreSQL database dump
--

\restrict lMJ06JcUeA8b49S3BS7xFJlLlndjQYsjog2FYDtlmfKDhPebs4HclhkWEIfy0g4

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

-- Started on 2026-04-19 16:44:12

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 243 (class 1259 OID 35628)
-- Name: bao_hiem_y_te; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.bao_hiem_y_te (
    id integer NOT NULL,
    benh_nhan_id integer,
    ma_the character varying(50) NOT NULL,
    han_su_dung date,
    noi_dang_ky_id integer,
    dia_chi_dang_ky text,
    muc_huong integer DEFAULT 80,
    is_active boolean DEFAULT true,
    created_at timestamp without time zone DEFAULT now()
);


ALTER TABLE public.bao_hiem_y_te OWNER TO postgres;

--
-- TOC entry 242 (class 1259 OID 35627)
-- Name: bao_hiem_y_te_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.bao_hiem_y_te_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.bao_hiem_y_te_id_seq OWNER TO postgres;

--
-- TOC entry 5199 (class 0 OID 0)
-- Dependencies: 242
-- Name: bao_hiem_y_te_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.bao_hiem_y_te_id_seq OWNED BY public.bao_hiem_y_te.id;


--
-- TOC entry 241 (class 1259 OID 35592)
-- Name: benh_nhan; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.benh_nhan (
    id integer NOT NULL,
    ma_benh_nhan character varying(20) NOT NULL,
    so_ho_so character varying(20),
    ho_ten character varying(200) NOT NULL,
    ngay_sinh date,
    gioi_tinh character varying(10),
    so_dien_thoai character varying(20),
    nguoi_than character varying(200),
    sdt_nguoi_than character varying(20),
    cccd character varying(20),
    quoc_tich_id integer,
    dan_toc_id integer,
    nghe_nghiep_id integer,
    tinh_thanh_id integer,
    dia_chi text,
    created_at timestamp without time zone DEFAULT now(),
    updated_at timestamp without time zone DEFAULT now()
);


ALTER TABLE public.benh_nhan OWNER TO postgres;

--
-- TOC entry 240 (class 1259 OID 35591)
-- Name: benh_nhan_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.benh_nhan_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.benh_nhan_id_seq OWNER TO postgres;

--
-- TOC entry 5200 (class 0 OID 0)
-- Dependencies: 240
-- Name: benh_nhan_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.benh_nhan_id_seq OWNED BY public.benh_nhan.id;


--
-- TOC entry 229 (class 1259 OID 35529)
-- Name: dm_dan_toc; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dm_dan_toc (
    id integer NOT NULL,
    ten character varying(100) NOT NULL
);


ALTER TABLE public.dm_dan_toc OWNER TO postgres;

--
-- TOC entry 228 (class 1259 OID 35528)
-- Name: dm_dan_toc_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dm_dan_toc_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.dm_dan_toc_id_seq OWNER TO postgres;

--
-- TOC entry 5201 (class 0 OID 0)
-- Dependencies: 228
-- Name: dm_dan_toc_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dm_dan_toc_id_seq OWNED BY public.dm_dan_toc.id;


--
-- TOC entry 237 (class 1259 OID 35565)
-- Name: dm_dich_vu; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dm_dich_vu (
    id integer NOT NULL,
    ten character varying(200) NOT NULL,
    don_gia numeric(15,2) DEFAULT 0 NOT NULL,
    mo_ta text,
    is_active boolean DEFAULT true
);


ALTER TABLE public.dm_dich_vu OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 35564)
-- Name: dm_dich_vu_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dm_dich_vu_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.dm_dich_vu_id_seq OWNER TO postgres;

--
-- TOC entry 5202 (class 0 OID 0)
-- Dependencies: 236
-- Name: dm_dich_vu_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dm_dich_vu_id_seq OWNED BY public.dm_dich_vu.id;


--
-- TOC entry 231 (class 1259 OID 35538)
-- Name: dm_nghe_nghiep; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dm_nghe_nghiep (
    id integer NOT NULL,
    ten character varying(100) NOT NULL
);


ALTER TABLE public.dm_nghe_nghiep OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 35537)
-- Name: dm_nghe_nghiep_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dm_nghe_nghiep_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.dm_nghe_nghiep_id_seq OWNER TO postgres;

--
-- TOC entry 5203 (class 0 OID 0)
-- Dependencies: 230
-- Name: dm_nghe_nghiep_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dm_nghe_nghiep_id_seq OWNED BY public.dm_nghe_nghiep.id;


--
-- TOC entry 235 (class 1259 OID 35556)
-- Name: dm_noi_dang_ky_the; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dm_noi_dang_ky_the (
    id integer NOT NULL,
    ten character varying(200) NOT NULL
);


ALTER TABLE public.dm_noi_dang_ky_the OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 35555)
-- Name: dm_noi_dang_ky_the_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dm_noi_dang_ky_the_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.dm_noi_dang_ky_the_id_seq OWNER TO postgres;

--
-- TOC entry 5204 (class 0 OID 0)
-- Dependencies: 234
-- Name: dm_noi_dang_ky_the_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dm_noi_dang_ky_the_id_seq OWNED BY public.dm_noi_dang_ky_the.id;


--
-- TOC entry 227 (class 1259 OID 35520)
-- Name: dm_quoc_tich; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dm_quoc_tich (
    id integer NOT NULL,
    ten character varying(100) NOT NULL
);


ALTER TABLE public.dm_quoc_tich OWNER TO postgres;

--
-- TOC entry 226 (class 1259 OID 35519)
-- Name: dm_quoc_tich_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dm_quoc_tich_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.dm_quoc_tich_id_seq OWNER TO postgres;

--
-- TOC entry 5205 (class 0 OID 0)
-- Dependencies: 226
-- Name: dm_quoc_tich_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dm_quoc_tich_id_seq OWNED BY public.dm_quoc_tich.id;


--
-- TOC entry 239 (class 1259 OID 35579)
-- Name: dm_thuoc; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dm_thuoc (
    id integer NOT NULL,
    ten character varying(200) NOT NULL,
    don_vi character varying(50) DEFAULT 'viên'::character varying,
    don_gia numeric(15,2) DEFAULT 0 NOT NULL,
    is_active boolean DEFAULT true
);


ALTER TABLE public.dm_thuoc OWNER TO postgres;

--
-- TOC entry 238 (class 1259 OID 35578)
-- Name: dm_thuoc_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dm_thuoc_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.dm_thuoc_id_seq OWNER TO postgres;

--
-- TOC entry 5206 (class 0 OID 0)
-- Dependencies: 238
-- Name: dm_thuoc_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dm_thuoc_id_seq OWNED BY public.dm_thuoc.id;


--
-- TOC entry 233 (class 1259 OID 35547)
-- Name: dm_tinh_thanh; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.dm_tinh_thanh (
    id integer NOT NULL,
    ten character varying(100) NOT NULL
);


ALTER TABLE public.dm_tinh_thanh OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 35546)
-- Name: dm_tinh_thanh_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.dm_tinh_thanh_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.dm_tinh_thanh_id_seq OWNER TO postgres;

--
-- TOC entry 5207 (class 0 OID 0)
-- Dependencies: 232
-- Name: dm_tinh_thanh_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.dm_tinh_thanh_id_seq OWNED BY public.dm_tinh_thanh.id;


--
-- TOC entry 245 (class 1259 OID 35652)
-- Name: ho_so_kham; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ho_so_kham (
    id integer NOT NULL,
    benh_nhan_id integer,
    bao_hiem_id integer,
    ngay_vao date DEFAULT CURRENT_DATE,
    ngay_ra date,
    chan_doan text,
    ket_luan text,
    hinh_thuc_ket_thuc character varying(50) DEFAULT 'Ra viện'::character varying,
    trang_thai character varying(30) DEFAULT 'Chờ khám'::character varying,
    bac_si_id integer,
    tong_tien_thuoc numeric(15,2) DEFAULT 0,
    tong_tien_dich_vu numeric(15,2) DEFAULT 0,
    created_at timestamp without time zone DEFAULT now(),
    updated_at timestamp without time zone DEFAULT now()
);


ALTER TABLE public.ho_so_kham OWNER TO postgres;

--
-- TOC entry 244 (class 1259 OID 35651)
-- Name: ho_so_kham_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.ho_so_kham_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.ho_so_kham_id_seq OWNER TO postgres;

--
-- TOC entry 5208 (class 0 OID 0)
-- Dependencies: 244
-- Name: ho_so_kham_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.ho_so_kham_id_seq OWNED BY public.ho_so_kham.id;


--
-- TOC entry 247 (class 1259 OID 35684)
-- Name: ke_thuoc; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.ke_thuoc (
    id integer NOT NULL,
    ho_so_kham_id integer,
    thuoc_id integer,
    so_luong integer DEFAULT 1 NOT NULL,
    lieu_dung character varying(200),
    don_gia numeric(15,2),
    thanh_tien numeric(15,2),
    ghi_chu text
);


ALTER TABLE public.ke_thuoc OWNER TO postgres;

--
-- TOC entry 246 (class 1259 OID 35683)
-- Name: ke_thuoc_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.ke_thuoc_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.ke_thuoc_id_seq OWNER TO postgres;

--
-- TOC entry 5209 (class 0 OID 0)
-- Dependencies: 246
-- Name: ke_thuoc_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.ke_thuoc_id_seq OWNED BY public.ke_thuoc.id;


--
-- TOC entry 222 (class 1259 OID 32952)
-- Name: refresh_tokens; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.refresh_tokens (
    id integer NOT NULL,
    user_id integer,
    token text NOT NULL,
    expires_at timestamp without time zone NOT NULL,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    is_revoked boolean DEFAULT false
);


ALTER TABLE public.refresh_tokens OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 32951)
-- Name: refresh_tokens_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.refresh_tokens_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.refresh_tokens_id_seq OWNER TO postgres;

--
-- TOC entry 5210 (class 0 OID 0)
-- Dependencies: 221
-- Name: refresh_tokens_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.refresh_tokens_id_seq OWNED BY public.refresh_tokens.id;


--
-- TOC entry 223 (class 1259 OID 34383)
-- Name: seq_ma_bn; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.seq_ma_bn
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.seq_ma_bn OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 34509)
-- Name: seq_so_hsba; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.seq_so_hsba
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.seq_so_hsba OWNER TO postgres;

--
-- TOC entry 224 (class 1259 OID 34465)
-- Name: seq_so_tt; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.seq_so_tt
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.seq_so_tt OWNER TO postgres;

--
-- TOC entry 249 (class 1259 OID 35706)
-- Name: su_dung_dich_vu; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.su_dung_dich_vu (
    id integer NOT NULL,
    ho_so_kham_id integer,
    dich_vu_id integer,
    so_luong integer DEFAULT 1,
    don_gia numeric(15,2),
    thanh_tien numeric(15,2),
    trang_thai character varying(30) DEFAULT 'Chờ xử lý'::character varying,
    ghi_chu text,
    created_at timestamp without time zone DEFAULT now()
);


ALTER TABLE public.su_dung_dich_vu OWNER TO postgres;

--
-- TOC entry 248 (class 1259 OID 35705)
-- Name: su_dung_dich_vu_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.su_dung_dich_vu_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.su_dung_dich_vu_id_seq OWNER TO postgres;

--
-- TOC entry 5211 (class 0 OID 0)
-- Dependencies: 248
-- Name: su_dung_dich_vu_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.su_dung_dich_vu_id_seq OWNED BY public.su_dung_dich_vu.id;


--
-- TOC entry 220 (class 1259 OID 32933)
-- Name: users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.users (
    id integer NOT NULL,
    username character varying(50) NOT NULL,
    email character varying(100) NOT NULL,
    password_hash text NOT NULL,
    role character varying(20) DEFAULT 'user'::character varying,
    created_at timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.users OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 32932)
-- Name: users_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.users_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.users_id_seq OWNER TO postgres;

--
-- TOC entry 5212 (class 0 OID 0)
-- Dependencies: 219
-- Name: users_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;


--
-- TOC entry 4945 (class 2604 OID 35631)
-- Name: bao_hiem_y_te id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bao_hiem_y_te ALTER COLUMN id SET DEFAULT nextval('public.bao_hiem_y_te_id_seq'::regclass);


--
-- TOC entry 4942 (class 2604 OID 35595)
-- Name: benh_nhan id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.benh_nhan ALTER COLUMN id SET DEFAULT nextval('public.benh_nhan_id_seq'::regclass);


--
-- TOC entry 4931 (class 2604 OID 35532)
-- Name: dm_dan_toc id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_dan_toc ALTER COLUMN id SET DEFAULT nextval('public.dm_dan_toc_id_seq'::regclass);


--
-- TOC entry 4935 (class 2604 OID 35568)
-- Name: dm_dich_vu id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_dich_vu ALTER COLUMN id SET DEFAULT nextval('public.dm_dich_vu_id_seq'::regclass);


--
-- TOC entry 4932 (class 2604 OID 35541)
-- Name: dm_nghe_nghiep id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_nghe_nghiep ALTER COLUMN id SET DEFAULT nextval('public.dm_nghe_nghiep_id_seq'::regclass);


--
-- TOC entry 4934 (class 2604 OID 35559)
-- Name: dm_noi_dang_ky_the id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_noi_dang_ky_the ALTER COLUMN id SET DEFAULT nextval('public.dm_noi_dang_ky_the_id_seq'::regclass);


--
-- TOC entry 4930 (class 2604 OID 35523)
-- Name: dm_quoc_tich id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_quoc_tich ALTER COLUMN id SET DEFAULT nextval('public.dm_quoc_tich_id_seq'::regclass);


--
-- TOC entry 4938 (class 2604 OID 35582)
-- Name: dm_thuoc id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_thuoc ALTER COLUMN id SET DEFAULT nextval('public.dm_thuoc_id_seq'::regclass);


--
-- TOC entry 4933 (class 2604 OID 35550)
-- Name: dm_tinh_thanh id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_tinh_thanh ALTER COLUMN id SET DEFAULT nextval('public.dm_tinh_thanh_id_seq'::regclass);


--
-- TOC entry 4949 (class 2604 OID 35655)
-- Name: ho_so_kham id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ho_so_kham ALTER COLUMN id SET DEFAULT nextval('public.ho_so_kham_id_seq'::regclass);


--
-- TOC entry 4957 (class 2604 OID 35687)
-- Name: ke_thuoc id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ke_thuoc ALTER COLUMN id SET DEFAULT nextval('public.ke_thuoc_id_seq'::regclass);


--
-- TOC entry 4927 (class 2604 OID 32955)
-- Name: refresh_tokens id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.refresh_tokens ALTER COLUMN id SET DEFAULT nextval('public.refresh_tokens_id_seq'::regclass);


--
-- TOC entry 4959 (class 2604 OID 35709)
-- Name: su_dung_dich_vu id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.su_dung_dich_vu ALTER COLUMN id SET DEFAULT nextval('public.su_dung_dich_vu_id_seq'::regclass);


--
-- TOC entry 4924 (class 2604 OID 32936)
-- Name: users id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq'::regclass);


--
-- TOC entry 5187 (class 0 OID 35628)
-- Dependencies: 243
-- Data for Name: bao_hiem_y_te; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.bao_hiem_y_te (id, benh_nhan_id, ma_the, han_su_dung, noi_dang_ky_id, dia_chi_dang_ky, muc_huong, is_active, created_at) FROM stdin;
36	8	HS17755757575557	2027-01-01	1	ct	80	f	2026-04-04 10:35:40.933118
37	8	HS17755757575557	2027-01-01	1	ct	95	f	2026-04-04 10:35:46.099085
38	8	HS12368888888888	2027-01-01	2	hgjjg	80	t	2026-04-04 10:36:16.608305
42	22	HS29008503850385	2027-01-01	4	fg	80	f	2026-04-18 05:57:10.095639
1	4	53535353	2027-01-01	1	can tho	80	f	2026-03-29 20:55:50.829386
2	4	53535353	2027-01-01	1	can tho	80	f	2026-03-29 20:55:55.002445
3	4	HS01324242424242	2029-01-01	5	cần thơ	80	f	2026-03-30 14:52:16.952352
4	4	HS01324242424242	2029-01-01	5	cần thơ	95	f	2026-03-30 14:52:20.067155
5	4	HS01324242424242	2029-01-01	5	cần thơ	100	f	2026-03-30 14:52:23.157484
6	4	HS01324242424242	2029-01-01	5	cần thơ	100	f	2026-03-30 14:52:26.692216
7	4	HS01224353535353	2029-01-01	4	cần thơ	80	f	2026-03-30 14:53:44.044703
8	4	HS15454545454545	2029-01-01	6	cần thơ	80	f	2026-03-30 14:56:53.448361
9	4	HS15454545454545	2029-01-01	6	cần thơ	80	f	2026-03-30 14:56:57.713929
10	4	HS15454545454545	2029-01-01	6	cần thơ	95	f	2026-03-30 14:57:00.640834
11	4	HS15454545454545	2029-01-01	6	cần thơ	100	f	2026-03-30 14:57:04.296214
12	4	HS15454545454545	2029-01-01	6	cần thơ	100	f	2026-03-30 14:57:07.931081
13	4	HS12244242422424	2029-01-01	5	ct	80	f	2026-03-30 15:00:35.345813
14	4	HS12244242422424	2029-01-01	5	ct	100	f	2026-03-30 15:00:43.457125
15	4	HS12244242422424	2029-01-01	5	ct	100	f	2026-03-30 15:01:11.209654
16	4	HS12545454545454	2029-09-01	6	ct	80	f	2026-03-30 15:08:25.802971
17	4	HS14734934937493	2029-01-01	5	tphcm	80	f	2026-03-30 15:12:30.791125
18	4	HS14734934937493	2029-01-01	5	tphcm	80	f	2026-03-30 15:12:34.627126
21	4	HS14839843948394	2030-01-01	3	ct	80	f	2026-03-30 15:20:17.750564
27	4	HS12244424244242	2029-01-01	4	ct	80	f	2026-03-30 15:40:41.318498
30	4	HS15353535353535	2029-12-01	3	ct	80	f	2026-03-30 15:50:02.157745
32	4	HS24472472942424	2038-12-01	3	ctere	80	f	2026-03-30 15:55:01.541943
19	4	HS14734934937493	2029-01-01	5	tphcm	95	f	2026-03-30 15:12:36.814887
20	4	HS14734934937493	2029-01-01	5	tphcm	95	f	2026-03-30 15:12:47.346075
22	4	HS14839843948394	2030-01-01	3	ct	80	f	2026-03-30 15:20:20.106393
23	4	HS13434343434343	2029-01-01	6	ct	80	f	2026-03-30 15:28:12.605996
24	4	HS13434343434343	2029-01-01	6	ct	100	f	2026-03-30 15:28:21.383495
25	4	HS14794297429472	2029-11-01	4	ct	80	f	2026-03-30 15:36:54.097845
26	4	HS35858757575757	2027-01-01	1	ct	80	f	2026-03-30 15:37:22.129173
28	4	HS15353535353535	2029-01-01	3	ct	80	f	2026-03-30 15:48:28.725304
29	4	HS15353535353535	2029-12-01	3	ct	80	f	2026-03-30 15:48:38.576213
31	4	HS14335544646466	2040-11-01	5	brvt	80	f	2026-03-30 15:53:02.262134
33	4	HS24472472942424	2038-12-01	3	thành phố Hồ Chí Minh	80	t	2026-03-30 15:59:21.541415
39	22	HS25555555555555	2028-01-01	4	ctyuuu	80	f	2026-04-18 05:30:44.463197
40	22	HS25555555555555	2028-01-01	4	ctyuuu	80	f	2026-04-18 05:30:47.240813
41	22	HS18858585885885	2028-01-01	4	ctyuuu	80	f	2026-04-18 05:31:00.964282
43	22	HS29008503850385	2027-01-01	4	fgggg	80	t	2026-04-18 06:07:28.928495
\.


--
-- TOC entry 5185 (class 0 OID 35592)
-- Dependencies: 241
-- Data for Name: benh_nhan; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.benh_nhan (id, ma_benh_nhan, so_ho_so, ho_ten, ngay_sinh, gioi_tinh, so_dien_thoai, nguoi_than, sdt_nguoi_than, cccd, quoc_tich_id, dan_toc_id, nghe_nghiep_id, tinh_thanh_id, dia_chi, created_at, updated_at) FROM stdin;
1	BN-2025-001	HS-0001	Nguyễn Văn An	1990-01-01	Nam	0901234567	\N	\N	012345678901	1	1	2	4	12 Nguyễn Trãi, Ninh Kiều, Cần Thơ	2026-03-29 20:07:00.007984	2026-03-29 20:07:00.007984
2	BN-2025-002	HS-0002	Trần Thị Bình	1985-05-15	Nữ	0912345678	\N	\N	234567890123	1	1	4	4	45 Lý Tự Trọng, Ninh Kiều, Cần Thơ	2026-03-29 20:07:00.007984	2026-03-29 20:07:00.007984
3	BN-2025-003	HS-0003	Lê Văn Cường	1978-08-22	Nam	0923456789	\N	\N	345678901234	1	1	3	4	78 Trần Phú, Bình Thủy, Cần Thơ	2026-03-29 20:07:00.007984	2026-03-29 20:07:00.007984
4	BN-2026-0004		Quang	\N	Nam	0898047036	nguyen	0898044333	898948390483	1	1	4	2	can tho	2026-03-29 20:55:00.468283	2026-04-01 14:55:12.589786
6	BN-2026-0005	HS-2026-0005	Lâm Nhật Hào	2004-01-22	Nam	0898043948	Phạm Cao Lạc	0975738535	353643646464	1	1	1	4	ctdgdgg	2026-04-03 20:46:54.674234	2026-04-03 20:59:25.271798
7	BN-2026-0006	HS-2026-0006	Pham Cao Lac	2004-01-22	Nam	0797997979	kiet	0788686868	646462626464	1	1	1	4	ct	2026-04-04 10:17:38.773409	2026-04-04 10:17:38.773409
8	BN-2026-0007	HS-2026-0008	Pham	2004-01-22	Nam	0979797979	vtdd	0878787878	767475757575	1	1	2	1	gdg	2026-04-04 10:19:13.797013	2026-04-04 10:21:43.692513
9	BN-2026-0008	HS-2026-0008	Ơiii	2004-11-22	Nam	0000000000	a	0000000000	000000000000	1	1	8	5	fffffffffff	2026-04-04 10:28:01.249453	2026-04-04 10:37:14.821298
10	BN-2026-0009	HS-2026-0009	Lâm Hào	2004-01-20	Nam	0898999999	Phạm cao lạc	0755555555	363444444444	1	40	1	1	dfgdg	2026-04-07 20:51:05.028144	2026-04-07 20:51:05.028144
11	BN-2026-0010	HS-2026-0010	Cao	2004-01-20	Nam	02666666666	Lam	0899999999	666666666666	1	3	1	1	gdgd	2026-04-07 21:22:18.357525	2026-04-07 21:22:18.357525
12	BN-2026-0011	HS-2026-0011	Nguyen	2004-11-20	Nam	02777777777	Se	0888888888	533333333333	2	3	\N	4	dgdgdg	2026-04-07 21:33:45.573981	2026-04-07 21:33:45.573981
13	BN-2026-0012	HS-2026-0012	Ghian	2004-01-22	Nam	0688888888	Dgd	6333333333	466666666666	1	3	6	4	gdgdg	2026-04-07 21:45:09.29208	2026-04-07 21:45:09.29208
14	BN-2026-0013	HS-2026-0013	Wang	2004-02-22	Nam	0666666666	Hao	0277777777	555555555555	1	1	6	4	êttetet	2026-04-07 21:51:19.350299	2026-04-07 21:51:19.350299
15	BN-2026-0014	HS-2026-0014	Hao	2004-11-22	Nam	02777777777	Gai	0566666666	355555555555	1	1	4	4	gdgdg	2026-04-07 21:55:48.047629	2026-04-07 21:55:48.047629
16	BN-2026-0015	HS-2026-0016-03	Kiet Lạc . Com.Com	2004-09-15	Nam	0898047036	Lam	0989544345	123456789234	5	24	6	6	camphuchia	2026-04-11 09:55:09.955858	2026-04-11 10:35:45.566957
20	BN-2026-0016	HS-2026-0016	Geg	2004-02-12	Nam	0555555555	Dgdg	0893333333	555555555555	1	1	3	4	gegeg	2026-04-11 10:46:53.76223	2026-04-11 10:46:53.76223
22	BN-2026-0017	HS-2026-0022-04	Lamnhathaozggg	2004-11-22	Nam	0898989898	Phamlac	0777777777	888888888888	1	1	6	1	hihihdddddddddhhhh	2026-04-18 05:27:01.885702	2026-04-18 09:22:45.044024
\.


--
-- TOC entry 5173 (class 0 OID 35529)
-- Dependencies: 229
-- Data for Name: dm_dan_toc; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.dm_dan_toc (id, ten) FROM stdin;
1	Kinh
2	Tày
3	Thái
4	Mường
5	Khmer
6	Hoa
7	Nùng
8	Khác
9	Dao
10	Gia Rai
11	Ê Đê
12	Ba Na
13	Sán Chay
14	Chăm
15	Xê Đăng
16	Sán Dìu
17	Hrê
18	Ra Glai
19	Mnông
20	Thổ
21	Stiêng
22	Khơ Mú
23	Bru - Vân Kiều
24	Cơ Tu
25	Giáy
26	Tà Ôi
27	Mạ
28	Giẻ Triêng
29	Co
30	Chơ Ro
31	Xinh Mun
32	Hà Nhì
33	Chu Ru
34	Lào
35	La Chí
36	Kháng
37	Phù Lá
38	La Hủ
39	La Ha
40	Pà Thẻn
41	Lự
42	Ngái
43	Chứt
44	Lô Lô
45	Mảng
46	Cờ Lao
47	Bố Y
48	Cống
49	Si La
50	Pu Péo
\.


--
-- TOC entry 5181 (class 0 OID 35565)
-- Dependencies: 237
-- Data for Name: dm_dich_vu; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.dm_dich_vu (id, ten, don_gia, mo_ta, is_active) FROM stdin;
1	Xét nghiệm máu	120000.00	\N	t
2	Chụp X-quang	200000.00	\N	t
3	Siêu âm bụng	150000.00	\N	t
4	Siêu âm tim	250000.00	\N	t
5	CT-Scan	800000.00	\N	t
6	MRI	1500000.00	\N	t
7	Nội soi dạ dày	500000.00	\N	t
8	Điện tim (ECG)	80000.00	\N	t
9	Đo huyết áp 24h	350000.00	\N	t
10	Khác	0.00	\N	t
\.


--
-- TOC entry 5175 (class 0 OID 35538)
-- Dependencies: 231
-- Data for Name: dm_nghe_nghiep; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.dm_nghe_nghiep (id, ten) FROM stdin;
1	Học sinh/Sinh viên
2	Công nhân
3	Nông dân
4	Cán bộ/Công chức
5	Kinh doanh
6	Về hưu
7	Nội trợ
8	Khác
\.


--
-- TOC entry 5179 (class 0 OID 35556)
-- Dependencies: 235
-- Data for Name: dm_noi_dang_ky_the; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.dm_noi_dang_ky_the (id, ten) FROM stdin;
1	BV Đa Khoa TW Cần Thơ
2	BV Nhi Đồng Cần Thơ
3	BV Ung Bướu Cần Thơ
4	BV Phụ Sản Cần Thơ
5	BV Đa Khoa TP.HCM
6	Phòng khám đa khoa
7	Khác
\.


--
-- TOC entry 5171 (class 0 OID 35520)
-- Dependencies: 227
-- Data for Name: dm_quoc_tich; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.dm_quoc_tich (id, ten) FROM stdin;
1	Việt Nam
2	Mỹ
3	Pháp
4	Nhật Bản
5	Hàn Quốc
6	Trung Quốc
7	Khác
\.


--
-- TOC entry 5183 (class 0 OID 35579)
-- Dependencies: 239
-- Data for Name: dm_thuoc; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.dm_thuoc (id, ten, don_vi, don_gia, is_active) FROM stdin;
1	Paracetamol 500mg	viên	1000.00	t
2	Amoxicillin 500mg	viên	3000.00	t
3	Ibuprofen 400mg	viên	2500.00	t
4	Omeprazol 20mg	viên	2500.00	t
5	Vitamin C 1000mg	viên	1500.00	t
6	Metformin 500mg	viên	2000.00	t
7	Amlodipine 5mg	viên	3500.00	t
8	Atorvastatin 20mg	viên	5000.00	t
9	Cefuroxime 500mg	viên	8000.00	t
10	Azithromycin 500mg	viên	12000.00	t
\.


--
-- TOC entry 5177 (class 0 OID 35547)
-- Dependencies: 233
-- Data for Name: dm_tinh_thanh; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.dm_tinh_thanh (id, ten) FROM stdin;
1	Hà Nội
2	TP. Hồ Chí Minh
3	Đà Nẵng
4	Cần Thơ
5	Hải Phòng
6	An Giang
7	Bình Dương
8	Đồng Nai
9	Long An
10	Tiền Giang
11	Vĩnh Long
12	Hậu Giang
13	Sóc Trăng
14	Bạc Liêu
15	Cà Mau
16	Kiên Giang
17	Khác
\.


--
-- TOC entry 5189 (class 0 OID 35652)
-- Dependencies: 245
-- Data for Name: ho_so_kham; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.ho_so_kham (id, benh_nhan_id, bao_hiem_id, ngay_vao, ngay_ra, chan_doan, ket_luan, hinh_thuc_ket_thuc, trang_thai, bac_si_id, tong_tien_thuoc, tong_tien_dich_vu, created_at, updated_at) FROM stdin;
1	4	\N	2026-03-29	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-03-29 20:56:43.216272	2026-03-29 20:56:43.216272
3	4	\N	2026-03-29	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-03-29 20:57:07.161692	2026-03-29 20:57:07.161692
4	4	\N	2026-03-29	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-03-29 20:59:29.978274	2026-03-29 20:59:29.978274
5	4	\N	2026-03-29	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-03-29 21:09:27.330946	2026-03-29 21:09:27.330946
6	4	\N	2026-03-29	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-03-29 21:09:34.620034	2026-03-29 21:09:34.620034
7	4	\N	2026-03-29	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-03-29 21:09:44.154639	2026-03-29 21:09:44.154639
8	4	\N	2026-03-29	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-03-29 21:12:34.370132	2026-03-29 21:12:34.370132
9	4	\N	2026-03-29	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-03-29 21:15:03.388814	2026-03-29 21:15:03.388814
10	4	\N	2026-03-29	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-03-29 21:15:21.899417	2026-03-29 21:15:21.899417
11	4	\N	2026-03-29	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-03-29 21:17:03.95331	2026-03-29 21:17:03.95331
12	4	\N	2026-03-30	2026-03-30	\N		Ra viện	Đã khám	\N	35500.00	550000.00	2026-03-30 09:07:52.475829	2026-03-30 09:20:54.956105
2	4	\N	2026-03-29	2026-03-30	\N	bệnh ỉa	Ra viện	Đã khám	\N	21000.00	2380000.00	2026-03-29 20:56:52.470938	2026-03-30 16:00:29.546995
13	14	\N	2026-04-07	2026-07-04	\N		Ra viện	Đã khám	\N	3500.00	200000.00	2026-04-07 21:51:44.92614	2026-04-07 21:55:09.578055
14	15	\N	2026-04-07	2026-07-04	\N	bệnh ia	Ra viện	Đã khám	\N	11500.00	200000.00	2026-04-07 21:55:58.689385	2026-04-07 21:56:22.919152
15	16	\N	2026-04-11	\N	\N	\N	Ra viện	Chờ khám	\N	0.00	0.00	2026-04-11 10:02:27.924349	2026-04-11 10:02:27.924349
16	16	\N	2026-04-11	\N	Cập nhật thông tin - Số hồ sơ: HS-2026-0016-02		Ra viện	Cập nhật hồ sơ	\N	0.00	0.00	2026-04-11 10:34:58.168631	2026-04-11 10:34:58.168631
17	16	\N	2026-04-11	\N	Cập nhật thông tin - Số hồ sơ: HS-2026-0016-03		Ra viện	Cập nhật hồ sơ	\N	0.00	0.00	2026-04-11 10:35:45.567736	2026-04-11 10:35:45.567736
19	22	\N	2026-04-18	\N	Cập nhật thông tin - Số hồ sơ: HS-2026-0022-02		Ra viện	Cập nhật hồ sơ	\N	0.00	0.00	2026-04-18 05:28:30.900044	2026-04-18 05:28:30.900044
18	22	\N	2026-04-18	2026-04-18	Cập nhật thông tin - Số hồ sơ: HS-2026-0022-01		Ra viện	Đã khám	\N	8500.00	1750000.00	2026-04-18 05:27:54.279033	2026-04-18 05:29:33.963963
20	22	\N	2026-04-18	\N	Cập nhật thông tin - Số hồ sơ: HS-2026-0022-03		Ra viện	Cập nhật hồ sơ	\N	0.00	0.00	2026-04-18 06:07:45.967885	2026-04-18 06:07:45.967885
21	22	\N	2026-04-18	\N	Cập nhật thông tin - Số hồ sơ: HS-2026-0022-04		Ra viện	Cập nhật hồ sơ	\N	0.00	0.00	2026-04-18 09:22:45.047084	2026-04-18 09:22:45.047084
\.


--
-- TOC entry 5191 (class 0 OID 35684)
-- Dependencies: 247
-- Data for Name: ke_thuoc; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.ke_thuoc (id, ho_so_kham_id, thuoc_id, so_luong, lieu_dung, don_gia, thanh_tien, ghi_chu) FROM stdin;
1	12	7	1		3500.00	3500.00	\N
2	12	6	10		2000.00	20000.00	\N
3	12	5	8		1500.00	12000.00	\N
4	2	7	1		3500.00	3500.00	\N
5	2	2	1		3000.00	3000.00	\N
6	2	10	1		12000.00	12000.00	\N
7	2	4	1		2500.00	2500.00	\N
8	13	7	1		3500.00	3500.00	\N
9	14	7	1		3500.00	3500.00	\N
10	14	9	1		8000.00	8000.00	\N
11	18	7	1		3500.00	3500.00	\N
12	18	3	1		2500.00	2500.00	\N
13	18	3	1		2500.00	2500.00	\N
\.


--
-- TOC entry 5166 (class 0 OID 32952)
-- Dependencies: 222
-- Data for Name: refresh_tokens; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.refresh_tokens (id, user_id, token, expires_at, created_at, is_revoked) FROM stdin;
1	5	83b4b07cf7ce431289aa7d68909a3beb	2026-04-14 20:28:09.89671	2026-04-07 20:28:09.91783	f
2	5	14e185e3db894c62804084fd47c53430	2026-04-14 20:29:07.328465	2026-04-07 20:29:07.329152	f
3	5	d3dcba6a9d0f4e0dbad4ba482480c5af	2026-04-14 20:46:55.462615	2026-04-07 20:46:55.479016	f
4	5	5e8023db6b064da4917320e096869b49	2026-04-14 20:49:06.000991	2026-04-07 20:49:06.001739	f
5	5	3da8429a9af74daeb9da689c05a7c2d7	2026-04-14 20:50:12.011473	2026-04-07 20:50:12.032809	f
6	5	357f15722c494badb3567eef188d2516	2026-04-14 21:07:25.413638	2026-04-07 21:07:25.417709	f
7	5	080381e6ab804d4d978e91373eddd9b6	2026-04-14 21:08:07.903762	2026-04-07 21:08:07.904025	f
8	5	c08187462eed4467a5d919413d6ce122	2026-04-14 21:08:53.510222	2026-04-07 21:08:53.510711	f
9	5	ceb77c312fb34743a1d3cdd7ad832138	2026-04-14 21:15:28.67852	2026-04-07 21:15:28.683828	f
10	5	868624581d0b4be0931a1e87fd90404e	2026-04-14 21:16:28.80893	2026-04-07 21:16:28.814325	f
11	5	2673986d1df64f58b7100dc2df86aa27	2026-04-14 21:17:34.868645	2026-04-07 21:17:34.869347	f
12	5	9da93fa0f87d46349caf6412e59a9d5a	2026-04-14 21:19:05.036399	2026-04-07 21:19:05.03662	f
13	5	c36af1dab775453c919f3c8279ac4947	2026-04-14 21:20:52.377639	2026-04-07 21:20:52.383162	f
14	5	51e9630449f74740ad548b96b8885c01	2026-04-14 21:32:16.165634	2026-04-07 21:32:16.173727	f
15	5	1da3dab3537c421393b29dedb8ba52c7	2026-04-14 21:42:56.600965	2026-04-07 21:42:56.604657	f
16	5	516806964f7645a5b27e2ae07ebdc193	2026-04-14 21:50:36.08163	2026-04-07 21:50:36.087352	f
17	5	b7468e63f70d4ed0bab2c01ab1f7a758	2026-04-14 21:54:38.342018	2026-04-07 21:54:38.347425	t
18	5	35709057a3614ae4aeeb36c4aa2eaba2	2026-04-18 09:26:47.307545	2026-04-11 09:26:47.333556	f
19	5	349cd6e92d824dde97fef35b71ed3d37	2026-04-18 09:34:01.053541	2026-04-11 09:34:01.05434	f
20	5	0b5e5841c2ec46e481500ae0107fbe6e	2026-04-18 09:47:38.905797	2026-04-11 09:47:38.909745	f
21	5	2687304a961e46c88e6b43ebeaf66e78	2026-04-18 09:48:05.548618	2026-04-11 09:48:05.548903	f
22	5	a84312b3b7214578ae553691820854cc	2026-04-18 09:49:56.649652	2026-04-11 09:49:56.64993	f
23	5	cb6a16ba764a40a388fe3d230f109f6f	2026-04-18 09:56:16.854689	2026-04-11 09:56:16.862753	f
24	5	649a481d71b94bbb94d0619c2099b424	2026-04-18 10:00:56.327659	2026-04-11 10:00:56.328023	t
25	5	623adfbd342d44ebbcf081fa93f820ba	2026-04-18 10:14:50.829097	2026-04-11 10:14:50.834975	f
26	5	e0b1825cc21045fe8bdd580d563db7cf	2026-04-18 10:17:48.393949	2026-04-11 10:17:48.398076	f
27	5	610de4e20a404adc8170443e00225a92	2026-04-18 10:29:25.235115	2026-04-11 10:29:25.241561	f
28	5	f59be27b73fc4859a68490a943448ce1	2026-04-18 10:34:35.162182	2026-04-11 10:34:35.167371	f
29	5	fa8c5349553c4de295bd590a08fa9c5b	2026-04-18 10:41:27.686002	2026-04-11 10:41:27.693937	f
30	5	5a7e198d07a34b83beb0015cd0b7b522	2026-04-18 10:44:25.353027	2026-04-11 10:44:25.358308	f
31	5	580609d6f90b4d9bbe3a1d3bdcafb5ac	2026-04-18 10:45:36.34805	2026-04-11 10:45:36.35341	f
32	5	2769e762ae134a6789cc43ee4ba0b028	2026-04-18 10:46:23.117669	2026-04-11 10:46:23.117977	f
33	5	e775c5bc59bd47f0bef104c29f84c52c	2026-04-25 05:22:43.700838	2026-04-18 05:22:43.71934	f
34	5	a5b134ee178c4f3f92a062c7dea62fc4	2026-04-25 05:26:14.710865	2026-04-18 05:26:14.711678	t
35	5	cf66be83dc6f43fc8090013f547a0f95	2026-04-25 05:39:41.442845	2026-04-18 05:39:41.462951	f
36	5	6bf3fb7d7c1a4507a154a2725b8886a0	2026-04-25 05:44:35.095543	2026-04-18 05:44:35.120095	f
37	5	9b2c27a2c0524f5bb4387747dfc2f736	2026-04-25 05:47:07.621248	2026-04-18 05:47:07.647375	f
38	5	eeb6f13f7e2a42b897536269aef6c9a8	2026-04-24 22:49:59.747174	2026-04-18 05:49:59.7664	f
39	5	1ab91f58cea54a5881587d0c8e1b1b66	2026-04-24 22:54:23.852154	2026-04-18 05:54:23.872715	f
40	5	1a186ff4a3724d52a90a1cc41f747435	2026-04-24 22:56:45.855595	2026-04-18 05:56:45.87246	f
41	5	5cb3386773fb440997a25937b102ba83	2026-04-24 23:07:12.66498	2026-04-18 06:07:12.683879	t
42	5	44ce2e9e36544bf5bac8d73cce0a8f8f	2026-04-25 02:21:24.637362	2026-04-18 09:21:24.64317	f
43	5	85fc38449ae94e3e8dd2c0afa9a2862c	2026-04-25 02:22:08.799429	2026-04-18 09:22:08.799646	t
44	5	8b883f7f905e407e951ef6f236e4d5f3	2026-04-25 02:30:43.929725	2026-04-18 09:30:43.934261	t
45	5	e69aa170c5f548c9800bd9d3c69c2267	2026-04-26 08:12:20.160957	2026-04-19 15:12:20.166661	f
\.


--
-- TOC entry 5193 (class 0 OID 35706)
-- Dependencies: 249
-- Data for Name: su_dung_dich_vu; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.su_dung_dich_vu (id, ho_so_kham_id, dich_vu_id, so_luong, don_gia, thanh_tien, trang_thai, ghi_chu, created_at) FROM stdin;
1	12	2	1	200000.00	200000.00	Chờ xử lý		2026-03-30 09:16:53.814116
2	12	9	1	350000.00	350000.00	Chờ xử lý		2026-03-30 09:17:18.696653
3	2	5	1	800000.00	800000.00	Chờ xử lý		2026-03-30 16:00:05.511548
4	2	8	1	80000.00	80000.00	Chờ xử lý		2026-03-30 16:00:08.554338
5	2	6	1	1500000.00	1500000.00	Chờ xử lý		2026-03-30 16:00:11.714418
6	13	2	1	200000.00	200000.00	Chờ xử lý		2026-04-07 21:55:03.401152
7	14	2	1	200000.00	200000.00	Chờ xử lý		2026-04-07 21:56:03.998543
8	18	6	1	1500000.00	1500000.00	Chờ xử lý		2026-04-18 05:29:23.546917
9	18	4	1	250000.00	250000.00	Chờ xử lý		2026-04-18 05:29:26.738893
\.


--
-- TOC entry 5164 (class 0 OID 32933)
-- Dependencies: 220
-- Data for Name: users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.users (id, username, email, password_hash, role, created_at) FROM stdin;
1	nguyenvana	vana@gmail.com	123456	admin	2026-03-22 20:33:44.899196
7	quangvuongnguyen	quangvuong@gmail.com	123456	admin	2026-03-22 20:58:38.521026
9	nguyenquanvuong	quangvuong99@gmail.com	999999	user	2026-03-22 21:11:39.899285
5	quangvuong	thib@gmail.com	vuong99	user	2026-03-22 20:48:43.452681
16	nguyenquangvuong	vuongihi@gmail.com	Quangvuong00	user	2026-04-04 10:48:38.260863
\.


--
-- TOC entry 5213 (class 0 OID 0)
-- Dependencies: 242
-- Name: bao_hiem_y_te_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.bao_hiem_y_te_id_seq', 43, true);


--
-- TOC entry 5214 (class 0 OID 0)
-- Dependencies: 240
-- Name: benh_nhan_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.benh_nhan_id_seq', 22, true);


--
-- TOC entry 5215 (class 0 OID 0)
-- Dependencies: 228
-- Name: dm_dan_toc_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dm_dan_toc_id_seq', 8, true);


--
-- TOC entry 5216 (class 0 OID 0)
-- Dependencies: 236
-- Name: dm_dich_vu_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dm_dich_vu_id_seq', 10, true);


--
-- TOC entry 5217 (class 0 OID 0)
-- Dependencies: 230
-- Name: dm_nghe_nghiep_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dm_nghe_nghiep_id_seq', 8, true);


--
-- TOC entry 5218 (class 0 OID 0)
-- Dependencies: 234
-- Name: dm_noi_dang_ky_the_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dm_noi_dang_ky_the_id_seq', 7, true);


--
-- TOC entry 5219 (class 0 OID 0)
-- Dependencies: 226
-- Name: dm_quoc_tich_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dm_quoc_tich_id_seq', 7, true);


--
-- TOC entry 5220 (class 0 OID 0)
-- Dependencies: 238
-- Name: dm_thuoc_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dm_thuoc_id_seq', 10, true);


--
-- TOC entry 5221 (class 0 OID 0)
-- Dependencies: 232
-- Name: dm_tinh_thanh_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.dm_tinh_thanh_id_seq', 17, true);


--
-- TOC entry 5222 (class 0 OID 0)
-- Dependencies: 244
-- Name: ho_so_kham_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ho_so_kham_id_seq', 21, true);


--
-- TOC entry 5223 (class 0 OID 0)
-- Dependencies: 246
-- Name: ke_thuoc_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.ke_thuoc_id_seq', 13, true);


--
-- TOC entry 5224 (class 0 OID 0)
-- Dependencies: 221
-- Name: refresh_tokens_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.refresh_tokens_id_seq', 45, true);


--
-- TOC entry 5225 (class 0 OID 0)
-- Dependencies: 223
-- Name: seq_ma_bn; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.seq_ma_bn', 1, false);


--
-- TOC entry 5226 (class 0 OID 0)
-- Dependencies: 225
-- Name: seq_so_hsba; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.seq_so_hsba', 1, false);


--
-- TOC entry 5227 (class 0 OID 0)
-- Dependencies: 224
-- Name: seq_so_tt; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.seq_so_tt', 1, false);


--
-- TOC entry 5228 (class 0 OID 0)
-- Dependencies: 248
-- Name: su_dung_dich_vu_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.su_dung_dich_vu_id_seq', 9, true);


--
-- TOC entry 5229 (class 0 OID 0)
-- Dependencies: 219
-- Name: users_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.users_id_seq', 16, true);


--
-- TOC entry 4993 (class 2606 OID 35640)
-- Name: bao_hiem_y_te bao_hiem_y_te_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bao_hiem_y_te
    ADD CONSTRAINT bao_hiem_y_te_pkey PRIMARY KEY (id);


--
-- TOC entry 4986 (class 2606 OID 35606)
-- Name: benh_nhan benh_nhan_ma_benh_nhan_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.benh_nhan
    ADD CONSTRAINT benh_nhan_ma_benh_nhan_key UNIQUE (ma_benh_nhan);


--
-- TOC entry 4988 (class 2606 OID 35604)
-- Name: benh_nhan benh_nhan_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.benh_nhan
    ADD CONSTRAINT benh_nhan_pkey PRIMARY KEY (id);


--
-- TOC entry 4974 (class 2606 OID 35536)
-- Name: dm_dan_toc dm_dan_toc_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_dan_toc
    ADD CONSTRAINT dm_dan_toc_pkey PRIMARY KEY (id);


--
-- TOC entry 4982 (class 2606 OID 35577)
-- Name: dm_dich_vu dm_dich_vu_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_dich_vu
    ADD CONSTRAINT dm_dich_vu_pkey PRIMARY KEY (id);


--
-- TOC entry 4976 (class 2606 OID 35545)
-- Name: dm_nghe_nghiep dm_nghe_nghiep_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_nghe_nghiep
    ADD CONSTRAINT dm_nghe_nghiep_pkey PRIMARY KEY (id);


--
-- TOC entry 4980 (class 2606 OID 35563)
-- Name: dm_noi_dang_ky_the dm_noi_dang_ky_the_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_noi_dang_ky_the
    ADD CONSTRAINT dm_noi_dang_ky_the_pkey PRIMARY KEY (id);


--
-- TOC entry 4972 (class 2606 OID 35527)
-- Name: dm_quoc_tich dm_quoc_tich_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_quoc_tich
    ADD CONSTRAINT dm_quoc_tich_pkey PRIMARY KEY (id);


--
-- TOC entry 4984 (class 2606 OID 35590)
-- Name: dm_thuoc dm_thuoc_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_thuoc
    ADD CONSTRAINT dm_thuoc_pkey PRIMARY KEY (id);


--
-- TOC entry 4978 (class 2606 OID 35554)
-- Name: dm_tinh_thanh dm_tinh_thanh_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.dm_tinh_thanh
    ADD CONSTRAINT dm_tinh_thanh_pkey PRIMARY KEY (id);


--
-- TOC entry 4996 (class 2606 OID 35667)
-- Name: ho_so_kham ho_so_kham_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ho_so_kham
    ADD CONSTRAINT ho_so_kham_pkey PRIMARY KEY (id);


--
-- TOC entry 4999 (class 2606 OID 35694)
-- Name: ke_thuoc ke_thuoc_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ke_thuoc
    ADD CONSTRAINT ke_thuoc_pkey PRIMARY KEY (id);


--
-- TOC entry 4970 (class 2606 OID 32964)
-- Name: refresh_tokens refresh_tokens_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.refresh_tokens
    ADD CONSTRAINT refresh_tokens_pkey PRIMARY KEY (id);


--
-- TOC entry 5001 (class 2606 OID 35717)
-- Name: su_dung_dich_vu su_dung_dich_vu_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.su_dung_dich_vu
    ADD CONSTRAINT su_dung_dich_vu_pkey PRIMARY KEY (id);


--
-- TOC entry 4964 (class 2606 OID 32950)
-- Name: users users_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_email_key UNIQUE (email);


--
-- TOC entry 4966 (class 2606 OID 32946)
-- Name: users users_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);


--
-- TOC entry 4968 (class 2606 OID 32948)
-- Name: users users_username_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_username_key UNIQUE (username);


--
-- TOC entry 4994 (class 1259 OID 35732)
-- Name: idx_baohi_benhnh; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_baohi_benhnh ON public.bao_hiem_y_te USING btree (benh_nhan_id);


--
-- TOC entry 4989 (class 1259 OID 35729)
-- Name: idx_benh_nhan_cccd; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_benh_nhan_cccd ON public.benh_nhan USING btree (cccd);


--
-- TOC entry 4990 (class 1259 OID 35730)
-- Name: idx_benh_nhan_ho_ten; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_benh_nhan_ho_ten ON public.benh_nhan USING btree (ho_ten);


--
-- TOC entry 4991 (class 1259 OID 35728)
-- Name: idx_benh_nhan_ma; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_benh_nhan_ma ON public.benh_nhan USING btree (ma_benh_nhan);


--
-- TOC entry 4997 (class 1259 OID 35731)
-- Name: idx_ho_so_benh_nhan; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX idx_ho_so_benh_nhan ON public.ho_so_kham USING btree (benh_nhan_id);


--
-- TOC entry 5007 (class 2606 OID 35641)
-- Name: bao_hiem_y_te bao_hiem_y_te_benh_nhan_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bao_hiem_y_te
    ADD CONSTRAINT bao_hiem_y_te_benh_nhan_id_fkey FOREIGN KEY (benh_nhan_id) REFERENCES public.benh_nhan(id) ON DELETE CASCADE;


--
-- TOC entry 5008 (class 2606 OID 35646)
-- Name: bao_hiem_y_te bao_hiem_y_te_noi_dang_ky_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.bao_hiem_y_te
    ADD CONSTRAINT bao_hiem_y_te_noi_dang_ky_id_fkey FOREIGN KEY (noi_dang_ky_id) REFERENCES public.dm_noi_dang_ky_the(id);


--
-- TOC entry 5003 (class 2606 OID 35612)
-- Name: benh_nhan benh_nhan_dan_toc_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.benh_nhan
    ADD CONSTRAINT benh_nhan_dan_toc_id_fkey FOREIGN KEY (dan_toc_id) REFERENCES public.dm_dan_toc(id);


--
-- TOC entry 5004 (class 2606 OID 35617)
-- Name: benh_nhan benh_nhan_nghe_nghiep_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.benh_nhan
    ADD CONSTRAINT benh_nhan_nghe_nghiep_id_fkey FOREIGN KEY (nghe_nghiep_id) REFERENCES public.dm_nghe_nghiep(id);


--
-- TOC entry 5005 (class 2606 OID 35607)
-- Name: benh_nhan benh_nhan_quoc_tich_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.benh_nhan
    ADD CONSTRAINT benh_nhan_quoc_tich_id_fkey FOREIGN KEY (quoc_tich_id) REFERENCES public.dm_quoc_tich(id);


--
-- TOC entry 5006 (class 2606 OID 35622)
-- Name: benh_nhan benh_nhan_tinh_thanh_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.benh_nhan
    ADD CONSTRAINT benh_nhan_tinh_thanh_id_fkey FOREIGN KEY (tinh_thanh_id) REFERENCES public.dm_tinh_thanh(id);


--
-- TOC entry 5009 (class 2606 OID 35678)
-- Name: ho_so_kham ho_so_kham_bac_si_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ho_so_kham
    ADD CONSTRAINT ho_so_kham_bac_si_id_fkey FOREIGN KEY (bac_si_id) REFERENCES public.users(id);


--
-- TOC entry 5010 (class 2606 OID 35673)
-- Name: ho_so_kham ho_so_kham_bao_hiem_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ho_so_kham
    ADD CONSTRAINT ho_so_kham_bao_hiem_id_fkey FOREIGN KEY (bao_hiem_id) REFERENCES public.bao_hiem_y_te(id);


--
-- TOC entry 5011 (class 2606 OID 35668)
-- Name: ho_so_kham ho_so_kham_benh_nhan_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ho_so_kham
    ADD CONSTRAINT ho_so_kham_benh_nhan_id_fkey FOREIGN KEY (benh_nhan_id) REFERENCES public.benh_nhan(id);


--
-- TOC entry 5012 (class 2606 OID 35695)
-- Name: ke_thuoc ke_thuoc_ho_so_kham_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ke_thuoc
    ADD CONSTRAINT ke_thuoc_ho_so_kham_id_fkey FOREIGN KEY (ho_so_kham_id) REFERENCES public.ho_so_kham(id) ON DELETE CASCADE;


--
-- TOC entry 5013 (class 2606 OID 35700)
-- Name: ke_thuoc ke_thuoc_thuoc_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.ke_thuoc
    ADD CONSTRAINT ke_thuoc_thuoc_id_fkey FOREIGN KEY (thuoc_id) REFERENCES public.dm_thuoc(id);


--
-- TOC entry 5002 (class 2606 OID 32965)
-- Name: refresh_tokens refresh_tokens_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.refresh_tokens
    ADD CONSTRAINT refresh_tokens_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.users(id) ON DELETE CASCADE;


--
-- TOC entry 5014 (class 2606 OID 35723)
-- Name: su_dung_dich_vu su_dung_dich_vu_dich_vu_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.su_dung_dich_vu
    ADD CONSTRAINT su_dung_dich_vu_dich_vu_id_fkey FOREIGN KEY (dich_vu_id) REFERENCES public.dm_dich_vu(id);


--
-- TOC entry 5015 (class 2606 OID 35718)
-- Name: su_dung_dich_vu su_dung_dich_vu_ho_so_kham_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.su_dung_dich_vu
    ADD CONSTRAINT su_dung_dich_vu_ho_so_kham_id_fkey FOREIGN KEY (ho_so_kham_id) REFERENCES public.ho_so_kham(id) ON DELETE CASCADE;


-- Completed on 2026-04-19 16:44:13

--
-- PostgreSQL database dump complete
--

\unrestrict lMJ06JcUeA8b49S3BS7xFJlLlndjQYsjog2FYDtlmfKDhPebs4HclhkWEIfy0g4

