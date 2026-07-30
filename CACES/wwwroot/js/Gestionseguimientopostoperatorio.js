

(() => {

    const Gestion = {

                    tablas: { },

                init() {
                    this.inicializarCheckpoints();
                this.inicializarPreguntas();
                this.inicializarSeguimientos();
                this.inicializarRespuestas();
                this.inicializarAlertas();
                this.registrarEventos();
        },

                // Helper genérico para mostrar errores de AJAX
                mostrarError(xhr, mensajeDefault) {
                    console.error(xhr.responseText);
                Swal.fire({
                    icon: 'error',
                title: 'Error',
                text: xhr.responseJSON?.mensaje ?? mensajeDefault
            });
        },

                badgeEstadoBool(estado) {
            return estado
                ? '<span class="badge bg-success">Activo</span>'
                : '<span class="badge bg-secondary">Inactivo</span>';
        },

                badgeEstadoSeguimiento(estado) {
            const mapa = {
                    Pendiente: 'bg-secondary',
                Completado: 'bg-success',
                Vencido: 'bg-dark',
                RequiereAtencion: 'bg-danger'
            };
                const clase = mapa[estado] ?? 'bg-secondary';
                return `<span class="badge ${clase}">${estado}</span>`;
        },

                badgeEstadoAlerta(estado) {
            const mapa = {
                    Pendiente: 'bg-warning text-dark',
                Contactado: 'bg-info text-dark',
                Resuelto: 'bg-success'
            };
                const clase = mapa[estado] ?? 'bg-secondary';
                return `<span class="badge ${clase}">${estado}</span>`;
        },

                formatearFecha(fecha) {
            if (!fecha) return '<span class="text-muted">—</span>';
                const d = new Date(fecha);
                return d.toLocaleDateString('es-ES', {year: 'numeric', month: 'short', day: '2-digit' });
        },

                // ===================== CHECKPOINTS =====================
                inicializarCheckpoints() {
            const tabla = $('#tbCheckpoints');
                if (!tabla.length) return;

                this.tablas.checkpoints = tabla.DataTable({
                    ajax: {
                    url: '/CheckPoint/ObtenerCheckPoints',
                type: 'GET',
                    dataSrc: (r) => (r && r.esCorrecto && Array.isArray(r.dato)) ? r.dato : [],
                    error: (xhr) => this.mostrarError(xhr, 'No fue posible cargar los checkpoints.')
                },
                columns: [
                {data: 'idCheckPoint' },
                {data: 'diaCheckPoint' },
                {data: 'estado', render: (v) => this.badgeEstadoBool(v) },
                {
                    data: null, orderable: false, searchable: false, className: 'text-center',
                        render: (data, type, row) => `
                <button class="btn btn-sm btn-outline-primary btnEditarCheckpoint"
                    data-id="${row.idCheckPoint}" data-dia="${row.diaCheckPoint}" data-estado="${row.estado}">
                    <i class="bi bi-pencil-square"></i>
                    Editar
                </button>
                <button class="btn btn-sm btn-outline-danger btnDesactivarCheckpoint" data-id="${row.idCheckPoint}">
                    <i class="bi bi-slash-circle"></i>
                    Desactivar
                </button>`
                    }
                ],
                language: {url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json', emptyTable: 'No hay checkpoints registrados.' }
            });
        },

                // ===================== PREGUNTAS =====================
                inicializarPreguntas() {
            const tabla = $('#tbPreguntas');
                if (!tabla.length) return;

                this.tablas.preguntas = tabla.DataTable({
                    ajax: {
                    url: '/api/PreguntasPOp',
                type: 'GET',
                    dataSrc: (r) => (r && r.esCorrecto && Array.isArray(r.dato)) ? r.dato : [],
                    error: (xhr) => this.mostrarError(xhr, 'No fue posible cargar las preguntas.')
                },
                columns: [
                {data: 'idPregunta' },
                {data: 'texto' },
                {data: null, render: (d) => `${d.valorMinimo} – ${d.valorMaximo}` },
                {data: 'umbralAlerta' },
                {
                    data: 'direccionAlerta',
                        render: (v) => v === 'MayorIgual' ? 'Mayor o igual' : 'Menor o igual'
                    },
                {data: 'estado', render: (v) => this.badgeEstadoBool(v) },
                {
                    data: null, orderable: false, searchable: false, className: 'text-center',
                        render: (data, type, row) => `
                <button class="btn btn-sm btn-outline-primary btnEditarPregunta" data-id="${row.idPregunta}">
                    <i class="bi bi-pencil-square"></i>
                    Editar
                </button>
                <button class="btn btn-sm btn-outline-danger btnEliminarPregunta" data-id="${row.idPregunta}">
                    <i class="bi bi-trash"></i>
                    Eliminar
                </button>`
                    }
                ],
                language: {url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json', emptyTable: 'No hay preguntas registradas.' }
            });
        },

                // ===================== SEGUIMIENTOS =====================
                inicializarSeguimientos() {
            const tabla = $('#tbSeguimientos');
                if (!tabla.length) return;

                this.tablas.seguimientos = tabla.DataTable({
                    ajax: {
                    url: '/SeguimientoPaciente/ObtenerTodos',
                type: 'GET',
                    dataSrc: (r) => (r && r.esCorrecto && Array.isArray(r.dato)) ? r.dato : [],
                    error: (xhr) => this.mostrarError(xhr, 'No fue posible cargar los seguimientos.')
                },
                columns: [
                {data: 'idSeguimiento' },
                {data: 'idCirugia' },
                {data: 'diaCheckpoint' },
                {data: 'fechaProgramada', render: (v) => this.formatearFecha(v) },
                {data: 'estado', render: (v) => this.badgeEstadoSeguimiento(v) },
                {data: 'fechaRegistro', render: (v) => this.formatearFecha(v) },
                {
                    data: null, orderable: false, searchable: false, className: 'text-center',
                        render: (data, type, row) => `
                <button class="btn btn-sm btn-outline-secondary btnVerRespuestas" data-id="${row.idSeguimiento}">
                    <i class="bi bi-eye"></i> Respuestas
                </button>`
                    }
                ],
                order: [[3, 'desc']],
                language: {url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json', emptyTable: 'No hay seguimientos registrados.' }
            });
        },

                // ===================== RESPUESTAS =====================
                inicializarRespuestas() {
            const tabla = $('#tbRespuestas');
                if (!tabla.length) return;

                this.tablas.respuestas = tabla.DataTable({
                    ajax: {
                    url: '/RespuestaSeguimiento/ObtenerTodas',
                type: 'GET',
                    dataSrc: (r) => (r && r.esCorrecto && Array.isArray(r.dato)) ? r.dato : [],
                    error: (xhr) => this.mostrarError(xhr, 'No fue posible cargar las respuestas.')
                },
                columns: [
                {data: 'idRespuesta' },
                {data: 'idSeguimiento' },
                {data: 'textoPregunta', defaultContent: '<span class="text-muted">—</span>' },
                {data: 'valorRespuesta' },
                {
                    data: 'generoAlerta',
                        render: (v) => v
                ? '<span class="badge bg-danger">Sí</span>'
                : '<span class="badge bg-success">No</span>'
                    }
                ],
                order: [[0, 'desc']],
                language: {url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json', emptyTable: 'No hay respuestas registradas.' }
            });
        },

                // ===================== ALERTAS =====================
                inicializarAlertas() {
            const tabla = $('#tbAlertas');
                if (!tabla.length) return;

                const soloPendientes = $('#chkSoloPendientes').is(':checked');

                this.tablas.alertas = tabla.DataTable({
                    ajax: {
                    url: soloPendientes ? '/AlertaStaff/ObtenerPendientes' : '/AlertaStaff/ObtenerTodas',
                type: 'GET',
                    dataSrc: (r) => {
                        if (!r || !r.esCorrecto || !Array.isArray(r.dato)) return [];
                        const pendientes = r.dato.filter(a => a.estado === 'Pendiente').length;
                const badge = $('#badgeAlertasPendientes');
                        if (pendientes > 0) {
                    badge.text(pendientes).removeClass('d-none');
                        } else {
                    badge.addClass('d-none');
                        }
                return r.dato;
                    },
                    error: (xhr) => this.mostrarError(xhr, 'No fue posible cargar las alertas.')
                },
                columns: [
                {data: 'idAlerta' },
                {data: 'idSeguimiento' },
                {data: 'idCirugia' },
                {data: 'fechaGenerada', render: (v) => this.formatearFecha(v) },
                {data: 'estado', render: (v) => this.badgeEstadoAlerta(v) },
                {data: 'nombreUsuarioAtendio', defaultContent: '<span class="text-muted">—</span>' },
                {data: 'observaciones', defaultContent: '<span class="text-muted">—</span>' },
                {
                    data: null, orderable: false, searchable: false, className: 'text-center',
                        render: (data, type, row) => row.estado === 'Resuelto'
                ? '<span class="text-muted small">Cerrada</span>'
                : `<button class="btn btn-sm btn-outline-primary btnAtenderAlerta" data-id="${row.idAlerta}">
                    <i class="bi bi-check2-circle"></i> Atender
                </button>`
                    }
                ],
                order: [[3, 'desc']],
                language: {url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json', emptyTable: 'No hay alertas registradas.' }
            });
        },

                registrarEventos() {

                    // -------- Checkpoints: crear --------
                    $('#formCrearCheckpoint').on('submit', function (e) {
                        e.preventDefault();
                        const formulario = $(this);

                        const diaCheckpoint = formulario.find('[name=DiaCheckpoint]').val();
                        if (diaCheckpoint === '' || diaCheckpoint === null) {
                            Swal.fire({ icon: 'warning', title: 'Atención', text: 'Complete el día del checkpoint.' });
                            return;
                        }

                        $.ajax({
                            url: '/ConfiguracionCheckPoint/CrearCheckPoint',
                            type: 'POST',
                            data: formulario.serialize(),
                            success: (r) => {
                                if (!r?.esCorrecto) {
                                    Swal.fire({ icon: 'error', title: 'Error', text: r?.mensaje ?? 'No se pudo crear el checkpoint.' });
                                    return;
                                }
                                bootstrap.Modal.getInstance(document.getElementById('crearCheckpointModal'))?.hide();
                                formulario[0].reset();
                                Swal.fire({ icon: 'success', title: 'Checkpoint creado', text: r.mensaje });
                                Gestion.tablas.checkpoints.ajax.reload(null, false);
                            },
                            error: (xhr) => Gestion.mostrarError(xhr, 'No fue posible crear el checkpoint.')
                        });
                    });

                // -------- Checkpoints: abrir modal editar --------
                $(document).on('click', '.btnEditarCheckpoint', function () {
                    $('#editCheckpointId').val($(this).data('id'));
                $('#editCheckpointDia').val($(this).data('dia'));
                $('#editCheckpointEstado').prop('checked', $(this).data('estado') === true || $(this).data('estado') === 'true');
                new bootstrap.Modal(document.getElementById('editarCheckpointModal')).show();
            });

                // -------- Checkpoints: guardar edición --------
                $('#formEditarCheckpoint').on('submit', function (e) {
                    e.preventDefault();
                const formulario = $(this);
                const id = $('#editCheckpointId').val();

                if ($('#editCheckpointDia').val() === '') {
                    Swal.fire({ icon: 'warning', title: 'Atención', text: 'Complete el día del checkpoint.' });
                return;
                }

                $.ajax({
                    url: `/ConfiguracionCheckPoint/ActualizarCheckPoint?id=${id}`,
                type: 'POST',
                data: formulario.serialize(),
                    success: (r) => {
                        if (!r?.esCorrecto) {
                    Swal.fire({ icon: 'error', title: 'Error', text: r?.mensaje ?? 'No se pudo actualizar el checkpoint.' });
                return;
                        }
                bootstrap.Modal.getInstance(document.getElementById('editarCheckpointModal'))?.hide();
                Swal.fire({icon: 'success', title: 'Checkpoint actualizado', text: r.mensaje });
                Gestion.tablas.checkpoints.ajax.reload(null, false);
                    },
                    error: (xhr) => Gestion.mostrarError(xhr, 'No fue posible actualizar el checkpoint.')
                });
            });

                // -------- Checkpoints: desactivar --------
                $(document).on('click', '.btnDesactivarCheckpoint', function () {
                const id = $(this).data('id');
                Swal.fire({
                    icon: 'warning', title: '¿Desactivar checkpoint?', showCancelButton: true, confirmButtonText: 'Sí, desactivar'
                }).then((res) => {
                    if (!res.isConfirmed) return;
                $.ajax({
                    url: `/ConfiguracionCheckPoint/DesactivarCheckPoint?id=${id}`,
                type: 'POST',
                        success: (r) => {
                            if (!r?.esCorrecto) {
                    Swal.fire({ icon: 'error', title: 'Error', text: r?.mensaje });
                return;
                            }
                Swal.fire({icon: 'success', title: 'Checkpoint desactivado' });
                Gestion.tablas.checkpoints.ajax.reload(null, false);
                        },
                        error: (xhr) => Gestion.mostrarError(xhr, 'No fue posible desactivar el checkpoint.')
                    });
                });
            });

                // -------- Preguntas: crear --------
                $('#formCrearPregunta').on('submit', function (e) {
                    e.preventDefault();
                const formulario = $(this);

                const texto = formulario.find('[name=Texto]').val()?.trim();
                const valorMinimo = formulario.find('[name=ValorMinimo]').val();
                const valorMaximo = formulario.find('[name=ValorMaximo]').val();
                const umbralAlerta = formulario.find('[name=UmbralAlerta]').val();
                const direccionAlerta = formulario.find('[name=DireccionAlerta]').val();

                if (!texto) {
                    Swal.fire({ icon: 'warning', title: 'Atención', text: 'Escriba el texto de la pregunta.' });
                return;
                }
                if (valorMinimo === '' || valorMaximo === '' || umbralAlerta === '') {
                    Swal.fire({ icon: 'warning', title: 'Atención', text: 'Complete los valores mínimo, máximo y umbral.' });
                return;
                }
                if (!direccionAlerta) {
                    Swal.fire({ icon: 'warning', title: 'Atención', text: 'Seleccione la dirección de la alerta.' });
                return;
                }

                const datos = {
                    Texto: texto,
                ValorMinimo: parseInt(valorMinimo),
                ValorMaximo: parseInt(valorMaximo),
                UmbralAlerta: parseInt(umbralAlerta),
                DireccionAlerta: parseInt(direccionAlerta)
                };
                $.ajax({
                    url: '/api/PreguntasPOp',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(datos),
                    success: (r) => {
                        if (!r?.esCorrecto) {
                    Swal.fire({ icon: 'error', title: 'Error', text: r?.mensaje ?? 'No se pudo crear la pregunta.' });
                return;
                        }
                bootstrap.Modal.getInstance(document.getElementById('crearPreguntaModal'))?.hide();
                formulario[0].reset();
                Swal.fire({icon: 'success', title: 'Pregunta creada', text: r.mensaje });
                Gestion.tablas.preguntas.ajax.reload(null, false);
                    },
                    error: (xhr) => Gestion.mostrarError(xhr, 'No fue posible crear la pregunta.')
                });
            });

                // -------- Preguntas: abrir modal editar --------
                $(document).on('click', '.btnEditarPregunta', function () {
                const id = $(this).data('id');
                $.ajax({
                    url: `/api/PreguntasPOp/${id}`,
                type: 'GET',
                    success: (r) => {
                        if (!r?.esCorrecto) {
                    Swal.fire({ icon: 'error', title: 'Error', text: r?.mensaje ?? 'No se encontró la pregunta.' });
                return;
                        }
                const p = r.dato;
                $('#editPreguntaId').val(p.idPregunta);
                $('#editPreguntaTexto').val(p.texto);
                $('#editPreguntaMin').val(p.valorMinimo);
                $('#editPreguntaMax').val(p.valorMaximo);
                $('#editPreguntaUmbral').val(p.umbralAlerta);
                $('#editPreguntaDireccion').val(p.direccionAlerta);
                $('#editPreguntaEstado').prop('checked', p.estado);
                new bootstrap.Modal(document.getElementById('editarPreguntaModal')).show();
                    },
                    error: (xhr) => Gestion.mostrarError(xhr, 'No fue posible cargar la pregunta.')
                });
            });

                // -------- Preguntas: guardar edición --------
                $('#formEditarPregunta').on('submit', function (e) {
                    e.preventDefault();

                const texto = $('#editPreguntaTexto').val()?.trim();
                const valorMinimo = $('#editPreguntaMin').val();
                const valorMaximo = $('#editPreguntaMax').val();
                const umbralAlerta = $('#editPreguntaUmbral').val();

                if (!texto) {
                    Swal.fire({ icon: 'warning', title: 'Atención', text: 'Escriba el texto de la pregunta.' });
                return;
                }
                if (valorMinimo === '' || valorMaximo === '' || umbralAlerta === '') {
                    Swal.fire({ icon: 'warning', title: 'Atención', text: 'Complete los valores mínimo, máximo y umbral.' });
                return;
                }

                const datos = {
                    idPregunta: parseInt($('#editPreguntaId').val()),
                Texto: texto,
                ValorMinimo: parseInt(valorMinimo),
                ValorMaximo: parseInt(valorMaximo),
                UmbralAlerta: parseInt(umbralAlerta),
                DireccionAlerta: $('#editPreguntaDireccion').val(),
                Estado: $('#editPreguntaEstado').is(':checked')
                };
                $.ajax({
                    url: '/api/PreguntasPOp',
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(datos),
                    success: (r) => {
                        if (!r?.esCorrecto) {
                    Swal.fire({ icon: 'error', title: 'Error', text: r?.mensaje ?? 'No se pudo actualizar la pregunta.' });
                return;
                        }
                bootstrap.Modal.getInstance(document.getElementById('editarPreguntaModal'))?.hide();
                Swal.fire({icon: 'success', title: 'Pregunta actualizada', text: r.mensaje });
                Gestion.tablas.preguntas.ajax.reload(null, false);
                    },
                    error: (xhr) => Gestion.mostrarError(xhr, 'No fue posible actualizar la pregunta.')
                });
            });

                // -------- Preguntas: eliminar --------
                $(document).on('click', '.btnEliminarPregunta', function () {
                const id = $(this).data('id');
                Swal.fire({
                    icon: 'warning', title: '¿Eliminar pregunta?', showCancelButton: true, confirmButtonText: 'Sí, eliminar'
                }).then((res) => {
                    if (!res.isConfirmed) return;
                $.ajax({
                    url: `/api/PreguntasPOp/${id}`,
                type: 'DELETE',
                        success: (r) => {
                            if (!r?.esCorrecto) {
                    Swal.fire({ icon: 'error', title: 'Error', text: r?.mensaje });
                return;
                            }
                Swal.fire({icon: 'success', title: 'Pregunta eliminada' });
                Gestion.tablas.preguntas.ajax.reload(null, false);
                        },
                        error: (xhr) => Gestion.mostrarError(xhr, 'No fue posible eliminar la pregunta.')
                    });
                });
            });

            // -------- Seguimientos: recargar --------
            $('#btnRecargarSeguimientos').on('click', () => Gestion.tablas.seguimientos.ajax.reload(null, false));

                // -------- Seguimientos: generar checkpoints --------
                $('#formGenerarCheckpoints').on('submit', function (e) {
                    e.preventDefault();
                const idCirugia = $(this).find('[name=IdCirugia]').val();

                if (idCirugia === '') {
                    Swal.fire({ icon: 'warning', title: 'Atención', text: 'Ingrese el id de la cirugía.' });
                return;
                }

                $.ajax({
                    url: `/SeguimientoPaciente/generar/${idCirugia}`,
                type: 'POST',
                    success: (r) => {
                        if (!r?.esCorrecto) {
                    Swal.fire({ icon: 'error', title: 'Error', text: r?.mensaje ?? 'No se pudieron generar los checkpoints.' });
                return;
                        }
                bootstrap.Modal.getInstance(document.getElementById('generarCheckpointsModal'))?.hide();
                Swal.fire({icon: 'success', title: 'Checkpoints generados', text: r.mensaje });
                Gestion.tablas.seguimientos.ajax.reload(null, false);
                    },
                    error: (xhr) => Gestion.mostrarError(xhr, 'No fue posible generar los checkpoints.')
                });
            });

                // -------- Seguimientos: ver respuestas (salta a la pestaña filtrando) --------
                $(document).on('click', '.btnVerRespuestas', function () {
                const id = $(this).data('id');
                $('#tab-respuestas-btn').tab('show');
                Gestion.tablas.respuestas.column(1).search(id).draw();
            });

            // -------- Respuestas: agregar fila dinámica --------
            const filaRespuestaHtml = () => `
                <div class="row align-items-end mb-2 filaRespuesta">
                    <div class="col-7">
                        <label class="form-label">Pregunta</label>
                        <select class="form-select selectPregunta" required></select>
                    </div>
                    <div class="col-3">
                        <label class="form-label">Valor</label>
                        <input type="number" class="form-control inputValor" required>
                    </div>
                    <div class="col-2">
                        <button type="button" class="btn btn-outline-danger btnQuitarRespuesta"><i class="bi bi-x-lg"></i></button>
                    </div>
                </div>`;

            const cargarSelectPreguntas = (select) => {
                    $.ajax({
                        url: '/api/PreguntasPOp/activas',
                        type: 'GET',
                        success: (r) => {
                            if (!r?.esCorrecto) return;
                            r.dato.forEach(p => select.append(`<option value="${p.idPregunta}">${p.texto}</option>`));
                        }
                    });
            };

                $('#registrarRespuestasModal').on('show.bs.modal', function () {
                    $('#filasRespuestas').empty();
                $('#respIdSeguimiento').val('');
                const fila = $(filaRespuestaHtml());
                $('#filasRespuestas').append(fila);
                cargarSelectPreguntas(fila.find('.selectPregunta'));
            });

                $('#btnAgregarRespuesta').on('click', function () {
                const fila = $(filaRespuestaHtml());
                $('#filasRespuestas').append(fila);
                cargarSelectPreguntas(fila.find('.selectPregunta'));
            });

                $(document).on('click', '.btnQuitarRespuesta', function () {
                if ($('.filaRespuesta').length > 1) {
                    $(this).closest('.filaRespuesta').remove();
                }
            });

                // -------- Respuestas: registrar --------
                $('#formRegistrarRespuestas').on('submit', function (e) {
                    e.preventDefault();

                const idSeguimientoTexto = $('#respIdSeguimiento').val();
                if (idSeguimientoTexto === '') {
                    Swal.fire({ icon: 'warning', title: 'Atención', text: 'Ingrese el id del seguimiento.' });
                return;
                }

                let filasIncompletas = false;
                $('.filaRespuesta').each(function () {
                    const pregunta = $(this).find('.selectPregunta').val();
                const valor = $(this).find('.inputValor').val();
                if (!pregunta || valor === '') {
                    filasIncompletas = true;
                    }
                });
                if (filasIncompletas) {
                    Swal.fire({ icon: 'warning', title: 'Atención', text: 'Seleccione la pregunta e ingrese el valor en cada fila.' });
                return;
                }

                const idSeguimiento = parseInt(idSeguimientoTexto);
                const respuestas = [];
                $('.filaRespuesta').each(function () {
                    respuestas.push({
                        IdSeguimiento: idSeguimiento,
                        IdPregunta: parseInt($(this).find('.selectPregunta').val()),
                        ValorRespuesta: parseInt($(this).find('.inputValor').val())
                    });
                });

                $.ajax({
                    url: '/RespuestaSeguimiento/RegistrarRespuestas',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(respuestas),
                    success: (r) => {
                        if (!r?.esCorrecto) {
                    Swal.fire({ icon: 'error', title: 'Error', text: r?.mensaje ?? 'No se pudieron registrar las respuestas.' });
                return;
                        }
                bootstrap.Modal.getInstance(document.getElementById('registrarRespuestasModal'))?.hide();
                Swal.fire({icon: 'success', title: 'Respuestas registradas', text: r.mensaje });
                Gestion.tablas.respuestas.ajax.reload(null, false);
                Gestion.tablas.seguimientos.ajax.reload(null, false);
                Gestion.tablas.alertas.ajax.reload(null, false);
                    },
                    error: (xhr) => Gestion.mostrarError(xhr, 'No fue posible registrar las respuestas.')
                });
            });

                // -------- Alertas: filtro pendientes / todas --------
                $('#chkSoloPendientes, #btnRecargarAlertas').on('change click', function () {
                const soloPendientes = $('#chkSoloPendientes').is(':checked');
                Gestion.tablas.alertas.ajax.url(soloPendientes ? '/AlertaStaff/ObtenerPendientes' : '/AlertaStaff/ObtenerTodas').load();
            });

                // -------- Alertas: abrir modal atender --------
                $(document).on('click', '.btnAtenderAlerta', function () {
                    $('#atenderIdAlerta').val($(this).data('id'));
                $('#atenderObservaciones').val('');
                new bootstrap.Modal(document.getElementById('atenderAlertaModal')).show();
            });

                // -------- Alertas: guardar atención --------
                $('#formAtenderAlerta').on('submit', function (e) {
                    e.preventDefault();

                if ($('#atenderEstado').val() === '') {
                    Swal.fire({ icon: 'warning', title: 'Atención', text: 'Seleccione el nuevo estado.' });
                return;
                }

                const datos = {
                    IdAlerta: parseInt($('#atenderIdAlerta').val()),
                Estado: parseInt($('#atenderEstado').val()),
                Observaciones: $('#atenderObservaciones').val()
                };
                $.ajax({
                    url: '/AlertaStaff/AtenderAlerta',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(datos),
                    success: (r) => {
                        if (!r?.esCorrecto) {
                    Swal.fire({ icon: 'error', title: 'Error', text: r?.mensaje ?? 'No se pudo atender la alerta.' });
                return;
                        }
                bootstrap.Modal.getInstance(document.getElementById('atenderAlertaModal'))?.hide();
                Swal.fire({icon: 'success', title: 'Alerta actualizada', text: r.mensaje });
                Gestion.tablas.alertas.ajax.reload(null, false);
                    },
                    error: (xhr) => {
                        if (xhr.status === 401 || xhr.status === 403) {
                    Swal.fire({ icon: 'error', title: 'Sin permiso', text: 'Esta acción requiere rol de Administrador.' });
                return;
                        }
                Gestion.mostrarError(xhr, 'No fue posible atender la alerta.');
                    }
                });
            });
        }
    };

                $(function () {
                    Gestion.init();
    });

})();







