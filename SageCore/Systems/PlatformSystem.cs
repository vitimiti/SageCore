// -----------------------------------------------------------------------
// <copyright file="PlatformSystem.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using SageCore.Logging;
using SageCore.NativeMethods.Sdl3;
using SageCore.Options;

namespace SageCore.Systems;

internal sealed class PlatformSystem : IDisposable
{
    private readonly ILogger _logger;
    private readonly AppOptions _options;

    public PlatformSystem([NotNull] ILogger logger, [NotNull] AppOptions options)
    {
        _logger = logger;
        _options = options;

        CommonLogging.LogInitializing(_logger, nameof(PlatformSystem));

        InitializeAppOptions();
        InitializeSdlLogging();

        CommonLogging.LogInitialized(_logger, nameof(PlatformSystem));
    }

    public void Dispose()
    {
        CommonLogging.LogDisposing(_logger, nameof(PlatformSystem));

        if (Sdl.LogOutputFunctionHandle.IsAllocated)
        {
            Sdl.LogOutputFunctionHandle.Free();
            PlatformLogging.LogSdlLogOutputFunctionFreed(_logger);
        }

        CommonLogging.LogDisposed(_logger, nameof(PlatformSystem));
    }

    private void InitializeAppOptions()
    {
        PlatformLogging.LogInitializingAppOptions(_logger);

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataNameString, _options.Name))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Name), _options.Name);
        }
        else
        {
            PlatformLogging.LogSuccessfullySetAppData(_logger, nameof(_options.Name), _options.Name);
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataVersionString, _options.Version.ToString()))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Version), _options.Version.ToString());
        }
        else
        {
            PlatformLogging.LogSuccessfullySetAppData(_logger, nameof(_options.Version), _options.Version.ToString());
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataIdentifierString, _options.Identifier))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Identifier), _options.Identifier);
        }
        else
        {
            PlatformLogging.LogSuccessfullySetAppData(_logger, nameof(_options.Identifier), _options.Identifier);
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataCreatorString, _options.Creator))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Creator), _options.Creator);
        }
        else
        {
            PlatformLogging.LogSuccessfullySetAppData(_logger, nameof(_options.Creator), _options.Creator);
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataCopyrightString, _options.Copyright))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Copyright), _options.Copyright);
        }
        else
        {
            PlatformLogging.LogSuccessfullySetAppData(_logger, nameof(_options.Copyright), _options.Copyright);
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataUrlString, _options.Url.ToString()))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.Url), _options.Url.ToString());
        }
        else
        {
            PlatformLogging.LogSuccessfullySetAppData(_logger, nameof(_options.Url), _options.Url.ToString());
        }

        if (!Sdl.TrySetAppMetadataProperty(Sdl.PropAppMetadataTypeString, _options.AppType))
        {
            PlatformLogging.LogFailedToSetAppData(_logger, nameof(_options.AppType), _options.AppType);
        }
        else
        {
            PlatformLogging.LogSuccessfullySetAppData(_logger, nameof(_options.AppType), _options.AppType);
        }

        PlatformLogging.LogAppOptionsInitialized(_logger);
    }

    private void InitializeSdlLogging()
    {
        PlatformLogging.LogInitializingSdlLogging(_logger);

        // TODO: Change the level depending on the chosen log level. Make Info the minimum.
        Sdl.SetLogPriorities(Sdl.LogPriority.Trace);
        PlatformLogging.LogSdlLogPrioritySet(_logger, Sdl.LogPriority.Trace);

        Sdl.SetLogOutputFunction(
            (category, priority, message) =>
            {
                switch (priority)
                {
                    case Sdl.LogPriority.Debug:
                        SdlLogging.LogDebug(_logger, category.ToString(), message);
                        break;
                    case Sdl.LogPriority.Info:
                        SdlLogging.LogInformation(_logger, category.ToString(), message);
                        break;
                    case Sdl.LogPriority.Warn:
                        SdlLogging.LogWarning(_logger, category.ToString(), message);
                        break;
                    case Sdl.LogPriority.Error:
                        SdlLogging.LogError(_logger, category.ToString(), message);
                        break;
                    case Sdl.LogPriority.Critical:
                        SdlLogging.LogCritical(_logger, category.ToString(), message);
                        break;
                    case Sdl.LogPriority.Invalid:
                    case Sdl.LogPriority.Trace:
                    case Sdl.LogPriority.Verbose:
                    default:
                        SdlLogging.LogTrace(_logger, category.ToString(), message);
                        break;
                }
            }
        );

        PlatformLogging.LogSdlLoggingInitialized(_logger);
    }
}
