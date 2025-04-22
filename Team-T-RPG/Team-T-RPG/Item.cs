using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_T_RPG
{
    class Item
    {
        public bool Store(ref bool StoreError, ref bool MoneyLack)
        {
            Console.Clear();
            Console.WriteLine("상점");
            Console.WriteLine("장비를 구입하거나 판매하는 상점입니다.");
            Console.WriteLine("\n[아이템 목록]");
            printStore();
            Console.WriteLine($"\n\nGold   {Money}");
            Console.WriteLine("\n구입 혹은 판매할 장비를 입력해주세요. (예: W1, A2, R3)");
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
                return true;
            }

            if (text.Length >= 2)
            {
                char type = char.ToUpper(text[0]);
                if (int.TryParse(text.Substring(1), out int number))
                {
                    switch (type)
                    {
                        case 'W': // 무기
                            if (number > 0 && number < weapon.Length)
                            {
                                if (!weaponTf[number])
                                {
                                    if (Money >= weaponDeal[number])
                                    {
                                        weaponTf[number] = !weaponTf[number]; // ! 보유값 반전
                                        Money -= weaponDeal[number]; // 금액차감
                                        return true;
                                    }
                                    else
                                    {
                                        MoneyLack = true;
                                        return false;
                                    }
                                }
                                if (weaponTf[number])
                                {
                                    weaponTf[number] = !weaponTf[number];
                                    Money += weaponDeal[number] / 2;
                                    return true;
                                }
                            }
                            break;

                        case 'A': // 보조 장비
                            if (number > 0 && number < assist.Length)
                            {
                                if (!assistTf[number])
                                {
                                    if (Money >= assistDeal[number])
                                    {
                                        assistTf[number] = !assistTf[number];
                                        Money -= assistDeal[number];
                                        return true;
                                    }
                                    else
                                    {
                                        MoneyLack = true;
                                        return false;
                                    }
                                }
                                if (assistTf[number])
                                {
                                    assistTf[number] = !assistTf[number];
                                    Money += assistDeal[number] / 2;
                                    return true;
                                }
                            }
                            break;

                        case 'R': // 갑옷
                            if (number > 0 && number < armor.Length)
                            {
                                if (!armorTf[number])
                                {
                                    if (Money >= armorDeal[number])
                                    {
                                        armorTf[number] = !armorTf[number];
                                        Money -= armorDeal[number];
                                        return true;
                                    }
                                    else
                                    {
                                        MoneyLack = true;
                                        return false;
                                    }
                                }
                                if (armorTf[number])
                                {
                                    armorTf[number] = !armorTf[number];
                                    Money += armorDeal[number] / 2;
                                    return true;
                                }
                            }
                            break;
                    }
                }
            }
            StoreError = true;
            return false;
        }

        public void printStore()
        {

            // 무기 목록
            Console.WriteLine("\n[무기 목록]");
            for (int i = 1; i < weapon.Length; i++)
            {
                Console.WriteLine($"W{i}.[{(weaponTf[i] ? "소지중" : "없음")}] {weapon[i]} {weaponDeal[i]}Gold");
            }
            // 보조 장비 목록
            Console.WriteLine("\n[보조 장비 목록]");
            for (int i = 1; i < assist.Length; i++)
            {
                Console.WriteLine($"A{i}.[{(assistTf[i] ? "소지중" : "없음")}] {assist[i]} {assistDeal[i]}Gold");
            }
            // 갑옷 목록
            Console.WriteLine("\n[갑옷 목록]");
            for (int i = 1; i < armor.Length; i++)
            {
                Console.WriteLine($"R{i}.[{(armorTf[i] ? "소지중" : "없음")}] {armor[i]} {armorDeal[i]}Gold");
            }
        }

    }
    

}

