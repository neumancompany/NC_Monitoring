# Pokyny ke spu�t�n�

Po sta�en� projektu je pot�eba p�ed jeho prvn�m spu�t�n�m nainstalovat knihovny pomoc� nastroje bower a nainstalovat bal��ky z nuget feed�. D�le je tak� pot�eba m�t na po��ta�i nainstalovan� .NET Core 2.1.5.

## Dodate�n� nastaven�
**Ve slo�ce NC_Monitoring/NC_Monitoring najdete soubor appsettings.json, ze m��ete p�idat vlastn� glob�ln� adminy. Pomoc� t�to struktury:**

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

**SMTP server lze nastavit, tak� pomoc� tohoto souboru:**

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

**!!! Konfiguraci emailu je tak� pot�eba nastavit v souboru appsettings.json ve slo�ce NC_Monitoring.ConsoleApp !!!**

**Otev�ete si p��kazovou ��dku v ko�enov� slo�ce projektu projektu a napi�te postupn� tyto p��kazy:**

- *bower install*
  - vy�aduje m�t nainstalovan� n�stroj *bower*
- *dotnet restore* 
  - vy�aduje nainstalovan� dotnet core
- *dotnet build* 
  - nem�lo by b�t pot�eba
- *dotnet run*  

ConnectionString datab�ze je nastaven� na LocalDB. Pokud by datab�ze neexistovala, tak by se m�la sama vytvo�it.

## P�ihla�ovac� �daje
admin@admin.cz | pw: admin12\
user@user.cz | pw: user12

## Aplikace dostupn� online

https://ncmonitoring20190122042034.azurewebsites.net