using System;

namespace Team_T_RPG
{
    public class Stats
    {
        public void UpdateLevel()
        {
            int experienceMax = 10 * Level * Level;
            if (experience >= experienceMax)
            {
                Level++;
                experience -= experienceMax;
            }
        }

        public void UpdateHpMax()
        {
            HpMax = Con * 2;
            if (Hp > HpMax)
            {
                Hp = HpMax;
            }
        }

        public void UpdateMpMax()
        {
            MpMax = Wis * 2;
            if (Mp > MpMax)
            {
                Mp = MpMax;
            }
        }

        public void UpdateStr()
        {
            int weaponBonus = 0;
            if (weaponEquip[3])
            {
                weaponBonus = weaponStats[3];
            }

            Str = startStr + weaponBonus + Level;
        }

        public void UpdateDex()
        {
            int weaponBonus = 0;
            if (weaponEquip[4])
            {
                weaponBonus += weaponStats[4];
            }

            Dex = startDex + weaponBonus + Level;
        }

        public void UpdateInt()
        {
            int weaponBonus = 0;
            if (weaponEquip[5])
            {
                weaponBonus += weaponStats[5];
            }

            Int = startInt + weaponBonus + Level;
        }

        public void UpdateCon()
        {
            int assistBonus = 0;
            if (assistEquip[5])
            {
                assistBonus += assistStats[5];
            }

            Con = startCon + assistBonus + Level;
        }

        public void UpdateWis()
        {
            int assistBonus = 0;
            if (assistEquip[4])
            {
                assistBonus += assistStats[4];
            }

            Wis = startWis + assistBonus + Level;
        }

        public void UpdateLuk()
        {
            int assistBonus = 0;
            if (assistEquip[3])
            {
                assistBonus += assistStats[3];
            }

            Luk = startLuk + assistBonus;
        }

        public void UpdateAtk()
        {
            int weaponBonus = 0;

            for (int i = 0; i < weaponEquip.Length; i++)
            {
                if (weaponEquip[i])
                {
                    weaponBonus += weaponAtk[i];
                }
            }

            Atk = 1 + Str + weaponBonus;
        }

        public void UpdateDef()
        {
            int assistBonus = 0;
            int armorBonus = 0;

            for (int i = 0; i < assistEquip.Length; i++)
            {
                if (assistEquip[i])
                {
                    assistBonus += assistDef[i];
                }
            }

            for (int i = 0; i < armorEquip.Length; i++)
            {
                if (armorEquip[i])
                {
                    armorBonus += armorDef[i];
                }
            }

            Def = Con / 3 + armorBonus + assistBonus;
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