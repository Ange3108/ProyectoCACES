document.addEventListener("DOMContentLoaded", function () {
    cargarDatosConvenio();
});

async function cargarDatosConvenio() {
    const id = document.getElementById("txtIdConvenio").value;

    try {
        const response = await fetch(`/Convenio/ActualizarConvenio/${id}`, {
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

        document.getElementById("txtNombre").value = data.nombre || '';
        document.getElementById("txtDescripcion").value = data.descripcion || '';
        document.getElementById("txtDescuentoPorcentaje").value = data.descuentoPorcentaje || data.DescuentoPorcentaje || 0;

        document.getElementById("txtTelefono").value = data.contactoTelefono || data.ContactoTelefono || data.telefonoContacto || data.telefono || '';

        document.getElementById("txtImagenUrl").value = data.imagenUrl || '';
        document.getElementById("chkEstado").checked = data.estado || data.activo;

    } catch (error) {
        console.error("Error:", error);
        Swal.fire({
            title: 'Error',
            text: 'No se pudieron cargar los datos del convenio.',
            icon: 'error',
            confirmButtonColor: '#163B73'
        });
    }
}

function guardarEdicionConvenio(event) {
    event.preventDefault();

    const id = document.getElementById("txtIdConvenio").value;
    const btnGuardar = document.getElementById("btnGuardar");
    btnGuardar.disabled = true;
    btnGuardar.innerHTML = '<span class="spinner-border spinner-border-sm" role="status"></span> Actualizando...';

    const dto = {
        idConvenio: parseInt(id),
        nombre: document.getElementById("txtNombre").value,
        descripcion: document.getElementById("txtDescripcion").value,
        DescuentoPorcentaje: parseFloat(document.getElementById("txtDescuentoPorcentaje").value),
        ContactoTelefono: document.getElementById("txtTelefono").value,
        imagenUrl: document.getElementById("txtImagenUrl").value,
        estado: document.getElementById("chkEstado").checked
    };

    fetch(`/Convenio/ActualizarConvenio/${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: JSON.stringify(dto)
    })
        .then(response => response.json())
        .then(data => {
            if (data.esCorrecto) {
                Swal.fire({
                    title: '¡Actualizado!',
                    text: data.mensaje,
                    icon: 'success',
                    confirmButtonColor: '#20C4D8'
                }).then(() => {
                    window.location.href = '/Convenio/ObtenerConvenios';
                });
            } else {
                Swal.fire({
                    title: 'Atención',
                    text: data.mensaje || "Error al actualizar el convenio.",
                    icon: 'warning',
                    confirmButtonColor: '#163B73'
                });
                btnGuardar.disabled = false;
                btnGuardar.innerHTML = '<i class="bi bi-pencil me-1"></i> Actualizar Convenio';
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
            btnGuardar.innerHTML = '<i class="bi bi-pencil me-1"></i> Actualizar Convenio';
        });
}