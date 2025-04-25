using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_T_RPG
{
    class Item
    {
        public void Store()
        {
            while (true) // 상점 루프 시작
            {
                bool StoreError = false;
                bool MoneyLack = false;
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("장비를 구입하거나 판매하는 상점입니다.");
                Console.WriteLine("\n[아이템 목록]");
                printStore();
                Console.WriteLine($"\n\nGold   {Data.Money}");
                Console.WriteLine("\n구입 혹은 판매할 장비를 입력해주세요. (예: W1, A2, R3, P4)");
                Console.WriteLine("판매 시 가격의 절반을 받습니다.");
                Console.WriteLine("\n\n0.나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                if (MoneyLack)
                {
                    Console.WriteLine("돈이 부족합니다.\n");
                    MoneyLack = false;
                }
                else if (StoreError)
                {
                    Console.WriteLine("잘못된 입력입니다.\n");
                    StoreError = false;
                }

                string text = Console.ReadLine();
                if (text == "0")
                {
                    Console.Clear();
                    return;
                }

                if (text.Length >= 2)
                {
                    char type = char.ToUpper(text[0]);
                    if (int.TryParse(text.Substring(1), out int number))
                    {
                        switch (type)
                        {
                            case 'W': // 무기
                                if (number > 0 && number < Data.weapon.Length)
                                {
                                    string itemName = Data.weapon[number];
                                    if (Data.weaponTf[number] == 0)
                                    {
                                        if (Data.Money >= Data.weaponDeal[number])
                                        {
                                            Data.weaponTf[number] = 1;
                                            Data.Money -= Data.weaponDeal[number];
                                            Console.WriteLine($"\"{itemName}\" 구매 완료!");
                                        }
                                        else
                                        {
                                            MoneyLack = true;
                                            Console.WriteLine("돈이 부족합니다.");
                                        }
                                    }
                                    else if (Data.weaponTf[number] == 1)
                                    {
                                        Data.weaponTf[number] = 0;
                                        Data.Money += Data.weaponDeal[number] / 2;
                                        Console.WriteLine($"\"{itemName}\"판매 완료!");
                                    }
                                }
                                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                                Console.ReadKey();
                                continue;

                            case 'A': // 보조 장비
                                if (number > 0 && number < Data.assist.Length)
                                {
                                    string itemName = Data.assist[number];
                                    if (Data.assistTf[number] == 0)
                                    {
                                        if (Data.Money >= Data.assistDeal[number])
                                        {
                                            Data.assistTf[number] = 1;
                                            Data.Money -= Data.assistDeal[number];
                                            Console.WriteLine($"\"{itemName}\"구매 완료!");
                                        }
                                        else
                                        {
                                            MoneyLack = true;
                                            Console.WriteLine("돈이 부족합니다.");
                                        }
                                    }
                                    else if (Data.assistTf[number] == 1)
                                    {
                                        Data.assistTf[number] = 0;
                                        Data.Money += Data.assistDeal[number] / 2;
                                        Console.WriteLine($"\"{itemName}\"판매 완료!");
                                    }
                                }
                                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                                Console.ReadKey();
                                continue;

                            case 'R': // 갑옷
                                if (number > 0 && number < Data.armor.Length)
                                {
                                    string itemName = Data.armor[number];
                                    if (Data.armorTf[number] == 0)
                                    {
                                        if (Data.Money >= Data.armorDeal[number])
                                        {
                                            Data.armorTf[number] = 1;
                                            Data.Money -= Data.armorDeal[number];
                                            Console.WriteLine($"\"{itemName}\"구매 완료!");
                                        }
                                        else
                                        {
                                            MoneyLack = true;
                                            Console.WriteLine("돈이 부족합니다.");
                                        }
                                    }
                                    else if (Data.armorTf[number] == 1)
                                    {
                                        Data.armorTf[number] = 0;
                                        Data.Money += Data.armorDeal[number] / 2;
                                        Console.WriteLine($"\"{itemName}\"판매 완료!");
                                    }
                                }
                                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                                Console.ReadKey();
                                continue;

                            case 'P': // 포션
                                {
                                    if (number > 0 && number < Data.potion.Length)
                                    {
                                        string itemName = Data.potion[number];
                                        if (Data.potionTf[number] < 5)
                                        {
                                            if (Data.Money >= Data.potionDeal[number])
                                            {
                                                Data.potionTf[number]++;
                                                Data.Money -= Data.potionDeal[number];

                                                int healAmount = Data.potionHp[number];
                                                Data.Hp = Math.Min(Data.Hp + healAmount, Data.HpMax);
                                                int manaAmount = Data.potionMp[number];
                                                Data.Mp = Math.Min(Data.Mp + healAmount, Data.MpMax);

                                                Console.WriteLine($"\"{itemName}\"구매 완료!(HP +{healAmount})");
                                                Console.WriteLine($"현재 소지 수: {Data.potionTf[number]} / 5");
                                            }
                                            else
                                            {
                                                MoneyLack = true;
                                                Console.WriteLine("돈이 부족합니다.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("포션은 최대 5개까지만 소지할 수 있습니다");
                                        }

                                    }
                                    Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                                    Console.ReadKey();
                                    continue;
                                }
                        }
                        StoreError = true;
                        continue;
                    }
                }
            }
        }


        public void printStore()
        {

            // 무기 목록
            Console.WriteLine("\n[무기 목록]");
            for (int i = 1; i < Data.weapon.Length; i++)
            {
                Console.WriteLine($"W{i}.[ {((Data.weaponTf[i] == 1) ? "소지중" : "없음")}] {Data.weapon[i]} {Data.weaponDeal[i]}Gold");
            }
            // 보조 장비 목록
            Console.WriteLine("\n[보조 장비 목록]");
            for (int i = 1; i < Data.assist.Length; i++)
            {
                Console.WriteLine($"A{i}.[  {((Data.assistTf[i] == 1) ? "소지중" : "없음")}] {Data.assist[i]} {Data.assistDeal[i]}Gold");
            }
            // 갑옷 목록
            Console.WriteLine("\n[갑옷 목록]");
            for (int i = 1; i < Data.armor.Length; i++)
            {
                Console.WriteLine($"R{i}.[ {((Data.armorTf[i] == 1) ? "소지중" : "없음")}] {Data.armor[i]} {Data.armorDeal[i]}Gold");
            }
            Console.WriteLine("\n[포션 목록]");
            for (int i = 1; i < Data.potion.Length; i++)
            {
                Console.WriteLine($"P{i}.[ {((Data.potionTf[i] == 1) ? "소지중" : "없음")}] {Data.potion[i]} {Data.potionDeal[i]}Gold");
            }
        }

    }


}
