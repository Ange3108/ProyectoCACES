(() => {

    const Configuracion = {

        todas: [],

        init() {
            this.cargarConfiguraciones();
            this.eventos();
        },

        eventos() {
            $('#selectCategoria').on('change', function () {
                Configuracion.renderizar($(this).val());
            });

            $('#btnNuevaConfiguracion').on('click', function () {
                $('#nuevaClave').val('');
                $('#nuevoValor').val('');
                $('#nuevoTipo').val('string');
                $('#nuevaCategoria').val('');
                $('#nuevaDescripcion').val('');
                $('#modalCrearConfiguracion').modal('show');
            });

            $('#btnGuardarConfiguracion').on('click', function () {
                Configuracion.crear();
            });

            $(document).on('click', '.btn-editar-configuracion', function () {
                const id = $(this).data('id');
                Configuracion.abrirModalEditar(id);
            });

            $('#btnActualizarConfiguracion').on('click', function () {
                Configuracion.actualizar();
            });
        },

        cargarConfiguraciones() {
            $.ajax({
                url: '/Configuracion/ObtenerTodos',
                type: 'GET',
                success: function (respuesta) {
                    if (!respuesta.esCorrecto) {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                        return;
                    }
                    Configuracion.todas = respuesta.dato ?? [];
                    Configuracion.poblarCategorias();
                    Configuracion.renderizar('');
                },
                error: function () {
                    Swal.fire({ title: 'Error', text: 'No fue posible cargar las configuraciones.', icon: 'error' });
                }
            });
        },

        poblarCategorias() {
            const categorias = [...new Set(Configuracion.todas.map(c => c.categoria).filter(Boolean))].sort();
            const select = $('#selectCategoria');
            const valorActual = select.val();
            select.find('option:not(:first)').remove();
            categorias.forEach(cat => select.append(`<option value="${cat}">${cat}</option>`));
            select.val(valorActual ?? '');
        },

        renderizar(categoria) {
            const lista = categoria
                ? Configuracion.todas.filter(c => c.categoria === categoria)
                : Configuracion.todas;

            if (lista.length === 0) {
                $('#contenedorConfiguraciones').html(`
                    <div class="alert alert-info text-center rounded-3 py-4">
                        <i class="bi bi-inbox fs-2 d-block mb-2 text-muted"></i>
                        No hay configuraciones registradas.
                    </div>`
                );
                return;
            }

            let filas = '';
            lista.forEach(c => {
                filas += `
                    <tr>
                        <td><code>${c.clave}</code></td>
                        <td>${c.valor}</td>
                        <td><span class="badge bg-secondary">${c.tipo}</span></td>
                        <td>${c.categoria}</td>
                        <td>${c.descripcion ?? ''}</td>
                        <td>
                            <button class="btn btn-sm btn-outline-primary btn-editar-configuracion"
                                data-id="${c.idConfiguracion}"
                                data-clave="${c.clave}"
                                data-valor="${c.valor}"
                                data-categoria="${c.categoria}"
                                data-descripcion="${c.descripcion ?? ''}">
                                <i class="bi bi-pencil"></i> Editar
                            </button>
                        </td>
                    </tr>`;
            });

            $('#contenedorConfiguraciones').html(`
                <table class="table table-hover align-middle mt-2">
                    <thead class="table-light">
                        <tr>
                            <th>Clave</th>
                            <th>Valor</th>
                            <th>Tipo</th>
                            <th>Categoría</th>
                            <th>Descripción</th>
                            <th style="width:120px;">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>${filas}</tbody>
                </table>`
            );
        },

        crear() {
            const clave = $('#nuevaClave').val()?.trim();
            const valor = $('#nuevoValor').val()?.trim();
            const categoria = $('#nuevaCategoria').val()?.trim();

            if (!clave) {
                Swal.fire({ title: 'Atención', text: 'Ingrese la clave.', icon: 'warning' });
                return;
            }
            if (!valor) {
                Swal.fire({ title: 'Atención', text: 'Ingrese el valor.', icon: 'warning' });
                return;
            }
            if (!categoria) {
                Swal.fire({ title: 'Atención', text: 'Ingrese la categoría.', icon: 'warning' });
                return;
            }

            const datos = {
                Clave: clave,
                Valor: valor,
                Tipo: $('#nuevoTipo').val(),
                Categoria: categoria,
                Descripcion: $('#nuevaDescripcion').val()?.trim() || null
            };

            $.ajax({
                url: '/Configuracion/CrearConfiguracion',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(datos),
                success: function (respuesta) {
                    if (respuesta.esCorrecto) {
                        Swal.fire({ title: 'Éxito', text: respuesta.mensaje, icon: 'success', timer: 1800, showConfirmButton: false });
                        $('#modalCrearConfiguracion').modal('hide');
                        Configuracion.cargarConfiguraciones();
                    } else {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                    }
                },
                error: function (xhr) {
                    const msg = xhr.responseJSON?.mensaje ?? 'No fue posible crear la configuración.';
                    Swal.fire({ title: 'Error', text: msg, icon: 'error' });
                }
            });
        },

        abrirModalEditar(id) {
            const btn = $(`.btn-editar-configuracion[data-id="${id}"]`);
            $('#editarIdConfiguracion').val(id);
            $('#editarClave').val(btn.data('clave'));
            $('#editarValor').val(btn.data('valor'));
            $('#editarCategoria').val(btn.data('categoria'));
            $('#editarDescripcion').val(btn.data('descripcion'));
            $('#modalEditarConfiguracion').modal('show');
        },

        actualizar() {
            const valor = $('#editarValor').val()?.trim();
            if (!valor) {
                Swal.fire({ title: 'Atención', text: 'Ingrese el valor.', icon: 'warning' });
                return;
            }

            const datos = {
                IdConfiguracion: parseInt($('#editarIdConfiguracion').val()),
                Valor: valor,
                Categoria: $('#editarCategoria').val()?.trim(),
                Descripcion: $('#editarDescripcion').val()?.trim() || null
            };

            $.ajax({
                url: '/Configuracion/ActualizarConfiguracion',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(datos),
                success: function (respuesta) {
                    if (respuesta.esCorrecto) {
                        Swal.fire({ title: 'Éxito', text: respuesta.mensaje, icon: 'success', timer: 1800, showConfirmButton: false });
                        $('#modalEditarConfiguracion').modal('hide');
                        Configuracion.cargarConfiguraciones();
                    } else {
                        Swal.fire({ title: 'Error', text: respuesta.mensaje, icon: 'error' });
                    }
                },
                error: function (xhr) {
                    const msg = xhr.responseJSON?.mensaje ?? 'No fue posible actualizar la configuración.';
                    Swal.fire({ title: 'Error', text: msg, icon: 'error' });
                }
            });
        }
    };

    $(function () {
        Configuracion.init();
    });

})();
