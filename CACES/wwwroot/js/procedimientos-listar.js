(() => {

    let tablaDataTable = null;

    const Procedimientos = {

        init() {
            this.cargarProcedimientos();
            this.escucharEventos();
        },

        cargarProcedimientos() {
            $.ajax({
                url: '/Procedimientos/ObtenerProcedimientosQuirur',
                type: 'GET',
                dataType: 'json',
                cache: false,
                headers: {
                    'X-Requested-With': 'XMLHttpRequest',
                    'Cache-Control': 'no-cache, no-store, must-revalidate',
                    'Pragma': 'no-cache',
                    'Expires': '0'
                },
                success: function (respuesta) {

                    if (!respuesta.exito) {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje || 'No se pudieron obtener los datos.', icon: 'error' });
                        return;
                    }

                    // Si DataTables ya existía, se destruye antes de reemplazar el contenido HTML
                    if (tablaDataTable) {
                        tablaDataTable.destroy();
                        tablaDataTable = null;
                    }

                    const lista = respuesta.datos || [];
                    let html = '';

                    if (lista.length === 0) {
                        html = `
                            <tr>
                                <td colspan="6" class="text-center py-4 text-muted">
                                    No hay procedimientos quirúrgicos registrados en el sistema.
                                </td>
                            </tr>
                        `;
                    } else {
                        lista.forEach(item => {
                            const precioFormateado = new Intl.NumberFormat('es-CR', { style: 'currency', currency: 'CRC' }).format(item.precioBase);
                            const descripcion = item.descripcion && item.descripcion.trim() !== '' ? item.descripcion : 'Sin descripción';

                            const estadoBadge = item.estado
                                ? '<span class="badge bg-success-subtle text-success border border-success-subtle rounded-pill px-3 py-1.5">Activo</span>'
                                : '<span class="badge bg-danger-subtle text-danger border border-danger-subtle rounded-pill px-3 py-1.5">Inactivo</span>';

                            const btnEstadoClass = item.estado ? 'btn-outline-danger' : 'btn-outline-success';
                            const btnEstadoIcon = item.estado ? 'bi-shield-slash' : 'bi-shield-check';
                            const btnEstadoTexto = item.estado ? 'Desactivar' : 'Activar';

                            html += `
                                <tr>
                                    <td class="fw-semibold">${item.nombre}</td>
                                    <td class="text-muted small">${descripcion}</td>
                                    <td>
                                        <span class="badge rounded-pill px-3 py-2" style="background:#DDF7F8;color:#0B6F73;">
                                            ${item.nombreEspecialidad || 'N/A'}
                                        </span>
                                    </td>
                                    <td class="fw-bold text-secondary">${precioFormateado}</td>
                                    <td>${estadoBadge}</td>
                                    <td>
                                        <div class="d-flex justify-content-center gap-2">
                                            <a href="/Procedimientos/EditarProcEnReportes/${item.id_Procedimiento}" class="btn btn-sm btn-outline-primary rounded-3">
                                                <i class="bi bi-pencil-square me-1"></i> Editar
                                            </a>
                                            <button type="button" 
                                                    class="btn btn-sm ${btnEstadoClass} rounded-2 btn-cambiar-estado" 
                                                    data-id="${item.id_Procedimiento}"
                                                    title="${btnEstadoTexto}">
                                                <i class="bi ${btnEstadoIcon}"></i> ${btnEstadoTexto}
                                            </button>
                                        </div>
                                    </td>
                                </tr>
                            `;
                        });
                    }

                    $('#tbodyProcedimientos').html(html);

                    // Inicializar DataTables solo si hay datos en la lista
                    if (lista.length > 0) {
                        tablaDataTable = $('#tbProcedimientos').DataTable({
                            language: {
                                url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                            },
                            responsive: true,
                            pageLength: 10,
                            lengthMenu: [5, 10, 25, 50],
                            columnDefs: [
                                { orderable: false, targets: 5 } // Deshabilita la ordenación en la columna de Acciones
                            ]
                        });
                    }
                },

                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar los procedimientos.', icon: 'error' });
                }
            });
        },

        escucharEventos() {
            const self = this;

            $(document).on('click', '.btn-cambiar-estado', function () {
                const id = $(this).data('id');
                const token = $('input[name="__RequestVerificationToken"]').val();

                Swal.fire({
                    title: '¿Está seguro?',
                    text: 'Desea cambiar el estado de este procedimiento.',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Sí, cambiar',
                    cancelButtonText: 'Cancelar'
                }).then((result) => {
                    if (result.isConfirmed) {
                        $.ajax({
                            url: '/Procedimientos/CambiarEstado',
                            type: 'POST',
                            data: { id: id },
                            headers: { 'RequestVerificationToken': token },
                            success: function (respuesta) {
                                if (respuesta.exito) {
                                    Swal.fire({ title: '¡Éxito!', text: respuesta.mensaje, icon: 'success' });
                                    self.cargarProcedimientos();
                                } else {
                                    Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                                }
                            },
                            error: function () {
                                Swal.fire({ title: 'Error', text: 'No se pudo modificar el estado del procedimiento.', icon: 'error' });
                            }
                        });
                    }
                });
            });
        }
    };

    $(function () {
        Procedimientos.init();
    });

})();