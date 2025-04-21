using System;
using System.Security.Principal;


class Program
{
    // 구현 완료
    // 화면 만들기 - 메인화면
    // 화면 만들기 - 상태보기
    // 화면 만들기 - 인벤토리
    // 화면 만들기 - 상점
    // 화면 만들기 - 상점 [구매] 
    // 화면 만들기 - 인벤토리 [장착관리]

    // 기능1 [All] - 선택한 화면으로 이동하기
    // 기능2 [Stat] - 캐릭터의 정보  표시 (변경되는 정보를 확인) - 레벨 / 이름 / 직업 / 공격력 / 방어력 / 체력 / Gold
    // 기능2_1 [Stat] - 장비 반영에 따른 정보 - 공격력/방어력
    // 기능3 [Inventory] - 보유 아이템 표시 (인벤토리)
    // 기능4 [Inventory] - 장비 장착
    // 기능5 [Shop] - 상점 리스트
    // 기능6 [Shop] - 구매 기능


    // =====================



    // 캐릭터 정보
    // 레벨 / 이름 / 직업 / 공격력 / 방어력 / 체력 / Gold
    // 추가 공격력 / 추가 방어력
    private static int level;
    private static string name;
    private static string job;
    private static int atk;
    private static int def;
    private static int hp;
    private static int gold;

    private static int extraAtk;
    private static int extraDef;


    // 아이템 <- 상점
    // 이름 / 장비타입 / 장비의밸류 / 설명 / 가격

    //                                       0           1           2              3     4            5
    private static string[] itemNames = { "수련자의 갑옷", "무쇠갑옷", "스파르타의 갑옷", "낡은 검", "청동 도끼", "스파르타의 창" };
    private static int[] itemType = { 1, 1, 1, 0, 0, 0 }; // 0: 무기, 1: 방어구
    private static int[] itemValue = { 5, 9, 15, 2, 5, 7 };
    private static string[] itemDesc = { "수련에 도움을 주는 갑옷입니다.", "무쇠로 만들어져 튼튼한 갑옷입니다.", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", "쉽게 볼 수 있는 낡은 검 입니다.", "어디선가 사용됐던거 같은 도끼입니다.", "스파르타의 전사들이 사용했다는 전설의 창입니다." };
    private static int[] itemPrice = { 1000, 2000, 3500, 600, 1500, 2500 };


    // 인벤토리
    private static List<int> inventory = new List<int>();
    private static List<int> equipList = new List<int>();


    static void Main(string[] args)
    {
        SetData();
        DisplayMainUI();
    }

    static void SetData()
    {
        level = 1;
        name = "Chad";
        job = "전사";
        atk = 10;
        def = 5;
        hp = 100;
        gold = 10000;

    }

    static void DisplayMainUI()
    {
        Console.Clear();
        Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
        Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("1. 상태 보기");
        Console.WriteLine("2. 인벤토리");
        Console.WriteLine("3. 상점");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");


        int result = CheckInput(1, 3);

        switch (result)
        {
            case 1:
                DisplayStatUI();
                break;

            case 2:
                DisplayInventoryUI();
                break;

            case 3:
                DisplayShopUI();
                break;
        }
    }

    static void DisplayStatUI()
    {
        Console.Clear();
        Console.WriteLine("상태 보기");
        Console.WriteLine("캐릭터의 정보가 표시됩니다.");

        //                 Lv.1
        Console.WriteLine($"Lv. {level:D2}");
        Console.WriteLine($"{name} {{ {job} }}");
        Console.WriteLine(extraAtk == 0 ? $"공격력 : {atk}" : $"공격력 : {atk + extraAtk} (+{extraAtk})");
        Console.WriteLine(extraDef == 0 ? $"방어력 : {def}" : $"방어력 : {def + extraDef} (+{extraDef})");
        Console.WriteLine($"체력 : {hp}");
        Console.WriteLine($"Gold : {gold} G");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");
        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, 0);

        switch (result)
        {
            case 0:
                DisplayMainUI();
                break;
        }
    }

    static void DisplayInventoryUI()
    {
        Console.Clear();
        Console.WriteLine("인벤토리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < inventory.Count; i++)
        {
            int targetItem = inventory[i];

            string displayEquipped = equipList.Contains(targetItem) ? "[E]" : "";
            Console.WriteLine($"- {displayEquipped} {itemNames[targetItem]}  |  {(itemType[targetItem] == 0 ? "공격력" : "방어력")} +{itemValue[targetItem]}  |  {itemDesc[targetItem]}");
        }
        Console.WriteLine();
        Console.WriteLine("1. 장착 관리");
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, 1);

        switch (result)
        {
            case 0:
                DisplayMainUI();
                break;

            case 1:
                DisplayEquipUI();
                break;
        }
    }

    static void DisplayEquipUI()
    {
        Console.Clear();
        Console.WriteLine("인벤토리 - 장착관리");
        Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < inventory.Count; i++)
        {
            int targetItem = inventory[i];

            string displayEquipped = equipList.Contains(targetItem) ? "[E]" : "";
            Console.WriteLine($"- {i + 1} {displayEquipped} {itemNames[targetItem]}  |  {(itemType[targetItem] == 0 ? "공격력" : "방어력")} +{itemValue[targetItem]}  |  {itemDesc[targetItem]}");
        }
        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, inventory.Count);

        switch (result)
        {
            case 0:
                DisplayInventoryUI();
                break;

            default:

                int targetItem = result - 1;
                bool isEquipped = equipList.Contains(targetItem);

                if (isEquipped)
                {
                    equipList.Remove(targetItem);
                    if (itemType[targetItem] == 0)
                        extraAtk -= itemValue[targetItem];
                    else
                        extraDef -= itemValue[targetItem];
                }
                else
                {
                    equipList.Add(targetItem);
                    if (itemType[targetItem] == 0)
                        extraAtk += itemValue[targetItem];
                    else
                        extraDef += itemValue[targetItem];
                }

                DisplayEquipUI();
                break;
        }
    }

    static void DisplayShopUI()
    {
        Console.Clear();
        Console.WriteLine("상점");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{gold} G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < itemNames.Length; i++)
        {
            Console.WriteLine($"- {itemNames[i]}  |  {(itemType[i] == 0 ? "공격력" : "방어력")} +{itemValue[i]}  |  {itemDesc[i]}  |  {(inventory.Contains(i) ? "구매완료" : $"{itemPrice[i]} G")}");
        }

        Console.WriteLine();
        Console.WriteLine("1. 아이템 구매");
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, 1);

        switch (result)
        {
            case 0:
                DisplayMainUI();
                break;

            case 1:
                DisplayBuyUI();
                break;
        }
    }

    static void DisplayBuyUI()
    {
        Console.Clear();
        Console.WriteLine("상점 - 아이템 구매");
        Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
        Console.WriteLine();
        Console.WriteLine("[보유 골드]");
        Console.WriteLine($"{gold} G");
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");

        for (int i = 0; i < itemNames.Length; i++)
        {
            Console.WriteLine($"- {i + 1} {itemNames[i]}  |  {(itemType[i] == 0 ? "공격력" : "방어력")} +{itemValue[i]}  |  {itemDesc[i]}  |  {(inventory.Contains(i) ? "구매완료" : $"{itemPrice[i]} G")}");
        }

        Console.WriteLine();
        Console.WriteLine("0. 나가기");
        Console.WriteLine();
        Console.WriteLine("원하시는 행동을 입력해주세요.");

        int result = CheckInput(0, itemNames.Length);

        switch (result)
        {
            case 0:
                DisplayShopUI();
                break;

            default:
                int targetItem = result - 1;
                // 이미 구매한 아이템이라면?
                if (inventory.Contains(targetItem))
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                    Console.WriteLine("Enter 를 눌러주세요.");
                    Console.ReadLine();
                }
                else // 구매가 가능할떄
                {
                    //   소지금이 충분하다
                    if (gold >= itemPrice[targetItem])
                    {
                        Console.WriteLine("구매를 완료했습니다.");
                        gold -= itemPrice[targetItem];
                        inventory.Add(targetItem);
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                        Console.WriteLine("Enter 를 눌러주세요.");
                        Console.ReadLine();
                    }

                    //   소지금이 부족핟
                }

                DisplayBuyUI();
                break;
        }
    }

    static int CheckInput(int min, int max)
    {
        int result;
        while (true)
        {
            string input = Console.ReadLine();
            bool isNumber = int.TryParse(input, out result);
            if (isNumber)
            {
                if (result >= min && result <= max)
                    return result;
            }
            Console.WriteLine("잘못된 입력입니다!!!!");
        }
    }
}