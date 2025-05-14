### Setup
- Clone The Repository
- Run Docker
- Run Ollama , use the following model:
``` bash
ollama pull mxbai-embed-large
```
- Run The Following Command. It creates a new Postgres Container accessible from port  5433
```bash
.\installVector.ps1
```
- create a new database called ```omdb```
- Run the following command ( ```Path_To_OMDB_DUMP``` refers to the path where the dump file of omdb database exists)

```bash
docker cp "Path_To_OMDB_DUMP" pgvector:/tmp/omdb.dump
```
- Restore The Database:
```bash
docker exec -it pgvector bash -c  "pg_restore -U postgres -d omdb /tmp/omdb.dump"
```

### Credits
- [Postgresql OMDB database Repo](https://github.com/credativ/omdb-postgresql)
- [pgvector](https://github.com/pgvector/pgvector)
- [pgvector for .NET](https://github.com/pgvector/pgvector-dotnet)
