using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Team_T_RPG
{
    class MainFrame
    {
        
        static void Main()
        {
            GameStarter gameStarter = new GameStarter(); // 게임스타터 인스턴스 생성 : 게임 시작, 캐릭터 생성 및 저장 데이터 불러오기
            gameStarter.StartScene();

            while (true) // 게임오버 조건 미달성 시 무한 반복
            {
                int daychangecheck = Data.Day;

                if (Data.Hp <= 0) // 우선 체력 체크 : 엔딩 조건 1
                {
                    Console.WriteLine("\n당신의 체력이 끝내 고갈되고 말았습니다!\n");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    for (int i = 0; i < 8; i++)
                    {
                        string death = "사망하셨습니다!";
                        Thread.Sleep(150);
                        Console.Write(death[i]);
                    }
                    Console.ResetColor();
                    break;
                }
                    
                if (!Data.duty) // 세금 납부
                {
                    PayTax();
                }

                if (Data.Money <= 0) // 납부할 골드 부족 시 사망 : 엔딩 조건 2
                {
                    Console.WriteLine("\n납부할 세금이 부족한 당신은, 형장의 이슬이 되고 맙니다!\n");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    for (int i = 0; i < 8; i++)
                    {
                        string death = "사망하셨습니다!";
                        Thread.Sleep(150);
                        Console.Write(death[i]);
                    }
                    Console.ResetColor();
                    break;
                }

                else
                {
                    TownScene(); // 마을 씬으로 진입
                    Data.duty = (Data.Day - daychangecheck == 0)? false : true; // 루프 전후로 일자변화 감지하여 세금납부 여부 확인
                }
            }


            Console.WriteLine($"\n당신의 생존 일수 : {Data.Day} 일\n");
            Console.WriteLine("다시 시작하려면 재실행해주세요.");
            // 사인을 넣고 싶으면 다 하고 추후 기능 구현하기!

        }
        
        public static int UserInputHandler(int min, int max, int failcount = 0)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("원하는 번호를 선택하세요.");
           
            string userInput = Console.ReadLine();
            bool isVaildInput = Int32.TryParse(userInput, out int result);
            
            if (isVaildInput && result >= min && result <= max)
            {
                return result;
            }
            else
            {
                int isFailAgain = (failcount == 0) ? 0 : 1; // 첫 실행 때만 0이고 다음 루프부터는 1
                ClearConsoleLine(3 + isFailAgain);
                failcount++;
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                return UserInputHandler(min, max, failcount);
            }
        }
        // min,max값 사잇값 입력 받아서 int로 리턴해줌. 호출할 때는 최소 최댓값만 넣어주세요! UserInputHandler(1,3) 이런 식으로
        // 줄 수 지워주는 함수 추가해서 이제 매번 새로 프린트할 필요 없이 커서가 아래서 놉니다.

        public static void PayTax() // 세금 계속 루프돌때마다 세금내면 곤란하니 bool값 써서 Day 변동이 있을 시만 걷기
        {
            if (Data.Day <= 7)
            {
                Console.WriteLine("\n7일 간은 세금이 면제됩니다.\n");
                return; // 7일이 지날 때까진 바로 리턴
            }
            
            if (Data.duty)
            {
                Console.WriteLine($"마을에 온 지 {Data.Day}일이 지났습니다.");
                Console.WriteLine($"당신은 세금으로 {Data.Day * 143} G를 납부해야 합니다.");
                Console.WriteLine($"( 현재 소지 골드 : {Data.Money} G )\n");
                Data.Money -= Data.Day * 143;
                Console.Write("세금 정산 중");
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(150);
                    Console.Write(" > ");
                    Thread.Sleep(150);
                }
                Console.WriteLine();
                /*
                세금 계산식에 대해서 논의 요망 : 인던하면 3일씩 보내는 거 같은데, 밀린 세금 한번에 받는 건지
                아니면 그냥 돌아온 날에만 해당하는 일자의 세금을 걷는 건지 알아야 할 듯.
                또한 왜 하필 143을 곱하셨나요? 그냥 적당한 밸런싱을 위한 수치?
                */
            }
        }

        public static void TownScene() // 가장 중심 씬이 될 마을. 각 선택지에 따라 기능 구현 (업무 여기서 나누는 느낌으로)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("마을은 살기엔 좋지만, 매일 세금을 납부하지 못하면 위험합니다."); // 추후 꾸밀 것 (시나리오 및 아트 담당?)
            Console.WriteLine("준비를 단단히 하고, 던전에 들어가야 합니다.");
            Console.WriteLine("1. 스탯 확인\n2. 인벤토리\n3. 상점\n4. 퀘스트\n5. 휴식\n6. 던전 진입\n");
            int userinput = UserInputHandler(1,6);
            switch (userinput)
            {
                case 1: // 스탯창
                    {
                        break;
                    }

                case 2: // 인벤토리
                    {
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
                        Console.Clear();
                        Console.Write("던전 입장 중");
                        for (int i = 0; i < 5; i++)
                        {
                            Thread.Sleep(300);
                            Console.Write(">");
                            Thread.Sleep(300);
                        }
                        Console.Clear();

                        bool dungeonError = false;
                        bool dungeonEnd = false;

                        Data.floorChange = true;

                        while (DungeonSystem.DungeonEntry(ref dungeonError, ref dungeonEnd))
                        {
                            if (dungeonEnd) break;
                        }
                        Console.Clear();
                        Data.Day++;

                        break;
                    }
            }


        }

        
        public static void ClearConsoleLine(int howmanyline)
        {
            // 현재 커서 위치 저장
            int currentLine = Console.CursorTop;

            // 지우려는 줄 수가 현재 줄보다 크지 않게 제한 : 커서가 1줄에 있는데 3줄 지운다고 하면 에러 나니까 1줄로 강제 내림
            howmanyline = Math.Min(howmanyline, currentLine);

            for (int i = 0; i < howmanyline; i++)
            {
                int lineToClear = currentLine - i - 1;
                Console.SetCursorPosition(0, lineToClear);
                Console.Write(new string(' ', Console.WindowWidth));
            }

            Console.SetCursorPosition(0, currentLine - howmanyline);
        }
        // 입력한 줄 수만큼 아래부터 콘솔라인 지워 줍니다. 편하게 호출해서 쓰세요. ClearConsoleLine(5) 이러면 5줄 지워줌.
    }
}
