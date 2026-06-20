(() => {

    const Especialidad = {

        init() {
            this.cargarEspecialidades();

        },

        cargarEspecialidades() {

            $.ajax({

                url: '/Especialidad/ObtenerEspecialidades',
                type: 'GET',

                success: function (respuesta) {

                    let html = '';

                    respuesta.dato.forEach((e, index) => {

                        const numero = String(index + 1).padStart(2, '0');

                        html += `
            <a href="/Especialidad/Detalles?id=${e.idEspecialidad}" class="directory-entry">

                <span class="directory-number">${numero}</span>

                <span class="directory-icon">
                    <i class="${e.icono}"></i>
                </span>

                <span class="directory-entry-body">
                    <h3>${e.nombre}</h3>
                    <p>${e.descripcion}</p>
                </span>

                <span class="directory-entry-link">
                    Ver detalle
                    <i class="bi bi-arrow-right directory-entry-arrow"></i>
                </span>

            </a>
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

       
    };

    $(function () {
        Especialidad.init();
    });

})();
