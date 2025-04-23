using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Team_T_RPG;

namespace Team_T_RPG
{
    public class Mob
    {
        public int Id { get; set; }                 // 몬스터 고유 ID
        public string Name { get; set; }            // 이름 (ex. 슬라임)
        public int MaxHp { get; set; }              // 최대 HP
        public int Attack { get; set; }             // 공격력
        public string Description { get; set; }     // 설명 (ex. 말랑한 슬라임)

        public Mob(int id, string name, int maxHp, int attack, string description)
        {
            Id = id;
            Name = name;
            MaxHp = maxHp;
            Attack = attack;
            Description = description;
        }

        //public override string ToString()
        //{
        //    return $"{Name} (HP: {MaxHp}, ATK: {Attack}) - {Description}";
        //}
    }
    public static class MobDatabase
    {
        public static List<Mob> Mobs = new List<Mob>()
        {
            new Mob(1, "슬라임", 30, 5, "말랑하고 약한 몬스터"),
            new Mob(2, "고블린", 50, 12, "작고 사나운 생명체"),
            new Mob(3, "스켈레톤", 80, 15, "해골로 된 전사")
        };

        public static Mob FindMob(int id)
        {
            return Mobs.Find(mob => mob.Id == id);
        }
    }
}

// 사용 예시
// Mob slime = MobDatabase.FindMob(1);
//Console.WriteLine(slime);
