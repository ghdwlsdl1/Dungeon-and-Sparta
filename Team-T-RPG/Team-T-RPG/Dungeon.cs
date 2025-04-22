using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Team_T_RPG;

public static class DungeonSystem
{
    public static void dungeonTime()
    {
        if (Data.dungeonHour >= 24)
        {
            Data.dungeonDay--;
            Data.dungeonHour -= 24;
        }
    }

    public static bool DungeonEntry(ref bool DungeonEntryError, ref bool DungeonEntryEnd)
    {
        dungeonTime();

        if (Data.dungeonDay == 0)
        {
            Data.dungeonDay = 3;
            Data.dungeonHour = 0;
            Data.floor = 1;
            Data.playerX = -1; Data.playerY = -1;
            Data.portalX = -2; Data.portalY = -2;
            Data.floorChange = true;
            dungeonEnd = true;
            return false;
        }

        if (Data.Hp <= 0)
        {
            dungeonEnd = true;
            return false;
        }

        if (Data.portalY == Data.playerY && Data.portalX == Data.playerX)
        {
            Data.playerX = -1; Data.playerY = -1;
            Data.portalX = -2; Data.portalY = -2;
            Data.floorChange = true;
            Data.floor++;
            Data.dungeonDay *= 2;

            Data.Money += 200 * Data.floor * Data.floor;
        }

        Dungeon.MiniMap();

        Dungeon.PrintMap();

        Console.WriteLine($"\n\n{Data.floor}층 던전 안 입니다.");
        Console.WriteLine($"던전 남은 일수   {Data.dungeonDay}");
        Console.WriteLine($"던전 지난 시간   {Data.dungeonHour}");
        Console.WriteLine("\n행동을 정해 주세요.");
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 이동하기");
        Console.WriteLine("4. 휴식하기");
        Console.WriteLine("5. 조사하기");

        if (DungeonEntryError)
        {
            Console.WriteLine("잘못된 입력입니다.\n");
            DungeonEntryError = false;
        }

        string text = Console.ReadLine();

        switch (text)
        {
            case "1":
                bool windowsError = false;
                bool windowsRepeat = SystemWindow.systemWindows(windowsError);
                while (!windowsRepeat)
                {
                    if (!windowsError) windowsError = true;
                    windowsRepeat = SystemWindow.systemWindows(windowsError);
                }
                break;

            case "2":
                bool invenError = false;
                bool invenEquipError = false;
                bool invenNext = false;

                bool inventoryRepeat = Inventory.inventory(invenError, out invenNext);
                while (!inventoryRepeat)
                {
                    if (!invenError) invenError = true;
                    inventoryRepeat = Inventory.inventory(invenError, out invenNext);
                }

                if (invenNext)
                {
                    bool inventoryEquipRepeat = Inventory.inventoryEquip(invenEquipError);
                    while (!inventoryEquipRepeat)
                    {
                        if (!invenEquipError) invenEquipError = true;
                        inventoryEquipRepeat = Inventory.inventoryEquip(invenEquipError);
                    }
                }
                Console.Clear();
                break;

            case "3":
                if (Data.dice6() >= 4)
                {
                    Data.dungeonHour += 2;
                    battle();
                }
                else
                {
                    Data.dungeonHour += 1;
                    bool moveError = false;
                    bool moveError2 = false;
                    bool moveRepeat = Dungeon.Move(ref moveError, ref moveError2);
                    while (!moveRepeat)
                    {
                        moveRepeat = Dungeon.Move(ref moveError, ref moveError2);
                    }
                    Console.Clear();
                }
                break;

            case "4":
                if (Data.dice20() >= 20)
                {
                    Console.Clear();
                    Console.WriteLine("편하게 쉬었습니다.");
                    Data.dungeonHour += 12;
                    Data.Hp += Data.HpMax;
                    Data.Mp += Data.MpMax;
                    //Stats.UpdateStats();                                                                                                        스텟업데이트필요
                }
                else if (Data.dice20() >= 10)
                {
                    Console.Clear();
                    Console.WriteLine("잠들었습니다.");
                    Data.dungeonDay -= 1;
                    Data.Hp += Data.HpMax;
                    Data.Mp += Data.MpMax;
                    //Stats.UpdateStats();                                                                                                        스텟업데이트필요
                }
                else
                {
                    Data.dungeonHour += 2;
                    battle();
                }
                break;

            case "5":
                Search();
                break;

            default:
                Console.Clear();
                DungeonEntryError = true;
                break;
        }
        return true;
    }

    public static class Dungeon
    {
        public static int GetMapSize(int floor)
        {
            return 5 + (floor - 1) * 5;
        }

        public static void MiniMap()
        {
            if (Data.floor == 1) //첫층 강제실행
            {
                Data.floorChange = true;
            }

            if (!Data.floorChange) // 이미 맵이 있는 상태면 탈출
            {
                return;
            }

            int size = GetMapSize(Data.floor); // 현재 층의 맵 크기
            Data.map = new char[size, size];

            // 지도 초기화: 빈 공간(' ')으로
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Data.map[y, x] = ' ';
                }
            }

            // 플레이어 위치 초기화
            if (Data.floorChange)
            {
                Data.playerX = Data.random.Next(0, size);
                Data.playerY = Data.random.Next(0, size);
            }

            // 포탈 위치 초기화
            if (Data.floorChange)
            {
                Data.portalX = Data.random.Next(0, size);
                Data.portalY = Data.random.Next(0, size);
            }

            // ===== 장애물 랜덤 배치 =====
            int wallCount = size * size / 10; // 맵 크기의 10%를 장애물로
            int placed = 0;

            while (placed < wallCount && Data.floorChange)
            {
                int wallX = Data.random.Next(0, size);
                int wallY = Data.random.Next(0, size);

                // 포탈과 플레이어 위치, 이미 배치된 칸은 제외
                if ((wallX == Data.playerX && wallY == Data.playerY) ||
                    (wallX == Data.portalX && wallY == Data.portalY) ||
                    Data.map[wallY, wallX] != ' ')
                    continue;

                Data.map[wallY, wallX] = '■'; // 장애물 기호
                placed++;
            }

            while (Data.floorChange)
            {
                int treasureX = Data.random.Next(0, size);
                int treasureY = Data.random.Next(0, size);
                if ((treasureX == Data.playerX && treasureY == Data.playerY) ||
                    (treasureX == Data.portalX && treasureY == Data.portalY) ||
                    Data.map[treasureY, treasureX] != ' ')
                    continue;

                Data.map[Data.treasureY, Data.treasureX] = ' ';
                break;
            }

            Data.floorChange = false;

            // 지도에 포탈, 플레이어 표시
            Data.map[Data.portalY, Data.portalX] = '◇';
            Data.map[Data.playerY, Data.playerX] = 'P';
        }

        public static void PrintMap()
        {
            int size = Data.map.GetLength(0);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Console.Write(Data.map[y, x] + " ");
                }
                Console.WriteLine();
            }
        }

        public static bool Move(ref bool moveError, ref bool moveError2)
        {
            Console.Clear();
            MiniMap();
            PrintMap();

            Console.WriteLine("이동 방향을 정해주세요.");
            Console.WriteLine("1. 위");
            Console.WriteLine("2. 아래");
            Console.WriteLine("3. 오른쪽");
            Console.WriteLine("4. 왼쪽");

            if (moveError)
            {
                Console.WriteLine("잘못된 입력입니다.\n");
                moveError = false;
            }
            else if (moveError2)
            {
                Console.WriteLine("이동할 수 없습니다.\n");
                moveError2 = false;
            }

            string text = Console.ReadLine();

            int newX = Data.playerX;
            int newY = Data.playerY;

            switch (text)
            {
                case "1": newY--; break;
                case "2": newY++; break;
                case "3": newX++; break;
                case "4": newX--; break;
                default:
                    Console.Clear();
                    moveError = true;
                    return false;
            }

            int size = GetMapSize(Data.floor);

            if (newX < 0 || newX >= size || newY < 0 || newY >= size || Data.map[newY, newX] == '■')
            {
                moveError2 = true;
                return false;
            }

            Data.map[Data.playerY, Data.playerX] = ' ';
            Data.playerX = newX;
            Data.playerY = newY;
            Data.map[Data.playerY, Data.playerX] = 'P';

            return true;
        }
    }

    public static void Search() // 탐색
    {
        int px = Data.playerX;
        int py = Data.playerY;

        // 보물과 같은 위치면 습득
        if (px == Data.treasureX && py == Data.treasureY)
        {
            Console.WriteLine("[보물을 획득했다]");
            Data.map[py, px] = ' '; // 보물 제거
            Data.treasureX = -1;
            Data.treasureY = -1;
            return;
        }

        int range = 1 + (Data.Wis / 10); // 지혜 수치로 탐색 범위 결정

        for (int y = py - range; y <= py + range; y++)
        {
            for (int x = px - range; x <= px + range; x++)
            {
                if ((x == px && y == py) || x < 0 || y < 0 || x >= Data.map.GetLength(0) || y >= Data.map.GetLength(1))
                    continue;

                if (x == Data.treasureX && y == Data.treasureY)
                {
                    if (Data.dice20() + (Data.Wis / 2) >= 10)
                    {
                        Console.WriteLine("[보물의 기운이 느껴진다…]");
                    }
                    else
                    {
                        Console.WriteLine("[뭔가 있을 것 같지만 확신이 없다.]");
                    }
                    return;
                }
            }
        }
    }


    public static void battle()
    {
        Console.Clear();
        Console.WriteLine("적을 만났습니다! 전투를 시작합니다.\n\n\n");

        int enemyHP = Data.random.Next(1 + (Data.floor * Data.floor), (Data.floor * 5) + (Data.floor * Data.floor));
        int enemyAttack = Data.random.Next(1 + Data.floor, 3 + Data.floor * 2);
        bool defense = false;
        bool battleError = false;

        while (Data.Hp > 0 && enemyHP > 0)
        {
            Console.WriteLine($"\n\n적의 체력:   {enemyHP}");
            Console.WriteLine($"기본 공격력: {enemyAttack}");
            Console.WriteLine($"\n\n현재 체력:   {Data.Hp} / {Data.HpMax}");
            Console.WriteLine($"현재 마나:   {Data.Mp} / {Data.MpMax}");
            Console.WriteLine("\n당신의 차례입니다. 행동을 선택하세요:");
            Console.WriteLine("1. 공격하기");
            Console.WriteLine("2. 방어하기");
            Console.WriteLine("3. 마법 사용하기 (MP 2 소모)");

            if (battleError) Console.WriteLine("잘못된 입력. 아무 행동도 하지 못했습니다.");
            string action = Console.ReadLine();
            defense = false;

            switch (action)
            {
                case "1":
                    Console.Clear();
                    int damage = 0;
                    int roll = Data.dice20();
                    damage = roll >= 20 ? Data.Atk * 3 :
                             roll >= 10 ? Data.Atk * 2 : Data.Atk;
                    Console.WriteLine($"공격! {damage}의 피해");
                    enemyHP -= damage;
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("방어 태세! 다음 턴 적의 피해가 체력 비례로 감소합니다.");
                    defense = true;
                    break;

                case "3":
                    Console.Clear();
                    int magicDamage = 0;
                    int mRoll = Data.dice20();
                    magicDamage = mRoll >= 20 ? Data.Int * 4 :
                                  mRoll >= 10 ? Data.Int * 3 : Data.Int * 2;

                    if (Data.Mp >= 2)
                    {
                        Console.WriteLine($"마법 공격! {magicDamage}의 피해");
                        enemyHP -= magicDamage;
                        Data.Mp -= 2;
                    }
                    else
                    {
                        Console.WriteLine("마나가 부족합니다.");
                    }
                    break;

                default:
                    Console.Clear();
                    battleError = true;
                    continue;
            }

            if (enemyHP <= 0)
            {
                Console.WriteLine("적을 쓰러뜨렸습니다!");
                Data.experience += Data.dice20() * Data.floor;
                Data.Money += Data.dice20() * (Data.floor * Data.floor);
                //Stats.UpdateStats();                                                                                                        스텟업데이트필요
                break;
            }

            Console.WriteLine("\n적의 차례입니다.");
            int enemyDice = Data.random.Next(Data.floor, Data.floor * 2);
            int enemyDamage = enemyAttack + enemyDice;

            int evasion = Math.Min(Data.Dex * 2, 101);
            int evasionRoll = Data.random.Next(1, 101);

            if (evasionRoll <= evasion)
            {
                Console.WriteLine($"회피 성공! (회피 확률: {evasion}%)");
            }
            else
            {
                if (defense)
                {
                    enemyDamage = (int)(enemyDamage * 0.5 * (1.0 - Data.Def * 0.05));
                    if (enemyDamage < 0) enemyDamage = 0;
                    Console.WriteLine($"방어 중! 피해 감소 → 실제 피해: {enemyDamage}");
                }
                else
                {
                    Console.WriteLine($"적의 공격! {enemyDamage}의 피해를 입었습니다.");
                }
                Data.Hp -= enemyDamage;
            }

            if (Data.Hp <= 0)
            {
                Console.WriteLine("당신은 쓰러졌습니다...");
                break;
            }
        }
    }


}
