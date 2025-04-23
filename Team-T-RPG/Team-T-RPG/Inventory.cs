using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Team_T_RPG
{
    public class Inventory
    {
        public void testInven()
        {
            for (int i = 0; i < Data.weapon.Length; i++)
            {
                if (Data.weaponTf[i] >= 0)
                {
                    string equippedMark = Data.weaponEquip[i] ? " (장착중)" : "";
                    if (i == 0)
                    {
                        // weapon[0]은 장비 없음이므로 갯수 출력 제외
                        Console.WriteLine($"{i}. {Data.weapon[i]}{equippedMark} 공격력: {Data.weaponAtk[i]}  스탯+: {Data.weaponStats[i]}{equippedMark}");
                    }
                    else
                    {
                        // 나머지 무기는 상세 정보 출력
                        Console.WriteLine($"{i}. {Data.weapon[i]} 소지 갯수: {Data.weaponTf[i]}  공격력: {Data.weaponAtk[i]}  스탯+: {Data.weaponStats[i]}{equippedMark}");
                    }
                }
            }
            Console.Write("\n장착할 무기 번호를 입력하세요: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int selectedIndex))
            {
                if (selectedIndex > 0 && selectedIndex < Data.weapon.Length && Data.weaponTf[selectedIndex] > 0)
                {
                    // 기존 무기 해제
                    for (int i = 0; i < Data.weaponEquip.Length; i++)
                    {
                        Data.weaponEquip[i] = false;
                    }

                    // 선택한 무기 장착
                    Data.weaponEquip[selectedIndex] = true;

                    Console.WriteLine($"\n▶ [{Data.weapon[selectedIndex]}]를 장착했습니다!");
                }
                else if (selectedIndex == 0)
                {
                    for (int i = 0; i < Data.weaponEquip.Length; i++)
                    {
                        Data.weaponEquip[i] = false;
                    }
                    Data.weaponEquip[0] = true;
                    Console.WriteLine($"\n▶ [{Data.weapon[selectedIndex]}]을 선택해 장비가 장착 해제되었습니다!");

                }
                else
                {
                    Console.WriteLine("※ 잘못된 선택이거나 해당 무기를 소지하고 있지 않습니다.");
                }
            }
            else
            {
                Console.WriteLine("※ 숫자를 입력해주세요.");
            }
        }
    }
}
