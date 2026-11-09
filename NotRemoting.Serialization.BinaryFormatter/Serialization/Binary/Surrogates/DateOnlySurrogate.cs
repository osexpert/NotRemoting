using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NotRemoting.Serialization.BinaryFormatter;

internal class DateOnlySurrogate : ISurrogate
{
	public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
	{
		var don = (DateOnly)obj;
		info.SetType(typeof(DateOnlyReference));
		info.AddValue("DayNumber", don.DayNumber);
	}

	public bool Handles(Type type, StreamingContext context)
	{
		var canHandle = type == typeof(DateOnly);
		return canHandle;
	}

	public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
	{
		throw new NotSupportedException();
	}

	[Serializable]
	internal struct DateOnlyReference : IObjectReference
	{
		private readonly int DayNumber;

		public DateOnlyReference(DateOnly don)
		{
			DayNumber = don.DayNumber;
		}

		public object GetRealObject(StreamingContext context)
		{
			return DateOnly.FromDayNumber(DayNumber);
		}
	}
}

