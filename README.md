# shared-pipelines-test

A deliberately small .NET solution that exists to verify the reusable workflows
in [bjornbouetsmith/shared-pipelines](https://github.com/bjornbouetsmith/shared-pipelines).

| Path | Purpose |
|---|---|
| `src/SharedPipelines.Sample` | Packable class library (`SharedPipelines.Sample`, version `0.1.0`). |
| `tests/SharedPipelines.Sample.Tests` | MSTest suite for the library. |
| `docker/SharedPipelines.Docker.slnx` | Separate solution used only by the Docker-aware pipeline. |
| `docker/docker-compose.yml` | nginx service that the Docker integration test talks to. |
| `docker/src/SharedPipelines.DockerSample` | HTTP probe used by the Docker integration test. |
| `docker/tests/SharedPipelines.DockerSample.Tests` | Integration test that requires the compose service. |
| `.github/workflows/ci.yml` | 19 line caller for `dotnet-ci-build.yml`. |
| `.github/workflows/docker-ci.yml` | Caller for `dotnet-docker-ci-build.yml`. |
| `.github/workflows/release.yml` | 26 line caller for `release-nuget.yml`. |
| `.github/workflows/increment-version.yml` | 47 line caller for `increment-version.yml`. |

No build, test, pack, publish or versioning logic lives in this repository: every
workflow body is a `uses:` reference pinned to `@v1` of the shared repository.

## Local commands

```bash
dotnet build SharedPipelines.Sample.slnx
dotnet test SharedPipelines.Sample.slnx
```

The Docker sample needs a running service:

```bash
docker compose -f docker/docker-compose.yml up -d
SERVICE_URL=http://127.0.0.1:8080/ dotnet test docker/SharedPipelines.Docker.slnx
docker compose -f docker/docker-compose.yml down -v
```

## What the pipelines exercise here

- **CI** — auto-discovery is bypassed by passing `solution` explicitly; SDK
  `10.0.x`; NuGet cache; `dotnet test` with the MSTest suite.
- **Docker CI** — starts `docker/docker-compose.yml` with a run-specific compose
  project name, waits for `http://127.0.0.1:8080/`, exports `SERVICE_URL` to the
  test process, writes TRX files to `TestResults`, uploads them as an artifact
  and always stops the stack afterwards.
- **Release** — `dotnet pack` of the packable project, GitHub Packages push into
  the `bjornbouetsmith` feed, artifact upload and the `allowed-actors`
  allow-list.
- **Increment version** — `prepare` bumps `Directory.Build.props`, builds and
  tests the release branch and opens a release PR; `tag` creates `v<version>`
  for the merged commit.

All four workflows are pinned to `v1.3.0`, the first release that contains
`dotnet-docker-ci-build.yml` and the Docker Compose actions. The existing CI,
release and version workflows were moved to the same tag so every workflow and
the composite actions it uses come from one release.
