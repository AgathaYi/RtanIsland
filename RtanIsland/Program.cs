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
            Console.WriteLine("▶ 르탄 아일랜드 ◀");
            Console.WriteLine("르탄 아일랜드의 바닷가에서" +
                "\n알 수 없는 괴생명체들이 출몰하고 있어요!" +
                "\n우리 르탄 아일랜드를 지켜주세요! \n\n[Enter]");
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
                Console.WriteLine($"{name}님은 {job}이 되었습니다." +
                    "르탄 아일랜드에 입장합니다!" +
                    "\n\n[Enter] 입장하기");
                Console.ReadLine();

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
                    Console.Write("직업을 선택하세요.\n1. 잠수꾼 \n2. 뱃사공 \n3. 사냥꾼\n>>");
                    string input = Console.ReadLine();
                    int.TryParse(input, out _job);

                    if (_job >= 1 && _job <= 3)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(500);
                        continue;
                    }
                }
                return (Job)_job; // enum
            }

            // b. 메뉴
            public void Menu()
            {
                Console.Clear();
                Console.WriteLine("■ 르탄 아일랜드 _ 메인 육지 ■");
                Console.WriteLine("1. 상태보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                //Console.WriteLine("4. 바다 던전 입장하기");
                Console.WriteLine("5. 휴식하기"); // 500G 내면 체력 100 회복

                Console.Write("\n0. 게임종료\n\n>>");

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
                    //case "4":
                    //    Ocean ocean = new Ocean();
                    case "5":
                        Rest rest = new Rest();
                        rest.RestUI(_player);
                        break;
                    case "0":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(500);
                        break;
                }
            }
        }


        // A.직업 enum
        public enum Job
        {
            잠수꾼 = 1,
            뱃사공,
            사냥꾼
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
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(500);
                        continue;
                    }
                }
            }
        }


        // B-2. 인벤토리
        class Inventory
        {
            private List<Item> _items = new List<Item>();
            public void Add(Item item) => _items.Add(item);

            public List<Item> Item => _items; //프로퍼티


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
                        Console.Write("0. 나가기\n\n>>");
                        if (Console.ReadLine() == "0")
                        {
                            return;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다");
                            Thread.Sleep(500);
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
                            Console.WriteLine("잘못된 입력입니다");
                            Thread.Sleep(500);
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
                        Console.WriteLine($"{i + 1}. {equippedTag}{item.ItemName} | 공격력 +{item.Attack} | 방어력 +{item.Defense} | 체력 +{item.Health}");
                    }
                    Console.Write("\n1. 장착/해제\n0. 나가기\n\n>>");

                    var line = Console.ReadLine();
                    if (line == "0")
                    {
                        return;
                    }
                    else if (line == "1")
                    {
                        Console.Write("장착 혹은 해제할 아이템 번호를 입력하세요: ");
                        var itemInput = Console.ReadLine();
                        int itemIndex;
                        if (int.TryParse(itemInput, out itemIndex) && itemIndex > 0 && itemIndex <= _items.Count)
                        {
                            var item = _items[itemIndex - 1];
                            item.IsEquipped = !item.IsEquipped; // 장착 상태 토글
                            Console.WriteLine($"{item.ItemName}이(가) {(item.IsEquipped ? "장착되었습니다." : "해제되었습니다.")}");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다");
                            Thread.Sleep(500);
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다");
                        Thread.Sleep(500);
                        continue;
                    }
                }
            }
        }


        // B-3. 상점
        class Shop
        {
            // 상점 생성자
            private Player _player;
            private Inventory _inv;
            // 배열로 변경
            private Item[] _tem;

            public Shop(Player player, Inventory inv)
            {
                _player = player;
                _inv = inv;
                _tem = new Item[]
                {
                    new Item(ItemList.산소통, "물 속에서 산소가 부족할 때 필요", 0, 0, 5, 50),
                    new Item(ItemList.물안경, "물 속에서 시야가 흐려지지 않게 도움", 0, 5, 0, 100),
                    new Item(ItemList.잠수복, "해녀들에게 인기 많던 바로 그 옷", 0, 10, 0, 250),
                    new Item(ItemList.래쉬가드, "빠른 건조에 탁월한 가벼운 기능성 수영복", 0, 7, 0, 200),
                    new Item(ItemList.나무작살, "나무라 오래쓰진 못하겠다", 5, 0, 0, 100),
                    new Item(ItemList.무쇠작살, "아주 그냥 작살나죠", 10, 0, 0, 300),
                    new Item(ItemList.낚시세트, "바다 사냥의 근본! 올인원 낚시용품!", 7, 0, 0, 500),
                    new Item(ItemList.작은배, "나무로 만든 귀여운 동동배", 5, 2, 0, 700),
                    new Item(ItemList.튼튼배, "내구성 좋고, 함포가 설치된 무적의 배", 10, 7, 0, 1500),
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

                    for (int i = 0; i < _tem.Length; i++)
                    {
                        var item = _tem[i];
                        string itStatus = item.IsPurchased ? "구매완료" : $"{item.Price} G";
                        //0이면 표시 X
                        string atkPart = item.Attack > 0 ? $" | 공격력 +{item.Attack}" : "";
                        string defPart = item.Defense > 0 ? $" | 방어력 +{item.Defense}" : "";
                        string hpPart = item.Health > 0 ? $" | 체력 +{item.Health}" : "";

                        var stats = new List<string>();
                        if (atkPart != "") stats.Add(atkPart);
                        if (defPart != "") stats.Add(defPart);
                        if (hpPart != "") stats.Add(hpPart);
                        string statsString = string.Join(" | ", stats);

                        Console.WriteLine($"{i + 1}. {item.ItemName} | {item.Description}" + (statsString!="" ? $"{statsString}" : "") + $" | 가격: {item.Price}G");
                    }

                    Console.WriteLine("\n1. 구매하기\n2. 판매하기\n0. 나가기\n\n>>");

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
                        if (int.TryParse(itemInput, out itemIndex) && itemIndex > 0 && itemIndex <= _tem.Length)
                        {
                            var item = _tem[itemIndex - 1];
                            if (item.IsPurchased)
                            {
                                Console.Write("이미 구매한 아이템입니다.\n\n >>");
                                Console.ReadLine();
                            }
                            else if (player.Gold >= item.Price)
                            {
                                player.Gold -= item.Price;
                                item.IsPurchased = true;
                                inv.Add(item);
                                Console.Write($"{item.ItemName}을(를) 구매했습니다.");
                                Thread.Sleep(1000);
                            }
                            else if (player.Gold < item.Price)
                            {
                                Console.WriteLine("Gold가 부족합니다.");
                                Thread.Sleep(500);
                            }
                            else
                            {
                                Console.WriteLine("잘못된 입력입니다");
                                Thread.Sleep(500);
                            }
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다");
                            Thread.Sleep(500);
                            continue;
                        }
                    }
                    else if (input == "2")
                    {
                        //SaleUI(player, inv);
                    }
                }
            }

            // 판매하기
            private void SaleUI(Player player, Inventory inv)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("<상점> - 아이템 판매\n보유하신 아이템을 판매가의 85% 가격으로 판매할 수 있습니다.");
                    Console.WriteLine($"\n[보유골드]\n{player.Gold}G");
                    Console.WriteLine("\n[아이템 목록]");

                    //인벤
                    var saleList = inv.Item.Where(x => x.IsPurchased).ToList();
                    if (saleList.Count == 0)
                    {
                        Console.WriteLine("판매할 아이템이 없습니다.");
                        Console.Write("0. 나가기\n\n>>");
                        if (Console.ReadLine() == "0")
                        {
                            return;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다");
                            Thread.Sleep(500);
                            continue;
                        }

                    }

                    //보유 아이템 목록
                    //for (int i = 0;)
                }
            }
        }


        // C. 아이템 자료 저장
        public enum ItemList
        {
            산소통 = 1,
            잠수복,
            래쉬가드,
            물안경,
            나무작살,
            무쇠작살,
            낚시세트,
            작은배,
            튼튼배
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

            public Item(ItemList item, string desc, int atk, int def, int hp, int price)
            {
                ItemName = item;
                Description = desc;
                Attack = atk;
                Defense = def;
                Health = hp;
                Price = price;

                IsEquipped = false;
                IsPurchased = false;
            }
        }


        // D. 바다던전
        //class Ocean
        //{

        //}


        // E. 휴식하기
        class Rest
        {
            public void RestUI(Player player)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("<휴식하기>");
                    Console.WriteLine("500G를 내면 체력을 100 회복합니다.");
                    Console.WriteLine($"현재 체력: {player.Health}/100");
                    Console.WriteLine($"보유 골드: {player.Gold}G\n");
                    Console.Write("1. 휴식하기(-500G)\n0. 나가기\n\n>>");
                    var input = Console.ReadLine();
                    if (input == "1")
                    {
                        if (player.Gold >= 500)
                        {
                            player.Gold -= 500;
                            player.Health = 100;
                            Console.WriteLine("휴식중...(3초)");
                            Thread.Sleep(3000);
                            Console.Clear();
                            Console.Write("체력이 100 회복되었습니다.\n\n>>[Enter]");
                            Console.ReadLine();
                            return;
                        }
                        else
                        {
                            Console.WriteLine("골드가 부족합니다.");
                            Thread.Sleep(500);
                        }
                    }
                    else if (input == "0")
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(500);
                        continue;
                    }
                }
            }
        }
    }
}