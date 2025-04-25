using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Team_T_RPG;
using static DungeonSystem.Dungeon;
using static System.Formats.Asn1.AsnWriter;

public static class DungeonSystem
{
    static Stats stats = new Stats();
    static Inventory inventory = new Inventory();
    //던전타임
    public static void dungeonTime()
    {
        if (Data.dungeonHour >= 24)
        {
            Data.dungeonDay--;
            Data.dungeonHour -= 24;
        }
    }
    public static string search_result_print = "";
    //던전UI
    public static bool DungeonEntry(ref bool DungeonEntryError, ref bool DungeonEntryEnd)
    {
        // === 턴 시작 시 몬스터 이동 ===
        if (Data.monsterTurn > 0)
        {
            for (int i = 0; i < Data.monsterTurn; i++)
            {
                MonstersSystem.MoveMonsters(); // 몬스터 움직임
            }

            Data.monsterTurn = 0; // 다 움직였으면 초기화
        }

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
                int targetMonsterCount = Data.floor * 2;
                int currentMonsterCount = Data.monsterPositions.Count;
                int toSpawn = targetMonsterCount - currentMonsterCount;

                if (toSpawn > 0)
                    MonstersSystem.PlaceMonsters(toSpawn);
                Data.dungeonHour += 2;
                return true;
            }
        }

        dungeonTime();

        if (Data.dungeonDay == 0)
        {
            DungeonEntryEnd = true;
            return false;
        }

        if (Data.Hp <= 0)
        {
            DungeonEntryEnd = true;
            return false;
        }
        //포탈통과
        if (Data.portalY == Data.playerY && Data.portalX == Data.playerX)
        {
            Data.playerX = -1; Data.playerY = -1;
            Data.portalX = -2; Data.portalY = -2;
            Data.floorChange = true;
            Data.floor++;
            Data.dungeonDay *= 2;

            Data.Money += 200 * Data.floor * Data.floor;
        }
        // 퀘스트
        QuestManager.ReportFloorReached(Data.floor);

        Console.WriteLine($"현재 층 수 : {Data.floor}");
        Console.WriteLine($"{Data.floor}층 붕괴까지 : {Data.dungeonDay} 일 {24 - Data.dungeonHour}시간 남음");
        Console.WriteLine("--------------------------------------------------------------------------------------");
        Dungeon.MiniMap();
        Dungeon.PrintMap();
        Console.WriteLine("--------------------------------------------------------------------------------------");
        if (DungeonEntryError)
        {
            Console.WriteLine("잘못된 입력입니다.\n");
            DungeonEntryError = false;
        }

        if (search_result_print != "")
        {
            Console.WriteLine("탐색 결과 : " + search_result_print);
            search_result_print = "";
        }

        if (Data.tired >= 12)
        {
            Console.WriteLine("피로가 몰려오는 것이 느껴집니다.");
            Console.WriteLine("--------------------------------------------------------------------------------------");
        }
        else if (Data.tired >= 20)
        {
            Console.WriteLine("극심한 피로로 인해 모든 행동에 제약이 생깁니다.");
            Console.WriteLine("--------------------------------------------------------------------------------------");
        }
        Console.WriteLine("무엇을 할까?\n");
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 이동하기");
        Console.WriteLine("4. 휴식하기");
        Console.WriteLine("5. 조사하기");
        string text = Console.ReadLine();
        
        switch (text)
        {
            case "1":
                Console.Clear();
                stats.ShowStatTable();
                break;

            case "2":
                Console.Clear();
                inventory.showInventory();
                Console.Clear();
                break;

            case "3":
                Data.monsterTurn += 1; Data.dungeonHour += 1; Data.tired += 1; Data.ultimate += 1;
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
                Data.monsterTurn += 6; Data.dungeonHour += 6; Data.tired -= 6; Data.ultimate += 6;
                Console.Clear();
                Console.WriteLine("당신은 지친 몸을 눕히고 휴식하기로 했습니다.\n");
                Console.ForegroundColor = ConsoleColor.Blue;
                MainFrame.SerialTextWrite("휴식 취하는 중 ▶▷▶▷▶");
                Console.ResetColor();
                Console.Clear();
                break;

            case "5":
                Data.monsterTurn += 3; Data.dungeonHour += 3; Data.tired += 3; Data.ultimate += 3;
                search_result_print = SearchSystem.Search();
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
            int monsterCount = Data.floor*2;
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
            Console.WriteLine($"현재 층 수 : {Data.floor}");
            Console.WriteLine($"{Data.floor}층 붕괴까지 : {Data.dungeonDay} 일 {24 - Data.dungeonHour}시간 남음");
            Console.WriteLine("--------------------------------------------------------------------------------------");
            MiniMap();
            PrintMap();
            Console.WriteLine("--------------------------------------------------------------------------------------");
            Console.WriteLine("어느 방향으로 갈까?\n");
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
                Console.WriteLine("벽과 마주한 당신은, 다른 방향으로 이동하기로 했습니다.\n");
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
        public static string Search()
        {
            int px = Data.playerX;
            int py = Data.playerY;
            string search_result = "";
            // 현재 위치에 보물이 있을 경우 즉시 획득
            if (px == Data.treasureX && py == Data.treasureY)
            {
                Console.Clear();
                int treasureGold = Data.dice20() * Data.floor * 70;
                Data.Money += treasureGold;
                Console.WriteLine("수상한 레버를 당기자, 작은 보물상자가 나타났다!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                MainFrame.SerialTextWrite($"상자 안에서 {treasureGold}G 를 발견했다!");
                Console.ResetColor();
                Thread.Sleep(1000);
                Console.Clear();
                Data.treasureX = -1; // 보물 위치 초기화
                Data.treasureY = -1;
                return search_result;
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
                            search_result = $"{direction}쪽에서 희미한 기운이 느껴진다…";
                        }
                        else
                        {
                            // 실패
                            Console.Clear();
                            search_result = "희미한 기운이 느껴지지만, 이내 사라졌다…";
                        }
                        return search_result; // 보물 찾기 여부에 관계없이 한 번만 시도함
                    }
                }
            }

            // 보물 자체가 탐색 범위에 없는 경우
            if (!found)
            {
                Console.Clear();
                search_result = "아무 것도 느껴지지 않는다…";
                return search_result;
            }
            return search_result;
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
        public static bool smokeShell = false;
        public static bool anger = false;
        public static bool immortality = false;
        public static int doping = 0;
        // 전투 시스템
        public static void Battle()
        {
            Stats stats = new Stats();
            // 콘솔 초기화 및 전투 시작 메시지

            Console.WriteLine("\n\n\n");
            MainFrame.SerialTextWrite("적을 만났습니다! 전투를 시작합니다.",70);
            Console.Clear();
            // 몬스터 능력치 설정 (층 수에 비례하여 HP, 공격력 생성)
            int monsterIndex = Data.random.Next(Data.monster.Length); // 배열 기반 몬스터 랜덤 선택
            string monsterName = Data.monster[monsterIndex]; // 몬스터 이름
            int monsterHp = Data.msHp[monsterIndex] + Data.random.Next(1 + (Data.floor * Data.floor), (Data.floor * 5) + (Data.floor * Data.floor));
            int monsterHpMX = monsterHp;
            int monsterAttack = Data.msAtk[monsterIndex] + Data.random.Next(1 + Data.floor, 3 + Data.floor * 2);
            bool battleError = false; // 잘못된 입력 여부

            // 속도값 설정
            int monsterSpeed = Data.msDex[monsterIndex] + Data.random.Next(Data.floor, Data.floor * 2);
            int playerSpeed = Data.Dex;
            bool playerTurn = playerSpeed >= monsterSpeed;

            int TimetoDie = 0;
            while (Data.Hp > 0 && monsterHp > 0)
            {
                if (immortality)
                {
                    TimetoDie++;
                    if (TimetoDie >= 5)
                    {
                        Console.WriteLine("불사의 힘이 빠져나가는 것이 느껴집니다.");
                        Data.Hp = 0;
                        return;
                    }
                }

                if (smokeShell)
                {
                    Console.WriteLine("자욱한 연기 사이로 몰래 도망쳤습니다!");
                    Data.Dex -= doping;
                    doping = 0;
                    stats.UpdateStats();
                    smokeShell = false;
                    return;
                }
                // ----------------- 플레이어 턴 -----------------
                if (playerTurn)
                {
                    // 현재 상태 출력
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[나의 차례]\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{monsterName}]");
                    Console.WriteLine($"체력: {monsterHp} / {monsterHpMX}");
                    Console.WriteLine($"공격력 : {monsterAttack}");
                    Console.Write("---------V");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("S---------\n");
                    Console.WriteLine($"[{Data.Name} / {Data.Job[Data.JobNames]}]");
                    Console.WriteLine($"체력: {Data.Hp} / {Data.HpMax}");
                    Console.WriteLine($"마나: {Data.Mp} / {Data.MpMax}\n");
                    Console.ResetColor();
                    Console.WriteLine("---------------------");
                    Console.WriteLine("당신의 선택은…\n1. 공격하기\n2. 마법 사용하기");
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
                            if (anger)
                            {
                                damage += Data.Con;
                                Console.WriteLine($"분노공격! {damage}의 피해 {damage/10}흡혈");
                                Data.Hp += damage / 10;
                                stats.UpdateStats();
                                monsterHp -= damage;
                            }
                            else
                            {
                                Console.WriteLine($"공격! {damage}의 피해");
                                monsterHp -= damage;
                            }
                            break;

                        case "2": // 마법 사용
                            Console.Clear();
                            Skill(ref monsterHp, monsterAttack, monsterHpMX);
                            break;

                        case "3": // 아이템 사용
                            Console.Clear();
                            bool usedPotion = inventory.showInventoryPotion();
                            if (!usedPotion)
                            {
                                Console.Clear();
                                continue;
                            }
                            Console.Clear();
                            break;

                        default: // 잘못된 입력
                            Console.Clear();
                            battleError = true;
                            continue; // 이 턴은 무효 처리
                    }

                    // 몬스터가 쓰러진 경우
                    if (monsterHp <= 0)
                    {
                        Console.WriteLine("적을 쓰러뜨렸습니다!");
                        QuestManager.ReportKill(monsterName);

                        int dropRoll = Data.dice20(); // Luk 반영 주사위
                        if (dropRoll >= 15) // 조건: 주사위 결과가 15 이상일 경우 드랍
                        {
                            Console.WriteLine($"[아이템 드랍] {Data.weapon[Data.monster_drop_weapon_index[monsterIndex]]}을(를) 획득했다!");
                            Data.weaponTf[Data.monster_drop_weapon_index[monsterIndex]]++;
                        }
                        else
                        {
                            Console.WriteLine("[아이템 드랍 실패] 이 녀석은 아무것도 가지고 있지 않았다...");
                        }
                        int drop_exp = Data.dice20() * Data.floor;
                        int drop_money = Data.dice20() * (Data.floor * Data.floor);
                        Console.WriteLine($"경험치({drop_exp})와 골드({drop_money}를 획득했다!)");
                        Data.experience += drop_exp;
                        Data.Money += drop_money;
                        Data.Dex -= doping;
                        doping = 0;
                        stats.UpdateStats();
                        anger = false;
                        immortality = false;
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
                    else if (immortality)
                    {
                        Console.WriteLine($"적의 공격! {enemyDamage}의 피해를 입었습니다.");
                        if(Data.Hp< enemyDamage)
                        {
                            Console.WriteLine($"불사 상태입니다 피가 1로 고정됩니다.");
                            Data.Hp = 1;
                        }
                        else Data.Hp -= enemyDamage;
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
            }

            // 전투 종료 대기
            Console.ReadKey();
            Console.Clear();
        }

        //스킬
        public static void Skill(ref int monsterHp, int monsterAttack, int monsterHpMX)
        {
            Stats stats = new Stats();

            Console.WriteLine($"\n적 체력: {monsterHp} / {monsterHpMX}");
            Console.WriteLine($"적공격력{monsterAttack}.");
            Console.WriteLine($"\n체력: {Data.Hp} / {Data.HpMax}");
            Console.WriteLine($"마나: {Data.Mp} / {Data.MpMax}");
            switch (Data.JobNames)
            {
                case 1: // 전사
                    Console.WriteLine("\n1. 강타(Mp2)"); //힘비례 대미지
                    Console.WriteLine("\n2. 붕대감기(Mp5)"); //피 채움
                    if (Data.ultimate >= 20) Console.WriteLine("\n3. 처형"); //궁극기 dungeonDay 쿨타임  처형
                    break;
                case 2: //도적
                    Console.WriteLine("\n1. 표창 투척(Mp2)"); // 민첩비례 대미지
                    Console.WriteLine("\n2. 연막탄 투척(Mp5)"); //도망치기
                    if (Data.ultimate >= 20) Console.WriteLine("\n3. 그림자 분신"); //궁극기 dungeonDay 쿨타임 민첩 도핑
                    break;
                case 3: //마법사
                    Console.WriteLine("\n1. 파이어 볼(Mp1)"); //지능비레 대미지
                    Console.WriteLine("\n2. 익스플로전(Mp3)"); //지능비례 중간 대미지
                    if (Data.ultimate >= 20) Console.WriteLine("\n3. 메테오"); //궁극기 dungeonDay 쿨타임 지능비례 대량 대미지
                    break;
                case 4: //야만인
                    Console.WriteLine("\n1. 몸통 박치기(Mp2)"); //채력비례 대미지
                    Console.WriteLine("\n2. 분노(Mp5)"); // 채력을 깎아서 흡혈추가
                    if (Data.ultimate >= 20) Console.WriteLine("\n3. 마지막 발악"); //궁극기 dungeonDay 좀비화
                    break;
            }
            Console.WriteLine("\n행동을 선택하세요:");
            string action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    switch (Data.JobNames)
                    {
                        case 1: // 전사
                                // Console.WriteLine("\n1. 강타(Mp2)"); //힘비례 대미지
                            Console.Clear();
                            int warriorDamage = 0;
                            int wRoll = Data.dice20();
                            warriorDamage = wRoll >= 20 ? Data.Atk + Data.Str * 2 :
                                          wRoll >= 10 ? Data.Atk + Data.Str :
                                          Data.Atk + Data.Str / 2;

                            if (Data.Mp >= 2)
                            {
                                Console.WriteLine($"강타! {warriorDamage}의 피해");
                                monsterHp -= warriorDamage;
                                Data.Mp -= 2;
                            }
                            else
                            {
                                Console.WriteLine("마나가 부족합니다.");
                            }

                            break;
                        case 2: //도적
                                // Console.WriteLine("\n1. 표창 투척(Mp2)"); // 민첩비례 대미지
                            Console.Clear();
                            int rogueDamage = 0;
                            int rRoll1 = Data.dice20();
                            rogueDamage = rRoll1 >= 20 ? Data.Atk + Data.Dex * 2 :
                                          rRoll1 >= 10 ? Data.Atk + Data.Dex :
                                          Data.Atk + Data.Dex / 2;

                            if (Data.Mp >= 2)
                            {
                                Console.WriteLine($"표창 투척! {rogueDamage}의 피해");
                                monsterHp -= rogueDamage;
                                Data.Mp -= 2;
                            }
                            else
                            {
                                Console.WriteLine("마나가 부족합니다.");
                            }

                            break;
                        case 3: //마법사
                                // Console.WriteLine("\n1. 파이어 볼(Mp1)"); //지능비레 대미지
                            Console.Clear();
                            int magicianDamage = 0;
                            int mRoll = Data.dice20();
                            magicianDamage = mRoll >= 20 ? Data.Atk / 2 + Data.Int * 3 :
                                          mRoll >= 10 ? Data.Atk / 2 + Data.Int * 2 :
                                          Data.Atk / 2 + Data.Int;

                            if (Data.Mp >= 1)
                            {
                                Console.WriteLine($"파이어 볼! {magicianDamage}의 피해");
                                monsterHp -= magicianDamage;
                                Data.Mp -= 1;
                            }
                            else
                            {
                                Console.WriteLine("마나가 부족합니다.");
                            }

                            break;
                        case 4: //야만인
                                // Console.WriteLine("\n1. 몸통 박치기(Mp2)"); //채력비례 대미지
                            Console.Clear();
                            int barbarianDamage = 0;
                            int bRoll = Data.dice20();
                            barbarianDamage = bRoll >= 20 ? Data.Atk + Data.Con * 2 :
                                          bRoll >= 10 ? Data.Atk + Data.Con :
                                          Data.Atk + Data.Con / 2;

                            if (Data.Mp >= 2)
                            {
                                Console.WriteLine($"몸통 박치기! {barbarianDamage}의 피해");
                                monsterHp -= barbarianDamage;
                                Data.Mp -= 2;
                            }
                            else
                            {
                                Console.WriteLine("마나가 부족합니다.");
                            }

                            break;
                    }

                    break;

                case "2":
                    switch (Data.JobNames)
                    {
                        case 1: // 전사
                                // Console.WriteLine("\n2. 붕대감기(Mp5)"); //피 채움
                            if (Data.Mp >= 5)
                            {
                                Console.WriteLine($"회복합니다.");
                                Data.Mp -= 5;
                                Data.Hp += Data.HpMax /2 ;
                                stats.UpdateStats();
                            }
                            else
                            {
                                Console.WriteLine("마나가 부족합니다.");
                            }

                            break;
                        case 2: //도적
                                // Console.WriteLine("\n2. 연막탄 투척"); //도망치기
                            if (Data.Mp >= 5)
                            {
                                Console.WriteLine($"연막탄을 던집니다.");
                                Data.Mp -= 5;
                                BattleSystem.smokeShell = true;
                            }
                            else
                            {
                                Console.WriteLine("마나가 부족합니다.");
                            }
                            break;
                        case 3: //마법사
                                // Console.WriteLine("\n2. 익스플로전"); //지능비례 대미지
                            Console.Clear();
                            int warriorDamage = 0;
                            int wRoll = Data.dice20();
                            warriorDamage = wRoll >= 20 ? Data.Atk + Data.Str * 2 :
                                          wRoll >= 10 ? Data.Atk + Data.Str :
                                          Data.Atk + Data.Str / 2;

                            if (Data.Mp >= 3)
                            {
                                Console.WriteLine($"익스플로전! {warriorDamage}의 피해");
                                monsterHp -= warriorDamage;
                                Data.Mp -= 3;
                            }
                            else
                            {
                                Console.WriteLine("마나가 부족합니다.");
                            }
                            break;
                        case 4: //야만인
                                // Console.WriteLine("\n2. 분노"); // 채력을 깎아서 흡혈추가
                            if (Data.Mp >= 5)
                            {
                                Console.WriteLine($"분노합니다.");
                                BattleSystem.anger = true;
                                Data.Mp -= 5;
                                Data.Hp = Data.Hp / 2;
                            }
                            else
                            {
                                Console.WriteLine("마나가 부족합니다.");
                            }
                            break;
                    }

                    break;

                case "3":
                    switch (Data.JobNames)
                    {
                        case 1: // 전사
                                // Console.WriteLine("\n3. 처형"); //궁극기 dungeonDay 쿨타임  처형
                            Console.Clear();
                            int warriorDamage = 0;
                            warriorDamage = monsterHpMX / 2;

                            if (Data.ultimate >= 20)
                            {
                                Console.WriteLine($"처형! {warriorDamage}의 피해");
                                monsterHp -= warriorDamage;
                                Data.ultimate = 0;
                            }
                            else
                            {
                                Console.WriteLine("사용할 수 없습니다.");
                            }

                            break;
                        case 2: //도적
                                // Console.WriteLine("\n3. 그림자 분신"); //궁극기 dungeonDay 쿨타임 민첩 도핑
                            doping = Data.Dex;
                            if (Data.ultimate >= 20)
                            {
                                Console.WriteLine($"그림자 분신을 사용합니다.");
                                Data.Dex += doping;
                                Data.ultimate = 0;
                            }
                            else
                            {
                                Console.WriteLine("사용할 수 없습니다.");
                            }

                            break;
                        case 3: //마법사
                                // Console.WriteLine("\n3. 메테오"); //궁극기 dungeonDay 쿨타임 지능비례 짱샌대미지
                            Console.Clear();
                            int magicianDamage = 0;
                            int mRoll = Data.dice20();
                            magicianDamage = mRoll >= 20 ? Data.Int * 15 : Data.Int * 10;

                            if (Data.ultimate >= 20)
                            {
                                Console.WriteLine($"메테오! {magicianDamage}의 피해");
                                monsterHp -= magicianDamage;
                                Data.ultimate = 0;
                            }
                            else
                            {
                                Console.WriteLine("사용할 수 없습니다.");
                            }

                            break;
                        case 4: //야만인
                                // Console.WriteLine("\n3. 마지막 발악"); //궁극기 dungeonDay 좀비화
                            if (Data.ultimate >= 20)
                            {
                                Console.WriteLine($"마지막 발악을 시작합니다.");
                                BattleSystem.immortality = true;
                                Data.ultimate = 0;
                            }
                            else
                            {
                                Console.WriteLine("사용할 수 없습니다.");
                            }

                            break;
                    }

                    break;
            }
        }
    }
    
        
}
