document.addEventListener("DOMContentLoaded", function () {
    cargarDatosProcedimiento();
});

async function cargarDatosProcedimiento() {
    const id = document.getElementById("txtIdProcedimiento").value;

    try {
        const response = await fetch(`/Procedimientos/EditarProcEnReportes/${id}`, {
            method: 'GET',
            cache: 'no-store',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Cache-Control': 'no-cache, no-store, must-revalidate',
                'Pragma': 'no-cache'
            }
        });

        if (!response.ok) throw new Error("Error al obtener los datos");
        const data = await response.json();

        if (data.exito || data.esCorrecto) {
            const procedimiento = data.procedimiento || data.dato;
            const especialidades = data.especialidades || [];

            // Poblar campos de texto e inputs
            document.getElementById("txtNombre").value = procedimiento.nombre || '';
            document.getElementById("txtDescripcion").value = procedimiento.descripcion || '';
            document.getElementById("txtPrecioBase").value = procedimiento.precioBase || 0;
            document.getElementById("chkEstado").checked = procedimiento.estado ?? procedimiento.activo ?? true;

            // Poblar el combo de Especialidades
            const selectEspecialidad = document.getElementById("selectEspecialidad");
            selectEspecialidad.innerHTML = '<option value="">-- Seleccione una especialidad --</option>';

            especialidades.forEach(esp => {
                const idEsp = esp.id_Especialidad || esp.idEspecialidad || esp.id;
                const nombreEsp = esp.nombreEspecialidad || esp.nombre;
                const selected = idEsp === (procedimiento.id_Especialidad || procedimiento.idEspecialidad) ? 'selected' : '';

                selectEspecialidad.innerHTML += `<option value="${idEsp}" ${selected}>${nombreEsp}</option>`;
            });

        } else {
            Swal.fire({
                title: 'Error',
                text: data.mensaje || 'No se pudieron cargar los datos del procedimiento.',
                icon: 'error',
                confirmButtonColor: '#163B73'
            });
        }

    } catch (error) {
        console.error("Error:", error);
        Swal.fire({
            title: 'Error',
            text: 'No se pudieron cargar los datos del procedimiento.',
            icon: 'error',
            confirmButtonColor: '#163B73'
        });
    }
}

function guardarEdicionProcedimiento(event) {
    event.preventDefault();

    const id = document.getElementById("txtIdProcedimiento").value;
    const btnGuardar = document.getElementById("btnGuardar");
    btnGuardar.disabled = true;
    btnGuardar.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span> Actualizando...';

    const dto = {
        id_Procedimiento: parseInt(id),
        nombre: document.getElementById("txtNombre").value,
        descripcion: document.getElementById("txtDescripcion").value,
        precioBase: parseFloat(document.getElementById("txtPrecioBase").value) || 0,
        id_Especialidad: parseInt(document.getElementById("selectEspecialidad").value) || 0,
        estado: document.getElementById("chkEstado").checked
    };

    const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value || '';

    fetch('/Procedimientos/EditarProcEnReportes', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest',
            'RequestVerificationToken': token
        },
        body: JSON.stringify(dto)
    })
        .then(response => response.json())
        .then(data => {
            if (data.exito || data.esCorrecto) {
                Swal.fire({
                    title: '¡Actualizado!',
                    text: data.mensaje || 'El procedimiento fue actualizado exitosamente.',
                    icon: 'success',
                    confirmButtonColor: '#20C4D8'
                }).then(() => {
                    window.location.href = '/Procedimientos/ObtenerProcedimientosQuirur';
                });
            } else {
                let mensajeError = data.mensaje || "Error al actualizar el procedimiento.";
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
                btnGuardar.innerHTML = '<i class="bi bi-pencil me-1"></i> Actualizar Procedimiento';
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
            btnGuardar.innerHTML = '<i class="bi bi-pencil me-1"></i> Actualizar Procedimiento';
        });
}