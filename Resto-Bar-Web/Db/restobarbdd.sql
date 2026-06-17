CREATE DATABASE RESTOBAR_DB;
GO

------------------------

USE RESTOBAR_DB;
GO

------------------------
---Creacion de tablas---
------------------------

CREATE TABLE Roles(
	IdRol int primary key,
	NombreRol varchar(30) not null
);

CREATE TABLE Usuarios(
	IdUsuario int identity(1,1) primary key,
	NombreUsuario varchar(50) not null,
	Contrasena varchar(10) not null,
	IdRol int not null,
	foreign key (IdRol) references Roles(IdRol)
);

CREATE TABLE Productos(
	IdProducto int identity(1,1) primary key,
	NombreProducto varchar(50) not null,
	DescripcionProducto varchar(150),
	Precio decimal(10,2) not null,
	Stock int not null
);

CREATE TABLE ProductosImagenes(
	IdImagen int identity (1,1) primary key,
	IdProducto int not null,
	UrlImagen varchar(500) not null,
	foreign key (IdProducto) references Productos(IdProducto)
);


CREATE TABLE Mesas(
	NroMesa int identity (1,1) primary key,
	IdUsuario int,
	MesaUrlImagen varchar(500) not null,
	Estado bit not null default 1,
	foreign key (IdUsuario) references Usuarios(IdUsuario)
);

CREATE TABLE MetodosDePago(
	IdMetodo int identity(1,1) primary key,
	NombreMetodo varchar(50) not null
);

CREATE TABLE EstadosPedido(
	IdEstadoPedido int identity (1,1) primary key,
	DetalleEstado varchar(50) not null
);

CREATE TABLE Pedidos(
	IdPedido int identity(1,1) primary key,
	NroMesa int not null,
	IdUsuario int not null,
	FechayHoraPedido datetime not null,
	PrecioTotal decimal (10,2) not null default 0,
	IdMetodo int,
	IdEstadoPedido int not null,
	foreign key (IdEstadoPedido) references EstadosPedido(IdEstadoPedido),
	foreign key (NroMesa) references Mesas(NroMesa),
	foreign key (IdUsuario) references Usuarios(IdUsuario),
	foreign key (IdMetodo) references MetodosDePago(IdMetodo)
);


CREATE TABLE DetallePedido(
	IdPedido int,
	IdProducto int,
	Cantidad int not null,
	PrecioUnitario decimal (10,2) not null,
	primary key (IdPedido, IdProducto),
	foreign key (IdPedido) references Pedidos(IdPedido),
	foreign key (IdProducto) references Productos(IdProducto)
);


--------------------
---Carga de datos---
--------------------

insert into Roles(IdRol, NombreRol)
values
(0, 'Admin'),
(1, 'Gerente'),
(2, 'Mozo');

insert into Usuarios(NombreUsuario, Contrasena, IdRol)
VALUES 
('admin', 'admin', 0),
('gerente', 'gerente', 1),
('mesero', 'mesero', 2)


insert into Productos(NombreProducto, DescripcionProducto, Precio, Stock)
values ('Fugazzeta', 'Pizza a la piedra rellena de queso mozzarella con cebolla cruda sobre la masa. Esta pizza es sin salsa.', 12000, 5);

insert into ProductosImagenes(IdProducto, UrlImagen)
values (1, 'https://preview.redd.it/argentinian-stuffed-pizza-fugazzeta-v0-jssh9gui8hxe1.jpg?width=640&crop=smart&auto=webp&s=74a25d84cd1f6d187272d85208cdc80058e5154e')

insert into Mesas(MesaUrlImagen)
VALUES 
('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSZIuCKX7-ftZfl1D7HHwKstHeF3Pmy8xl5mw&s'),
('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSZIuCKX7-ftZfl1D7HHwKstHeF3Pmy8xl5mw&s'),
('https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSZIuCKX7-ftZfl1D7HHwKstHeF3Pmy8xl5mw&s')
);