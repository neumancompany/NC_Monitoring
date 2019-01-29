# Pokyny ke spuštìní

Po stažení projektu je potøeba pøed jeho prvním spuštìním nainstalovat knihovny pomocí nastroje bower a nainstalovat balíèky z nuget feedù. Dále je také potøeba mít na poèítaèi nainstalovaný .NET Core 2.1.5.

## Dodateèná nastavení
**Ve složce NC_Monitoring/NC_Monitoring najdete soubor appsettings.json, ze mùžete pøidat vlastní globální adminy. Pomocí této struktury:**

```json
"GlobalAdmins": [
    {
      "UserName": "vas@email.cz",
      "Password": "*****"
    },
    {
      "UserName": "dalsi@email.cz",
      "Password": "******"
    }
  ],
```

**SMTP server lze nastavit, také pomocí tohoto souboru:**

```json
  "Email": {
    "From": "veselytest@seznam.cz",
    "Smtp": {
      "Host": "smtp.seznam.cz",
      "Port": 25,
      "Username": "veselytest@seznam.cz",
      "Password": "******"
    }
  }
```

**!!! Konfiguraci emailu je také potøeba nastavit v souboru appsettings.json ve složce NC_Monitoring.ConsoleApp !!!**

**Otevøete si pøíkazovou øádku v koøenové složce projektu projektu a napište postupnì tyto pøíkazy:**

- *bower install*
  - vyžaduje mít nainstalovaný nástroj *bower*
- *dotnet restore* 
  - vyžaduje nainstalovaný dotnet core
- *dotnet build* 
  - nemìlo by být potøeba
- *dotnet run*  

ConnectionString databáze je nastavený na LocalDB. Pokud by databáze neexistovala, tak by se mìla sama vytvoøit.

## Pøihlašovací údaje
admin@admin.cz | pw: admin12\
user@user.cz | pw: user12

## Aplikace dostupná online

https://ncmonitoring20190122042034.azurewebsites.net