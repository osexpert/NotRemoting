using System.Collections.Generic;

namespace NotRemoting.RpcMessaging;


public class AsyncEnumCallMessage : IMessage
{
	public AsyncEnumCallMessage()
	{

	}

	public AsyncEnumCallMessage(BinaryReaderEx r)
	{
		Deserialize(r);
	}

	public string ParameterName { get; set; }

	public int Position { get; set; }

	//public MessageType MessageType => MessageType.AsyncEnumCall;

	public int CacheKey => Position;

	public void Deserialize(BinaryReaderEx r)
	{
		ParameterName = r.ReadString();
		Position = r.ReadVarInt();
	}

	public void Deserialize(Stack<object?> st)
	{
	}

	public void Serialize(BinaryWriterEx w, Stack<object?> st)
	{
		w.Write(ParameterName);
		w.WriteVarInt(Position);
	}
}

