// -----------------------------------------------------------------------
// <copyright file="Gpu.cs" company="SageCore Contributors">
// 2025 Copyright (c) SageCore Contributors. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace SageCore.NativeMethods.Sdl3;

internal static partial class Sdl
{
    public const uint GpuShaderFormatSpirv = 1U << 1;
    public const uint GpuShaderFormatDxil = 1U << 3;
    public const uint GpuShaderFormatMsl = 1U << 4;

    public enum GpuLoadOp
    {
        Clear = 1,
    }

    public enum GpuStoreOp
    {
        Store,
    }

    [NativeMarshalling(typeof(SafeHandleMarshaller<GpuDeviceSafeHandle>))]
    public sealed partial class GpuDeviceSafeHandle : SafeHandle
    {
        public GpuDeviceSafeHandle()
            : base(invalidHandleValue: nint.Zero, ownsHandle: true) { }

        public override bool IsInvalid => handle == nint.Zero;

        public static GpuDeviceSafeHandle Create(uint formatFlags, bool debugMode = false, string? name = null) =>
            CreateGpuDevice(formatFlags, debugMode, name);

        public bool TryClaimWindow(WindowSafeHandle window) => ClaimWindow(this, window);

        public void ReleaseWindow(WindowSafeHandle window) => ReleaseWindowFromGpuDevice(this, window);

        protected override bool ReleaseHandle()
        {
            DestroyGpuDevice(handle);
            return true;
        }

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_CreateGPUDevice", StringMarshalling = StringMarshalling.Utf8)]
        private static partial GpuDeviceSafeHandle CreateGpuDevice(
            uint formatFlags,
            [MarshalAs(UnmanagedType.I4)] bool debugMode,
            string? name
        );

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_ClaimWindowForGPUDevice")]
        [return: MarshalAs(UnmanagedType.I4)]
        private static partial bool ClaimWindow(GpuDeviceSafeHandle device, WindowSafeHandle window);

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(
            DllName,
            EntryPoint = "SDL_ReleaseWindowFromGPUDevice",
            StringMarshalling = StringMarshalling.Utf8
        )]
        private static partial void ReleaseWindowFromGpuDevice(GpuDeviceSafeHandle device, WindowSafeHandle window);

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_DestroyGPUDevice")]
        private static partial void DestroyGpuDevice(nint device);
    }

    [NativeMarshalling(typeof(SafeHandleMarshaller<GpuCommandBufferSafeHandle>))]
    public sealed partial class GpuCommandBufferSafeHandle : SafeHandle
    {
        public GpuCommandBufferSafeHandle()
            : base(invalidHandleValue: nint.Zero, ownsHandle: true) { }

        public override bool IsInvalid => handle == nint.Zero;

        public static GpuCommandBufferSafeHandle Acquire(GpuDeviceSafeHandle device) => AcquireGpuCommandBuffer(device);

        public bool TryWaitAndAcquireSwapchainTexture(
            WindowSafeHandle window,
            out GpuTextureSafeHandle swapchainTexture,
            out uint swapchainTextureWidth,
            out uint swapchainTextureHeight
        ) =>
            WaitAndAcquireSwapchainTexture(
                this,
                window,
                out swapchainTexture,
                out swapchainTextureWidth,
                out swapchainTextureHeight
            );

        public GpuRenderPassSafeHandle BeginRenderPass(IReadOnlyCollection<GpuColorTargetInfo> colorTargetInfos) =>
            BeginGpuRenderPass(this, [.. colorTargetInfos], (uint)colorTargetInfos.Count, nint.Zero);

        public GpuRenderPassSafeHandle BeginRenderPass(
            IReadOnlyCollection<GpuColorTargetInfo> colorTargetInfos,
            GpuDepthStencilTargetInfo depthStencilTargetInfo
        ) => BeginGpuRenderPass(this, [.. colorTargetInfos], (uint)colorTargetInfos.Count, in depthStencilTargetInfo);

        public bool TrySubmit() => SubmitGpuCommandBuffer(this);

        // Nothing to do here, as the command buffer is destroyed when the device is destroyed.
        protected override bool ReleaseHandle() => true;

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_AcquireGPUCommandBuffer")]
        private static partial GpuCommandBufferSafeHandle AcquireGpuCommandBuffer(GpuDeviceSafeHandle device);

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_WaitAndAcquireGPUSwapchainTexture")]
        [return: MarshalAs(UnmanagedType.I4)]
        private static partial bool WaitAndAcquireSwapchainTexture(
            GpuCommandBufferSafeHandle commandBuffer,
            WindowSafeHandle window,
            out GpuTextureSafeHandle swapchainTexture,
            out uint swapchainTextureWidth,
            out uint swapchainTextureHeight
        );

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_BeginGPURenderPass")]
        private static partial GpuRenderPassSafeHandle BeginGpuRenderPass(
            GpuCommandBufferSafeHandle commandBuffer,
            [In]
            [MarshalUsing(
                typeof(ArrayMarshaller<GpuColorTargetInfo, GpuColorTargetInfo>),
                CountElementName = nameof(numColorTargets)
            )]
                GpuColorTargetInfo[] colorTargetInfos,
            uint numColorTargets,
            nint depthStencilTargetInfo
        );

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_BeginGPURenderPass")]
        private static partial GpuRenderPassSafeHandle BeginGpuRenderPass(
            GpuCommandBufferSafeHandle commandBuffer,
            [In]
            [MarshalUsing(
                typeof(ArrayMarshaller<GpuColorTargetInfo, GpuColorTargetInfo>),
                CountElementName = nameof(numColorTargets)
            )]
                GpuColorTargetInfo[] colorTargetInfos,
            uint numColorTargets,
            in GpuDepthStencilTargetInfo depthStencilTargetInfo
        );

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_SubmitGPUCommandBuffer")]
        [return: MarshalAs(UnmanagedType.I4)]
        private static partial bool SubmitGpuCommandBuffer(GpuCommandBufferSafeHandle commandBuffer);
    }

    [NativeMarshalling(typeof(SafeHandleMarshaller<GpuTextureSafeHandle>))]
    public sealed class GpuTextureSafeHandle : SafeHandle
    {
        public GpuTextureSafeHandle()
            : base(invalidHandleValue: nint.Zero, ownsHandle: true) { }

        public override bool IsInvalid => handle == nint.Zero;

        // Nothing to do here, as the texture is destroyed when the device is destroyed.
        protected override bool ReleaseHandle() => true;
    }

    [NativeMarshalling(typeof(SafeHandleMarshaller<GpuRenderPassSafeHandle>))]
    public sealed partial class GpuRenderPassSafeHandle : SafeHandle
    {
        public GpuRenderPassSafeHandle()
            : base(invalidHandleValue: nint.Zero, ownsHandle: true) { }

        public override bool IsInvalid => handle == nint.Zero;

        public void End() => EndGpuRenderPass(handle);

        // Nothing to do here, as the render pass is destroyed when the device is destroyed.
        protected override bool ReleaseHandle() => true;

        [DefaultDllImportSearchPaths(DllImportSearchPath.UserDirectories)]
        [LibraryImport(DllName, EntryPoint = "SDL_EndGPURenderPass")]
        private static partial void EndGpuRenderPass(nint renderPass);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GpuColorTargetInfo
    {
        public nint Texture;
        public uint MipLevel;
        public uint LayerOrDepthPlane;
        public FColor ClearColor;
        public GpuLoadOp LoadOp;
        public GpuStoreOp StoreOp;
        public nint ResolveTexture;
        public uint ResolveMipLevel;
        public uint ResolveLayer;
        public int Cycle;
        public int CycleResolveTexture;
        public byte Padding1;
        public byte Padding2;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GpuDepthStencilTargetInfo
    {
        public nint Texture;
        public float ClearDepth;
        public GpuLoadOp LoadOp;
        public GpuStoreOp StoreOp;
        public GpuLoadOp StencilLoadOp;
        public GpuStoreOp StencilStoreOp;
        public int Cycle;
        public byte ClearStencil;
        public byte Padding1;
        public byte Padding2;
    }
}
