# DockerDemo — ASP.NET Core + SQL Server, containerized

A minimal, working example to practice Docker fundamentals on your own stack
(C# / ASP.NET / SQL Server) before a technical interview.

## What's in here

- `Dockerfile` — multi-stage build: compiles the app in an SDK image, then
  copies just the published output into a smaller runtime image.
- `docker-compose.yml` — runs the web app *and* a SQL Server container
  together, networked so they can talk to each other by service name.
- `Program.cs` / `DockerDemo.csproj` — a tiny ASP.NET Core app with two
  endpoints: `/` (proves the container is running) and `/db-check` (proves
  it can reach the SQL Server container).

## Prerequisites

Install Docker Desktop, then confirm it works:

```bash
docker run hello-world
```

## Run it

From this folder:

```bash
docker compose up --build
```

This does three things worth understanding, in order:
1. Builds the `webapp` image from the `Dockerfile`.
2. Pulls the official SQL Server image (first run only — cached after that).
3. Starts both containers on a shared network Docker creates automatically.

Once it's running, open:
- http://localhost:8080/ — should say the app is running.
- http://localhost:8080/db-check — should return the SQL Server version,
  proving the two containers connected successfully.

Stop everything with `Ctrl+C`, then `docker compose down` to remove the
containers (add `-v` to also delete the SQL Server data volume).

## Useful commands to practice while this is running

```bash
docker ps                     # see both containers running
docker logs <container-id>    # view a container's output/logs
docker exec -it <container-id> bash   # get a shell inside a running container
docker images                 # see the images you've built/pulled
```

## Why this setup, specifically

This mirrors a realistic version of what a C#/.NET + SQL Server engineer
would actually containerize day to day — not a toy "hello world" — so you
have a concrete, honest story for an interview: "I built a multi-stage
Dockerfile for an ASP.NET app and wired it to a SQL Server container with
docker-compose."
