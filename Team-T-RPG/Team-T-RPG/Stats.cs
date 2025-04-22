using System;

namespace Team_T_RPG
{
    public class Stats
    {
        // 레벨을 업데이트하는 메서드
        public void UpdateLevel()
        {
            int experienceMax = 10 * Level * Level;  // 최대 경험치 계산
            if (experience >= experienceMax)  // 경험치가 최대 경험치를 초과하면
            {
                Level++;  // 레벨 상승
                experience -= experienceMax;  // 초과한 경험치를 남겨둠
            }
        }

        // 최대 HP를 업데이트하는 메서드
        public void UpdateHpMax()
        {
            HpMax = Con * 2;  // 최대 HP는 체력(Con) * 2
            if (Hp > HpMax)  // 
            {
                Hp = HpMax;  // HP를 최대 HP로 설정
            }
        }

        // 최대 MP를 업데이트하는 메서드
        public void UpdateMpMax()
        {
            MpMax = Wis * 2;  // 최대 MP는 지능(Wis) * 2
            if (Mp > MpMax)  // 
            {
                Mp = MpMax;  // MP를 최대 MP로 설정
            }
        }

        // 힘(Strength)을 업데이트하는 메서드
        public void UpdateStr()
        {
            int weaponBonus = 0;
            if (weaponEquip[3])  // 무기가 장착되어 있으면
            {
                weaponBonus = weaponStats[3];  // 무기에서 제공하는 보너스 추가
            }

            Str = startStr + weaponBonus + Level;  // 기본 힘 + 무기 보너스 + 레벨
        }

        // 민첩(Dexterity)을 업데이트하는 메서드
        public void UpdateDex()
        {
            int weaponBonus = 0;
            if (weaponEquip[4])  // 무기가 장착되어 있으면
            {
                weaponBonus += weaponStats[4];  // 무기에서 제공하는 보너스를 추가
            }

            Dex = startDex + weaponBonus + Level;  // 기본 민첩 + 무기 보너스 + 레벨
        }

        // 지능(Intelligence)을 업데이트하는 메서드
        public void UpdateInt()
        {
            int weaponBonus = 0;
            if (weaponEquip[5])  // 무기가 장착되어 있으면
            {
                weaponBonus += weaponStats[5];  // 무기에서 제공하는 보너스를 추가
            }

            Int = startInt + weaponBonus + Level;  // 기본 지능 + 무기 보너스 + 레벨
        }

        // 체력(Constitution)을 업데이트하는 메서드
        public void UpdateCon()
        {
            int assistBonus = 0;
            if (assistEquip[5])  // 보조 장비가 장착되어 있으면
            {
                assistBonus += assistStats[5];  // 보조 장비에서 제공하는 보너스 추가
            }

            Con = startCon + assistBonus + Level;  // 기본 체력 + 보조 장비 보너스 + 레벨
        }

        // 지혜(Wisdom)을 업데이트하는 메서드
        public void UpdateWis()
        {
            int assistBonus = 0;
            if (assistEquip[4])  // 보조 장비가 장착되어 있으면
            {
                assistBonus += assistStats[4];  // 보조 장비에서 제공하는 보너스 추가
            }
            Wis = startWis + assistBonus + Level;  // 기본 지혜 + 보조 장비 보너스 + 레벨
        }

        // 행운(Luck)을 업데이트하는 메서드
        public void UpdateLuk()
        {
            int assistBonus = 0;
            if (assistEquip[3])  // 보조 장비가 장착되어 있으면
            {
                assistBonus += assistStats[3];  // 보조 장비에서 제공하는 보너스 추가
            }
            Luk = startLuk + assistBonus;  // 기본 행운 + 보조 장비 보너스
        }

        // 공격력(Attack)을 업데이트하는 메서드
        public void UpdateAtk()
        {
            int weaponBonus = 0;

            for (int i = 0; i < weaponEquip.Length; i++)  // 모든 무기 장착 여부를 확인
            {
                if (weaponEquip[i])  // 무기가 장착되어 있으면
                {
                    weaponBonus += weaponAtk[i];  // 무기에서 제공하는 공격력 추가
                }
            }

            Atk = 1 + Str + weaponBonus;  // 기본 공격력 + 힘 + 무기 보너스
        }

        // 방어력(Defense)을 업데이트하는 메서드
        public void UpdateDef()
        {
            int assistBonus = 0;
            int armorBonus = 0;

            for (int i = 0; i < assistEquip.Length; i++)  // 모든 보조 장비 확인
            {
                if (assistEquip[i])  // 보조 장비가 장착되어 있으면
                {
                    assistBonus += assistDef[i];  // 보조 장비에서 제공하는 방어력 추가
                }
            }
            for (int i = 0; i < armorEquip.Length; i++)  // 모든 방어구 확인
            {
                if (armorEquip[i])  // 방어구가 장착되어 있으면
                {
                    armorBonus += armorDef[i];  // 방어구에서 제공하는 방어력 추가
                }
            }
            Def = Con / 3 + armorBonus + assistBonus;  // 체력의 1/3 + 방어구 보너스 + 보조 장비 보너스
        }

        // 모든 스탯을 업데이트하는 메서드
        public void UpdateStats()
        {
            UpdateLevel();  // 레벨 업데이트

            UpdateStr();  // 힘 업데이트
            UpdateDex();  // 민첩 업데이트
            UpdateInt();  // 지능 업데이트
            UpdateCon();  // 체력 업데이트
            UpdateWis();  // 지혜 업데이트
            UpdateLuk();  // 행운 업데이트

            UpdateHpMax();  // 최대 HP 업데이트
            UpdateMpMax();  // 최대 MP 업데이트
            UpdateAtk();  // 공격력 업데이트
            UpdateDef();  // 방어력 업데이트
        }
    }
}
