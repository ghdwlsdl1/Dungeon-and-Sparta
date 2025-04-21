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
            
            GameStarter gameStarter = new GameStarter(); // 게임스타터 불러오기 : 게임 시작, 캐릭터 생성 및 저장 데이터 불러오기
            gameStarter.StartScene();

            while (true) // 게임오버 조건 미달성 시 무한 반복

            {
                PayTax(); // 세금 납부

                if (Data.Hp == 0 || Data.Money <= 0) // 체력 == 0 || 세금을 내고 골드가 음수값이 된 경우 게임오버
                    break;
                else
                {
                    TownScene(); // 마을 씬으로 진입
                }
            }

            Console.WriteLine("게임 오버!\n 다시 시작하려면 재실행해주세요.");
            // 사인을 넣고 싶으면 다 하고 추후 기능 구현하기!

        }



        
        public static int UserInputHandler(int min, int max, int failcount = 0)
        {
            Console.WriteLine("--------------------------------");
            Console.WriteLine("원하는 행동을 선택하세요.");
           
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
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                return UserInputHandler(min, max, failcount);
            }
        }
        // min,max값 사잇값 입력 받아서 int로 리턴해줌. 호출할 때는 최소 최댓값만 넣어주세요! UserInputHandler(1,3) 이런 식으로
        // 줄 수 지워주는 함수 추가해서 이제 매번 새로 프린트할 필요 없이 커서가 아래서 놉니다.

        public static void PayTax() // 세금 계속 루프돌때마다 세금내면 곤란하니 bool값 써서 Day 변동이 있을 시만 걷기
        {
            Console.WriteLine("세금내라우 동무");
        }

        public static void TownScene() // 가장 중심 씬이 될 마을. 각 선택지에 따라 기능 구현 (업무 여기서 나누는 느낌으로)
        {
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
