// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: kunde.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace AutoReservation.Service.Grpc {
  internal static partial class KundeService
  {
    static readonly string __ServiceName = "AutoReservation.KundeService";


    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::AutoReservation.Service.Grpc.KundeReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of KundeService</summary>
    [grpc::BindServiceMethod(typeof(KundeService), "BindService")]
    public abstract partial class KundeServiceBase
    {
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(KundeServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder().Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, KundeServiceBase serviceImpl)
    {
    }

  }
}
#endregion