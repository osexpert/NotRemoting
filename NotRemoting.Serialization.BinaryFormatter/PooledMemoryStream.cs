#if false
using System.IO;
using Microsoft.IO;

namespace NotRemoting.Serialization.BinaryFormatter
{
	public static class PooledMemoryStream
	{
		private static readonly RecyclableMemoryStreamManager _manager = new RecyclableMemoryStreamManager();

		public static MemoryStream GetStream() => _manager.GetStream();
	}
}
#endif
