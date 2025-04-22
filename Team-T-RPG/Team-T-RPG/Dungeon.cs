using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Team_T_RPG;
using static DungeonSystem.Dungeon;

public static class DungeonSystem
{
    //던전타임
    public static void dungeonTime()
    {
        if (Data.dungeonHour >= 24)
        {
            Data.dungeonDay--;
            Data.dungeonHour -= 24;
        }
    }
    //던전입장
    public static bool DungeonEntry(ref bool DungeonEntryError, ref bool DungeonEntryEnd)
    {
        // === 턴 시작 시 몬스터 이동 ===
        Dungeon.MoveMonsters();

        // === 충돌 체크 ===
        for (int i = 0; i < Data.monsterPositions.Count; i++)
        {
            if (Data.playerX == Data.monsterPositions[i].x &&
                Data.playerY == Data.monsterPositions[i].y)
            {
                Battle();

                Data.monsterPositions.RemoveAt(i);
                Data.map[Data.playerY, Data.playerX] = ' ';
                Dungeon.PlaceMonsters(1); // 리젠
                Data.dungeonHour += 2;

                return true;
            }
        }
        dungeonTime();

        if (Data.dungeonDay == 0)
        {
            Data.dungeonDay = 3;
            Data.dungeonHour = 0;
            Data.floor = 1;
            Data.playerX = -1; Data.playerY = -1;
            Data.portalX = -2; Data.portalY = -2;
            Data.floorChange = true;
            DungeonEntryEnd = true;
            return false;
        }

        if (Data.Hp <= 0)
        {
            DungeonEntryEnd = true;
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
        
                break;

            case "2":
                
                break;

            case "3":
                    Data.dungeonHour += 1;
                    bool moveError = false;
                    bool moveError2 = false;
                    bool moveRepeat = Dungeon.Move(ref moveError, ref moveError2);
                    while (!moveRepeat)
                    {
                        moveRepeat = Dungeon.Move(ref moveError, ref moveError2);
                    }
                    Console.Clear();
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
                    Battle();
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
    //던전
    public static class Dungeon
    {
        public static int GetMapSize(int floor)
        {
            return 5 + (floor - 1) * 5;
        }

        public static void MiniMap()
        {
            if (Data.floor == 1) Data.floorChange = true; // 첫 층 강제 실행
            if (!Data.floorChange) return;

            int size = GetMapSize(Data.floor);
            Data.map = new char[size, size];

            // 지도 초기화
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Data.map[y, x] = ' ';
                }
            }

            // 1. 벽 랜덤 배치
            int wallCount = size * size / 10;
            int placed = 0;
            while (placed < wallCount)
            {
                int wallX = Data.random.Next(0, size);
                int wallY = Data.random.Next(0, size);

                if (Data.map[wallY, wallX] != ' ')
                    continue;

                Data.map[wallY, wallX] = '■';
                placed++;
            }

            // 2. 플레이어 위치 배치
            while (true)
            {
                int px = Data.random.Next(0, size);
                int py = Data.random.Next(0, size);

                if (Data.map[py, px] == ' ')
                {
                    Data.playerX = px;
                    Data.playerY = py;
                    break;
                }
            }

            // 3. 포탈 위치 배치
            while (true)
            {
                int gx = Data.random.Next(0, size);
                int gy = Data.random.Next(0, size);

                if (Data.map[gy, gx] == ' ' && (gx != Data.playerX || gy != Data.playerY))
                {
                    Data.portalX = gx;
                    Data.portalY = gy;
                    break;
                }
            }

            // 4. 보물 위치 배치
            while (true)
            {
                int tx = Data.random.Next(0, size);
                int ty = Data.random.Next(0, size);

                if (Data.map[ty, tx] == ' ' &&
                    (tx != Data.playerX || ty != Data.playerY) &&
                    (tx != Data.portalX || ty != Data.portalY))
                {
                    Data.treasureX = tx;
                    Data.treasureY = ty;
                    break;
                }
            }

            Data.floorChange = false;

            int monsterCount = Data.floor; // 층 수에 비례한 몬스터 수
            PlaceMonsters(monsterCount);

            // 5. 지도에 포탈, 플레이어 표시
            Data.map[Data.portalY, Data.portalX] = '◇';
            Data.map[Data.playerY, Data.playerX] = 'P';
        }

        public static void PlaceMonsters(int count)
        {
            Data.monsterPositions.Clear();

            int size = Data.map.GetLength(0);
            int placed = 0;

            while (placed < count)
            {
                int x = Data.random.Next(0, size);
                int y = Data.random.Next(0, size);

                // 플레이어, 포탈, 벽, 다른 몬스터와 겹치면 건너뜀
                if (Data.map[y, x] != ' ' ||
                    (x == Data.playerX && y == Data.playerY) ||
                    (x == Data.portalX && y == Data.portalY) ||
                    Data.monsterPositions.Any(pos => pos.x == x && pos.y == y))
                    continue;

                Data.monsterPositions.Add((x, y));
                Data.map[y, x] = 'M';
                placed++;
            }
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
            MiniMap(); // floorChange true일 때만 작동
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

            // 이동 처리
            Data.map[Data.playerY, Data.playerX] = ' ';
            Data.playerX = newX;
            Data.playerY = newY;
            Data.map[Data.playerY, Data.playerX] = 'P';

            return true;
        }

        public static void MoveMonsters()
        {
            int size = Data.map.GetLength(0);
            var newPositions = new List<(int x, int y)>();

            foreach (var m in Data.monsterPositions)
            {
                int[] dx = { 0, 1, 0, -1 };
                int[] dy = { -1, 0, 1, 0 };

                int dir = Data.random.Next(0, 4);
                int nx = m.x + dx[dir];
                int ny = m.y + dy[dir];

                // 범위 밖, 벽, 플레이어, 다른 몬스터랑 겹치면 이동 안 함
                if (nx < 0 || ny < 0 || nx >= size || ny >= size) continue;
                if (Data.map[ny, nx] != ' ' || (nx == Data.playerX && ny == Data.playerY)) continue;
                if (newPositions.Any(pos => pos.x == nx && pos.y == ny)) continue;

                newPositions.Add((nx, ny));
                Data.map[m.y, m.x] = ' '; // 이전 위치 비움
                Data.map[ny, nx] = 'M';   // 새 위치에 몬스터 배치
            }

            // 새 위치 반영
            Data.monsterPositions = newPositions;
        }
    }

    // 방향 문자열을 반환하는 메서드
    private static string GetDirection(int dx, int dy)
    {
        string vertical = dy < 0 ? "북" : dy > 0 ? "남" : "";
        string horizontal = dx < 0 ? "서" : dx > 0 ? "동" : "";
        return vertical + horizontal;
    }
    // 조사하기
    public static void Search()
    {
        int px = Data.playerX;
        int py = Data.playerY;

        if (px == Data.treasureX && py == Data.treasureY)
        {
            Console.WriteLine("보물을 획득했다");
            Data.map[py, px] = ' ';
            Data.treasureX = -1;
            Data.treasureY = -1;
            return;
        }

        int range = 1 + (Data.Wis / 10);
        bool found = false;

        for (int y = py - range; y <= py + range; y++)
        {
            for (int x = px - range; x <= px + range; x++)
            {
                if ((x == px && y == py) || x < 0 || y < 0 || x >= Data.map.GetLength(0) || y >= Data.map.GetLength(1))
                    continue;

                if (x == Data.treasureX && y == Data.treasureY)
                {
                    found = true;
                    int roll = Data.dice20() + (Data.Wis / 2);
                    if (roll >= 10)
                    {
                        int dx = x - px;
                        int dy = y - py;
                        string direction = GetDirection(dx, dy);
                        Console.WriteLine($"{direction}에서 희미한 기운이 느껴진다…");
                    }
                    else
                    {
                        Console.WriteLine("뭔가 있을 것 같지만 확신이 없다.");
                    }
                    return;
                }
            }
        }

        if (!found)
        {
            Console.WriteLine("[아무것도 느껴지지 않는다.]");
        }
    }

    public static void Battle()
    {
        Console.Clear();
        Console.WriteLine("적을 만났습니다! 전투를 시작합니다.\n\n\n");

        int enemyHP = Data.random.Next(1 + (Data.floor * Data.floor), (Data.floor * 5) + (Data.floor * Data.floor));
        int enemyAttack = Data.random.Next(1 + Data.floor, 3 + Data.floor * 2);
        bool battleError = false;

        int playerSpeed = Data.Dex; //민첩비례 속도값
        int enemySpeed = Data.random.Next(Data.floor, Data.floor * 2);  //적의 속도값
        bool playerTurn = playerSpeed >= enemySpeed;// 턴조건

        while (Data.Hp > 0 && enemyHP > 0)
        {
            if (playerTurn)
            {
                Console.WriteLine($"\n\n적의 체력:   {enemyHP}");
                Console.WriteLine($"기본 공격력: {enemyAttack}");
                Console.WriteLine($"\n\n현재 체력:   {Data.Hp} / {Data.HpMax}");
                Console.WriteLine($"현재 마나:   {Data.Mp} / {Data.MpMax}");
                Console.WriteLine("\n당신의 차례입니다. 행동을 선택하세요:");
                Console.WriteLine("1. 공격하기");
                Console.WriteLine("2. 마법 사용하기 (MP 2 소모)");
                Console.WriteLine("3. 장비 사용하기");

                if (battleError) Console.WriteLine("잘못된 입력. 아무 행동도 하지 못했습니다.");
                string action = Console.ReadLine();

                switch (action)
                {
                    case "1":  // 공격
                        Console.Clear();
                        int damage = 0;
                        int roll = Data.dice20();
                        damage = roll >= 20 ? Data.Atk * 3 :
                                 roll >= 10 ? Data.Atk * 2 :
                                 Data.Atk;

                        Console.WriteLine($"공격! {damage}의 피해");
                        enemyHP -= damage;
                        break;

                    case "2": // 마법
                        Console.Clear();
                        int magicDamage = 0;
                        int mRoll = Data.dice20();
                        magicDamage = mRoll >= 20 ? Data.Int * 4 :
                                      mRoll >= 10 ? Data.Int * 3 :
                                      Data.Int * 2;

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

                    case "3":  // 장비




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

                playerTurn = false;
            }
            else
            {
                Console.WriteLine("\n적의 차례입니다.");
                int enemyDice = Data.random.Next(Data.floor, Data.floor * 2);
                float reduction = Data.Def / (100f + Data.Def);
                int bestEnemyDamage = enemyAttack + enemyDice;
                int enemyDamage = Math.Max(1, (int)(bestEnemyDamage * (1 - reduction)));
                int evasion = Math.Min(Data.Dex * 2, 101);
                int evasionRoll = Data.random.Next(1, 101);
                if (evasionRoll <= evasion)
                {
                    Console.WriteLine($"회피 성공! (회피 확률: {evasion}%)");
                }
                else
                {
                    Console.WriteLine($"적의 공격! {enemyDamage}의 피해를 입었습니다.");
                    Data.Hp -= enemyDamage;
                }

                if (Data.Hp <= 0)
                {
                    Console.WriteLine("당신은 쓰러졌습니다...");
                    break;
                }
                playerTurn = true;
            }
        }
    }

    public static void Skill(ref int enemyHP, int enemyAttack)
    {
        while (Data.Hp > 0 && enemyHP > 0)
        {
            Console.WriteLine($"\n\n적의 체력:   {enemyHP}");
            Console.WriteLine($"기본 공격력: {enemyAttack}");
            Console.WriteLine($"\n\n현재 체력:   {Data.Hp} / {Data.HpMax}");
            Console.WriteLine($"현재 마나:   {Data.Mp} / {Data.MpMax}");
            Console.WriteLine("\n행동을 선택하세요:");

            string action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    break;

                case "2":

                    break;

                case "3":

                    break;

                default:
                    continue;
            }
        }
    }
}
