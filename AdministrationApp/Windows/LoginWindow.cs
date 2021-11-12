using System;
using Xwt;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using AdministrationApp;
using AdministrationApp.API;

namespace ChamponshipMenagerApp
{
    public class LoginWindow:Window
    {
        public bool ISLoginSuccsesful;
        private PasswordEntry password_box;
        private TextEntry login_box;

        public LoginWindow()
        {
            Title = "Connect to server";
            Width = 200;
            Height = 200;
            Closed += MainWindow_Closed;

            var boxController = new VBox();

            boxController.PackStart(new Label("Login"));
            login_box = new TextEntry();
            boxController.PackStart(login_box);

            boxController.PackStart(new Label("Password"));
            password_box = new PasswordEntry();
            boxController.PackStart(password_box);

            var Login_Btn = new Button("Login");
            Login_Btn.Clicked += (sender, e) => Login(new CachedAPI());
            boxController.PackEnd(Login_Btn);

            var Test_Btn = new Button("Test UI");
            Test_Btn.Clicked += (sender, e) => Login(new MockAPI());
            boxController.PackEnd(Test_Btn);

            Content = boxController;
        }

        void Login(IAPIClient client)
        {
            if (client.IsServerOnline())
            {
                var LoginSuccsessful = client.LoginToServer(login_box.Text,password_box.Password);
                if (LoginSuccsessful)
                {
                    var window = new MainControlPanel(client);
                    window.Show();
                    ISLoginSuccsesful = true;
                    Close();
                }
                else MessageDialog.ShowError("Could not login to remote server");
            }
            else MessageDialog.ShowError("Could not ping remote server");
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (!ISLoginSuccsesful) Environment.Exit(0);
        }
    }
}
