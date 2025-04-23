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

    //던전UI
    public static bool DungeonEntry(ref bool DungeonEntryError, ref bool DungeonEntryEnd)
    {
        // === 턴 시작 시 몬스터 이동 ===
        MonstersSystem.MoveMonsters();

        // === 충돌 체크 ===
        for (int i = Data.monsterPositions.Count - 1; i >= 0; i--)
        {
            int mx = Data.monsterPositions[i].x;
            int my = Data.monsterPositions[i].y;

            int dx = Math.Abs(Data.playerX - mx);
            int dy = Math.Abs(Data.playerY - my);

            if (dx <= 1 && dy <= 1)
            {
                BattleSystem.Battle();
                Data.monsterPositions.RemoveAt(i);
                Data.map[my, mx] = ' ';
                MonstersSystem.PlaceMonsters(Data.floor);
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
                    BattleSystem.Battle();
                }
                break;

            case "5":
                SearchSystem.Search();
                break;

            default:
                Console.Clear();
                DungeonEntryError = true;
                break;
        }
        return true;
    }

    //던전생성
    public static class Dungeon
    {
        //크기
        public static int GetMapSize(int floor)
        {
            return 10 + (floor - 1) * 5;
        }
        //미니맵
        public static void MiniMap()
        {
            // 층 변경이 감지되지 않으면 실행하지 않음
            if (!Data.floorChange) return;

            // 현재 층 수에 따라 맵 크기를 계산하여 2차원 맵 배열 초기화
            int size = GetMapSize(Data.floor);
            Data.map = new char[size, size];

            // 1. 맵 전체를 빈 공간(' ')으로 초기화
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Data.map[y, x] = ' ';
                }
            }

            // 2. 랜덤하게 벽('■')을 배치
            int wallCount = size * size / 10; // 전체 맵의 약 10%를 벽으로 설정
            int placed = 0;
            while (placed < wallCount)
            {
                int wallX = Data.random.Next(0, size);
                int wallY = Data.random.Next(0, size);

                // 이미 채워진 곳이 아니면 벽을 배치
                if (Data.map[wallY, wallX] != ' ')
                    continue;

                Data.map[wallY, wallX] = '■';
                placed++;
            }

            // 3. 플레이어의 시작 위치를 빈 공간에 랜덤하게 배치
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

            // 4. 포탈의 위치를 빈 공간에 랜덤하게 배치 (플레이어 위치 제외)
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

            // 5. 보물의 위치를 빈 공간에 랜덤하게 배치 (플레이어 및 포탈 위치 제외)
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

            // 맵 변경 완료 후 플래그 초기화
            Data.floorChange = false;

            // 6. 현재 층 수에 비례하여 몬스터 배치
            int monsterCount = Data.floor;
            MonstersSystem.PlaceMonsters(monsterCount);

            // 7. 최종적으로 포탈과 플레이어 위치를 맵에 시각적으로 표시
            Data.map[Data.portalY, Data.portalX] = '◇'; // 포탈
            Data.map[Data.playerY, Data.playerX] = 'P'; // 플레이어
        }
        //지도표시
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

        // 플레이어 이동
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

            // 현재 위치 기준으로 새 위치 계산
            int newX = Data.playerX;
            int newY = Data.playerY;

            // 방향 선택 처리
            switch (text)
            {
                case "1": newY--; break; // 위로 이동
                case "2": newY++; break; // 아래로 이동
                case "3": newX++; break; // 오른쪽 이동
                case "4": newX--; break; // 왼쪽 이동
                default:
                    // 잘못된 입력 처리
                    Console.Clear();
                    moveError = true;
                    return false;
            }

            int size = GetMapSize(Data.floor);

            // 맵 범위 초과 또는 벽이면 이동 불가
            if (newX < 0 || newX >= size || newY < 0 || newY >= size || Data.map[newY, newX] == '■')
            {
                moveError2 = true;
                return false;
            }

            // 이동 처리
            Data.map[Data.playerY, Data.playerX] = ' '; // 이전 위치 지우기
            Data.playerX = newX;
            Data.playerY = newY;
            Data.map[Data.playerY, Data.playerX] = 'P'; // 새 위치 표시

            return true;
        }
    }

    //조사 시스템
    public static class SearchSystem
    {
        // 조사방향
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

            // 현재 위치에 보물이 있을 경우 즉시 획득
            if (px == Data.treasureX && py == Data.treasureY)
            {
                Console.Clear();
                Console.WriteLine("보물을 획득했다");
                Data.treasureX = -1; // 보물 위치 초기화
                Data.treasureY = -1;
                return;
            }

            int range = 2 + (Data.Wis / 2); // 탐색 범위 (지혜에 비례)
            bool found = false;

            // 탐색 범위 내 모든 좌표 확인
            for (int y = py - range; y <= py + range; y++)
            {
                for (int x = px - range; x <= px + range; x++)
                {
                    // 자기 자신 또는 맵 밖은 제외
                    if ((x == px && y == py) || x < 0 || y < 0 || x >= Data.map.GetLength(0) || y >= Data.map.GetLength(1))
                        continue;

                    // 탐색 범위 내에 보물이 있을 경우
                    if (x == Data.treasureX && y == Data.treasureY)
                    {
                        found = true;
                        int roll = Data.dice20() + (Data.Wis / 2); // 탐색 성공 확률 계산

                        if (roll >= 10)
                        {
                            // 성공: 방향 힌트 제공
                            Console.Clear();
                            int dx = x - px;
                            int dy = y - py;
                            string direction = GetDirection(dx, dy);
                            Console.WriteLine($"{direction}에서 희미한 기운이 느껴진다…");
                        }
                        else
                        {
                            // 실패
                            Console.Clear();
                            Console.WriteLine("뭔가 있을 것 같지만 확신이 없다.");
                        }
                        return; // 보물 찾기 여부에 관계없이 한 번만 시도함
                    }
                }
            }

            // 보물 자체가 탐색 범위에 없는 경우
            if (!found)
            {
                Console.Clear();
                Console.WriteLine("[아무것도 느껴지지 않는다.]");
            }
        }
    }

    //몬스터 시스템
    public static class MonstersSystem
    {
        // 몬스터 배치
        public static void PlaceMonsters(int count)
        {
            // 기존에 있던 몬스터 초기화 ('M' 제거)
            for (int y = 0; y < Data.map.GetLength(0); y++)
            {
                for (int x = 0; x < Data.map.GetLength(1); x++)
                {
                    if (Data.map[y, x] == 'M')
                        Data.map[y, x] = ' ';
                }
            }

            // 이전 몬스터 위치 정보 초기화
            Data.monsterPositions.Clear();

            int size = Data.map.GetLength(0); // 맵 크기
            int placed = 0; // 배치된 몬스터 수

            // 몬스터를 주어진 수만큼 랜덤 위치에 배치
            while (placed < count)
            {
                int x = Data.random.Next(0, size);
                int y = Data.random.Next(0, size);

                // 몬스터가 배치될 수 없는 위치 조건
                if (Data.map[y, x] != ' ' ||                      // 빈 공간이 아니거나
                    (x == Data.playerX && y == Data.playerY) ||   // 플레이어 위치거나
                    (x == Data.portalX && y == Data.portalY) ||   // 포탈 위치거나
                    Data.monsterPositions.Any(pos => pos.x == x && pos.y == y)) // 이미 배치된 몬스터 위치일 경우
                    continue;

                // 조건을 만족하면 해당 위치에 몬스터 배치
                Data.monsterPositions.Add((x, y)); // 위치 정보 저장
                Data.map[y, x] = 'M';              // 맵에 'M' 표시
                placed++;
            }
        }
        // 몬스터 움직임
        public static void MoveMonsters()
        {
            // 맵이나 몬스터 위치 정보가 없으면 실행하지 않음
            if (Data.map == null || Data.monsterPositions == null) return;

            int size = Data.map.GetLength(0); // 맵 크기
            var newPositions = new List<(int x, int y)>(); // 몬스터의 새로운 위치 저장용 리스트

            // 각 몬스터마다 순회
            foreach (var m in Data.monsterPositions)
            {
                // 유효하지 않은 좌표는 건너뜀
                if (m.x < 0 || m.y < 0 || m.x >= size || m.y >= size) continue;

                // 4방향(상하좌우) 랜덤 순서로 섞음
                var directions = new List<(int dx, int dy)>
                {
                    (0, -1), // 위
                    (1, 0),  // 오른쪽
                    (0, 1),  // 아래
                    (-1, 0)  // 왼쪽
                }.OrderBy(_ => Data.random.Next()).ToList();

                bool moved = false;

                // 랜덤한 방향 순서대로 이동 시도
                foreach (var (dx, dy) in directions)
                {
                    int nx = m.x + dx;
                    int ny = m.y + dy;

                    // 맵 밖이면 무시
                    if (nx < 0 || ny < 0 || nx >= size || ny >= size) continue;

                    // 빈칸이 아니거나, 플레이어 위치거나, 이미 다른 몬스터가 이동한 자리면 무시
                    if (Data.map[ny, nx] != ' ' ||
                        (nx == Data.playerX && ny == Data.playerY) ||
                        newPositions.Any(pos => pos.x == nx && pos.y == ny))
                        continue;

                    // 이동 가능한 경우
                    newPositions.Add((nx, ny));         // 새 위치에 등록
                    Data.map[m.y, m.x] = ' ';           // 원래 위치 비움
                    Data.map[ny, nx] = 'M';             // 새 위치에 몬스터 배치
                    moved = true;
                    break; // 이동 성공하면 더 이상 시도 안 함
                }

                // 모든 방향 이동 실패 → 기존 위치 유지
                if (!moved)
                {
                    newPositions.Add((m.x, m.y));
                }
            }

            // 몬스터 위치 갱신
            Data.monsterPositions = newPositions;
        }
    }

    //전투 시스탬
    public static class BattleSystem
    {
        // 전투 시스템
        public static void Battle()
        {
            // 콘솔 초기화 및 전투 시작 메시지
            Console.Clear();
            Console.WriteLine("적을 만났습니다! 전투를 시작합니다.\n\n\n");

            // 몬스터 능력치 설정 (층 수에 비례하여 HP, 공격력 생성)
            int monsterHP = Data.random.Next(1 + (Data.floor * Data.floor), (Data.floor * 5) + (Data.floor * Data.floor));
            int monsterAttack = Data.random.Next(1 + Data.floor, 3 + Data.floor * 2);
            bool battleError = false; // 잘못된 입력 여부

            // 속도값 설정
            int monsterSpeed = Data.random.Next(Data.floor, Data.floor * 2);
            int playerSpeed = Data.Dex;
            bool playerTurn = playerSpeed >= monsterSpeed;
            while (Data.Hp > 0 && monsterHP > 0)
            {

                // ----------------- 플레이어 턴 -----------------
                if (playerTurn)
                {
                    // 현재 상태 출력
                    Console.WriteLine($"\n[적 체력: {monsterHP}]  [플레이어 체력: {Data.Hp} / {Data.HpMax}]  [마나: {Data.Mp} / {Data.MpMax}]");
                    Console.WriteLine("플레이어 턴입니다.\n1. 공격하기\n2. 마법 사용하기 (MP 2 소모)\n3. 장비 사용하기");

                    // 이전 턴에 잘못된 입력이 있었는지 표시
                    if (battleError) Console.WriteLine("잘못된 입력. 아무 행동도 하지 못했습니다.");

                    string input = Console.ReadLine();

                    switch (input)
                    {
                        case "1": // 일반 공격
                            Console.Clear();
                            int roll = Data.dice20(); // d20 주사위 굴림
                                                      // 주사위 결과에 따라 데미지 배율 적용
                            int damage = roll >= 20 ? Data.Atk * 3 :
                                         roll >= 10 ? Data.Atk * 2 :
                                         Data.Atk;

                            Console.WriteLine($"공격! {damage}의 피해");
                            monsterHP -= damage;
                            break;

                        case "2": // 마법 사용 (아직 구현 안 됨)
                            Console.Clear();
                            Console.WriteLine("마법 사용은 아직 구현되지 않았습니다.");
                            break;

                        case "3": // 장비 사용 (아직 구현 안 됨)
                            Console.Clear();
                            Console.WriteLine("장비 사용은 아직 구현되지 않았습니다.");
                            break;

                        default: // 잘못된 입력
                            Console.Clear();
                            battleError = true;
                            continue; // 이 턴은 무효 처리
                    }

                    // 몬스터가 쓰러진 경우
                    if (monsterHP <= 0)
                    {
                        Console.WriteLine("적을 쓰러뜨렸습니다!");
                        Data.experience += Data.dice20() * Data.floor; // 경험치 지급
                        Data.Money += Data.dice20() * (Data.floor * Data.floor); // 돈 지급
                                                                                 //Stats.UpdateStats(); // 스탯 갱신 필요 시 주석 해제
                                                                                 //Quest.ReportKill("몹 이름");
                        break;
                    }

                    playerTurn = false; // 다음은 몬스터 턴
                }

                // ----------------- 몬스터 턴 -----------------
                if (!playerTurn)
                {
                    Console.WriteLine("\n적의 차례입니다.");

                    int enemyDice = Data.random.Next(Data.floor, Data.floor * 2);
                    float reduction = Data.Def / (100f + Data.Def); // 데미지 감소율 (방어력 기반)
                    int bestEnemyDamage = monsterAttack + enemyDice;
                    int enemyDamage = Math.Max(1, (int)(bestEnemyDamage * (1 - reduction))); // 최소 피해 1

                    // 회피 확률 계산
                    int evasion = Math.Min(Data.Dex * 2, 101); //민첩에 비례
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

                    // 플레이어 사망 시 전투 종료
                    if (Data.Hp <= 0)
                    {
                        Console.WriteLine("당신은 쓰러졌습니다...");
                        break;
                    }
                    playerTurn = true; // 다음은 몬스터 턴
                }

                // 턴 종료 시 현재 상태 출력
                Console.WriteLine($"\n[플레이어 HP: {Data.Hp}]  [몬스터 HP: {monsterHP}]\n");
            }

            // 전투 종료 대기
            Console.ReadKey();
        }

        //스킬
        public static void Skill(ref int monsterHP, int monsterAttack)
        {
            while (Data.Hp > 0 && monsterHP > 0)
            {
                Console.WriteLine($"\n\n적의 체력:   {monsterHP}");
                Console.WriteLine($"기본 공격력: {monsterAttack}");
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
}
