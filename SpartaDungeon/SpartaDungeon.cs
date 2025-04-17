using SpartaDungeon;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using System.Collections.Generic;


namespace SpartaDungeon.GameCore
{


    public enum EquipmentSlot
    {
        Weapon,
        Armor,
        Accessory,
    }
    public enum Itemtype
    {
        Weapon,
        Armor,
        Accessory

    }

            

    //메뉴 매니저
    class MenuManager
    {
        //메인 메뉴 메서드
        public void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전");
            Console.WriteLine("5. 휴식");
            Console.WriteLine("6. 저장");
            Console.WriteLine("7. 불러오기\n");
            Console.Write("원하시는 행동을 입력해주세요.");
        }
        //정보창 메뉴 메서드
        public void ShowStatus(Character player)
        {
            while (true)
            {
                Console.Clear();
                player.PrintStat();
                Console.WriteLine();
                Console.WriteLine("0. 돌아가기");
                Console.WriteLine("\n");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                if (int.TryParse(Console.ReadLine(), out int menuChoice))
                {
                    if (menuChoice == 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("숫자를 입력해주세요.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 숫자만 입력해주세요.");
                    Console.ReadKey();
                }
            }
        }
        //인벤토리 메뉴 메서드
        public void ShowInventory(Inventory inventory, Character player)
        {
            while (true)
            {
                Console.Clear();
                inventory.PrintInventory(player);
                Console.WriteLine();
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 돌아가기");
                Console.WriteLine("\n");

                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("1. 장착 관리");
                            ShowEquipMenu(player);
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("잘못된 번호입니다.");
                            Console.ReadKey();
                            break;
                    }

                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 숫자만 입력해주세요.");
                    Console.ReadKey();
                }

            }
        }
        //장착 메뉴 메서드
        public void ShowEquipMenu(Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("보유중인 아이템을 관리할 수 있습니다.\n");

                player.Inventory.PrintInventory(player);

                Console.WriteLine("현재 장착 아이템: ");
                player.Equipment.ShowEquippedItems();

                Console.WriteLine("\n0. 나가기\n");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int idx))
                {
                    if (idx == 0) break;

                    if (idx > 0 && idx <= player.Inventory.Items.Count)
                    {
                        Item selected = player.Inventory.Items[idx - 1];
                        EquipmentManager eq = player.Equipment;

                        if (player.Equipment.IsEquipped(selected))
                        {
                            Console.Clear();
                            EquipmentSlot slot = eq.GetSlotFromItem(selected);
                            eq.UnequipItem(slot, player);
                            Console.WriteLine($"{selected.Name}을(를) 해제했습니다.");
                        }
                        else
                        {
                            player.Equipment.EquipItem(selected, player);
                            Console.WriteLine($"{selected.Name}을(를) 장착했습니다!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 번호입니다.");
                    }
                }
                Console.WriteLine("계속하려면 아무 키나 누르세요.");
                Console.ReadKey();
            }
        }
        //상점 메뉴 메서드
        public void ShowShopMenu(Character player)
        {
            //상점 아이템 리스트
            List<Item> shopItems = new List<Item>
            {
                new Item("수련자 갑옷", 0, 3, "수련에 도움을 주는 갑옷입니다. ",500,Itemtype.Armor),
                new Item("무쇠갑옷", 0, 5, ".무쇠로 만들어져 튼튼한 갑옷입니다. ",800,Itemtype.Armor),
                new Item("스파르타의 갑옷", 0, 10, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다. ",1200,Itemtype.Armor),
                new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검 입니다. ",300,Itemtype.Weapon),
                new Item("청동 도끼", 5, 0, "어디선가 사용됐던거 같은 도끼입니다. ",700,Itemtype.Weapon),
                new Item("스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다. ",1500,Itemtype.Weapon),
                new Item("은 반지",1,1,"공격력과 방어력을 소폭 높여주는 반지입니다.",600,Itemtype.Accessory),
                new Item("금 반지",3,2,"공격력과 방어력을 높여주는 반지입니다.",1000,Itemtype.Accessory),
                new Item("옥 반지",5,4,"한국의 얼과 혼이 담겨있다고 알려지는 반지입니다.",1700,Itemtype.Accessory),
                new Item("절대 반지",10,10,"공격력과 방어력을 소폭 높여주는 반지입니다.",5000,Itemtype.Accessory)
            };

            List<Item> purchasedItems = new List<Item>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                Console.WriteLine("[보유골드]");
                Console.WriteLine(player.Gold + " G\n");

                Console.WriteLine("\n[구매 가능한 아이템]");
                for (int i = 0; i < shopItems.Count; i++)
                {
                    Item item = shopItems[i];
                    if (!purchasedItems.Contains(item))
                    {
                        Console.Write($"{i + 1}. ");
                        item.PrintInfo();
                    }
                }
                Console.WriteLine("\n[이미 구매한 아이템]");
                foreach (Item item in purchasedItems)
                {
                    Console.Write($"{shopItems.IndexOf(item) + 1}.");
                    item.PrintInfo("[구매완료]");
                }
                Console.WriteLine("\n98. 판매 메뉴로 이동");
                Console.WriteLine("\n0. 돌아가기");
                Console.WriteLine("원하시는 행동을 입력해주세요.");

                string input = Console.ReadLine();

                if (int.TryParse(input, out int choice))
                {
                    if (choice == 0) break;
                    else if(choice == 98)
                    {
                        ShowSellMenu(player);
                        continue;
                    }
                    //구매 가능한 아이템 선택
                    if (choice > 0 && choice <= shopItems.Count)
                    {
                        Item selectedItem = shopItems[choice - 1];

                        //이미 구매한 아이템인지 확인
                        if (purchasedItems.Contains(selectedItem))
                        {
                            Console.WriteLine("이 아이템은 이미 구매하셨습니다.");
                        }
                        else if (player.Gold >= selectedItem.Price)
                        {
                            player.Gold -= selectedItem.Price;
                            player.Inventory.AddItem(selectedItem);
                            purchasedItems.Add(selectedItem);
                            Console.WriteLine($"\n {selectedItem.Name} 구매를 완료했습니다!");
                        }
                        else
                        {
                            Console.WriteLine("\n Gold가 부족합니다.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 번호입니다.");
                    }
                }
                else
                {
                    Console.WriteLine("숫자를 입력해주세요.");
                }

                Console.WriteLine("\n계속하려면 아무키나 누르세요...");
                Console.ReadKey();
            }
        }
        //판매 메뉴 메서드
        private void ShowSellMenu(Character player)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("v판매할 아이템을 선택하세요. \n");
                
                if(player.Inventory.Items.Count == 0)
                {
                    Console.WriteLine("판매할 아이템이 없습니다.");
                    Console.WriteLine("아무 키나 눌러 상점으로 돌아갑니다...");
                    Console.ReadKey();
                    return;
                }
                for(int i = 0; i< player.Inventory.Items.Count; i++)
                {
                    Console.Write($"{i + 1}. ");
                    player.Inventory.Items[i].PrintInfo();
                }
                Console.WriteLine("0. 돌아가기");
                Console.WriteLine("원하시는 행동을 입력해주세요");
                string input = Console.ReadLine();

                if (input == "0") break;
                if(int.TryParse(input, out int choice) && choice >= 1 && choice <= player.Inventory.Items.Count)
                {
                    Item seleted = player.Inventory.Items[choice - 1];
                    int sellPrice = seleted.Price / 2;
                    player.Inventory.Items.RemoveAt(choice - 1);
                    player.Gold += sellPrice;
                    Console.WriteLine($"\n{seleted.Name}을(를) 판매했습니다. + {sellPrice}G");
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
            }
        }
        //던전 메뉴 메서드
        public void ShowDungeonMenu(Character player)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine($"1. 첫번째 던전   | 권장 방어력: {FirstDungeon.RequiredDefense} | 보상: {FirstDungeon.BaseReward}");
                Console.WriteLine($"2. 두번째 던전   | 권장 방어력: {SecondDungeon.RequiredDefense} | 보상: {SecondDungeon.BaseReward}");
                Console.WriteLine($"3. 세번째 던전   | 권장 방어력: {ThirdDungeon.RequiredDefense} | 보상: {ThirdDungeon.BaseReward}");
                Console.WriteLine("\n0. 돌아가기");
                Console.WriteLine("\n 원하시는 행동을 입력해주세요.");
                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            FirstDungeon.Enter(player);
                            break;
                        case 2:
                            SecondDungeon.Enter(player);

                            break;
                        case 3:
                            ThirdDungeon.Enter(player);
                            break;
                        case 0:
                            return;
                        default:
                            Console.WriteLine("잘못된 번호입니다.");
                            break;

                    }

                }
                else
                {
                    Console.WriteLine("숫자를 입력해주세요.");
                }
                if (player.Hp <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("캐락터가 사망했습니다!. 게임을 처음부터 다시 시작합니다.");
                    Console.WriteLine("계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                    break;
                }
            }
        }//첫번째 던전
        Dungeon FirstDungeon = new Dungeon
        {
            Name = "첫번째 던전",
            RequiredDefense = 7,
            BaseReward = 200

        };//두번째 던전
        Dungeon SecondDungeon = new Dungeon
        {
            Name = "두번째 던전",
            RequiredDefense = 10,
            BaseReward = 500

        };//세번째 던전
        Dungeon ThirdDungeon = new Dungeon
        {
            Name = "세번째 던전",
            RequiredDefense = 13,
            BaseReward = 700

        };


        public void Rest(Character player)
        {
            int RestCost = 300;
            int RecoverAmount = 50;

            Console.Clear();
            Console.WriteLine($"휴식을 취하시겠습니까? (Gold {RestCost} 소모, 체력 + {RecoverAmount})");
            Console.WriteLine("1. 예");
            Console.WriteLine("2. 아니요");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int choice))
                    {

                    if(choice == 1)
                    {
                        if(player.Gold >= RestCost)
                        {
                            if (player.Hp >= player.MaxHp)
                            {
                                Console.WriteLine("\n이미 체력이 가득 찼습니다.");
                                break;
                            }
                            else
                            {
                                player.Gold -= RestCost;
                                player.Hp += RecoverAmount;
                                if (player.Hp > player.MaxHp)
                                {
                                    player.Hp = player.MaxHp;
                                }
                                Console.WriteLine($"\n휴식을 완료했습니다. 체력 + {RecoverAmount}, 현재 체력: {player.Hp}");
                                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                                Console.ReadKey();

                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nGold가 부족합니다.");
                            Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                            Console.ReadKey();

                            break;
                        }
                    }
                    else if(choice == 2)
                    {
                        Console.WriteLine("\n휴식을 취하지 않았습니다.");
                        Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();

                        break;
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                        Console.ReadKey();
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                    Console.ReadKey();
                    break;
                }

            }

        }
    }

    [JsonDerivedType(typeof(Warrior), typeDiscriminator: "Warrior")]
    [JsonDerivedType(typeof(Archer), typeDiscriminator: "Archer")]

    public abstract class Character
    {
        //캐릭터 스탯 부모 클래스
        public string Name { get; set; }
        [JsonInclude] public string JobName { get; protected set; }
        [JsonInclude] public int Attack { get; set; }
        [JsonInclude] public int Defense { get; set; }
        [JsonInclude] public int Hp { get; set; }
        [JsonInclude] public int Gold { get; set; }
        [JsonInclude] public Inventory Inventory { get; set; }
        [JsonInclude] public EquipmentManager Equipment { get; set; }
        [JsonInclude] public int BaseAttack { get; protected set; }
        [JsonInclude] public int BaseDefense { get; protected set; }
        [JsonInclude] public string ClassType;
        [JsonInclude] public int MaxHp { get; set; }
       

        //캐릭터 생성 시 이름 설정 및 인벤토리와 장비 매니저 초기화
        public Character()
        {
            Inventory = new Inventory();
            Equipment = new EquipmentManager();
        }

        public Character(string name) : this()
        {
            Name = name;
        }


        //직업마다 스텟을 다르게 하기 위한 추상 메서드
        public abstract void InitializeStats();

        //캐릭터 스탯 출력
        public void PrintStat()
        {
            Console.WriteLine("Lv. 01");
            Console.WriteLine($"{this.Name}({this.JobName})");

            string attackBonuse = Attack > BaseAttack ? $" (+{Attack - BaseAttack})" : "";
            string defenseBonuse = Defense > BaseDefense ? $" (+{Defense - BaseDefense})" : "";

            Console.WriteLine($"공격력 : {BaseAttack}{attackBonuse}");
            Console.WriteLine($"방어력 : {BaseDefense}{defenseBonuse}");
            Console.WriteLine($"체 력 : {Hp} / {MaxHp}");
            Console.WriteLine($"Gold : {Gold} G");
        }

        // 장착시 능력치 상승 메서드
        public void IncreaseAttack(int amount)
        {
            Attack += amount;
        }
        public void IncreaseDefense(int amount)
        {
            Defense += amount;
        }
        //로드 후 장비 보정
        public void RecalculateStats()
        {
            // 기본 능력치로 초기화
            Attack = BaseAttack;
            Defense = BaseDefense;

            // 무기/방어구
            if (Equipment?.EquippedItems != null)
            {
                foreach (var item in Equipment.EquippedItems.Values)
                {
                    IncreaseAttack(item.Attack);
                    IncreaseDefense(item.Defense);
                }
            }

            // 악세서리
            if (Equipment?.Accessories != null)
            {
                foreach (var item in Equipment.Accessories)
                {
                    IncreaseAttack(item.Attack);
                    IncreaseDefense(item.Defense);
                }
            }
        }





    }
    //직업 전사
    class Warrior : Character
    {
        public Warrior() : base("") { }
        public Warrior(string name) : base(name)
        {
            JobName = "전사";
            InitializeStats();
            ClassType = "Warrior";
        }
        //전사 스텟
        public override void InitializeStats()
        {
            JobName = "전사";
            BaseAttack = 8;
            BaseDefense = 8;
            Hp = 120;
            MaxHp = 120;
            Gold = 1500;
            Attack = BaseAttack;
            Defense = BaseDefense;
        }
    }
    //직업 궁수
    class Archer : Character
    {
        public Archer() : base("") { }
        public Archer(string name) : base(name)
        {
            JobName = "궁수";
            InitializeStats();
            ClassType = "Archer";
        }
        //궁수 스텟
        public override void InitializeStats()
        {
            JobName = "궁수";
            BaseAttack = 12;
            BaseDefense = 5;
            Hp = 100;
            MaxHp = 100;
            Gold = 1500;
            Attack = BaseAttack;
            Defense = BaseDefense;
        }
    }
    //아이템 클래스
    public class Item
    {
        [JsonInclude] public string Name { get; set; }
        [JsonInclude] public string Description { get; set; }
        [JsonInclude] public int Price { get; set; }
        [JsonInclude] public int Attack { get; set; }
        [JsonInclude] public int Defense { get; set; }
        [JsonInclude] public Itemtype Type { get; set; }

        public Item(string name, int attack = 0, int defense = 0, string description = "", int price = 0, Itemtype type = Itemtype.Weapon)
        {
            Name = name;
            Attack = attack;
            Defense = defense;
            Description = description;
            Price = price;
            Type = type;
        }
        //아이템 정보 메서드
        public void PrintInfo()
        {
            Console.WriteLine($"{Name}  | 공격력: {Attack}  | 방어력: {Defense}  |  설명: {Description}  |  가격: {Price} G\n");
        }
        public void PrintInfo(string extra = "")
        {
            Console.WriteLine($"{extra}{Name}  | 공격력: {Attack}  | 방어력: {Defense}  |  설명: {Description}  |  가격: {Price} G\n");
        }

        public override bool Equals(object obj)
        {
            if (obj is not Item other) return false;
            return Name == other.Name && Type == other.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Type);
        }


    }
    //인벤토리(가방)메서드
    public class Inventory
    {
        [JsonInclude] public List<Item> Items { get; set; } = new();
        //가방 리스트
        public Inventory()
        {
            Items = new List<Item>();
        }
        //아이템 추가 메서드(구매)
        public void AddItem(Item item)
        {
            Items.Add(item);
        }
        //인벤토리 출력 메서드
        public void PrintInventory(Character player)
        {
            Console.WriteLine("인벤토리 목록: \n");
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                bool isEquipped = player.Equipment.IsEquipped(item);
                string equippedText = isEquipped ? "[E] " : "";
                Console.Write($"{i + 1}. ");
                item.PrintInfo(equippedText);
            }
        }
        //아이템 장착(사용)
        public void UseItem(Character player, Item item)
        {
            player.IncreaseAttack(item.Attack);
            player.IncreaseDefense(item.Defense);
        }
        //null 방어 코드
        public void FixNullReferences()
        {
            if (Items == null)
            {
                Items = new List<Item>();
            }
        }
    }
    //장착 매니저
    public class EquipmentManager
    {
        [JsonInclude] public Dictionary<EquipmentSlot, Item> EquippedItems { get; set; }
        [JsonInclude] public List<Item> Accessories { get; set; }

        public bool IsEquipped(Item item)
        {
            return EquippedItems.Values.Any(e => e.Name == item.Name && e.Type == item.Type)
                || Accessories.Any(a => a.Name == item.Name && a.Type == item.Type);
        }

        public EquipmentManager()
        {
            EquippedItems = new Dictionary<EquipmentSlot, Item>();
            Accessories = new List<Item>();
        }
        //장착했을때
        public void EquipItem(Item item, Character player)
        {
            EquipmentSlot slot = GetSlotFromItem(item);

            if (slot == EquipmentSlot.Accessory)
            {
                if (Accessories.Contains(item))
                {
                    Console.WriteLine("이미 장착된 악세서리입니다.");
                    return;
                }

                if (Accessories.Count >= 2)
                {
                    Console.WriteLine("악세서리는 최대 2개까지 장착할 수 있습니다.");
                    return;
                }

                Accessories.Add(item);
            }
            else
            {       //기존 장비가 있다면 제거
                if (EquippedItems.TryGetValue(slot, out Item equipped))
                {
                    player.IncreaseAttack(-equipped.Attack);
                    player.IncreaseDefense(-equipped.Defense);
                }
                //새로운 장비 등록
                EquippedItems[slot] = item;
            }//장착하는 아이템 인벤토리에 없으면 추가
            if (!player.Inventory.Items.Contains(item))
            {
                player.Inventory.AddItem(item);
            }

            player.IncreaseAttack(item.Attack);
            player.IncreaseDefense(item.Defense);

        }


        //장착 해제했을때
        public void UnequipItem(EquipmentSlot slot, Character player)
        {
            if (slot == EquipmentSlot.Accessory)
            {
                if (Accessories.Count == 0)
                {
                    Console.WriteLine("장착된 악세서리가 없습니다.");
                    return;
                }

                Console.WriteLine("해제할 악세서리를 선택하세요: \n");
                for (int i = 0; i < Accessories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Accessories[i].Name}");
                }
                //장착 악세서리 해제
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= Accessories.Count)
                {
                    Item removed = Accessories[choice - 1];
                    Accessories.RemoveAt(choice - 1);
                    player.IncreaseAttack(-removed.Attack);
                    player.IncreaseDefense(-removed.Defense);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
                return;
            }
            //장착 아이템 해제
            if (EquippedItems.TryGetValue(slot, out Item equippedItem))
            {
                player.IncreaseAttack(-equippedItem.Attack);
                player.IncreaseDefense(-equippedItem.Defense);
                EquippedItems.Remove(slot);
                // Console.WriteLine($"{equippedItem.Name}을(를) 해제했습니다.");
            }
        }
        //장착한 아이템 표시
        public void ShowEquippedItems()
        {
            foreach (var pair in EquippedItems)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value.Name}");
            }

            for (int i = 0; i < Accessories.Count; i++)
            {
                Console.WriteLine($"Accessory {i + 1}: {Accessories[i].Name}");
            }
        }
        //장착한 슬롯이 무기/방어구인지 여부 확인
        public EquipmentSlot GetSlotFromItem(Item item)
        {
            return item.Type switch
            {
                Itemtype.Weapon => EquipmentSlot.Weapon,
                Itemtype.Armor => EquipmentSlot.Armor,
                Itemtype.Accessory => EquipmentSlot.Accessory,
                _ => throw new Exception("해당 아이템은 장착할 수 없습니다.")
            };
        }
        //장착한 아이템 불러오기
        public void LoadEquippedItems(Dictionary<EquipmentSlot, Item> equipped, List<Item> Accessories, Character player)
        {
            EquippedItems = equipped ?? new Dictionary<EquipmentSlot, Item>();
            Accessories = Accessories ?? new List<Item>();

            foreach (var item in EquippedItems.Values)
            {
                player.IncreaseAttack(item.Attack);
                player.IncreaseDefense(item.Defense);
            }

            foreach (var item in Accessories)
            {
                player.IncreaseAttack(item.Attack);
                player.IncreaseDefense(item.Defense);
            }
        }
        //null 방어코드(장착 아이템, 악세사리)
        public void FixNullReferences()
        {
            if (EquippedItems == null)
            {
                EquippedItems = new Dictionary<EquipmentSlot, Item>();
            }
            if (Accessories == null)
            {
                Accessories = new List<Item>();
            }
        }

    }
    //던전 클래스
    public class Dungeon
    {
        public string Name { get; set; }
        public int RequiredDefense { get; set; }
        public int BaseReward { get; set; }

        private Random random = new();
        private Random Hprandom = new();

        //던전 입장 메서드
        public void Enter(Character player)
        {
            Console.Clear();
            Console.WriteLine($"[{Name}] 던전에 입장 시도 중...\n");
            int beforeHp = player.Hp;
            while (true)
            {   
                //방어력 > 권장방어력
                if (player.Defense >= RequiredDefense)
                {
                    Console.WriteLine("안전하게 던전을 클리어 했습니다.\n");
                    if (player.Defense > RequiredDefense)
                    {
                        int Hpdecrease = random.Next((25 - (player.Defense - RequiredDefense)), (35 - (player.Defense - RequiredDefense)));
                        player.Hp -= Hpdecrease;
                    }
                    Console.WriteLine("[탐험 결과]\n");
                    Console.WriteLine($"{beforeHp} -> {player.Hp}");
                    GrantReward(player);
                    break;
                }//방어력 < 권장방어력
                else
                {
                    Console.WriteLine($"현재 방어력이 권장 방어력이 보다 낮습니다. ({player.Defense} < {RequiredDefense})");
                    Console.WriteLine("40%의 확률로 던전을 실패할 수 있습니다.");
                    Console.WriteLine("1. 도전한다.");
                    Console.WriteLine("2. 돌아간다.");
                    Console.WriteLine("\n원하시는 행동을 입력해주세요.");

                    string input = Console.ReadLine();
                    if (input == "2")
                    {
                        Console.WriteLine("던전 입장을 취소했습니다.");
                        Console.ReadKey();
                        return;
                    }
                    else if (input == "1")
                    {
                        int chance = random.Next(100);
                        if (chance < 40)
                        {
                            Console.WriteLine("\n 던전 클리어 실패!");
                            player.Hp /= 2;
                            Console.WriteLine($"당신의 체력이 절반으로 줄어듭니다. 현재 체력: {player.Hp}");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("\n간신히 던전을 클리어했습니다.");
                            int Hpdecrease = random.Next((25 + (RequiredDefense - player.Defense)), (35 + (RequiredDefense - player.Defense)));
                            player.Hp -= Hpdecrease;
                            Console.WriteLine($"{beforeHp} -> {player.Hp}");
                            GrantReward(player);
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
            Console.ReadKey();
        }
            //던전 보상 메서드
            private void GrantReward(Character player)
        {
            int reward = BaseReward + player.Attack * 10;
            int bonusreward = player.Attack * 10;
            player.Gold += reward;
            Console.WriteLine($"Gold: {BaseReward}+{bonusreward} G");

        }




    }










//저장 메서드
static class SaveSystem
{
    const string SaveFile = "save.json";

    public static void Save(Character player)
    {
        if (player is Warrior) player.ClassType = "Warrior";
        else if (player is Archer) player.ClassType = "Archer";

        //Json 저장 옵션
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
            Converters = { new JsonStringEnumConverter() }

        };

        string json = JsonSerializer.Serialize(player, options);
        File.WriteAllText(SaveFile, json);
        Console.WriteLine("\n게임이 저장되었습니다.");
        Console.ReadKey();
    }
    //불러오기 메서드
    public static Character Load()
    {
        if (!File.Exists(SaveFile))
        {
            return null;
        }

        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            Converters = { new JsonStringEnumConverter() }
        };

        string json = File.ReadAllText(SaveFile);
        Character loaded = JsonSerializer.Deserialize<Character>(json, options);
        //로드된 캐릭터 스탯 계산
        if (loaded != null)
        {
            loaded.Attack = loaded.BaseAttack;
            loaded.Defense = loaded.BaseDefense;
            loaded.Inventory.FixNullReferences();
            loaded.Equipment.FixNullReferences();
            loaded.RecalculateStats();
        }
        Console.WriteLine("\n게임을 불러왔습니다.");
        Console.ReadKey();
        return loaded;
    }

}







    internal class SpartaDungeon
    {
        static void Main(string[] args)
        {
                ShowIntro();
                Character player = null;
                Console.Clear();
                Console.WriteLine("1. 새 게임 시작");
                Console.WriteLine("2. 게임 불러오기");
                Console.WriteLine("원하시는 행동을 선택해주세요.");
                string input = Console.ReadLine();
                if (input == "1")
                {

                    //이름입력
                    Console.Clear();
                    Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                    Console.WriteLine("원하시는 이름을 설정해주세요.");
                    string name = Console.ReadLine();
                    Console.WriteLine($"입력하신 이름은 {name} 입니다");
                    Console.Clear();

                    //직업 선택
                    while (true)
                    {
                        Console.WriteLine("직업을 선택하세요 \n1.전사 2. 궁수");
                        int job = int.Parse(Console.ReadLine());
                        if (job == 1)
                        {
                            player = new Warrior(name);
                            break;
                        }
                        else if (job == 2)
                        {
                            player = new Archer(name);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("잘못된 입력입니다.");
                        }
                    }
                }
                else if (input == "2")
                {
                    player = SaveSystem.Load();
                }

                if (player == null)
                {
                    Console.WriteLine("플레이어 정보가 없습니다. 먼저 게임을 시작하거나 불러오세요.");
                    Console.ReadKey();
                    return;
                }

                MenuManager menu = new MenuManager();


            while (player.Hp > 0)
            {
                //메인 메뉴
                menu.ShowMainMenu();

                if (int.TryParse(Console.ReadLine(), out int choice))
                    {

                    switch (choice)
                    {
                        //스텟창 확인
                        case 1:
                            menu.ShowStatus(player);
                            break;
                        //인벤토리 확인
                        case 2:
                            menu.ShowInventory(player.Inventory, player);
                            Console.Clear();
                            break;

                        //상점탭 확인
                        case 3:
                            menu.ShowShopMenu(player);
                            Console.Clear();
                            break;
                        //던전 진입
                        case 4:
                            menu.ShowDungeonMenu(player);
                            break;

                        case 5:
                            menu.Rest(player);
                            break;
                            
                        //세이브
                        case 6:
                            SaveSystem.Save(player);
                            break;
                        //로드
                        case 7:
                            player = SaveSystem.Load() ?? player;
                            break;


                        //잘못된 입력
                        default:
                            Console.WriteLine("잘못된 입력입니다.");
                            Console.ReadKey();
                            break;

                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 숫자만 입력해주세요.");
                    Console.ReadKey();
                }
            }


            //캐릭터 사망
            Console.Clear();
            Console.WriteLine("캐락터가 사망했습니다!. 게임을 처음부터 다시 시작합니다.");
            Console.WriteLine("계속하려면 아무 키나 누르세요...");
            Console.ReadKey();
        }


                                // 인트로
    public static void ShowIntro()
    {

        Console.Clear();
        Console.WriteLine(@"
                                  _________                    __                   
                                 /   _____/__________ ________/  |______            
                                 \_____  \\____ \__  \\_  __ \   __\__  \           
                                 /        \  |_> > __ \|  | \/|  |  / __ \_         
                                /_______  /   __(____  /__|   |__| (____  /         
                                        \/|__|       \/                 \/          
                                ________                                            
                                \______ \  __ __  ____    ____   ____  ____   ____  
                                 |    |  \|  |  \/    \  / ___\_/ __ \/  _ \ /    \ 
                                 |    `   \  |  /   |  \/ /_/  >  ___(  <_> )   |  \
                                /_______  /____/|___|  /\___  / \___  >____/|___|  /
                                        \/           \//_____/      \/           \/    
                                                                    
                                       ★ Sparta Dungeon - 영웅의 전당 ★

                                                                                   ");
        Console.WriteLine("\n\n\n\n\n\n");
        Console.WriteLine("\t\t\t\t\t엔터를 눌러 게임을 시작하세요...");
        Console.ReadLine();
    }



}

}




