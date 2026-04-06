# CRM PRUEBA FULLSTACK — PoC clientes y parcelas

**Qué hay acá:** una prueba técnica full stack — formulario en **Vue 3** (Vite + TypeScript) y **Web API .NET 8** con **EF Core**. Si no configuras SQL Server, los clientes van a **memoria** y se pierden al cerrar la API; si pones la cadena de conexión, usas **SQL Server** y al arrancar aplica las migraciones.

---

## Requisitos

| Herramienta | Para qué la usas |
|-------------|------------------|
| [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0) | Compilar la API y correr los tests |
| Node.js 20+ | `npm install` y `npm run dev` del front |
| Docker | Opcional: levantar SQL Server sin instalarlo en la máquina |

---

## Backend (API)

Desde la **raíz del repo**:

```bash
dotnet restore
dotnet run --project src/AgriculturalCrm.Api --launch-profile http
```

Por defecto queda en **http://localhost:5131** (perfil `http` en `launchSettings.json`).

- Si **`ConnectionStrings:DefaultConnection`** está vacía → **EF In-Memory** (al cortar el proceso, chau datos).
- Si la defines (user-secrets o variable de entorno) → **SQL Server** y `MigrateAsync` al inicio.

**Swagger:** [http://localhost:5131/swagger](http://localhost:5131/swagger)

### SQL Server con Docker (ejemplo)

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=TuPasswordSegura123!" -p 1433:1433 --name sql-crm -d mcr.microsoft.com/mssql/server:2022-latest
```

**No subas la cadena al repo.** Ejemplo con user-secrets:

```bash
cd src/AgriculturalCrm.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=AgriculturalCrm;User Id=sa;Password=TuPasswordSegura123!;TrustServerCertificate=True;"
```

También puedes usar la variable de entorno `ConnectionStrings__DefaultConnection`.

### Migraciones EF (si las regeneras)

Necesitas `dotnet-ef` y una instancia que el diseño pueda tocar (SQL o LocalDB). Si quieres, define `CRM_DESIGN_CONNECTION` apuntando a tu servidor.

```bash
dotnet tool install -g dotnet-ef
set CRM_DESIGN_CONNECTION=Server=localhost,1433;...
dotnet ef migrations add NombreMigracion --project src/AgriculturalCrm.Api --output-dir Data/Migrations
```

---

## Frontend (Vue)

```bash
cd frontend
npm install
npm run dev
```

Abre **http://localhost:5173**. Vite hace **proxy** de `/api` a **http://localhost:5131** (mira `vite.config.ts`). Tienes que tener la API arriba para que el formulario no falle al enviar.

---

## Pruebas unitarias

```bash
dotnet test
```

Los tests usan **Moq** sobre el controlador; no tocan SQL real.

---

## CI — Azure Pipelines

En la raíz está **`azure-pipelines.yml`**. En Azure DevOps: nuevo pipeline → YAML existente → ese archivo. En push a **`main`** o **`master`**: restore, build y test (agente Ubuntu con SDK .NET 8).

---

## Entrega (checklist)

| Ítem | Qué tienes que hacer |
|------|---------------------|
| Repositorio | Sube el repo a **Azure Repos** y dale acceso de lectura a quien revise |
| Contenido | Vue + .NET + `azure-pipelines.yml` + `PROMPTS.md` (lo exige el enunciado) |
| Historial | Commits que se entiendan por fases (abajo un ejemplo) |
| IA | Completa `PROMPTS.md`: objetivo, prompt exacto y ajuste humano por cada uso significativo |

**Ideas de mensajes de commit:**

1. `feat(api): EF In-Memory, POST/GET clientes, Swagger`  
2. `feat(api): SQL Server, migraciones, cadena segura`  
3. `test: unitarias del controlador con mocks`  
4. `feat(ui): formulario Vue y consumo REST`  
5. `ci: pipeline Azure`  
6. `docs: README y PROMPTS.md`

---

## Subir el repositorio a Azure Repos

1. En **Azure DevOps** → tu proyecto → **Repos** → **Files** → copia la URL **HTTPS** o **SSH** del repo vacío (o crea el proyecto si aún no existe).
2. En la **raíz de esta carpeta** (`crm-parcelas-poc`):

```bash
git remote add origin https://dev.azure.com/<org>/<proyecto>/_git/<repo>
git push -u origin main
```

Si Azure te pide autenticación, usa un **Personal Access Token (PAT)** como contraseña o configura **Git Credential Manager**.

3. En **Pipelines** → **New pipeline** → elige el repo → **Existing Azure Pipelines YAML file** → rama `main` → `/azure-pipelines.yml` → ejecutá una vez para validar.
4. Entrega la **URL del repositorio** y asegúrate de que los revisores tengan **lectura** (o el repo sea accesible según indiquen).
