using System;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Interfaces
{
    public interface IAreaToCheckMetadataProvider
    {
        ServerAreaToCheck GetArea(int Id);
        int AddArea(ServerAreaToCheck area);
        void EditArea(ServerAreaToCheck area);
        List<ServerAreaToCheck> GetAllAreas();
        void RemoveArea(int Id);
    }
}
