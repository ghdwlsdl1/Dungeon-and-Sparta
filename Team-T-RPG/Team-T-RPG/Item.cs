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
                Console.Clear();
                Art.MakeImage("Image/store.png", width: 60);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[상점]");
                Console.ResetColor();
                Console.WriteLine("장비를 구입하거나 판매하는 상점입니다.\n");

                Console.WriteLine("카테고리를 선택해주세요:");
                Console.WriteLine("1. 무기");
                Console.WriteLine("2. 보조 장비");
                Console.WriteLine("3. 갑옷");
                Console.WriteLine("4. 포션");
                Console.WriteLine("0. 나가기");
                Console.WriteLine($"\nGold: {Data.Money}");

                Console.Write("\n입력: ");
                string categoryInput = Console.ReadLine();

                if (categoryInput == "0")
                {
                    Console.Clear();
                    return;
                }

                switch (categoryInput)
                {
                    case "1":
                        WeaponMenu();
                        break;
                    case "2":
                        AssistMenu();
                        break;
                    case "3":
                        ArmorMenu();
                        break;
                    case "4":
                        PotionMenu();
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey();
                        break;
                }
            }
        }
        void WeaponMenu()
        {
            while (true)
            {
                Console.Clear();
                Art.MakeImage("Image/store.png", width: 60);
                Console.WriteLine("[무기 목록]\n");
                for (int i = 1; i < Data.weapon.Length; i++)
                {
                    int[] stats = Data.weaponStats[i];
                    string statText = $"(힘 +{stats[0]} / 민첩 +{stats[1]} / 지능 +{stats[2]})";
                    string ownText = Data.weaponTf[i] == 1 ? "(보유 중)" : "";
                    Console.WriteLine($"{i}. {Data.weapon[i]} (공격력 +{Data.weaponAtk[i]}) - {statText} {ownText} - {Data.weaponDeal[i]} Gold");
                }
                Console.WriteLine("0. 뒤로가기");
                Console.WriteLine($"\nGold: {Data.Money}");

                Console.Write("\n구매/판매할 무기 번호를 입력하세요: ");
                string input = Console.ReadLine();
                if (input == "0") break;

                if (int.TryParse(input, out int number) && number > 0 && number < Data.weapon.Length)
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
                            Console.WriteLine("돈이 부족합니다.");
                        }
                    }
                    else
                    {
                        Data.weaponTf[number] = 0;
                        Data.Money += Data.weaponDeal[number] / 2;
                        Console.WriteLine($"\"{itemName}\" 판매 완료!");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }

        void AssistMenu()
        {
            while (true)
            {
                Console.Clear();
                Art.MakeImage("Image/store.png", width: 60);
                Console.WriteLine("[보조 장비 목록]\n");
                for (int i = 1; i < Data.assist.Length; i++)
                {
                    int[] stats = Data.assistStats[i];
                    string statText = $"(힘 +{stats[0]} / 민첩 +{stats[1]} / 지능 +{stats[2]})";
                    string ownText = Data.assistTf[i] == 1 ? "(보유 중)" : "";
                    Console.WriteLine($"{i}. {Data.assist[i]} (방어력 +{Data.assistDef[i]}) - {statText} {ownText} - {Data.assistDeal[i]} Gold");
                }
                Console.WriteLine("0. 뒤로가기");
                Console.WriteLine($"\nGold: {Data.Money}");

                Console.Write("\n구매/판매할 보조 장비 번호를 입력하세요: ");
                string input = Console.ReadLine();
                if (input == "0") break;

                if (int.TryParse(input, out int number) && number > 0 && number < Data.assist.Length)
                {
                    string itemName = Data.assist[number];
                    if (Data.assistTf[number] == 0)
                    {
                        if (Data.Money >= Data.assistDeal[number])
                        {
                            Data.assistTf[number] = 1;
                            Data.Money -= Data.assistDeal[number];
                            Console.WriteLine($"\"{itemName}\" 구매 완료!");
                        }
                        else
                        {
                            Console.WriteLine("돈이 부족합니다.");
                        }
                    }
                    else
                    {
                        Data.assistTf[number] = 0;
                        Data.Money += Data.assistDeal[number] / 2;
                        Console.WriteLine($"\"{itemName}\" 판매 완료!");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }

        void ArmorMenu()
        {
            while (true)
            {
                Console.Clear();
                Art.MakeImage("Image/store.png", width: 60);
                Console.WriteLine("[갑옷 목록]\n");
                for (int i = 1; i < Data.armor.Length; i++)
                {
                    Console.WriteLine($"{i}. {Data.armor[i]} (방어력 +{Data.armorDef[i]}) {(Data.armorTf[i] == 1 ? "(보유 중)" : "")} - {Data.armorDeal[i]} Gold");
                }
                Console.WriteLine("0. 뒤로가기");
                Console.WriteLine($"\nGold: {Data.Money}");

                Console.Write("\n구매/판매할 갑옷 번호를 입력하세요: ");
                string input = Console.ReadLine();
                if (input == "0") break;

                if (int.TryParse(input, out int number) && number > 0 && number < Data.armor.Length)
                {
                    string itemName = Data.armor[number];
                    if (Data.armorTf[number] == 0)
                    {
                        if (Data.Money >= Data.armorDeal[number])
                        {
                            Data.armorTf[number] = 1;
                            Data.Money -= Data.armorDeal[number];
                            Console.WriteLine($"\"{itemName}\" 구매 완료!");
                        }
                        else
                        {
                            Console.WriteLine("돈이 부족합니다.");
                        }
                    }
                    else
                    {
                        Data.armorTf[number] = 0;
                        Data.Money += Data.armorDeal[number] / 2;
                        Console.WriteLine($"\"{itemName}\" 판매 완료!");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }

        void PotionMenu()
        {
            while (true)
            {
                Console.Clear();
                Art.MakeImage("Image/store.png", width: 60);
                Console.WriteLine("[포션 목록]");
                for (int i = 1; i < Data.potion.Length; i++)
                {
                    Console.WriteLine($"{i}. {Data.potion[i]} (HP +{Data.potionHp[i]}, MP +{Data.potionMp[i]}) - {Data.potionDeal[i]} Gold (소지: {Data.potionTf[i]}/5)");
                }
                Console.WriteLine("0. 뒤로가기");
                Console.WriteLine($"\nGold: {Data.Money}");

                Console.Write("\n구매할 포션 번호를 입력하세요: ");
                string input = Console.ReadLine();
                if (input == "0") break;

                if (int.TryParse(input, out int number) && number > 0 && number < Data.potion.Length)
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
                            Data.Mp = Math.Min(Data.Mp + manaAmount, Data.MpMax);

                            Console.WriteLine($"\"{itemName}\" 구매 완료! (HP +{healAmount})");
                        }
                        else
                        {
                            Console.WriteLine("돈이 부족합니다.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("포션은 최대 5개까지만 소지할 수 있습니다.");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }


        public void printStore()
        {

            // 무기 목록
            Console.WriteLine("\n[무기 목록]");
            for (int i = 1; i < Data.weapon.Length; i++)
            {
                Console.WriteLine($"W{i}.[x{Data.weaponTf[i]}] {Data.weapon[i]} {Data.weaponDeal[i]}Gold");
            }
            // 보조 장비 목록
            Console.WriteLine("\n[보조 장비 목록]");
            for (int i = 1; i < Data.assist.Length; i++)
            {
                Console.WriteLine($"A{i}.[x{Data.assistTf[i]}] {Data.assist[i]} {Data.assistDeal[i]}Gold");
            }
            // 갑옷 목록
            Console.WriteLine("\n[갑옷 목록]");
            for (int i = 1; i < Data.armor.Length; i++)
            {
                Console.WriteLine($"R{i}.[x{Data.armorTf[i]}] {Data.armor[i]} {Data.armorDeal[i]}Gold");
            }
            Console.WriteLine("\n[포션 목록]");
            for (int i = 1; i < Data.potion.Length; i++)
            {
                Console.WriteLine($"P{i}.[x{Data.potionTf[i]}] {Data.potion[i]} {Data.potionDeal[i]}Gold");
            }
        }

    }


}
