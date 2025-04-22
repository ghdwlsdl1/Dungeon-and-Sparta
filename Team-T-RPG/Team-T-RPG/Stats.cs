using System;

namespace Team_T_RPG
{
    public class Stats
    {
        public  void UpdateLevel()
        {
            int experienceMax = 10 * Data.Level * Data.Level;
            if (Data.experience >= experienceMax)
            {
                Data.Level++;
                Data.experience -= experienceMax;
            }
        }
        public  void UpdateHpMax()
        {
            Data.HpMax = Data.Con * 2;  // 최대 HP는 체력(Con) * 2
            if (Data.Hp > Data.HpMax)  // 최대 HP가 현재 HP를 초과하면
            {
                Data.Hp = Data.HpMax;  // HP를 최대 HP로 설정
            }
        }
        // 최대 MP를 업데이트하는 메서드
        public  void UpdateMpMax()
        {
            Data.MpMax = Data.Wis * 2;  // 최대 MP는 지능(Wis) * 2
            if (Data.Mp > Data.MpMax)  // 최대 MP가 현재 MP를 초과하면
            {
                Data.Mp = Data.MpMax;  // MP를 최대 MP로 설정
            }
        }
        // 힘(Strength)을 업데이트하는 메서드
        public  void UpdateStr()
        {
            int weaponBonus = 0;
            int weaponEquipNum = GetEquippedWeaponIndex();

            if (weaponEquipNum >= 0)
            {
                weaponBonus = Data.weaponStats[weaponEquipNum];
            }


            Data.Str = Data.startStr + weaponBonus + Data.Level;  // 기본 힘 + 무기 보너스 + 레벨
        }

        // 민첩(Dexterity)을 업데이트하는 메서드
        public  void UpdateDex()
        {
            int weaponBonus = GetWeaponBonus();
            Data.Dex = Data.startDex + weaponBonus + Data.Level;  // 기본 민첩 + 무기 보너스 + 레벨  int weaponEquipNum = 0;
        }

        // 지능(Intelligence)을 업데이트하는 메서드
        public  void UpdateInt()
        {
            int weaponBonus = GetWeaponBonus();
            Data.Int = Data.startInt + weaponBonus + Data.Level;  // 기본 지능 + 무기 보너스 + 레벨
        }

        // 체력(Constitution)을 업데이트하는 메서드
        public  void UpdateCon()
        {
            int assistBonus = 0;
            if (Data.assistEquip[5])  // 보조 장비가 장착되어 있으면
            {
                assistBonus += Data.assistStats[5];  // 보조 장비에서 제공하는 보너스 추가
            }

            Data.Con = Data.startCon + assistBonus + Data.Level;  // 기본 체력 + 보조 장비 보너스 + 레벨
        }

        // 지혜(Wisdom)을 업데이트하는 메서드
        public  void UpdateWis()
        {
            int assistBonus = 0;
            if (Data.assistEquip[4])  // 보조 장비가 장착되어 있으면
            {
                assistBonus += Data.assistStats[4];  // 보조 장비에서 제공하는 보너스 추가
            }
            Data.Wis = Data.startWis + assistBonus + Data.Level;  // 기본 지혜 + 보조 장비 보너스 + 레벨
        }

        // 행운(Luck)을 업데이트하는 메서드
        public  void UpdateLuk()
        {
            int assistBonus = 0;
            if (Data.assistEquip[3])  // 보조 장비가 장착되어 있으면
            {
                assistBonus += Data.assistStats[3];  // 보조 장비에서 제공하는 보너스 추가
            }
            Data.Luk = Data.startLuk + assistBonus;  // 기본 행운 + 보조 장비 보너스
        }

        // 공격력(Attack)을 업데이트하는 메서드
        public  void UpdateAtk()
        {
            int weaponBonus = 0;

            for (int i = 0; i < Data.weaponEquip.Length; i++)  // 모든 무기 장착 여부를 확인
            {
                if (Data.weaponEquip[i])  // 무기가 장착되어 있으면
                {
                    weaponBonus += Data.weaponAtk[i];  // 무기에서 제공하는 공격력 추가
                }
            }

            Data.Atk = 1 + Data.Str + weaponBonus;  // 기본 공격력 + 힘 + 무기 보너스
        }
        // 방어력(Defense)을 업데이트하는 메서드
        public  void UpdateDef()
        {
            int assistBonus = 0;
            int armorBonus = 0;

            for (int i = 0; i < Data.assistEquip.Length; i++)  // 모든 보조 장비 확인
            {
                if (Data.assistEquip[i])  // 보조 장비가 장착되어 있으면
                {
                    assistBonus += Data.assistDef[i];  // 보조 장비에서 제공하는 방어력 추가
                }
            }
            for (int i = 0; i < Data.armorEquip.Length; i++)  // 모든 방어구 확인
            {
                if (Data.armorEquip[i])  // 방어구가 장착되어 있으면
                {
                    armorBonus += Data.armorDef[i];  // 방어구에서 제공하는 방어력 추가
                }
            }
            Data.Def = Data.Con / 3 + armorBonus + assistBonus;  // 체력의 1/3 + 방어구 보너스 + 보조 장비 보너스
        }
        public  int GetEquippedWeaponIndex()
        {
            for (int i = 0; i < Data.weaponEquip.Length; i++)
            {
                if (Data.weaponEquip[i])//장착한 무기는 true해뒀기에 true를 찾는다
                {
                    return i; // 첫 번째 true인 인덱스를 반환
                }
            }
            return -1; // 장착된 무기가 없으면 -1 반환
        }
        public  int GetWeaponBonus()
        {
            int weaponEquipNum = GetEquippedWeaponIndex();

            if (weaponEquipNum >= 0)
            {
                return Data.weaponStats[weaponEquipNum];
            }

            return 0; // 장착된 무기가 없으면 보너스는 0
        }
        public  void testInven()
        {
            for (int i = 0; i < Data.weapon.Length; i++)
            {
                if (Data.weaponTf[i] >= 0)
                {
                    string equippedMark = Data.weaponEquip[i] ? " (장착중)" : "";
                    if (i == 0)
                    {
                        // weapon[0]은 장비 없음이므로 갯수 출력 제외
                        Console.WriteLine($"{i}. {Data.weapon[i]}{equippedMark} 공격력: {Data.weaponAtk[i]}  스탯+: {Data.weaponStats[i]}{equippedMark}");
                    }
                    else
                    {
                        // 나머지 무기는 상세 정보 출력
                        Console.WriteLine($"{i}. {Data.weapon[i]} 소지 갯수: {Data.weaponTf[i]}  공격력: {Data.weaponAtk[i]}  스탯+: {Data.weaponStats[i]}{equippedMark}");
                    }
                }
            }
            Console.Write("\n장착할 무기 번호를 입력하세요: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int selectedIndex))
            {
                if (selectedIndex > 0 && selectedIndex < Data.weapon.Length && Data.weaponTf[selectedIndex] > 0)
                {
                    // 기존 무기 해제
                    for (int i = 0; i < Data.weaponEquip.Length; i++)
                    {
                        Data.weaponEquip[i] = false;
                    }

                    // 선택한 무기 장착
                    Data.weaponEquip[selectedIndex] = true;

                    Console.WriteLine($"\n▶ [{Data.weapon[selectedIndex]}]를 장착했습니다!");
                }
                else if (selectedIndex == 0)
                {
                    for (int i = 0; i < Data.weaponEquip.Length; i++)
                    {
                        Data.weaponEquip[i] = false;
                    }
                    Data.weaponEquip[0] = true;
                    Console.WriteLine($"\n▶ [{Data.weapon[selectedIndex]}]을 선택해 장비가 장착 해제되었습니다!");

                }
                else
                {
                    Console.WriteLine("※ 잘못된 선택이거나 해당 무기를 소지하고 있지 않습니다.");
                }
            }
            else
            {
                Console.WriteLine("※ 숫자를 입력해주세요.");
            }
        }
        // 모든 스탯을 업데이트하는 메서드
        public  void UpdateStats()
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
