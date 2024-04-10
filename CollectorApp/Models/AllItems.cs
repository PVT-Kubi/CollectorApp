using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorApp.Models
{
    internal class AllItems
    {
        public ObservableCollection<Item> Items { get; set; } = new ObservableCollection<Item>();
        public Dictionary<string, string> ItemTypes { get; set; } = new Dictionary<string, string>();
        public List<string> columnTypes { get; set; } = new List<string>();
        public string collectionName;
        public AllItems(string collectionName) {
            this.collectionName = collectionName;
            this.columnTypes.Add("string");
            this.columnTypes.Add("integer");
            this.columnTypes.Add("multiple-choice");
            LoadItems(collectionName);
        }

        public string getColllectionName() { return this.collectionName; }

        public void sortSelledItems(string path)
        {
            string text = File.ReadAllText(path);
            string[] itemsData = text.Split('\n');
            List<string>forwardItems = new List<string>();
            List<string> lastItems = new List<string>();
            foreach (string item in itemsData) 
            {
                string[] itemData = item.Split('\t');
                if (itemData.Length > 1)
                {
                    if (itemData[2] == "sprzedany")
                        lastItems.Add(item);
                    else
                        forwardItems.Add(item);
                }
                else
                {
                    forwardItems.Add(item);
                }

            }

            forwardItems.AddRange(lastItems);
            string new_text = string.Join("\n", forwardItems);
            if(File.Exists(path))
            {
                File.WriteAllText(path, new_text);
            }
            
        }

        public void LoadItems(string cName)
        {
            Items.Clear();
            ItemTypes.Clear();

            string path = FileSystem.AppDataDirectory;
            List<Item> items= new List<Item>();

            if (File.Exists(Path.Combine(path, $"{cName}.cl.txt")))
            {
                
                sortSelledItems(Path.Combine(path, $"{cName}.cl.txt"));
                string text = File.ReadAllText(Path.Combine(path, $"{cName}.cl.txt"));
                string[] itemsData = text.Split('\n');
                List<string> itemColumns = new List<string>();

                for(int j = 0; j < itemsData.Length; j++)
                {
                    string[] itemData = itemsData[j].Split("\t");
                    if (j == 1) 
                        itemColumns.AddRange(itemData);
                    else if(j > 1)
                    {
                        Item item = new Item();
                        
                        for(int k = 0; k < itemData.Length; k++)
                        {
                            if (item.Data.ContainsKey(itemColumns[k].Split(";")[0]))
                            {
                                item.Data[itemColumns[k].Split(";")[0]] = itemData[k];
                            }
                            else
                            {
                                item.Data.Add(itemColumns[k].Split(";")[0], itemData[k]);
                            }
                        }
                        items.Add(item);
                    }


                }
                foreach(string column in itemColumns)
                {
                    string[] columnSplit = column.Split(";");
                    if (!ItemTypes.ContainsKey(columnSplit[0]))
                        ItemTypes.Add(columnSplit[0], columnSplit[1]);
                }

                //i know it's stupid but i don't have energy to use brain anymore
                Item lastItem = new Item();
                foreach (Item item in items)
                {
                    Items.Add(item);
                }

            }
        }

        public int AddItem(string cName, Dictionary<string, dynamic> data)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{cName}.cl.txt");
            if (!File.Exists(path))
                return 1;

            Item item = new Item();
            item.Data = data;
            string text = File.ReadAllText(path);
            string itemData = text;
            bool first= false;
            foreach(var i in data)
            {
                if (!first)
                {
                    itemData += $"\n{i.Value}";
                    first = true;
                }
                else
                {
                    itemData += $"\t{i.Value}";
                }
            }

            File.WriteAllText(path, itemData);
            Items.Add(item);
            return 0;
        }

        public Item getItem(string iName)
        {
            foreach (Item item in Items)
            {
                if (item.Data["Name"] == iName)
                {
                    return item;
                }
            }
            return null;
        }

        public int getItemIndex(string iName)
        {

            foreach (Item item in Items)
            {
                if (item.Data["Name"] == iName)
                {
                    return Items.IndexOf(item);
                }
            }
            return 0;
           
        }

        public int DeleteItem(string iName)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{collectionName}.cl.txt");
            if (!File.Exists(path))
                return 1;
            string text = File.ReadAllText(path);
            string[] itemsData = text.Split('\n');
            List<string> newItems = new List<string>();
            for (int i = 0; i < itemsData.Length; i++)
            {
                if (itemsData[i].Split('\t')[0] != iName)
                {
                    newItems.Add(itemsData[i]);
                }
            }
            string newData = string.Join("\n", newItems);
            File.WriteAllText(path, newData);
            return 0;
        }

        public int EditItem(string cName, Dictionary<string, dynamic> data, int itemIndex)
        {
            string path = Path.Combine(FileSystem.AppDataDirectory, $"{cName}.cl.txt");
            if (!File.Exists(path))
                return 1;

            Item item = new Item();
            item.Data = data;
            string text = File.ReadAllText(path);
            string[] itemsData = text.Split('\n');
            for(int i = 0; i < itemsData.Length; i++)
            {
                if(i == itemIndex + 2)
                {
                    itemsData[i] = "";
                    bool first = true;
                    foreach(var itemData in data)
                    {
                        if (first)
                        {
                            itemsData[i] += $"{itemData.Value}";
                            first = false;
                        }
                        else
                        {
                            itemsData[i] += $"\t{itemData.Value}";
                        }
                    }

                }
            }
            string newData = string.Join("\n", itemsData);
            File.WriteAllText(path, newData);
            Items[itemIndex] = item;
            return 0;
        }

        public void AddColumn(string cName, string columnName, string columnType)
        {
           
            if(!ItemTypes.ContainsKey(columnName))
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, $"{cName}.cl.txt");
                string text = File.ReadAllText(path);
                string newText = "";
                string[] itemData = text.Split('\n');
                int counter = 0;
                foreach(string s in itemData)
                {
                    if (counter == 1)
                    {
                        newText += "\n" + s + $"\t{columnName};{columnType}";
                        counter ++;
                    }
                    else if(counter > 1)
                    {
                        newText +=  "\n" + s +"\t";
                    }
                    else
                    {
                        newText += s;
                        counter++;
                    }
                }
                ItemTypes.Add(columnName, columnType);
                
                File.WriteAllText (path, newText);
            }
            
        }
    }
}
