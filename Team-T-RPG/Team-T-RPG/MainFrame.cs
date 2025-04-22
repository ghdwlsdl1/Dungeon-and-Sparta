using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_T_RPG
{

    class MainFrame
    {

        static void Stats()
        {
            MainFrame.UpdateStats();
            Console.WriteLine("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine("");
            Console.WriteLine($"힘    : {Data.Str}");
            Console.WriteLine($"민첩  : {Data.Dex}");
            Console.WriteLine($"지능  : {Data.Int}");
            Console.WriteLine($"체력  : {Data.Con}");
            Console.WriteLine($"지혜  : {Data.Wis}");
            Console.WriteLine($"행운  : {Data.Luk}");
            Console.WriteLine($"공격력: {Data.Atk}");
            Console.WriteLine($"방어력: {Data.Def}");
            Console.WriteLine("");
            Console.WriteLine($"레벨 : {Data.Level}");
            Console.WriteLine($"chad ( {Data.Job[0]} )");
            Console.WriteLine($"공격력 : {Data.Atk}");
            Console.WriteLine($"방어력 : {Data.Def}");
            Console.WriteLine($"HP : {Data.Hp}/{Data.HpMax}");
            Console.WriteLine($"Gold   : {Data.Money}");
            Console.WriteLine("");
            Console.WriteLine("1.장비교체");
            Console.WriteLine("0.나가기");
            string input = Console.ReadLine();
            int outInput;
            bool intInput = int.TryParse(input, out outInput);
            if (intInput)
            {
                if (outInput == 1)
                {
                    Console.WriteLine("장비교체");
                }
            }
            else
            {
                Console.WriteLine("다시입력");
            }
        }

        static void Main()
        {

            StartScene(); // 게임 시작, 이어하기 및 캐릭터 생성 (이어하기는 추후 구현)

            while (true) // 아래 조건 미충족 시 break

            {
                PayTax(); // 세금 납부

                if (Data.Level == 0) // 체력 == 0 || 세금을 내고 골드가 음수값이 된 경우 게임오버 처리
                    break;
                else
                {
                    TownScene(); // 마을 씬으로 진입
                }
            }

            Console.WriteLine("게임 오버!\n 다시 시작하려면 재실행해주세요.");
            // 사인을 넣고 싶으면 다 하고 추후 기능 구현하기!

        }


        public static void StartScene()
        {
            GameStarter gameStarter = new GameStarter();

            gameStarter.StartSceneArt();
            Console.WriteLine("던전 엔 스파르타에 오신 것을 환영합니다.");

            Console.WriteLine("1. 새로 시작");
            Console.WriteLine("2. 이어하기");

            int userinput = UserInputHandler(1, 2);


            /*
             이어하기는 나중에 만들고 ("추후 구현 예정입니다") 안내 띄우기
             이어하기 선택 시 불러올 데이터를 전부다 Data.cs에 몰아넣고 싶으신 거죠?
             새로 시작 해서 캐릭터 만들기 들어가기
            */
        } // 시작 시 새로하기 or 이어하기 선택창 (이어하기는 추후 구현) + 캐릭터 생성까지.
        public static int UserInputHandler(int min, int max) // min,max값 사잇값 받아주는 메서드
        {
            Console.WriteLine("원하는 행동을 선택하세요.");

            string userInput = Console.ReadLine();
            bool isVaildInput = Int32.TryParse(userInput, out int result);

            if (isVaildInput && result >= min && result <= max)
            {
                return result;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                return UserInputHandler(min, max);
            }

        }

        public static void PayTax() // 세금 계속 루프돌때마다 세금내면 곤란하니 bool값 써서 Day 변동이 있을 시만 걷기
        {
            Console.WriteLine("세금내라우 동무");
        }

        public static void TownScene() // 가장 중심 씬이 될 마을. 각 선택지에 따라 기능 구현 (업무 여기서 나누는 느낌으로)
        {
            Console.WriteLine("1.스탯창");
            Console.WriteLine("2.인벤토리");
            Console.WriteLine("3");
            Console.WriteLine("4");
            Console.WriteLine("5");
            Console.WriteLine("6");
            int userinput = UserInputHandler(1, 6);
            switch (userinput)
            {
                case 1: // 스탯창
                    {
                        Stats();
                        break;
                    }

                case 2: // 인벤토리
                    {
                        testInven();
                        break;
                    }

                case 3: // 상점
                    {
                        break;
                    }

                case 4: // 퀘스트 <<
                    {
                        break;
                    }

                case 5: // 휴식 <<
                    {
                        break;
                    }

                case 6: // 던전 입장
                    {
                        break;
                    }
            }


        }
        public static void UpdateLevel()
        {
            int experienceMax = 10 * Data.Level * Data.Level;
            if (Data.experience >= experienceMax)
            {
                Data.Level++;
                Data.experience -= experienceMax;
            }
        }
        public static void UpdateHpMax()
        {
            Data.HpMax = Data.Con * 2;  // 최대 HP는 체력(Con) * 2
            if (Data.Hp < Data.HpMax)  // 최대 HP가 현재 HP를 초과하면
            {
                Data.Hp = Data.HpMax;  // HP를 최대 HP로 설정
            }
        }
        // 최대 MP를 업데이트하는 메서드
        public static void UpdateMpMax()
        {
            Data.MpMax = Data.Wis * 2;  // 최대 MP는 지능(Wis) * 2
            if (Data.Mp < Data.MpMax)  // 최대 MP가 현재 MP를 초과하면
            {
                Data.Mp = Data.MpMax;  // MP를 최대 MP로 설정
            }
        }
        // 힘(Strength)을 업데이트하는 메서드
        public static void UpdateStr()
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
        public static void UpdateDex()
        {
            int weaponBonus = GetWeaponBonus();
            Data.Dex = Data.startDex + weaponBonus + Data.Level;  // 기본 민첩 + 무기 보너스 + 레벨  int weaponEquipNum = 0;
        }

        // 지능(Intelligence)을 업데이트하는 메서드
        public static void UpdateInt()
        {
            int weaponBonus = GetWeaponBonus();
            Data.Int = Data.startInt + weaponBonus + Data.Level;  // 기본 지능 + 무기 보너스 + 레벨
        }

        // 체력(Constitution)을 업데이트하는 메서드
        public static void UpdateCon()
        {
            int assistBonus = 0;
            if (Data.assistEquip[5])  // 보조 장비가 장착되어 있으면
            {
                assistBonus += Data.assistStats[5];  // 보조 장비에서 제공하는 보너스 추가
            }

            Data.Con = Data.startCon + assistBonus + Data.Level;  // 기본 체력 + 보조 장비 보너스 + 레벨
        }

        // 지혜(Wisdom)을 업데이트하는 메서드
        public static void UpdateWis()
        {
            int assistBonus = 0;
            if (Data.assistEquip[4])  // 보조 장비가 장착되어 있으면
            {
                assistBonus += Data.assistStats[4];  // 보조 장비에서 제공하는 보너스 추가
            }
            Data.Wis = Data.startWis + assistBonus + Data.Level;  // 기본 지혜 + 보조 장비 보너스 + 레벨
        }

        // 행운(Luck)을 업데이트하는 메서드
        public static void UpdateLuk()
        {
            int assistBonus = 0;
            if (Data.assistEquip[3])  // 보조 장비가 장착되어 있으면
            {
                assistBonus += Data.assistStats[3];  // 보조 장비에서 제공하는 보너스 추가
            }
            Data.Luk = Data.startLuk + assistBonus;  // 기본 행운 + 보조 장비 보너스
        }

        // 공격력(Attack)을 업데이트하는 메서드
        public static void UpdateAtk()
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
        public static void UpdateDef()
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
        public static int GetEquippedWeaponIndex()
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
        public static int GetWeaponBonus()
        {
            int weaponEquipNum = GetEquippedWeaponIndex();

            if (weaponEquipNum >= 0)
            {
                return Data.weaponStats[weaponEquipNum];
            }

            return 0; // 장착된 무기가 없으면 보너스는 0
        }
        public static void testInven()
        {
            for (int i = 0; i < Data.weapon.Length; i++)
            {
                if (Data.weaponTf[i] >= 0)
                {
                    string equippedMark = Data.weaponEquip[i] ? " (장착중)" : "";
                    Console.WriteLine($"{i}. {Data.weapon[i]} 소지 갯수: {Data.weaponTf[i]}  공격력: {Data.weaponAtk[i]}  스탯+: {Data.weaponStats[i]}{equippedMark}");
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
                    Console.WriteLine($"\n▶ [{Data.weapon[selectedIndex]}]를 장착했습니다!");

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
        public static void UpdateStats()
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
