using System.Reflection;
using NotRemoting.Serialization;
using Grpc.Net.Compression;

namespace NotRemoting.RpcMessaging;

public class RequestMessage
{
	// only used on deserialize
	internal MethodInfo Method { get; set;  }

	internal string ServiceName { get; }
	internal string MethodName { get; }

	internal AsyncEnumCallResultMessage AsyncEnumCallResultMessage { get; }
	internal DelegateResultMessage DelegateResultMessage { get; }
	internal MethodCallMessage MethodCallMessage { get; }

	public RequestType RequestType { get; }

	internal ISerializerAdapter Serializer { get; }
	internal ICompressionProvider? Compressor { get; }


	public RequestMessage(DelegateResultMessage drm, string serviceName, string methodName, ISerializerAdapter serializer, ICompressionProvider? compressor)
	{
		DelegateResultMessage = drm;
		RequestType = RequestType.DelegateResult;
		Serializer = serializer;
		Compressor = compressor;
		ServiceName = serviceName;
		MethodName = methodName;
	}

	public RequestMessage(MethodCallMessage mcm, string serviceName, string methodName, ISerializerAdapter serializer, ICompressionProvider? compressor)
	{
		MethodCallMessage = mcm;
		RequestType = RequestType.MethodCall;
		Serializer = serializer;
		Compressor = compressor;
		ServiceName = serviceName;
		MethodName = methodName;
	}

	public RequestMessage(AsyncEnumCallResultMessage aem, string serviceName, string methodName, ISerializerAdapter serializer, ICompressionProvider? compressor)
	{
		AsyncEnumCallResultMessage = aem;
		RequestType = RequestType.AsyncEnumCallResult;
		Serializer = serializer;
		Compressor = compressor;
		ServiceName = serviceName;
		MethodName = methodName;
	}

	public static RequestMessage Deserialize(IRemotingParty r, Stream s, RequestType mType, string serviceName, string methodName,
		MethodInfo method, ISerializerAdapter serializer, ICompressionProvider? compressor)
	{
		RequestMessage res;
		if (mType == RequestType.DelegateResult)
			res = new RequestMessage(NotRemoting.Serializer.Deserialize<DelegateResultMessage>(r, s, method, serializer, compressor), serviceName, methodName, serializer, compressor);
		else if (mType == RequestType.MethodCall)
			res = new RequestMessage(NotRemoting.Serializer.Deserialize<MethodCallMessage>(r, s, method, serializer, compressor), serviceName, methodName, serializer, compressor);
		else if (mType == RequestType.AsyncEnumCallResult)
			res = new RequestMessage(NotRemoting.Serializer.Deserialize<AsyncEnumCallResultMessage>(r, s, method, serializer, compressor), serviceName, methodName, serializer, compressor);
		else
			throw new Exception();

		res.Method = method;
		return res;
	}

	internal void Serialize(IRemotingParty r, Stream s, MethodInfo method)
	{
		if (RequestType == RequestType.DelegateResult)
			NotRemoting.Serializer.Serialize(r, s, method, DelegateResultMessage, Serializer, Compressor);
		else if (RequestType == RequestType.MethodCall)
			NotRemoting.Serializer.Serialize(r, s, method, MethodCallMessage, Serializer, Compressor);
		else if (RequestType == RequestType.AsyncEnumCallResult)
			NotRemoting.Serializer.Serialize(r, s, method, AsyncEnumCallResultMessage, Serializer, Compressor);
		else
			throw new Exception();

	}
}

public enum RequestType
{
	MethodCall = 1,
	DelegateResult = 2,

	//ResponseType.MethodResult = 3,
	//ResponseType.DelegateCall = 4,
	//ResponseType.AsyncEnumCall = 5,

	AsyncEnumCallResult = 6,

	//ResponseType.AsyncEnumReturnResult = 7
}

