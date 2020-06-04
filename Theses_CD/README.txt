API automaticky po připojení k DB vytvoří struktu tabulek a nahraje testovací data (jen pokud už neexistují).
Přihlašovací údaje do aplikací jsou všude stejné:
- aplikace pro mobilní pracovníky (login: dcemp, pass: 1234)
- aplikace pro vedoucí pracovníky (login: mngr, pass: 1234)

Webové werze aplikací jsou dostupny na adresách:
https://dayss-clients.azurewebsites.net/#/login
http://dayss-clients.azurewebsites.net/manager_app/#/manager_app/login
https://daycaress-api-test.azurewebsites.net/index.html - API

Služby přes které je hosting uskutečněn jsou zdarma, proto prvotní spuštění trvá déle a muže dojít k chybě 502 (stačí stránku aktualizovat).
K urychlení načítání doporučuji spustit API a jednu z aplikací současně.

Vzhledem ktomu, že jsem byl nucen použít omezené množství Docker repozitářů, nefunguje u managerské aplikace na Azuru HTTPS. 
Z tohoto důvodu nefunguje určování polohy.

Podepsané APK nachazející se /multiplatform_app/build/android. Je nakonfigurováno tak, aby se připojovalo k API bežící v Azuru.
Je tedy potřeba nejdříve spustit stránku s API v prohlížeči a počkat na načtení.

IPA soubor jsem negeneroval, z důvodu restrikcí, které Apple má u šíření iOS aplikací na zařízení, které nejsou ve stejném vývojářském týmu.

Doproručená metoda spuštění systému lokálně je pomocí nástroje Docker.

SERVER
Vytvoření image API - ve složce server spustit přikaz: 
    docker build . -f .\Dockerfile --tag dss_api:latest

Spuštění včetně databáze - ve složce server spustit příkaz:
    docker-compose up

API by měla být dostupná na adrese: http://localhost:57316/.

WEBOVÁ VERZE MULTIPLAFORMNÍ APLIKACE
Vytvoření image aplikace - ve složce multiplatform_app spustit přikaz: 
    docker build . -f .\Dockerfile --tag dss_app:latest

Spuštění - ve složce multiplatform_app spustit přikaz:
    docker run -p 10000:80 dss_app:latest

Aplikace by měla být dostupná na adrese: http://localhost:10000/.

WEBOVÁ APLIKACE PRO MANAGERY
Vytvoření image aplikace - ve složce web_app spustit přikaz: 
    docker build . -f .\Dockerfile --tag dss_webapp:latest

Spuštění - ve složce web_app spustit přikaz:
    docker run -p 10001:80 dss_webapp:latest

Aplikace by měla být dostupná na adrese: http://localhost:10001/.


Spuštění webových verzí aplikací lokálně ze souborů ve složkách /multiplatform_app/build a /web_app/build není možné z důvodů popsaných v sekci 6 mé práce.
(možná by to mohl vyřešit nějaký CORS plugin do prohlížeče, ale nestetoval jsem)
Lze je nahrát na webový server a měli by fungovat bez problému, ale bude potřeba změnit adresu API v configuračním souboru ve složce assets.

Spuštění API lokálně je možné v podobě konzolové aplikace (/server/bin/DaycareSolutionSystem.Api.Host.exe). Ovšem vyžaduje přípojení k databázovému serveru (postrges).
V případě spouštění db serveru lokálně je nutno změnit connection string v appsettings.json souboru ve stejné složce.
V případě spouštění db serveru z dockeru: docker run --name local-db -e POSTGRES_PASSWORD=password -d -p 5432:5432 postgres:alpine (není potřeba měnit connection string).


Spuštění pomocí vývojářských nástrojů.
SERVER - Visual Studio (https://visualstudio.microsoft.com/cs/) - .NET Core 3.0+ SDK

Klienti 
- Node (https://nodejs.org/en/) - potřebný nástroj

Managerská aplikace
1. ve složce src spustit příkaz npm install
2. ve složce src spustit příkaz npm install -g @angular/cli@latest
3. ve složce src spustit příkaz ng serve
4. aplikace by měla být dostupná na adrese: http://localhost:4200/.

Multiplatformní aplikace
1. ve složce src spustit příkaz npm install
2. ve složce src spustit příkaz npm install -g @ionic/cli
3. ve složce src spustit příkaz ionic serve
4. aplikace by měla být dostupná na adrese: http://localhost:8100/.


V případě generování nativní aplikace z kódu (složka /multiplatform_app/src/):
1. spustit příkaz ionic build - před tímto krokem je potřeba v konfigurační souboru ve složce /src/src/assets změnit adresu API
     na adresu ke které se zařízení bude připojovat.
2. spustit příkaz npx cap copy
3. spustit příkaz npx cap sync

IOS - pouze na macOS a s IDE xCode
Spustit příkaz npx cap open IOS.

Android - potřebné IDE Android Studio
Spustit příkaz npx cap open android.
Zde je potřeba někdy build v rámci Android Studia spustit 2x, kvůli špatné synchronizaci pluginů třetích stran.


