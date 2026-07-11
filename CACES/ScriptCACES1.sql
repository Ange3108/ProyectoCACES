--DROP DATABASE [CACES];
--GO

CREATE DATABASE CACES;
GO

USE CACES;
GO


CREATE TABLE Icono(
    Id_Icono INT PRIMARY KEY IDENTITY(1,1),
    Codigo VARCHAR(100) NOT NULL,   
    Nombre VARCHAR(100) NOT NULL,   -- 'Cardiología' (nombre visible en el selector)
);
GO

CREATE TABLE Especialidad(
    Id_Especialidad INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(30) NOT NULL,
    Descripcion VARCHAR(200) NOT NULL,
    Id_Icono INT NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
    Estado BIT NOT NULL,
    CONSTRAINT FK_Especialidad_Icono FOREIGN KEY (Id_Icono) REFERENCES Icono(Id_Icono)
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
    Medicamentos VARCHAR(200) NOT NULL,
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
    Activo BIT NOT NULL,
    CONSTRAINT FK_Horarios_Medico FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico)
);

CREATE TABLE Citas(
    Id_Cita INT PRIMARY KEY IDENTITY(1,1),
    Id_Paciente INT NOT NULL,
    Id_Medico INT NOT NULL,
    Id_Especialidad INT NOT NULL,
    Id_Horario INT NOT NULL,
    Fecha Date NOT NULL,
    Motivo VARCHAR(100) NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
    FechaDeModificacion DATETIME NULL,
    Estado TINYINT NOT NULL,
	CONSTRAINT FK_Citas_Medicos FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico),
	CONSTRAINT FK_Citas_Pacientes FOREIGN KEY (Id_Paciente) REFERENCES Pacientes(Id_Paciente),
    CONSTRAINT FK_Citas_Especialidad FOREIGN KEY (Id_Especialidad) REFERENCES Especialidad(Id_Especialidad),
    CONSTRAINT FK_Citas_Horario FOREIGN KEY (Id_Horario) REFERENCES HorariosDisponibles(Id_Horario)
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
    Nombre VARCHAR(50) NOT NULL,
	Descripcion VARCHAR(100) NOT NULL,
    Duracion VARCHAR(100) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
	FechaDeRegistro DATETIME NOT NULL,
	Estado BIT NOT NULL,
);
GO

CREATE TABLE Procedimiento(
    Id_Procedimiento INT PRIMARY KEY IDENTITY(1,1),
    Id_Especialidad INT NOT NULL,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(200),
    PrecioBase DECIMAL(10,2),
    Estado BIT NOT NULL,

    CONSTRAINT FK_Procedimiento_Especialidad
    FOREIGN KEY(Id_Especialidad)
    REFERENCES Especialidad(Id_Especialidad)
);
GO
CREATE TABLE Precios(
    Id_Precio INT PRIMARY KEY IDENTITY(1,1),
    Id_Medico INT NOT NULL,
    Id_Procedimiento INT NOT NULL,
    Costo DECIMAL(10,2) NOT NULL,
    Detalles VARCHAR(100) NOT NULL,

    CONSTRAINT FK_Precios_Medico FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico),
    CONSTRAINT FK_Precios_Procedimiento FOREIGN KEY (Id_Procedimiento) REFERENCES Procedimiento(Id_Procedimiento)
);
GO

CREATE TABLE Cirugias(
    Id_Cirugia INT PRIMARY KEY IDENTITY(1,1),
    Id_Paciente INT NOT NULL,
    Id_Medico INT NOT NULL,
    Id_Procedimiento INT NOT NULL,
    Id_Horario INT NOT NULL,
    Id_Cita INT not null,
    Estado BIT NOT NULL,
    CONSTRAINT FK_Cirugias_Paciente FOREIGN KEY (Id_Paciente) REFERENCES Pacientes(Id_Paciente),
    CONSTRAINT FK_Cirugias_Medico FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico),
    CONSTRAINT FK_Cirugias_Procedimiento FOREIGN KEY (Id_Procedimiento) REFERENCES Procedimiento(Id_Procedimiento),
    CONSTRAINT FK_Cirugia_Fecha FOREIGN KEY (Id_Horario) REFERENCES HorariosDisponibles(Id_Horario),
    CONSTRAINT FK_Cirugia_Cita FOREIGN KEY (Id_Cita) REFERENCES Citas(Id_Cita),

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

create table ConfiguracionQuirofano(
    id int Primary Key Identity(1,1),
    CupoMaximoDiario int
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

ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO


use CACES
CREATE TABLE [dbo].[UsuarioRoles](
	[Id_Usuario] [int] NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.UsuarioRoles] PRIMARY KEY CLUSTERED 
(
	[Id_Usuario] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[UsuarioRoles] WITH CHECK ADD CONSTRAINT [FK_UsuarioRoles_Usuarios] FOREIGN KEY([Id_Usuario])
REFERENCES [dbo].[Usuarios] ([Id_Usuario])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuarioRoles] CHECK CONSTRAINT [FK_UsuarioRoles_Usuarios]
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

-- Catálogo general de íconos médicos (Bootstrap Icons)
INSERT INTO Icono (Codigo, Nombre) VALUES
('bi bi-heart-pulse', 'Corazón'),
('bi bi-clipboard2-pulse', 'Signos vitales'),
('bi bi-eye', 'Ojo'),
('bi bi-emoji-smile', 'Sonrisa'),
('bi bi-gender-female', 'Mujer'),
('bi bi-gender-male', 'Hombre'),
('bi bi-bandaid', 'Vendaje'),
('bi bi-ear', 'Oído'),
('bi bi-lungs', 'Pulmones'),
('bi bi-droplet-half', 'Gota'),
('bi bi-thermometer-half', 'Termómetro'),
('bi bi-capsule', 'Cápsula'),
('bi bi-clipboard2-plus', 'Historial Médico'),
('bi bi-person-arms-up', 'Rehabilitación'),
('bi bi-magic', 'Estética'),
('bi bi-ribbon', 'Lazo'),
('bi bi-brain', 'Cerebro'),
('bi bi-clipboard2-heart', 'Cuidados generales'),
('bi bi-activity', 'Actividad'),
('bi bi-hospital', 'Hospital');
GO

INSERT INTO Especialidad
(Nombre, Descripcion, Id_Icono, FechaDeRegistro, Estado)
VALUES
('Cirugía General','Procedimientos quirúrgicos generales laparoscópicos y convencionales.',
    1, GETDATE(), 1),

('Ginecología','Procedimientos quirúrgicos relacionados con el sistema reproductor femenino.',
    5, GETDATE(), 1),

('Cirugía Oncológica Mamaria','Procedimientos quirúrgicos relacionados con patologías mamarias.',
    16, GETDATE(), 1),

('Cirugía Plástica','Procedimientos estéticos y reconstructivos.',
    15, GETDATE(), 1),

('Ortopedia y Traumatología','Tratamiento quirúrgico de fracturas y lesiones articulares.',
    14, GETDATE(), 1),

('Otorrinolaringología','Procedimientos quirúrgicos de oído, nariz y garganta.',
    8, GETDATE(), 1);
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
 '1996-03-15', 'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', NEWID(), 0, NULL, 0, 0, 1),
 
 ('Liam', 'Ramírez', 'Mora', 'liam.medico@caces.com', '22334455', 'liam.jpg',
 GETDATE(), NULL, 1, 'Alajuela', 42, '8888-4444',
 '1984-07-12', 'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', NEWID(), 0, NULL, 0, 0, 1),

('Ana', 'Fernández', 'Rojas', 'ana.medico@caces.com', '33445566', 'ana.jpg',
 GETDATE(), NULL, 1, 'Cartago', 31, '8888-5555',
 '1995-11-08', 'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', NEWID(), 0, NULL, 0, 0, 1),

 ('David', 'Jiménez', 'Vargas', 'david.paciente@caces.com', '44556677', 'david.jpg',
GETDATE(), NULL, 1, 'Heredia', 38, '8888-6666',
'1987-03-21', 'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', NEWID(), 0, NULL, 0, 0, 1),

('Juana', 'Solano', 'Castro', 'juana.paciente@caces.com', '55667788', 'maria.jpg',
GETDATE(), NULL, 1, 'San José', 35, '8888-7777',
'1990-09-15', 'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', NEWID(), 0, NULL, 0, 0, 1);
GO

-- MEDICOS
INSERT INTO Medicos (Id_Especialidad, Id_Usuario, Experiencia, Certificaciones, FechaDeRegistro) VALUES
(1, 2, 10, 'Especialista en Cirugía General y Laparoscópica', GETDATE()),
(2, 4, 8, 'Especialista en Ginecología y Obstetricia', GETDATE()),
(4, 5, 12, 'Especialista en Cirugía Plástica y Reconstructiva', GETDATE());
GO

-- HISTORIAL MEDICO
INSERT INTO Historial_Medico (Alergias, Enfermedades_Crónicas, Detalles, Tipo_Sangre, Medicamentos,Antecedentes, FechaDeCreacion, FechaDeModificacion) VALUES
('Penicilina', 'Diabetes tipo 2', 'Paciente controlado', 'O+', 'Omeprazol 20mg, Tramadol 100mg', 'Abuelo materno con antecedentes de diabetes tipo 2', GETDATE(), NULL),
('Ninguna', 'Hipertensión', 'Paciente bajo control médico', 'A+', 'Ninguno', 'Madre con hipertensión arterial crónica', GETDATE(), NULL),
('Aspirina', 'Ninguna', 'Paciente sano', 'B+', 'Cefazolina 1g, Paracetamol 500mg', 'Sin antecedentes familiares de riesgo', GETDATE(), NULL);
GO

-- PACIENTES
INSERT INTO Pacientes (Id_Usuario, Id_Historial) VALUES
(3, 1),
(6, 2),
(7, 3);
GO

-- HORARIOS DISPONIBLES
INSERT INTO HorariosDisponibles (Id_Medico, DiaSemana, HoraInicio, Activo) VALUES
(1, 0, '08:00', 1),
(2, 1, '09:00', 1),
(3, 2, '07:00', 1);
GO

-- CITAS
INSERT INTO Citas (Id_Paciente, Id_Medico, Id_Especialidad, Id_Horario,Fecha, Motivo, FechaDeRegistro, FechaDeModificacion, Estado)VALUES
(1, 1, 1, 1, '2026-07-01','Valoración para colecistectomía laparoscópica',GETDATE(), NULL, 1),
(2, 2, 2, 2, '2026-07-02','Valoración para histerectomía',GETDATE(), NULL, 1),
(3, 3, 4, 3, '2026-07-03', 'Consulta para lipoescultura',GETDATE(), NULL, 1);
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
INSERT INTO Paquetes (Nombre, Descripcion, Duracion, Precio, FechaDeRegistro, Estado) VALUES
('Paquete Laparoscopia', 'Paquete Laparoscopia - Cirugía mínimamente invasiva', '3 meses', '2500.00', GETDATE(), 1),
('Paquete Oncología', 'Paquete Oncología - Tratamiento integral', '6 meses', '5000.00', GETDATE(), 1),
('Paquete Cirugía', 'Paquete Cirugía - Procedimiento quirúrgico', '4 meses', '3500.00', GETDATE(), 1);
GO

INSERT INTO Procedimiento
(Id_Especialidad, Nombre, Descripcion, PrecioBase, Estado)
VALUES
(1, 'Colecistectomía', 'Extirpación quirúrgica de la vesícula biliar.', 1500.00, 1),
(1, 'Apendicectomía', 'Extirpación quirúrgica del apéndice.', 1200.00, 1),
(1, 'Cura de Hernia', 'Reparación quirúrgica de hernias abdominales.', 1100.00, 1),
(1, 'Hernia de Hiato', 'Corrección quirúrgica de hernia hiatal.', 1800.00, 1),
(1, 'Acalasia', 'Tratamiento quirúrgico de la acalasia esofágica.', 2200.00, 1),
(1, 'Esplenectomía', 'Extirpación quirúrgica del bazo.', 2500.00, 1),
(1, 'Colectomía', 'Resección parcial o total del colon.', 3000.00, 1),
(1, 'Gastrectomía', 'Extirpación parcial o total del estómago.', 3500.00, 1),
(2, 'Histerectomía', 'Extirpación quirúrgica del útero.', 2000.00, 1),
(2, 'Esterilización', 'Procedimiento quirúrgico de esterilización femenina.', 800.00, 1),
(2, 'Quiste de Ovario', 'Resección de quistes ováricos.', 1300.00, 1),
(2, 'Resección de Teratoma', 'Extracción quirúrgica de teratomas.', 1600.00, 1),
(3, 'Mastectomía', 'Extirpación parcial o total de la mama.', 2400.00, 1),
(4, 'Lipoescultura', 'Procedimiento estético para moldear el contorno corporal.', 2800.00, 1),
(4, 'Dermolipectomía', 'Extirpación de exceso de piel y tejido adiposo.', 3200.00, 1),
(4, 'Lifting Facial', 'Procedimiento de rejuvenecimiento facial.', 4000.00, 1),
(4, 'Liposucción', 'Extracción de grasa localizada mediante succión.', 2100.00, 1),
(5, 'Reducción de Fracturas', 'Alineación y estabilización de fracturas óseas.', 1700.00, 1),
(5, 'Osteosíntesis', 'Fijación interna de fracturas mediante implantes.', 2300.00, 1),
(5, 'Artroscopia', 'Procedimiento mínimamente invasivo para articulaciones.', 1900.00, 1),
(6, 'Cirugía de Senos Paranasales', 'Intervención quirúrgica de los senos paranasales.', 1400.00, 1);

-- PRECIOS
INSERT INTO Precios
(Id_Medico, Id_Procedimiento, Costo, Detalles) VALUES
(1, 1, 1500.00, 'Colecistectomía laparoscópica'),
(2, 13, 2000.00, 'Mastectomía especializada'),
(3, 2, 1800.00, 'Apendicectomía convencional');
GO

-- CIRUGIAS
INSERT INTO Cirugias
(Id_Paciente, Id_Medico, Id_Procedimiento, Id_Horario, Id_Cita, Estado)
VALUES
(1, 1, 1, 1,1, 1),   -- Colecistectomía
(2, 2, 13, 2,2, 1),  -- Mastectomía
(3, 3, 2, 3,3, 1);   -- Apendicectomía
GO

-- NOTICIAS
INSERT INTO Noticias (Titulo, Contenido, FechaDePublicacion, FechaDeModificacion, Imagen, Estado) VALUES
('Ventajas de la cirugía laparoscópica', 'La laparoscopia reduce el trauma quirúrgico y acelera la recuperación...', GETDATE(), NULL, 'noticia1.png', 1),
('Avances en tratamiento del cáncer', 'Nuevas terapias oncológicas ofrecen mejores resultados en pacientes...', GETDATE(), NULL, 'noticia2.png', 1),
('Preparación para cirugía: lo que debes saber', 'Prepararse correctamente antes de una cirugía es fundamental...', GETDATE(), NULL, 'noticia3.png', 1);
GO


-- ASIGNAR ROLES A USUARIOS
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES
('user-admin-001', '1'),    -- Juan es Administrador
('user-medico-002', '2'),   -- Oscar es Médico
('user-paciente-003', '3'); -- María es Paciente
GO

INSERT INTO UsuarioRoles (Id_Usuario, RoleId)
VALUES
(1, '1'),
(2, '2'),
(3, '3'),
(4, '2'), -- Paciente
(5, '2'); -- Paciente
GO



SELECT DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Usuarios'
AND COLUMN_NAME = 'Estado';

UPDATE Usuarios
SET PasswordHash = 'vk9oxOJiD5aPcsdU83YBvVgNVjLrvgij3NO2UQAh88I=',
    Estado = 1
WHERE CorreoElectronico = 'juan.admin@caces.com';

--Cupo del quirofano

insert into ConfiguracionQuirofano(CupoMaximoDiario)
values 
( 5)
GO