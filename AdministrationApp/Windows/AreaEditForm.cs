using System;
using Xwt;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using Xwt.Drawing;
using System.IO;
using AdministrationApp.Controls;
using AdministrationApp.Windows;
namespace AdministrationApp
{
    public class AreaEditForm:Dialog
    {
        private int AreaID = -1;
        private TextEntry nameBox = new TextEntry();
        private TextEntry terrainBox = new TextEntry();

        private ObjectsInAreaList objectsInArea = new ObjectsInAreaList();
        private Action<ServerAreaToCheck> OnSave;
        private ImageSelector Image = new ImageSelector();
        private readonly IAPIClient client;

        public AreaEditForm(IAPIClient client)
        {
            var box = new VBox();
            box.PackStart(new Label("Name"));
            box.PackStart(nameBox);
            box.PackStart(new Label("Terrain"));
            box.PackStart(terrainBox);
            box.PackStart(new Label("Area preview"));
            box.PackStart(Image);
            box.PackStart(new Label("Objects"));
            box.PackStart(objectsInArea);
            box.PackStart(Button_Box());

            box.PackEnd(Button_Bar());
            Content = box;
            this.client = client;
        }

        public void ExposeAreaForEditing(ServerAreaToCheck area,Action<ServerAreaToCheck> onSubmit)
        {
            AreaID = area.Id;
            nameBox.Text = area.name;
            terrainBox.Text = area.terrain;
            Image.Image = Utills.GetImageFromBase64(area.image);
            foreach (var item in area.ObjectsInArea)
            {
                objectsInArea.AddObject(client.GetObject(item));
            }
            CreateNewArea(onSubmit);
        }
        public void CreateNewArea(Action<ServerAreaToCheck> onSubmit)
        {
            OnSave = onSubmit;
            Show();
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
            var button = new Button("Edit area");
            button.Clicked += delegate {
                var obj = objectsInArea.GetSelectedObject();
                if (obj != null)
                {
                    new ObjectEditForm().ModifyObject(obj, (obj2) =>
                    {
                        client.UpdateObject(obj2);
                        objectsInArea.EditObject(obj2);
                    });
                }

            };
            return button;
        }
        private Button Remove_Button()
        {
            var button = new Button("Remove area");
            button.Clicked += delegate {
                var area = objectsInArea.GetSelectedObject();
                objectsInArea.RemoveObject(area);
                client.RemoveReport(area.Id);
            };
            return button;
        }
        private Button Add_Button()
        {
            var button = new Button("Add area");
            button.Clicked += delegate {
                new ObjectEditForm().CreateNewObject( (obj) =>
                {
                    var objectWithUpdatedID = client.AddObject(obj);
                    objectsInArea.AddObject(objectWithUpdatedID);
                });
            };
            return button;
        }
        private HBox Button_Bar()
        {
            var box = new HBox();
            box.PackStart(DiscardButton(), true);
            box.PackStart(SaveButton(), true);
            return box;
        }
        private Button SaveButton()
        {
            var btn = new Button("Save");
            btn.Clicked += delegate {
                var area = new ServerAreaToCheck()
                {
                    Id = AreaID,
                    image = Utills.GetBase64FromImage(Image.Image),
                    name = nameBox.Text,
                    terrain = terrainBox.Text,
                    ObjectsInArea = objectsInArea.GetAllObjectsIDs()
                };
                OnSave(area);
                Close();
            };
            return btn;
        }
        private Button DiscardButton()
        {
            var btn = new Button("Discard");
            btn.Clicked += delegate {
                Close();
            };
            return btn;
        }
    }
}
