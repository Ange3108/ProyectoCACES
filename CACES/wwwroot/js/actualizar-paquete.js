document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("formActualizarPaquete");
    if (!form) return;

    const idPaquete = document.getElementById("IdPaquete").value;

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        const btnActualizar = document.getElementById("btnActualizar");
        const divError = document.getElementById("msjErrorGeneral");
        divError.innerText = "";

        const paqueteDTO = {
            idPaquete: parseInt(idPaquete),
            nombre: document.getElementById("Nombre").value.trim(),
            precio: parseFloat(document.getElementById("Precio").value) || 0,
            duracion: document.getElementById("Duracion").value.trim(),
            descripcion: document.getElementById("Descripcion").value.trim(),
            estado: document.getElementById("switchEstado")?.checked ?? true
        };

        btnActualizar.disabled = true;

        try {
            const response = await fetch(`/Paquete/ActualizarPaquete/${idPaquete}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: JSON.stringify(paqueteDTO)
            });

            const resultado = await response.json();

            if (response.ok && resultado.esCorrecto) {
                window.location.href = '/Paquete/ObtenerPaquetes';
            } else {
                divError.innerText = resultado.mensaje || "No se pudo actualizar el paquete.";
            }
        } catch (error) {
            console.error("Error:", error);
            divError.innerText = "Error de conexión al actualizar el paquete.";
        } finally {
            btnActualizar.disabled = false;
        }
    });
});