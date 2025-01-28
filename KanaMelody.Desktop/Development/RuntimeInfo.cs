using System;
using System.Diagnostics.CodeAnalysis;

namespace KanaMelody.Development;

public static class RuntimeInfo
{
    public static Platform OS { get; }
    
    static RuntimeInfo()
    {
        if (OperatingSystem.IsWindows())
            OS = Platform.Windows;
        if (OperatingSystem.IsIOS())
            OS = OS == 0 ? Platform.iOS : throw new InvalidOperationException($"Tried to set OS Platform to {nameof(Platform.iOS)}, but is already {Enum.GetName(OS)}");
        if (OperatingSystem.IsAndroid())
            OS = OS == 0 ? Platform.Android : throw new InvalidOperationException($"Tried to set OS Platform to {nameof(Platform.Android)}, but is already {Enum.GetName(OS)}");
        if (OperatingSystem.IsMacOS())
            OS = OS == 0 ? Platform.macOS : throw new InvalidOperationException($"Tried to set OS Platform to {nameof(Platform.macOS)}, but is already {Enum.GetName(OS)}");
        if (OperatingSystem.IsLinux())
            OS = OS == 0 ? Platform.Linux : throw new InvalidOperationException($"Tried to set OS Platform to {nameof(Platform.Linux)}, but is already {Enum.GetName(OS)}");

        if (OS == 0)
            throw new PlatformNotSupportedException("Operating system could not be detected correctly.");
    }
    
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum Platform
    {
        Windows = 1,
        Linux = 2,
        macOS = 3,
        iOS = 4,
        Android = 5
    }
}