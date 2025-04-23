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

        public static int Money = 2000;
        //====================마을 시스템====================
        public static bool duty = false;
        public static int Day = 1;
        //====================아이탬====================
        public static string[] weapon = { "없음", "무딘 검", "강철 검", "전투용 망치", "단검", "지팡이" };// 아이탬 이름
        //public static bool[] weaponTf = { false, false, false, false, false, false }; // 소지 여부
        public static int[] weaponTf = { 0, 2, 1, 1, 1, 1 }; // 탬 갯수
        public static bool[] weaponEquip = { false, false, true, false, false, false }; // 장착 여부
        //public static int[] weaponEquip = { 0, 0, 0, 0, 0, 0 }; // 0없음 1있음 2착용
        public static int[] weaponAtk = { 0, 2, 5, 7, 3, 2 }; // 추가 공격력
        public static int[] weaponStats = { 0, 0, 0, 5, 5, 7 }; // 추가 스텟
        public static int[] weaponDeal = { 0, 1000, 3000, 5000, 5000, 5000 }; // 금액

        public static string[] assist = { "없음", "철제 장화", "철제 투구", "행운의 부적", "빛나는 반지", "태양 목걸이" };
        public static int[] assistTf = { 0, 0, 0, 0, 0, 0 }; // 탬 갯수
        public static bool[] assistEquip = { false, false, false, false, false, false };
        public static int[] assistDef = { 0, 1, 2, 0, 0, 0 }; // 추가 방어력
        public static int[] assistStats = { 0, 0, 0, 3, 5, 5 }; // 추가 스텟
        public static int[] assistDeal = { 0, 2000, 4000, 5000, 5000, 5000 }; // 금액

        public static string[] armor = { "없음", "낡은 갑옷", "가죽 갑옷", "사슬 갑옷", "강철 갑옷", "전사의 판금 갑옷" };
        public static int[] armorTf = { 0, 0, 0, 0, 0, 0 }; // 탬 갯수
        public static bool[] armorEquip = { false, false, false, false, false, false };
        public static int[] armorDef = { 0, 1, 3, 5, 7, 10 }; // 추가 방어력
        public static int[] armorDeal = { 0, 1000, 2000, 3000, 4000, 5000 }; // 금액




        //====================주사위====================
        public static Random random = new Random(); //랜덤
        public static int dice20() //20면 주사위
        {
            return random.Next(1, 21) + Luk;
        }
        public static int dice6() //6면 주사위
        {
            return random.Next(1, 7);
        }
        //====================던전 시스템====================
        public static int dungeonDay = 3;
        public static int dungeonHour = 0;
        //====================던전 맵====================
        public static int playerX = -1; //포탈좌표x
        public static int playerY = -1; //포탈좌표x
        public static int portalX = -1; //포탈좌표x
        public static int portalY = -1; //포탈좌표y
        public static int floor = 1; //층수
        public static char[,] map; //
    }
}
