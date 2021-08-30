using System;
using System.IO;

namespace Pix_API.Providers
{
	public class DiskIndexSaver : ILastIndexSaver
	{
		private readonly string indexFilename;

		public DiskIndexSaver(string IndexFilename)
		{
			indexFilename = IndexFilename;
		}

		public int LoadLastUnusedIndex()
		{
			if (File.Exists(indexFilename))
			{
				return Convert.ToInt32(File.ReadAllText(indexFilename));
			}
			return 0;
		}

		public void SaveNewLastUnusedIndex(int index)
		{
			File.WriteAllText(indexFilename, Convert.ToString(index));
		}
	}
}
