function resolverFoto(foto) {
    if (!foto || typeof foto !== 'string' || foto.trim() === '') return null;
    const fotoLimpia = foto.trim();
    if (fotoLimpia.startsWith('http')) return fotoLimpia;
    return '/img/' + fotoLimpia;
}

function formatearFecha(fechaIso) {
    if (!fechaIso) return 'N/D';
    // Previene el desfase de zona horaria parseando la fecha limpia en AAAA-MM-DD
    const partes = fechaIso.split('T')[0].split('-');
    if (partes.length === 3) {
        return `${partes[2]}/${partes[1]}/${partes[0]}`;
    }
    const fecha = new Date(fechaIso);
    return fecha.toLocaleDateString('es-ES', { day: '2-digit', month: '2-digit', year: 'numeric' });
}

function cargarPerfilUsuario() {
    $.ajax({
        url: '/Perfil/VerPerfil',
        type: 'GET',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        dataType: 'json',
        success: function (response) {
            console.log("Respuesta recibida:", response);

            // Acepta tanto si el backend responde con response.dato como si responde directamente con el DTO
            const d = response.dato ?? response.Dato ?? response;

            if (!d) {
                console.error("No se encontraron datos en la respuesta");
                return;
            }

            // Extraer variables soportando camelCase y PascalCase
            const idUsuario = d.idUsuario ?? d.IdUsuario;
            const nombres = d.nombres ?? d.Nombres ?? '';
            const primerApellido = d.primerApellido ?? d.PrimerApellido ?? '';
            const segundoApellido = d.segundoApellido ?? d.SegundoApellido ?? '';
            const correoElectronico = d.correoElectronico ?? d.CorreoElectronico ?? 'N/D';
            const dui = d.dui ?? d.DUI ?? d.Dui ?? '---';
            const telefono = d.telefono ?? d.Telefono ?? '---';
            const estado = d.estado ?? d.Estado ?? false;
            const nacimiento = d.nacimiento ?? d.Nacimiento;
            const direccion = d.direccion ?? d.Direccion ?? '---';
            const tipoSangre = d.tipoSangre ?? d.TipoSangre ?? 'N/D';
            const alergias = d.alergias ?? d.Alergias ?? 'Ninguna registrada';
            const enfermedadesCronicas = d.enfermedadesCronicas ?? d.EnfermedadesCronicas ?? 'Ninguna registrada';
            const medicamentosActuales = d.medicamentosActuales ?? d.MedicamentosActuales ?? 'Ninguno registrado';
            const idHistorial = d.idHistorial ?? d.IdHistorial;
            const foto = d.foto ?? d.Foto;

            // 1. Estado (Badge)
            if (estado) {
                $('#badgeEstado').removeClass('bg-danger-subtle text-danger').addClass('bg-success-subtle text-success');
                $('#lblEstado').text('Cuenta Activa');
            } else {
                $('#badgeEstado').removeClass('bg-success-subtle text-success').addClass('bg-danger-subtle text-danger');
                $('#lblEstado').text('Cuenta Inactiva');
            }

            // 2. Cabecera
            const nombreCompleto = `${nombres} ${primerApellido} ${segundoApellido}`.trim();
            $('#lblNombreCabecera').text(nombreCompleto || '---');
            $('#lblCorreo').text(correoElectronico);

            // 3. Avatar
            const fotoUrl = resolverFoto(foto);
            if (fotoUrl) {
                $('#imgAvatar').attr('src', fotoUrl).show();
                $('#avatar-iniciales').hide();
            } else {
                const iniciales = `${nombres.charAt(0)}${primerApellido.charAt(0)}`.toUpperCase();
                $('#imgAvatar').hide();
                $('#avatar-iniciales').text(iniciales || 'U').css('display', 'flex');
            }

            // 4. Información Personal
            $('#lblNombres').text(nombres || '---');
            $('#lblApellidos').text(`${primerApellido} ${segundoApellido}`.trim() || '---');
            $('#lblDUI').text(dui);
            $('#lblTelefono').text(telefono);
            $('#lblDireccion').text(direccion);
            $('#lblNacimiento').text(formatearFecha(nacimiento));

            // 5. Enlaces
            if (idUsuario) {
                $('#btnEditarPerfil').attr('href', `/Perfil/ActualizarPerfilObt?id=${idUsuario}`);
            }
            if (idHistorial) {
                $('#btnHistorial').attr('href', `/HistorialMedico/Detalle?id=${idHistorial}`);
            }

            // 6. Ficha Médica
            $('#lblTipoSangre').text(tipoSangre);
            $('#lblAlergias').text(alergias);
            $('#lblEnfermedadesCronicas').text(enfermedadesCronicas);
            $('#lblMedicamentosActuales').text(medicamentosActuales);
        },
        error: function (xhr) {
            console.error("Error al cargar perfil:", xhr.responseText);
            if (typeof Swal !== 'undefined') {
                Swal.fire('Error', 'No se pudieron cargar los datos del perfil', 'error');
            }
        }
    });
}

$(function () {
    cargarPerfilUsuario();
});