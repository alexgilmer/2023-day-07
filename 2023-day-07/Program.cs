namespace _2023_day_07;

internal class Program
{
    static void Main(string[] args)
    {
        bool testMode = false;

        IList<string> inputs = testMode ? GetTestInput() : GetPuzzleInput();

        List<PokerHand> hands = inputs.Select(s =>
            {
                string[] parts = s.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                PokerHand hand = new PokerHand()
                {
                    Cards = parts[0],
                    Bid = int.Parse(parts[1])
                };
                return hand;
            })
            .Order()
            .ToList();

        long result = 0;
        for (int i = 0; i < hands.Count; i++)
        {
            int rank = i + 1;
            result += rank * hands[i].Bid;
        }

        Console.WriteLine(result);
    }

    static IList<string> GetPuzzleInput()
    {
        string file = Path.Combine(Environment.CurrentDirectory, "puzzle-input.txt");
        using StreamReader sr = new StreamReader(file);
        List<string> input = [];

        while (!sr.EndOfStream)
        {
            input.Add(sr.ReadLine()!);
        }

        return input;
    }

    static IList<string> GetTestInput()
    {
        return [
            "32T3K 765",
            "T55J5 684",
            "KK677 28",
            "KTJJT 220",
            "QQQJA 483"
            ];
    }
}
