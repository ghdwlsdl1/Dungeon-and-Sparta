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
            if (Data.weaponEquip[3] == 1)
            {
                weaponBonus += Data.weaponStats[3];
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
            if (Data.weaponEquip[4] == 1)
            {
                weaponBonus += Data.weaponStats[4];
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
            if (Data.weaponEquip[5] == 1)
            {
                weaponBonus += Data.weaponStats[5];
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
            if (Data.assistEquip[5] == 1)
            {
                assistBonus += Data.assistStats[5];
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
            if (Data.assistEquip[4] == 1)
            {
                assistBonus += Data.assistStats[4];
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
            if (Data.assistEquip[3] == 1)
            {
                assistBonus += Data.assistStats[3];
            }

            Data.Luk = Data.startLuk + assistBonus;
        }

        public void UpdateAtk()
        {
            int weaponBonus = 0;

            for (int i = 0; i < Data.weaponEquip.Length; i++)
            {
                if ((Data.weaponEquip[i] == 1))
                {
                    weaponBonus += Data.weaponAtk[i];
                }
            }

            Data.Atk = 1 + Data.Str + weaponBonus;
        }

        public void UpdateDef()
        {
            int assistBonus = 0;
            int armorBonus = 0;

            for (int i = 0; i < Data.assistEquip.Length; i++)
            {
                if ((Data.assistEquip[i] == 1))
                {
                    assistBonus += Data.assistDef[i];
                }
            }

            for (int i = 0; i < Data.armorEquip.Length; i++)
            {
                if ((Data.armorEquip[i] == 1))
                {
                    armorBonus += Data.armorDef[i];
                }
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
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine("");
            Console.WriteLine($"레벨 : {Data.Level}");
            Console.WriteLine($"chad ( {Data.Job[Data.JobNames]} )");
            Console.WriteLine($" 힘   : {Data.Str}");
            Console.WriteLine($"민첩  : {Data.Dex}");
            Console.WriteLine($"지능  : {Data.Int}");
            Console.WriteLine($"체력  : {Data.Con}");
            Console.WriteLine($"지혜  : {Data.Wis}");
            Console.WriteLine($"행운  : {Data.Luk}");
            Console.WriteLine($"공격력: {Data.Atk}");
            Console.WriteLine($"방어력: {Data.Def}");
            Console.WriteLine($"HP : {Data.Hp}/{Data.HpMax}");
            Console.WriteLine($"Gold   : {Data.Money}");
            Console.WriteLine("");
            Console.WriteLine("1.장비교체");
            Console.WriteLine("0.나가기");

            int userinput = MainFrame.UserInputHandler(0, 1);

            switch (userinput)
            {
                case 1:
                    Console.WriteLine("장비 교체");
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;

                case 0:
                    Console.WriteLine("나가겠습니다 테스트용");
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;
            }

        }
    }
}
