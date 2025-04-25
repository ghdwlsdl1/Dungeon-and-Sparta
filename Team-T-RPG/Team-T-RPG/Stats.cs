using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Team_T_RPG
{
    public class Stats
    {
        public int weaponNumFind()
        {
            for (int i = 0; i < Data.weaponEquip.Length; i++)
            {
                if (Data.weaponEquip[i] == 1)
                    return i;
            }
            return -1;
        }
        public int assistNumFind()
        {
            for (int i = 0; i < Data.assistEquip.Length; i++)
            {
                if (Data.assistEquip[i] == 1)
                    return i;
            }
            return -1;
        }
        public int armorNumFind()
        {
            for (int i = 0; i < Data.armorEquip.Length; i++)
            {
                if (Data.armorEquip[i] == 1)
                    return i;
            }
            return -1;
        }
        public void UpdateLevel()
        {
            int experienceMax = 10 * Data.Level * Data.Level;
            if (Data.experience >= experienceMax)
            {
                Data.Level++;
                Data.experience -= experienceMax;
            }
        }

        public void UpdateHpMax()
        {
            Data.HpMax = Data.Con * 2;
            if (Data.Hp > Data.HpMax)
            {
                Data.Hp = Data.HpMax;
            }
        }

        public void UpdateMpMax()
        {
            Data.MpMax = Data.Wis * 2;
            if (Data.Mp > Data.MpMax)
            {
                Data.Mp = Data.MpMax;
            }
        }

        public void UpdateStr()
        {

            int weaponBonus = 0;
            int weaponIndex = weaponNumFind();
            if (weaponIndex != -1 &&Data.weaponEquip[weaponNumFind()] == 1)
            {
                weaponBonus += Data.weaponStats[weaponNumFind()][0];
            }

            if (Data.tired >= 20)
            {
                Data.Str = (Data.startStr + weaponBonus + Data.Level)
                * Math.Max(5, 100 - Math.Min((Data.tired >= 20 ? (Data.tired - 20) * 5 : 0), 95)) / 100;
            }
            else
            {
                Data.Str = Data.startStr + weaponBonus + Data.Level;
            }
        }

        public void UpdateDex()
        {
            int weaponBonus = 0;
            int weaponIndex = weaponNumFind();
            if (weaponIndex != -1 && Data.weaponEquip[weaponNumFind()] == 1)
            {
                weaponBonus += Data.weaponStats[weaponNumFind()][1];
            }

            if (Data.tired >= 20)
            {
                Data.Dex = (Data.startDex + weaponBonus + Data.Level)
                * Math.Max(5, 100 - Math.Min((Data.tired - 20) * 5, 95)) / 100;
            }
            else
            {
                Data.Dex = Data.startDex + weaponBonus + Data.Level;
            }
        }

        public void UpdateInt()
        {
            int weaponBonus = 0;
            int weaponIndex = weaponNumFind();
            if (weaponIndex != -1 && Data.weaponEquip[weaponNumFind()] == 1)
            {
                weaponBonus += Data.weaponStats[weaponNumFind()][2];
            }

            if (Data.tired >= 20)
            {
                Data.Int = (Data.startInt + weaponBonus + Data.Level)
                * Math.Max(5, 100 - Math.Min((Data.tired - 20) * 5, 95)) / 100;
            }
            else
            {
                Data.Int = Data.startInt + weaponBonus + Data.Level;
            }
        }

        public void UpdateCon()
        {
            int assistBonus = 0;
            int assistIndex = assistNumFind();
            if (assistIndex != -1&& Data.assistEquip[assistNumFind()] == 1)
            {
                assistBonus += Data.assistStats[assistNumFind()][0];
            }

            if (Data.tired >= 20)
            {
                Data.Con = (Data.startCon + assistBonus + Data.Level)
                * Math.Max(5, 100 - Math.Min((Data.tired - 20) * 5, 95)) / 100;
            }
            else
            {
                Data.Con = Data.startCon + assistBonus + Data.Level;
            }
        }

        public void UpdateWis()
        {
            int assistBonus = 0;
            int assistIndex = assistNumFind();
            if (assistIndex != -1 && Data.assistEquip[assistNumFind()] == 1)
            {
                assistBonus += Data.assistStats[assistNumFind()][1];
            }

            if (Data.tired >= 20)
            {
                Data.Wis = (Data.startWis + assistBonus + Data.Level)
                * Math.Max(5, 100 - Math.Min((Data.tired - 20) * 5, 95)) / 100;
            }
            else
            {
                Data.Wis = Data.startWis + assistBonus + Data.Level;
            }
        }

        public void UpdateLuk()
        {
            int assistBonus = 0;
            int assistIndex = assistNumFind();
            if (assistIndex != -1 && Data.assistEquip[assistNumFind()] == 1)
            {
                assistBonus += Data.assistStats[assistNumFind()][2];
            }

            Data.Luk = Data.startLuk + assistBonus;
        }

        public void UpdateAtk()
        {
            int weaponBonus = 0;

            int weaponIndex = assistNumFind();
            if (weaponIndex != -1 && Data.weaponEquip[weaponNumFind()] == 1)
            {
                weaponBonus += Data.weaponAtk[weaponNumFind()];
            }


            Data.Atk = 1 + Data.Str + weaponBonus;
        }

        public void UpdateDef()
        {
            int assistBonus = 0;
            int armorBonus = 0;


            int assistIndex = assistNumFind();
            if (assistIndex != -1 && Data.assistEquip[assistNumFind()] == 1)
            {
                assistBonus += Data.assistDef[assistNumFind()];
            }


            int armorIndex = armorNumFind();
            if (armorIndex != -1 && Data.armorEquip[armorNumFind()] == 1)
            {
                armorBonus += Data.armorDef[armorNumFind()];
            }


            Data.Def = Data.Con / 3 + armorBonus + assistBonus;
        }

        public void UpdateStats()
        {
            UpdateLevel();

            UpdateStr();
            UpdateDex();
            UpdateInt();
            UpdateCon();
            UpdateWis();
            UpdateLuk();

            UpdateHpMax();
            UpdateMpMax();
            UpdateAtk();
            UpdateDef();
        }

        public void ShowStatTable()
        {
            UpdateStats();
            Console.Clear();
            Art.MakeImage("Image/Stats.png", width: 60);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[상태 보기]");
            Console.ResetColor();
            Console.WriteLine("");
            Console.WriteLine($"레벨 : {Data.Level}");
            Console.WriteLine($"캐릭터명 : {Data.Name} ");
            Console.WriteLine($"직업  : {Data.Job[Data.JobNames]}");
            Console.WriteLine($" 힘   : {Data.Str}");
            Console.WriteLine($"민첩  : {Data.Dex}");
            Console.WriteLine($"지능  : {Data.Int}");
            Console.WriteLine($"체력  : {Data.Con}");
            Console.WriteLine($"지혜  : {Data.Wis}");
            Console.WriteLine($"행운  : {Data.Luk}");
            Console.WriteLine($"공격력: {Data.Atk}");
            Console.WriteLine($"방어력: {Data.Def}");
            Console.WriteLine($"HP : {Data.Hp}/{Data.HpMax}");
            Console.WriteLine($"MP : {Data.Mp}/{Data.MpMax}");
            Console.WriteLine($"Gold   : {Data.Money}");
            Console.WriteLine("");
            Console.WriteLine("0.나가기");

            int userinput = MainFrame.UserInputHandler(0, 0);

            switch (userinput)
            {
                //case 1:
                //    Console.WriteLine("장비 교체");
                //    Thread.Sleep(1000);
                //    Console.Clear();
                //    break;

                case 0:
                    Console.Clear();
                    break;
            }

        }
    }
}
