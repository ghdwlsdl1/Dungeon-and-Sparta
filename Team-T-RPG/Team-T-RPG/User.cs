using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_T_RPG
{
    public class User
    {
        public string Name = ""; // 이름
        public int JobNames = 0; // 직업 선택 번호
        public string[] Job = { "없음", "전사", "도적", "마법사", "야만인" };// 직업이름
        public int Hp = 0; // 현재 HP
        public int HpMax = 0;// 최대 HP
        public int Mp = 0; // 현재 MP
        public int MpMax = 0; //최대 MP
        public int Level = 1; //레벨 기초 값1
        public int experience = 0; //경험치

        public int Str = 0;  // 힘
        public int Dex = 0;  // 민첩
        public int Int = 0;  // 지능
        public int Con = 0;  // 건강
        public int Wis = 0;  // 지혜
        public int Luk = 0;  // 운
        public int startStr = 0;  // 힘
        public int startDex = 0;  // 민첩
        public int startInt = 0;  // 지능
        public int startCon = 0;  // 건강
        public int startWis = 0;  // 지혜
        public int startLuk = 0;  // 운

        public int Atk = 0; //공격력
        public int Def = 0; // 방어력

        public int Money = 2000;


    }
}
