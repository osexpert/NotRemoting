namespace NotRemoting;

/// <summary>
/// Describes a single call context entry.
/// </summary>

public class CallContextEntry : ISerializable
{
	public CallContextEntry()
	{
	}

	public CallContextEntry(BinaryReaderEx r)
	{
		Deserialize(r);
	}

	/// <summary>
	/// Gets or sets the name of the call context entry. 
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Gets or sets the value of the call context entry.
	/// </summary>
	public string Value { get; set; }

	public void Deserialize(BinaryReaderEx r)
	{
		Name = r.ReadString();
		Value = r.ReadString();
	}

	public void Deserialize(Stack<object?> st)
	{
	}

	public void Serialize(BinaryWriterEx w, Stack<object?> st)
	{
		w.Write(Name);
		w.Write(Value);
	}
}

