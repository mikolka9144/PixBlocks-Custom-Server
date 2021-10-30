using System;
namespace Pix_API.ChecklistReviewerApp.Interfaces
{
    public interface ITokenProvider
    {
        string GetTokenForUser(int UserId);
        int GetUserForToken(string token);
    }
}
