# P01 - ai destekli kişisel finans api

Oct 28, 2026 · @Emre

## proje özeti

kullanıcı bankasından indirdiği hesap dökümünü (csv/excel) yükler; sistem işlemleri okur, tekrarları ayıklar, ai ile kategorize eder, dövizli işlemleri güncel kurla tl'ye çevirir. kullanıcı bütçe koyar, aşınca uyarı alır; ay sonunda ai ona harcamalarını yorumlayan bir özet çıkarır.

- **kimin için:** bireysel kullanıcı (kobi değil). birden fazla banka hesabı ve kartı olan, harcamasını tek yerden görmek isteyen biri.
- **neden bu proje:** türkiye'de .net ilanlarının ağırlığı finans ve bankacılıkta. dosya işleme, idempotency, arka plan işleri, ai entegrasyonu, auth, cache, rate limit ve deploy tek projede görülür.
- **bağımsızlık:** bitirme projesinden tamamen ayrı tutulur; ortak kod paylaşılmaz.
- **süre:** 200 günlük planın 1-28. günleri, 18 adım. fazla gelen iş pazar buffer'ında kapatılır.

## kapsam

**içeride:**

- banka/kart hesapları tanımlama
- csv/excel döküm import (en az 2 banka formatı + genel şablon), arka planda işleme
- işlem listeleme, filtreleme, manuel işlem ekleme, not ve etiket
- ai kategorizasyon + kural tabanlı yedek + kullanıcı düzeltmesinden öğrenen kurallar
- çoklu para birimi (dış kur api'si)
- bütçeler ve bütçe aşım uyarısı
- abonelik tespiti (düzenli tekrar eden ödemeler)
- raporlar: aylık özet, kategori dağılımı, trend, ai aylık yorum
- jwt + refresh token, api key (üçüncü parti erişim)
- angular dashboard (ai ile yazılır)

**bilerek dışarıda:**

- bankalara doğrudan bağlanma (open banking) yok, sadece dosya
- ödeme alma, abonelik satışı yok (P06'nın işi)
- verilerle sohbet / rag yok (P07'nin işi)
- mobil uygulama yok

## teknoloji yığını

| katman | seçim |
| --- | --- |
| backend | .net 10, asp.net core minimal api, c# 14 |
| veritabanı | postgresql + ef core 10 |
| cache | hybridcache + redis |
| arka plan | backgroundservice + channels (import kuyruğu) |
| ai | microsoft.extensions.ai (ichatclient), geliştirmede yerel model veya openai |
| kur verisi | dış kur api'si (aday: frankfurter / tcmb, 4. adımda doğrulanır) |
| dosya okuma | csv: csvhelper, excel: closedxml |
| log ve izleme | serilog + correlation id + health checks |
| test | xunit + testcontainers (gerçek postgres ile) |
| frontend | angular (signals), ai ile yazılır |
| dağıtım | docker multi-stage + compose, github actions, ghcr, vps |

## veri modeli

her kayıt bir kullanıcıya aittir; bir kullanıcı başkasının verisini hiçbir endpoint'ten göremez.

| entity | alanlar | not |
| --- | --- | --- |
| User | id, email, passwordHash, displayName, baseCurrency, createdAt | baseCurrency varsayılan TRY |
| RefreshToken | id, userId, tokenHash, expiresAt, revokedAt, replacedById | rotation zinciri |
| ApiKey | id, userId, name, prefix, keyHash, scopes, lastUsedAt, revokedAt | anahtarın kendisi saklanmaz |
| Account | id, userId, name, bankName, type (bank/card/cash), currency |  |
| ImportJob | id, userId, accountId, fileName, fileHash, format, status, totalRows, importedRows, skippedRows, error, createdAt | aynı fileHash ikinci kez işlenmez |
| Transaction | id, userId, accountId, date, description, amount, currency, amountInBase, fxRate, categoryId, categorySource (ai/rule/user), merchant, note, dedupHash, importJobId | dedupHash tekil: aynı işlem iki kez yazılmaz |
| Category | id, userId (null = sistem), name, parentId, icon | sistem + kullanıcı kategorileri |
| CategoryRule | id, userId, pattern, categoryId, priority, createdFrom (user/ai) | "migros → market" |
| Budget | id, userId, categoryId, monthlyLimit, currency, alertAt (ör. %80) |  |
| Subscription | id, userId, merchant, amount, currency, period, nextExpectedDate, status (detected/confirmed/rejected) |  |
| FxRate | id, base, quote, rate, date, source | günlük kur, (base, quote, date) tekil |
| MonthlyInsight | id, userId, month, summaryText, model, createdAt | ai aylık yorumu, ayda bir üretilir |

## endpoint listesi

tümü `/api/v1` altında. auth dışındaki her şey giriş ister; `/external` uçları api key ile çalışır.

| grup | method | yol | açıklama |
| --- | --- | --- | --- |
| auth | POST | /auth/register | kayıt |
| auth | POST | /auth/login | giriş, access + refresh token |
| auth | POST | /auth/refresh | refresh token rotation |
| auth | POST | /auth/logout | refresh token iptali |
| auth | GET | /me | profil |
| auth | PATCH | /me | baseCurrency, displayName |
| hesaplar | GET / POST | /accounts | listele / ekle |
| hesaplar | PATCH / DELETE | /accounts/{id} | güncelle / sil |
| import | POST | /imports | dosya yükle, `202 accepted` + job id |
| import | GET | /imports | import geçmişi |
| import | GET | /imports/{id} | durum: queued / processing / done / failed |
| işlemler | GET | /transactions | cursor pagination, filtre: tarih, hesap, kategori, tutar, metin |
| işlemler | POST | /transactions | manuel işlem |
| işlemler | GET / PATCH / DELETE | /transactions/{id} | detay / not-kategori düzelt / sil |
| işlemler | POST | /transactions/recategorize | seçilen işlemleri ai ile yeniden kategorize et |
| kategoriler | GET / POST | /categories | sistem + özel kategoriler |
| kurallar | GET / POST | /rules | otomatik kategori kuralları |
| kurallar | DELETE | /rules/{id} | kural sil |
| bütçeler | GET / POST | /budgets | listele / oluştur |
| bütçeler | PATCH / DELETE | /budgets/{id} | güncelle / sil |
| bütçeler | GET | /budgets/status | bu ay harcanan / kalan, uyarılar |
| abonelikler | GET | /subscriptions | tespit edilenler |
| abonelikler | PATCH | /subscriptions/{id} | onayla / reddet |
| raporlar | GET | /reports/monthly?month= | aylık gelir-gider özeti |
| raporlar | GET | /reports/categories?from=&to= | kategori dağılımı |
| raporlar | GET | /reports/trend?months= | aylık trend |
| raporlar | GET / POST | /reports/insights/{month} | ai aylık yorum: getir / yeniden üret |
| kur | GET | /fx/rates?date= | günlük kurlar |
| api key | GET / POST | /api-keys | listele / oluştur (anahtar sadece bir kez gösterilir) |
| api key | DELETE | /api-keys/{id} | iptal |
| dış erişim | GET | /external/transactions, /external/reports/monthly | api key ile salt okunur erişim |
| sistem | GET | /health, /health/ready | sağlık kontrolleri |

## zor kısımlar ve mimari kararlar

1. **import ve idempotency:** dosyanın hash'i (`fileHash`) aynı dosyanın ikinci kez işlenmesini engeller. her işlemin `dedupHash`'i (hesap + tarih + tutar + açıklama) çakışan dökümlerde aynı işlemin iki kez yazılmasını engeller; veritabanında unique index ile garanti altına alınır.
2. **banka formatları:** her banka için bir `IStatementParser`. yeni banka = yeni sınıf, mevcut kod değişmez. tanınmayan format açık bir hata ile reddedilir.
3. **arka plan işleme:** `POST /imports` dosyayı kaydedip job'u kuyruğa (channel) atar ve hemen `202` döner. `BackgroundService` kuyruktan okur, satırları batch halinde yazar, job durumunu günceller. uygulama yeniden başlarsa yarım kalan job'lar `queued` durumuna döner.
4. **kategorizasyon sırası:** önce kullanıcı kuralları, sonra sistem kuralları, en son ai. ai sadece kuralların yakalamadığı işlemlere, batch halinde gider (maliyet ve hız). ai yanıtı yapılandırılmış json olarak alınır; geçersiz yanıt ya da hata durumunda işlem "kategorisiz" kalır, import asla ai yüzünden başarısız olmaz.
5. **öğrenen kurallar:** kullanıcı bir işlemin kategorisini düzeltirse, aynı merchant için kural önerilir veya oluşturulur. sonraki importlarda ai'ye gitmeye gerek kalmaz.
6. **para birimi:** tutar orijinal para biriminde saklanır; `amountInBase` ve kullanılan `fxRate` işlem tarihindeki kurla yazılır. kur sonradan değişse de geçmiş raporlar oynamaz. paralar her zaman `decimal`.
7. **abonelik tespiti:** aynı merchant'tan, benzer tutarda (±%10), düzenli aralıkla (\~30 gün) en az 3 kez çıkan ödemeler aday olur; kullanıcı onaylar veya reddeder.
8. **veri izolasyonu:** her sorgu `userId` ile filtrelenir (ef core global query filter). başka kullanıcının id'si ile istek gelirse `404` döner, `403` değil (varlığı bile sızdırılmaz).
9. **okuma ve yazma:** listeler ve raporlar dto'ya projection ile okunur; güncellemeler tracking ile entity çekip alan atayarak yapılır, körlemesine `Update()` kullanılmaz.
10. **rate limit ve cache:** ai üreten uçlar (insights, recategorize) daha sıkı limitli. raporlar hybridcache'te tutulur, yeni import gelince ilgili kullanıcının cache'i geçersiz kılınır.

## 18 adımlık yol haritası

plandaki 18 adımın teknik teması korunur, finans domain'inin işi bu adımlara dağıtılır. "ağır" işaretli günlerde yetişmeyen kısım o haftanın pazar buffer'ına kayar.

| adım | gün | tarih | iş | yük |
| --- | --- | --- | --- | --- |
| 1 | 1 | 28.10 | bu tasarım dokümanı + boş .net 10 repo | normal |
| 2 | 2 | 29.10 | solution iskeleti (api, application, domain, infrastructure, tests) + github actions build + boş angular projesi | normal |
| 3 | 4 | 31.10 | tüm entity'ler + ef core + postgres + ilk migration + sistem kategorileri seed | ağır |
| 4 | 6 | 02.11 | dış kur api'si: httpclientfactory + resilience, FxRate kaydetme | normal |
| 5 | 7 | 03.11 | import kuyruğu (channel) + backgroundservice + ilk banka csv parser + dedup + günlük kur job'u | ağır |
| 6 | 9 | 05.11 | hesaplar, işlemler, kategoriler endpoint'leri + typed results + openapi | normal |
| 7 | 10 | 06.11 | işlemlerde cursor pagination + filtre + sıralama | normal |
| 8 | 11 | 07.11 | auth: kayıt/giriş, jwt + refresh token rotation + angular login ve işlem tablosu | ağır |
| 9 | 14 | 10.11 | policy tabanlı yetki + global query filter (veri izolasyonu) + api key + /external uçları | normal |
| 10 | 15 | 11.11 | validation + problemdetails + global error handler + excel parser ve ikinci banka formatı | normal |
| 11 | 16 | 12.11 | hybridcache + redis + raporlar (aylık, kategori, trend) | ağır |
| 12 | 18 | 14.11 | ai kategorizasyon (ichatclient, kural → ai sırası, yedek) + rate limiting | ağır |
| 13 | 20 | 16.11 | serilog + correlation id + health checks + bütçeler + abonelik tespiti | ağır |
| 14 | 21 | 17.11 | unit testler: parser, dedup, kategorizasyon sırası, abonelik tespiti | normal |
| 15 | 23 | 19.11 | integration test (testcontainers) + ai aylık yorum (insights) | normal |
| 16 | 24 | 20.11 | docker multi-stage + compose (api, postgres, redis, frontend) + dashboard sayfaları | ağır |
| 17 | 25 | 21.11 | ci/cd: image ghcr'a push + vps'e deploy | normal |
| 18 | 28 | 24.11 | readme v1: mimari diyagram, kararlar, demo gif, ölçümler | normal |

## bitti sayılma kriterleri

- [ ] canlıda: vps'te çalışıyor, bir url'den erişiliyor
- [ ] gerçek bir banka dökümü yüklenip kategorize edilebiliyor, aynı dosya ikinci kez yüklenince tekrar yazılmıyor
- [ ] ai kapalıyken de sistem çalışıyor (kurallar + "kategorisiz")
- [ ] bir kullanıcı başkasının verisine hiçbir yoldan erişemiyor (integration test ile kanıtlı)
- [ ] unit + integration testler ci'da yeşil
- [ ] dashboard: giriş, import, işlem tablosu, bütçe ve rapor grafikleri çalışıyor
- [ ] readme: ne yaptığı, mimari diyagram, önemli kararlar ve nedenleri, nasıl çalıştırılır, demo gif
- [ ] her commit'in diff'ini satır satır anlatabiliyorum
