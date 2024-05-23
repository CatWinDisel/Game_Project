using System;

internal class Program
{
    public static void Main(string[] args)
    {
        int size_of_deck = 35;
        int[,] Cards = Create_Desk();
        Cards = Shuffle(Cards);

        int[,] Players_Cards_0 = new int[36, 2];
        int[,] Players_Cards_1 = new int[36, 2];
        for (int i = 0; i < 36; i++)
        {
            Players_Cards_0[i, 0] = -1;
            Players_Cards_0[i, 1] = -1;
            Players_Cards_1[i, 0] = -1;
            Players_Cards_1[i, 1] = -1;
        }

        Give_Cards_To_Players(ref Cards, ref size_of_deck, ref Players_Cards_0);
        Give_Cards_To_Players(ref Cards, ref size_of_deck, ref Players_Cards_1);

        int trump = Cards[13,0]; //козырь
        if (trump == 0)
        {
            Console.WriteLine("Козырь: Черви");
        }
        else if(trump == 1)
        {
            Console.WriteLine("Козырь: Бубна");
        }
        else if (trump == 2)
        {
            Console.WriteLine("Козырь: Трефа");
        }
        else if (trump == 3)
        {
            Console.WriteLine("Козырь: Пики");
        }

        //int first_move = First_Move(Players_Cards_0, Players_Cards_1, trump);

        int next_move = 0; // Первый ход
        while (true)
        {
            int amount_0 = amount_of_cards( ref Players_Cards_0);
            int amount_1 = amount_of_cards(ref Players_Cards_1);

            if (amount_0 + size_of_deck == -1)
            {
                Console.WriteLine("Победил Игрок!");
                break;
            }

            if(amount_1 + size_of_deck == -1)
            {
                Console.WriteLine("Победил Компьютер!"); 
                break;
            }

            Give_Cards_To_Players(ref Cards, ref size_of_deck, ref Players_Cards_0);
            Give_Cards_To_Players(ref Cards, ref size_of_deck, ref Players_Cards_1);

            Console.WriteLine("Карты игрока:");
            Output(Players_Cards_0, trump);
            Console.WriteLine();
            Console.WriteLine("Карты компьютера:");
            Output(Players_Cards_1, trump);
            Console.WriteLine();

            next_move = Step(ref Players_Cards_0, ref Players_Cards_1, next_move, trump);

        }
        
    }

    public static int amount_of_cards(ref int[,] array)
    {
        int count = 0;
        for (int x = 0; x < 36; x++)
        {
            if (array[x,0] != -1)
            {
                count++;
            }
        }
        return count; 
    }

    public static int First_Move(int[,] first_players_deck, int[,] second_players_deck, int trump) //только для первого раза
    {
        int first_players_min = 0;
        int second_players_min = 0;
        int winner = -1;
        for (int i = 0; i < 6; i++)
        {
            if (first_players_deck[i,0] == trump) 
            {
                first_players_min = first_players_deck[i, 1];
            }
            if (second_players_deck[i, 0] == trump)
            {
                second_players_min = second_players_deck[i, 1];
            }

        }

        if (first_players_min > second_players_min)
        {
            winner = 0;
        }

        else if(first_players_min < second_players_min)
        {
            winner = 1;
        }

        else if(first_players_min == second_players_min)
        {
            Random rnd = new Random();
            winner = rnd.Next(0, 2);
        }

        return winner;
    
    }

    public static int Step(ref int[,] card_deck_0,ref int[,] card_deck_1, int winner, int trump)
    {
        int next = winner;
        int[,] Desk_play = new int[13, 2]; //игровая доска

        for (int x = 0; x < 13; x++)
        {
            Desk_play[x, 0] = -1;
            Desk_play[x, 1] = -1;
        }

        int[,] Attack_deck = new int[36, 2];
        int[,] Defence_deck = new int[36, 2];

        if (winner == 0)
        {
            Attack_deck = card_deck_0;
            Defence_deck = card_deck_1;
        }

        else
        {
            Attack_deck = card_deck_1;
            Defence_deck = card_deck_0;
        }

        for (int i = 0; i < 12; i+=2)
        {
            int[] attack_card = {0, 0};
            if (winner == 0)
            {
                int number;
                while(true)
                { 
                    int test = 0;
                    Console.WriteLine("------------------------------------------------");
                    Console.Write("Выберите карту, если ходить нечем, введите 0 : ");
                    number = Convert.ToInt32(Console.ReadLine()) - 1;

                    if (number == -1)
                    {
                        Console.WriteLine("Бито");
                        Console.WriteLine("------------------------------------------------");
                        next = 1;
                        return next;
                    }

                    attack_card[0] = Attack_deck[number, 0];
                    attack_card[1] = Attack_deck[number, 1];
                    
                    if (attack_card[0] != -1)
                    {
                        test++;
                    }
                    if (i != 0)
                    {
                        for (int d = 0; d <= 12; d++)
                        {
                            if (attack_card[1] == Desk_play[d, 1])
                            {
                                test++;
                                
                                break;
                            }
                        }
                    }
                    else { test++; }
                    
                    if (test == 2)
                    {
                        Console.WriteLine("------------------------------------------------");
                        Console.Write("Вы походили картой : ");
                        MinOutput(attack_card);
                        Console.WriteLine("------------------------------------------------");
                        Attack_deck[number, 0] = -1; Attack_deck[number, 1] = -1; //Переносим карту на поле
                        break;

                    }
                }
            }

            if(winner == 1) 
            {
          
                while (true)
                {
                    int min = 10;
                    int iter = 0;
                    for ( int d = 0; d < 36; d++ )
                    {
                        for (int k = 0; k < 12; k++)
                        {
                            if (i == 0)
                            {
                                if (Attack_deck[d,0] != trump && Attack_deck[d,0] != -1)
                                {
                                    if (Attack_deck[d,1] < min)
                                    {
                                        min = Attack_deck[d, 1];
                                        iter = d;
                                    }
                                }
                            }
                            else
                            {
                                if (Attack_deck[d, 1] == Desk_play[k,1] && Attack_deck[d, 0] != -1)
                                {
                                    if (Attack_deck[d, 1] < min)
                                    {
                                        min = Attack_deck[d, 1];
                                        iter = d;
                                    }
                                }
                            }
                        }
                    }

                    if (min != 10)
                    {
                        attack_card[0] = Attack_deck[iter, 0];
                        attack_card[1] = Attack_deck[iter, 1];
                        Attack_deck[iter, 0] = -1;
                        Attack_deck[iter, 1] = -1;
                        break;
                    }
                    if (min == 10)
                    {
                        Console.WriteLine("------------------------------------------------");
                        Console.WriteLine("Бито");
                        Console.WriteLine("------------------------------------------------");
                        next = 0;
                        return next;
                    }

                }

                Console.WriteLine("------------------------------------------------");
                Console.Write("Компьютер походил картой : ");
                MinOutput(attack_card);
                Console.WriteLine("------------------------------------------------");
            }

            Desk_play[i, 0] = attack_card[0];
            Desk_play[i, 1] = attack_card[1];

            if (winner == 0)
            {
                int count = 0;
                for (int j = 0; j < Defence_deck.Length/2; j++)
                {
                    if (Defence_deck[j, 0] != -1)//убираем мусор
                    {
                        if (Defence_deck[j, 0] == Desk_play[i, 0])//сверяем масть
                        {
                            if (Defence_deck[j, 1] > Desk_play[i, 1])//сверяем значение карт
                            {
                                int[] defense_card = new int[2];
                                defense_card[0] = Defence_deck[j, 0];
                                defense_card[1] = Defence_deck[j, 1];
                                Desk_play[i + 1, 0] = Defence_deck[j, 0];
                                Desk_play[i + 1, 1] = Defence_deck[j, 1];
                                Console.Write("Компьютер отбиваeтся картой : ");
                                Defence_deck[j, 0] = -1;
                                Defence_deck[j, 1] = -1;
                                MinOutput(defense_card);
                                count++;
                                Console.WriteLine("------------------------------------------------");
                                Console.WriteLine("Игровая доска");
                                Output(Desk_play, trump);
                                Console.WriteLine("------------------------------------------------");
                                Console.WriteLine();
                                Console.WriteLine("Ваша колода : ");
                                Output(Attack_deck, trump);
                                break;

                            }
                        }
                        else if (Defence_deck[j,0] == trump & Desk_play[i,0] != trump) //при козыре
                        {
                            int[] defense_card = new int[2];
                            defense_card[0] = Defence_deck[j, 0];
                            defense_card[1] = Defence_deck[j, 1];
                            Desk_play[i + 1, 0] = Defence_deck[j, 0];
                            Desk_play[i + 1, 1] = Defence_deck[j, 1];
                            Console.Write("Компьютер отбиваeтся картой : ");
                            Defence_deck[j, 0] = -1;
                            Defence_deck[j, 1] = -1;
                            MinOutput(defense_card);
                            count++;
                            Console.WriteLine("------------------------------------------------");
                            Console.WriteLine("Игровая доска");
                            Output(Desk_play, trump);
                            Console.WriteLine("------------------------------------------------");
                            Console.WriteLine();
                            Console.WriteLine("Ваша колода : ");
                            Output(Attack_deck, trump);
                            break;
                        }

                    }
                }
                if (count == 0)
                {
                    for (int x = 0; x < 12; x++)
                    {
                        if (Desk_play[x, 1] != -1)
                        {
                            for (int y = 0; y < 36; y++)
                            {
                                if (Defence_deck[y, 1] == -1)
                                {
                                    Defence_deck[y, 0] = Desk_play[x, 0];
                                    Defence_deck[y, 1] = Desk_play[x, 1];
                                    Desk_play[x, 0] = -1;
                                    Desk_play[x, 1] = -1;
                                    break;
                                }
                            }
                        }
                    }
                    Console.WriteLine("Компьютер забирает");
                    Console.WriteLine("------------------------------------------------");
                    card_deck_1 = Defence_deck;
                    card_deck_0 = Attack_deck;
                    next = 0;
                    return next;
                }
            }


            if(winner == 1) 
            {

                int count = 0;
                
                for (int n = 0; n < Defence_deck.Length / 2; n++)
                {
                    if (Desk_play[i, 0] == Defence_deck[n, 0])
                    {
                        if (Desk_play[i, 1] < Defence_deck[n, 1])
                        {
                            count++;
                        }
                    }
                    else if (Desk_play[i, 0] != trump & Defence_deck[n,0] == trump)
                    {
                        count++;
                    }
                }

                if (count == 0)
                {
                    for (int x = 0; x < 12; x++)
                    {
                        if (Desk_play[x, 1] != -1)
                        {
                            for (int y = 0; y < 36; y++)
                            {
                                if (Defence_deck[y,1] == -1)
                                {
                                    Defence_deck[y, 0] = Desk_play[x, 0];
                                    Defence_deck[y, 1] = Desk_play[x, 1];
                                    Desk_play[x, 0] = -1;
                                    Desk_play[x, 1] = -1;
                                    break;
                                }
                            }
                        }
                    }
                    card_deck_0 = Defence_deck;
                    card_deck_1 = Attack_deck;
                    Console.WriteLine("Вы не можете отбиться");
                    next = 1;
                    return next;

                }

                if (count != 0)
                {
                    while (true)
                    {
                        Console.Write("Выберите карту для защиты : ");
                        int number = Convert.ToInt32(Console.ReadLine()) - 1;
                        if (Defence_deck[number, 0] == Desk_play[i, 0]) //сверка масти
                        {
                            if (Defence_deck[number, 1] > Desk_play[i, 1]) //сверка значений
                            {
                                int[] defense_card = new int[2];
                                defense_card[0] = Defence_deck[number, 0];
                                defense_card[1] = Defence_deck[number, 1];
                                Desk_play[i + 1, 0] = Defence_deck[number, 0];
                                Desk_play[i + 1, 1] = Defence_deck[number, 1];
                                Console.Write("Вы отбились картой : ");
                                MinOutput(defense_card);
                                Console.WriteLine();
                                Defence_deck[number, 0] = -1;
                                Defence_deck[number, 1] = -1;
                                Console.WriteLine("------------------------------------------------");
                                Console.WriteLine("Игровая доска");
                                Output(Desk_play, trump);
                                Console.WriteLine("------------------------------------------------");
                                Console.WriteLine();
                                Console.WriteLine("Ваша колода : ");
                                Output(Defence_deck, trump);
                                break;
                            }
                        }
                        else if (Defence_deck[number, 0] == trump & Desk_play[i,0] != trump)
                        {
                            int[] defense_card = new int[2];
                            defense_card[0] = Defence_deck[number, 0];
                            defense_card[1] = Defence_deck[number, 1];
                            Desk_play[i + 1, 0] = Defence_deck[number, 0];
                            Desk_play[i + 1, 1] = Defence_deck[number, 1];
                            Console.WriteLine("Вы отбились картой: ");
                            MinOutput(defense_card);
                            Console.WriteLine();
                            Defence_deck[number, 0] = -1;
                            Defence_deck[number, 1] = -1;
                            Console.WriteLine("------------------------------------------------");
                            Console.WriteLine("Игровая доска");
                            Output(Desk_play, trump);
                            Console.WriteLine("------------------------------------------------");
                            Console.WriteLine();
                            Console.WriteLine("Ваша колода : ");
                            Output(Defence_deck, trump);
                            break;
                        }
                    }
                }
            }
        }

        return 0;

    }


    public static void MinOutput(int[] array)
    {
        switch (array[0])
        {
            case 0:
                Console.Write("Черви ");
                break;
            case 1:
                Console.Write("Бубна ");
                break;
            case 2:
                Console.Write("Трефа ");
                break;
            case 3:
                Console.Write("Пики  ");
                break;
        }

        switch (array[1])
        {
            case 0:
                Console.WriteLine("6");
                break;
            case 1:
                Console.WriteLine("7");
                break;
            case 2:
                Console.WriteLine("8");
                break;
            case 3:
                Console.WriteLine("9");
                break;
            case 4:
                Console.WriteLine("10");
                break;
            case 5:
                Console.WriteLine("Валет");
                break;
            case 6:
                Console.WriteLine("Дама");
                break;
            case 7:
                Console.WriteLine("Король");
                break;
            case 8:
                Console.WriteLine("Туз");
                break;

        }
    }

    public static void Output(int[,] array, int trump) 
    {
        for (int i = 0; i < array.Length/2; ++i)
        {
            if (array[i,0] != -1)
            {
                Console.Write((i+1) + ": ");
            }
            switch (array[i, 0])
            {
                case 0:
                    if(trump == 0) { Console.Write("Черви (козырь) "); }
                    else { Console.Write( "Черви "); }
                    break;

                case 1:
                    if (trump == 1) { Console.Write("Бубна (козырь) "); }
                    else { Console.Write("Бубна "); }
                    break;

                case 2:
                    if (trump == 2) { Console.Write("Трефа (козырь) "); }
                    else { Console.Write("Трефа "); }
                    break;

                case 3:
                    if (trump == 3) { Console.Write("Пики (козырь) "); }
                    else { Console.Write("Пики "); }
                    break;

            }

            switch (array[i, 1])
            {
                case 0:
                    Console.WriteLine("6");
                    break;
                case 1:
                    Console.WriteLine("7");
                    break;
                case 2:
                    Console.WriteLine("8");
                    break;
                case 3:
                    Console.WriteLine("9");
                    break;
                case 4:
                    Console.WriteLine("10");
                    break;
                case 5:
                    Console.WriteLine("Валет");
                    break;
                case 6:
                    Console.WriteLine("Дама");
                    break;
                case 7:
                    Console.WriteLine("Король");
                    break;
                case 8:
                    Console.WriteLine("Туз");
                    break;

            }

        }
    
    }

    public static int[,] Create_Desk() 
    {
        int [,] card_stack = new int[36, 2];

        int n = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                card_stack[n, 0] = i;
                n++;

            }
        }
        int m = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                card_stack[m, 1] = j;
                m++;

            }
        }
        return card_stack;
    }

    public static int[,] Shuffle(int[,] array)
    {
        var random = new Random();
        for (int i = 35; i >= 0; i--)
        {
            int j = random.Next(i);
            var t_0 = array[i, 0];
            var t_1 = array[i, 1];
            array[i, 0] = array[j, 0];
            array[i, 1] = array[j, 1];
            array[j, 0] = t_0;
            array[j, 1] = t_1;
        }
        return array;
    }

    public static void Give_Cards_To_Players(ref int[,] array, ref int n,ref int[,] players_cards)
    {

        int count = 0; 
        for (int j = 0; j < players_cards.Length/2; j++)
        {
            if (players_cards[j,0] != -1)
            {
                count++;
            }
        }

        {
            for (int x = 0; x < 36; x++)
            {
                if (count < 6)
                {
                    if (players_cards[x, 0] == -1 & n >= 0)
                    {
                        players_cards[x, 0] = array[n, 0];
                        players_cards[x, 1] = array[n, 1];

                        array[n, 0] = -1;
                        array[n, 1] = -1;

                        n--;
                        count++;
                    }
                }

                else { break; }
                
            }

        }

        for (int x = 0; x < 36; x++)
        {
            if (players_cards[x, 0] == -1)
            {
                for (int y = x; y < 36; y++)
                {
                    if (players_cards[y, 0] != -1)
                    {
                        players_cards[x, 0] = players_cards[y, 0];
                        players_cards[x, 1] = players_cards[y, 1];
                        players_cards[y, 0] = -1;
                        players_cards[y, 1] = -1;
                        break;

                    }
                }
            }
        }
    }
}








