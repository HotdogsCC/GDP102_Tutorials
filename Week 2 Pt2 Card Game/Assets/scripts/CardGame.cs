using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class CardGame : MonoBehaviour
{
    public int[] deck;
    public GameObject[] cardSlots;
    public List<Sprite> cardSprites;

    int deckIndex = 0;
    bool inGame = true;
    public int Bank = 1000;
    public int Pot = 0;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI potText;
    public TextMeshProUGUI bankText;
    public int maxContribution = 50;
    int playNumber;
    int extra;
    const int numberCardsInSuit = 13;
    const int numberSuits = 4;
    const int totalCards = numberSuits * numberCardsInSuit;
    // Start is called before the first frame update
    void Start()
    {
        LoadCardTextures();
        createDeck();
        resetTouranament();
    }

    // Update is called once per frame
    void Update()
    {
        bankText.text = "Bank: " + Bank.ToString();
        potText.text = "Pot: " + Pot.ToString();
        if (inGame)
        {
            playGame();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                resetGame();
            }
        }
    }

    // reset the whole tournament
    void resetTouranament()
    {
        deckIndex = 0;
        Bank = 1000;
        Pot = 0;
        shuffle();
        resetGame();
    }

    // reset an individual game
    void resetGame()
    {
        // If we're about to run out of cards then reshuffle and start again
        if (deckIndex >= totalCards-5) 
        {
            deckIndex = 0;
            shuffle();
        }
        // clear the graphics for the cards on the screen
        foreach (var card in cardSlots)
        {
            card.GetComponent<SpriteRenderer>().sprite = null;
        }
        playNumber = 0;
        // deal our first card, 0 indicates this can't lose
        dealCard(0);
        feedbackText.text = "New Game! Choose Hi or Low";
        inGame = true; // set our state so we know we are in a game
        Bank -= 10; // take 10 off the bank and stick it in the pot
        // note: Currently we don't check if the bank is empty.  We should quite the game if it is
        Pot = 10;
        extra = 0;
    }
    void playGame()
        {


        // to add AI you need to add code to replace the code to read the keyboard with suitable AI heuristics.
        // e.g. instenad of checking for right arrow you might increase the stake if you think there is a good chance of winning
        // You can get the points value of the last dealt card using the following function getCardValue(deck[deckIndex]);
        // Press right arrow to to increase the bid
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Bank > 0 && (extra < maxContribution))
            {
                Bank -= 10;
                Pot += 10;
                extra += 10;
            }
        }
        int direction = 0;
        // up arrow to bet on the next card being higher
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = 1;
        }

        // down arrow to bet on the next card being lower
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = -1;
        }

        // If we have picked a direction then deal the next card and check if we have won
        if (direction != 0)
        {
            if (dealCard(direction))
            {
                // if we won the bet then see if this is the last card in the hand
                if(playNumber == 5)
                {
                    feedbackText.text = "You Win! Space to play again";
                    Bank += (2*Pot); // If we win the hand then get the pot back times 2
                    Pot = 0;
                    inGame = false;
                }
                else
                {
                    feedbackText.text = "Good Job! Choose Hi or Low?";
                    extra = 0;
                }

            }
            else
            {
                // if we lose the bet then we lose everything and the next hand begins
                feedbackText.text = "You Lose! Space to play again";
                Pot = 0;
                inGame = false;
            }
        }

    }

    // deal a card and return true if the player guessed correctly if it's high or low
    bool dealCard(int direction)
    {
        bool result = true;
        int thisCardIDX = deck[deckIndex];
        cardSlots[playNumber].GetComponent<SpriteRenderer>().sprite = cardSprites[thisCardIDX];

        if (direction != 0)
        {
            int previousCardIDX = deck[deckIndex - 1];
            int previousFaceValue = getCardValue(previousCardIDX);
            int newFaceValue = getCardValue(thisCardIDX);
            if ( (newFaceValue != 1) && (previousFaceValue != 1) ) // if it's an Ace or the previous card was an Ace, then wins because Ace is high and low
            {
                int delta = newFaceValue - previousFaceValue;  //0 means the values are the same , > 0 new card is greater, < 0 new card is lower
                if ((direction * delta) < 0)
                {
                    result = false; // if cards are not equal and we've chosen wrong:
                }
            }
        }
        deckIndex++; // Increase the deal number for next time
        playNumber++; // Increase the play number
        return result;
    }

    int getCardValue(int cardIndex)
    {
        return (cardIndex % numberCardsInSuit) + 1;
    }

    void createDeck()
    {
        deck = new int[totalCards];
        for (int cardIdx = 0; cardIdx < totalCards; cardIdx++)
        {
            deck[cardIdx] = cardIdx;
        }
    }

    void shuffle()
    {
        int shuffleIterations = 255;
        for(int i = 0; i < shuffleIterations; i++)
        {
            int randomPos1 = Random.Range(0, totalCards);
            int randomPos2 = Random.Range(0, totalCards);

            var temp = deck[randomPos1];
            deck[randomPos1] = deck[randomPos2];
            deck[randomPos2] = temp;
        }
    }

    // loads all the card textures into an array
    void LoadCardTextures()
    {
        // get list ready for the card textures
        cardSprites = new List<Sprite>();
        // iterate over all the suits
        for (int suit = 0; suit < numberSuits; suit++)
        {
            // itterate over all the cards in the suit
            for (int cardNumber = 1; cardNumber <= numberCardsInSuit; cardNumber++)
            {
                // path is relative to the Unity resource folder in assets
                string cardFileName = "cardTextures/";
                // create the file name so we can load the correct card by name
                switch (suit)
                {
                    case 0:
                        cardFileName += "Club";
                        break;
                    case 1:
                        cardFileName += "Diamond";
                        break;
                    case 2:
                        cardFileName += "Heart";
                        break;
                    default:
                        cardFileName += "Spade";
                        break;
                }

                switch (cardNumber)
                {
                    case 1:
                        cardFileName += "A";
                        break;
                    case 11:
                        cardFileName += "J";
                        break;
                    case 12:
                        cardFileName += "Q";
                        break;

                    case 13:
                        cardFileName += "K";
                        break;
                    default:
                        cardFileName += cardNumber.ToString();
                        break;
                }

                // load the card textures
                var texture = Resources.Load<Sprite>(cardFileName);
                if(texture == null)
                {
                    Debug.Log("Failed to load card texture: "+cardFileName);
                }
                cardSprites.Add(texture); // add reference to texture to the list
            }
        }
    }
}
