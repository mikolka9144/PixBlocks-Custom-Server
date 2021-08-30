namespace Pix_API
{
	public interface ILastIndexSaver
	{
		int LoadLastUnusedIndex();

		void SaveNewLastUnusedIndex(int index);
	}
}
