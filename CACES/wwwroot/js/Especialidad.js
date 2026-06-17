(() => {

    const Especialidad = {

        init() {
            this.cargarEspecialidades();
            this.registrarEventos();
        },

        cargarEspecialidades() {

            $.ajax({

                url: '/Especialidad/ObtenerEspecialidades',
                type: 'GET',

                success: function (respuesta) {

                    let html = '';

                    respuesta.dato.forEach(e => {

                        html += `
            <div class="col-12 col-md-6 col-lg-4">

                <article class="card h-100 border-0 shadow-sm specialty-card">

                    <div class="card-body d-flex flex-column">

                        <div class="mb-3">
                            <i class="${e.icono} info-icon"></i>
                        </div>

                        <h4 class="card-title mb-2">
                            ${e.nombre}
                        </h4>

                        <p class="text-muted mb-3 flex-grow-1">
                            ${e.descripcion}
                        </p>

                        <a href="/Cita/GestionCitas" class="btn btn-info w-100 mt-auto">
                            Reservar cita
                        </a>

                    </div>

                </article>

            </div>
        `;
                    });

                    $('#contenedorEspecialidades').html(html);
                },

                error: function () {

                    Swal.fire({
                        title: 'Error',
                        text: 'No fue posible cargar las especialidades.',
                        icon: 'error'
                    });
                }
            });
        },

        registrarEventos() {

            $(document).on('submit', '#formEspecialidad', function (e) {

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
                            });

                            $('#formEspecialidad')[0].reset();
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
                            text: 'Ocurrió un error inesperado.',
                            icon: 'error'
                        });
                    }
                });

            });
        }
    };

    $(function () {
        Especialidad.init();
    });

})();