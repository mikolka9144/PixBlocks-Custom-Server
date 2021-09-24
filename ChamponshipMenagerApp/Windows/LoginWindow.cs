using System;
using Xwt;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace ChamponshipMenagerApp
{
    public class LoginWindow:Window
    {
        public bool ISLoginSuccsesful;
        private TextEntry login_box;

        public LoginWindow()
        {
            Title = "Connect to server";
            Width = 200;
            Height = 200;
            Closed += MainWindow_Closed;

            var boxController = new VBox();
            boxController.PackStart(new Label("Server adress"));
            login_box = new TextEntry
            {
                Text = LoginData_Saver.Load()
            };
            boxController.PackStart(login_box);
            var Login_Btn = new Button("Login");
            Login_Btn.Clicked += Login_Btn_Clicked;
            boxController.PackEnd(Login_Btn);
            Content = boxController;
        }

        void Login_Btn_Clicked(object sender, EventArgs e)
        {

            var url = login_box.Text;
            var ping_request = new TcpClient();
            try
            {
                var seg = url.Split(':');
                ping_request.Connect(seg[0], Convert.ToInt32(seg[1]));
                ping_request.Close();
            }
            catch
            {
                MessageDialog.ShowError("Could not ping remote server");
                return;
            }
            var api = new ChampionshipsAPI($"http://{login_box.Text}/");
            LoginData_Saver.Save(login_box.Text);
            var window = new MainWindow(api);
            window.Show();
            ISLoginSuccsesful = true;
            Close();
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if (!ISLoginSuccsesful) Environment.Exit(0);
        }
    }
}
