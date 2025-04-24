
using System.Text;

namespace DynamicEvaluator.Logic;

internal sealed class QuineMcclusky
{
    private static Dictionary<int, List<Implicant>> Group(List<Implicant> implicants)
    {
        var group = new Dictionary<int, List<Implicant>>();
        foreach (Implicant m in implicants)
        {
            int count = Utilities.GetOneCount(m.Mask);

            if (!group.ContainsKey(count))
                group.Add(count, new List<Implicant>());

            group[count].Add(m);
        }

        return group;
    }

    private static bool Simplify(ref List<Implicant> implicants)
    {
        //Group by number of 1's and determine relationships by comparing.
        var groups = Group(implicants).OrderBy(i => i.Key).ToDictionary(i => i.Key, i => i.Value);

        var relationships = new List<ImplicantRelationship>();
        for (int i = 0; i < groups.Keys.Count; i++)
        {
            if (i == (groups.Keys.Count - 1)) break;

            var q = from a in groups[groups.Keys.ElementAt(i)]
                    from b in groups[groups.Keys.ElementAt(i + 1)]
                    where Utilities.GetDifferences(a.Mask, b.Mask) == 1
                    select new ImplicantRelationship(a, b);

            relationships.AddRange(q);
        }

        /*
         * For each relationship, find the affected minterms and remove them.
         * Then add a new implicant which simplifies the affected minterms.
         */
        foreach (ImplicantRelationship relationship in relationships)
        {
            var toRemove = new List<Implicant>();

            foreach (Implicant implicant in implicants)
            {
                if (relationship.Contains(implicant))
                    toRemove.Add(implicant);
            }

            foreach (Implicant implicant in toRemove) implicants.Remove(implicant);

            var newImplicant = new Implicant();
            newImplicant.Mask = Utilities.GetMask(relationship.A.Mask, relationship.B.Mask);
            newImplicant.Minterms.AddRange(relationship.A.Minterms);
            newImplicant.Minterms.AddRange(relationship.B.Minterms);

            bool exist = implicants.Any(m => m.Mask == newImplicant.Mask);

            if (!exist)
                implicants.Add(newImplicant);
        }

        //Return true if simplification occurred, false otherwise.
        return !(relationships.Count == 0);
    }

    private static void PopulateMatrix(ref bool[,] matrix, List<Implicant> implicants, List<int> inputs)
    {
        for (int m = 0; m < implicants.Count; m++)
        {
            int y = implicants.IndexOf(implicants[m]);

            foreach (int i in implicants[m].Minterms)
            {
                for (int index = 0; index < inputs.Count; index++)
                {
                    if (i == inputs[index])
                        matrix[y, index] = true;
                }
            }
        }
    }

    private static List<Implicant> SelectImplicants(List<Implicant> implicants, List<int> inputs)
    {
        var lstToRemove = new List<int>(inputs);
        var final = new List<Implicant>();
        int runnumber = 0;
        while (lstToRemove.Count != 0)
        {
            //Implicant[] weightedTerms = WeightImplicants(implicants, final, lstToRemove);
            foreach (var m in implicants)
            {
                bool add = false;

                if (Utilities.ContainsSubList(lstToRemove, m.Minterms))
                {
                    add = true;
                    if (lstToRemove.Count < m.Minterms.Count) break;
                }
                else add = false;

                if (((lstToRemove.Count <= m.Minterms.Count) && add == false) || runnumber > 5)
                {
                    if (Utilities.ContainsAtleastOne(lstToRemove, m.Minterms) && runnumber > 5) add = true;
                }

                if (add)
                {
                    final.Add(m);
                    foreach (int r in m.Minterms) lstToRemove.Remove(r);
                }
            }
            foreach (var item in final) implicants.Remove(item); //ami benne van már 1x, az még 1x ne
            ++runnumber;
        }

        return final;
    }

    private static string GetFinalExpression(List<Implicant> implicants, QuineMcCluskeyConfig config)
    {
        int longest = 0;
        StringBuilder final = new();

        foreach (Implicant m in implicants)
        {
            if (m.Mask.Length > longest)
                longest = m.Mask.Length;
        }

        for (int i = implicants.Count - 1; i >= 0; i--)
        {
            if (config.Negate)
            {
                final
                    .Append(implicants[i].ToString(longest, config))
                    .Append(" & ");
            }
            else
            {
                final
                    .Append(implicants[i].ToString(longest, config))
                    .Append(" | ");
            }
        }
        string ret = final.ToString();
        if (ret.Length > 3)
        {
            ret = ret[0..^3];
        }
        return ret switch
        {
            " + " => "true",
            "" => "false",
            _ => ret,
        };
    }

    public static string GetSimplified(IEnumerable<int> care, IEnumerable<int> dontcre, int variables, QuineMcCluskeyConfig? config = null)
    {
        config ??= new QuineMcCluskeyConfig();

        var implicants = new List<Implicant>();

        var all = care.Concat(dontcre).OrderBy(x => x).Distinct().ToList();

        foreach (var item in all)
        {
            var m = new Implicant
            {
                Mask = Utilities.GetBinaryValue(item, variables),
            };
            m.Minterms.Add(item);
            implicants.Add(m);
        }

        //int count = 0;
        while (Simplify(ref implicants))
        {
            //Populate a matrix.
            bool[,] matrix = new bool[implicants.Count, all.Count]; //x, y
            PopulateMatrix(ref matrix, implicants, all);
        }

        List<Implicant> selected;
        if (config.HazardFree) selected = implicants;

        else selected = SelectImplicants(implicants, care.ToList());

        return GetFinalExpression(selected, config);
    }
}