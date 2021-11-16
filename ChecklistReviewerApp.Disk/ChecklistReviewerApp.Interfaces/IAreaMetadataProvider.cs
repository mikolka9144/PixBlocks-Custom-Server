using System;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace Pix_API.ChecklistReviewerApp.Interfaces
{
    public interface IAreaMetadataProvider
    {
        ServerAreaToCheck GetArea(int Id);
        void AddArea(ServerAreaToCheck area);
        void EditArea(ServerAreaToCheck area);
        List<ServerAreaToCheck> GetAllAreas();
        void RemoveArea(int Id);
    }
}
