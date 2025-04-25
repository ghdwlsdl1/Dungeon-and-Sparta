using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Team_T_RPG
{
    class MainFrame
    {
        // 필드 선언 : 지금 스테틱이 좀 많아서, 여기서 인스턴스 만들고 가려고 합니다.
        static Stats stats = new Stats();
        static Inventory inventory = new Inventory();
        static Item item = new Item();
        static GameStarter gameStarter = new GameStarter();
        static void Main()
        {
            
            QuestManager.Initialize();
            
            gameStarter.StartScene();
            
            while (true) // 게임오버 조건 미달성 시 무한 반복
            {
                int daychangecheck = Data.Day;

                if (Data.Hp <= 0) // 우선 체력 체크 : 엔딩 조건 1
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save.json");
                    File.Delete(path);
                    Art.MakeImage("Image/death.png", width: 60);
                    Console.WriteLine("\n당신의 체력이 끝내 고갈되고 말았습니다!\n");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    SerialTextWrite("사망하셨습니다!", 150);
                    Console.ResetColor();
                    break;
                }
                    
                if (Data.duty) // 세금 납부
                {
                    PayTax();
                }

                if (Data.Money < 0) // 납부할 골드 부족 시 사망 : 엔딩 조건 2
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "save.json");
                    File.Delete(path);
                    Art.MakeImage("Image/death.png", width: 60);
                    Console.WriteLine("\n납부할 세금이 부족한 당신은, 마을 밖으로 쫒겨났습니다.\n 마을 밖의 마물들은 너무 강했습니다.");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    SerialTextWrite("사망하셨습니다!", 150);
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
            Environment.Exit(0);
            // 사인을 넣고 싶으면 다 하고 추후 기능 구현하기!

        }
        
        public static void TownScene() // 가장 중심 씬이 될 마을. 각 선택지에 따라 기능 구현 (업무 여기서 나누는 느낌으로)
        {
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.Title = $"Dungeon & Sparta : {Data.Name} / {Data.Day}일차";
            Art.MakeImage("Image/village.png", width: 60);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"[스파르탄 마을] : Day {Data.Day}");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"♥ {Data.Hp} / {Data.HpMax}  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"♠ {Data.Mp} / {Data.MpMax}  ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" $ {Data.Money} G  \n");
            Console.ResetColor();
            Console.WriteLine("--------------------------------------------------------------------------------------\n");
            Console.WriteLine("1. 스탯 확인\n2. 인벤토리\n3. 상점\n4. 퀘스트\n5. 휴식\n6. 던전 진입\n\n0. 저장하기");
            
            int userinput = UserInputHandler(0,6);
            switch (userinput)
            {
                case 1: // 스탯창
                    {
                        Console.Clear();
                        stats.ShowStatTable();
                        break;
                    }

                case 2: // 인벤토리
                    {
                        Console.Clear();
                        inventory.showInventory();
                        Console.Clear();
                        break;
                    }

                case 3: // 상점
                    {
                        Console.Clear();
                        item.Store();
                        Console.Clear();
                        break;
                    }

                case 4: // 퀘스트 <<
                    {
                        QuestManager.ShowQuestList();
                        break;
                    }

                case 5: // 휴식 <<
                    {
                        Console.Clear();
                        Rest();
                        Console.Clear();
                        break;
                    }

                case 6: // 던전 입장
                    {
                        Console.Clear();
                        Sound.PlaySound("dungeonBgm");
                        Art.MakeImage("Image/dungeonentry.png", width: 60);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        SerialTextWrite("던전으로 들어가는 중 ▶▷▶▷▶",150);
                        Console.ResetColor();
                        Console.Clear();

                        bool dungeonError = false;
                        bool dungeonEnd = false;

                        Data.floorChange = true;

                        while (DungeonSystem.DungeonEntry(ref dungeonError, ref dungeonEnd))
                        {
                            if (dungeonEnd) break;
                        }
                        Console.Clear();
                        Datareset();
                        Data.Day++;
                        break;
                    }
                    case 0:
                    {
                        Console.Clear();
                        SaveSystem.Save();
                        Console.Write("저장이 완료되었습니다.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        break;
                    }
            }


        }

        public static void PayTax() // 세금 계속 루프돌때마다 세금내면 곤란하니 bool값 써서 Day 변동이 있을 시만 걷기
        {
            if (Data.Day <= 1)
            {
                Console.WriteLine("\n ※첫 날은 세금이 면제됩니다. \n");
                return; // 첫날은 바로 리턴
            }

            if (Data.duty)
            {
                Art.MakeImage("Image/receptionist.png", width: 60);
                Console.WriteLine($"마을에 온 지 {Data.Day}일이 지났습니다.");
                Console.WriteLine($"당신은 세금으로 {Data.Day * 143} G를 납부해야 합니다.");
                Console.WriteLine($"( 현재 소지 골드 : {Data.Money} G )\n");
                Data.Money -= Data.Day * 143;
                Console.Write("세금 정산 중");
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(150);
                    if (i%2 == 0) Console.Write(" ▶ ");
                    else Console.Write(" ▷ ");
                    Thread.Sleep(150);
                }
                Console.Clear();
                /*
                세금 계산식에 대해서 논의 요망 : 인던하면 3일씩 보내는 거 같은데, 밀린 세금 한번에 받는 건지
                아니면 그냥 돌아온 날에만 해당하는 일자의 세금을 걷는 건지 알아야 할 듯.
                또한 왜 하필 143을 곱하셨나요? 그냥 적당한 밸런싱을 위한 수치?
                */
            }
        }

        public static int UserInputHandler(int min, int max, int failcount = 0)
        {
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.Write("▶ ");

            string userInput = Console.ReadLine();
            
            bool isVaildInput = Int32.TryParse(userInput, out int result);

            if (isVaildInput && result >= min && result <= max)
            {
                return result;
            }
            else
            {
                int isFailAgain = (failcount == 0) ? 0 : 1; // 첫 실행 때만 0이고 다음 루프부터는 1
                ClearConsoleLine(2 + isFailAgain);
                failcount++;
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                return UserInputHandler(min, max, failcount);
            }
        }
        // min,max값 사잇값 입력 받아서 int로 리턴해줌. 호출할 때는 최소 최댓값만 넣어주세요! UserInputHandler(1,3) 이런 식으로
        // 줄 수 지워주는 함수 추가해서 이제 매번 새로 프린트할 필요 없이 커서가 아래서 놉니다.
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

        public static void SerialTextWrite(string text, int textspeed = 50)
        {
            string serialtext = text;
            int length = serialtext.Length;

            for (int i = 0; i < length; i++)
            {
                Console.Write(serialtext[i]);
                Thread.Sleep(textspeed);
            }
        }
        // 텍스트 하나씩 출력하는 메서드. Console.Write이라고 생각하면 됨. 엔터 직접 \n 넣어서 치셔야 함!
        // 속도 조절을 원하면 뒤에 숫자 적어넣어 주세요(밀리초)
        public static void Rest()
        {

            Art.MakeImage("Image/hotel.png", width: 60);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("[휴식]\n");
            Console.ResetColor();
            Console.WriteLine("당신은 지친 몸을 눕히기 위해 여관으로 향했습니다.");

            Console.WriteLine("여관주인 : ");
            SerialTextWrite("『 마침 좋은 방이 남았다네.\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            SerialTextWrite("  500 G");
            Console.ResetColor();
            SerialTextWrite("만 내고 잠시 쉬고 가는 건 어떤가?』\n\n");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"♥ {Data.Hp} / {Data.HpMax}  ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"♠ {Data.Mp} / {Data.MpMax}  ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($" $ {Data.Money} G  \n\n");
            Console.ResetColor();

            Console.WriteLine("1. 휴식하기 (+ Hp,Mp 100 % / - 500 G )");
            Console.WriteLine("0. 돌아가기\n");

            int userinput = UserInputHandler(0,1);
            switch (userinput)
            {
                case 0:
                    break;
                case 1:
                    {
                        if (Data.Money >= 500)
                        {
                            Data.Money -= 500;
                            Data.Hp += Data.HpMax;
                            Data.Mp += Data.MpMax;
                            stats.UpdateMpMax();
                            stats.UpdateHpMax();

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Blue;
                            SerialTextWrite("휴식 취하는 중 ▶▷▶▷▶");
                            Console.ResetColor();
                            Console.Clear();
                            break;
                        }
                        else
                        {
                            Console.WriteLine(" 골드가 부족합니다!\n 쫒겨났습니다.");
                            Thread.Sleep(1500);
                            break;
                        }
                    }
            }
        }

        public static void Datareset()
        {
            Data.dungeonDay = 3;   //날짜
            Data.dungeonHour = 0;   //시간
            Data.playerX = -1; //플레이어좌표x
            Data.playerY = -1;
            Data.portalX = -2; //포탈좌표
            Data.portalY = -2;
            Data.treasureX = -1; //보물좌표
            Data.treasureY = -1;
            Data.floor = 1; //층수
            Data.floorChange = false; //층변경감지
            Data.monsterPositions = new List<(int x, int y)>();
            Data.map = null; //맵
            Data.monsterTurn = 0; //몬스터 행동
            Data.tired = 0;
            Data.ultimate = 0;
        }


    }
}
