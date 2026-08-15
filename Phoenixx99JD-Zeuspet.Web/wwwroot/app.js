// ============================================
// API helpers
// ============================================

const api = {
    async get(url) {
        const res = await fetch(url);
        if (!res.ok) throw new Error("Error al obtener los datos.");
        return res.json();
    },
    async enviar(url, opciones) {
        const res = await fetch(url, {
            method: opciones.metodo || "GET",
            headers: { "Content-Type": "application/json" },
            body: opciones.cuerpo ? JSON.stringify(opciones.cuerpo) : undefined
        });
        if (!res.ok) {
            const texto = await res.text();
            throw new Error(texto || "Ocurrio un error.");
        }
        return res.status === 204 ? null : res.json();
    }
};

// ============================================
// Utilidades
// ============================================

const $ = (id) => document.getElementById(id);

let clientesCache = [];
let mascotasCache = [];

function toast(mensaje, tipo = "exito") {
    const t = $("toast");
    t.textContent = mensaje;
    t.className = `toast ${tipo}`;
    t.hidden = false;
    clearTimeout(t._tiempo);
    t._tiempo = setTimeout(() => (t.hidden = true), 2800);
}

function abrirModal(titulo) {
    $("modal-titulo").textContent = titulo;
    $("modal").hidden = false;
}

function cerrarModal() {
    $("modal").hidden = true;
}

function campoModal(etiqueta, nombre, tipo = "text", valor = "", requerido = true) {
    return `<label for="${nombre}">${etiqueta}</label>
            <input id="${nombre}" name="${nombre}" type="${tipo}" value="${valor}" ${requerido ? "required" : ""}>`;
}

// ============================================
// Navegacion por pestanas
// ============================================

document.querySelectorAll(".pestana").forEach((btn) => {
    btn.addEventListener("click", () => {
        document.querySelectorAll(".pestana").forEach((b) => b.classList.remove("activa"));
        document.querySelectorAll(".vista").forEach((v) => v.classList.remove("activa"));
        btn.classList.add("activa");
        $("vista-" + btn.dataset.vista).classList.add("activa");

        if (btn.dataset.vista === "dashboard") cargarEstadisticas();
        if (btn.dataset.vista === "clientes") cargarClientes();
        if (btn.dataset.vista === "mascotas") cargarMascotas();
        if (btn.dataset.vista === "servicios") cargarServicios();
    });
});

// ============================================
// Dashboard
// ============================================

async function cargarEstadisticas() {
    try {
        const stats = await api.get("/api/estadisticas");
        $("stat-clientes").textContent = stats.clientes;
        $("stat-mascotas").textContent = stats.mascotas;

        const lista = $("stat-especies");
        lista.innerHTML = "";
        const especies = stats.especies || {};
        for (const [especie, cantidad] of Object.entries(especies)) {
            const li = document.createElement("li");
            li.innerHTML = `<span>${especie}</span><strong>${cantidad}</strong>`;
            lista.appendChild(li);
        }
    } catch (err) {
        toast(err.message, "error");
    }
}

// ============================================
// Clientes
// ============================================

async function cargarClientes() {
    try {
        clientesCache = await api.get("/api/clientes");
        renderizarClientes(clientesCache);
    } catch (err) {
        toast(err.message, "error");
    }
}

function renderizarClientes(clientes) {
    const cuerpo = $("tabla-clientes");
    const vacio = $("vacio-clientes");

    if (clientes.length === 0) {
        cuerpo.innerHTML = "";
        vacio.hidden = false;
        return;
    }
    vacio.hidden = true;

    cuerpo.innerHTML = clientes.map((c) => `
        <tr>
            <td><span class="insignia">${c.id}</span></td>
            <td><strong>${c.nombre}</strong></td>
            <td>${c.edad}</td>
            <td>${c.telefono}</td>
            <td>${c.email}</td>
            <td>${c.direccion}</td>
            <td>${c.mascotas ? c.mascotas.length : 0}</td>
            <td>
                <button class="btn btn-editar" data-editar-cliente="${c.id}">Editar</button>
                <button class="btn btn-peligro" data-eliminar-cliente="${c.id}">Eliminar</button>
            </td>
        </tr>`).join("");
}

$("buscador-clientes").addEventListener("input", (e) => {
    const texto = e.target.value.toLowerCase();
    const filtrados = clientesCache.filter((c) => c.nombre.toLowerCase().includes(texto));
    renderizarClientes(filtrados);
});

$("tabla-clientes").addEventListener("click", async (e) => {
    const btnEditar = e.target.closest("[data-editar-cliente]");
    const btnEliminar = e.target.closest("[data-eliminar-cliente]");

    if (btnEditar) {
        const cliente = clientesCache.find((c) => c.id === btnEditar.dataset.editarCliente);
        abrirModalCliente(cliente);
    }
    if (btnEliminar) {
        if (!confirm("¿Seguro que deseas eliminar este cliente y sus mascotas?")) return;
        try {
            await api.enviar(`/api/clientes/${btnEliminar.dataset.eliminarCliente}`, { metodo: "DELETE" });
            toast("Cliente eliminado.");
            cargarClientes();
        } catch (err) {
            toast(err.message, "error");
        }
    }
});

$("btn-nuevo-cliente").addEventListener("click", () => abrirModalCliente(null));

function abrirModalCliente(cliente) {
    abrirModal(cliente ? "Editar cliente" : "Nuevo cliente");
    $("modal-forma").innerHTML = `
        ${campoModal("Nombre", "nombre", "text", cliente?.nombre ?? "")}
        ${campoModal("Edad", "edad", "number", cliente?.edad ?? "", true)}
        ${campoModal("Teléfono", "telefono", "text", cliente?.telefono ?? "")}
        ${campoModal("Email", "email", "email", cliente?.email ?? "")}
        ${campoModal("Dirección", "direccion", "text", cliente?.direccion ?? "")}
        <button type="submit" class="btn btn-primario">Guardar</button>`;

    const forma = $("modal-forma");
    forma.onsubmit = async (e) => {
        e.preventDefault();
        const datos = {
            nombre: forma.nombre.value.trim(),
            edad: Number(forma.edad.value),
            telefono: forma.telefono.value.trim(),
            email: forma.email.value.trim(),
            direccion: forma.direccion.value.trim()
        };
        try {
            if (cliente) {
                await api.enviar(`/api/clientes/${cliente.id}`, { metodo: "PUT", cuerpo: datos });
                toast("Cliente actualizado.");
            } else {
                await api.enviar("/api/clientes", { metodo: "POST", cuerpo: datos });
                toast("Cliente registrado.");
            }
            cerrarModal();
            cargarClientes();
        } catch (err) {
            toast(err.message, "error");
        }
    };
}

// ============================================
// Mascotas
// ============================================

async function cargarMascotas() {
    try {
        const [mascotas, clientes] = await Promise.all([
            api.get("/api/mascotas"),
            api.get("/api/clientes")
        ]);
        mascotasCache = mascotas;
        clientesCache = clientes;
        renderizarMascotas(mascotas);
    } catch (err) {
        toast(err.message, "error");
    }
}

function nombreDueno(clienteId) {
    const c = clientesCache.find((x) => x.id === clienteId);
    return c ? c.nombre : "Sin dueño";
}

function renderizarMascotas(mascotas) {
    const cuerpo = $("tabla-mascotas");
    const vacio = $("vacio-mascotas");
    const especie = $("filtro-especie").value;
    const texto = $("buscador-mascotas").value.toLowerCase();

    const filtradas = mascotas.filter((m) => {
        const cumpleTexto = m.nombre.toLowerCase().includes(texto);
        const cumpleEspecie = !especie || m.especie === especie;
        return cumpleTexto && cumpleEspecie;
    });

    if (filtradas.length === 0) {
        cuerpo.innerHTML = "";
        vacio.hidden = false;
        return;
    }
    vacio.hidden = true;

    cuerpo.innerHTML = filtradas.map((m) => `
        <tr>
            <td><span class="insignia">${m.id}</span></td>
            <td><strong>${m.nombre}</strong></td>
            <td>${m.especie}</td>
            <td>${m.raza || "—"}</td>
            <td>${m.edad}</td>
            <td>${nombreDueno(m.clienteId)}</td>
            <td>
                <button class="btn btn-editar" data-editar-mascota="${m.id}">Editar</button>
                <button class="btn btn-peligro" data-eliminar-mascota="${m.id}">Eliminar</button>
            </td>
        </tr>`).join("");
}

$("buscador-mascotas").addEventListener("input", () => renderizarMascotas(mascotasCache));
$("filtro-especie").addEventListener("change", () => renderizarMascotas(mascotasCache));

$("tabla-mascotas").addEventListener("click", async (e) => {
    const btnEditar = e.target.closest("[data-editar-mascota]");
    const btnEliminar = e.target.closest("[data-eliminar-mascota]");

    if (btnEditar) {
        const mascota = mascotasCache.find((m) => m.id === btnEditar.dataset.editarMascota);
        abrirModalMascota(mascota);
    }
    if (btnEliminar) {
        if (!confirm("¿Seguro que deseas eliminar esta mascota?")) return;
        try {
            await api.enviar(`/api/mascotas/${btnEliminar.dataset.eliminarMascota}`, { metodo: "DELETE" });
            toast("Mascota eliminada.");
            cargarMascotas();
        } catch (err) {
            toast(err.message, "error");
        }
    }
});

$("btn-nueva-mascota").addEventListener("click", () => abrirModalMascota(null));

function abrirModalMascota(mascota) {
    if (clientesCache.length === 0) {
        toast("Primero debes registrar un cliente.", "error");
        return;
    }
    abrirModal(mascota ? "Editar mascota" : "Nueva mascota");

    const opcionesDuenos = clientesCache
        .map((c) => `<option value="${c.id}" ${mascota && c.id === mascota.clienteId ? "selected" : ""}>${c.nombre}</option>`)
        .join("");

    $("modal-forma").innerHTML = `
        ${campoModal("Nombre", "nombre", "text", mascota?.nombre ?? "")}
        ${campoModal("Especie", "especie", "text", mascota?.especie ?? "")}
        ${campoModal("Raza", "raza", "text", mascota?.raza ?? "", false)}
        ${campoModal("Edad", "edad", "number", mascota?.edad ?? "", true)}
        <label for="clienteId">Dueño</label>
        <select id="clienteId" name="clienteId">${opcionesDuenos}</select>
        <button type="submit" class="btn btn-primario">Guardar</button>`;

    const forma = $("modal-forma");
    forma.onsubmit = async (e) => {
        e.preventDefault();
        const datos = {
            nombre: forma.nombre.value.trim(),
            especie: forma.especie.value.trim(),
            raza: forma.raza.value.trim(),
            edad: Number(forma.edad.value),
            clienteId: forma.clienteId.value
        };
        try {
            if (mascota) {
                await api.enviar(`/api/mascotas/${mascota.id}`, { metodo: "PUT", cuerpo: datos });
                toast("Mascota actualizada.");
            } else {
                await api.enviar("/api/mascotas", { metodo: "POST", cuerpo: datos });
                toast("Mascota registrada.");
            }
            cerrarModal();
            cargarMascotas();
        } catch (err) {
            toast(err.message, "error");
        }
    };
}

// ============================================
// Servicios
// ============================================

async function cargarServicios() {
    try {
        const mascotas = await api.get("/api/mascotas");
        const select = $("servicio-mascota");
        select.innerHTML = mascotas
            .map((m) => `<option value="${m.id}">${m.nombre} (${m.especie})</option>`)
            .join("");
        $("resultado-servicio").hidden = true;
    } catch (err) {
        toast(err.message, "error");
    }
}

$("btn-atender").addEventListener("click", async () => {
    const tipo = $("servicio-tipo").value;
    const mascotaId = $("servicio-mascota").value;
    if (!mascotaId) {
        toast("No hay mascotas para atender.", "error");
        return;
    }
    try {
        const resultado = await api.enviar("/api/servicios/atender", {
            metodo: "POST",
            cuerpo: { tipo, mascotaId }
        });
        const div = $("resultado-servicio");
        div.textContent = resultado;
        div.hidden = false;
    } catch (err) {
        toast(err.message, "error");
    }
});

// ============================================
// Modal general
// ============================================

$("modal-cerrar").addEventListener("click", cerrarModal);
$("modal").addEventListener("click", (e) => {
    if (e.target === $("modal")) cerrarModal();
});

// ============================================
// Inicio
// ============================================

cargarEstadisticas();
