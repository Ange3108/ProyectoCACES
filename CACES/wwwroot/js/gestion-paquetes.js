(function () {
    const contenedor = document.getElementById("contenedorTablaPaquetes");
    if (!contenedor) return;

    fetch('/Paquete/ObtenerPaquetes', {
        headers: { 'X-Requested-With': 'XMLHttpRequest' }
    })
        .then(res => {
            if (!res.ok) throw new Error("Error en la respuesta");
            return res.json();
        })
        .then(paquetes => {
            if (!paquetes || paquetes.length === 0) {
                contenedor.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center py-4 text-muted">
                        No hay paquetes registrados en el sistema.
                    </td>
                </tr>`;
                return;
            }

            let html = '';
            for (let i = 0; i < paquetes.length; i++) {
                const p = paquetes[i];
                html += `
                <tr>
                    <td class="fw-semibold">${escapeHtml(p.nombre)}</td>
                    <td class="text-muted small">${escapeHtml(p.descripcion || 'Sin descripción')}</td>
                    <td>
                        <span class="badge rounded-pill px-3 py-2" style="background:#DDF7F8; color:#0B6F73;">
                            ${escapeHtml(p.duracion)}
                        </span>
                    </td>
                    <td class="fw-bold text-secondary">USD $${p.precio}</td>
                    <td>
                        ${p.estado ?
                        '<span class="badge bg-success-subtle text-success border border-success-subtle rounded-pill px-3 py-1.5">Activo</span>' :
                        '<span class="badge bg-danger-subtle text-danger border border-danger-subtle rounded-pill px-3 py-1.5">Inactivo</span>'
                    }
                    </td>
                    <td>
                        <div class="d-flex justify-content-center gap-2">
                            <a href="/Paquete/ActualizarPaquete/${p.idPaquete}" class="btn btn-sm btn-outline-primary rounded-3">
                                <i class="bi bi-pencil-square me-1"></i> Editar
                            </a>
                        </div>
                    </td>
                </tr>`;
            }
            contenedor.innerHTML = html;
        })
        .catch(err => {
            console.error(err);
            contenedor.innerHTML = `
            <tr>
                <td colspan="6" class="text-center py-4 text-danger">
                    Ocurrió un error al cargar los paquetes.
                </td>
            </tr>`;
        });
})();

function escapeHtml(texto) {
    if (!texto) return '';
    return texto.toString()
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;");
}