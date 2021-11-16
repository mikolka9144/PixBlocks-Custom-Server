using System;
using AdministrationApp.Controls;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using Xwt;
namespace AdministrationApp.Windows
{
    public class ObjectEditForm:Window
    {
        private Action<ClientObjectInArea> OnSubmit;
        private int ID = -1;
        private ImageSelector image = new ImageSelector();
        private TextEntry name = new TextEntry();
        public ObjectEditForm()
        {
            Title = "Edit object";
            var box = new VBox();
            box.PackStart(new Label("Name"));
            box.PackStart(name);
            box.PackStart(new Label("Preview Image"));
            box.PackStart(image);

            box.PackEnd(Save_button());
            Content = box;
        }
        public void CreateNewObject(Action<ClientObjectInArea> onSubmit)
        {
            OnSubmit = onSubmit;
            Show();
        }
        public void ModifyObject(ServerObjectInArea obj,string base64image, Action<ClientObjectInArea> onSubmit)
        {
            ID = obj.Id;
            name.Text = obj.name;
            image.Image = Utills.GetImageFromBase64(base64image);
            CreateNewObject(onSubmit);
        }
        private Button Save_button()
        {
            var btn = new Button("Submit");
            btn.Clicked += delegate {
                OnSubmit(new ClientObjectInArea
                {
                    Id = ID,
                    image = Utills.GetBase64FromImage(image.Image),
                    name = name.Text
                });
                Close();
            };
            return btn;
        }
    }
}
