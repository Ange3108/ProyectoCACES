// ACCIÓN 3: Eliminar cuenta propia desde configuración
$("#btnEliminarCuenta").click(function () {
    var urlEliminar = $(this).data("url");
    var urlRedireccion = $(this).data("redirect");

    Swal.fire({
        title: '¿Estás completamente seguro?',
        text: "Esta acción es irreversible. Se perderán todos tus datos médicos, citas y registros en CACES de forma permanente.",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#dc3545', // Rojo peligro
        cancelButtonColor: '#6c757d',  // Gris
        confirmButtonText: 'Sí, eliminar permanentemente',
        cancelButtonText: 'Cancelar',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {

            $.ajax({
                url: urlEliminar,
                type: 'POST',
                success: function (res) {
                    if (res.esCorrecto) {
                        Swal.fire({
                            title: 'Cuenta Eliminada',
                            text: res.mensaje,
                            icon: 'success',
                            confirmButtonText: 'Entendido'
                        }).then(() => {
                            // Redirecciona directo al Login
                            window.location.href = urlRedireccion;
                        });
                    } else {
                        Swal.fire('Error', res.mensaje, 'error');
                    }
                },
                error: function () {
                    Swal.fire('Error', 'No se pudo procesar la solicitud en este momento.', 'error');
                }
            });

        }
    });
});