using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardController : MonoBehaviour
{
    List<Card> deck;
    Card[] hand;
    public int numDecks = 1;
    public float discardCooldown = 5f;
    public Text handText;
    public Text valueText;

    bool canDiscard = true;
    float discardCDTimer = 0f;
    int handValue;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize deck
        deck = new List<Card>();
        FillDeck(deck, numDecks);
        Extensions.Shuffle<Card>(deck);
        hand = new Card[5];

        //Deal first hand
        Deal(5);
        //handText.text = printHand(hand);
        printHand(hand);
        handValue = AddedValue(hand);
        valueText.text = "Added Value: " + handValue;

        var projectile = gameObject.GetComponentInChildren<FireProjectile>();
        projectile.damage = handValue;
        //Prints out entire deck
        //foreach (Card c in deck)
        //{
        //    Debug.Log(c.printCard());
        //}

        //Prints out hand
        //foreach(Card c in hand)
        //{
        //    Debug.Log(c.printCard());
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Make it so that the user can discard more than 1 card at a time
        if (canDiscard)
        {
            HandleDiscard();
        } else
        {
            if (discardCDTimer >= discardCooldown)
            {
                canDiscard = true;
                discardCDTimer = 0;
            } else
            {
                discardCDTimer += Time.deltaTime;
            }
        }

        if (deck.Count == 0)
        {
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
                for (int j = 1; j < 13; j++)
                {
                    deck.Add(new Card(j, suite));
                }
            }
        }
    }

    //Deals a specified amount of cards
    void Deal(int n)
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

    //Prints the current hand
    private void printHand(Card[] hand)
    {
        int x = 290;
        foreach(Card c in hand)
        {
            //Debug.Log(c.printCard());
            GameObject card = GameObject.Find(c.printCard());
            RectTransform tf = card.GetComponent<RectTransform>();
            tf.SetPositionAndRotation(new Vector3(300 + x, 50), tf.rotation);
            x += 65;
        }
    }

    //Handles user input related to discarding cards from your hand
    //TODO: FIX THIS TERRIBLE CODE
    private void HandleDiscard()
    {
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
                tf.SetPositionAndRotation(new Vector3(tf.position.x, 30), tf.rotation);
            } else {
                tf.SetPositionAndRotation(new Vector3(tf.position.x, 50), tf.rotation);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E)) {
            for (int i = 0; i < 5; i++) {
                if (hand[i].discardQueue) {
                    Card c = hand[i];
                    hand[i] = deck[0];
                    GameObject card = GameObject.Find(c.printCard());
                    RectTransform tf = card.GetComponent<RectTransform>();
                    tf.SetPositionAndRotation(new Vector3(20, -100), tf.rotation);
                    deck.RemoveAt(0);
                }
            }

            canDiscard = false;
            printHand(hand);
            handValue = AddedValue(hand);
            valueText.text = "Added Value: " + handValue;
            var projectile = gameObject.GetComponentInChildren<FireProjectile>();
            projectile.damage = handValue;
        }
    }

    //Adds up the value of hands
    int AddedValue(Card[] hand)
    {
        int total = 0;
        foreach (Card c in hand)
        {
            total += c.value;
        }

        return total;
    }
}

//Enum representing the suite of a card
public enum Suite
{
    Diamond, Heart, Spade, Club
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
