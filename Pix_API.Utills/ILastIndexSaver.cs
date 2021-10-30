namespace Pix_API.Base.Utills
{
	public interface ILastIndexSaver
	{
		int LoadLastUnusedIndex();

		void SaveNewLastUnusedIndex(int index);
	}
}
