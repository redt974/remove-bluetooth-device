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
