using System.Text;
using System.Transactions;

List<string> lines = new();
using (StreamReader reader = new(args[0]))
{
    while (!reader.EndOfStream)
    {
        lines.Add(reader.ReadLine());
    }
}

const int iterations = 1_000_000_000;

solve(false);
solve(true);
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
        Dictionary<string, int> rockFormations = new();
        for (int iteration = 1; iteration <= iterations; iteration++)
        {
            StringBuilder sb = new();
            foreach(var line in map)
                foreach(char c in line)
                    sb.Append(c);
            string formationString = sb.ToString();
            if (rockFormations.ContainsKey(formationString))
            {
                int cycle = iteration - rockFormations[formationString];
                int moveForward = cycle * ((iterations - iteration) / cycle);
                iteration += moveForward;
            }
            rockFormations[formationString] = iteration;
            moveRocks(ref map, Cardinal.N);
            moveRocks(ref map, Cardinal.W);
            moveRocks(ref map, Cardinal.S);
            moveRocks(ref map, Cardinal.E);
        }
        Console.WriteLine($"part2: \t\t{totalLoad(map)}");
    }
    else //part1
    {
        moveRocks(ref map, Cardinal.N);
        Console.WriteLine($"part1: \t\t{totalLoad(map)}");
    }

}
void moveRocks(ref List<List<char>> map,Cardinal dirrection)
{
    //north, then west, then south, then east
    int iMove = 0;
    int jMove = 0;
    switch (dirrection)
    {
        case Cardinal.N: iMove--; break;
        case Cardinal.S: iMove++; break;
        case Cardinal.W: jMove--; break;
        case Cardinal.E: jMove++; break;
    }
    bool changed;
    do
    {
        changed = false;
        for (int i = 0; i < map.Count; i++)
        {
            if (i + iMove < 0 || i + iMove >= map.Count) continue;
            for (int j = 0; j < map[i].Count; j++)
            {
                if(j + jMove < 0 || j + jMove >= map[i].Count) continue;
                if (map[i][j] == 'O' && map[i + iMove][j + jMove] == '.')
                {
                    map[i][j] = '.';
                    map[i + iMove][j + jMove] = 'O';
                    changed = true;
                }
            }
        }

    } while (changed);
}

int totalLoad(in List<List<char>> map)
{
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
enum Cardinal { N, S, W, E }

