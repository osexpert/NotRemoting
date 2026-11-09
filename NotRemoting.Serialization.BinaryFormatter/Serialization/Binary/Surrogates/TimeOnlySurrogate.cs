using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NotRemoting.Serialization.BinaryFormatter;

internal class TimeOnlySurrogate : ISurrogate
{
	public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
	{
		var ton = (TimeOnly)obj;
		info.SetType(typeof(TimeOnlyReference));
		info.AddValue("Ticks", ton.Ticks);
	}

	public bool Handles(Type type, StreamingContext context)
	{
		var canHandle = type == typeof(TimeOnly);
		return canHandle;
	}

	public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
	{
		throw new NotSupportedException();
	}

	[Serializable]
	internal struct TimeOnlyReference : IObjectReference
	{
		private readonly long Ticks;

		public TimeOnlyReference(TimeOnly ton)
		{
			Ticks = ton.Ticks;
		}

		public object GetRealObject(StreamingContext context)
		{
			return new TimeOnly(Ticks);
		}
	}
}


