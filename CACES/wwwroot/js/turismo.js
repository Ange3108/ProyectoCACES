document.addEventListener("DOMContentLoaded", () => {
    cargarPaquetes();
});

async function cargarPaquetes() {
    const contenedor = document.getElementById("contenedorPaquetes");
    if (!contenedor) return;

    try {
        const response = await fetch(window.location.href, {
            cache: 'no-store',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Cache-Control': 'no-cache, no-store, must-revalidate',
                'Pragma': 'no-cache'
            }
        });

        if (!response.ok) throw new Error("Error al obtener los datos.");

        const paquetes = await response.json();
        const esAdmin = document.getElementById("esAdminFlag")?.value === "true";

        if (!paquetes || paquetes.length === 0) {
            contenedor.innerHTML = `
                <div class="col-12">
                    <div class="empty-state">
                        <i class="bi bi-archive"></i>
                        <p>No hay paquetes disponibles en este momento.</p>
                    </div>
                </div>`;
            return;
        }

        contenedor.innerHTML = paquetes.map(p => `
            <div class="col-12 col-md-6 col-lg-4">
                <article class="card paquete-card h-100">
                    <div class="card-body d-flex flex-column">
                        <h5 class="paquete-nombre">${escapeHtml(p.nombre)}</h5>
                        <p class="paquete-descripcion">${escapeHtml(p.descripcion || '')}</p>

                        <div class="paquete-precio">
                            <span>Precio total</span>
                            USD $${p.precio}
                        </div>

                        <div class="badge-duracion">
                            <i class="bi bi-clock"></i> ${escapeHtml(p.duracion)}
                        </div>

                        <hr class="divider-soft mt-auto" />

                        <div class="d-flex gap-2">
                            <a href="https://wa.me" class="btn-reservar" target="_blank" rel="noopener noreferrer">
                                <i class="bi bi-whatsapp"></i> Reservar
                            </a>
                            ${esAdmin ? `
                                <a href="/Paquete/ActualizarPaquete/${p.idPaquete}" class="btn-editar-paquete" title="Editar paquete">
                                    <i class="bi bi-pencil-square"></i>
                                </a>
                            ` : ''}
                        </div>
                    </div>
                </article>
            </div>
        `).join('');

    } catch (error) {
        console.error("Error:", error);
        contenedor.innerHTML = `<div class="alert alert-danger text-center">Ocurrió un error al cargar los paquetes.</div>`;
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