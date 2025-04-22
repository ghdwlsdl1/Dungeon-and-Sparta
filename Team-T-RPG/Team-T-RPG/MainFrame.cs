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
            Stats ss = new Stats();
            ss.UpdateStats();

            //MainFrame.UpdateStats();
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
                        Stats ss = new Stats();
                        ss.testInven();
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
       
    }
}
