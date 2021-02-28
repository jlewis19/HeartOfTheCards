using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardController : MonoBehaviour
{
    List<Card> deck;
    List<Card> hand;
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

        //Deal first hand
        hand = new List<Card>();
        hand.Capacity = 5;
        Deal(5);
        handText.text = printHand(hand);
        handValue = AddedValue(hand);
        valueText.text = "Added Value: " + handValue;
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
                foreach(Value value in Value.GetValues(typeof(Value)))
                {
                    deck.Add(new Card(value, suite));
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
                hand.Add(deck[0]);
                deck.RemoveAt(0);
            }
            else
            {
                Debug.Log("Deck empty");
            }
        }
    }

    //Prints the current hand
    private string printHand(List<Card> hand)
    {
        string result = "";

        foreach(Card c in hand)
        {
            result += c.printCard() + " ";
        }

        return result;
    }

    //Handles user input related to discarding cards from your hand
    //TODO: FIX THIS TERRIBLE CODE
    private void HandleDiscard()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hand.RemoveAt(0);
            Deal(1);
            canDiscard = false;
            handText.text = printHand(hand);
            handValue = AddedValue(hand);
            valueText.text = "Added Value: " + handValue;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hand.RemoveAt(1);
            Deal(1);
            canDiscard = false;
            handText.text = printHand(hand);
            handValue = AddedValue(hand);
            valueText.text = "Added Value: " + handValue;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hand.RemoveAt(2);
            Deal(1);
            canDiscard = false;
            handText.text = printHand(hand);
            handValue = AddedValue(hand);
            valueText.text = "Added Value: " + handValue;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            hand.RemoveAt(3);
            Deal(1);
            canDiscard = false;
            handText.text = printHand(hand);
            handValue = AddedValue(hand);
            valueText.text = "Added Value: " + handValue;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            hand.RemoveAt(4);
            Deal(1);
            canDiscard = false;
            handText.text = printHand(hand);
            handValue = AddedValue(hand);
            valueText.text = "Added Value: " + handValue;
        }

    }

    //Adds up the value of hands
    //Faces and Ace: J = 11, Q = 12, K = 13, A = 14
    int AddedValue(List<Card> hand)
    {
        int total = 0;
        foreach (Card c in hand)
        {
            var value = c.value;
            switch (value)
            {
                case Value.Ace:
                    total += 14;
                    break;
                case Value.Two:
                    total += 2;
                    break;
                case Value.Three:
                    total += 3;
                    break;
                case Value.Four:
                    total += 4;
                    break;
                case Value.Five:
                    total += 5;
                    break;
                case Value.Six:
                    total += 6;
                    break;
                case Value.Seven:
                    total += 7;
                    break;
                case Value.Eight:
                    total += 8;
                    break;
                case Value.Nine:
                    total += 9;
                    break;
                case Value.Ten:
                    total += 10;
                    break;
                case Value.Jack:
                    total += 11;
                    break;
                case Value.Queen:
                    total += 12;
                    break;
                case Value.King:
                    total += 13;
                    break;
                default:
                    total += 0;
                    break;
            }
        }

        return total;
    }
}

//Enum representing the value of a card
public enum Value
{
    Ace, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
}

//Enum representing the suite of a card
public enum Suite
{
    Diamonds, Hearts, Spades, Clubs
}

//Class representing an individual card
public class Card
{
    public Value value;
    public Suite suite;

    //Constructor
    public Card(Value v, Suite s)
    {
        value = v;
        suite = s;
    }

    //Prints a card as "value of suite"
    public string printCard()
    {
        return value + " of " + suite;
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
