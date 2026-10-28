# 001 - veri modeli kuralları

- durum: kabul edildi
- tarih: 28.10.2026 (adım 3)

## bağlam

12 entity'lik ilk şema yazılırken id, para, tarih, enum ve isimlendirme için her tabloda aynı kararın tekrar verilmesi gerekiyordu. kararlar baştan sabitlenmezse tablolar arasında tutarsızlık ve sonradan pahalı migration'lar çıkar. ayrıca tasarımda `fileHash` ve `dedupHash` için "tekil" denmiş ama kapsamı (global mi, kullanıcı/hesap başına mı) belirtilmemişti.

## karar

| konu | karar |
| --- | --- |
| id | `Guid.CreateVersion7()`, uygulamada üretilir |
| para | `decimal`, `numeric(19,4)`; kur `numeric(18,8)` |
| para birimi | iso 4217 kodu, `character(3)` |
| gün bilgisi | `DateOnly` → `date` (işlem tarihi, kur tarihi, ay) |
| an bilgisi | `DateTimeOffset` utc → `timestamptz` |
| enum | string olarak, `varchar(32)` |
| isimlendirme | `snake_case` (`EFCore.NamingConventions`) |
| ilişkiler | sadece foreign key, navigation property yok |
| fileHash | `(userId, fileHash)` unique |
| dedupHash | `(accountId, dedupHash)` unique |

## alternatifler

- **int / bigint identity id:** küçük ve hızlı ama id veritabanına gitmeden bilinmez; sıralı olduğu için kayıt sayısı dışarı sızar. **guid v4:** rastgele olduğu için b-tree index'te sayfa bölünmesine yol açar. v7 ikisinin artısını birleştirir.
- **`double` para:** ikilik kayan nokta, kuruş hatası üretir. reddedildi.
- **`DateTime` her yerde:** `Kind` belirsizliği ve saat dilimi kayması (gün bilgisi gece yarısında bir önceki güne düşebilir). reddedildi.
- **enum sayı olarak:** daha az yer kaplar ama sql'de okunmaz; enum sırası değişirse eski kayıtların anlamı kayar.
- **pascalcase isimler:** postgres'te her sorguda tırnak gerektirir.
- **navigation property'ler:** okumalar dto projection ile yapıldığı için gereksiz; yanlışlıkla nesne grafiği yükleme riskini getirir.
- **global unique fileHash:** iki kullanıcının aynı dosyayı yüklemesi meşru; global unique birini engellerdi.

## sonuç

- tüm tablolar aynı kurallarla tutarlı; ortak kurallar `FinTrackDbContext.ConfigureConventions` içinde tek yerde.
- tekrar engeli kodda değil veritabanında garanti; uygulama hatası bile çift kayıt yazamaz.
- `DateTimeOffset` yazarken offset'in utc (0) olması gerekir; npgsql başka offset'i reddeder.
- ilişkili veri gerektiğinde açık `join` / projection yazmak gerekir.
