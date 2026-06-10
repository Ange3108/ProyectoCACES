$(document).ready(function () {

    // ACCIÓN 1: Al verificar el correo
    $("#btnVerificar").click(function () {
        var correoInput = $("#txtCorreo").val();
        $("#errorCorreo").text("");

        if (!correoInput) {
            $("#errorCorreo").text("Por favor, escribe tu correo electrónico.");
            return;
        }

        // Leemos la URL asignada en el botón desde el HTML
        var urlVerificar = $("#btnVerificar").data("url");

        $.ajax({
            url: urlVerificar,
            type: 'POST',
            data: { correo: correoInput },
            success: function (res) {
                if (res.exito) {
                    // Se guardan los datos devueltos en los inputs ocultos del DTO
                    $("#hdnToken").val(res.token);
                    $("#hdnCorreo").val(res.correo);

                    // Ocultamos la sección de correo y mostramos la de la nueva clave
                    $("#seccionCorreo").fadeOut(300, function () {
                        $("#seccionClave").fadeIn(300);
                    });
                } else {
                    Swal.fire('Error', res.mensaje, 'error');
                }
            },
            error: function () {
                Swal.fire('Error', 'Hubo un problema de comunicación con el servidor.', 'error');
            }
        });
    });

    // ACCIÓN 2: Al guardar la nueva contraseña
    $("#btnRestablecer").click(function () {
        var nuevaClave = $("#txtNuevaClave").val();
        $("#errorClave").text("");

        if (!nuevaClave || nuevaClave.length < 6) {
            $("#errorClave").text("La contraseña debe tener al menos 6 caracteres.");
            return;
        }

        // Se arma el objeto exactamente igual al RestablecerContrasenaDTO
        var dataDTO = {
            CorreoElectronico: $("#hdnCorreo").val(),
            Token: $("#hdnToken").val(),
            NuevaContrasena: nuevaClave
        };

        // Leemos las URLs asignadas en el botón desde el HTML
        var urlRestablecer = $("#btnRestablecer").data("url");
        var urlRedireccion = $("#btnRestablecer").data("redirect");

        $.ajax({
            url: urlRestablecer,
            type: 'POST',
            data: dataDTO,
            success: function (res) {
                if (res.exito) {
                    Swal.fire({
                        title: '¡Éxito!',
                        text: res.mensaje,
                        icon: 'success',
                        confirmButtonText: 'Ir al Login'
                    }).then((result) => {
                        if (result.isConfirmed) {
                            // Redirección dinámica basada en la ruta del botón
                            window.location.href = urlRedireccion;
                        }
                    });
                } else {
                    Swal.fire('Error', res.mensaje, 'error');
                }
            },
            error: function () {
                Swal.fire('Error', 'No se pudo actualizar la contraseña. Intente más tarde.', 'error');
            }
        });
    });
});