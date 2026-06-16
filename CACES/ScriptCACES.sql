DROP DATABASE [CACES];
GO

CREATE DATABASE CACES;
GO

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
    Estado BIT NOT NULL,
    Direccion VARCHAR(250) NOT NULL,
    Edad INT NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    Nacimiento DATE NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    SecurityStamp NVARCHAR(MAX) NOT NULL,
    TwoFactorEnabled BIT NOT NULL DEFAULT 0,
    LockoutEndDateUtc DATETIME NULL,
    LockoutEnabled BIT NOT NULL DEFAULT 0,
    AccessFailedCount INT NOT NULL DEFAULT 0,
    EmailConfirmed BIT NOT NULL DEFAULT 0,
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
    Medicmanetos VARCHAR(200) NOT NULL,
    Antecedentes VARCHAR(50) NOT NULL,
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



-- ============================================
-- INSERTS DE DATOS INICIALES PARA CACES
-- ============================================

-- ROLES
INSERT INTO [dbo].[AspNetRoles] ([Id], [Name]) VALUES
('1', 'Administrador'),
('2', 'Medico'),
('3', 'Paciente');
GO

-- ESPECIALIDADESZ
INSERT INTO Especialidad (Nombre, Descripcion, Icono, FechaDeRegistro, Estado) VALUES
('Laparoscopia', 'Cirugía mínimamente invasiva con cámara', 'laparoscopia-icon.png', GETDATE(), 1),
('Oncología', 'Especialista en diagnóstico y tratamiento del cáncer', 'oncology-icon.png', GETDATE(), 1),
('Cirugía', 'Cirugía general y procedimientos quirúrgicos', 'surgery-icon.png', GETDATE(), 1);
GO

-- USUARIOS (Admin, Médico, Paciente)
-- Contraseña para los 3 usuarios: Admin124578*
INSERT INTO Usuarios
(Nombres, PrimerApellido, SegundoApellido, CorreoElectronico, DUI, Foto,
 FechaDeRegistro, FechaDeModificacion, Estado, Direccion, Edad, Telefono,
 Nacimiento, PasswordHash, SecurityStamp, TwoFactorEnabled, LockoutEndDateUtc,
 LockoutEnabled, AccessFailedCount, EmailConfirmed)
VALUES
('Juan', 'García', 'López', 'juan.admin@caces.com', '12345678', 'juan.jpg',
 GETDATE(), NULL, 1, 'San José', 30, '8888-1111',
 '1994-05-10', 'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', NEWID(), 0, NULL, 0, 0, 1),

('Oscar', 'López', 'Varillas', 'oscar.medico@caces.com', '87654321', 'oscar.jpg',
 GETDATE(), NULL, 1, 'Cartago', 35, '8888-2222',
 '1989-08-20', 'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', NEWID(), 0, NULL, 0, 0, 1),

('María', 'Hernández', 'Gómez', 'maria.paciente@caces.com', '11223344', 'maria.jpg',
 GETDATE(), NULL, 1, 'Heredia', 28, '8888-3333',
 '1996-03-15', 'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', NEWID(), 0, NULL, 0, 0, 1);
GO

-- MEDICOS
INSERT INTO Medicos (Id_Especialidad, Id_Usuario, Experiencia, Telefono, Certificaciones, FechaDeRegistro) VALUES
(1, 2, 10, '2-2222-2222', 'Licenciado en Medicina, Especialista en Laparoscopia', GETDATE()),
(2, 2, 8, '2-3333-3333', 'Licenciado en Medicina, Especialista en Oncología', GETDATE()),
(3, 2, 12, '2-4444-4444', 'Licenciado en Medicina, Especialista en Cirugía General', GETDATE());
GO

-- HISTORIAL MEDICO
INSERT INTO Historial_Medico (Alergias, Enfermedades_Crónicas, Detalles, Tipo_Sangre, Medicamentos,Antecedentes, FechaDeCreacion, FechaDeModificacion) VALUES
('Penicilina', 'Diabetes tipo 2', 'Paciente controlado', 'O+', 'Omeprazol 20mg, Tramadol 100mg', GETDATE(), NULL),
('Ninguna', 'Hipertensión', 'Paciente bajo control médico', 'A+', 'Ninguno', GETDATE(), NULL),
('Aspirina', 'Ninguna', 'Paciente sano', 'B+', 'Cefazolina 1g, Paracetamol 500mg', GETDATE(), NULL);
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


-- ASIGNAR ROLES A USUARIOS
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES
('user-admin-001', '1'),    -- Juan es Administrador
('user-medico-002', '2'),   -- Oscar es Médico
('user-paciente-003', '3'); -- María es Paciente
GO


ALTER TABLE Usuarios
ALTER COLUMN Estado TINYINT NOT NULL;
GO

SELECT DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Usuarios'
AND COLUMN_NAME = 'Estado';

UPDATE Usuarios
SET PasswordHash = 'vk9oxOJiD5aPcsdU83YBvVgNVjLrvgij3NO2UQAh88I=',
    Estado = 1
WHERE CorreoElectronico = 'juan.admin@caces.com';