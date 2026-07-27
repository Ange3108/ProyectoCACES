document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("formCrearPaquete");
    if (!form) return;

    form.addEventListener("submit", async (e) => {
        e.preventDefault();

        const btnGuardar = document.getElementById("btnGuardar");
        const divError = document.getElementById("msjErrorGeneral");
        divError.innerText = "";

        const paqueteDTO = {
            nombre: document.getElementById("Nombre").value.trim(),
            precio: parseFloat(document.getElementById("Precio").value) || 0,
            duracion: document.getElementById("Duracion").value.trim(),
            descripcion: document.getElementById("Descripcion").value.trim()
        };

        btnGuardar.disabled = true;

        try {
            const response = await fetch('/Paquete/CrearPaquete', {
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
                divError.innerText = resultado.mensaje || "No se pudo crear el paquete.";
            }
        } catch (error) {
            console.error("Error:", error);
            divError.innerText = "Error de conexión al guardar el paquete.";
        } finally {
            btnGuardar.disabled = false;
        }
    });
});