using System;
class VideoPoker {
    static long currency = 100000;  
    static long bet = 0;
    static int[] deck = new int[52]; //Карти мають вигляд 3-цифрового числа, де перша цифра - масть, а дві інші -
    static int[] table = new int [5];  // значення. 1 - треф, 2 - піки, 3 - буба, 4 - чирва. 01-туз, 02-10 цифри, 11,12,13 - валет, дама, король
    static int[] newTable = new int[5];
    static string holds;
  static void Main() {
      while(true){
          while(true){
     Console.WriteLine($"На балансі:{currency}. Зробіть ставку");
     long.TryParse(Console.ReadLine(), out bet);
     if(bet < 1){
         Console.WriteLine("Введіть додатнє число");
     }
     else if(bet > currency){
         Console.WriteLine("Ставка перевищує кількість грошей");
     }
     else{
         break;
     }
   }
     currency -= bet;
    Generate();
    Shuffle();
    Play();
    Analyze();
      }
  }
  static void Play(){
      for(int i = 0; i<5;i++){
          table[i] = deck[i];
          newTable[i] = deck[i+5];
      }
      
      Print(table);
      
      while(true){
          
      Console.WriteLine("\n Введіть порядкові номери карт, які хочете тримати, у вигляді одного числа");
      holds = Console.ReadLine(); 
      bool isProblem = false;
      
      for(int i = 0; i < holds.Length; i++){
          if(holds[i] != '1' && holds[i] != '2' && holds[i] != '3' && holds[i] != '4' && holds[i] != '5'){
              Console.WriteLine("Код може містити тільки цифри від 1 до 5");
              isProblem = true;
              break;
          }
      }
      
      if(!isProblem){
          break;
      }
    }
    for(int i = 0; i < holds.Length; i++){
        newTable[(int)holds[i] - '1'] = table[(int)holds[i] - '1'];
    }
    Print(newTable);
  }
 static void Generate(){
      int index = 0;
      for(int i = 1; i<5; i++){ //генерує масть 
          for(int j = 1; j<14; j++){ //генерує значення
              deck[index] = j+i*100;
              index++;
          }
      }
  }
  static void Shuffle(){
      Random rand = new Random();
      for(int i = 0; i<2000; i++){
        int index1 = rand.Next(52);
        int index2 = rand.Next(52);
        int savei1 = deck[index1];
        deck[index1] = deck[index2];
        deck[index2] = savei1;
      }
  }
  static void Print(int[] array){
      for(int i = 0; i<5; i++){
          int card = array[i];
          
          if(card%100 < 11 && card%100 > 1){
              Console.Write(card%100);
          }
          else if(card%100 == 1){
              Console.Write("A");
          }
          else if(card%100 == 11){
              Console.Write("J");
          }
          else if(card%100 == 12){
              Console.Write("Q");
          }
          else if(card%100 == 13){
              Console.Write("K");
          }
          
          if(card/100 == 1){
              Console.Write("♣");
          }
         else if(card/100 == 2){
              Console.Write("♠");
          }
           else if(card/100 == 3){
              Console.Write("♦");
          }
           else if(card/100 == 4){
              Console.Write("♥");
          }
          Console.Write(" ");
      }
  }
  static void Analyze(){
      bool isStraight = false;
      bool isFlush = false;
      bool isJacksOrBetter = false;
      bool isPair = false; // ця пара використовується при фулл хаусі
      bool isTwoPair = false;
      bool isSet = false;
      bool isFour = false;
      bool mayRoyal = false;
      int nOfPairs = 0;
      int[] values = new int[5];
      int[] pairs = new int[13]; //  цифра 1 в комірці означає, що певна карта парується
      int suit0 = newTable[0]/100;
      int suit1 = newTable[1]/100;
      int suit2 = newTable[2]/100;
      int suit3 = newTable[3]/100;
      int suit4 = newTable[4]/100;
       //перевірка на флеш
      if(suit0 == suit1 && suit0 == suit2 && suit0 == suit3 && suit0 == suit4){
          isFlush = true;
      }
      // створюємо масив лише зі значеннями і сортуємо
      for(int i = 0; i<5; i++){
          values[i] = newTable[i]%100;
      }
      Array.Sort(values);
      
      if(values[0] == 1 && values[1] == 10 && values[2] == 11 && values[3] == 12 && values[4] == 13){
          isStraight = true; // перевірка на стріт (10,В,Д,К,Т)
          mayRoyal = true;  
      }
      else{  //перевірка на інші стріти
          int collisions = 0;
      for(int i = 0; i<4; i++){
          if(values[i] == values[i+1]+1){
              collisions++;
          }
       }
       if(collisions == 4){
           isStraight = true;
       }
      }
        // перевіряємо кількість карт усіх видів в роздачі
      for(int i = 1; i<14; i++){
          int collisions = 0;
          for(int j = 0; j<5; j++){
             if(values[j] == i){
                 collisions++;
             }
          }
          if(collisions == 2){ //реєструємо пари
              pairs[i-1] = 1;
          }
          else if(collisions == 3){ //сет
              isSet = true;
          }
          else if(collisions == 4){ //каре
             isFour = true;
          }
      }
      //рахуємо кількість пар
      for(int i = 0; i<13; i++){
          nOfPairs += pairs[i];
      }
      
      if(nOfPairs == 2){
          isTwoPair = true;
      }
      else if(nOfPairs == 1){ 
          isPair = true;
         if(pairs[0] == 1 || pairs[10] == 1 || pairs[11] == 1 || pairs[12] == 1){ //якщо пара одна, перевіряємо чи вона старша
          isJacksOrBetter = true;
       }
     }
      
      if(isStraight && isFlush && mayRoyal){
          Console.WriteLine("!РОЯЛ ФЛЕШ!");
          currency += bet * 250;
      }
      else if(isFlush && isStraight){
          Console.WriteLine("!Стріт Флеш!");
          currency += bet * 50;
      }
      else if(isFour){
          Console.WriteLine("КАРЕ");
          currency += bet * 25;
      }
      else if(isSet && isPair){
          Console.WriteLine("Фулл Хаус");
          currency += bet * 9;
      }
      else if(isFlush){
          Console.WriteLine("Флеш");
          currency += bet * 6;
      }
      else if(isStraight){
          Console.WriteLine("Стріт");
          currency += bet * 4;
      }
      else if(isSet){
          Console.WriteLine("Три однакові");
          currency += bet * 3;
      }
      else if(isTwoPair){
          Console.WriteLine("Дві пари");
          currency += bet * 2;
      }
      else if(isJacksOrBetter){
          Console.WriteLine("Пара старших карт");
          currency += bet;
      }
      else{
          Console.WriteLine("Програш, спробуйте знову");
      }
  }
}
