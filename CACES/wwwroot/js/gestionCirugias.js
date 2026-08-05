const GestionCirugias = {

    init() {
        if ($('#tbGestionCirugias').length) {
            this.cargarTabla();
        }
    },

    cargarTabla() {
        $('#tbGestionCirugias').DataTable({
            ajax: {
                url: '/Cirugia/ObtenerListadoCirugias',
                type: 'GET',
                dataSrc: function (res) {
                    return (res && res.esCorrecto && Array.isArray(res.dato)) ? res.dato : [];
                },
                error: function (xhr) {
                    console.error('Error al cargar cirugías:', xhr.responseText);
                }
            },
            language: {
                url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
            },
            columns: [
                { data: 'idCirugia', className: 'text-center fw-bold' },
                { data: 'nombrePaciente' },
                { data: 'procedimiento' },
                { data: 'medicoResponsable' },
                {
                    data: 'fechaProcedimiento',
                    render: data => {
                        if (!data) return '';
                        const f = new Date(data);
                        return f.toLocaleDateString('es-ES', { year: 'numeric', month: '2-digit', day: '2-digit' });
                    }
                },
                {
                    data: 'horaProcedimiento',
                    render: h => {
                        if (!h) return '--:--';
                        if (typeof h === 'string') return h.substring(0, 5);
                        return String(h);
                    }
                },
                {
                    data: 'estado',
                    className: 'text-center',
                    render: estado => {
                        const badges = {
                            0: '<span class="badge bg-danger-subtle text-danger border border-danger px-3 py-2">Cancelada</span>',
                            1: '<span class="badge bg-warning-subtle text-warning border border-warning px-3 py-2">Pendiente</span>',
                            2: '<span class="badge bg-success-subtle text-success border border-success px-3 py-2">Finalizada</span>'
                        };
                        return badges[estado] ?? estado;
                    }
                },
                {
                    data: null,
                    orderable: false,
                    searchable: false,
                    className: 'text-center',
                    render: (data, type, row) => {
                        const id = row.idCirugia;
                        const esPendiente = row.estado === 1;

                        let botones = `
                            <button class="btn btn-sm btn-outline-info rounded-3" onclick="GestionCirugias.verSeguimiento(${id})" title="Ver Seguimiento">
                                <i class="bi bi-heart-pulse"></i>
                            </button>
                        `;

                        if (esPendiente) {
                            botones += `
                                <button class="btn btn-sm btn-outline-success rounded-3" onclick="GestionCirugias.completarCirugia(${id})" title="Completar">
                                    <i class="bi bi-check-circle"></i>
                                </button>
                                <button class="btn btn-sm btn-outline-danger rounded-3" onclick="GestionCirugias.cancelarCirugia(${id})" title="Cancelar">
                                    <i class="bi bi-x-circle"></i>
                                </button>
                            `;
                        }

                        return `<div class="d-flex justify-content-center gap-2">${botones}</div>`;
                    }
                }
            ]
        });
    },

    completarCirugia(id) {
        Swal.fire({
            title: '¿Completar cirugía?',
            text: 'Esto marcará la cirugía como finalizada y generará el seguimiento post-operatorio.',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Sí, completar',
            cancelButtonText: 'Cancelar'
        }).then(result => {
            if (!result.isConfirmed) return;

            fetch(`/Cirugia/CompletarCirugia?id=${id}`, { method: 'POST' })
                .then(r => r.json())
                .then(res => {
                    if (res.esCorrecto) {
                        Swal.fire('Listo', res.mensaje || 'Cirugía completada.', 'success')
                            .then(() => $('#tbGestionCirugias').DataTable().ajax.reload(null, false));
                    } else {
                        Swal.fire('Atención', res.mensaje || 'No se pudo completar la cirugía.', 'warning');
                    }
                })
                .catch(() => Swal.fire('Error', 'Ocurrió un error inesperado.', 'error'));
        });
    },

    cancelarCirugia(id) {
        Swal.fire({
            title: '¿Cancelar cirugía?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            confirmButtonText: 'Sí, cancelar',
            cancelButtonText: 'No'
        }).then(result => {
            if (!result.isConfirmed) return;

            const formData = new FormData();
            formData.append('idCirugia', id);

            fetch('/Cirugia/CancelarCirugia', { method: 'POST', body: formData })
                .then(r => r.json())
                .then(res => {
                    if (res.esCorrecto) {
                        Swal.fire('Cancelada', res.mensaje || 'Cirugía cancelada.', 'success')
                            .then(() => $('#tbGestionCirugias').DataTable().ajax.reload(null, false));
                    } else {
                        Swal.fire('Atención', res.mensaje || 'No se pudo cancelar.', 'warning');
                    }
                })
                .catch(() => Swal.fire('Error', 'Ocurrió un error inesperado.', 'error'));
        });
    },

    verSeguimiento(idCirugia) {
        const contenedor = $('#contenidoSeguimiento');
        contenedor.html('<div class="text-center text-muted py-3">Cargando...</div>');
        $('#modalSeguimiento').modal('show');

        fetch(`/Cirugia/ObtenerSeguimientoPorCirugia?idCirugia=${idCirugia}`)
            .then(r => r.json())
            .then(res => {
                if (!res.esCorrecto || !res.dato || res.dato.length === 0) {
                    contenedor.html(`<div class="text-center text-muted py-3">${res.mensaje || 'Aún no hay checkpoints generados para esta cirugía.'}</div>`);
                    return;
                }

                const filas = res.dato.map(s => `
                    <tr>
                        <td class="text-center">Día ${s.diaCheckpoint}</td>
                        <td>${new Date(s.fechaProgramada).toLocaleDateString('es-ES')}</td>
                        <td class="text-center">${s.estado}</td>
                    </tr>
                `).join('');

                contenedor.html(`
                    <table class="table table-sm table-borderless">
                        <thead>
                            <tr><th>Checkpoint</th><th>Fecha Programada</th><th class="text-center">Estado</th></tr>
                        </thead>
                        <tbody>${filas}</tbody>
                    </table>
                `);
            })
            .catch(() => contenedor.html('<div class="text-center text-danger py-3">Error al cargar el seguimiento.</div>'));
    }
};

$(document).ready(function () {
    GestionCirugias.init();
});