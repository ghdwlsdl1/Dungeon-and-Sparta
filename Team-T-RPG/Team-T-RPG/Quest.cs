using System;
using System.Collections.Generic;

namespace Team_T_RPG
{
    public enum QuestState
    {
        NotAccepted,    // 수락 전
        InProgress,     // 진행 중
        Completed,      // 완료 가능
        Rewarded        // 완료됨
    }

    // 공통 인터페이스
    public interface IQuestGoal
    {
        string Description { get; }     // 퀘스트 목표 설명
        string ProgressText { get; }    // 진행 상태 텍스트
        bool IsComplete { get; }        // 완료 여부
        void UpdateProgress(params object[] args);  // 진행 상태 업데이트
    }

    // 몬스터 처치
    public class KillGoal : IQuestGoal
    {
        public string TargetMob { get; set; }   // 목표 몬스터 이름
        public int Required { get; set; }       // 필요 처치 수
        public int Current { get; set; }        // 현재 처치 수

        public string Description => $"{TargetMob} {Required}마리 처치";
        public string ProgressText => $"({Current}/{Required})";
        public bool IsComplete => Current >= Required;

        public void UpdateProgress(params object[] args)
        {
            string mobName = args[0] as string;
            if (mobName == TargetMob)
                Current++;
        }
    }

    // 아이템 납품
    public class DeliverGoal : IQuestGoal
    {
        public string ItemName { get; set; }
        public int Required { get; set; }
        public int Delivered { get; set; }

        public string Description => $"{ItemName} {Required}개 납품";
        public string ProgressText => $"({Delivered}/{Required})";
        public bool IsComplete => Delivered >= Required;

        public void UpdateProgress(params object[] args)
        {
            string item = args[0] as string;
            int count = (int)args[1];

            //if (item == ItemName && Item.Has(item, count))
            //{
            //    Item.Remove(item, count);
            //    Delivered += count;
            //}
            // ★ 만약 플레이어가 특정 아이템(ItemName)을 몇개 만큼(Item.Has(특정아이템, 개수))보유하고 있다면, 인벤토리에서 퀘스트 개수만큼 제거하는 로직.
            // ★ 아이템 보유를 확인하는 함수, 아이템을 제거하는 함수가 필요.

        }
    }

    // 특정 장소 도달
    public class ReachFloorGoal : IQuestGoal
    {
        public int RequiredFloor { get; set; }
        public bool Reached { get; set; }

        public string Description => $"던전 {RequiredFloor}층 도달";
        public string ProgressText => Reached ? "(도달 완료)" : "(미도달)";
        public bool IsComplete => Reached;

        public void UpdateProgress(params object[] args)
        {
            if (args.Length > 0 && args[0] is int currentFloor)
            {
                if (currentFloor >= RequiredFloor)
                    Reached = true;
            }
        }
    }

    // 퀘스트 본체
    public class Quest
    {
        public string Title { get; set; }       // 퀘스트 제목
        public string Description { get; set; } // 퀘스트 설명
        public IQuestGoal Goal { get; set; }     // 퀘스트 목표
        public string RewardItem { get; set; }  // 보상 아이템
        public int RewardGold { get; set; }     // 보상 골드
        public QuestState State { get; set; }   // 퀘스트 상태

        public bool IsComplete() => Goal.IsComplete;    // 퀘스트 완료 여부 체크
    }

   
    public static class QuestManager
    {
        public static List<Quest> Quests = new List<Quest>();

        // 퀘스트 초기 등록
        public static void Initialize()
        {
            Quests.Add(new Quest
            {
                Title = "고블린 처치",
                Description = "던전에 있는 고블린을 처치하자!",
                Goal = new KillGoal { TargetMob = "고블린", Required = 3 },
                // ★ RewardItem = "박쥐모피",
                RewardGold = 10,
                State = QuestState.NotAccepted
            });

            Quests.Add(new Quest
            {
                Title = "포션 납품",
                Description = "포션 3개를 마을에 납품하자!",
                Goal = new DeliverGoal { ItemName = "포션", Required = 3 },
                // ★ RewardItem = "감사의 편지",
                RewardGold = 5,
                State = QuestState.NotAccepted
            });

            Quests.Add(new Quest
            {
                Title = "던전 3층 도달",
                Description = "던전 3층까지 가보자!",
                Goal = new ReachFloorGoal { RequiredFloor = 3 },
                // ★ RewardItem = "탐험가의 반지",
                RewardGold = 20,
                State = QuestState.NotAccepted
            });
        }

        // 퀘스트 목록 출력
        public static void ShowQuestList()
        {
            Console.Clear();
            Console.WriteLine("퀘스트 목록\n");

            for (int i = 0; i < Quests.Count; i++)
            {
                var q = Quests[i];
                string status = q.State switch
                {
                    QuestState.NotAccepted => "(수락 가능)",
                    QuestState.InProgress => "(진행중)",
                    QuestState.Completed => "(완료 가능)",
                    QuestState.Rewarded => "(완료됨)",
                    _ => ""
                };
                Console.WriteLine($"{i + 1}. {q.Title} {status}");
            }
            Console.WriteLine($"\n{Quests.Count + 1}. 마을로 돌아가기");

            Console.WriteLine("\n퀘스트를 선택하세요.\n");
            int userinput = MainFrame.UserInputHandler(1, Quests.Count+1);

            if (userinput == Quests.Count + 1)
            {
                Console.Clear();
                MainFrame.TownScene();
            }
            else
            {
                ShowQuestDetail(userinput - 1);
            }
        }

        // 퀘스트 상세 정보 출력 및 처리
        public static void ShowQuestDetail(int index)
        {
            var q = Quests[index];
            Console.Clear();
            Console.WriteLine("퀘스트 상세\n");
            Console.WriteLine($"제목: {q.Title}");
            Console.WriteLine($"설명: {q.Description}");
            Console.WriteLine($"목표: {q.Goal.Description} {q.Goal.ProgressText}");
            Console.WriteLine($"\n보상: {q.RewardItem}, {q.RewardGold}G");

            switch (q.State)
            {
                case QuestState.NotAccepted:
                    Console.WriteLine("\n1. 수락\n2. 돌아가기");
                    break;
                case QuestState.InProgress:
                    Console.WriteLine("\n(진행 중)\n2. 돌아가기");
                    break;
                case QuestState.Completed:
                    Console.WriteLine("\n1. 보상 받기\n2. 돌아가기");
                    break;
                case QuestState.Rewarded:
                    Console.WriteLine("\n(완료된 퀘스트입니다)\n2. 돌아가기");
                    break;
            }

            Console.Write("\n>> ");
            int userinput = MainFrame.UserInputHandler(1, 2);
            if (q.State == QuestState.NotAccepted && userinput == 1)
            {
                q.State = QuestState.InProgress;
                Console.Clear();
                ShowQuestList();
            }
            else if (q.State == QuestState.Completed && userinput == 1)
            {
                GrantReward(q);
                Console.Clear();
                ShowQuestList();
            }
            else if (userinput == 2)
            {
                Console.Clear();
                ShowQuestList();
            }
        }

        // 보상 지급
        public static void GrantReward(Quest q)
        {
            if (!string.IsNullOrEmpty(q.RewardItem))
            {
                // Item reward = ItemDatabase.Items.Find(item => item.Name == q.RewardItem);
                // if (reward != null)
                //    Inventory.Add(reward);
                //    ★ 아이템이 존재하면 인벤토리에 추가. 사용자 정의 함수 이므로 별도의 구현 필요
                //    ★ Find()는 못 찾으면 null 반환하기에 null 체크 필요
            }

            Data.Money += q.RewardGold;
            q.State = QuestState.Rewarded;

            Console.WriteLine($"\n보상을 받았습니다! {q.RewardItem} + {q.RewardGold}G");
            Console.ReadKey();
            /*
            Item.Add(q.RewardItem);
            */
        }

        // 퀘스트 조건 체크용 함수들
        public static void ReportKill(string mobName)
        {
            foreach (var q in Quests)
            {
                if (q.State == QuestState.InProgress)
                {
                    q.Goal.UpdateProgress(mobName);
                    if (q.IsComplete()) q.State = QuestState.Completed;
                }
            }
        }
        // ★ QuestManager.ReportKill(Name); 를 토대로 몬스터 처치 시 호출하면 됨.
        //  하지만, 몬스터가 많아지면 이걸 다 체크하는 게 비효율적일 수 있음.
        //  이 경우 당장 생각나는 해결법은 첫째, 몬스터 클래스에 퀘스트를 체크하는 함수를 넣는 것.
        //  몬스터가 Die() 상태가 되면, QuestManager.ReportKill(Name)를 호출하는 것.
        //  둘째, 몬스터 죽을 때 퀘스트를 체크하는 함수를 호출하는 게 아니라, 퀘스트에서 몬스터를 체크하는 함수를 호출하는 것.
        //  만약 몬스터가 죽는 걸로 퀘스트뿐만 아니라 업적, 로그 등도 체크한다면 코드가 길어지게 되니 이벤트 시스템을 도입하는 것도 좋음.
        //  옵저버 패턴의 이용

        public static void ReportDelivery(string itemName, int count)
        {
            foreach (var q in Quests)
            {
                if (q.State == QuestState.InProgress)
                {
                    q.Goal.UpdateProgress(itemName, count);
                    if (q.IsComplete()) q.State = QuestState.Completed;
                }
            }
        }

        public static void ReportFloorReached(int floor)
        {
            foreach (var q in Quests)
            {
                if (q.State == QuestState.InProgress)
                {
                    q.Goal.UpdateProgress(floor);
                    if (q.IsComplete()) q.State = QuestState.Completed;
                }
            }
        }
    }
}
