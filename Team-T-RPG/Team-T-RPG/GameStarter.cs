using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_T_RPG
{
    public class GameStarter // 저장 데이터 불러오기 및 캐릭터 생성하는 파트
    {
        public void StartSceneArt()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("________                     ____                          \r\n\\______ \\   __ __   ____    / ___\\   ____   ____    ____   \r\n |    |  \\ |  |  \\ /    \\  / /_/  >_/ __ \\ /  _ \\  /    \\  \r\n |    `   \\|  |  /|   |  \\ \\___  / \\  ___/(  <_> )|   |  \\ \r\n/_______  /|____/ |___|  //_____/   \\___  >\\____/ |___|  / \r\n        \\/             \\/               \\/             \\/  \r\n                                                           \r\n");
            Console.ForegroundColor= ConsoleColor.Cyan;
            Console.Write("                          ____                             \r\n                         /  _ \\                            \r\n                         >  _ </\\                          \r\n                        /  <_\\ \\/                          \r\n                        \\_____\\ \\                          \r\n                               \\/                          \r\n");
            Console.ForegroundColor= ConsoleColor.DarkRed;
            Console.WriteLine("                                                           \r\n      _________                        __                  \r\n     /   _____/______ _____  _______ _/  |_ _____          \r\n     \\_____  \\ \\____ \\\\__  \\ \\_  __ \\\\   __\\\\__  \\         \r\n     /        \\|  |_> >/ __ \\_|  | \\/ |  |   / __ \\_       \r\n    /_______  /|   __/(____  /|__|    |__|  (____  /       \r\n            \\/ |__|        \\/                    \\/        \r\n                                                           ");
            Console.ResetColor();
        }

        /*
            이어하기는 나중에 만들고 ("추후 구현 예정입니다") 안내 띄우기
            이어하기 선택 시 불러올 데이터를 전부다 Data.cs에 몰아넣고 싶으신 거죠?
            새로 시작 해서 캐릭터 만들기 들어가기
        */

        public void StartScene()
        {
            while (true)
            {
                StartSceneArt();
                MainFrame.SerialTextWrite("던전 엔 스파르타에 오신 것을 환영합니다.\n\n");

                Console.WriteLine("1. 새로 하기");
                Console.WriteLine("2. 이어하기");

                int userinput = MainFrame.UserInputHandler(1, 2);

                if (userinput == 1)
                {
                    Console.Clear();
                    
                    EnteringTown();
                    ChooseJob();
                    NameCharacter();
                    break;

                }
                else if (userinput == 2) // 추후 이어하기 구현 시 여기서 다 불러오고 break
                {
                    Console.Clear();
                    Console.WriteLine("저장 기능은 미구현입니다. 죄송합니다.\n잠시 후 시작화면으로 돌아갑니다.");
                    Thread.Sleep(2000); // 2초 대기
                    Console.Clear();
                }
            }
        } // 시작 시 새로하기 or 이어하기 선택창 (이어하기는 추후 구현) + 캐릭터 생성까지.

        public void NameCharacter()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[모험가 명부]\n");
                Console.ResetColor();
                Console.WriteLine("당신의 이름을 적어주세요. (10자 이하)");
                string userinput = Console.ReadLine();

                if ((userinput?.Length ?? 0) > 10 || (userinput?.Length ?? 0) == 0) // ?. 연산자와 ?? 연산자 활용해서 null에러 방지
                {
                    MainFrame.ClearConsoleLine(4); // 입력한 줄 지워버리고 아래 출력
                    Console.WriteLine("이름이 너무 짧거나 깁니다. 다시 입력하세요.");
                }
                else
                {
                    Data.Name = userinput;
                    break;
                }
            }
        }

        public void ChooseJob()
        {
            Console.Clear();
            
            Console.WriteLine("\n당신의 직업을 선택해 주세요.\n직업에 따라 시작 스텟이 다를 수 있으며,\n확률에 따라 보너스 스텟이 추가로 부여됩니다.\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("1. 전사 ( + Str )");
            Console.WriteLine("2. 도적 ( + Dex )");
            Console.WriteLine("3. 법사 ( + Int )");
            Console.WriteLine("4. 야만인 ( + Con )\n");
            Console.ResetColor();
            int userinput = MainFrame.UserInputHandler(1,4);
            Console.Clear();

            // 유저인풋 받고 직업선택에 따른 스텟 적용 시작 (1) 직업 인덱스 할당
            Data.JobNames = userinput;

            // 스탯 적용 (2) 랜덤 스텟보너스 적용 
            Data.startStr = Data.dice6();
            Data.startDex = Data.dice6();
            Data.startInt = Data.dice6();
            Data.startCon = Data.dice6();
            Data.startWis = Data.dice6();
            Data.startLuk = Data.dice6();

            // 스탯 적용 (3) 직업에 따른 스텟보너스 적용
            switch (userinput)
            {
                case 1: // 전사
                    Data.startStr += 2; // 힘
                    Data.startDex += 2; // 민첩
                    Data.startCon += 3; // 체력
                    break;

                case 2: // 도적
                    Data.startDex += 4; // 민첩
                    Data.startStr += 1; // 힘
                    Data.startCon += 2; // 체력
                    break;

                case 3: // 마법사
                    Data.startInt += 3; // 지능
                    Data.startWis += 3; // 마나
                    Data.startCon += 1; // 체력
                    break;

                case 4: // 야만인
                    Data.startStr += 2; // 힘
                    Data.startCon += 5; // 체력
                    break;
            }
            // (4) 스텟에 따른 최종 스테이터스 갱신 및 Hp,Mp 풀회복
            Stats stats = new Stats();
            stats.UpdateStats();
            Data.Hp += Data.HpMax;
            Data.Mp += Data.MpMax;
        }

        public void EnteringTown()
        {
            Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            MainFrame.SerialTextWrite("정신 차리는 중", 200);
            Console.ResetColor();
            Console.Clear();
            MainFrame.SerialTextWrite("\n뭐지? 기절했던 건가?\n");
            MainFrame.SerialTextWrite("\n어리둥절해하며 몸을 일으켜 보니, " +
                "마치 중세 같아 보이는 마을의 광장 한복판이었다.\n" +
                "어처구니 없는 상황도 상황이지만, 몸 속에서 느껴지는 ", 25);
            Console.ForegroundColor = ConsoleColor.Yellow;
            MainFrame.SerialTextWrite("이 알 수 없는 힘");
            Console.ResetColor();
            MainFrame.SerialTextWrite("은 뭐지?\n\n");
            Console.WriteLine("0. 전직하기");
            int userinput = MainFrame.UserInputHandler(0,0);
        }

        public void BriefExplain()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  [{Data.Job[Data.JobNames]}의 적성이 개화합니다!]\n");
            Console.ResetColor();
            MainFrame.SerialTextWrite("지나가는 사람을 붙잡아 물어보니, 이런 일이 처음이 아니라는 듯\n자연스럽게", 25);
            Console.ForegroundColor = ConsoleColor.Yellow;
            MainFrame.SerialTextWrite("<모험가 연합>",250);
            Console.ResetColor();
            MainFrame.SerialTextWrite("이라 써 있는 간판을 가리켰다.\n",25);
            Console.WriteLine("0. 건물로 들어가기");
            int userinput = MainFrame.UserInputHandler(0, 0);
            Console.Clear();
            MainFrame.SerialTextWrite("건물로 들어가니, 퉁명스러운 표정의 안내원이 귀찮다는 듯 날 맞이했다.\n", 25);
            MainFrame.SerialTextWrite("저희는 마을 밖의 마물들로부터의 보호는 물론, 여러분들의 식량, 치안 관리 까지 생존을 위한 모든 것을 제공하고 있습니다. 물론 무료는 아니고, 매일 세금을 납부하셔야 합니다. 못 내면 추방이에요!");

        }

    }
}
