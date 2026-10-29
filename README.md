# shared-pipelines-test

A deliberately small .NET solution that exists to verify the reusable workflows
in [bjornbouetsmith/shared-pipelines](https://github.com/bjornbouetsmith/shared-pipelines).

| Path | Purpose |
|---|---|
| `src/SharedPipelines.Sample` | Packable class library (`SharedPipelines.Sample`, version `0.1.0`). |
| `tests/SharedPipelines.Sample.Tests` | MSTest suite for the library. |
| `.github/workflows/ci.yml` | 19 line caller for `dotnet-ci-build.yml`. |
| `.github/workflows/release.yml` | 26 line caller for `release-nuget.yml`. |
| `.github/workflows/increment-version.yml` | 47 line caller for `increment-version.yml`. |

No build, test, pack, publish or versioning logic lives in this repository: every
workflow body is a `uses:` reference pinned to `@v1` of the shared repository.

## Local commands

```bash
dotnet build SharedPipelines.Sample.slnx
dotnet test SharedPipelines.Sample.slnx
```

## What the pipelines exercise here

- **CI** — auto-discovery is bypassed by passing `solution` explicitly; SDK
  `10.0.x`; NuGet cache; `dotnet test` with the MSTest suite.
- **Release** — `dotnet pack` of the packable project, GitHub Packages push into
  the `bjornbouetsmith` feed, artifact upload and the `allowed-actors`
  allow-list.
- **Increment version** — `prepare` bumps `Directory.Build.props`, builds and
  tests the release branch and opens a release PR; `tag` creates `v<version>`
  for the merged commit.
