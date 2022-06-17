# Minimal API

Use a minimal API with a Postgres database or an in-memory database (when no Postgres database is found).

<br>

### Docker Run

Run in docker container:
```
docker run -d --name minimal-api \
--network host \
-e ConnectionStrings__DefaultConnection=<ConnectionString> \
-d vitormoschetta/todo.minimal.api
```

<br>

### Docker Compose

Run in docker container via docker-compose:
```
docker-compose up -d --build
```

<br>


### Route documentation (swagger):

<http://localhost:6001/swagger/index.html>





