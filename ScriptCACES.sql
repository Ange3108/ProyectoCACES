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
	CorreoElectronico VARCHAR(200) NOT NULL,
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
    Telefono INT NOT NULL,
    Certificaciones VARCHAR(50) NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
	CONSTRAINT FK_Medicos_Especialidad FOREIGN KEY (Id_Especialidad) REFERENCES Especialidad(Id_Especialidad),
	CONSTRAINT FK_Medico_Usuario FOREIGN KEY (Id_Usuario) REFERENCES Usuarios(Id_Usuario)
);
GO
CREATE TABLE Historial_Medico(
    Id_Historial INT PRIMARY KEY IDENTITY(1,1),
    Alergias VARCHAR(20) NOT NULL,
    Detalles VARCHAR(100) NOT NULL,
    Tipo_Sangre VARCHAR(10) NOT NULL,
    Anteriores VARCHAR(50) NOT NULL,
    FechaDelHistorial DATETIME NOT NULL,
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
CREATE TABLE Citas(
    Id_Cita INT PRIMARY KEY IDENTITY(1,1),
    Id_Paciente INT NOT NULL,
    Id_Medico INT NOT NULL,
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL,
    Motivo VARCHAR(100) NOT NULL,
    FechaDeRegistro DATETIME NOT NULL,
    FechaDeModificacion DATETIME NULL,
    Estado TINYINT NOT NULL,
	CONSTRAINT FK_Citas_Medicos FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico),
	CONSTRAINT FK_Citas_Pacientes FOREIGN KEY (Id_Paciente) REFERENCES Pacientes(Id_Paciente)
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
	Detalles VARCHAR(100) NOT NULL,
	CONSTRAINT FK_Precios_Por_Medico FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico)
);
GO
CREATE TABLE Cirugias(
    Id_Cirugia INT PRIMARY KEY IDENTITY(1,1),
	Id_Paciente INT NOT NULL,
	Id_Medico INT NOT NULL,
	Id_Especialidad INT NOT NULL,
    Fecha DATE NOT NULL,
	CONSTRAINT FK_Cirugias_Paciente FOREIGN KEY (Id_Paciente) REFERENCES Pacientes(Id_Paciente),
	CONSTRAINT FK_Cirugias_Medico FOREIGN KEY (Id_Medico) REFERENCES Medicos(Id_Medico),
	CONSTRAINT FK_Cirugias_Especialidad FOREIGN KEY (Id_Especialidad) REFERENCES Especialidad(Id_Especialidad)

);




