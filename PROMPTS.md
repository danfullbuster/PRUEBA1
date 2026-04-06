# PROMPTS.md — Registro y auditoría de IA

**Requisito del enunciado:** registrar interacciones significativas con IA (objetivo, prompt textual, ajuste humano).

**Nota:** Los prompts marcados con *(reconstrucción)* resumen el alcance acordado cuando el mensaje literal no está en el hilo actual; si tu historial de Cursor conserva el texto exacto, sustituye ese bloque por copiar/pegar para cumplir al pie de la letra con “texto exacto”.

---

## Uso 1 — Implementación base de la PoC (backend, EF, tests, CI)

**1. Objetivo:**  
Levantar la prueba de concepto full stack alineada al enunciado: Web API .NET 8 con `POST`/`GET` `/api/clientes`, EF Core (In-Memory y SQL Server con migraciones), validaciones y Swagger; proyecto de pruebas con xUnit y mocks; pipeline Azure; frontend Vue 3 con formulario que consuma la API.

**2. Prompt(s) utilizado(s):**  
*(Reconstrucción basada en el alcance del repositorio; reemplazar por el/los mensaje(s) literal(es) de tu primera sesión en Cursor si los tenés.)*

> Necesito una PoC CRM “Registro de clientes y parcelas”: API .NET 8 con Entity Framework (In-Memory por defecto y SQL Server cuando haya cadena de conexión segura), controlador de clientes con validación de modelo y Swagger, tests unitarios del controlador aislados con Moq, `azure-pipelines.yml` que restaure, compile y ejecute tests, y un front Vue 3 + TypeScript con Vite, formulario con validación en cliente y proxy a la API.

**3. Ajuste humano:**  
Revisión de `Program.cs` (por ejemplo Swagger siempre disponible; redirección HTTPS condicionada al entorno para no romper el perfil HTTP local). Ajuste de tests cuando la API devolvía `ValidationProblemDetails` en lugar de un tipo de resultado distinto. Fijación del perfil de lanzamiento `http` y puerto coherente con el front. Eliminación de comentarios “tutorial” para dejar el código más limpio. Branding visible “CRM PRUEBA FULLSTACK” en Swagger y UI. Verificación manual de Swagger, `dotnet test` y flujo front + API.

---

## Uso 2 — Documentación: tono y estilo (README / guías)

**1. Objetivo:**  
Alinear el tono de la documentación con un registro informal en **tú** (español latinoamericano), evitando mezcla con voseo.

**2. Prompt(s) utilizado(s):**  

> que el tono sea mas tu

**3. Ajuste humano:**  
Unificación de conjugaciones a **tú** (p. ej. `configurás` → `configuras`, `Tenés` → `Tienes`) en `README.md` y archivos de guía; revisión final de consistencia antes de entregar.

---

## Uso 3 — Entender el enunciado (Fase 3 y EF 2.1 / 2.2)

**1. Objetivo:**  
Comprender qué exige la prueba en pruebas unitarias y en las iteraciones de base de datos In-Memory vs SQL Server, y cómo se refleja en el código.

**2. Prompt(s) utilizado(s):**  

> y esto: Fase 3: Pruebas de Unidad  
> • Crea un proyecto de pruebas (xUnit, NUnit o MSTest).  
> • Implementa pruebas unitarias para el controlador de clientes.  
> • Nota: Tus pruebas unitarias deben seguir aislando el entorno usando la base de datos In-Memory o mocks, independientemente de que la aplicación principal se haya migrado a SQL Server en la Iteración 2.2.

> y esto: Iteración 2.1 (Base de datos In-Memory): Implementa el acceso a datos utilizando Entity Framework Core configurado con el proveedor In-Memory.  
> …  
> Iteración 2.2 (Migración a SQL Server): …

**3. Ajuste humano:**  
No se generó código nuevo solo por estas preguntas: sirvieron para **validar** que el diseño actual (Moq sobre `IClientesService`; bifurcación In-Memory/SQL en `Program.cs`) cumple la consigna. Cualquier duda puntual se contrastó con los archivos reales (`ClientesControllerTests.cs`, `Program.cs`, migraciones).

---

## Uso 4 — Guía de entrega para el candidato

**1. Objetivo:**  
Tener un documento único que explique fases, entregables y checklist frente al repositorio.

**2. Prompt(s) utilizado(s):**  

> dame un documento explicando todo esto, para que lo entienda y sepa que debo hacer

**3. Ajuste humano:**  
Revisión del contenido frente al **enunciado oficial** cuando lo compartiste después: actualización de la numeración de fases (1–5), entregables y obligatoriedad de `PROMPTS.md`. El archivo vive como `GUIA_ENTREGA_PRUEBA.md` y se mantiene como apoyo, no sustituye el PDF de la empresa.

---

## Uso 5 — Contraste “¿está todo listo?” frente a la guía local

**1. Objetivo:**  
Verificar cobertura del proyecto respecto a una guía previa y detectar huecos (Azure, Git, PROMPTS, CRUD vs alcance real).

**2. Prompt(s) utilizado(s):**  

> todo ya está listo? lee todo el documento que te hice al inicio y compara

**3. Ajuste humano:**  
La comparación mostró que **código** vs **entrega** son cosas distintas: falta acción humana en Azure DevOps (repo, pipeline, URL), completar registro de IA cuando aplique, e historial de commits. También se aclaró que `guia_preguntas_prueba_analista.md` (otra carpeta) no es la consigna de esta PoC.

---

## Uso 6 — Enunciado oficial y entregable PROMPTS.md

**1. Objetivo:**  
Alinear el repositorio con el documento que envió la empresa (incluido requisito especial de auditoría de IA y lista de entregables).

**2. Prompt(s) utilizado(s):**  

> [Texto completo de la prueba técnica “Desarrollador Full Stack (Semi-Senior)” pegado en el chat, incluyendo Requisito Especial y Entregables.]

> ESTE ES EL ARCHIVO QUE ME ENVIARON

*(Hubo un mensaje previo pidiendo eliminar `PROMPTS.md` por interpretación errónea; al recibir el enunciado oficial se revirtió: el archivo es obligatorio.)*

**3. Ajuste humano:**  
Restauración de `PROMPTS.md`, actualización de `README.md` y `GUIA_ENTREGA_PRUEBA.md` para reflejar **PROMPTS.md** como entregable. Esta misma sesión: completar este archivo con el detalle de usos y prompts literales o reconstrucciones señaladas.

---

## Uso 7 — Redacción del registro de prompts para entrega

**1. Objetivo:**  
Dejar `PROMPTS.md` listo para revisión, en lenguaje claro y alineado al hilo de trabajo.

**2. Prompt(s) utilizado(s):**  

> ayudame a revisar el chat y hacer los prompts profesionales para llenar este doc

**3. Ajuste humano:**  
Sustituir el bloque *(reconstrucción)* del **Uso 1** por tus prompts literales desde Cursor si querés cumplimiento estricto de “texto exacto”. Añadir o fusionar entradas si usaste otras herramientas (Copilot en IDE, otro modelo) con interacciones significativas no listadas aquí.
