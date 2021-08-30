namespace Pix_API.Interfaces
{
	public interface IBrandingProvider
	{
		string GetBase64LogoForUser(int userId);
	}
}
