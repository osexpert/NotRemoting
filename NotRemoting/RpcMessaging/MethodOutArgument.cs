namespace NotRemoting.RpcMessaging;

/// <summary>
/// Serializable message that describes an out parameter.
/// </summary>
public class MethodOutArgument : ISerializable
{
	public MethodOutArgument()
	{
	}

	public MethodOutArgument(BinaryReaderEx r)
	{
		Deserialize(r);
	}

	/// <summary>
	/// Gets or sets the name of the parameter.
	/// </summary>
	public string ParameterName { get; set; }


	public int Position { get; set; }


	/// <summary>
	/// Gets or sets the out value of the parameter.
	/// </summary>
	public object? OutValue { get; set; }

	public void Serialize(BinaryWriterEx w, Stack<object?> st)
	{
		w.Write(ParameterName);
		w.WriteVarInt(Position);

		st.Push(OutValue);
	}

	public void Deserialize(BinaryReaderEx r)
	{
		ParameterName = r.ReadString();
		Position = r.ReadVarInt();
	}

	public void Deserialize(Stack<object?> st)
	{
		OutValue = st.Pop();
	}
}
