# ilerleme

## adım 1 - tasarım (28.10)

- yapıldı: `docs/design.md` yazıldı. kapsam, veri modeli, endpoint'ler, mimari kararlar ve 18 adımlık yol haritası.
- öğrenildi: kodlamadan önce "bilerek dışarıda" listesini yazmak kapsamın büyümesini engelliyor.
- kaldı: yok.

## adım 2 - iskelet, ci, angular (28.10)

- yapıldı: `FinTrack.slnx` ile domain / application / infrastructure / api ve iki test projesi. `GET /health` ve onu `WebApplicationFactory` ile doğrulayan integration test. github actions'ta backend (restore, build, test) ve frontend (npm ci, build, test) job'ları. `frontend/` altında boş angular 21 projesi.
- öğrenildi: `Directory.Build.props` ortak ayarları tek yerde topluyor; `global.json` + `rollForward` sdk'yı sabitliyor. `.gitattributes` satır sonu sorununu `core.autocrlf`'ten bağımsız çözüyor. npm 10.9.2'de vitest peer bağımlılıklarında bir arborist hatası var; npm 11'e geçilerek çözüldü.
- kaldı: unit test projesi şimdilik boş, ilk testler parser ve dedup ile gelecek (adım 5 ve 14).
