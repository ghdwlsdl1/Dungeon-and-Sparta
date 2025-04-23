using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_T_RPG
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Item(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        //public override string ToString()
        //{
        //    return $"{Name} - {Description}";
        //}

    }
    public static class ItemDatabase
    {
        public static List<Item> Items = new List<Item>()   
        {
            new Item(1, "체력 포션", "HP를 50 회복한다."),
            new Item(2, "철검", "공격력을 약간 올려주는 기본 검."),
            new Item(3, "모험가의 망토", "방어력을 소폭 상승시킨다.")
        };

        public static Item FindItem(int id)
        {
            return Items.Find(item => item.Id == id);
        }
    }
}

// 사용 예시

