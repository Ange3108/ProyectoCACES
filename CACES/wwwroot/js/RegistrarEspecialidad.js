function formatearIcono(opcion) {
    if (!opcion.id) return opcion.text;
    const codigo = $(opcion.element).data('codigo');
    return $(`<span><i class="${codigo}" style="margin-right: 8px;"></i>${opcion.text}</span>`);
}

(() => {

    const RegistroEspecialidad = {

        init() {
            this.cargarIconos();
            this.registrarEventos();
        },

        cargarIconos() {

            $.ajax({
                url: '/Icono/ObtenerIconos',
                type: 'GET',
                success: function (respuesta) {

                    let select = $('#Icono');

                    select.empty();
                    select.append('<option value="">Seleccione un icono</option>');

                    (respuesta.dato || []).forEach(c => {
                        select.append(`
                            <option value="${c.idIcono}" data-codigo="${c.codigo}">
                                ${c.nombre}
                            </option>
                        `);
                    });

                    select.select2({
                        width: '100%',
                        templateResult: formatearIcono,
                        templateSelection: formatearIcono
                    });
                },

                error: function () {
                    Swal.fire({
                        title: 'Error',
                        text: 'No fue posible cargar los íconos.',
                        icon: 'error'
                    });
                }
            });
        },

        registrarEventos() {

            $('#formEspecialidad').on('submit', function (e) {

                e.preventDefault();

                $.ajax({
                    url: $(this).attr('action'),
                    type: 'POST',
                    data: $(this).serialize(),

                    success: function (respuesta) {

                        if (respuesta.esCorrecto) {

                            Swal.fire({
                                title: 'Correcto',
                                text: respuesta.mensaje,
                                icon: 'success'
                            }).then(() => {
                                window.location.href = '/Especialidad/ListadoEspecialidad';
                            });
                        }
                        else {

                            Swal.fire({
                                title: 'Error',
                                text: respuesta.mensaje,
                                icon: 'error'
                            });
                        }
                    },

                    error: function () {

                        Swal.fire({
                            title: 'Error',
                            text: 'Ocurrió un error al registrar la especialidad.',
                            icon: 'error'
                        });
                    }
                });
            });
        }
    };

    $(function () {
        RegistroEspecialidad.init();
    });

})();