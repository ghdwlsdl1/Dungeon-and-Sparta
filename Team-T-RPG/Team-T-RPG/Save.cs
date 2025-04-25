using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Team_T_RPG
{
    public class TempData
        {
        [JsonInclude]
        public string Name{ get; set; }
        public int JobNames{ get; set; }
        public string[] Job{ get; set; }
        public int Hp { get; set; }
        public int HpMax { get; set; }
        public int Mp{ get; set; }
            public int MpMax { get; set; }
        public int Level { get; set; }
        public int experience { get; set; }

        public int Str { get; set; }
        public int Dex { get; set; }
        public int Int { get; set; }
        public int Con { get; set; }
        public int Wis { get; set; }
        public int Luk { get; set; }
        public int startStr { get; set; }
        public int startDex { get; set; }
        public int startInt { get; set; }
        public int startCon { get; set; }
        public int startWis { get; set; }
        public int startLuk { get; set; }

        public int Atk { get; set; }
        public int Def { get; set; }

        public int Money { get; set; }
        //====================마을 시스템====================
        public bool duty { get; set; }
        public int Day { get; set; }
        //====================아이탬====================
        public string[] weapon { get; set; }
        public int[] weaponTf { get; set; }
        public int[] weaponEquip { get; set; }
        public int[] weaponAtk { get; set; }
        //public static int[] weaponStats = { 0, 0, 0, 5, 5, 7, 0, 0, 0 }; // 추가 스텟
        public int[][] weaponStats { get; set; }
        public int[] weaponDeal { get; set; }

        public string[] assist { get; set; }
        public int[] assistTf { get; set; }
        public int[] assistEquip { get; set; }
        public int[] assistDef { get; set; }
        //public static int[] assistStats = { 0, 0, 0, 3, 5, 5 }; // 추가 스텟
        public int[][] assistStats { get; set; }
       
            public string[] armor { get; set; }
        public int[] armorTf { get; set; }
        public int[] armorEquip { get; set; }
        public int[] armorDef { get; set; }
        public int[] armorDeal { get; set; }

        public string[] potion { get; set; }
        public int[] potionTf { get; set; }
        public int[] potionMp { get; set; }
        public int[] potionHp { get; set; }
        public int[] potionDeal { get; set; }
        //====================몬스터====================

        public string[] monster { get; set; }
        public int[] monster_drop_weapon_index { get; set; }
        public int[] msAtk { get; set; }
        public int[] msHp { get; set; }
        public int[] msDex { get; set; }

    }


    public static class SaveSystem
    {
        public static void Save()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save.json");
            TempData temp = new TempData();
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };
            CopyStaticToInstance(temp);
            string json = JsonSerializer.Serialize(temp, options);
            File.WriteAllText(path, json);
            Console.WriteLine(temp.Money);
            Console.WriteLine(json);
            Console.WriteLine(path);
        }

        public static void Load()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save.json");
            Art.MakeImage("Image/dungeon.png", width: 60);
            MainFrame.SerialTextWrite("세이브파일을 불러오는 중...");
                string json = File.ReadAllText(path);
                TempData loaded = JsonSerializer.Deserialize<TempData>(json);
                CopyInstanceToStatic(loaded);
        }

        public static void CopyStaticToInstance(object target)
        {
            var staticFields = typeof(Data).GetFields(BindingFlags.Static | BindingFlags.Public);
            var instanceProperties = target.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var staticField in staticFields)
            {
                var value = staticField.GetValue(null);
                Console.WriteLine($"[STATIC → TEMP] {staticField.Name} = {value}");

                var targetProp = instanceProperties.FirstOrDefault(p =>
                    p.Name == staticField.Name && p.PropertyType == staticField.FieldType && p.CanWrite);

                if (targetProp != null)
                {
                    targetProp.SetValue(target, value);
                }
            }
        }

        public static void CopyInstanceToStatic(object source)
        {
            var staticFields = typeof(Data).GetFields(BindingFlags.Static | BindingFlags.Public);
            var instanceProperties = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var prop in instanceProperties)
            {
                var value = prop.GetValue(source);
                Console.WriteLine($"[TEMP → STATIC] {prop.Name} = {value}");

                var targetField = staticFields.FirstOrDefault(f =>
                    f.Name == prop.Name && f.FieldType == prop.PropertyType);

                if (targetField != null)
                {
                    targetField.SetValue(null, value);
                }
            }
        }
    }

}
