using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using System.Text.Json;
namespace SmartHome.Common.Models.Configs;


// Base config class
public abstract class DeviceConfig
{
    public const string MakeNewDevice = "makeNewDevice";
}

public static class DeviceExtensions
{
    public static T GetConfig<T>(this Device device) where T : DeviceConfig
    {
        if (!device.IsLoaded())
            device.LoadDeviceConfig();

        return device.Config as T
            ?? throw new Exception($"Device config mismatch, requested {typeof(T)} but got {device.Config?.GetType()} instead");
    }
    public static void LoadDeviceConfig(this Device device)
    {
        if (device.IsLoaded()) return;

        if (string.IsNullOrWhiteSpace(device.JsonObjectConfig))
            throw new Exception("Device configuration is missing or empty.");

        device.Config = device.Type switch
        {
            DeviceType.Lamp => DeserializeConfig<LampConfig>(device),
            DeviceType.Televisie => DeserializeConfig<TelevisieConfig>(device),
            DeviceType.Router => DeserializeConfig<RouterConfig>(device),
            DeviceType.Wasmachine => DeserializeConfig<WasmachineConfig>(device),
            _ => throw new ArgumentException($"Unsupported device type: {device.Type}")
        };
    }

    public static string GetImage(this Device device)
    {
        if (device.Type == DeviceType.Lamp)
            return GetLampImage(device);
        else
            return device.Type.GetImage();
    }
    private static string GetLampImage(Device device)
    {
        var lampConfig = device.GetConfig<LampConfig>();
        return lampConfig.Ingeschakeld
            ? "Afbeeldingen/licht-aan.png"
            : GetImage(DeviceType.Lamp); //on image
    }

    public static string GetImage(this DeviceType deviceType)
    {
        return deviceType switch
        {
            DeviceType.Lamp => "Afbeeldingen/licht-uit.png",
            DeviceType.Televisie => "Afbeeldingen/televisie.png",
            DeviceType.Wasmachine => "Afbeeldingen/wasmachine.png",
            DeviceType.Router => "Afbeeldingen/router.png",
            _ => throw new NotImplementedException($"Unsupported device type for image: {deviceType}")
        };
    }
    public static void LoadMultipleDeviceConfigs(this IEnumerable<Device> devices)
    {
        foreach (var device in devices)
        {
            device.LoadDeviceConfig();
        }
    }

    public static void SaveMultipleDeviceConfigs(this IEnumerable<Device> devices)
    {
        foreach (var device in devices)
        {
            device.SaveDeviceConfig();
        }
    }

    public static bool IsLoaded(this Device device) => device.Config != null;

    public static void SaveDeviceConfig(this Device device)
    {
        if (!device.IsLoaded())
            throw new Exception("Device configuration is not set.");

        device.JsonObjectConfig = JsonSerializer.Serialize(device.Config);
    }

    private static T DeserializeConfig<T>(Device device) where T : DeviceConfig, new()
    {
        if (device.JsonObjectConfig == DeviceConfig.MakeNewDevice)
            return new T();

        return JsonSerializer.Deserialize<T>(device.JsonObjectConfig)
            ?? throw new Exception($"Invalid config for {device.Type}");
    }
}
