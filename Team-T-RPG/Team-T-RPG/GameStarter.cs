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
            Console.WriteLine("________                     ____                          \r\n\\______ \\   __ __   ____    / ___\\   ____   ____    ____   \r\n |    |  \\ |  |  \\ /    \\  / /_/  >_/ __ \\ /  _ \\  /    \\  \r\n |    `   \\|  |  /|   |  \\ \\___  / \\  ___/(  <_> )|   |  \\ \r\n/_______  /|____/ |___|  //_____/   \\___  >\\____/ |___|  / \r\n        \\/             \\/               \\/             \\/  \r\n                                                           \r\n                          ____                             \r\n                         /  _ \\                            \r\n                         >  _ </\\                          \r\n                        /  <_\\ \\/                          \r\n                        \\_____\\ \\                          \r\n                               \\/                          \r\n                                                           \r\n      _________                        __                  \r\n     /   _____/______ _____  _______ _/  |_ _____          \r\n     \\_____  \\ \\____ \\\\__  \\ \\_  __ \\\\   __\\\\__  \\         \r\n     /        \\|  |_> >/ __ \\_|  | \\/ |  |   / __ \\_       \r\n    /_______  /|   __/(____  /|__|    |__|  (____  /       \r\n            \\/ |__|        \\/                    \\/        \r\n                                                           ");

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
                Console.WriteLine("던전 엔 스파르타에 오신 것을 환영합니다.");

                Console.WriteLine("1. 새로 시작");
                Console.WriteLine("2. 이어하기");

                int userinput = MainFrame.UserInputHandler(1, 2);

                if (userinput == 1)
                {
                    Console.Clear();
                    NameCharacter();
                    ChooseJob();
                    break;

                }
                else if (userinput == 2) // 추후 이어하기 구현 시 여기서 다 불러오고 break
                {
                    Console.Clear();
                    Console.WriteLine("저장 기능은 미구현입니다. 죄송합니다.\n잠시 후 시작화면으로 돌아갑니다.");
                    Thread.Sleep(1500); // 1.5초 대기
                    Console.Clear();
                }
            }
        } // 시작 시 새로하기 or 이어하기 선택창 (이어하기는 추후 구현) + 캐릭터 생성까지.

        public void NameCharacter()
        {
            while (true)
            {
                Console.WriteLine("캐릭터를 생성합니다. \n닉네임을 입력하세요. (10자 이하)");
                string userinput = Console.ReadLine();

                if ((userinput?.Length ?? 0) > 10 || (userinput?.Length ?? 0) == 0) // ?. 연산자와 ?? 연산자 활용해서 null에러 방지
                {
                    MainFrame.ClearConsoleLine(4); // 입력한 줄 지워버리고 아래 출력
                    Console.WriteLine("닉네임이 너무 짧거나 깁니다. 다시 입력하세요.");
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
            Console.WriteLine($"당신의 캐릭터 이름 : {Data.Name}");
            Console.WriteLine("당신의 직업을 선택해 주세요.\n 직업에 따라 시작 스텟이 다를 수 있으며,\n확률에 따라 보너스 스텟이 추가로 부여됩니다.\n");

            Console.WriteLine("1. 전사 ( + Str )");
            Console.WriteLine("2. 도적 ( + Dex )");
            Console.WriteLine("3. 법사 ( + Int )");
            Console.WriteLine("4. 야만인 ( + Con )\n");

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
            //(4) 스텟에 따른 최종 스테이터스 갱신 및 Hp,Mp 풀회복
            Stats stats = new Stats();
            stats.UpdateStats();
            Data.Hp += Data.HpMax;
            Data.Mp += Data.MpMax;
        }
    }
}
