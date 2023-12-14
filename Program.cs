using System.Text;
using System.Threading.Channels;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

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
    bool traverseRows = (int)dirrection>=2;//traverse Rows or Cols
    bool forward = (int)dirrection%2==1;//traverse forwards or backwards

    int xMax = traverseRows ? map.Count : map[0].Count;
    int yMax = traverseRows ? map[0].Count : map.Count;

    bool changed;
    do
    {
        changed = false;
        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y+1 < yMax; y++)
            {
                int i = traverseRows ? forward ? x : xMax - x - 1 : forward ? y : yMax - y - 1;
                int j = traverseRows ? forward ? y : yMax - y - 1 : forward ? x : xMax - x - 1;
                int i2 = traverseRows ? forward ? x : xMax - x - 1 : forward ? y + 1 : yMax - (y + 1) - 1;
                int j2 = traverseRows ? forward ? y + 1 : yMax - (y + 1) - 1 : forward ? x : xMax - x - 1;
                if (map[i][j] == 'O' && map[i2][j2] == '.')
                {
                    map[i][j] = '.';
                    map[i2][j2] = 'O';
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
enum Cardinal
{
    N = 0,
    S = 1,
    W = 2,
    E = 3,
}

