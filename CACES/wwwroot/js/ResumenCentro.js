(() => {

    const ResumenCentro = {

        intervalo: null,

        init() {
            this.cargarResumen();
            // Actualiza automáticamente cada 60 segundos
            this.intervalo = setInterval(() => this.cargarResumen(), 60000);
        },

        cargarResumen() {

            $('#resumenLoader').show();

            $.ajax({
                url: '/Home/ResumenCentro',
                type: 'GET',

                success: function (data) {

                    // Pills de la barra
                    $('#numCirugias').text(data.cirugias);
                    $('#numCitas').text(data.citas);
                    $('#numPacientes').text(data.pacientes);

                    // Tarjetas expandidas
                    $('#statCirugias').text(data.cirugias);
                    $('#statCitas').text(data.citas);
                    $('#statPacientes').text(data.pacientes);
                    $('#statMedicos').text(data.medicos);

                    const fecha = new Date(data.timestamp);
                    const hora = fecha.toLocaleTimeString('es-ES', { hour: '2-digit', minute: '2-digit' });

                    $('#resumenHora').text(`Actualizado ${hora}`);
                    $('#resumenUpdateTime').text(`Última actualización: ${hora}`);
                },

                error: function () {
                    $('#resumenHora').text('Error al actualizar');
                },

                complete: function () {
                    $('#resumenLoader').hide();
                }
            });
        }
    };

    // Función global porque se llama desde el onclick del HTML
    window.toggleResumen = function () {

        const expanded = document.getElementById('resumenExpanded');
        const chevron = document.getElementById('resumenChevron');

        expanded.classList.toggle('show');
        chevron.style.transform = expanded.classList.contains('show')
            ? 'rotate(180deg)'
            : 'rotate(0deg)';
    };

    $(function () {
        ResumenCentro.init();
    });

})();