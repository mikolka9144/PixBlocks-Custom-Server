namespace Pix_API.Base.Utills
{
	public class IdAssigner
	{
		private int _next_empty_id;

		private ILastIndexSaver IndexSaver;

		public int NextEmptyId
		{
			get
			{
				IndexSaver.SaveNewLastUnusedIndex(_next_empty_id + 1);
				return _next_empty_id++;
			}
		}

		public IdAssigner(ILastIndexSaver saver)
		{
			_next_empty_id = saver.LoadLastUnusedIndex();
			IndexSaver = saver;
		}
	}
}
