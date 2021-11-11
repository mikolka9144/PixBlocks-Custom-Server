using System;
using Xwt;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Collections.Generic;
using AdministrationApp.Controls;
using System.Linq;

namespace AdministrationApp.Windows
{
    public class UserEditForm:Window
    {
        int ID;
        TextEntry LoginBox = new TextEntry();
        TextEntry PasswordBox = new TextEntry();
        CheckBox IsAdmin = new CheckBox("Admin");
        private Action<User> OnSave;
        private AreaSelector AreaSelector;
        private readonly List<ServerAreaToCheck> areas;

        public UserEditForm(List<ServerAreaToCheck> areas)
        {
            AreaSelector = new AreaSelector();
            this.areas = areas;

            var box = new VBox();
            box.PackStart(new Label("Login"));
            box.PackStart(LoginBox);
            box.PackStart(new Label("Password"));
            box.PackStart(PasswordBox);
            box.PackStart(IsAdmin);
            box.PackStart(new Label("Areas"));
            box.PackStart(AreaSelector,true);
            box.PackEnd(Save_Button());
            Content = box;
        }
        public Button Save_Button()
        {
            var btn = new Button("Save");
            btn.Clicked += (sender, e) => 
            {
                var user = new User
                {
                    Id = ID,
                    login = LoginBox.Text,
                    passwordHash = PasswordBox.Text,
                    areasToCheckIds = AreaSelector.GetCheckedAreas().Select(s => s.Id).ToList()
                };
                if (IsAdmin.State == CheckBoxState.On) user.IsAdmin = true;
                OnSave(user);
                Close();
            };
            return btn;
        }
        public void ExposeUserForEditing(User user,Action<User> onSave)
        {
            ID = user.Id;
            LoginBox.Text = user.login;
            PasswordBox.Text = user.passwordHash;

            if (user.IsAdmin) IsAdmin.State = CheckBoxState.On;
            CreateNewUser(onSave,user.areasToCheckIds);
        }
        public void CreateNewUser(Action<User> onSave)
        {
            CreateNewUser(onSave, new List<int>());
        }
        private void CreateNewUser(Action<User> onSave,List<int> areasToCheckIds)
        {
            AreaSelector.CheckMany(areas, areasToCheckIds);
            OnSave = onSave;
            Show();
        }
    }
}
