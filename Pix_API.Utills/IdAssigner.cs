using System;
using System.Collections.Generic;
using System.Linq;

namespace Pix_API
{
    public class IdAssigner
    {
        private int _next_empty_id;

        public int NextEmptyId
        {
            get => _next_empty_id++;
        }
        public IdAssigner(List<int> used_id_list)
        {
            _next_empty_id = used_id_list.Any() ? used_id_list.Max() + 1 : 0;
        }
        public IdAssigner(int next_empty_Id)
        {
            _next_empty_id = next_empty_Id;
        }
    }

}
