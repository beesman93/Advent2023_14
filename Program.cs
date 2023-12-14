List<string> lines = new();
using (StreamReader reader = new(args[0]))
{
    while (!reader.EndOfStream)
    {
        lines.Add(reader.ReadLine());
    }
}

const int letRocksSettleFor = 1_000;
const int maxCycleDetect = 1_000;
const int cycles = 1_000_000_000;


solve(true);
solve(false);
void solve(bool part2)
{
    List<List<char>> map = new();
    foreach (string line in lines)
    {
        map.Add(new());
        foreach (char c in line)
        {
            map.Last().Add(c);   
        }
    }

    if (part2)
    {
        int cycleConfidence = 0;
        int potentialCycle = -1;
        int[] cycleDetector = new int[maxCycleDetect];

        for (int cycle = 1; cycle <= cycles; cycle++)
        {
            if (cycle < letRocksSettleFor)
                _ = cycle_2(ref map);
            else if (cycle < letRocksSettleFor + maxCycleDetect)
            {
                int curr = cycle_2(ref map);
                cycleDetector[cycle - letRocksSettleFor] = curr;
                if (potentialCycle > 0)
                {
                    if (curr == cycleDetector[cycle - letRocksSettleFor - potentialCycle])
                    {
                        cycleConfidence++;
                        if (cycleConfidence == potentialCycle)
                        {
                            //ladies and gentlemen, we got him - just skip to end
                            while (cycle <= cycles)
                                cycle += potentialCycle;
                            cycle -= potentialCycle;//give room for the last couple
                        }
                    }
                    else
                    {
                        potentialCycle = -1;//false positive, find another one
                    }
                }
                if (cycle - letRocksSettleFor > 0)
                    if (curr == cycleDetector[0])
                        potentialCycle = cycle - letRocksSettleFor;
            }
            else
            {
                int curr = cycle_2(ref map);
                if (potentialCycle < 0)
                    throw new Exception("No cycle found, increase the settle rocks or cycle detect or give up");
                else
                {
                    //Console.WriteLine($"{cycle}: \t\t{curr}");
                    if(cycle==cycles)
                        Console.WriteLine($"part2: \t\t{curr}");
                }
            }
        }
    }
    else //part1
    {
        int changeCount = -1;
        while (changeCount != 0)
        {
            changeCount = 0;
            for (int i = 1; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    if (map[i][j] == 'O' && map[i - 1][j] == '.')
                    {
                        map[i][j] = '.';
                        map[i - 1][j] = 'O';
                        changeCount++;
                    }
                }
            }

        }
        int curr = 0;
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == 'O')
                    curr += map.Count - i;
            }
        }
        Console.WriteLine($"part1: \t\t{curr}");
    }

}

int cycle_2(ref List<List<char>> map)
{
    //north, then west, then south, then east
    int changeCount = -1;
    while (changeCount != 0)
    {
        changeCount = 0;
        for (int i = 1; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == 'O' && map[i - 1][j] == '.')
                {
                    map[i][j] = '.';
                    map[i - 1][j] = 'O';
                    changeCount++;
                }
            }
        }

    }

    changeCount = -1;
    while (changeCount != 0)
    {
        changeCount = 0;
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 1; j < map[i].Count; j++)
            {
                if (map[i][j] == 'O' && map[i][j - 1] == '.')
                {
                    map[i][j] = '.';
                    map[i][j - 1] = 'O';
                    changeCount++;
                }
            }
        }

    }

    changeCount = -1;
    while (changeCount != 0)
    {
        changeCount = 0;
        for (int i = 0; i + 1 < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == 'O' && map[i + 1][j] == '.')
                {
                    map[i][j] = '.';
                    map[i + 1][j] = 'O';
                    changeCount++;
                }
            }
        }

    }

    changeCount = -1;
    while (changeCount != 0)
    {
        changeCount = 0;
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j + 1 < map[i].Count; j++)
            {
                if (map[i][j] == 'O' && map[i][j + 1] == '.')
                {
                    map[i][j] = '.';
                    map[i][j + 1] = 'O';
                    changeCount++;
                }
            }
        }

    }
    int curr = 0;
    for (int i = 0; i < map.Count; i++)
    {
        for (int j = 0; j < map[i].Count; j++)
        {
            if (map[i][j] == 'O')
                curr += map.Count - i;
        }
    }
    return curr;
}

