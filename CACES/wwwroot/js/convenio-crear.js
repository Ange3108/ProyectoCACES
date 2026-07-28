function guardarNuevoConvenio(event) {
    event.preventDefault();

    const btnGuardar = document.getElementById("btnGuardar");
    btnGuardar.disabled = true;
    btnGuardar.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> Guardando...';

    const dto = {
        nombre: document.getElementById("txtNombre").value,
        descripcion: document.getElementById("txtDescripcion").value,
        DescuentoPorcentaje: parseFloat(document.getElementById("txtDescuentoPorcentaje").value),
        ContactoTelefono: document.getElementById("txtTelefono").value,
        imagenUrl: document.getElementById("txtImagenUrl").value,
        estado: true // Por defecto activo al crear
    };

    fetch('/Convenio/CrearConvenio', {
        method: 'POST',
        cache: 'no-store',
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest',
            'Cache-Control': 'no-cache, no-store, must-revalidate',
            'Pragma': 'no-cache'
        },
        body: JSON.stringify(dto)
    })
        .then(response => response.json())
        .then(data => {
            if (data.esCorrecto) {
                Swal.fire({
                    title: '¡Éxito!',
                    text: data.mensaje,
                    icon: 'success',
                    confirmButtonColor: '#20C4D8'
                }).then(() => {
                    window.location.href = '/Convenio/ObtenerConvenios';
                });
            } else {
                Swal.fire({
                    title: 'Atención',
                    text: data.mensaje || "Error al guardar el convenio.",
                    icon: 'warning',
                    confirmButtonColor: '#163B73'
                });
                btnGuardar.disabled = false;
                btnGuardar.innerHTML = '<i class="bi bi-save me-1"></i> Guardar Convenio';
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
            btnGuardar.innerHTML = '<i class="bi bi-save me-1"></i> Guardar Convenio';
        });
}