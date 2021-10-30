using System;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Interfaces
{
    public interface IAreaToCheckMetadataProvider
    {
        AreaToCheck GetArea(int Id);
        void AddArea(AreaToCheck area);
    }
}
