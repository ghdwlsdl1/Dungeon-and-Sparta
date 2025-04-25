using System;
namespace Team_T_RPG
{
    public static class Data
    {
        public static string Name = ""; // 이름
        public static int JobNames = 0; // 직업 선택 번호
        public static string[] Job = { "없음", "전사", "도적", "마법사", "야만인" };// 직업이름
        public static int Hp = 0; // 현재 HP
        public static int HpMax = 0;// 최대 HP
        public static int Mp = 0; // 현재 MP
        public static int MpMax = 0; //최대 MP
        public static int Level = 1; //레벨 기초 값1
        public static int experience = 0; //경험치

        public static int Str = 0;  // 힘
        public static int Dex = 0;  // 민첩
        public static int Int = 0;  // 지능
        public static int Con = 0;  // 건강
        public static int Wis = 0;  // 지혜
        public static int Luk = 0;  // 운
        public static int startStr = 0;  // 힘
        public static int startDex = 0;  // 민첩
        public static int startInt = 0;  // 지능
        public static int startCon = 0;  // 건강
        public static int startWis = 0;  // 지혜
        public static int startLuk = 0;  // 운

        public static int Atk = 0; //공격력
        public static int Def = 0; // 방어력

        public static int Money = 2000000000;
        //====================마을 시스템====================
        public static bool duty = false;
        public static int Day = 1;
        //====================아이탬====================
        public static string[] weapon = { "없음", "롱소드", "단검", "지팡이", "도끼", "천상의 검", "밤의 송곳니", "빛의 지팡이", "파멸의 도끼" };// 아이탬 이름
        public static int[] weaponTf = { 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 소지 여부
        public static int[] weaponEquip = { 0, 0, 0, 0, 0, 0, 0, 0, 0 }; // 장착 여부
        public static int[] weaponAtk = { 0, 5, 5, 7, 3, 2, 0, 0, 0 }; // 추가 공격력
        //public static int[] weaponStats = { 0, 0, 0, 5, 5, 7, 0, 0, 0 }; // 추가 스텟
        public static int[][] weaponStats = new int[][] {
                    //힘 ,민첩, 지능
            new int[] { 0,  0,  0 },//없음
            new int[] { 0,  0,  0 },//롱소드
            new int[] { 0,  0,  0 },//단검
            new int[] { 0,  0,  5 },//지팡이
            new int[] { 5,  0,  0 },//도끼
            new int[] { 0,  5,  0 },//천상의 검
            new int[] { 0,  7,  0 },//밤의 송곳니
            new int[] { 0,  0,  7 },//빛의 지팡이
            new int[] { 7,  0,  0 }//파멸의 도끼
        };
        public static int[] weaponDeal = { 0, 1000, 1000, 1000, 1000, 1000, 5000, 5000, 5000, 5000 }; // 금액

        public static string[] assist = { "없음", "히터 실드", "행운의 부적", "빛나는 반지", "도바킨 투구", "태양 목걸이" };
        public static int[] assistTf = { 0, 0, 0, 0, 0, 0 };
        public static int[] assistEquip = { 0, 0, 0, 0, 0, 0 };
        public static int[] assistDef = { 0, 1, 2, 0, 0, 0, }; // 추가 방어력
        //public static int[] assistStats = { 0, 0, 0, 3, 5, 5 }; // 추가 스텟
        public static int[][] assistStats = new int[][] {
                    //건강,지혜,운
            new int[] { 0,  0,  0 },//없음
            new int[] { 0,  2,  0 },//히터 실드
            new int[] { 0,  0,  5 },//행운의 부적
            new int[] { 0,  3,  0 },//빛나는 반지
            new int[] { 2,  0,  0 },//도바킨 투구
            new int[] { 5,  0,  0 },//태양 목걸이
        };
        public static int[] assistDeal = { 0, 1000, 1000, 1000, 1000, 1000 }; // 금액

        public static string[] armor = { "없음", "낡은 갑옷", "가죽 갑옷", "사슬 갑옷", "강철 갑옷", "판금 갑옷" };
        public static int[] armorTf = { 0, 0, 0, 0, 0, 0 };
        public static int[] armorEquip = { 0, 0, 0, 0, 0, 0 };
        public static int[] armorDef = { 0, 1, 3, 5, 7, 10 }; // 추가 방어력
        public static int[] armorDeal = { 0, 1000, 2000, 3000, 4000, 5000 }; // 금액

        public static string[] potion = { "없음", "체력 물약", "상처약", "고급 상처약", "마나 물약", "파워 엘릭서" };
        public static int[] potionTf = { 0, 0, 0, 0, 0, 0 };
        public static int[] potionMp = { 0, 0, 0, 0, 50, 20 };//MP
        public static int[] potionHp = { 0, 10, 30, 50, 0, 40 }; // 추가 체력은 회복이라고 생각했습니다.
        public static int[] potionDeal = { 0, 10, 20, 30, 40, 50 }; // 금액 
        //====================몬스터====================

        public static string[] monster = { "고블린", "오크", "골램", "리치" };
        public static int[] monster_drop_weapon_index = { 2, 1, 3, 4 };
        public static int[] msAtk = { 0, 3, 3, 10 }; // 기본 공격력
        public static int[] msHp = { 5, 10, 15, 5 }; // 기본 체력
        public static int[] msDex = { 5, 0, -5, 0 }; // 기본 이속

        //====================주사위====================
        public static Random random = new Random(); // static으로 변경

        public static int dice20() // 20면 주사위
        {
            return random.Next(1, 21) + Luk;
        }

        public static int dice6() // 6면 주사위
        {
            return random.Next(1, 7);
        }
        //====================던전 시스템====================
        public static int dungeonDay = 3;   //날짜
        public static int dungeonHour = 0;   //시간
        //====================던전 맵====================
        public static int playerX = -1; //플레이어좌표x
        public static int playerY = -1;
        public static int portalX = -2; //포탈좌표
        public static int portalY = -2;
        public static int treasureX = -1; //보물좌표
        public static int treasureY = -1;
        public static int floor = 1; //층수
        public static bool floorChange = false; //층변경감지
        public static char[,] map; //맵
        public static List<(int x, int y)> monsterPositions = new List<(int x, int y)>();
        public static int monsterTurn = 0; //몬스터 행동
        public static int tired = 0;
        public static int ultimate = 0;
    }
}
