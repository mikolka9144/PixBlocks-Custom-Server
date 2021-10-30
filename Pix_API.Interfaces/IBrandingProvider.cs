namespace Pix_API.PixBlocks.Interfaces
{
	public interface IBrandingProvider
	{
		string GetBase64LogoForUser(int userId);
	}
}
