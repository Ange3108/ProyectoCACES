CREATE DATABASE CACES;
GO

USE [CACES]
GO
/****** Object:  Table [dbo].[AlertaStaff]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlertaStaff](
	[Id_Alerta] [int] IDENTITY(1,1) NOT NULL,
	[Id_Seguimiento] [int] NOT NULL,
	[FechaGenerada] [datetime] NOT NULL,
	[Estado] [int] NOT NULL,
	[Id_Usuario_Atendio] [int] NULL,
	[Observaciones] [varchar](1000) NULL,
	[FechaAtencion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Alerta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArchivosHistorial]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArchivosHistorial](
	[Id_Archivo] [int] IDENTITY(1,1) NOT NULL,
	[Id_Historial] [int] NOT NULL,
	[NombreArchivo] [varchar](200) NOT NULL,
	[RutaArchivo] [varchar](500) NOT NULL,
	[TipoArchivo] [varchar](50) NULL,
	[FechaDeSubida] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Archivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 12/09/2026 10:48:24 a. m. ******/
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
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 12/09/2026 10:48:24 a. m. ******/
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
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 12/09/2026 10:48:24 a. m. ******/
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
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 12/09/2026 10:48:24 a. m. ******/
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
/****** Object:  Table [dbo].[Cirugias]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cirugias](
	[Id_Cirugia] [int] IDENTITY(1,1) NOT NULL,
	[Id_Paciente] [int] NOT NULL,
	[Id_Medico] [int] NOT NULL,
	[Id_Procedimiento] [int] NOT NULL,
	[Id_Horario] [int] NOT NULL,
	[Id_Cita] [int] NOT NULL,
	[Estado] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Cirugia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Citas]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Citas](
	[Id_Cita] [int] IDENTITY(1,1) NOT NULL,
	[Id_Paciente] [int] NOT NULL,
	[Id_Medico] [int] NOT NULL,
	[Id_Especialidad] [int] NOT NULL,
	[Id_Horario] [int] NOT NULL,
	[Fecha] [date] NOT NULL,
	[Motivo] [varchar](100) NOT NULL,
	[FechaDeRegistro] [datetime] NOT NULL,
	[FechaDeModificacion] [datetime] NULL,
	[Estado] [tinyint] NOT NULL,
	[Id_Procedimiento] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Cita] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Configuracion]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Configuracion](
	[Id_Configuracion] [int] IDENTITY(1,1) NOT NULL,
	[Clave] [varchar](100) NOT NULL,
	[Valor] [varchar](500) NOT NULL,
	[Tipo] [varchar](20) NOT NULL,
	[Categoria] [varchar](100) NOT NULL,
	[Descripcion] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Configuracion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConfiguracionCheckpoints]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConfiguracionCheckpoints](
	[Id_CheckPoint] [int] IDENTITY(1,1) NOT NULL,
	[DiaCheckpoint] [int] NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_CheckPoint] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConfiguracionCotizacion]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConfiguracionCotizacion](
	[Id_Configuracion] [int] IDENTITY(1,1) NOT NULL,
	[PorcentajeEquipo] [decimal](5, 2) NOT NULL,
	[CostoEstadiaDiaria] [decimal](10, 2) NOT NULL,
	[PorcentajeImpuesto] [decimal](5, 2) NOT NULL,
	[Estado] [bit] NOT NULL,
	[FechaDeRegistro] [datetime2](7) NOT NULL,
	[FechaDeModificacion] [datetime2](7) NULL,
 CONSTRAINT [PK_ConfiguracionCotizacion] PRIMARY KEY CLUSTERED 
(
	[Id_Configuracion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConfiguracionQuirofano]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConfiguracionQuirofano](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[CupoMaximoDiario] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Convenios]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Convenios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](150) NOT NULL,
	[Descripcion] [nvarchar](max) NULL,
	[DescuentoPorcentaje] [decimal](5, 2) NULL,
	[ContactoTelefono] [nvarchar](20) NULL,
	[ImagenUrl] [nvarchar](500) NULL,
	[Estado] [bit] NOT NULL,
	[FechaCreacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cotizacion]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cotizacion](
	[Id_Cotizacion] [int] IDENTITY(1,1) NOT NULL,
	[Id_Paciente] [int] NOT NULL,
	[Id_Medico] [int] NOT NULL,
	[Id_Procedimiento] [int] NOT NULL,
	[FechaSolicitud] [datetime] NOT NULL,
	[PrecioBase] [decimal](10, 2) NOT NULL,
	[Descuento] [decimal](10, 2) NOT NULL,
	[Impuesto] [decimal](10, 2) NOT NULL,
	[Total] [decimal](10, 2) NOT NULL,
	[Observaciones] [varchar](500) NULL,
	[Estado] [tinyint] NOT NULL,
	[FechaDeRegistro] [datetime] NOT NULL,
	[FechaDeModificacion] [datetime] NULL,
	[CostoEquipo] [decimal](10, 2) NULL,
	[CostoEstadia] [decimal](10, 2) NULL,
	[DiasEstadia] [int] NULL,
	[HonorariosMedico] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Cotizacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Especialidad]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Especialidad](
	[Id_Especialidad] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](30) NOT NULL,
	[Descripcion] [varchar](200) NOT NULL,
	[Id_Icono] [int] NOT NULL,
	[FechaDeRegistro] [datetime] NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Especialidad] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Historial_Medico]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Historial_Medico](
	[Id_Historial] [int] IDENTITY(1,1) NOT NULL,
	[Alergias] [varchar](200) NOT NULL,
	[Enfermedades_Crónicas] [varchar](200) NOT NULL,
	[Detalles] [varchar](100) NOT NULL,
	[Tipo_Sangre] [varchar](10) NOT NULL,
	[Medicamentos] [varchar](200) NOT NULL,
	[Antecedentes] [varchar](50) NOT NULL,
	[FechaDeCreacion] [datetime] NOT NULL,
	[FechaDeModificacion] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Historial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HorariosDisponibles]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HorariosDisponibles](
	[Id_Horario] [int] IDENTITY(1,1) NOT NULL,
	[Id_Medico] [int] NOT NULL,
	[DiaSemana] [int] NOT NULL,
	[HoraInicio] [time](7) NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Horario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Icono]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Icono](
	[Id_Icono] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [varchar](100) NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Icono] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medicos]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medicos](
	[Id_Medico] [int] IDENTITY(1,1) NOT NULL,
	[Id_Especialidad] [int] NOT NULL,
	[Id_Usuario] [int] NOT NULL,
	[Experiencia] [int] NOT NULL,
	[Certificaciones] [varchar](200) NOT NULL,
	[FechaDeRegistro] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Medico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Noticias]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Noticias](
	[Id_Noticia] [int] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](200) NOT NULL,
	[Contenido] [varchar](max) NOT NULL,
	[FechaDePublicacion] [datetime] NOT NULL,
	[FechaDeModificacion] [datetime] NULL,
	[Imagen] [varchar](200) NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Noticia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notificaciones]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notificaciones](
	[Id_Notificacion] [int] IDENTITY(1,1) NOT NULL,
	[Evento] [varchar](100) NOT NULL,
	[CanalPlataforma] [bit] NOT NULL,
	[CanalEmail] [bit] NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Notificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificacionUsuario]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificacionUsuario](
	[Id_NotificacionUsuario] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Evento] [varchar](100) NOT NULL,
	[Titulo] [varchar](100) NOT NULL,
	[Mensaje] [varchar](500) NOT NULL,
	[Leido] [bit] NULL,
	[FechaCreacion] [datetime] NOT NULL,
	[FechaLectura] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_NotificacionUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pacientes]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pacientes](
	[Id_Paciente] [int] IDENTITY(1,1) NOT NULL,
	[Id_Usuario] [int] NOT NULL,
	[Id_Historial] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Paciente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Paquetes]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Paquetes](
	[Id_Paquete] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NOT NULL,
	[Descripcion] [varchar](100) NOT NULL,
	[Duracion] [varchar](100) NOT NULL,
	[Precio] [decimal](18, 2) NOT NULL,
	[FechaDeRegistro] [datetime] NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Paquete] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Precios]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Precios](
	[Id_Precio] [int] IDENTITY(1,1) NOT NULL,
	[Id_Medico] [int] NOT NULL,
	[Id_Procedimiento] [int] NOT NULL,
	[Costo] [decimal](10, 2) NOT NULL,
	[Detalles] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Precio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PreguntaSeguimiento]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PreguntaSeguimiento](
	[Id_Pregunta] [int] IDENTITY(1,1) NOT NULL,
	[Texto] [varchar](500) NOT NULL,
	[ValorMinimo] [int] NOT NULL,
	[ValorMaximo] [int] NOT NULL,
	[UmbralAlerta] [int] NOT NULL,
	[DireccionAlerta] [int] NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Pregunta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Procedimiento]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Procedimiento](
	[Id_Procedimiento] [int] IDENTITY(1,1) NOT NULL,
	[Id_Especialidad] [int] NOT NULL,
	[Nombre] [varchar](100) NOT NULL,
	[Descripcion] [varchar](200) NULL,
	[PrecioBase] [decimal](10, 2) NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Procedimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recetas]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recetas](
	[Id_Receta] [int] IDENTITY(1,1) NOT NULL,
	[Id_Cita] [int] NOT NULL,
	[Medicamentos] [varchar](max) NOT NULL,
	[Instrucciones] [varchar](500) NULL,
	[FechaDeRegistro] [datetime] NOT NULL,
	[FechaDeVencimiento] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Receta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RespuestaSeguimiento]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RespuestaSeguimiento](
	[Id_Respuesta] [int] IDENTITY(1,1) NOT NULL,
	[Id_Seguimiento] [int] NOT NULL,
	[Id_Pregunta] [int] NOT NULL,
	[ValorRespuesta] [int] NOT NULL,
	[GeneroAlerta] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Respuesta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SeguimientoPaciente]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SeguimientoPaciente](
	[Id_Seguimiento] [int] IDENTITY(1,1) NOT NULL,
	[Id_Cirugia] [int] NOT NULL,
	[DiaCheckpoint] [int] NOT NULL,
	[FechaProgramada] [datetime] NOT NULL,
	[Estado] [int] NOT NULL,
	[FechaRegistro] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Seguimiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Soportes]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Soportes](
	[Id_Soporte] [int] IDENTITY(1,1) NOT NULL,
	[Id_Usuario] [int] NOT NULL,
	[Asunto] [nvarchar](150) NOT NULL,
	[Mensaje] [nvarchar](1000) NOT NULL,
	[FechaConsulta] [datetime] NOT NULL,
	[Estado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Soporte] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UsuarioRoles]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
/****** Object:  Table [dbo].[Usuarios]    Script Date: 12/09/2026 10:48:24 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id_Usuario] [int] IDENTITY(1,1) NOT NULL,
	[Nombres] [varchar](100) NOT NULL,
	[PrimerApellido] [varchar](100) NOT NULL,
	[SegundoApellido] [varchar](100) NOT NULL,
	[CorreoElectronico] [varchar](200) NOT NULL,
	[DUI] [varchar](10) NOT NULL,
	[Foto] [varchar](200) NOT NULL,
	[FechaDeRegistro] [datetime] NOT NULL,
	[FechaDeModificacion] [datetime] NULL,
	[Estado] [bit] NOT NULL,
	[Direccion] [varchar](250) NOT NULL,
	[Edad] [int] NOT NULL,
	[Telefono] [varchar](20) NOT NULL,
	[Nacimiento] [date] NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[SecurityStamp] [nvarchar](max) NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[EmailConfirmed] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id_Usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ArchivosHistorial] ON 

INSERT [dbo].[ArchivosHistorial] ([Id_Archivo], [Id_Historial], [NombreArchivo], [RutaArchivo], [TipoArchivo], [FechaDeSubida]) VALUES (1, 1, N'Radiografia_torax.pdf', N'/archivos/radiografias/rxtoraz1.pdf', N'radiografía', CAST(N'2026-08-05T13:21:37.780' AS DateTime))
INSERT [dbo].[ArchivosHistorial] ([Id_Archivo], [Id_Historial], [NombreArchivo], [RutaArchivo], [TipoArchivo], [FechaDeSubida]) VALUES (2, 2, N'Analisis_sangre.pdf', N'/archivos/analisis/sangre2.pdf', N'análisis', CAST(N'2026-08-05T13:21:37.780' AS DateTime))
INSERT [dbo].[ArchivosHistorial] ([Id_Archivo], [Id_Historial], [NombreArchivo], [RutaArchivo], [TipoArchivo], [FechaDeSubida]) VALUES (3, 3, N'EEG_2024.pdf', N'/archivos/estudios/eeg3.pdf', N'estudio', CAST(N'2026-08-05T13:21:37.780' AS DateTime))
SET IDENTITY_INSERT [dbo].[ArchivosHistorial] OFF
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1', N'Administrador')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'2', N'Medico')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'3', N'Paciente')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'user-admin-001', N'1')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'user-medico-002', N'2')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'user-paciente-003', N'3')
GO
SET IDENTITY_INSERT [dbo].[Cirugias] ON 

INSERT [dbo].[Cirugias] ([Id_Cirugia], [Id_Paciente], [Id_Medico], [Id_Procedimiento], [Id_Horario], [Id_Cita], [Estado]) VALUES (1, 1, 1, 1, 1, 1, 2)
INSERT [dbo].[Cirugias] ([Id_Cirugia], [Id_Paciente], [Id_Medico], [Id_Procedimiento], [Id_Horario], [Id_Cita], [Estado]) VALUES (2, 2, 2, 13, 2, 2, 0)
INSERT [dbo].[Cirugias] ([Id_Cirugia], [Id_Paciente], [Id_Medico], [Id_Procedimiento], [Id_Horario], [Id_Cita], [Estado]) VALUES (3, 3, 3, 2, 3, 3, 1)
INSERT [dbo].[Cirugias] ([Id_Cirugia], [Id_Paciente], [Id_Medico], [Id_Procedimiento], [Id_Horario], [Id_Cita], [Estado]) VALUES (4, 4, 3, 15, 13, 4, 0)
INSERT [dbo].[Cirugias] ([Id_Cirugia], [Id_Paciente], [Id_Medico], [Id_Procedimiento], [Id_Horario], [Id_Cita], [Estado]) VALUES (5, 4, 7, 18, 16, 5, 2)
INSERT [dbo].[Cirugias] ([Id_Cirugia], [Id_Paciente], [Id_Medico], [Id_Procedimiento], [Id_Horario], [Id_Cita], [Estado]) VALUES (6, 4, 2, 10, 9, 6, 2)
INSERT [dbo].[Cirugias] ([Id_Cirugia], [Id_Paciente], [Id_Medico], [Id_Procedimiento], [Id_Horario], [Id_Cita], [Estado]) VALUES (7, 5, 1, 3, 5, 7, 1)
INSERT [dbo].[Cirugias] ([Id_Cirugia], [Id_Paciente], [Id_Medico], [Id_Procedimiento], [Id_Horario], [Id_Cita], [Estado]) VALUES (8, 7, 7, 20, 16, 8, 1)
SET IDENTITY_INSERT [dbo].[Cirugias] OFF
GO
SET IDENTITY_INSERT [dbo].[Citas] ON 

INSERT [dbo].[Citas] ([Id_Cita], [Id_Paciente], [Id_Medico], [Id_Especialidad], [Id_Horario], [Fecha], [Motivo], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Id_Procedimiento]) VALUES (1, 1, 1, 1, 1, CAST(N'2026-07-01' AS Date), N'Valoración para colecistectomía laparoscópica', CAST(N'2026-08-05T13:21:37.780' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Citas] ([Id_Cita], [Id_Paciente], [Id_Medico], [Id_Especialidad], [Id_Horario], [Fecha], [Motivo], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Id_Procedimiento]) VALUES (2, 2, 2, 2, 2, CAST(N'2026-07-02' AS Date), N'Valoración para histerectomía', CAST(N'2026-08-05T13:21:37.780' AS DateTime), NULL, 1, 9)
INSERT [dbo].[Citas] ([Id_Cita], [Id_Paciente], [Id_Medico], [Id_Especialidad], [Id_Horario], [Fecha], [Motivo], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Id_Procedimiento]) VALUES (3, 3, 3, 4, 3, CAST(N'2026-07-03' AS Date), N'Consulta para lipoescultura', CAST(N'2026-08-05T13:21:37.780' AS DateTime), NULL, 1, 14)
INSERT [dbo].[Citas] ([Id_Cita], [Id_Paciente], [Id_Medico], [Id_Especialidad], [Id_Horario], [Fecha], [Motivo], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Id_Procedimiento]) VALUES (4, 4, 3, 4, 13, CAST(N'2026-08-12' AS Date), N'y', CAST(N'2026-08-05T19:24:51.340' AS DateTime), NULL, 1, 15)
INSERT [dbo].[Citas] ([Id_Cita], [Id_Paciente], [Id_Medico], [Id_Especialidad], [Id_Horario], [Fecha], [Motivo], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Id_Procedimiento]) VALUES (5, 4, 7, 5, 16, CAST(N'2026-08-05' AS Date), N'prueba seguimientio', CAST(N'2026-08-05T19:43:59.477' AS DateTime), NULL, 1, 18)
INSERT [dbo].[Citas] ([Id_Cita], [Id_Paciente], [Id_Medico], [Id_Especialidad], [Id_Horario], [Fecha], [Motivo], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Id_Procedimiento]) VALUES (6, 4, 2, 2, 9, CAST(N'2026-08-05' AS Date), N'ghbfdh', CAST(N'2026-08-05T22:49:37.637' AS DateTime), NULL, 1, 10)
INSERT [dbo].[Citas] ([Id_Cita], [Id_Paciente], [Id_Medico], [Id_Especialidad], [Id_Horario], [Fecha], [Motivo], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Id_Procedimiento]) VALUES (7, 5, 1, 1, 5, CAST(N'2026-08-20' AS Date), N'Dolor', CAST(N'2026-08-16T16:20:52.440' AS DateTime), NULL, 1, 3)
INSERT [dbo].[Citas] ([Id_Cita], [Id_Paciente], [Id_Medico], [Id_Especialidad], [Id_Horario], [Fecha], [Motivo], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Id_Procedimiento]) VALUES (8, 7, 7, 5, 16, CAST(N'2026-08-26' AS Date), N'Dolor en la articulación', CAST(N'2026-08-17T02:43:46.413' AS DateTime), NULL, 1, 20)
SET IDENTITY_INSERT [dbo].[Citas] OFF
GO
SET IDENTITY_INSERT [dbo].[Configuracion] ON 

INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (1, N'Smtp.Host', N'smtp.gmail.com', N'string', N'Smtp', N'Servidor SMTP utilizado para el envío de correos')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (2, N'Smtp.Puerto', N'587', N'int', N'Smtp', N'Puerto del servidor SMTP')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (3, N'Smtp.Usuario', N'bebepulpo0403@gmail.com', N'string', N'Smtp', N'Usuario/correo remitente del sistema')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (4, N'Smtp.UsarSSL', N'true', N'bool', N'Smtp', N'Indica si la conexión SMTP usa SSL/TLS')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (5, N'Smtp.Password', N'sncy xtbl qoci zswc', N'string', N'Smtp', N'Contraseña del correo para enviar las notificaciones')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (6, N'NotificarCorreo', N'true', N'bool', N'Notificaciones', N'Habilita el envío de notificaciones por correo')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (7, N'NotificarSMS', N'false', N'bool', N'Notificaciones', N'Habilita el envío de notificaciones por SMS')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (8, N'NotificarSistema', N'true', N'bool', N'Notificaciones', N'Habilita el badge de alertas dentro del panel administrativo')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (9, N'DiasCheckpoint1', N'3', N'int', N'Seguimiento', N'Día posterior a la cirugía para el primer checkpoint')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (10, N'DiasCheckpoint2', N'7', N'int', N'Seguimiento', N'Día posterior a la cirugía para el segundo checkpoint')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (11, N'DiasCheckpoint3', N'15', N'int', N'Seguimiento', N'Día posterior a la cirugía para el tercer checkpoint')
INSERT [dbo].[Configuracion] ([Id_Configuracion], [Clave], [Valor], [Tipo], [Categoria], [Descripcion]) VALUES (12, N'HoraEnvioRecordatorios', N'08:00', N'string', N'Seguimiento', N'Hora del día en que el job de Hangfire envía los recordatorios')
SET IDENTITY_INSERT [dbo].[Configuracion] OFF
GO
SET IDENTITY_INSERT [dbo].[ConfiguracionCheckpoints] ON 

INSERT [dbo].[ConfiguracionCheckpoints] ([Id_CheckPoint], [DiaCheckpoint], [Estado]) VALUES (1, 1, 1)
INSERT [dbo].[ConfiguracionCheckpoints] ([Id_CheckPoint], [DiaCheckpoint], [Estado]) VALUES (2, 3, 1)
INSERT [dbo].[ConfiguracionCheckpoints] ([Id_CheckPoint], [DiaCheckpoint], [Estado]) VALUES (3, 7, 1)
INSERT [dbo].[ConfiguracionCheckpoints] ([Id_CheckPoint], [DiaCheckpoint], [Estado]) VALUES (4, 15, 1)
SET IDENTITY_INSERT [dbo].[ConfiguracionCheckpoints] OFF
GO
SET IDENTITY_INSERT [dbo].[ConfiguracionCotizacion] ON 

INSERT [dbo].[ConfiguracionCotizacion] ([Id_Configuracion], [PorcentajeEquipo], [CostoEstadiaDiaria], [PorcentajeImpuesto], [Estado], [FechaDeRegistro], [FechaDeModificacion]) VALUES (1, CAST(35.00 AS Decimal(5, 2)), CAST(250.00 AS Decimal(10, 2)), CAST(13.00 AS Decimal(5, 2)), 1, CAST(N'2026-08-05T13:21:37.8933333' AS DateTime2), NULL)
SET IDENTITY_INSERT [dbo].[ConfiguracionCotizacion] OFF
GO
SET IDENTITY_INSERT [dbo].[ConfiguracionQuirofano] ON 

INSERT [dbo].[ConfiguracionQuirofano] ([id], [CupoMaximoDiario]) VALUES (1, 5)
SET IDENTITY_INSERT [dbo].[ConfiguracionQuirofano] OFF
GO
SET IDENTITY_INSERT [dbo].[Convenios] ON 

INSERT [dbo].[Convenios] ([Id], [Nombre], [Descripcion], [DescuentoPorcentaje], [ContactoTelefono], [ImagenUrl], [Estado], [FechaCreacion]) VALUES (1, N'Convenio de Laboratorio Clínico', N'Tu procesamiento de análisis clínicos cuenta con prioridad VIP en la línea de análisis para agilizar tu diagnóstico médico. El convenio te asegura tarifas corporativas exclusivas en perfiles hormonales, químicos e inmunológicos generales. Los resultados se envían automáticamente al sistema interno de CACES para que tu especialista los revise de inmediato.', CAST(20.00 AS Decimal(5, 2)), N'8584-6870', N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQAyBAy2__h8h1u-HyzGM-CPbIb5RcPQwdsxSHtB6iNX8i0k4lJ07e8eyow&s=10', 1, CAST(N'2026-08-05T13:21:37.877' AS DateTime))
INSERT [dbo].[Convenios] ([Id], [Nombre], [Descripcion], [DescuentoPorcentaje], [ContactoTelefono], [ImagenUrl], [Estado], [FechaCreacion]) VALUES (2, N'Convenio de Imagenología Radiológica', N'Acceso preferencial a estudios de Resonancia Magnética (RMN), Tomografía Computarizada (TAC), Ultrasonidos y Rayos X de última generación. Interpretación garantizada por radiólogos certificados internacionalmente, asegurando informes con precisión diagnóstica de estándar global. Recibe tus imágenes en formato digital compatible para que puedas descargarlas, llevarlas contigo o compartirlas con tus médicos en tu país de origen.', CAST(25.00 AS Decimal(5, 2)), N'8584-6990', N'https://www.campustraining.es/wp-content/uploads/2024/01/Densidades-radiologicas.png', 1, CAST(N'2026-08-05T13:21:37.877' AS DateTime))
SET IDENTITY_INSERT [dbo].[Convenios] OFF
GO
SET IDENTITY_INSERT [dbo].[Especialidad] ON 

INSERT [dbo].[Especialidad] ([Id_Especialidad], [Nombre], [Descripcion], [Id_Icono], [FechaDeRegistro], [Estado]) VALUES (1, N'Cirugía General', N'Procedimientos quirúrgicos generales laparoscópicos y convencionales.', 1, CAST(N'2026-08-05T13:21:37.743' AS DateTime), 1)
INSERT [dbo].[Especialidad] ([Id_Especialidad], [Nombre], [Descripcion], [Id_Icono], [FechaDeRegistro], [Estado]) VALUES (2, N'Ginecología', N'Procedimientos quirúrgicos relacionados con el sistema reproductor femenino.', 5, CAST(N'2026-08-05T13:21:37.743' AS DateTime), 1)
INSERT [dbo].[Especialidad] ([Id_Especialidad], [Nombre], [Descripcion], [Id_Icono], [FechaDeRegistro], [Estado]) VALUES (3, N'Cirugía Oncológica Mamaria', N'Procedimientos quirúrgicos relacionados con patologías mamarias.', 16, CAST(N'2026-08-05T13:21:37.743' AS DateTime), 1)
INSERT [dbo].[Especialidad] ([Id_Especialidad], [Nombre], [Descripcion], [Id_Icono], [FechaDeRegistro], [Estado]) VALUES (4, N'Cirugía Plástica', N'Procedimientos estéticos y reconstructivos.', 15, CAST(N'2026-08-05T13:21:37.743' AS DateTime), 1)
INSERT [dbo].[Especialidad] ([Id_Especialidad], [Nombre], [Descripcion], [Id_Icono], [FechaDeRegistro], [Estado]) VALUES (5, N'Ortopedia y Traumatología', N'Tratamiento quirúrgico de fracturas y lesiones articulares.', 14, CAST(N'2026-08-05T13:21:37.743' AS DateTime), 1)
INSERT [dbo].[Especialidad] ([Id_Especialidad], [Nombre], [Descripcion], [Id_Icono], [FechaDeRegistro], [Estado]) VALUES (6, N'Otorrinolaringología', N'Procedimientos quirúrgicos de oído, nariz y garganta.', 8, CAST(N'2026-08-05T13:21:37.743' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Especialidad] OFF
GO
SET IDENTITY_INSERT [dbo].[Historial_Medico] ON 

INSERT [dbo].[Historial_Medico] ([Id_Historial], [Alergias], [Enfermedades_Crónicas], [Detalles], [Tipo_Sangre], [Medicamentos], [Antecedentes], [FechaDeCreacion], [FechaDeModificacion]) VALUES (1, N'Penicilina', N'Diabetes tipo 2', N'Paciente controlado', N'O+', N'Omeprazol 20mg, Tramadol 100mg', N'Abuelo materno con antecedentes de diabetes tipo 2', CAST(N'2026-08-05T13:21:37.760' AS DateTime), NULL)
INSERT [dbo].[Historial_Medico] ([Id_Historial], [Alergias], [Enfermedades_Crónicas], [Detalles], [Tipo_Sangre], [Medicamentos], [Antecedentes], [FechaDeCreacion], [FechaDeModificacion]) VALUES (2, N'Ninguna', N'Hipertensión', N'Paciente bajo control médico', N'A+', N'Ninguno', N'Madre con hipertensión arterial crónica', CAST(N'2026-08-05T13:21:37.760' AS DateTime), NULL)
INSERT [dbo].[Historial_Medico] ([Id_Historial], [Alergias], [Enfermedades_Crónicas], [Detalles], [Tipo_Sangre], [Medicamentos], [Antecedentes], [FechaDeCreacion], [FechaDeModificacion]) VALUES (3, N'Aspirina', N'Ninguna', N'Paciente sano', N'B+', N'Cefazolina 1g, Paracetamol 500mg', N'Sin antecedentes familiares de riesgo', CAST(N'2026-08-05T13:21:37.760' AS DateTime), NULL)
INSERT [dbo].[Historial_Medico] ([Id_Historial], [Alergias], [Enfermedades_Crónicas], [Detalles], [Tipo_Sangre], [Medicamentos], [Antecedentes], [FechaDeCreacion], [FechaDeModificacion]) VALUES (4, N'd', N'd', N'd', N'qwe', N'd', N'd', CAST(N'2026-08-05T19:24:20.707' AS DateTime), NULL)
INSERT [dbo].[Historial_Medico] ([Id_Historial], [Alergias], [Enfermedades_Crónicas], [Detalles], [Tipo_Sangre], [Medicamentos], [Antecedentes], [FechaDeCreacion], [FechaDeModificacion]) VALUES (6, N'Mosquitos', N'Trigleceridos', N'Nada', N'Rho+', N'Novaztatina', N'Hernia Discal', CAST(N'2026-08-16T16:18:26.280' AS DateTime), NULL)
INSERT [dbo].[Historial_Medico] ([Id_Historial], [Alergias], [Enfermedades_Crónicas], [Detalles], [Tipo_Sangre], [Medicamentos], [Antecedentes], [FechaDeCreacion], [FechaDeModificacion]) VALUES (8, N'asd', N'asda', N'asd', N'asd', N'asdasd', N'asdas', CAST(N'2026-08-17T02:25:32.457' AS DateTime), NULL)
INSERT [dbo].[Historial_Medico] ([Id_Historial], [Alergias], [Enfermedades_Crónicas], [Detalles], [Tipo_Sangre], [Medicamentos], [Antecedentes], [FechaDeCreacion], [FechaDeModificacion]) VALUES (10, N'Rinitis', N'Hipertensión', N'Nada', N'O-', N'Amilodipina, acetaminofen', N'Hernia discal', CAST(N'2026-08-17T02:48:01.037' AS DateTime), CAST(N'2026-08-17T02:48:01.040' AS DateTime))
SET IDENTITY_INSERT [dbo].[Historial_Medico] OFF
GO
SET IDENTITY_INSERT [dbo].[HorariosDisponibles] ON 

INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (1, 1, 0, CAST(N'08:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (2, 1, 1, CAST(N'13:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (3, 1, 2, CAST(N'08:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (4, 1, 3, CAST(N'13:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (5, 1, 4, CAST(N'08:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (6, 1, 5, CAST(N'13:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (7, 2, 1, CAST(N'09:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (8, 2, 2, CAST(N'14:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (9, 2, 3, CAST(N'09:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (10, 2, 4, CAST(N'14:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (11, 2, 5, CAST(N'09:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (12, 3, 2, CAST(N'07:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (13, 3, 3, CAST(N'12:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (14, 3, 4, CAST(N'07:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (15, 3, 5, CAST(N'12:00:00' AS Time), 1)
INSERT [dbo].[HorariosDisponibles] ([Id_Horario], [Id_Medico], [DiaSemana], [HoraInicio], [Estado]) VALUES (16, 7, 3, CAST(N'14:00:00' AS Time), 1)
SET IDENTITY_INSERT [dbo].[HorariosDisponibles] OFF
GO
SET IDENTITY_INSERT [dbo].[Icono] ON 

INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (1, N'bi bi-heart-pulse', N'Corazón')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (2, N'bi bi-clipboard2-pulse', N'Signos vitales')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (3, N'bi bi-eye', N'Ojo')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (4, N'bi bi-emoji-smile', N'Sonrisa')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (5, N'bi bi-gender-female', N'Mujer')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (6, N'bi bi-gender-male', N'Hombre')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (7, N'bi bi-bandaid', N'Vendaje')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (8, N'bi bi-ear', N'Oído')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (9, N'bi bi-lungs', N'Pulmones')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (10, N'bi bi-droplet-half', N'Gota')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (11, N'bi bi-thermometer-half', N'Termómetro')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (12, N'bi bi-capsule', N'Cápsula')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (13, N'bi bi-clipboard2-plus', N'Historial Médico')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (14, N'bi bi-person-arms-up', N'Rehabilitación')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (15, N'bi bi-magic', N'Estética')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (16, N'bi bi-award', N'Lazo')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (18, N'bi bi-clipboard2-heart', N'Cuidados generales')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (19, N'bi bi-activity', N'Actividad')
INSERT [dbo].[Icono] ([Id_Icono], [Codigo], [Nombre]) VALUES (20, N'bi bi-hospital', N'Hospital')
SET IDENTITY_INSERT [dbo].[Icono] OFF
GO
SET IDENTITY_INSERT [dbo].[Medicos] ON 

INSERT [dbo].[Medicos] ([Id_Medico], [Id_Especialidad], [Id_Usuario], [Experiencia], [Certificaciones], [FechaDeRegistro]) VALUES (1, 1, 2, 10, N'Especialista en Cirugía General y Laparoscópica', CAST(N'2026-08-05T13:21:37.757' AS DateTime))
INSERT [dbo].[Medicos] ([Id_Medico], [Id_Especialidad], [Id_Usuario], [Experiencia], [Certificaciones], [FechaDeRegistro]) VALUES (2, 2, 4, 8, N'Especialista en Ginecología y Obstetricia', CAST(N'2026-08-05T13:21:37.757' AS DateTime))
INSERT [dbo].[Medicos] ([Id_Medico], [Id_Especialidad], [Id_Usuario], [Experiencia], [Certificaciones], [FechaDeRegistro]) VALUES (3, 4, 5, 12, N'Especialista en Cirugía Plástica y Reconstructiva', CAST(N'2026-08-05T13:21:37.757' AS DateTime))
INSERT [dbo].[Medicos] ([Id_Medico], [Id_Especialidad], [Id_Usuario], [Experiencia], [Certificaciones], [FechaDeRegistro]) VALUES (4, 1, 8, 15, N'Especialista en Cirugía General, Cirugía de Emergencias y Laparoscopía Avanzada', CAST(N'2026-08-05T13:21:37.757' AS DateTime))
INSERT [dbo].[Medicos] ([Id_Medico], [Id_Especialidad], [Id_Usuario], [Experiencia], [Certificaciones], [FechaDeRegistro]) VALUES (5, 3, 9, 11, N'Especialista en Cirugía Oncológica Mamaria y Patología Mamaria', CAST(N'2026-08-05T13:21:37.757' AS DateTime))
INSERT [dbo].[Medicos] ([Id_Medico], [Id_Especialidad], [Id_Usuario], [Experiencia], [Certificaciones], [FechaDeRegistro]) VALUES (6, 6, 10, 9, N'Especialista en Otorrinolaringología y Cirugía Endoscópica Nasosinusal', CAST(N'2026-08-05T13:21:37.757' AS DateTime))
INSERT [dbo].[Medicos] ([Id_Medico], [Id_Especialidad], [Id_Usuario], [Experiencia], [Certificaciones], [FechaDeRegistro]) VALUES (7, 5, 11, 18, N'Especialista en Ortopedia, Reemplazo Articular y Traumatología Deportiva', CAST(N'2026-08-05T13:21:37.757' AS DateTime))
INSERT [dbo].[Medicos] ([Id_Medico], [Id_Especialidad], [Id_Usuario], [Experiencia], [Certificaciones], [FechaDeRegistro]) VALUES (8, 2, 12, 13, N'Especialista en Ginecología, Obstetricia y Cirugía Mínimamente Invasiva', CAST(N'2026-08-05T13:21:37.757' AS DateTime))
SET IDENTITY_INSERT [dbo].[Medicos] OFF
GO
SET IDENTITY_INSERT [dbo].[Noticias] ON 

INSERT [dbo].[Noticias] ([Id_Noticia], [Titulo], [Contenido], [FechaDePublicacion], [FechaDeModificacion], [Imagen], [Estado]) VALUES (1, N'Ventajas de la cirugía laparoscópica', N'La laparoscopia reduce el trauma quirúrgico y acelera la recuperación...', CAST(N'2026-08-05T13:21:37.797' AS DateTime), NULL, N'noticia1.png', 1)
INSERT [dbo].[Noticias] ([Id_Noticia], [Titulo], [Contenido], [FechaDePublicacion], [FechaDeModificacion], [Imagen], [Estado]) VALUES (2, N'Avances en tratamiento del cáncer', N'Nuevas terapias oncológicas ofrecen mejores resultados en pacientes...', CAST(N'2026-08-05T13:21:37.797' AS DateTime), NULL, N'noticia2.png', 1)
INSERT [dbo].[Noticias] ([Id_Noticia], [Titulo], [Contenido], [FechaDePublicacion], [FechaDeModificacion], [Imagen], [Estado]) VALUES (3, N'Preparación para cirugía: lo que debes saber', N'Prepararse correctamente antes de una cirugía es fundamental...', CAST(N'2026-08-05T13:21:37.797' AS DateTime), NULL, N'noticia3.png', 1)
SET IDENTITY_INSERT [dbo].[Noticias] OFF
GO
SET IDENTITY_INSERT [dbo].[Notificaciones] ON 

INSERT [dbo].[Notificaciones] ([Id_Notificacion], [Evento], [CanalPlataforma], [CanalEmail], [Estado]) VALUES (1, N'Seguimiento Alerta', 1, 1, 1)
INSERT [dbo].[Notificaciones] ([Id_Notificacion], [Evento], [CanalPlataforma], [CanalEmail], [Estado]) VALUES (2, N'RecordatorioCheckpoint', 1, 1, 1)
SET IDENTITY_INSERT [dbo].[Notificaciones] OFF
GO
SET IDENTITY_INSERT [dbo].[NotificacionUsuario] ON 

INSERT [dbo].[NotificacionUsuario] ([Id_NotificacionUsuario], [IdUsuario], [Evento], [Titulo], [Mensaje], [Leido], [FechaCreacion], [FechaLectura]) VALUES (2, 3, N'RecordatorioCheckpoint', N'Recordatorio de seguimiento post-operatorio', N'Es momento de completar tu encuesta de seguimiento (día 1).', 0, CAST(N'2026-08-05T23:15:30.963' AS DateTime), NULL)
INSERT [dbo].[NotificacionUsuario] ([Id_NotificacionUsuario], [IdUsuario], [Evento], [Titulo], [Mensaje], [Leido], [FechaCreacion], [FechaLectura]) VALUES (3, 14, N'RecordatorioCheckpoint', N'Recordatorio de seguimiento post-operatorio', N'Es momento de completar tu encuesta de seguimiento (día 1).', 1, CAST(N'2026-08-05T23:15:33.327' AS DateTime), CAST(N'2026-09-12T05:13:13.993' AS DateTime))
SET IDENTITY_INSERT [dbo].[NotificacionUsuario] OFF
GO
SET IDENTITY_INSERT [dbo].[Pacientes] ON 

INSERT [dbo].[Pacientes] ([Id_Paciente], [Id_Usuario], [Id_Historial]) VALUES (1, 3, 1)
INSERT [dbo].[Pacientes] ([Id_Paciente], [Id_Usuario], [Id_Historial]) VALUES (2, 6, 2)
INSERT [dbo].[Pacientes] ([Id_Paciente], [Id_Usuario], [Id_Historial]) VALUES (3, 7, 3)
INSERT [dbo].[Pacientes] ([Id_Paciente], [Id_Usuario], [Id_Historial]) VALUES (4, 14, 4)
INSERT [dbo].[Pacientes] ([Id_Paciente], [Id_Usuario], [Id_Historial]) VALUES (5, 16, 6)
INSERT [dbo].[Pacientes] ([Id_Paciente], [Id_Usuario], [Id_Historial]) VALUES (7, 20, 10)
SET IDENTITY_INSERT [dbo].[Pacientes] OFF
GO
SET IDENTITY_INSERT [dbo].[Paquetes] ON 

INSERT [dbo].[Paquetes] ([Id_Paquete], [Nombre], [Descripcion], [Duracion], [Precio], [FechaDeRegistro], [Estado]) VALUES (1, N'Paquete Laparoscopia', N'Paquete Laparoscopia - Cirugía mínimamente invasiva', N'3 meses', CAST(2500.00 AS Decimal(18, 2)), CAST(N'2026-08-05T13:21:37.787' AS DateTime), 1)
INSERT [dbo].[Paquetes] ([Id_Paquete], [Nombre], [Descripcion], [Duracion], [Precio], [FechaDeRegistro], [Estado]) VALUES (2, N'Paquete Oncología', N'Paquete Oncología - Tratamiento integral', N'6 meses', CAST(5000.00 AS Decimal(18, 2)), CAST(N'2026-08-05T13:21:37.787' AS DateTime), 1)
INSERT [dbo].[Paquetes] ([Id_Paquete], [Nombre], [Descripcion], [Duracion], [Precio], [FechaDeRegistro], [Estado]) VALUES (3, N'Paquete Cirugía', N'Paquete Cirugía - Procedimiento quirúrgico', N'4 meses', CAST(3500.00 AS Decimal(18, 2)), CAST(N'2026-08-05T13:21:37.787' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Paquetes] OFF
GO
SET IDENTITY_INSERT [dbo].[Precios] ON 

INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (1, 1, 1, CAST(900.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por colecistectomía')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (2, 1, 2, CAST(750.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por apendicectomía')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (3, 1, 3, CAST(850.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por reparación de hernia')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (4, 1, 4, CAST(1200.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por cirugía de hernia de hiato')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (5, 1, 5, CAST(1300.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por tratamiento quirúrgico de acalasia')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (6, 1, 6, CAST(1400.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por esplenectomía')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (7, 1, 7, CAST(1800.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por colectomía')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (8, 1, 8, CAST(2000.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por gastrectomía')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (9, 1, 12, CAST(1500.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por procedimiento de resección')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (10, 1, 13, CAST(1600.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por mastectomía')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (11, 2, 9, CAST(1500.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por histerectomía')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (12, 2, 10, CAST(600.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por esterilización quirúrgica')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (13, 2, 11, CAST(1100.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por cirugía de quiste de ovario')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (14, 3, 14, CAST(1500.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por lipoescultura')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (15, 3, 15, CAST(1800.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por dermolipectomía')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (16, 3, 16, CAST(2000.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por lifting facial')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (17, 3, 17, CAST(1400.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por liposucción')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (18, 3, 18, CAST(1700.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por cirugía de reducción')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (19, 3, 21, CAST(1800.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por cirugía estética de senos')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (20, 4, 19, CAST(1600.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por osteosíntesis')
INSERT [dbo].[Precios] ([Id_Precio], [Id_Medico], [Id_Procedimiento], [Costo], [Detalles]) VALUES (21, 4, 20, CAST(1400.00 AS Decimal(10, 2)), N'Honorarios médicos en USD por artroscopia')
SET IDENTITY_INSERT [dbo].[Precios] OFF
GO
SET IDENTITY_INSERT [dbo].[PreguntaSeguimiento] ON 

INSERT [dbo].[PreguntaSeguimiento] ([Id_Pregunta], [Texto], [ValorMinimo], [ValorMaximo], [UmbralAlerta], [DireccionAlerta], [Estado]) VALUES (1, N'Del 1 al 10, ¿cómo calificaría su dolor actual?', 1, 10, 7, 0, 1)
INSERT [dbo].[PreguntaSeguimiento] ([Id_Pregunta], [Texto], [ValorMinimo], [ValorMaximo], [UmbralAlerta], [DireccionAlerta], [Estado]) VALUES (2, N'Del 1 al 10, ¿qué tan hinchada/inflamada ve la zona operada?', 1, 10, 7, 0, 1)
INSERT [dbo].[PreguntaSeguimiento] ([Id_Pregunta], [Texto], [ValorMinimo], [ValorMaximo], [UmbralAlerta], [DireccionAlerta], [Estado]) VALUES (3, N'Del 1 al 10, ¿qué tan bien está durmiendo?', 1, 10, 3, 1, 1)
INSERT [dbo].[PreguntaSeguimiento] ([Id_Pregunta], [Texto], [ValorMinimo], [ValorMaximo], [UmbralAlerta], [DireccionAlerta], [Estado]) VALUES (4, N'¿Ha tenido fiebre? (0 = No, 1 = Sí)', 0, 1, 1, 0, 1)
INSERT [dbo].[PreguntaSeguimiento] ([Id_Pregunta], [Texto], [ValorMinimo], [ValorMaximo], [UmbralAlerta], [DireccionAlerta], [Estado]) VALUES (5, N'¿Ha notado sangrado o secreción anormal? (0 = No, 1 = Sí)', 0, 1, 1, 0, 1)
INSERT [dbo].[PreguntaSeguimiento] ([Id_Pregunta], [Texto], [ValorMinimo], [ValorMaximo], [UmbralAlerta], [DireccionAlerta], [Estado]) VALUES (6, N'Del 1 al 10, ¿qué tan satisfecho está con su recuperación general?', 1, 10, 4, 1, 1)
SET IDENTITY_INSERT [dbo].[PreguntaSeguimiento] OFF
GO
SET IDENTITY_INSERT [dbo].[Procedimiento] ON 

INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (1, 1, N'Colecistectomía', N'Extirpación quirúrgica de la vesícula biliar.', CAST(1500.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (2, 1, N'Apendicectomía', N'Extirpación quirúrgica del apéndice.', CAST(1200.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (3, 1, N'Cura de Hernia', N'Reparación quirúrgica de hernias abdominales.', CAST(1100.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (4, 1, N'Hernia de Hiato', N'Corrección quirúrgica de hernia hiatal.', CAST(1800.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (5, 1, N'Acalasia', N'Tratamiento quirúrgico de la acalasia esofágica.', CAST(2200.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (6, 1, N'Esplenectomía', N'Extirpación quirúrgica del bazo.', CAST(2500.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (7, 1, N'Colectomía', N'Resección parcial o total del colon.', CAST(3000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (8, 1, N'Gastrectomía', N'Extirpación parcial o total del estómago.', CAST(3500.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (9, 2, N'Histerectomía', N'Extirpación quirúrgica del útero.', CAST(2000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (10, 2, N'Esterilización', N'Procedimiento quirúrgico de esterilización femenina.', CAST(800.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (11, 2, N'Quiste de Ovario', N'Resección de quistes ováricos.', CAST(1300.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (12, 2, N'Resección de Teratoma', N'Extracción quirúrgica de teratomas.', CAST(1600.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (13, 3, N'Mastectomía', N'Extirpación parcial o total de la mama.', CAST(2400.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (14, 4, N'Lipoescultura', N'Procedimiento estético para moldear el contorno corporal.', CAST(2800.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (15, 4, N'Dermolipectomía', N'Extirpación de exceso de piel y tejido adiposo.', CAST(3200.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (16, 4, N'Lifting Facial', N'Procedimiento de rejuvenecimiento facial.', CAST(4000.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (17, 4, N'Liposucción', N'Extracción de grasa localizada mediante succión.', CAST(2100.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (18, 5, N'Reducción de Fracturas', N'Alineación y estabilización de fracturas óseas.', CAST(1700.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (19, 5, N'Osteosíntesis', N'Fijación interna de fracturas mediante implantes.', CAST(2300.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (20, 5, N'Artroscopia', N'Procedimiento mínimamente invasivo para articulaciones.', CAST(1900.00 AS Decimal(10, 2)), 1)
INSERT [dbo].[Procedimiento] ([Id_Procedimiento], [Id_Especialidad], [Nombre], [Descripcion], [PrecioBase], [Estado]) VALUES (21, 6, N'Cirugía de Senos Paranasales', N'Intervención quirúrgica de los senos paranasales.', CAST(1400.00 AS Decimal(10, 2)), 1)
SET IDENTITY_INSERT [dbo].[Procedimiento] OFF
GO
SET IDENTITY_INSERT [dbo].[Recetas] ON 

INSERT [dbo].[Recetas] ([Id_Receta], [Id_Cita], [Medicamentos], [Instrucciones], [FechaDeRegistro], [FechaDeVencimiento]) VALUES (1, 1, N'Omeprazol 20mg, Tramadol 100mg', N'Tomar según indicaciones postoperatorias', CAST(N'2026-08-05T13:21:37.787' AS DateTime), CAST(N'2026-09-04T13:21:37.787' AS DateTime))
INSERT [dbo].[Recetas] ([Id_Receta], [Id_Cita], [Medicamentos], [Instrucciones], [FechaDeRegistro], [FechaDeVencimiento]) VALUES (2, 2, N'Doxorrubicina IV, Ciclofosfamida IV', N'Administrar según protocolo de quimioterapia', CAST(N'2026-08-05T13:21:37.787' AS DateTime), CAST(N'2026-09-04T13:21:37.787' AS DateTime))
INSERT [dbo].[Recetas] ([Id_Receta], [Id_Cita], [Medicamentos], [Instrucciones], [FechaDeRegistro], [FechaDeVencimiento]) VALUES (3, 3, N'Cefazolina 1g, Paracetamol 500mg', N'Tomar antibiótico y analgésico postoperatorio', CAST(N'2026-08-05T13:21:37.787' AS DateTime), CAST(N'2026-09-04T13:21:37.787' AS DateTime))
SET IDENTITY_INSERT [dbo].[Recetas] OFF
GO
SET IDENTITY_INSERT [dbo].[SeguimientoPaciente] ON 

INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (1, 1, 1, CAST(N'2026-08-05T22:48:09.013' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (2, 1, 3, CAST(N'2026-07-04T00:00:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (3, 1, 7, CAST(N'2026-07-08T00:00:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (4, 1, 15, CAST(N'2026-07-16T00:00:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (5, 5, 1, CAST(N'2026-08-06T00:00:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (6, 5, 3, CAST(N'2026-08-08T00:00:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (7, 5, 7, CAST(N'2026-08-12T00:00:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (8, 5, 15, CAST(N'2026-08-20T00:00:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (9, 6, 1, CAST(N'2026-08-05T22:58:22.650' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (10, 6, 3, CAST(N'2026-08-08T00:00:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (11, 6, 7, CAST(N'2026-08-12T00:00:00.000' AS DateTime), 0, NULL)
INSERT [dbo].[SeguimientoPaciente] ([Id_Seguimiento], [Id_Cirugia], [DiaCheckpoint], [FechaProgramada], [Estado], [FechaRegistro]) VALUES (12, 6, 15, CAST(N'2026-08-20T00:00:00.000' AS DateTime), 0, NULL)
SET IDENTITY_INSERT [dbo].[SeguimientoPaciente] OFF
GO
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (1, N'1')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (2, N'2')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (3, N'3')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (4, N'2')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (5, N'2')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (8, N'2')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (9, N'2')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (10, N'2')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (11, N'2')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (12, N'2')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (14, N'3')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (16, N'3')
INSERT [dbo].[UsuarioRoles] ([Id_Usuario], [RoleId]) VALUES (20, N'3')
GO
SET IDENTITY_INSERT [dbo].[Usuarios] ON 

INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (1, N'Juan', N'García', N'López', N'juan.admin@caces.com', N'12345678', N'juan.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'San José', 30, N'8888-1111', CAST(N'1994-05-10' AS Date), N'vk9oxOJiD5aPcsdU83YBvVgNVjLrvgij3NO2UQAh88I=', N'9D6EC357-90A5-4DF1-A470-8B9D3B4179A9', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (2, N'Oscar', N'López', N'Barillas', N'oscar.medico@caces.com', N'87654321', N'oscar.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'Cartago', 35, N'8888-2222', CAST(N'1989-08-20' AS Date), N'2nunQfIEgqm5rAd6Tj+JdJYsQbcdUyS3w/5F9oxe/Gk=', N'23D53917-614F-4D31-AFD2-582AF4F3E6FC', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (3, N'María', N'Hernández', N'Gómez', N'maria.paciente@caces.com', N'11223344', N'maria.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'Heredia', 28, N'8888-3333', CAST(N'1996-03-15' AS Date), N'R6GvfeUKq9IZPWHh9hvY0+1D2ywQMANwAYxuux6bYIE=', N'07EEE2E7-D00B-46B2-8FB1-A6DA84BA7784', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (4, N'Liam', N'Ramírez', N'Mora', N'liam.medico@caces.com', N'22334455', N'liam.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'Alajuela', 42, N'8888-4444', CAST(N'1984-07-12' AS Date), N'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', N'EC4376D2-B128-4AEB-A5CB-D7E7A9DAE9BB', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (5, N'Ana', N'Fernández', N'Rojas', N'ana.medico@caces.com', N'33445566', N'ana.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'Cartago', 31, N'8888-5555', CAST(N'1995-11-08' AS Date), N'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', N'CF4F6977-8691-4D6B-8BE2-B76BC16B2398', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (6, N'David', N'Jiménez', N'Vargas', N'david.paciente@caces.com', N'44556677', N'david.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'Heredia', 38, N'8888-6666', CAST(N'1987-03-21' AS Date), N'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', N'C018951B-D51A-4296-AA72-61BE16141D51', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (7, N'Juana', N'Solano', N'Castro', N'juana.paciente@caces.com', N'55667788', N'maria.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'San José', 35, N'8888-7777', CAST(N'1990-09-15' AS Date), N'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', N'4425BE66-B583-4563-94CF-82CE9566AC93', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (8, N'Carlos', N'Méndez', N'Soto', N'carlos.medico@caces.com', N'66778899', N'carlos.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'San José', 45, N'8888-8881', CAST(N'1981-02-18' AS Date), N'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', N'A126B608-8FE1-4BEF-B8EC-81D180597B5D', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (9, N'Laura', N'Vargas', N'Ramírez', N'laura.medico@caces.com', N'77889900', N'/img/DoctorDaniel.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'Heredia', 39, N'8888-8882', CAST(N'1987-10-05' AS Date), N'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', N'B52E58B0-4F6D-41D0-BF26-3B28ACD92A58', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (10, N'Andrés', N'Rojas', N'Campos', N'andres.medico@caces.com', N'88990011', N'andres.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'Alajuela', 41, N'8888-8883', CAST(N'1985-06-27' AS Date), N'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', N'F03873F6-76E8-490B-80FD-E351DDB75892', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (11, N'Gabriela', N'Castillo', N'León', N'gabriela.medico@caces.com', N'99001122', N'gabriela.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'Cartago', 36, N'8888-8884', CAST(N'1990-01-13' AS Date), N'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', N'742F4E4B-4961-4526-BCC8-B111FD5EC38B', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (12, N'Fernando', N'Pérez', N'Mora', N'fernando.medico@caces.com', N'10111223', N'fernando.jpg', CAST(N'2026-08-05T13:21:37.750' AS DateTime), NULL, 1, N'Puntarenas', 50, N'8888-8885', CAST(N'1976-09-30' AS Date), N'JcBurUY9uDRE3vIxPnJxbyof74B3VLL0n5AQVU/k0yw=', N'F1485996-13C6-4864-A1ED-965DB7DC2078', 0, NULL, 0, 0, 1)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (14, N'Angelik', N'Guatemala', N'camacho', N'angeguateca31@gmail.com', N'1111111111', N'default.jpg', CAST(N'2026-08-05T19:24:18.550' AS DateTime), NULL, 1, N'colosdhsalj', 0, N'83860838', CAST(N'2026-08-01' AS Date), N'5jMBp/iGnQ9R29NdcB6zTYrUcw1qRb3Rh1c0wV/Czng=', N'fd2f6eef-c569-4db2-8e79-f2afc5ac9db8', 0, NULL, 0, 0, 0)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (16, N'Jorge Horacio', N'Guatemala', N'Cisneros', N'guate60jo@gmail.com', N'068996223', N'default.jpg', CAST(N'2026-08-16T16:18:24.147' AS DateTime), CAST(N'2026-08-16T16:25:50.787' AS DateTime), 1, N'Residencial 3 Marías casa 6B Barva de Heredia', 0, N'86527005', CAST(N'1960-01-06' AS Date), N'5jMBp/iGnQ9R29NdcB6zTYrUcw1qRb3Rh1c0wV/Czng=', N'cb8b590c-690b-4692-b159-91bda3480408', 0, NULL, 0, 0, 0)
INSERT [dbo].[Usuarios] ([Id_Usuario], [Nombres], [PrimerApellido], [SegundoApellido], [CorreoElectronico], [DUI], [Foto], [FechaDeRegistro], [FechaDeModificacion], [Estado], [Direccion], [Edad], [Telefono], [Nacimiento], [PasswordHash], [SecurityStamp], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [EmailConfirmed]) VALUES (20, N'Emily', N'Camacho', N'Villegas', N'emiazcv@gmail.com', N'180360500', N'default.jpg', CAST(N'2026-08-17T02:41:24.497' AS DateTime), NULL, 1, N'Residencial 3 Marías casa 6B Barva de Heredia', 0, N'86586541', CAST(N'1972-09-20' AS Date), N'+kttamGAZLwCCQ/y4uiPb9urWBT5y7zEig5bYgsmdVs=', N'e4b49de1-7628-4488-820a-ff990af4aeca', 0, NULL, 0, 0, 0)
SET IDENTITY_INSERT [dbo].[Usuarios] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Configur__E8181E119C52ACAF]    Script Date: 12/09/2026 10:48:24 a. m. ******/
ALTER TABLE [dbo].[Configuracion] ADD UNIQUE NONCLUSTERED 
(
	[Clave] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Usuarios__531402F3B6F3A041]    Script Date: 12/09/2026 10:48:24 a. m. ******/
ALTER TABLE [dbo].[Usuarios] ADD UNIQUE NONCLUSTERED 
(
	[CorreoElectronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Usuarios_DUI]    Script Date: 12/09/2026 10:48:24 a. m. ******/
ALTER TABLE [dbo].[Usuarios] ADD  CONSTRAINT [UQ_Usuarios_DUI] UNIQUE NONCLUSTERED 
(
	[DUI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConfiguracionCheckpoints] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Convenios] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Convenios] ADD  DEFAULT (getdate()) FOR [FechaCreacion]
GO
ALTER TABLE [dbo].[Cotizacion] ADD  DEFAULT (getdate()) FOR [FechaSolicitud]
GO
ALTER TABLE [dbo].[Cotizacion] ADD  DEFAULT ((0)) FOR [Descuento]
GO
ALTER TABLE [dbo].[Cotizacion] ADD  DEFAULT ((0)) FOR [Impuesto]
GO
ALTER TABLE [dbo].[Cotizacion] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Cotizacion] ADD  DEFAULT (getdate()) FOR [FechaDeRegistro]
GO
ALTER TABLE [dbo].[Notificaciones] ADD  DEFAULT ((1)) FOR [CanalPlataforma]
GO
ALTER TABLE [dbo].[Notificaciones] ADD  DEFAULT ((1)) FOR [CanalEmail]
GO
ALTER TABLE [dbo].[Notificaciones] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[PreguntaSeguimiento] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Soportes] ADD  DEFAULT (getdate()) FOR [FechaConsulta]
GO
ALTER TABLE [dbo].[Soportes] ADD  DEFAULT ((1)) FOR [Estado]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((0)) FOR [TwoFactorEnabled]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((0)) FOR [LockoutEnabled]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((0)) FOR [AccessFailedCount]
GO
ALTER TABLE [dbo].[Usuarios] ADD  DEFAULT ((0)) FOR [EmailConfirmed]
GO
ALTER TABLE [dbo].[AlertaStaff]  WITH CHECK ADD  CONSTRAINT [FK_AlertaStaff_Seguimiento] FOREIGN KEY([Id_Seguimiento])
REFERENCES [dbo].[SeguimientoPaciente] ([Id_Seguimiento])
GO
ALTER TABLE [dbo].[AlertaStaff] CHECK CONSTRAINT [FK_AlertaStaff_Seguimiento]
GO
ALTER TABLE [dbo].[AlertaStaff]  WITH CHECK ADD  CONSTRAINT [FK_AlertaStaff_Usuario] FOREIGN KEY([Id_Usuario_Atendio])
REFERENCES [dbo].[Usuarios] ([Id_Usuario])
GO
ALTER TABLE [dbo].[AlertaStaff] CHECK CONSTRAINT [FK_AlertaStaff_Usuario]
GO
ALTER TABLE [dbo].[ArchivosHistorial]  WITH CHECK ADD  CONSTRAINT [FK_ArchivosHistorial_Historial] FOREIGN KEY([Id_Historial])
REFERENCES [dbo].[Historial_Medico] ([Id_Historial])
GO
ALTER TABLE [dbo].[ArchivosHistorial] CHECK CONSTRAINT [FK_ArchivosHistorial_Historial]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[Cirugias]  WITH CHECK ADD  CONSTRAINT [FK_Cirugia_Cita] FOREIGN KEY([Id_Cita])
REFERENCES [dbo].[Citas] ([Id_Cita])
GO
ALTER TABLE [dbo].[Cirugias] CHECK CONSTRAINT [FK_Cirugia_Cita]
GO
ALTER TABLE [dbo].[Cirugias]  WITH CHECK ADD  CONSTRAINT [FK_Cirugia_Fecha] FOREIGN KEY([Id_Horario])
REFERENCES [dbo].[HorariosDisponibles] ([Id_Horario])
GO
ALTER TABLE [dbo].[Cirugias] CHECK CONSTRAINT [FK_Cirugia_Fecha]
GO
ALTER TABLE [dbo].[Cirugias]  WITH CHECK ADD  CONSTRAINT [FK_Cirugias_Medico] FOREIGN KEY([Id_Medico])
REFERENCES [dbo].[Medicos] ([Id_Medico])
GO
ALTER TABLE [dbo].[Cirugias] CHECK CONSTRAINT [FK_Cirugias_Medico]
GO
ALTER TABLE [dbo].[Cirugias]  WITH CHECK ADD  CONSTRAINT [FK_Cirugias_Paciente] FOREIGN KEY([Id_Paciente])
REFERENCES [dbo].[Pacientes] ([Id_Paciente])
GO
ALTER TABLE [dbo].[Cirugias] CHECK CONSTRAINT [FK_Cirugias_Paciente]
GO
ALTER TABLE [dbo].[Cirugias]  WITH CHECK ADD  CONSTRAINT [FK_Cirugias_Procedimiento] FOREIGN KEY([Id_Procedimiento])
REFERENCES [dbo].[Procedimiento] ([Id_Procedimiento])
GO
ALTER TABLE [dbo].[Cirugias] CHECK CONSTRAINT [FK_Cirugias_Procedimiento]
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD  CONSTRAINT [FK_Citas_Especialidad] FOREIGN KEY([Id_Especialidad])
REFERENCES [dbo].[Especialidad] ([Id_Especialidad])
GO
ALTER TABLE [dbo].[Citas] CHECK CONSTRAINT [FK_Citas_Especialidad]
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD  CONSTRAINT [FK_Citas_Horario] FOREIGN KEY([Id_Horario])
REFERENCES [dbo].[HorariosDisponibles] ([Id_Horario])
GO
ALTER TABLE [dbo].[Citas] CHECK CONSTRAINT [FK_Citas_Horario]
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD  CONSTRAINT [FK_Citas_Medicos] FOREIGN KEY([Id_Medico])
REFERENCES [dbo].[Medicos] ([Id_Medico])
GO
ALTER TABLE [dbo].[Citas] CHECK CONSTRAINT [FK_Citas_Medicos]
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD  CONSTRAINT [FK_Citas_Pacientes] FOREIGN KEY([Id_Paciente])
REFERENCES [dbo].[Pacientes] ([Id_Paciente])
GO
ALTER TABLE [dbo].[Citas] CHECK CONSTRAINT [FK_Citas_Pacientes]
GO
ALTER TABLE [dbo].[Citas]  WITH CHECK ADD  CONSTRAINT [FK_Citas_Procedimiento] FOREIGN KEY([Id_Procedimiento])
REFERENCES [dbo].[Procedimiento] ([Id_Procedimiento])
GO
ALTER TABLE [dbo].[Citas] CHECK CONSTRAINT [FK_Citas_Procedimiento]
GO
ALTER TABLE [dbo].[Cotizacion]  WITH CHECK ADD  CONSTRAINT [FK_Cotizacion_Medico] FOREIGN KEY([Id_Medico])
REFERENCES [dbo].[Medicos] ([Id_Medico])
GO
ALTER TABLE [dbo].[Cotizacion] CHECK CONSTRAINT [FK_Cotizacion_Medico]
GO
ALTER TABLE [dbo].[Cotizacion]  WITH CHECK ADD  CONSTRAINT [FK_Cotizacion_Paciente] FOREIGN KEY([Id_Paciente])
REFERENCES [dbo].[Pacientes] ([Id_Paciente])
GO
ALTER TABLE [dbo].[Cotizacion] CHECK CONSTRAINT [FK_Cotizacion_Paciente]
GO
ALTER TABLE [dbo].[Cotizacion]  WITH CHECK ADD  CONSTRAINT [FK_Cotizacion_Procedimiento] FOREIGN KEY([Id_Procedimiento])
REFERENCES [dbo].[Procedimiento] ([Id_Procedimiento])
GO
ALTER TABLE [dbo].[Cotizacion] CHECK CONSTRAINT [FK_Cotizacion_Procedimiento]
GO
ALTER TABLE [dbo].[Especialidad]  WITH CHECK ADD  CONSTRAINT [FK_Especialidad_Icono] FOREIGN KEY([Id_Icono])
REFERENCES [dbo].[Icono] ([Id_Icono])
GO
ALTER TABLE [dbo].[Especialidad] CHECK CONSTRAINT [FK_Especialidad_Icono]
GO
ALTER TABLE [dbo].[HorariosDisponibles]  WITH CHECK ADD  CONSTRAINT [FK_Horarios_Medico] FOREIGN KEY([Id_Medico])
REFERENCES [dbo].[Medicos] ([Id_Medico])
GO
ALTER TABLE [dbo].[HorariosDisponibles] CHECK CONSTRAINT [FK_Horarios_Medico]
GO
ALTER TABLE [dbo].[Medicos]  WITH CHECK ADD  CONSTRAINT [FK_Medico_Usuario] FOREIGN KEY([Id_Usuario])
REFERENCES [dbo].[Usuarios] ([Id_Usuario])
GO
ALTER TABLE [dbo].[Medicos] CHECK CONSTRAINT [FK_Medico_Usuario]
GO
ALTER TABLE [dbo].[Medicos]  WITH CHECK ADD  CONSTRAINT [FK_Medicos_Especialidad] FOREIGN KEY([Id_Especialidad])
REFERENCES [dbo].[Especialidad] ([Id_Especialidad])
GO
ALTER TABLE [dbo].[Medicos] CHECK CONSTRAINT [FK_Medicos_Especialidad]
GO
ALTER TABLE [dbo].[NotificacionUsuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Notificacion] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuarios] ([Id_Usuario])
GO
ALTER TABLE [dbo].[NotificacionUsuario] CHECK CONSTRAINT [FK_Usuario_Notificacion]
GO
ALTER TABLE [dbo].[Pacientes]  WITH CHECK ADD  CONSTRAINT [FK_Paciente_Usuario] FOREIGN KEY([Id_Usuario])
REFERENCES [dbo].[Usuarios] ([Id_Usuario])
GO
ALTER TABLE [dbo].[Pacientes] CHECK CONSTRAINT [FK_Paciente_Usuario]
GO
ALTER TABLE [dbo].[Pacientes]  WITH CHECK ADD  CONSTRAINT [FK_Pacientes_Historial] FOREIGN KEY([Id_Historial])
REFERENCES [dbo].[Historial_Medico] ([Id_Historial])
GO
ALTER TABLE [dbo].[Pacientes] CHECK CONSTRAINT [FK_Pacientes_Historial]
GO
ALTER TABLE [dbo].[Precios]  WITH CHECK ADD  CONSTRAINT [FK_Precios_Medico] FOREIGN KEY([Id_Medico])
REFERENCES [dbo].[Medicos] ([Id_Medico])
GO
ALTER TABLE [dbo].[Precios] CHECK CONSTRAINT [FK_Precios_Medico]
GO
ALTER TABLE [dbo].[Precios]  WITH CHECK ADD  CONSTRAINT [FK_Precios_Procedimiento] FOREIGN KEY([Id_Procedimiento])
REFERENCES [dbo].[Procedimiento] ([Id_Procedimiento])
GO
ALTER TABLE [dbo].[Precios] CHECK CONSTRAINT [FK_Precios_Procedimiento]
GO
ALTER TABLE [dbo].[Procedimiento]  WITH CHECK ADD  CONSTRAINT [FK_Procedimiento_Especialidad] FOREIGN KEY([Id_Especialidad])
REFERENCES [dbo].[Especialidad] ([Id_Especialidad])
GO
ALTER TABLE [dbo].[Procedimiento] CHECK CONSTRAINT [FK_Procedimiento_Especialidad]
GO
ALTER TABLE [dbo].[Recetas]  WITH CHECK ADD  CONSTRAINT [FK_Recetas_Cita] FOREIGN KEY([Id_Cita])
REFERENCES [dbo].[Citas] ([Id_Cita])
GO
ALTER TABLE [dbo].[Recetas] CHECK CONSTRAINT [FK_Recetas_Cita]
GO
ALTER TABLE [dbo].[RespuestaSeguimiento]  WITH CHECK ADD  CONSTRAINT [FK_RespuestaSeguimiento_Pregunta] FOREIGN KEY([Id_Pregunta])
REFERENCES [dbo].[PreguntaSeguimiento] ([Id_Pregunta])
GO
ALTER TABLE [dbo].[RespuestaSeguimiento] CHECK CONSTRAINT [FK_RespuestaSeguimiento_Pregunta]
GO
ALTER TABLE [dbo].[RespuestaSeguimiento]  WITH CHECK ADD  CONSTRAINT [FK_RespuestaSeguimiento_Seguimiento] FOREIGN KEY([Id_Seguimiento])
REFERENCES [dbo].[SeguimientoPaciente] ([Id_Seguimiento])
GO
ALTER TABLE [dbo].[RespuestaSeguimiento] CHECK CONSTRAINT [FK_RespuestaSeguimiento_Seguimiento]
GO
ALTER TABLE [dbo].[SeguimientoPaciente]  WITH CHECK ADD  CONSTRAINT [FK_SeguimientoPaciente_Cirugia] FOREIGN KEY([Id_Cirugia])
REFERENCES [dbo].[Cirugias] ([Id_Cirugia])
GO
ALTER TABLE [dbo].[SeguimientoPaciente] CHECK CONSTRAINT [FK_SeguimientoPaciente_Cirugia]
GO
ALTER TABLE [dbo].[Soportes]  WITH CHECK ADD  CONSTRAINT [FK_Soportes_Usuarios] FOREIGN KEY([Id_Usuario])
REFERENCES [dbo].[Usuarios] ([Id_Usuario])
GO
ALTER TABLE [dbo].[Soportes] CHECK CONSTRAINT [FK_Soportes_Usuarios]
GO
ALTER TABLE [dbo].[UsuarioRoles]  WITH CHECK ADD  CONSTRAINT [FK_UsuarioRoles_Usuarios] FOREIGN KEY([Id_Usuario])
REFERENCES [dbo].[Usuarios] ([Id_Usuario])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UsuarioRoles] CHECK CONSTRAINT [FK_UsuarioRoles_Usuarios]
GO
USE [master]
GO
ALTER DATABASE [CACES] SET  READ_WRITE 
GO
