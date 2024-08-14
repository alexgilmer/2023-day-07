using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2023_day_07;

public class PokerHand : IComparable<PokerHand>
{
    private static Dictionary<char, int> CardRanks = new()
    {
        {'J', 1 },
        {'2', 2 },
        {'3', 3 },
        {'4', 4 },
        {'5', 5 },
        {'6', 6 },
        {'7', 7 },
        {'8', 8 },
        {'9', 9 },
        {'T', 10 },
        {'Q', 11 },
        {'K', 12 },
        {'A', 13 },

    };
    private PokerHandRank? _rank = null;
    private long _cardValue = -1;
    public string Cards { get; set; }
    public int Bid { get; set; }

    public PokerHandRank PokerHandRank
    {
        get
        {
            if (_rank != null)
                return (PokerHandRank)_rank;

            _rank = EvaluateHand(this);
            return (PokerHandRank)_rank;
        }
    }
    public long CardValue
    {
        get
        {
            if (_cardValue != -1)
                return _cardValue;

            _cardValue = EvaluateCards(this);
            return _cardValue;
        }
    }

    private static long EvaluateCards(PokerHand hand)
    {
        long result = 0;
        for (int i = 0; i < hand.Cards.Length; i++)
        {
            int exponent = hand.Cards.Length - i - 1;
            int @base = CardRanks.Count;
            int value = CardRanks[hand.Cards[i]];

            result += value * (long)Math.Pow(@base, exponent);
        }
        return result;
    }

    private static PokerHandRank EvaluateHand(PokerHand hand)
    {
        Dictionary<char, int> counts = [];
        foreach(char l in hand.Cards)
        {
            counts.TryAdd(l, 0);
            counts[l]++;
        }

        // joker rule addition
        int jokers = 0;
        if (counts.Count > 1 && counts.TryGetValue('J', out int value))
        {
            jokers = value;
            counts.Remove('J');
        }

        List<int> groups = counts.Select(kvp => kvp.Value).OrderDescending().ToList();
        if (jokers > 0)
        {
            groups[0] += jokers;
        }

        switch (groups.Count)
        {
            case 1:
                return PokerHandRank.FiveOfKind;
            case 2:
                //4-1 or 3-2
                if (groups[0] == 4)
                    return PokerHandRank.FourOfKind;
                return PokerHandRank.FullHouse;
            case 3:
                //3-1-1, 2-2-1
                if (groups[0] == 3)
                    return PokerHandRank.ThreeOfKind;
                return PokerHandRank.TwoPair;
            case 4:
                return PokerHandRank.OnePair;
            case 5:
                return PokerHandRank.HighCard;
            default:
                throw new InvalidOperationException("Invalid hand count: " + groups.Count);
        }
    }

    public int CompareTo(PokerHand? other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (this.PokerHandRank != other.PokerHandRank)
        {
            return (int)this.PokerHandRank - (int)other.PokerHandRank;
        }

        return (int)(this.CardValue - other.CardValue);
    }

    private int CompareCards(PokerHand other)
    {
        for (int i = 0; i < Cards.Length; i++)
        {
            if (Cards[i] != other.Cards[i])
            {
                return CardRanks[Cards[i]] - CardRanks[other.Cards[i]];
            }
        }
        return 0;
    }
}

public enum PokerHandRank
{
    HighCard = 1,
    OnePair = 2,
    TwoPair = 3,
    ThreeOfKind = 4,
    FullHouse = 5,
    FourOfKind = 6,
    FiveOfKind = 7
}
