// -----------------------------------------------------------------------
// <copyright file="PlatformSystem.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Logging;
using SageCore.Logging;
using SageCore.NativeMethods.Sdl3;
using SageCore.Options;

namespace SageCore.Systems;

internal sealed class PlatformSystem : IDisposable
{
    private readonly ILogger _logger;
    private readonly AppOptions _options;

    public PlatformSystem(ILogger logger, AppOptions options)
    {
        _logger = logger;
        _options = options;

        InitializeAppOptions();
        InitializeSdlLogging();
    }

    public void Dispose()
    {
        if (Sdl.LogOutputFunctionHandle.IsAllocated)
        {
            Sdl.LogOutputFunctionHandle.Free();
        }
    }

    private void InitializeAppOptions()
    {
        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataNameString, _options.Name))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Name), _options.Name);
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataVersionString, _options.Version.ToString()))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Version), _options.Version.ToString());
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataIdentifierString, _options.Identifier))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Identifier), _options.Identifier);
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataCreatorString, _options.Creator))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Creator), _options.Creator);
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataCopyrightString, _options.Copyright))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Copyright), _options.Copyright);
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataUrlString, _options.Url.ToString()))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Url), _options.Url.ToString());
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataTypeString, _options.AppType))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.AppType), _options.AppType);
        }
    }

    private void InitializeSdlLogging()
    {
        // TODO: Change the level depending on the chosen log level. Make Info the minimum.
        Sdl.SetLogPriorities(Sdl.LogPriority.Trace);

        Sdl.SetLogOutputFunction(
            (category, priority, message) =>
            {
                switch (priority)
                {
                    case Sdl.LogPriority.Debug:
                        SdlLogging.Debug(_logger, category.ToString(), message);
                        break;
                    case Sdl.LogPriority.Info:
                        SdlLogging.Information(_logger, category.ToString(), message);
                        break;
                    case Sdl.LogPriority.Warn:
                        SdlLogging.Warning(_logger, category.ToString(), message);
                        break;
                    case Sdl.LogPriority.Error:
                        SdlLogging.Error(_logger, category.ToString(), message);
                        break;
                    case Sdl.LogPriority.Critical:
                        SdlLogging.Critical(_logger, category.ToString(), message);
                        break;
                    case Sdl.LogPriority.Invalid:
                    case Sdl.LogPriority.Trace:
                    case Sdl.LogPriority.Verbose:
                    default:
                        SdlLogging.Trace(_logger, category.ToString(), message);
                        break;
                }
            }
        );
    }
}
