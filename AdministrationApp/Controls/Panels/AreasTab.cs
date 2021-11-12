using System;
using Xwt;
using System.Collections.Generic;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;

namespace AdministrationApp.Controls.Panels
{
    public class AreasTab:VBox
    {
        private AreasTable areasTable;
        private readonly IAPIClient client;

        public AreasTab(IAPIClient client)
        {
            areasTable = new AreasTable(client.GetAllAreasToCheck());
            PackStart(areasTable, true);
            PackEnd(Button_Box());
            this.client = client;
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
                var area = areasTable.GetSelectedArea();
                if (area != null)
                {
                    var previousTerrain = area.terrain;
                    new AreaEditForm(client).ExposeAreaForEditing(area, (obj) =>
                    {
                        client.EditArea(obj);
                        areasTable.EditArea(obj, previousTerrain);
                    });
                }

            };
            return button;
        }
        private Button Remove_Button()
        {
            var button = new Button("Remove area");
            button.Clicked += delegate {
                var area = areasTable.GetSelectedArea();
                areasTable.RemoveArea(area);
                client.RemoveArea(area.Id);
            };
            return button;
        }
        private Button Add_Button()
        {
            var button = new Button("Add area");
            button.Clicked += delegate {
                new AreaEditForm(client).CreateNewArea((obj) =>
                {
                    var areaWithUpdatedID = client.AddArea(obj);
                    areasTable.AddArea(areaWithUpdatedID);
                });
            };
            return button;
        }
    }
}
