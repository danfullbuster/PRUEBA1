# Registro de uso de asistentes de IA — Prueba técnica Full Stack

Documento de **transparencia** sobre el empleo de herramientas de IA (Cursor y modelos asociados) durante el desarrollo de la prueba de concepto. Estructura por uso: **objetivo**, **prompt(s) utilizado(s)** e **intervención humana posterior**.

---

## Alcance y fuente

- **Contexto inicial:** La implementación base se orientó con el **enunciado íntegro** de la prueba (*Desarrollador Full Stack Semi-Senior*, CRM agrícola, fases y entregables), facilitado al inicio del hilo de trabajo en Cursor.
- **Entradas siguientes:** Corresponden a **interacciones puntuales** posteriores (documentación, modelo de datos, pruebas y revisión de cierre del registro y del repositorio).
- **Trazabilidad:** [Sesión de trabajo — PoC CRM](fbaa3a8e-8f3e-4353-b256-ba54840bbfb2).

*Los prompts del apartado 2 están redactados en **lenguaje profesional** para el registro de entrega; cuando el mensaje en el chat fue informal, se indica el **original entre paréntesis** al final de cada ítem.*

---

## Uso 1 — Documentación y preparación de entrega

**1. Objetivo:** Homogeneizar el tono de la documentación, producir guías de entrega comprensibles y, en fases posteriores, completar el README con SQL Server en Docker, user-secrets y placeholders (sin credenciales reales en el repositorio).

**2. Prompt(s) utilizado(s):**

1. Pedido de ajustar la documentación a un **tono más natural o reconocible**, menos genérico. *(Original: «que el tono sea mas tu».)*
2. Solicitud de un **documento guía** que explique la consigna y los pasos a seguir, orientado a quien debe ejecutar la entrega. *(Original: «dame un documento explicando todo esto, para que lo entienda y sepa que debo hacer».)*
3. Petición de **revisar el historial del chat** y redactar de forma ordenada los prompts a incluir en este registro. *(Original: «ayudame a revisar el chat y hacer los prompts profesionales para llenar este doc».)*
4. Instrucciones para **ampliar el README** con contenedor SQL Server, user-secrets y marcadores en lugar de credenciales reales. *(Reconstrucción a partir de la misma línea de trabajo en el hilo; no hay un único mensaje literal conservado.)*

**3. Ajuste humano:** Revisión y unificación de estilo; reorganización de secciones; incorporación de feedback; validación final de comandos, secretos y buenas prácticas antes de publicar.

---

## Uso 2 — Persistencia (iteraciones 2.1 y 2.2) y pruebas aisladas (Fase 3)

**1. Objetivo:** Alinear la solución con la iteración 2.1 (EF Core In-Memory), la 2.2 (SQL Server, cadena segura, migraciones, Docker sugerido) y la Fase 3 (pruebas unitarias del controlador con In-Memory o mocks, independientes del motor SQL de la aplicación principal).

**2. Prompt(s) utilizado(s):**

**A) Iteraciones 2.1 y 2.2**

Solicitud de **interpretación e implementación** conforme al fragmento del enunciado oficial pegado en el chat, introducido de forma coloquial («y esto:») seguido del texto de la prueba. Versión consolidada del pedido:

> Confírmame cómo debe aplicarse en el proyecto lo siguiente:
>
> **Iteración 2.1 (Base de datos In-Memory):** Implementa el acceso a datos utilizando Entity Framework Core configurado con el proveedor In-Memory. El objetivo es tener el controlador y la lógica funcional rápidamente. El código debe almacenarse en el repositorio en un commit específico.
>
> **Iteración 2.2 (Migración a SQL Server):** Una vez que la versión In-Memory funcione, modifica el proyecto para utilizar un motor real de SQL Server (te sugerimos levantarlo localmente usando Docker). Debes configurar la cadena de conexión de forma segura y generar las Migraciones de Entity Framework correspondientes para crear el esquema de base de datos. El código debe almacenarse en el repositorio en un commit específico.

*(El pegado original presentaba saltos de línea irregulares; el contenido coincide con el transcript.)*

**B) Fase 3 — Pruebas unitarias**

> ¿Cómo debo implementar lo siguiente? *(En el chat: «cómo se hace esto ?»)*
>
> **Fase 3: Pruebas de Unidad**
> - Crea un proyecto de pruebas (xUnit, NUnit o MSTest).
> - Implementa pruebas unitarias para el controlador de clientes.
> - **Nota:** Tus pruebas unitarias deben seguir aislando el entorno usando la base de datos In-Memory o mocks, independientemente de que la aplicación principal se haya migrado a SQL Server en la Iteración 2.2.

**3. Ajuste humano:** Bifurcación en `Program.cs` (cadena vacía → In-Memory; cadena definida → SQL Server); `MigrateAsync()` condicionado a `IsRelational()`; migraciones bajo `Data/Migrations`; fábrica de contexto para diseño con `CRM_DESIGN_CONNECTION`; pruebas del controlador con Moq sin dependencia de motor relacional en el proyecto de test; revisión de modelo e índices.

---

## Uso 3 — Revisión del registro de IA y del proyecto completo

**1. Objetivo:** **Me informaron que faltaba algo** en la entrega o en la documentación de uso de IA; por esa razón se abordó una **revisión de cierre** para completar el registro (`PROMPTS.md`), contrastarlo con el trabajo real y comprobar que el **repositorio en su conjunto** cumple la consigna.

**2. Prompt(s) utilizado(s):**

1. **Revisión integral motivada por feedback:** solicitud de revisar este documento (completitud de los usos declarados, redacción y trazabilidad) y de **repasar todo el proyecto** —estructura, código, pruebas, pipelines, README y demás documentación— para **cubrir lo señalado como pendiente** y detectar otras inconsistencias o lagunas frente al enunciado.

*(Original en el chat: «pon en el punto 4 que me informaron que faltaba algo, y por eso lo hice». En la versión actual del archivo este apartado es el **Uso 3**.)*

**3. Ajuste humano:** Respuesta al requerimiento recibido (incorporación de lo faltante en documentación o en el repo); lectura y validación manual del registro; recorrido del árbol del repositorio; ejecución o verificación de build, pruebas y CI según checklist de entrega; criterio final sobre qué adoptar de las sugerencias del asistente.

