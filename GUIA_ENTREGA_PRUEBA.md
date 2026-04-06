# Guía de la prueba full stack — alineada al enunciado oficial

Este archivo **complementa** el documento de la empresa (*Prueba Técnica: Desarrollador Full Stack Semi-Senior*). **Manda el PDF/mail de ellos**; aquí solo mapeamos fases y checklist contra **este repositorio**. Comandos (puertos, Docker, user-secrets): [README.md](README.md).

---

## 1. Qué evalúan (resumen del enunciado)

- SPA **Vue** (u otro JS/TS) + **Web API .NET 8**.
- Código **mantenible** y bien organizado.
- Código en **Azure Repos** + **CI en Azure Pipelines**; revisan **historial de commits**.
- **Requisito especial:** [PROMPTS.md](PROMPTS.md) en la raíz — por cada uso significativo de IA: **Objetivo**, **Prompt(s) utilizado(s)** (texto exacto), **Ajuste humano**. Los **entregables** del enunciado lo listan junto a Vue, .NET y `azure-pipelines.yml`.

---

## 2. Fases oficiales (numeración del enunciado)

| Fase | Contenido del enunciado | En este repo |
|------|-------------------------|--------------|
| **1** | Repositorio en **Azure DevOps**; todo el código ahí; commits que muestren el proceso | Lo hacés vos (URL + permisos de lectura). El código fuente está en esta carpeta; falta **subir** y commitear con historia clara. |
| **2** | `POST /api/clientes`, `GET /api/clientes`, validaciones, **Swagger**; **2.1** In-Memory + commit; **2.2** SQL Server + cadena segura + migraciones + commit | Implementado: controlador, EF In-Memory si cadena vacía, SQL + `MigrateAsync` si hay cadena, migraciones en `Data/Migrations/`. |
| **3** | Proyecto de tests (xUnit/NUnit/MSTest), tests del **controlador de clientes**, aislados (In-Memory en test o **mocks**) | `tests/AgriculturalCrm.Api.Tests` con **xUnit + Moq**. |
| **4** | Vue **3** + Composition API recomendado; formulario **responsivo**; validación en cliente; consumo REST; feedback al usuario | `frontend/` con Vue 3, `<script setup>`, validación y mensajes de éxito/error. |
| **5** | `azure-pipelines.yml` en la raíz; pipeline en Azure que en push a **rama principal**: restore, **build backend**, **tests** | El YAML está en la raíz; falta **crear el pipeline** en Azure DevOps apuntando a ese archivo. |

En conjunto, el **código de la PoC** cubre las fases **2–5** del enunciado; la **fase 1** y parte de la **5** son acciones **en Azure + Git** que solo vos podés cerrar.

---

## 3. Iteración 2.1 — Base de datos In-Memory

### Qué piden

- Acceso a datos con **EF Core** y proveedor **`UseInMemoryDatabase`**.
- Objetivo: **controlador y lógica funcionando rápido** sin instalar SQL.
- Suele pedirse que eso quede reflejado en un **commit específico** del historial.

### Qué implica técnicamente

- Un `DbContext` con tus entidades (aquí: `CrmDbContext`, `Cliente`).
- Registro en DI: `AddDbContext` + `options.UseInMemoryDatabase("unNombre")`.
- Paquete `Microsoft.EntityFrameworkCore.InMemory`.

### Cómo está en este proyecto

- En `appsettings.json`, `ConnectionStrings:DefaultConnection` está **vacía**.
- En `Program.cs`, si la cadena está vacía, se registra el contexto con **In-Memory**.

**Cómo probarlo:** levantá la API sin definir cadena (ver [README.md](README.md)), usá Swagger o el front: los datos **se pierden** al cerrar el proceso.

---

## 4. Iteración 2.2 — Migración a SQL Server

### Qué piden

- Misma aplicación, pero persistencia en **SQL Server real**.
- **Cadena de conexión segura** (no commitear usuario/clave en texto claro en el repo).
- **Migraciones de EF** que creen/actualicen el esquema.
- SQL local recomendado con **Docker**.
- Otro **commit** (o conjunto de commits) que muestre esa evolución.

### Qué implica técnicamente

- Paquete `Microsoft.EntityFrameworkCore.SqlServer` y `UseSqlServer(connectionString)`.
- Archivos generados por `dotnet ef migrations add ...` (en este repo: carpeta `Data/Migrations/`).
- Al arrancar (o en despliegue), aplicar migraciones: aquí se usa `MigrateAsync()` **solo si** la base es relacional (`IsRelational()`), para no romper In-Memory.

### Cómo está en este proyecto

- Si configurás `DefaultConnection` (por **user-secrets** o variable de entorno `ConnectionStrings__DefaultConnection`), la API usa **SQL Server** y aplica migraciones al iniciar.
- Instrucciones Docker y user-secrets: [README.md](README.md).

**Cómo probarlo:** levantá SQL con Docker, configurá la cadena con user-secrets, reiniciá la API y verificá en Swagger que los datos **persisten** tras reiniciar.

---

## 5. Fase 3 — Pruebas unitarias

### Qué piden

- Proyecto de tests (**xUnit**, NUnit o MSTest).
- Pruebas del **controlador de clientes** (o el que indiquen).
- **Aislamiento:** In-Memory **dentro del test** o **mocks**, de modo que **no dependan** de SQL aunque la app principal use SQL Server.

### Dos formas válidas

1. **Mocks (lo que usa este repo):** el controlador depende de una interfaz (`IClientesService`). En el test creás un `Mock<IClientesService>`, definís respuestas y verificás códigos HTTP y que se llamó al servicio. **No hay base de datos en el test.**

2. **EF In-Memory en el test:** tiene sentido si probás repositorios o servicios que usan `DbContext` directamente; configurás un segundo `DbContext` con `UseInMemoryDatabase` solo para tests.

### Dónde está en este proyecto

- Proyecto: `tests/AgriculturalCrm.Api.Tests/`
- Archivo principal: `ClientesControllerTests.cs` (xUnit + Moq).

**Comando:** desde la raíz del repo, `dotnet test`.

---

## 6. Historial de Git (commits por iteración)

La consigna suele decir: **un commit para 2.1 y otro para 2.2** (y a veces uno por fase).

**Qué evalúan:** que se entienda la evolución (API → In-Memory → SQL → tests → CI → docs), no que tengas cien commits vacíos.

**Si ya tenés todo en un solo estado** (como un `Program.cs` que elige In-Memory o SQL según la cadena):

- Es **válido** funcionalmente: demostrás 2.1 corriendo sin cadena y 2.2 con cadena + Docker.
- Para el **historial**, podés usar mensajes claros por tema, por ejemplo: `feat(api): EF In-Memory y CRUD clientes`, luego `feat(api): SQL Server, migraciones y cadena segura`, etc. Si en algún momento te piden commits “puros” por iteración, habría que reorganizar con cuidado (`git rebase`) o recrear la secuencia; no es obligatorio si el enunciado solo pide “commit específico” de forma general.

**Ideas de mensajes** (también en [README.md](README.md)):

1. API + EF In-Memory + Swagger  
2. SQL Server + migraciones  
3. Tests unitarios con mocks  
4. Front Vue  
5. Pipeline Azure  
6. README + PROMPTS.md  

---

## 7. CI (Azure Pipelines)

- En la raíz: `azure-pipelines.yml`.
- En Azure DevOps: crear pipeline desde ese YAML; en push a `main` o `master` debería **restaurar, compilar y ejecutar tests**.

Verificá que el pipeline use **.NET 8** y que `dotnet test` pase en limpio.

---

## 8. IA y `PROMPTS.md` (requisito especial — entregable obligatorio)

El documento que te enviaron exige **PROMPTS.md** en la raíz. Por cada interacción significativa con IA:

1. **Objetivo**  
2. **Prompt(s) utilizado(s)** — texto exacto  
3. **Ajuste humano** — qué cambiaste después  

Rellená [PROMPTS.md](PROMPTS.md) antes de entregar la URL del repo (no inventes prompts: copiá del chat).

---

## 9. Checklist antes de entregar (entregables del enunciado)

**Entregables explícitos**

- [ ] **URL del repositorio en Azure DevOps** con permiso de lectura (o público).
- [ ] En el repo: código **Vue**, código **.NET**, **`azure-pipelines.yml`**, **`PROMPTS.md`**.
- [ ] **`README.md` corto**: cómo levantar local (incluye comando Docker SQL si aplica).

**Verificación técnica**

- [ ] `dotnet build` y `dotnet test` pasan en tu máquina.
- [ ] API en **In-Memory**: sin cadena, creás un cliente y listás (Swagger o UI).
- [ ] API en **SQL**: Docker + cadena segura (user-secrets/env), migraciones, datos persisten tras reinicio.
- [ ] Front: `npm run dev`, proxy a la API, formulario con validación cliente + feedback al usuario.
- [ ] Swagger documenta la API.
- [ ] Sin secretos en archivos trackeados.
- [ ] Pipeline en Azure DevOps configurado; push a rama principal ejecuta restore, build backend, tests.
- [ ] Historial de commits **legible**; ideal: **un commit para iteración 2.1** y **otro para 2.2** (como pide el enunciado).

---

## 10. Dónde está cada cosa en el repo

| Tema | Ubicación principal |
|------|---------------------|
| API, In-Memory / SQL, migraciones al arranque | `src/AgriculturalCrm.Api/Program.cs` |
| Modelo EF | `src/AgriculturalCrm.Api/Data/CrmDbContext.cs` |
| Migraciones | `src/AgriculturalCrm.Api/Data/Migrations/` |
| Controlador clientes | `src/AgriculturalCrm.Api/Controllers/ClientesController.cs` |
| Lógica de negocio / acceso datos | `src/AgriculturalCrm.Api/Services/` |
| Tests | `tests/AgriculturalCrm.Api.Tests/` |
| Front Vue | `frontend/` |
| CI | `azure-pipelines.yml` |
| Comandos y Docker | [README.md](README.md) |
| Registro de IA (entregable) | [PROMPTS.md](PROMPTS.md) |

---

## 11. Si algo de la consigna no coincide al 100%

El enunciado **oficial** de la empresa manda sobre esta guía. Usá este archivo como **mapa**; si te piden un endpoint extra, otro nombre de rama o otro proveedor de CI, adaptá el código y actualizá README / esta guía solo si te sirve para vos.

---

*Documento orientado a la PoC **CRM PRUEBA FULLSTACK** en este repositorio.*
