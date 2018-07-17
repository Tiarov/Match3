using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Match3Engine
{
    public class Match3Table
    {
        public Match3Table(int x, int y, int variableCount)
        {
            XCount = x;
            YCount = y;
            VariableCount = variableCount;
            Table = GenerateTiles(XCount, YCount, variableCount);
        }

        public int XCount { get; private set; }
        public int YCount { get; private set; }
        public int VariableCount { get; private set; }
        public Match3TileModel[,] Table { get; private set; }

        public bool TrySwap(Match3TileModel m1, Match3TileModel m2)
        {
            var x1 = m1.X;
            var x2 = m2.X;
            var y1 = m1.Y;
            var y2 = m2.Y;

            if (x1 < 0 || x1 >= XCount || y1 < 0 || y1 >= YCount ||
                x2 < 0 || x2 >= XCount || y2 < 0 || y2 >= YCount ||
                x1 == x2 && y1 == y2)
                return false;

            if (Mathf.Abs(x1 - x2) == 1 && Mathf.Abs(y1 - y2) == 1 || Mathf.Abs(x1 - x2) > 1 || Mathf.Abs(y1 - y2) > 1)
                return false;

            Table[x1, y1] = m2;
            m2.ChangeCoordinates(x1, y1);
            Table[x2, y2] = m1;
            m1.ChangeCoordinates(x2, y2);

            return true;
        }

        public bool TryCleanTiles(List<Match3TileModel> executeList)
        {
            var count = 0;
            for (var i = 0; i < XCount; i++)
            {
                for (var j = 0; j < YCount; j++)
                {
                    if (Table[i, j] != null)
                        count += TryClean(i, j, executeList);
                }
            }

            return count > 0;
        }

        public void SdvigTiles(int sdvigCount = 1)
        {
            for (int i = 0; i < XCount; i++)
            {
                for (int f = 0; f < sdvigCount; f++)
                {
                    for (int j = 0; j < YCount; j++)
                    {
                        if (Table[i, j] == null && j + 1 < YCount)
                        {
                            Table[i, j] = Table[i, j + 1];
                            Table[i, j + 1] = null;
                            if (Table[i, j] != null)
                                Table[i, j].ChangeCoordinates(i, j);
                        }
                    }
                }
            }
        }

        public List<Match3TileModel> GenerateAddictiveTiles(bool isSdvig = false)
        {
            if (isSdvig)
                SdvigTiles(YCount);

            var resArray = new List<Match3TileModel>();
            for (int i = 0; i < XCount; i++)
            {
                for (int j = 0; j < YCount; j++)
                {
                    if (Table[i, j] == null)
                    {
                        Table[i, j] = ResolveTile(i, j, Table);

                        resArray.Add(Table[i, j]);
                    }
                }
            }

            return resArray;
        }

        private Match3TileModel[,] GenerateTiles(int x, int y, int variableCount)
        {
            var array = new Match3TileModel[x, y];

            for (var i = 0; i < x; i++)
            {
                for (var j = 0; j < y; j++)
                {
                    array[i, j] = ResolveTile(i, j, array);
                }
            }
            return array;
        }

        private Match3TileModel ResolveTile(int i, int j, Match3TileModel[,] array)
        {
            int index;
            do
            {
                index = Random.Range(1, VariableCount + 1);
            } while
            (i > 1 && array[i - 1, j] != null && array[i - 1, j].Index == index ||
             j > 1 && array[i, j - 1] != null && array[i, j - 1].Index == index);

            return new Match3TileModel(i, j, index);
        }

        private int TryClean(int x, int y, List<Match3TileModel> executeList)
        {
            int result1 = 0;
            int result2 = 0;
            if (Table[x, y] == null)
                return 0;

            var currentIndex = Table[x, y].Index;

            if (x + 3 <= XCount)
                for (int i = x; i < x + 3; i++)
                    if (Table[i, y] != null && Table[i, y].Index == currentIndex)
                        result1++;

            if (y + 3 <= YCount)
                for (int i = y; i < y + 3; i++)
                    if (Table[x, i] != null && Table[x, i].Index == currentIndex)
                        result2++;

            if (result1 >= 3 || result2 >= 3)
                return CleanRecursive(x, y, currentIndex, executeList);

            return 0;
        }

        private int CleanRecursive(int x, int y, int index, List<Match3TileModel> executeList)
        {
            if (x < XCount && y < YCount && x >= 0 && y >= 0 && Table[x, y] != null && Table[x, y].Index == index)
            {
                executeList.Add(Table[x, y]);
                Table[x, y] = null;
                return 1 + CleanRecursive(x + 1, y, index, executeList) + CleanRecursive(x - 1, y, index, executeList) +
                       CleanRecursive(x, y + 1, index, executeList) + CleanRecursive(x, y - 1, index, executeList);
            }

            return 0;
        }
    }
}