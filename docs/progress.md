# ilerleme

## adım 1 - tasarım (28.10)

- yapıldı: `docs/design.md` yazıldı. kapsam, veri modeli, endpoint'ler, mimari kararlar ve 18 adımlık yol haritası.
- öğrenildi: kodlamadan önce "bilerek dışarıda" listesini yazmak kapsamın büyümesini engelliyor.
- kaldı: yok.

## adım 2 - iskelet, ci, angular (28.10)

- yapıldı: `FinTrack.slnx` ile domain / application / infrastructure / api ve iki test projesi. `GET /health` ve onu `WebApplicationFactory` ile doğrulayan integration test. github actions'ta backend (restore, build, test) ve frontend (npm ci, build, test) job'ları. `frontend/` altında boş angular 21 projesi.
- öğrenildi: `Directory.Build.props` ortak ayarları tek yerde topluyor; `global.json` + `rollForward` sdk'yı sabitliyor. `.gitattributes` satır sonu sorununu `core.autocrlf`'ten bağımsız çözüyor. npm 10.9.2'de vitest peer bağımlılıklarında bir arborist hatası var; npm 11'e geçilerek çözüldü.
- kaldı: unit test projesi şimdilik boş, ilk testler parser ve dedup ile gelecek (adım 5 ve 14).

## adım 3 - entity'ler, ef core, postgres (28.10)

- yapıldı: 12 domain entity + 6 enum. `FinTrackDbContext`, entity başına `IEntityTypeConfiguration`, snake_case isimlendirme. `compose.yaml` ile postgres 18 (5433 portu). `dotnet-ef` local tool manifest ile 10.0.14'e sabit. `InitialCreate` migration'ı uygulandı: 12 tablo, 8 unique index, 13 sistem kategorisi.
- öğrenildi: tekrar engeli (`dedupHash`, `fileHash`, kur) unique index ile veritabanında garanti; denendi, ikinci kayıt reddediliyor. `HasData` için sabit id gerekiyor, yoksa her migration seed'i yeniden yazar. kararlar `docs/decisions/001-veri-modeli-kurallari.md`'de.
- kaldı: global query filter (userId) adım 9'da. migration'ların testcontainers ile test edilmesi adım 15'te.
