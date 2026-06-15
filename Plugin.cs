using LabApi.Events.CustomHandlers;
using LabApi.Loader.Features.Plugins;

namespace SCPList;

using System;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader.Features.Plugins.Enums;
using LabApi.Events.CustomHandlers;
public class Plugin : Plugin<Config>
{
    public static Plugin Instance { get; private set; } = null!;
    public override string Name => "Sapphire Watermark";
    public override string Author => "Alex";
    public override string Description => "Sapphire Watermark";
    public override Version Version => new(1, 0, 0);
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);
    public override LoadPriority Priority => LoadPriority.High;
    private EventsHandler BigEventsPenis { get; } = new();


    public override void Enable()
    {
        if (Config is null)
            throw new Exception("Config is null!");
        
        if (Config.Debug)
            Logger.Debug("Debug mode enabled.");
            
        Instance = this;
        CustomHandlersManager.RegisterEventsHandler(BigEventsPenis);
    }

    public override void Disable()
    {
        Instance = null!;
        CustomHandlersManager.UnregisterEventsHandler(BigEventsPenis);
    }
}
