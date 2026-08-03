function guardarNuevoProcedimiento(event) {
    event.preventDefault();

    const btnGuardar = document.getElementById("btnGuardar");
    btnGuardar.disabled = true;
    btnGuardar.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Guardando...';

    // Construcción del DTO según la estructura de InsertarProcedimientosDto
    const dto = {
        nombre: document.getElementById("txtNombre").value,
        descripcion: document.getElementById("txtDescripcion").value,
        precioBase: parseFloat(document.getElementById("txtPrecioBase").value) || 0,
        id_Especialidad: parseInt(document.getElementById("selectEspecialidad").value) || 0,
        estado: true
    };

    // Validación previa básica en cliente
    if (!dto.nombre || dto.id_Especialidad <= 0 || dto.precioBase <= 0) {
        Swal.fire({
            title: 'Campos Incompletos',
            text: 'Por favor, ingrese el nombre, seleccione una especialidad y especifique un precio válido.',
            icon: 'warning',
            confirmButtonColor: '#163B73'
        });
        btnGuardar.disabled = false;
        btnGuardar.innerHTML = '<i class="bi bi-save me-1"></i> Guardar Procedimiento';
        return;
    }

    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value || '';

    fetch('/Procedimientos/Crear', {
        method: 'POST',
        cache: 'no-store',
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': token,
            'Cache-Control': 'no-cache, no-store, must-revalidate',
            'Pragma': 'no-cache'
        },
        body: JSON.stringify(dto)
    })
        .then(response => response.json())
        .then(data => {
            if (data.exito || data.esCorrecto) {
                Swal.fire({
                    title: '¡Éxito!',
                    text: data.mensaje || 'Procedimiento registrado exitosamente.',
                    icon: 'success',
                    confirmButtonColor: '#20C4D8'
                }).then(() => {
                    window.location.href = '/Procedimientos/ObtenerProcedimientosQuirur';
                });
            } else {
                let mensajeError = data.mensaje || 'Error al guardar el procedimiento.';
                if (data.errores && data.errores.length > 0) {
                    mensajeError = data.errores.join('<br>');
                }

                Swal.fire({
                    title: 'Atención',
                    html: mensajeError,
                    icon: 'warning',
                    confirmButtonColor: '#163B73'
                });
                btnGuardar.disabled = false;
                btnGuardar.innerHTML = '<i class="bi bi-save me-1"></i> Guardar Procedimiento';
            }
        })
        .catch(error => {
            console.error("Error:", error);
            Swal.fire({
                title: 'Error de Conexión',
                text: 'No se pudo conectar con el servidor.',
                icon: 'error',
                confirmButtonColor: '#163B73'
            });
            btnGuardar.disabled = false;
            btnGuardar.innerHTML = '<i class="bi bi-save me-1"></i> Guardar Procedimiento';
        });
}