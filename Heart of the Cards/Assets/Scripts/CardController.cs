using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardController : MonoBehaviour
{
    List<Card> deck;
    Card[] hand;
    Card[] storage;
    public int numDecks = 1;
    public float discardCooldown = 5f;
    public Text handText;
    public Text valueText;
    public int distanceToNextCard = 65;
    public float projectileCooldown = 2f;
    public bool hasHand = true; 
    public int healAmount = -20; //MUST BE NEGATIVE
    public GameObject player;
    public int damageMultiplier = 2;
    public GameObject enemy;

    bool canDiscard = true;
    float discardCDTimer = 0f;
    //int handValue;
    float timeElapsed = 0;
    Hand currentHand;
    Suite flushSuite;// = Suite.None;
    bool hasFlush = false;
    bool hasDamageBuff = false;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize deck
        deck = new List<Card>();
        FillDeck(deck, numDecks);
        Extensions.Shuffle<Card>(deck);
        hand = new Card[5];
        storage = new Card[3];

        //Deal first hand
        Deal(5, hand);
        //handText.text = printHand(hand);
        printHand(hand);
        currentHand = FindHand(hand);
        print(currentHand);
        UpdateProjectileDamage(currentHand);

        Deal(3, storage);
        for (int i = 0; i < 3; i++) {
            GameObject card = GameObject.Find(storage[i].printCard());
            RectTransform tf = card.GetComponent<RectTransform>();
            tf.localScale -= new Vector3(.3f, .3f, .3f);
        }
        printStorage(storage);

        UpdateHandText();

        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GameObject.FindGameObjectWithTag("Enemy");

        //handValue = AddedValue(hand);
        //valueText.text = "Added Value: " + handValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasHand) {
            timeElapsed += Time.deltaTime;
            if (timeElapsed >= projectileCooldown) {
                for (int i = 0; i < 5; i++) {
                    DealCardAt(i, hand);
                }

                currentHand = FindHand(hand);
                print(currentHand);
                UpdateProjectileDamage(currentHand);
                UpdateHandText();

                hasHand = true;
                timeElapsed = 0;
                printHand(hand);
            } else {
                return;
            }
        }

        //TODO: Make it so that the user can discard more than 1 card at a time
        if (canDiscard) {
            HandleDiscard();
        } else {
            if (discardCDTimer >= discardCooldown) {
                canDiscard = true;
                discardCDTimer = 0;
            } else {
                discardCDTimer += Time.deltaTime;
            }
        }

        if (deck.Count == 0) {
            Debug.Log("Re-shuffling deck");
            FillDeck(deck, numDecks);
            Extensions.Shuffle<Card>(deck);
        }
    }

    //Fills deck with cards (default deck)
    void FillDeck(List<Card> deck, int numDecks)
    {
        //Repeat for number of decks
        for(int i = 0; i < numDecks; ++i)
        {
            //Iterate through suites
            foreach(Suite suite in Suite.GetValues(typeof(Suite))) {
                //Iterate through values
                for (int j = 1; j <= 13; j++)
                {
                    deck.Add(new Card(j, suite));
                }
            }
        }
    }

    //Deals a specified amount of cards
    void Deal(int n, Card[] hand)
    {
        for(int i = 0; i < n; ++i)
        {
            if (deck.Count > 0)
            {
                hand[i] = deck[0];
                deck.RemoveAt(0);
            }
            else
            {
                Debug.Log("Deck empty");
            }
        }
    }

    void DealCardAt(int i, Card[] hand) {
        if (deck.Count == 0) {
            FillDeck(deck, 1);
            Extensions.Shuffle<Card>(deck);
        }

        int j = 0;
        while (Contains(hand, deck[j]) || Contains(storage, deck[j])) {
            j++;
            if (j >= deck.Count) {
                FillDeck(deck, 1);
                Extensions.Shuffle<Card>(deck);
            }
        }

        hand[i] = deck[j];
        deck.RemoveAt(j);
    }

    public void ThrowProjectile() {
        HandleFlush();
        UnPrintHand(hand);
        hasHand = false;
    }

    private void UnPrintHand(Card[] hand) {
        foreach(Card c in hand) {
            GameObject card = GameObject.Find(c.printCard());
            RectTransform tf = card.GetComponent<RectTransform>();
            tf.SetPositionAndRotation(new Vector3(20, -100), tf.rotation);
        }
    }

    //Prints the current hand
    private void printHand(Card[] hand)
    {
        GameObject storageBlock = GameObject.FindGameObjectWithTag("HandTextBG");

        float x = storageBlock.transform.position.x;
        float y = storageBlock.transform.position.y;

        int z = 0;
        foreach(Card c in hand)
        {
            //Debug.Log(c.printCard());
            GameObject card = GameObject.Find(c.printCard());

            Canvas canvas;
            if (!card.TryGetComponent<Canvas>(out canvas)) {
                canvas = card.AddComponent<Canvas>();
                canvas.overrideSorting = true;
            }

            canvas.sortingOrder = z;

            RectTransform tf = card.GetComponent<RectTransform>();
            tf.SetPositionAndRotation(new Vector3(x - 210, y - 115), tf.rotation);
            x += distanceToNextCard;
            z++;
        }
    }

    private void printStorage(Card[] storage) {
        GameObject storageBlock = GameObject.FindGameObjectWithTag("StoredCardsBG");

        float x = storageBlock.transform.position.x;
        float y = storageBlock.transform.position.y;
        int z = 1;

        foreach (Card c in storage) {
            GameObject card = GameObject.Find(c.printCard());

            Canvas canvas;
            if (!card.TryGetComponent<Canvas>(out canvas)) {
                canvas = card.AddComponent<Canvas>();
                canvas.overrideSorting = true;
            }

            canvas.sortingOrder = z;

            RectTransform tf = card.GetComponent<RectTransform>();
            tf.SetPositionAndRotation(new Vector3(x - 65, y - 10), tf.rotation);
            x += distanceToNextCard;
            z++;
        }
    }

    //Handles user input related to discarding cards from your hand
    //TODO: FIX THIS TERRIBLE CODE
    private void HandleDiscard() {
        if (CheckStorage()) {
            return;
        }

        int index = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            index = 0;
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            index = 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            index = 2;
        } else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            index = 3;
        } else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            index = 4;
        }

        if (index != -1) {
            hand[index].discardQueue = !hand[index].discardQueue;
            GameObject card = GameObject.Find(hand[index].printCard());
            RectTransform tf = card.GetComponent<RectTransform>();
            if (hand[index].discardQueue) {
                tf.SetPositionAndRotation(new Vector3(tf.position.x, tf.position.y + 20), tf.rotation);
            } else {
                tf.SetPositionAndRotation(new Vector3(tf.position.x, tf.position.y - 20), tf.rotation);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E)) {
            for (int i = 0; i < 5; i++) {
                if (hand[i].discardQueue) {
                    hand[i].discardQueue = false;
                    Card c = hand[i];
                    GameObject card = GameObject.Find(c.printCard());
                    RectTransform tf = card.GetComponent<RectTransform>();
                    tf.SetPositionAndRotation(new Vector3(20, -100), tf.rotation);
                    DealCardAt(i, hand);
                }
            }

            canDiscard = false;
            printHand(hand);
            
            // finds the value of this hand
            currentHand = FindHand(hand);
            print(currentHand);
            UpdateProjectileDamage(currentHand);

            UpdateHandText();

            /*
            handValue = AddedValue(hand);
            valueText.text = "Added Value: " + handValue;*/
        }
    }

    private bool CheckStorage() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            int toStore = -1;
            int inQueue = 0;
            for (int i = 0; i < 5; i++) {
                if (hand[i].discardQueue) {
                    toStore = i;
                    inQueue++;
                }
            }

            if (toStore == -1 || inQueue != 1) {
                return false;
            }

            int toReplace = -1;
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                toReplace = 0;
            } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
                toReplace = 1;
            } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
                toReplace = 2;
            } else {
                return false;
            }

            Card temp = storage[toReplace];
            storage[toReplace] = hand[toStore];
            hand[toStore] = temp;
            storage[toReplace].discardQueue = false;


            GameObject card = GameObject.Find(storage[toReplace].printCard());
            RectTransform tf = card.GetComponent<RectTransform>();
            tf.localScale -= new Vector3(.3f, .3f, .3f);

            card = GameObject.Find(hand[toStore].printCard());
            tf = card.GetComponent<RectTransform>();
            tf.localScale += new Vector3(.3f, .3f, .3f);

            printHand(hand);
            printStorage(storage); 
            
            currentHand = FindHand(hand);
            print(currentHand);
            UpdateProjectileDamage(currentHand);

            UpdateHandText();

            return true;
        }

        return false;
    }

    void UpdateProjectileDamage(Hand handVal) {
        int damage;
        if (handVal == Hand.HighCard) {
            damage = 5;
        } else if (handVal == Hand.Pair) {
            damage = 10;
        } else if (handVal == Hand.TwoPair) {
            damage = 15;
        } else if (handVal == Hand.ThreeOfAKind) {
            damage = 25;
        } else if (handVal == Hand.Straight) {
            damage = 40;
        } else if (handVal == Hand.Flush) {
            damage = 50;
        } else if (handVal == Hand.FullHouse) {
            damage = 75;
        } else if (handVal == Hand.FourOfAKind) {
            damage = 100;
        } else if (handVal == Hand.StraightFlush) {
            damage = 150;
        } else {
            damage = 200;
        } 

        var projectile = gameObject.GetComponentInChildren<FireProjectile>(); 
        if (hasDamageBuff) {
            hasDamageBuff = false;
            damage *= damageMultiplier;
        }
        projectile.damage = damage;
    }

    void UpdateHandText() {
        handText.text = "Current Hand:\t" + currentHand;
        var projectile = gameObject.GetComponentInChildren<FireProjectile>();
        valueText.text = "Damage:\t" + projectile.damage;
    }

    //Adds up the value of hands
    int AddedValue(Card[] hand) {
        int total = 0;
        foreach (Card c in hand)
        {
            total += c.value;
        }

        return total;
    }

    Hand FindHand(Card[] hand) {
        int[] numPerRank = new int[13];
        for (int i = 0; i < hand.Length; i++) {
            int index = hand[i].value - 2;
            if (index == -1) {
                index = 12;
            }
            numPerRank[index] += 1;
        }

        int pairs = 0;
        bool three = false;
        bool four = false;
        int streak = 0;
        bool straight = false;
        for (int i = 0; i < numPerRank.Length; i++) {
            if (numPerRank[i] == 1) {
                streak++;
                if (streak == 5) {
                    straight = true;
                }
            } else {
                streak = 0;
            }
            if (numPerRank[i] == 2) {
                pairs++;
            } else if (numPerRank[i] == 3) {
                three = true;
            } else if (numPerRank[i] == 4) {
                four = true;
            }
        }
        bool flush = CheckFlush(hand);

        if (straight && flush) {
            hasFlush = true;
            if (HighCard(hand) == 1) {
                return Hand.RoyalFlush;
            } else {
                return Hand.StraightFlush;
            }
        } else if (four) {
            return Hand.FourOfAKind;
        } else if (pairs == 1 && three) {
            return Hand.FullHouse;
        } else if (flush) {
            hasFlush = true;
            return Hand.Flush;
        } else if (straight) {
            return Hand.Straight;
        } else if (three) {
            return Hand.ThreeOfAKind;
        } else if (pairs == 2) {
            return Hand.TwoPair;
        } else if (pairs == 1) {
            return Hand.Pair;
        } else {
            return Hand.HighCard;
        }
    }

    bool Contains(Card[] hand, Card card) {
        for (int i = 0; i < hand.Length; i++) {
            if (hand[i].suite == card.suite && hand[i].value == card.value) {
                return true;
            }
        }
        return false;
    }

    bool CheckFlush(Card[] hand) {
        Suite suite = hand[0].suite;
        for (int i = 1; i < hand.Length; i++) {
            if (hand[i].suite != suite) {
                return false;
            }
        }
        flushSuite = suite;
        return true;
    }

    int HighCard(Card[] hand) {
        int max = -1;
        for (int i = 0; i < hand.Length; i++) {
            if (hand[i].value == 1) {
                return 1;
            }
            if (hand[i].value > max) {
                max = hand[i].value;
            }
        }
        return max;
    }

    void HandleFlush() {
        if (hasFlush) {
            switch (flushSuite) {
                // case Suite.None:
                //return; //Does nothing <3
                case Suite.Heart:
                    player.GetComponent<PlayerHealth>().TakeDamage(healAmount);
                    //flushSuite = Suite.None;
                    break;
                case Suite.Diamond:
                    player.GetComponent<PlayerHealth>().hasArmor = true;
                    // flushSuite = Suite.None;
                    break;
                case Suite.Club:
                    enemy.GetComponent<EnemyAttacks>().Stun();
                    // flushSuite = Suite.None;
                    break;
                case Suite.Spade:
                    hasDamageBuff = true;
                    // flushSuite = Suite.None;
                    break;
            }
            hasFlush = false;
        }
    }
}

//Enum representing the suite of a card
public enum Suite {
    Diamond, Heart, Spade, Club
}

//Enum representing types of hands
public enum Hand {
    HighCard, Pair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush
}

//Class representing an individual card
public class Card
{
    public int value;
    public Suite suite;
    public bool discardQueue;

    //Constructor
    public Card(int v, Suite s)
    {
        value = v;
        suite = s;
        discardQueue = false;
    }

    //Prints a card as "value of suite"
    public string printCard()
    {
        return suite + value.ToString();
    }
}

static class Extensions
{
    //Shuffle method based on System.Random that randomizes the order of a list
    //sourced from https://stackoverflow.com/a/1262619
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
