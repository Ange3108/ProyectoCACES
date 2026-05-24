--CREATE DATABASE CACES;
--GO

USE CACES;
GO
CREATE TABLE Especialidad(
    Id_Especialidad INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(30) NOT NULL,
    Descripcion VARCHAR(200) NOT NULL,
    Icono VARCHAR(100) NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
	Estado BIT NOT NULL
);
GO
CREATE TABLE Usuarios(
    Id_Usuario INT PRIMARY KEY IDENTITY(1,1),
	Nombres VARCHAR(100) NOT NULL,
    PrimerApellido VARCHAR(100) NOT NULL,
    SegundoApellido VARCHAR(100) NOT NULL,
	CorreoElectronico VARCHAR(200) NOT NULL UNIQUE,
	DUI VARCHAR(10) NOT NULL,
	Foto VARCHAR(200) NOT NULL,
	FechaDeRegistro DATETIME NOT NULL,
    FechaDeModificacion DATETIME NULL,
	Estado BIT NOT NULL
	CONSTRAINT UQ_Usuarios_DUI UNIQUE (DUI)

);
GO
CREATE TABLE Medicos(
    Id_Medico INT PRIMARY KEY IDENTITY(1,1),
    Id_Especialidad INT NOT NULL,
	Id_Usuario INT NOT NULL,
    Experiencia INT NOT NULL,
    Telefono VARCHAR(30) NOT NULL,
    Certificaciones VARCHAR(200) NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
	CONSTRAINT FK_Medicos_Especialidad FOREIGN KEY (Id_Especialidad) REFERENCES Especialidad(Id_Especialidad),
	CONSTRAINT FK_Medico_Usuario FOREIGN KEY (Id_Usuario) REFERENCES Usuarios(Id_Usuario)
);
GO
CREATE TABLE Historial_Medico(
    Id_Historial INT PRIMARY KEY IDENTITY(1,1),
    Alergias VARCHAR(200) NOT NULL,
    Enfermedades_Crónicas varchar(200) NOT NULL,
    Detalles VARCHAR(100) NOT NULL,
    Tipo_Sangre VARCHAR(10) NOT NULL,
    Anteriores VARCHAR(50) NOT NULL,
    FechaDeCreacion DATETIME NOT NULL,
    FechaDeModificacion DATETIME NULL
);
GO
CREATE TABLE Pacientes(
    Id_Paciente INT PRIMARY KEY IDENTITY(1,1),
	Id_Usuario INT NOT NULL,
    Id_Historial INT NOT NULL,
	CONSTRAINT FK_Pacientes_Historial FOREIGN KEY (Id_Historial) REFERENCES Historial_Medico(Id_Historial),
	CONSTRAINT FK_Paciente_Usuario FOREIGN KEY (Id_Usuario) REFERENCES Usuarios(Id_Usuario)
);
GO
CREATE TABLE HorariosDisponibles(
    Id_Horario INT PRIMARY KEY IDENTITY(1,1),
    Id_Medico INT NOT NULL,
    DiaSemana INT NOT NULL, -- 0=Lunes, 6=Domingo
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    Activo BIT NOT NULL,
    CONSTRAINT FK_Horarios_Medico FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico)
);

CREATE TABLE Citas(
    Id_Cita INT PRIMARY KEY IDENTITY(1,1),
    Id_Paciente INT NOT NULL,
    Id_Medico INT NOT NULL,
    Id_Especialidad INT NOT NULL,
    Fecha Int NOT NULL,
    Hora TIME NOT NULL,
    Motivo VARCHAR(100) NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
    FechaDeModificacion DATETIME NULL,
    Estado TINYINT NOT NULL,
	CONSTRAINT FK_Citas_Medicos FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico),
	CONSTRAINT FK_Citas_Pacientes FOREIGN KEY (Id_Paciente) REFERENCES Pacientes(Id_Paciente),
    CONSTRAINT FK_Citas_Especialidad FOREIGN KEY (Id_Especialidad) REFERENCES Especialidad(Id_Especialidad),
    CONSTRAINT FK_Citas_Fecha FOREIGN KEY (Fecha) REFERENCES HorariosDisponibles(Id_Horario)
);


CREATE TABLE ArchivosHistorial(
    Id_Archivo INT PRIMARY KEY IDENTITY(1,1),
    Id_Historial INT NOT NULL,
    NombreArchivo VARCHAR(200) NOT NULL,
    RutaArchivo VARCHAR(500) NOT NULL,
    TipoArchivo VARCHAR(50), -- radiografía, análisis, comprobante, etc
    FechaDeSubida DATETIME NOT NULL,
    CONSTRAINT FK_ArchivosHistorial_Historial FOREIGN KEY (Id_Historial) REFERENCES Historial_Medico(Id_Historial)
);
GO

CREATE TABLE Recetas(
    Id_Receta INT PRIMARY KEY IDENTITY(1,1),
    Id_Cita INT NOT NULL,
    Medicamentos VARCHAR(MAX) NOT NULL,
    Instrucciones VARCHAR(500),
    FechaDeRegistro DATETIME NOT NULL,
    FechaDeVencimiento DATETIME NOT NULL,
    CONSTRAINT FK_Recetas_Cita FOREIGN KEY (Id_Cita) REFERENCES Citas(Id_Cita)
);
GO
CREATE TABLE Paquetes(
    Id_Paquete INT PRIMARY KEY IDENTITY(1,1),
	Id_Paciente INT NOT NULL,
	Descripcion VARCHAR(100) NOT NULL,
    Duracion VARCHAR(100) NOT NULL,
    Precio VARCHAR(100) NOT NULL,
	FechaDeRegistro DATETIME NOT NULL,
	Estado BIT NOT NULL,
	CONSTRAINT FK_Paquetes_Pacientes FOREIGN KEY (Id_Paciente) REFERENCES Pacientes(Id_Paciente)
);
GO
CREATE TABLE Precios(
    Id_Precio INT PRIMARY KEY IDENTITY(1,1),
	Id_Medico INT NOT NULL,
    Costo DECIMAL(10,2) NOT NULL,
	Detalles VARCHAR(100) NOT NULL,
	CONSTRAINT FK_Precios_Por_Medico FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico)
);
GO
CREATE TABLE Cirugias(
    Id_Cirugia INT PRIMARY KEY IDENTITY(1,1),
	Id_Paciente INT NOT NULL,
	Id_Medico INT NOT NULL,
	Id_Especialidad INT NOT NULL,
    Fecha int NOT NULL,
	CONSTRAINT FK_Cirugias_Paciente FOREIGN KEY (Id_Paciente) REFERENCES Pacientes(Id_Paciente),
	CONSTRAINT FK_Cirugias_Medico FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico),
	CONSTRAINT FK_Cirugias_Especialidad FOREIGN KEY (Id_Especialidad) REFERENCES Especialidad(Id_Especialidad),
    CONSTRAINT FK_Cirugía_Fecha FOREIGN KEY (Fecha) REFERENCES HorariosDisponibles(Id_Horario)

);

CREATE TABLE Noticias(
    Id_Noticia INT PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(200) NOT NULL,
    Contenido VARCHAR(MAX) NOT NULL,
    FechaDePublicacion DATETIME NOT NULL,
    FechaDeModificacion DATETIME NULL,
    Imagen VARCHAR(200),
    Estado BIT NOT NULL
);




/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 12/11/2024 13:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 12/11/2024 13:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 12/11/2024 13:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 12/11/2024 13:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 12/11/2024 13:29:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO


-- ============================================
-- INSERTS DE DATOS INICIALES PARA CACES
-- ============================================

-- ROLES
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES
('1', 'Administrador'),
('2', 'Medico'),
('3', 'Paciente');
GO

-- ESPECIALIDADES
INSERT INTO Especialidad (Nombre, Descripcion, Icono, FechaDeRegistro, Estado) VALUES
('Laparoscopia', 'Cirugía mínimamente invasiva con cámara', 'laparoscopia-icon.png', GETDATE(), 1),
('Oncología', 'Especialista en diagnóstico y tratamiento del cáncer', 'oncology-icon.png', GETDATE(), 1),
('Cirugía', 'Cirugía general y procedimientos quirúrgicos', 'surgery-icon.png', GETDATE(), 1);
GO

-- USUARIOS (Admin, Médico, Paciente)
INSERT INTO Usuarios (Nombres, PrimerApellido, SegundoApellido, CorreoElectronico, DUI, Foto, FechaDeRegistro, FechaDeModificacion, Estado) VALUES
('Juan', 'García', 'López', 'juan.admin@caces.com', '12345678', 'juan.jpg', GETDATE(), NULL, 1),
('Oscar', 'López', 'Varillas', 'oscar.medico@caces.com', '87654321', 'oscar.jpg', GETDATE(), NULL, 1),
('María', 'Hernández', 'Gómez', 'maria.paciente@caces.com', '11223344', 'maria.jpg', GETDATE(), NULL, 1);
GO

-- MEDICOS
INSERT INTO Medicos (Id_Especialidad, Id_Usuario, Experiencia, Telefono, Certificaciones, FechaDeRegistro) VALUES
(1, 2, 10, '2-2222-2222', 'Licenciado en Medicina, Especialista en Laparoscopia', GETDATE()),
(2, 2, 8, '2-3333-3333', 'Licenciado en Medicina, Especialista en Oncología', GETDATE()),
(3, 2, 12, '2-4444-4444', 'Licenciado en Medicina, Especialista en Cirugía General', GETDATE());
GO

-- HISTORIAL MEDICO
INSERT INTO Historial_Medico (Alergias, Enfermedades_Crónicas, Detalles, Tipo_Sangre, Anteriores, FechaDeCreacion, FechaDeModificacion) VALUES
('Penicilina', 'Diabetes tipo 2', 'Paciente controlado', 'O+', 'Apendicitis 2015', GETDATE(), NULL),
('Ninguna', 'Hipertensión', 'Paciente bajo control médico', 'A+', 'Ninguna', GETDATE(), NULL),
('Aspirina', 'Ninguna', 'Paciente sano', 'B+', 'Fractura de brazo 2018', GETDATE(), NULL);
GO

-- PACIENTES
INSERT INTO Pacientes (Id_Usuario, Id_Historial) VALUES
(3, 1),
(3, 2),
(3, 3);
GO

-- HORARIOS DISPONIBLES
INSERT INTO HorariosDisponibles (Id_Medico, DiaSemana, HoraInicio, HoraFin, Activo) VALUES
(1, 0, '08:00', '12:00', 1), -- Lunes - Laparoscopia
(2, 1, '09:00', '13:00', 1), -- Martes - Oncología
(3, 2, '07:00', '11:00', 1); -- Miércoles - Cirugía
GO

-- CITAS
INSERT INTO Citas (Id_Paciente, Id_Medico, Id_Especialidad, Fecha, Hora, Motivo, FechaDeRegistro, FechaDeModificacion, Estado) VALUES
(1, 1, 1, 1, '09:00', 'Evaluación para cirugía laparoscópica', GETDATE(), NULL, 1),
(2, 2, 2, 2, '10:30', 'Consulta oncológica inicial', GETDATE(), NULL, 1),
(3, 3, 3, 3, '08:00', 'Evaluación preoperatoria', GETDATE(), NULL, 1);
GO

-- ARCHIVOS HISTORIAL
INSERT INTO ArchivosHistorial (Id_Historial, NombreArchivo, RutaArchivo, TipoArchivo, FechaDeSubida) VALUES
(1, 'Radiografia_torax.pdf', '/archivos/radiografias/rxtoraz1.pdf', 'radiografía', GETDATE()),
(2, 'Analisis_sangre.pdf', '/archivos/analisis/sangre2.pdf', 'análisis', GETDATE()),
(3, 'EEG_2024.pdf', '/archivos/estudios/eeg3.pdf', 'estudio', GETDATE());
GO

-- RECETAS
INSERT INTO Recetas (Id_Cita, Medicamentos, Instrucciones, FechaDeRegistro, FechaDeVencimiento) VALUES
(1, 'Omeprazol 20mg, Tramadol 100mg', 'Tomar según indicaciones postoperatorias', GETDATE(), DATEADD(DAY, 30, GETDATE())),
(2, 'Doxorrubicina IV, Ciclofosfamida IV', 'Administrar según protocolo de quimioterapia', GETDATE(), DATEADD(DAY, 30, GETDATE())),
(3, 'Cefazolina 1g, Paracetamol 500mg', 'Tomar antibiótico y analgésico postoperatorio', GETDATE(), DATEADD(DAY, 30, GETDATE()));
GO

-- PAQUETES
INSERT INTO Paquetes (Id_Paciente, Descripcion, Duracion, Precio, FechaDeRegistro, Estado) VALUES
(1, 'Paquete Laparoscopia - Cirugía mínimamente invasiva', '3 meses', '$2500.00', GETDATE(), 1),
(2, 'Paquete Oncología - Tratamiento integral', '6 meses', '$5000.00', GETDATE(), 1),
(3, 'Paquete Cirugía - Procedimiento quirúrgico', '4 meses', '$3500.00', GETDATE(), 1);
GO

-- PRECIOS
INSERT INTO Precios (Id_Medico, Costo, Detalles) VALUES
(1, 1500.00, 'Cirugía laparoscópica'),
(2, 2000.00, 'Consulta oncológica especializada'),
(3, 1800.00, 'Cirugía general');
GO

-- CIRUGIAS
INSERT INTO Cirugias (Id_Paciente, Id_Medico, Id_Especialidad, Fecha) VALUES
(1, 1, 1, 1),
(2, 2, 2, 2),
(3, 3, 3, 3);
GO

-- NOTICIAS
INSERT INTO Noticias (Titulo, Contenido, FechaDePublicacion, FechaDeModificacion, Imagen, Estado) VALUES
('Ventajas de la cirugía laparoscópica', 'La laparoscopia reduce el trauma quirúrgico y acelera la recuperación...', GETDATE(), NULL, 'laparoscopia-news.jpg', 1),
('Avances en tratamiento del cáncer', 'Nuevas terapias oncológicas ofrecen mejores resultados en pacientes...', GETDATE(), NULL, 'oncology-news.jpg', 1),
('Preparación para cirugía: lo que debes saber', 'Prepararse correctamente antes de una cirugía es fundamental...', GETDATE(), NULL, 'surgery-news.jpg', 1);
GO

-- USUARIOS DE ASPNET IDENTITY
INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES
('user-admin-001', 'juan.admin@caces.com', 1, 'hashed_password_here', 'stamp-001', '2-2222-2222', 1, 0, NULL, 1, 0, 'juan.admin@caces.com'),
('user-medico-002', 'oscar.medico@caces.com', 1, 'hashed_password_here', 'stamp-002', '2-3333-3333', 1, 0, NULL, 1, 0, 'carlos.medico@caces.com'),
('user-paciente-003', 'maria.paciente@caces.com', 1, 'hashed_password_here', 'stamp-003', '2-5555-5555', 1, 0, NULL, 1, 0, 'maria.paciente@caces.com');
GO

-- ASIGNAR ROLES A USUARIOS
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES
('user-admin-001', '1'),    -- Juan es Administrador
('user-medico-002', '2'),   -- Oscar es Médico
('user-paciente-003', '3'); -- María es Paciente
GO