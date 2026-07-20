# Remove Bluetooth Device :

## Simple Executable pour supprimer en une ligne de commande un appareil Bluetooth de Windows

## Create the project :

```
dotnet new console -n RemoveBluetoothDevice
```

## Ajouter le Package SDK Windows :

```
cd RemoveBluetoothDevice
dotnet add package Microsoft.Windows.SDK.Contracts
```

### `RemoveBluetoothDevice.csproj` :

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0-windows10.0.19041.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

</Project>
```

### `Program.cs` :

```cs
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

class Program
{
    static async Task<int> Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: RemoveBluetoothDevice.exe <device name or MAC>");
            return 3;
        }

        string target = args[0];

        Console.WriteLine($"Recherche : {target}");

        string selector = BluetoothDevice.GetDeviceSelector();

        DeviceInformationCollection devices =
            await DeviceInformation.FindAllAsync(selector);


        foreach (var device in devices)
        {
            Console.WriteLine(
                $"{device.Name} - {device.Id}"
            );


            bool match =
                device.Name.Contains(
                    target,
                    StringComparison.OrdinalIgnoreCase
                )
                ||
                device.Id.Contains(
                    target,
                    StringComparison.OrdinalIgnoreCase
                );


            if (!match)
                continue;


            Console.WriteLine(
                $"Suppression de {device.Name}"
            );


            var result =
                await device.Pairing.UnpairAsync();


            if (result.Status ==
                DeviceUnpairingResultStatus.Unpaired)
            {
                Console.WriteLine(
                    "Suppression réussie"
                );

                return 0;
            }


            Console.WriteLine(
                $"Echec : {result.Status}"
            );

            return 2;
        }


        Console.WriteLine(
            "Appareil introuvable"
        );

        return 1;
    }
}
```

## Créer le fichier .exe du Projet :

```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## Trouver le fichier .exe :

```
cd bin\Release\net8.0-windows10.0.19041.0\win-x64\publish\ && ls
```

## Comment l'utiliser ?

```
.\RemoveBluetoothDevice.exe "DualSense Edge Wireless Controller"

ou

.\RemoveBluetoothDevice.exe "D42F4B21D8FB" # Adresse MAC
```