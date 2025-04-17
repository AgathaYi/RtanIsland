using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Xml.Linq;

namespace RtanIsland
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 인트로
            Console.WriteLine("텍스트 RPG (스파르타 던전) 개인과제 20250418 제출예정");
            Console.ReadLine();

            // 게임입장
            var game = new GameLogic();
            game.Run();
        }


        // ** 실행 제어
        class GameLogic
        {
            private Player _player;
            private Inventory _inventory = new Inventory();
            private Shop _shop;

            public void Run()
            {
                SelectInfo();
                while (true)
                {
                    Menu();
                }
            }

            // a. 정보 입력
            public void SelectInfo()
            {
                string name = GetName();
                string job = ChooseJob().ToString();

                // 초기 보상: 이름, 레벨, *직업, 공격력, 방어력, 체력, 골드
                _player = new Player(name, 1, (Job)Enum.Parse(typeof(Job), job), 10, 5, 100, 1000); // enum으로 변환
                _shop = new Shop(_player, _inventory);

                // 플레이어 생성
                Console.Clear();
                Console.WriteLine($"스파르타 마을에 오신 {job} {name}님, 환영합니다.\r\n던전으로 들어가기전 활동을 할 수 있습니다.");
                Menu();
            }

            // a-1. 이름 입력 별도 메서드
            private string GetName()
            {
                string name;

                do
                {
                    Console.Clear();
                    Console.Write("이름을 입력하세요 \n이름: ");
                    name = Console.ReadLine() ?? ""; // null 체크
                }
                while (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(name));

                return name;
            }

            // a-2. 직업 선택 별도 메서드

            private Job ChooseJob()
            {
                int _job = 0;

                while (true)
                {
                    Console.Clear();
                    Console.Write("직업을 선택하세요.\n1. 전사 \n2. 법사 \n3. 궁수\n>>");
                    string input = Console.ReadLine();
                    int.TryParse(input, out _job);

                    if (_job >= 1 && _job <= 3)
                    {
                        break;
                    }
                    //else if (string.IsNullOrWhiteSpace(input) || string.IsNullOrEmpty(input))
                    //{
                    //    continue;
                    //}
                    else
                    {
                        continue;
                    }
                }
                return (Job)_job; // enum
            }

            // b. 메뉴
            public void Menu()
            {
                Console.Clear();
                Console.WriteLine("■ 메인메뉴");
                Console.WriteLine("1. 상태보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");

                Console.Write("\n0. 나가기\n\n>>");

                string inputMenu = Console.ReadLine();
                bool isMenu = int.TryParse(inputMenu, out int _menu);

                // 1. 상태보기, 2. 인벤토리, 3. 상점, 0. 나가기, 그외 잘못된 입력시 while문 반복
                switch (inputMenu)
                {
                    case "1":
                        _player.StateUI();
                        break;
                    case "2":
                        _inventory.invUI(_player);
                        break;
                    case "3":
                        _shop.ShopUI(_player, _inventory);
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }

            // A.직업 enum
            public enum Job
            {
                전사 = 1,
                법사,
                궁수
            }


            // B-1. 상태보기
            class Player
            {
                // 생성자
                private Inventory _inv;
                private Shop _shop;

                public string Name { get; set; }
                public int Level { get; set; }
                public Job Job { get; set; }
                public int Attack { get; set; }
                public int Defense { get; set; }
                public int Health { get; set; }
                public int Gold { get; set; }
                public Player(string name, int level, Job job, int attack, int defense, int health, int gold)
                {
                    Name = name;
                    Level = level;
                    Job = job;
                    Attack = attack;
                    Defense = defense;
                    Health = health;
                    Gold = gold;
                }

                //상태보기 출력
                public void StateUI()
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"<상태보기>\n{Name}님의 캐릭터 정보가 표시됩니다." +
                        $"\nLv. {Level}" +
                        $"\n직업: {Job}" +
                        $"\n공격력: {Attack}" +
                        $"\n방어력: {Defense}" +
                        $"\n체력: {Health}/100" +
                        $"\nGold: {Gold}G");

                        Console.Write("\n0. 나가기\n\n>>");

                        if (Console.ReadLine() == "0")
                        {
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }


            // M-2. 인벤토리
            class Inventory
            {
                private List<Item> _items = new List<Item>();

                public void Add(Item item) => _items.Add(item);

                // 인벤토리 출력
                public void invUI(Player player)
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("<인벤토리>");
                        Console.WriteLine($"[보유 골드]{player.Gold}G\n");

                        if (_items.Count == 0)
                        {
                            Console.WriteLine("인벤토리에 아이템이 없습니다.");
                            Console.WriteLine("0. 나가기\n>>");
                            if (Console.ReadLine() == "0")
                            {
                                return;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("[아이템 목록]");
                            for (int i = 0; i < _items.Count; i++)
                            {
                                var item = _items[i];
                                string equippedTag = item.IsEquipped ? "[E] " : "";
                                Console.WriteLine($"{i + 1}. {equippedTag}{item.ItemName} | 공격력 +{item.Attack} | 방어력 +{item.Defense} | 체력 +{item.Health} | 가격: {item.Price}G");
                            }

                            Console.Write("\n1. 장착 관리\n0. 나가기\n>>");
                            var cmd = Console.ReadLine();
                            if (cmd == "1")
                            {
                                EquipUI(player);
                            }
                            else if (cmd == "0")
                            {
                                return;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }

                // 장착 관리
                public void EquipUI(Player player)
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("<장착 관리>");
                        for (int i = 0; i < _items.Count; i++)
                        {
                            var item = _items[i];
                            string equippedTag = item.IsEquipped ? "[E] " : "";
                            Console.WriteLine($"{i + 1}. {equippedTag}{item.ItemName} | 공격력 +{item.Attack} | 방어력 +{item.Defense} | 체력 +{item.Health} | 가격: {item.Price}G");
                        }
                        Console.Write("\n0. 나가기\n>>");

                        var line = Console.ReadLine();
                        if (line == "0")
                        {
                            return;
                        }
                        else
                        {
                            int index;
                            if (int.TryParse(line, out index) && index > 0 && index <= _items.Count)
                            {
                                var item = _items[index - 1];
                                item.IsEquipped = !item.IsEquipped; // 장착 상태 토글
                                Console.WriteLine($"{item.ItemName}이(가) {(item.IsEquipped ? "장착되었습니다." : "해제되었습니다.")}");
                                Console.ReadLine();
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }

            // M-3. 상점
            class Shop
            {
                // 상점 생성자
                private Player _player;
                private Inventory _inv;
                private List<Item> _tem;

                public Shop(Player player, Inventory inv)
                {
                    _player = player;
                    _inv = inv;
                    _tem = new List<Item>()
                    {
                        //이름, 공격력, 방어력, 체력, 가격
                        new Item(ItemList.무쇠갑옷, "0", 5, 0, 100),
                        new Item(ItemList.깃털화살, "10", 0, 0, 200),
                        new Item(ItemList.낡은검, "5", 0, 0, 300),
                        new Item(ItemList.HP포션, "0", 0, 5, 50)
                    };
                }


                //상점 출력
                public void ShopUI(Player player, Inventory inv)
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("<상점> - 아이템 구매\n필요한 아이템을 얻을 수 있는 상점입니다.");
                        Console.WriteLine($"\n[보유골드]\n{player.Gold}G");
                        Console.WriteLine("\n[아이템 목록]");

                        for (int i = 0; i < _tem.Count; i++)
                        {
                            var item = _tem[i];
                            string itStatus = item.IsPurchased ? "구매완료" : $"{item.Price} G"; // 구매 여부
                            Console.WriteLine($"{i + 1}. {item.ItemName} | 공격력 +{item.Attack} | 방어력 +{item.Defense} | 체력 +{item.Health} | 가격: {item.Price}G");
                        }

                        Console.WriteLine("\n1. 구매하기");
                        Console.Write("\n0. 나가기\n\n>>");

                        var input = Console.ReadLine();
                        if (input == "0")
                        {
                            return;
                        }
                        else if (input == "1")
                        {
                            Console.Write("구매할 아이템 번호를 입력하세요: ");
                            var itemInput = Console.ReadLine();
                            int itemIndex;
                            if (int.TryParse(itemInput, out itemIndex) && itemIndex > 0 && itemIndex <= _tem.Count)
                            {
                                var item = _tem[itemIndex - 1];
                                if (item.IsPurchased)
                                {
                                    Console.WriteLine("이미 구매한 아이템입니다.");
                                    Console.ReadLine();
                                }
                                else if (player.Gold >= item.Price)
                                {
                                    player.Gold -= item.Price;
                                    item.IsPurchased = true;
                                    inv.Add(item);
                                    Console.WriteLine($"{item.ItemName}을(를) 구매했습니다.");
                                    Console.ReadLine();
                                }
                                else
                                {
                                    Console.WriteLine("Gold가 부족합니다.");
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
            }

            //아이템 목록
            public enum ItemList
            {
                무쇠갑옷 = 1,
                깃털화살,
                낡은검,
                HP포션
            }

            public class Item
            {
                public ItemList ItemName { get; set; }
                public string Description { get; set; }
                public int Attack { get; set; }
                public int Defense { get; set; }
                public int Health { get; set; }
                public int Price { get; set; }

                public bool IsEquipped { get; set; } // 장착 여부
                public bool IsPurchased { get; set; } // 구매 여부

                public Item(ItemList item, string desc, int atk, int def, int price)
                {
                    ItemName = item;
                    Description = desc;
                    Attack = atk;
                    Defense = def;
                    Price = price;

                    IsEquipped = false;
                    IsPurchased = false;
                }
            }
        }
    }
}