
## WebApplication1 Backend

### Что это
Back-end часть интернет магазина.

### Как запустить?
1. Склонировать этот репозиторий
2. Переименовать файл ```.env.template``` в ```.env``` и изменить находящийся в нем пароль
3. Выполнить команду ```docker-compose up -d```
4. Бекенд будет доступен на http://localhost:8001/swagger

### Необходимое программное обеспечение
1. Git ([ссылка](https://git-scm.com/downloads))
2. Docker ([ссылка](https://www.docker.com/))

### Используемые технологии
1. ASP.NET Core 6 WebAPI
2. EFCore (Code-first)
3. PostgreSQL (SQL Database)
4. AutoMapper (Mapping)
5. xUnit (Testing)
6. AutoBogus (Tests)
7. BenchmarkDotNet
