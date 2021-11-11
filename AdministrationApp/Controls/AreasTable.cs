using System;
using System.Collections.Generic;
using Xwt;
using Pix_API.ChecklistReviewerApp.Interfaces.Models;
using System.Linq;
namespace AdministrationApp
{
    public class AreasTable:TreeView
    {
        internal static DataField<string> Name = new DataField<string>();
        internal static DataField<int> Id = new DataField<int>();
        private List<TerrainDescriptor> Terrain = new List<TerrainDescriptor>();
        private TreeStore store;
        public AreasTable(List<ServerAreaToCheck> areas)
        {
            store = new TreeStore(Id,Name);
            DataSource = store;
            Columns.Add("Name", Name).CanResize = true;
            Columns.Add("ID", Id);

            foreach (var item in areas)
            {
                AddArea(item);
            }
        }

        public void AddArea(ServerAreaToCheck area)
        {
            var terrain = Terrain.FirstOrDefault(s => s.Name == area.terrain);
            if(terrain == null)
            {
                terrain = new TerrainDescriptor(area.terrain, store.AddNode());
                Terrain.Add(terrain);
            }
            terrain.AddArea(area);
        }
        public void RemoveArea(ServerAreaToCheck area)
        {
            var terrain = Terrain.FirstOrDefault(s => s.Name == area.terrain);
            if (terrain != null)
            {
                terrain.RemoveArea(area);
            }
        }
        public void EditArea(ServerAreaToCheck area,string PreviousTerrain)
        {
            Terrain.First(s => s.Name == PreviousTerrain).RemoveArea(area);
            AddArea(area);
        }
        public ServerAreaToCheck GetSelectedArea()
        {
            var row = store.GetNavigatorAt(SelectedRow);
            foreach (var item in Terrain.SelectMany(s => s.areas))
            {
                if (item.Area.Id == row.GetValue(Id)) return item.Area;
            }
            return null;
        }
    }

    class TerrainDescriptor
    {
        public TerrainDescriptor(string name,TreeNavigator navigator)
        {
            Name = name;
            Navigator = navigator;
            navigator.SetValues(AreasTable.Name, name,AreasTable.Id,-1);
        }
        public void AddArea(ServerAreaToCheck area)
        {
            var child = Navigator.AddChild();
            child.SetValues(AreasTable.Name, area.name,AreasTable.Id,area.Id);
            areas.Add(new AreaDescriptor(area, child.Clone()));
            Navigator.MoveToParent();
        }
        public void RemoveArea(ServerAreaToCheck area)
        {
            var desc = areas.FirstOrDefault(s => s.Area.Id == area.Id);
            if(desc != null)
            {
                desc.Navigator.Remove();
                areas.Remove(desc);
            }
        }
    
        public string Name { get; }
        public TreeNavigator Navigator { get; }
        public List<AreaDescriptor> areas { get; } = new List<AreaDescriptor>();
    }
    class AreaDescriptor
    {
        public AreaDescriptor(ServerAreaToCheck area,TreeNavigator navigator)
        {
            Area = area;
            Navigator = navigator;
        }

        public ServerAreaToCheck Area { get; }
        public TreeNavigator Navigator { get; }
    }
}
