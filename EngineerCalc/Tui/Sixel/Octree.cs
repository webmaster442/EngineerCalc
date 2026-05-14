using System.Runtime.CompilerServices;

using SkiaSharp;

namespace EngineerCalc.Tui.Sixel;

//https://raw.githubusercontent.com/bacowan/cSharpColourQuantization/refs/heads/master/ColourQuantization/Octree.cs
internal static class Octree
{
    public static SKColor[] Quantize(SKBitmap bitmap, int colourCount)
    {
        var quantizer = new PaletteQuantizer();
        var pixels = bitmap.Pixels;

        int w = bitmap.Width;
        int h = bitmap.Height;

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                var colour = pixels[y * w + x];
                quantizer.AddColour(colour);
            }
        }
        quantizer.Quantize(colourCount);

        SKColor[] retPixels = new SKColor[w * h];

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                var orig = pixels[y * w + x];
                var colour = quantizer.GetQuantizedColour(orig);
                retPixels[y * w + x] = colour;
            }
        }

        return retPixels;
    }

    private class PaletteQuantizer
    {
        private readonly Node _root;
        private readonly Dictionary<int, List<Node>> _levelNodes;

        public PaletteQuantizer()
        {
            _root = new Node(this);
            _levelNodes = new Dictionary<int, List<Node>>();
            for (int i = 0; i < 8; i++)
            {
                _levelNodes[i] = new List<Node>();
            }
        }

        public void AddColour(SKColor colour)
        {
            _root.AddColour(colour, 0);
        }

        public void AddLevelNode(Node node, int level)
        {
            _levelNodes[level].Add(node);
        }

        private readonly Dictionary<SKColor, SKColor> _cache = new();

        public SKColor GetQuantizedColour(SKColor colour)
        {
            if (_cache.TryGetValue(colour, out SKColor value))
            {
                return value;
            }
            else
            {
                value = _root.GetColour(colour, 0);
                _cache.Add(colour, value);
                return value;
            }
        }

        public void Quantize(int colourCount)
        {
            var nodesToRemove = _levelNodes[7].Count - colourCount;
            int level = 6;
            var toBreak = false;
            while (level >= 0 && nodesToRemove > 0)
            {
                var leaves = _levelNodes[level]
                    .Where(n => n.ChildrenCount - 1 <= nodesToRemove)
                    .OrderBy(n => n.ChildrenCount);
                foreach (var leaf in leaves)
                {
                    if (leaf.ChildrenCount > nodesToRemove)
                    {
                        toBreak = true;
                        continue;
                    }
                    nodesToRemove -= (leaf.ChildrenCount - 1);
                    leaf.Merge();
                    if (nodesToRemove <= 0)
                    {
                        break;
                    }
                }
                _levelNodes.Remove(level + 1);
                level--;
                if (toBreak)
                {
                    break;
                }
            }
        }
    }

    private class Node
    {
        private readonly PaletteQuantizer _parent;
        private Node[] _children = new Node[8];
        private SKColor Colour { get; set; }
        private int Count { get; set; }

        public int ChildrenCount { get; private set; }

        public Node(PaletteQuantizer parent)
        {
            this._parent = parent;
        }

        public void AddColour(SKColor colour, int level)
        {
            if (level < 8)
            {
                var index = Node.GetIndex(colour, level);
                if (_children[index] == null)
                {
                    var newNode = new Node(_parent);
                    _children[index] = newNode;
                    ChildrenCount++;
                    _parent.AddLevelNode(newNode, level);
                }
                _children[index].AddColour(colour, level + 1);
            }
            else
            {
                Colour = colour;
                Count++;
            }
        }

        public SKColor GetColour(SKColor colour, int level)
        {
            if (ChildrenCount == 0)
            {
                return Colour;
            }
            else
            {
                var index = Node.GetIndex(colour, level);
                return _children[index].GetColour(colour, level + 1);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetIndex(SKColor colour, int level)
        {

            int shift = 7 - level;
            byte mask = (byte)(1 << shift);


            int index = (colour.Red & mask) >> shift |
                        (((colour.Green & mask) >> shift) << 1) |
                        (((colour.Blue & mask) >> shift) << 2);

            return index;
        }

        public void Merge()
        {
            Colour = Average(_children.Where(c => c != null).Select(c => (c.Colour, c.Count)));
            Count = _children.Sum(c => c?.Count ?? 0);
            _children = new Node[8];
        }

        private static SKColor Average(IEnumerable<(SKColor color, int count)> colours)
        {
            var totals = colours.Sum(c => c.count);
            return new SKColor(red: (byte)(colours.Sum(c => c.color.Red * c.count) / totals),
                               green: (byte)(colours.Sum(c => c.color.Green * c.count) / totals),
                               blue: (byte)(colours.Sum(c => c.color.Blue * c.count) / totals),
                               alpha: 255);
        }
    }
}
