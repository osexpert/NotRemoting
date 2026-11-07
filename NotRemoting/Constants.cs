namespace NotRemoting;

public static class Constants
{

	public const string SessionIdHeaderKey = "not-session-id";

	public const string GrpcServiceName = "NotRemoting";

	/// <summary>
	/// v2: 
	/// - removed type name infos (security, now uses target method types)
	/// - changed call context to string values
	/// v3:
	/// - changed a lot...not pushing all args. Adding ParameterName\Position?
	/// - Add result types (kind).
	/// v4:
	/// - include execption map in gorelizer
	/// - move request type to top
	/// v5:
	/// - if only one type, no need to make generic Args (except for protobuf)
	/// </summary>
	internal static readonly byte SerializationVersion = 5;


	//		internal const string UserAgentHeaderKey = "user-agent";
	//		internal const string DotnetClientAgentStart = "grpc-dotnet/";
	//		internal const string NativeClientAgentStart = "grpc-csharp/";
}

