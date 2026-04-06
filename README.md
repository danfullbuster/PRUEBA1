# Agricultural CRM PoC

```bash
dotnet restore
dotnet run --project src/AgriculturalCrm.Api --launch-profile http
```

API en **http://localhost:5131** (perfil `http`). Swagger: **http://localhost:5131/swagger**.

- Sin cadena de conexión → EF **In-Memory** (los datos se pierden al cerrar la API).
- Con cadena → **SQL Server** y migraciones al iniciar.

### SQL Server con Docker

Elige una **contraseña fuerte solo para tu máquina** (no la copies del README ni la subas a Git). Sustituye `<TU_PASSWORD_SA>` en ambos sitios por el mismo valor.

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=<TU_PASSWORD_SA>" -p 1433:1433 --name sql-crm -d mcr.microsoft.com/mssql/server:2022-latest
```

La cadena de conexión va en **user-secrets** (o en variables de entorno), nunca en archivos del repo:

```bash
cd src/AgriculturalCrm.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=AgriculturalCrm;User Id=sa;Password=<TU_PASSWORD_SA>;TrustServerCertificate=True;"
```

Alternativa: variable de entorno `ConnectionStrings__DefaultConnection` con el mismo criterio (valor local, no en Git).

```bash
cd frontend
npm install
npm run dev
```

```bash
dotnet test
```
