# Agricultural CRM PoC

```bash
dotnet restore
dotnet run --project src/AgriculturalCrm.Api --launch-profile http
```

API en **http://localhost:5131** (perfil `http`). Swagger: **http://localhost:5131/swagger**.

- Sin cadena de conexión → EF **In-Memory** (los datos se pierden al cerrar la API).
- Con cadena → **SQL Server** y migraciones al iniciar.

### SQL Server con Docker

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=TuPasswordSegura123!" -p 1433:1433 --name sql-crm -d mcr.microsoft.com/mssql/server:2022-latest
```

No subas la cadena al repositorio. En desarrollo, **user-secrets**:

```bash
cd src/AgriculturalCrm.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=AgriculturalCrm;User Id=sa;Password=TuPasswordSegura123!;TrustServerCertificate=True;"
```

Alternativa: variable de entorno `ConnectionStrings__DefaultConnection`.

```bash
cd frontend
npm install
npm run dev
```

```bash
dotnet test
```
