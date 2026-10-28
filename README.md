# fintrack

ai destekli kişisel finans api'si. banka dökümünü (csv/excel) yüklersin; sistem işlemleri okur, tekrarları ayıklar, kategorize eder (önce kurallar, sonra ai), dövizi tl'ye çevirir, bütçe ve rapor sunar.

> geliştirme aşamasında. ayrıntılı tasarım: [`docs/design.md`](docs/design.md), ilerleme: [`docs/progress.md`](docs/progress.md).

## teknoloji

.net 10 (asp.net core minimal api), ef core 10 + postgresql, hybridcache + redis, microsoft.extensions.ai, xunit + testcontainers, angular 21, docker, github actions.

## yapı

```
src/
  FinTrack.Domain           entity'ler ve iş kuralları, hiçbir projeye bağlı değil
  FinTrack.Application      use case'ler, dto'lar, arayüzler
  FinTrack.Infrastructure   veritabanı, cache, dış servisler
  FinTrack.Api              minimal api uçları
tests/
  FinTrack.UnitTests
  FinTrack.IntegrationTests
frontend/                   angular
docs/
```

referans yönü: domain ← application ← infrastructure ← api.

## nasıl çalıştırılır

gereksinimler: .net sdk 10.0.4xx, node 22, npm 11.

```bash
# backend
dotnet build
dotnet test
dotnet run --project src/FinTrack.Api   # http://localhost:5155/health

# frontend
cd frontend
npm ci
npm start                               # http://localhost:4200
npm test -- --watch=false
```
