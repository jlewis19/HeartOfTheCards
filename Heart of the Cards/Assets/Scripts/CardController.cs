using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
public class CardController : MonoBehaviour
{
    List<Card> deck;
    List<Card> hand;
    public int numDecks = 1;
    public Text handText;
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
        //Prints out entire deck
        //foreach (Card c in deck)
        //{
        //    Debug.Log(c.printCard());
        //}

        //Prints out hand
        foreach(Card c in hand)
        {
            Debug.Log(c.printCard());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
