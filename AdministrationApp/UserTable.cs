using System;
using Xwt;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Collections.Generic;

namespace AdministrationApp
{
    public class UserTable:ListView
    {
        private DataField<int> Id = new DataField<int>();
        private DataField<string> Login = new DataField<string>();
        private ListStore store;
        public UserTable(List<User> users)
        {
            store = new ListStore(Id,Login);
            DataSource = store;
            Columns.Add("ID", Id);
            Columns.Add("Login",Login);
        }
        public int AddUser(User user)
        {
            var index = store.AddRow();
            store.SetValues(index,
                Id, user.Id,
                Login, user.login);
            return index;
        }
        public void RemoveUser(int row)
        {
            store.RemoveRow(row);
        }
        public void EditUser(int row,User user)
        {
            store.SetValues(row,
                Id, user.Id,
                Login, user.login);
        }
    }
}
