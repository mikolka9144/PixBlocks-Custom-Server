using System;
using Xwt;
using AdministrationApp.Controls;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Collections.Generic;

namespace AdministrationApp.Windows
{
    public class UsersTab:VBox
    {
        private UsersTable users = new UsersTable();
        private readonly IAPIClient client;

        public UsersTab(IAPIClient client)
        {
            PackStart(users,true);
            PackEnd(Button_Box());
            this.client = client;
            foreach (var item in client.GetAllUsers())
            {
                users.AddUser(item);
            }
        }
        private HBox Button_Box()
        {
            var box = new HBox();
            box.PackStart(Add_Button(), true);
            box.PackStart(Edit_Button(), true);
            box.PackStart(Remove_Button(), true);
            return box;
        }
        private Button Edit_Button()
        {
            var button = new Button("Edit user");
            button.Clicked += delegate {
                var user = users.GetSelectedObject();
                if (user != null)
                {
                    new UserEditForm(client.GetAllAreasToCheck()).ExposeUserForEditing(user, (obj) =>
                    {
                        client.EditUser(obj);
                        users.UpdateUser(obj);
                    });
                }

            };
            return button;
        }
        private Button Remove_Button()
        {
            var button = new Button("Remove user");
            button.Clicked += delegate {
                var user = users.GetSelectedObject();
                users.RemoveUser(user);
                client.RemoveUser(user.Id);
            };
            return button;
        }
        private Button Add_Button()
        {
            var button = new Button("Add user");
            button.Clicked += delegate {
                new UserEditForm(client.GetAllAreasToCheck()).CreateNewUser((obj) =>
                {
                    var userWithUpdatedID = client.AddUser(obj);
                    users.AddUser(userWithUpdatedID);
                });
            };
            return button;
        }
    }
}
