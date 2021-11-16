using System;
using System.Collections.Generic;
using System.Linq;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using Xwt;

namespace AdministrationApp.Controls
{
    public class AreaSelector : TreeView
    {
        internal DataField<string> NameField = new DataField<string>();
        internal DataField<bool> IsSelectedField = new DataField<bool>();
        private List<TerrainDescriptor> TerrainField = new List<TerrainDescriptor>();
        private CheckBoxCellView ChechedView;
        private TreeStore store;
        public AreaSelector()
        {
            ChechedView = new CheckBoxCellView(IsSelectedField)
            {
                Editable = true
            };
            store = new TreeStore(NameField,IsSelectedField);
            DataSource = store;
            Columns.Add("Name", NameField).CanResize = true;
            Columns.Add(new ListViewColumn("Checked", ChechedView));


        }
        public void CheckMany(List<ServerAreaToCheck> areas, List<int> checkedAreaIDs)
        {
            foreach (var item in areas)
            {
                AddArea(item, checkedAreaIDs.Any(s => s == item.Id));
            }
        }
        private void AddArea(ServerAreaToCheck area,bool checkArea)
        {
            var terrain = TerrainField.FirstOrDefault(s => s.Name == area.terrain);
            if (terrain == null)
            {
                terrain = new TerrainDescriptor(area.terrain, store.AddNode(),NameField,IsSelectedField);
                TerrainField.Add(terrain);
            }
            terrain.AddArea(area,checkArea);
        }

        public List<ServerAreaToCheck> GetCheckedAreas()
        {
            return TerrainField.SelectMany(s => s.areas)
                .Where(s => s.IsChecked())
                .Select(s => s.Area)
                .ToList();
        }
    }

    class TerrainDescriptor
    {
        private readonly IDataField<string> nameField;
        private readonly IDataField<bool> isSelected;

        public TerrainDescriptor(string name, TreeNavigator navigator,IDataField<string> NameField,IDataField<bool> IsSelected)
        {
            Name = name;
            Navigator = navigator;
            nameField = NameField;
            isSelected = IsSelected;
            navigator.SetValue(NameField, name);
        }
        public void AddArea(ServerAreaToCheck area,bool isAreaChecked)
        {
            var child = Navigator.AddChild();
            child.SetValues(nameField, area.name, isSelected,isAreaChecked);
            areas.Add(new AreaDescriptor(area, child.Clone(),isSelected));
            Navigator.MoveToParent();
        }
        public string Name { get; }
        public TreeNavigator Navigator { get; }
        public List<AreaDescriptor> areas { get; } = new List<AreaDescriptor>();
    }
    class AreaDescriptor
    {
        private readonly IDataField<bool> isSelected;

        public AreaDescriptor(ServerAreaToCheck area, TreeNavigator navigator,IDataField<bool> IsSelected)
        {
            Area = area;
            Navigator = navigator;
            isSelected = IsSelected;
        }
        public bool IsChecked()
        {
            return Navigator.GetValue(isSelected);
        }
        public ServerAreaToCheck Area { get; }
        public TreeNavigator Navigator { get; }
    }
}




