using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectorApp.Models
{
    internal class Item
    {
        public Dictionary<string, dynamic> Data { get; set; }

        public Item() {
            Data.Add("Name", "");
            Data.Add("Price", 0);
            Data.Add("State", "");
            Data.Add("Rating", 0);
            Data.Add("Comment", "");
            Data.Add("Picture", "");
        }
    }
}
