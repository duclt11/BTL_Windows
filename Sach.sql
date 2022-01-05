use master
go
create database QLCuaHangSach
go
use QLCuaHangSach 
go
Create table Sach (
	[MaSach] int identity(0,1) NOT NULL,
	[TieuDe] Nvarchar(100) NOT NULL,
	[AnhBia] Image NULL,
	[NgayPhatHanh] Datetime NOT NULL,
	[GiaBan] int NOT NULL,
	[TacGia] Nvarchar(30) NULL,
	[NhaPhatHanh] Nvarchar(30) NULL,
	[SoLuongCo] Integer NULL,
	Primary Key  ([MaSach]),
	 

) 
go
Create table NhanVien(
	MaNV int identity(0,1) primary key not null,
	HoTen nvarchar(30),
	Tuoi int,
	DiaChi nvarchar(30),
	isAdmin bit,
	TenDangNhap char(20),
	MatKhau char(20)
)
go

Create table DonHang (
	[MaDH] int identity(0,1) NOT NULL,
	[NgayLapDon] Datetime NOT NULL,
	[GhiChu] Nvarchar(500) NULL,
	[TenKhachHang] Nvarchar(30) NULL,
	[SDT] Char(10) NULL,
	[DiaChi] Nvarchar(20) NULL,
	MaNV int,
	constraint fk5 foreign key(MaNV) references NhanVien(MaNV),
	Primary Key  ([MaDH])
) 
go

Create table ChiTietDonHang (
	[MaSach] int NOT NULL,
	[MaDH] int NOT NULL,
	[SoLuong] Integer NULL,
	[DonGia] int NULL,
	Primary Key  ([MaSach],[MaDH]),
	constraint fk1 foreign key(MaSach) references Sach(MaSach),
	constraint fk2 foreign key(MaDH) references DonHang(MaDH)
) 
go
Create table XuongNhap (
	[MaXN] int identity(0,1) NOT NULL,
	[TenXN] Nvarchar(100) NULL,
	[SDT] Char(10) NULL,
	[DiaChi] Nvarchar(200) NULL,
Primary Key  ([MaXN])
) 
go
Create table PhieuNhap (
	[SoPN] int identity(0,1) NOT NULL,
	[MaXN] int NOT NULL,
	[NgayNhap] Datetime NULL,
	[GhiChu] Nvarchar(500) NULL,
	[MaNV] int,
	Primary Key  ([SoPN]),
	constraint fk6 foreign key(MaXN) references XuongNhap(MaXN),
	constraint fk7 foreign key(MaNV) references NhanVien(MaNV)
) 
go

Create table ChiTietPhieuNhap (
	[MaSach] int NOT NULL,
	[SoPN] int NOT NULL,
	[SoLuong] Integer NULL,
	[DonGia] int NULL,
	Primary Key  ([MaSach],[SoPN]),
	constraint fk3 foreign key(MaSach) references Sach(MaSach),
	constraint fk4 foreign key(SoPN) references PhieuNhap(SoPN)
) 
go
insert into XuongNhap(TenXN,DiaChi,SDT) values(N'Công Ty Cổ Phần Phát Hành Sách Tp. HCM',N'Hồ Chí Minh','0236271289'),
(N'Công Ty Cổ Phần Sách & Thiết Bị Giáo Dục Trí Tuệ',N'Hà Nội','0983247583'),
(N'Công Ty Cổ Phần Sách Giáo Dục Tại Thành Phố Hà Nội',N'Hà Nội','0984738123')
go
insert into NhanVien(TenDangNhap,MatKhau,isAdmin) values('Admin','123',1)
insert into NhanVien(TenDangNhap,MatKhau,isAdmin,HoTen,Tuoi,DiaChi) values('LanNT','123',0,N'Nguyễn Thị Lan',20,'Hà Nội')
go
insert into Sach(TieuDe,NgayPhatHanh,GiaBan,TacGia,NhaPhatHanh,SoLuongCo) values
(N'Tiếng việt lớp 10','2020/12/23',2000,'',N'Nhà xuất bản giáo dục',100),
(N'Tiếng việt lớp 11','2020/12/23',2000,'',N'Nhà xuất bản giáo dục',100),
(N'Tiếng việt lớp 12','2020/12/23',2000,'',N'Nhà xuất bản giáo dục',100)
go
insert into ChiTietPhieuNhap(MaSach,SoPN,SoLuong,DonGia) values(0,0,10,1000)
Go

/* Roles permissions */


/* Users permissions */


