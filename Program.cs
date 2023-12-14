using System.Diagnostics;
using System.Text;

const int part2desiredIterations = 1_000_000_000;
List<string> lines = new();
using (StreamReader reader = new(args[0]))
{
    while (!reader.EndOfStream)
    {
        lines.Add(reader.ReadLine());
    }
}


Stopwatch sw = Stopwatch.StartNew();
int p1 = solve(false);
sw.Stop();
Console.WriteLine($"part1:\t\ttime:{sw.ElapsedMilliseconds}ms");

sw.Restart();
int p2 = solve(true);
sw.Stop();
Console.WriteLine($"part2:\t\ttime:{sw.ElapsedMilliseconds}ms");

Console.WriteLine($"part1: \t\t{p1}");
Console.WriteLine($"part2: \t\t{p2}");

int solve(bool part2)
{
    char[][] map = new char[lines.Count][];
    for (int i = 0; i < lines.Count; i++)
    {
        map[i] = new char[lines[i].Length];
        for (int j = 0; j < lines[i].Length; j++)
        {
            map[i][j]= lines[i][j];
        }
    }

    if (part2)
    {
        Dictionary<string, int> rockFormations = new();
        for (int iteration = 1; iteration <= part2desiredIterations; iteration++)
        {
            StringBuilder sb = new();
            foreach(var line in map)
                    sb.Append(line);
            string formationString = sb.ToString();
            if (rockFormations.ContainsKey(formationString))
            {
                int cycle = iteration - rockFormations[formationString];
                int moveForward = cycle * ((part2desiredIterations - iteration) / cycle);
                iteration += moveForward;
            }
            rockFormations[formationString] = iteration;
            moveRocks(ref map, Cardinal.N);
            moveRocks(ref map, Cardinal.W);
            moveRocks(ref map, Cardinal.S);
            moveRocks(ref map, Cardinal.E);
        }
    }
    else //part1
        moveRocks(ref map, Cardinal.N);
    return totalLoad(map);
}

void moveRocks(ref char[][]map, Cardinal dirrection)
{
    bool traverseRows = (int)dirrection >= 2;//traverse Rows or Cols
    bool forward = (int)dirrection % 2 == 1;//traverse forwards or backwards

    int xMax = traverseRows ? map.Length : map[0].Length;
    int yMax = traverseRows ? map[0].Length : map.Length;

    for (int x = 0; x < xMax; x++)
    {
        int boulderCount = 0;
        Stack<Tuple<int, int>> visited = new();
        for (int y = 0; y <= yMax; y++)
        {
            int i = traverseRows ? forward ? x : xMax - x - 1 : forward ? y : yMax - y - 1;
            int j = traverseRows ? forward ? y : yMax - y - 1 : forward ? x : xMax - x - 1;
            visited.Push(new(i, j));
            switch (y==yMax?'#':map[i][j])
            {
                case 'O':
                    map[i][j] = '.';
                    boulderCount++;
                    break;
                case '.':
                    break;
                case '#':
                    visited.Pop();
                    for (; boulderCount > 0;--boulderCount)
                        map[visited.Peek().Item1][visited.Pop().Item2] = 'O';
                    break;
            }
        }
    }
}

int totalLoad(in char[][] map)
{
    int curr = 0;
    for (int i = 0; i < map.Length; i++)
    {
        for (int j = 0; j < map[i].Length; j++)
        {
            if (map[i][j] == 'O')
                curr += map.Length - i;
        }
    }
    return curr;
}
enum Cardinal
{
    N = 0,
    S = 1,
    W = 2,
    E = 3,
}

