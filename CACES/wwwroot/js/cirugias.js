const Cirugias = {

    init() {
        if ($('#tbMisProcedimientos').length) {
            this.cargarMisCirugias();
        }

        
    },

    
    

    guardarCirugia() {
        const dto = {
            IdPaciente: 0,
            IdEspecialidad: parseInt($('#IdEspecialidad').val(), 10) || 0,
            IdProcedimiento: parseInt($('#IdProcedimiento').val(), 10) || null,
            IdMedico: parseInt($('#IdMedico').val(), 10) || 0,
            IdHorario: parseInt($('#IdHorario').val(), 10) || 0,
            FechaCita: $('#FechaCita').val(),
            Hora: $('#Hora').val() ? $('#Hora').val() + ":00" : "00:00:00",
            Motivo: $('#Motivo').val().trim() || "Agendamiento de Cirugia Médico"
        };

        $('#btnGuardar').prop('disabled', true);

        fetch('/Cirugia/AgendarCirugiaJson', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(dto)
        })
            .then(r => r.json())
            .then(res => {
                $('#btnGuardar').prop('disabled', false);

                if (res.esCorrecto) {
                    Swal.fire('¡Éxito!', res.mensaje || 'La cirugía fue agendada correctamente.', 'success')
                        .then(() => {
                            window.location.href = '/Cirugia/MisCirugias';
                        });
                } else {
                    Swal.fire('Atención', res.mensaje || 'No se pudo agendar la cirugía.', 'warning');
                }
            })
            .catch(() => {
                $('#btnGuardar').prop('disabled', false);
                Swal.fire('Error', 'Ocurrió un error inesperado al procesar la solicitud.', 'error');
            });
    },

    cargarMisCirugias() {
        if (!$.fn.DataTable.isDataTable('#tbMisProcedimientos')) {
            $('#tbMisProcedimientos').DataTable({
                ajax: {
                    url: '/Cirugia/ObtenerMisCirugias',
                    type: 'GET',
                    dataSrc: function (res) {
                        if (res && res.esCorrecto && Array.isArray(res.dato)) {
                            if (res.dato.length === 0) {
                                $('#sinMisProcedimientos').removeClass('d-none');
                            } else {
                                $('#sinMisProcedimientos').addClass('d-none');
                            }
                            return res.dato;
                        }

                        $('#sinMisProcedimientos').removeClass('d-none');
                        return [];
                    },
                    error: function (xhr, error, thrown) {
                        console.error('Error al cargar cirugías:', xhr.responseText);
                        $('#sinMisProcedimientos').removeClass('d-none');
                    }
                },
                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                },
                columns: [
                    {
                        data: null,
                        className: 'text-center fw-bold',
                        render: (data, type, row, meta) => meta.row + 1
                    },
                    {
                        data: 'procedimiento',
                        render: data =>
                            `<span class="fw-semibold text-primary">
                <i class="bi bi-activity me-1"></i>${data}
            </span>`
                    },
                    
                    {
                        data: 'medicoResponsable'
                    },
                    {
                        data: 'fechaProcedimiento',
                        render: data => {
                            if (!data) return '';
                            return new Date(data).toLocaleDateString('es-ES', {
                                year: 'numeric',
                                month: '2-digit',
                                day: '2-digit'
                            });
                        }
                    },
                    {
                        data: 'horaProcedimiento',
                        render: h => h ? h.substring(0, 5) : '--:--'
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
                            const idCirugia = row.idCirugia;
                            const puedeCancel = row.estado === 1;

                            return puedeCancel ? `
            <div class="d-flex justify-content-center gap-2">
                <button class="btn btn-sm btn-outline-danger rounded-3" onclick="Cirugias.cancelarCirugia(${idCirugia})" title="Cancelar">
                    <i class="bi bi-x-circle"></i>
                </button>
            </div>
        ` : '';
                        }
                        }
                ]
            });
        }
    },

    cancelarCirugia(idCirugia) {
        Swal.fire({
            title: '¿Está seguro?',
            text: '¿Desea cancelar esta cirugía agendada?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Sí, cancelar',
            cancelButtonText: 'No, mantener'
        }).then((result) => {
            if (result.isConfirmed) {
                const formData = new FormData();
                formData.append('idCirugia', idCirugia);

                fetch('/Cirugia/CancelarCirugia', {
                    method: 'POST',
                    body: formData
                })
                    .then(r => r.json())
                    .then(res => {
                        if (res.esCorrecto) {
                            Swal.fire('Cancelado', res.mensaje || 'La cirugía ha sido cancelada.', 'success')
                                .then(() => {
                                    if ($.fn.DataTable.isDataTable('#tbMisProcedimientos')) {
                                        $('#tbMisProcedimientos').DataTable().ajax.reload(null, false);
                                    } else {
                                        window.location.reload();
                                    }
                                });
                        } else {
                            Swal.fire('Atención', res.mensaje || 'No se pudo cancelar la cirugía.', 'warning');
                        }
                    })
                    .catch(err => {
                        console.error('Error al cancelar:', err);
                        Swal.fire('Error', 'Ocurrió un error inesperado al procesar la solicitud.', 'error');
                    });
            }
        });
    },

    limpiarSelect(element, placeholder) {
        element.empty().append(`<option value="">${placeholder}</option>`).prop('disabled', true);
    }
};

$(document).ready(function () {
    Cirugias.init();
});