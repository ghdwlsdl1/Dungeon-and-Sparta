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
            if (Data.weaponEquip[3])
            {
                weaponBonus = Data.weaponStats[3];
            }

            Data.Str = Data.startStr + weaponBonus + Data.Level;
        }

        public void UpdateDex()
        {
            int weaponBonus = 0;
            if (Data.weaponEquip[4])
            {
                weaponBonus += Data.weaponStats[4];
            }

            Data.Dex = Data.startDex + weaponBonus + Data.Level;
        }

        public void UpdateInt()
        {
            int weaponBonus = 0;
            if (Data.weaponEquip[5])
            {
                weaponBonus += Data.weaponStats[5];
            }

            Data.Int = Data.startInt + weaponBonus + Data.Level;
        }

        public void UpdateCon()
        {
            int assistBonus = 0;
            if (Data.assistEquip[5])
            {
                assistBonus += Data.assistStats[5];
            }

            Data.Con = Data.startCon + assistBonus + Data.Level;
        }

        public void UpdateWis()
        {
            int assistBonus = 0;
            if (Data.assistEquip[4])
            {
                assistBonus += Data.assistStats[4];
            }

            Data.Wis = Data.startWis + assistBonus + Data.Level;
        }

        public void UpdateLuk()
        {
            int assistBonus = 0;
            if (Data.assistEquip[3])
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
                if (Data.weaponEquip[i])
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
                if (Data.assistEquip[i])
                {
                    assistBonus += Data.assistDef[i];
                }
            }

            for (int i = 0; i < Data.armorEquip.Length; i++)
            {
                if (Data.armorEquip[i])
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
    }
}
