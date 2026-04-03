# CRM agrícola — PoC clientes y parcelas

SPA **Vue 3** (Vite + TypeScript) + **Web API .NET 8** con **EF Core** (In-Memory si no hay cadena de conexión; **SQL Server** en producción/local con Docker).

## Requisitos

- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- Node.js 20+ (para el frontend)
- Docker (opcional, para SQL Server local)

## Backend

Desde la raíz del repositorio:

```bash
dotnet restore
dotnet run --project src/AgriculturalCrm.Api
```

Por defecto la API usa **base en memoria** si `ConnectionStrings:DefaultConnection` está vacía. Swagger en desarrollo: `http://localhost:5131/swagger`.

### SQL Server con Docker

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=TuPasswordSegura123!" -p 1433:1433 --name sql-crm -d mcr.microsoft.com/mssql/server:2022-latest
```

Configure la cadena de conexión **sin commitear secretos**:

```bash
cd src/AgriculturalCrm.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=AgriculturalCrm;User Id=sa;Password=TuPasswordSegura123!;TrustServerCertificate=True;"
```

También puede usar la variable de entorno `ConnectionStrings__DefaultConnection`.

Al arrancar la API con SQL Server, se aplican las migraciones automáticamente (`MigrateAsync`).

### Migraciones EF (opcional)

Si necesita regenerar migraciones (requiere `dotnet-ef` y SQL Server o LocalDB para diseño):

```bash
dotnet tool install -g dotnet-ef
# Opcional: apuntar el diseño a su instancia
set CRM_DESIGN_CONNECTION=Server=localhost,1433;...
dotnet ef migrations add NombreMigracion --project src/AgriculturalCrm.Api --output-dir Data/Migrations
```

## Frontend

```bash
cd frontend
npm install
npm run dev
```

Abra `http://localhost:5173`. El proxy de Vite reenvía `/api` al backend en `http://localhost:5131`.

## Pruebas

```bash
dotnet test
```

Las pruebas del controlador usan **Moq** (sin SQL real).

## CI (Azure Pipelines)

En la raíz está `azure-pipelines.yml`. Cree un pipeline en Azure DevOps apuntando a ese archivo; en push a `main` o `master` restaura, compila y ejecuta tests.

## Entregables de la prueba técnica

- Suba este repositorio a **Azure Repos** y comparta la URL (lectura para revisores).
- Mantenga un historial de commits claro (véase sugerencia abajo).
- Complete `PROMPTS.md` con cada uso significativo de IA durante su trabajo.

### Sugerencia de commits (fases)

1. `feat(api): iteración 2.1 — EF In-Memory, POST/GET clientes, Swagger`  
2. `feat(api): iteración 2.2 — SQL Server, migraciones, cadena segura`  
3. `test: pruebas unitarias del controlador con mocks`  
4. `feat(ui): formulario Vue 3 y consumo del API`  
5. `ci: azure-pipelines.yml`  
6. `docs: README y PROMPTS.md`
