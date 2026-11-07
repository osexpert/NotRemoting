using System.Collections.Generic;

namespace NotRemoting.RpcMessaging;


public class AsyncEnumReturnResultMessage : IMessage
{
	public object? Value { get; set; }

	public AsyncEnumReturnResultMessage()
	{

	}

	public AsyncEnumReturnResultMessage(BinaryReaderEx r)
	{
		Deserialize(r);
	}

	//public MessageType MessageType => MessageType.AsyncEnumReturnResult;

	public int CacheKey => 0;

	public void Deserialize(BinaryReaderEx r)
	{
	}

	public void Deserialize(Stack<object?> st)
	{
		Value = st.Pop();
	}

	public void Serialize(BinaryWriterEx w, Stack<object?> st)
	{
		st.Push(Value);
	}
}

