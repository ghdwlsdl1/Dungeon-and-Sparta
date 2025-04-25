using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_T_RPG
{
    public class Inventory
    {
        public void showInventory()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("0.뒤로가기");
                Console.WriteLine("1.무기");
                Console.WriteLine("2.보조장비");
                Console.WriteLine("3.아머");
                Console.WriteLine("4.포션");
                int userinput = MainFrame.UserInputHandler(0, 4);
                switch (userinput)
                {
                    case 0:
                        return;
                    case 1:
                        showInventoryWeapon();
                        break;
                    case 2:
                        showInventoryAssist();
                        break;
                    case 3:
                        showInventoryArmor();
                        break;
                    case 4:
                        showInventoryPotion();
                        break;

                }

            }
        }
        public void showInventoryWeapon()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"-1. 장비해제 ");
                for (int i = 1; i < Data.weapon.Length; i++)
                {
                    if (Data.weaponTf[i] > 0)
                    {
                        string equippedMark = (Data.weaponEquip[i] == 1) ? " (장착중)" : "";
                        string statText = $"힘:{Data.weaponStats[i][0]} 민첩:{Data.weaponStats[i][1]} 지능:{Data.weaponStats[i][2]} ";

                        // 나머지 무기는 상세 정보 출력
                        Console.WriteLine($"{i}. {Data.weapon[i]} 소지 갯수: {Data.weaponTf[i]}  공격력: {Data.weaponAtk[i]}  스탯: {statText}{equippedMark}");

                    }
                    else
                    {
                        Console.WriteLine($"{i}.미획득");
                    }
                }
                Console.Write("\n장착할 무기 번호를 입력하세요: ");

                Console.WriteLine("0.뒤로가기");
                int userinput = MainFrame.UserInputHandler(-1, Data.weapon.Length);
                if (userinput == 0)
                {
                    break; // 메뉴 종료
                }

                if (userinput > 0 && userinput < Data.weapon.Length && Data.weaponTf[userinput] > 0)
                {
                    // 기존 무기 해제
                    for (int i = 0; i < Data.weaponEquip.Length; i++)
                    {
                        Data.weaponEquip[i] = 0;
                    }

                    // 선택한 무기 장착
                    Data.weaponEquip[userinput] = 1;

                    Console.WriteLine($"\n▶ [{Data.weapon[userinput]}]를 장착했습니다!");
                    Thread.Sleep(1000);
                }
                else if (userinput == -1)
                {
                    for (int i = 0; i < Data.weaponEquip.Length; i++)
                    {
                        Data.weaponEquip[i] = 0;
                    }
                    Data.weaponEquip[0] = 1;
                    Console.WriteLine($"\n▶ 장비가 장착 해제되었습니다!");

                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine("※ 잘못된 선택이거나 해당 무기를 소지하고 있지 않습니다.");
                    Thread.Sleep(1000);
                }
            }
        }
        public void showInventoryAssist()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"0. 장비해제 ");
                for (int i = 1; i < Data.assist.Length; i++)
                {
                    if (Data.assistTf[i] > 0)
                    {
                        string equippedMark = (Data.assistEquip[i] == 1) ? " (장착중)" : "";
                        string statText = $"건강:{Data.assistStats[i][0]} 지혜:{Data.assistStats[i][1]} 운:{Data.assistStats[i][2]} ";
                        Console.WriteLine($"{i}. {Data.assist[i]} 소지 갯수: {Data.assistTf[i]}  방어력: {Data.assistDef[i]}  스탯: {statText}{equippedMark}");


                    }
                    else
                    {
                        Console.WriteLine($"{i}.미획득");
                    }
                }
                Console.Write("\n장착할 보조장비 번호를 입력하세요: ");
                Console.WriteLine("-1.뒤로가기");
                int userinput = MainFrame.UserInputHandler(-1, Data.assist.Length);
                if (userinput == -1)
                {
                    break; // 메뉴 종료
                }

                if (userinput > 0 && userinput < Data.assist.Length && Data.assistTf[userinput] > 0)
                {
                    for (int i = 0; i < Data.assistEquip.Length; i++)
                    {
                        Data.assistEquip[i] = 0;
                    }

                    Data.assistEquip[userinput] = 1;

                    Console.WriteLine($"\n▶ [{Data.assist[userinput]}]를 장착했습니다!");
                    Thread.Sleep(1000);
                }
                else if (userinput == 0)
                {
                    for (int i = 0; i < Data.assistEquip.Length; i++)
                    {
                        Data.assistEquip[i] = 0;
                    }
                    Data.assistEquip[0] = 1;
                    Console.WriteLine($"\n▶ 장비가 장착 해제되었습니다!");

                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine("※ 잘못된 선택이거나 해당 무기를 소지하고 있지 않습니다.");
                    Thread.Sleep(1000);
                }

            }
        }
        public void showInventoryArmor()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"0. 장비해제 ");
                for (int i = 1; i < Data.armor.Length; i++)
                {
                    if (Data.armorTf[i] > 0)
                    {
                        string equippedMark = (Data.armorEquip[i] == 1) ? " (장착중)" : "";
                        string statText = $"방어력:{Data.armorDef[i]} ";
                        Console.WriteLine($"{i}. {Data.armor[i]} 소지 갯수: {Data.armorTf[i]}  방어력: {Data.armorDef[i]} {equippedMark}");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.WriteLine($"{i}.미획득");
                    }
                }
                Console.Write("\n장착할 무기 번호를 입력하세요: ");
                Console.WriteLine("-1.뒤로가기");
                int userinput = MainFrame.UserInputHandler(-1, Data.armor.Length);
                if (userinput == -1)
                {
                    break; // 메뉴 종료
                }

                if (userinput > 0 && userinput < Data.armor.Length && Data.armorTf[userinput] > 0)
                {
                    // 기존 무기 해제
                    for (int i = 0; i < Data.armorEquip.Length; i++)
                    {
                        Data.armorEquip[i] = 0;
                    }

                    // 선택한 무기 장착
                    Data.armorEquip[userinput] = 1;

                    Console.WriteLine($"\n▶ [{Data.armor[userinput]}]를 장착했습니다!");
                    Thread.Sleep(1000);
                }
                else if (userinput == 0)
                {
                    for (int i = 0; i < Data.armorEquip.Length; i++)
                    {
                        Data.armorEquip[i] = 0;
                    }
                    Data.armorEquip[0] = 1;
                    Console.WriteLine($"\n▶ 장비가 장착 해제되었습니다!");
                    Thread.Sleep(1000);

                }
                else
                {
                    Console.WriteLine("※ 잘못된 선택이거나 해당 무기를 소지하고 있지 않습니다.");
                    Thread.Sleep(1000);
                }

            }
        }
        public void showInventoryPotion()
        {
            while (true)
            {
                Console.Clear();
                for (int i = 0; i < Data.potion.Length; i++)
                {

                    if (i == 0)
                    {
                        Console.WriteLine("0. 없음");
                    }
                    else
                    {
                        //string potionText = $"{i} {Data.potion[i]} 소지 갯수 :{Data.potionTf[i]} HP회복량 : {Data.potionHpAndMP[i][0]} MP회복량 : {Data.potionHpAndMP[i][1]}";
                        string potionText = $"{i} {Data.potion[i]} 소지 갯수 :{Data.potionTf[i]} HP회복량 : {Data.potionHp[i]} MP회복량 : {Data.potionMp[i]}";
                        Console.WriteLine(potionText);
                    }
                }
                Console.WriteLine("-1.뒤로가기");
                Console.WriteLine("사용하실 포션의 번호를 입력하세요.");
                int userinput = MainFrame.UserInputHandler(-1, Data.potion.Length);
                if (userinput == -1)
                {
                    break;
                }
                else if (userinput == 0)
                {
                    Console.WriteLine("아무일도 없었다.");
                    Thread.Sleep(1000);
                }
                else if (userinput > 0)
                {
                    if (Data.potionTf[userinput] > 0)
                    {
                        Console.WriteLine($"({Data.potion[userinput]})포션을 사용했습니다");
                        Data.potionTf[userinput] -= 1;
                        Thread.Sleep(1000);
                        if (Data.Hp < Data.HpMax)
                        {
                            //Data.Hp += Data.potionHpAndMP[userinput][0];
                            Data.Hp += Data.potionHp[userinput];
                            if (Data.Hp > Data.HpMax)
                            {
                                Data.Hp = Data.HpMax;
                            }
                        }
                        if (Data.Mp < Data.MpMax)
                        {
                            //Data.Mp += Data.potionHpAndMP[userinput][1];
                            Data.Mp += Data.potionMp[userinput];
                            if (Data.Mp > Data.MpMax)
                            {
                                Data.Mp = Data.MpMax;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("포션이 부족합니다");
                        Thread.Sleep(1000);
                    }



                }
            }
        }
    }
}
