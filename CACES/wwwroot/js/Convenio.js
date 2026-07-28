document.addEventListener("DOMContentLoaded", () => {
    cargarConvenios();
});

async function cargarConvenios() {
    const tbody = document.getElementById("tbodyConvenios");
    if (!tbody) return;

    try {
        const response = await fetch('/Convenio/ObtenerConvenios', {
            cache: 'no-store',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Cache-Control': 'no-cache, no-store, must-revalidate',
                'Pragma': 'no-cache'
            }
        });

        if (!response.ok) throw new Error("Error en la respuesta");

        const convenios = await response.json();

        if (!convenios || convenios.length === 0) {
            tbody.innerHTML = `
            <tr>
                <td colspan="6" class="text-center py-4 text-muted">
                    No hay convenios registrados en el sistema.
                </td>
            </tr>`;
            return;
        }

        let html = '';
        for (let i = 0; i < convenios.length; i++) {
            const c = convenios[i];
            const descuentoVal = c.descuentoPorcentaje !== undefined ? c.descuentoPorcentaje : (c.DescuentoPorcentaje !== undefined ? c.DescuentoPorcentaje : 0);

            const telefonoVal = c.contactoTelefono || c.ContactoTelefono || c.telefonoContacto || c.telefono || 'Sin teléfono';

            html += `
            <tr>
                <td class="ps-4 fw-semibold">${escapeHtml(c.nombre)}</td>
                <td class="text-muted small">${escapeHtml(c.descripcion || 'Sin descripción')}</td>
                <td>
                    <span class="badge rounded-pill px-3 py-2" style="background:#DDF7F8; color:#0B6F73;">
                        ${descuentoVal}%
                    </span>
                </td>
                <td>${escapeHtml(telefonoVal)}</td>
                <td>
                    ${c.estado || c.activo ?
                    '<span class="badge bg-success-subtle text-success border border-success-subtle rounded-pill px-3 py-1.5">Activo</span>' :
                    '<span class="badge bg-danger-subtle text-danger border border-danger-subtle rounded-pill px-3 py-1.5">Inactivo</span>'
                }
                </td>
                <td class="text-center pe-4">
                    <div class="d-flex justify-content-center gap-2">
                        <a href="/Convenio/ActualizarConvenio/${c.idConvenio || c.id}" class="btn btn-sm btn-outline-primary rounded-3">
                            <i class="bi bi-pencil-square me-1"></i> Editar
                        </a>
                    </div>
                </td>
            </tr>`;
        }
        tbody.innerHTML = html;

    } catch (err) {
        console.error(err);
        tbody.innerHTML = `
        <tr>
            <td colspan="6" class="text-center py-4 text-danger">
                Ocurrió un error al cargar los convenios.
            </td>
        </tr>`;
    }
}

function escapeHtml(texto) {
    if (!texto) return '';
    return texto.toString()
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;");
}