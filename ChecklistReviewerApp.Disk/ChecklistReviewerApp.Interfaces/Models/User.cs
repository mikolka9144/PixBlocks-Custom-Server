using System;
using System.Collections.Generic;

namespace Pix_API.ChecklistReviewerApp.Interfaces.Models
{
    [Serializable]
    public class User
    {
        public int Id;
        public string login;
        public string passwordHash;
        public List<int> areasToCheckIds;
    }
}
