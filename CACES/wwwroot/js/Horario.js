(() => {

    const diasSemana = ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'];

    const Horario = {

        idMedicoActual: null,

        init() {
            this.eventos();
        },

        eventos() {
            // Cuando el admin selecciona un médico en el select
            $('#selectMedico').on('change', function () {
                const idMedico = $(this).val();
                if (idMedico) {
                    Horario.idMedicoActual = idMedico;
                    Horario.cargarHorarios(idMedico);
                } else {
                    $('#contenedorHorarios').html('');
                }
            });

            // Abrir modal de crear: preseleccionar médico actual y limpiar campos
            $('#modalCrearHorario').on('show.bs.modal', function () {
                $('#nuevoMedico').val(Horario.idMedicoActual ?? '');
                $('#nuevoDia').val('');
                $('#nuevoHora').val('');
            });

            // Crear horario
            $('#btnGuardarHorario').on('click', function () {
                Horario.crearHorario();
            });

            // Editar horario (delegado porque la tabla se renderiza dinámicamente)
            $(document).on('click', '.btn-editar-horario', function () {
                const id = $(this).data('id');
                Horario.abrirModalEditar(id);
            });

            // Guardar edición
            $('#btnActualizarHorario').on('click', function () {
                Horario.actualizarHorario();
            });

            // Desactivar horario
            $(document).on('click', '.btn-desactivar-horario', function () {
                const id = $(this).data('id');
                Horario.desactivarHorario(id);
            });
        },

        cargarHorarios(idMedico) {
            $.ajax({
                url: `/Horario/ObtenerHorariosPorMedico?idMedico=${idMedico}`,
                type: 'GET',
                success: function (respuesta) {
                    if (!respuesta.esCorrecto || !respuesta.dato || respuesta.dato.length === 0) {
                        $('#contenedorHorarios').html(`
                            <div class="alert alert-info text-center rounded-3 py-4">
                                <i class="bi bi-calendar-x fs-2 d-block mb-2 text-muted"></i>
                                Este médico no tiene horarios registrados.
                            </div>`
                        );
                        return;
                    }

                    let filas = '';
                    respuesta.dato.forEach(h => {
                        const dia = diasSemana[h.diaSemana] ?? h.diaSemana;
                        const hora = (h.horaInicio ?? '').toString().substring(0, 5);
                        const estado = h.activo
                            ? '<span class="badge bg-success">Activo</span>'
                            : '<span class="badge bg-secondary">Inactivo</span>';

                        filas += `
                            <tr>
                                <td>${dia}</td>
                                <td>${hora}</td>
                                <td>${estado}</td>
                                <td>
                                    <button class="btn btn-sm btn-outline-primary btn-editar-horario me-1"
                                        data-id="${h.id_Horario}"
                                        data-dia="${h.diaSemana}"
                                        data-hora="${hora}"
                                        data-activo="${h.activo}">
                                        <i class="bi bi-pencil"></i>
                                    </button>
                                    ${h.activo ? `
                                    <button class="btn btn-sm btn-outline-danger btn-desactivar-horario"
                                        data-id="${h.id_Horario}">
                                        <i class="bi bi-slash-circle"></i>
                                    </button>` : ''}
                                </td>
                            </tr>`;
                    });

                    $('#contenedorHorarios').html(`
                        <table class="table table-hover align-middle mt-3">
                            <thead class="table-light">
                                <tr>
                                    <th>Día</th>
                                    <th>Hora</th>
                                    <th>Estado</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody>${filas}</tbody>
                        </table>`
                    );
                },
                error: function () {
                    Swal.fire({
                        title: 'Error',
                        text: 'No fue posible cargar los horarios.',
                        icon: 'error'
                    });
                }
            });
        },

        crearHorario() {
            const idMedicoSeleccionado = $('#nuevoMedico').val();
            const horaSeleccionada = $('#nuevoHora').val();

            const datos = {
                id_Medico: idMedicoSeleccionado,
                diaSemana: parseInt($('#nuevoDia').val()),
                horaInicio: horaSeleccionada,
                activo: true
            };

            if (!idMedicoSeleccionado) {
                Swal.fire({ title: 'Atención', text: 'Seleccione un médico.', icon: 'warning' });
                return;
            }

            if (!horaSeleccionada) {
                Swal.fire({ title: 'Atención', text: 'Seleccione la hora de inicio.', icon: 'warning' });
                return;
            }

            $.ajax({
                url: '/Horario/CrearHorario',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(datos),
                success: function (respuesta) {
                    if (respuesta.esCorrecto) {
                        Swal.fire({ title: 'Éxito', text: respuesta.mensaje, icon: 'success', timer: 1800, showConfirmButton: false });
                        $('#modalCrearHorario').modal('hide');
                        // Si el médico creado es el que está seleccionado en el panel principal, recarga su tabla
                        if (idMedicoSeleccionado === Horario.idMedicoActual) {
                            Horario.cargarHorarios(Horario.idMedicoActual);
                        }
                    } else {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                    }
                },
                error: function (xhr) {
                    const msg = xhr.responseJSON?.mensaje ?? 'No fue posible crear el horario.';
                    Swal.fire({ title: 'Error', text: msg, icon: 'error' });
                }
            });
        },

        abrirModalEditar(id) {
            const btn = $(`.btn-editar-horario[data-id="${id}"]`);
            $('#editarIdHorario').val(id);
            $('#editarDia').val(btn.data('dia'));
            $('#editarHora').val(btn.data('hora'));
            $('#editarActivo').prop('checked', btn.data('activo') === true || btn.data('activo') === 'true');
            $('#modalEditarHorario').modal('show');
        },

        actualizarHorario() {
            const id = $('#editarIdHorario').val();
            const horaSeleccionada = $('#editarHora').val();

            if (!horaSeleccionada) {
                Swal.fire({ title: 'Atención', text: 'Seleccione la hora de inicio.', icon: 'warning' });
                return;
            }

            const datos = {
                id_Medico: Horario.idMedicoActual,
                diaSemana: parseInt($('#editarDia').val()),
                horaInicio: horaSeleccionada,
                activo: $('#editarActivo').is(':checked')
            };

            $.ajax({
                url: `/Horario/ActualizarHorario?id=${id}`,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(datos),
                success: function (respuesta) {
                    if (respuesta.esCorrecto) {
                        Swal.fire({ title: 'Éxito', text: respuesta.mensaje, icon: 'success', timer: 1800, showConfirmButton: false });
                        $('#modalEditarHorario').modal('hide');
                        Horario.cargarHorarios(Horario.idMedicoActual);
                    } else {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                    }
                },
                error: function (xhr) {
                    const msg = xhr.responseJSON?.mensaje ?? 'No fue posible actualizar el horario.';
                    Swal.fire({ title: 'Error', text: msg, icon: 'error' });
                }
            });
        },

        desactivarHorario(id) {
            Swal.fire({
                title: '¿Desactivar horario?',
                text: 'El médico dejará de recibir citas ese día.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Sí, desactivar',
                cancelButtonText: 'Cancelar'
            }).then(result => {
                if (!result.isConfirmed) return;

                $.ajax({
                    url: `/Horario/DesactivarHorario?id=${id}`,
                    type: 'POST',
                    success: function (respuesta) {
                        if (respuesta.esCorrecto) {
                            Swal.fire({ title: 'Desactivado', text: respuesta.mensaje, icon: 'success', timer: 1800, showConfirmButton: false });
                            Horario.cargarHorarios(Horario.idMedicoActual);
                        } else {
                            Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                        }
                    },
                    error: function () {
                        Swal.fire({ title: 'Error', text: 'No fue posible desactivar el horario.', icon: 'error' });
                    }
                });
            });
        }
    };

    $(function () {
        Horario.init();
    });

})();