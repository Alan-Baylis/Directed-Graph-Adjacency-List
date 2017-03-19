using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectedGraphAdjacencyList
{
    public class Link<T>
    {
        // get and set for Station and Colour
        public Station<T> AdjVertex { get; set; }
        public int Colour { get; set; }

        // constructor for Link
        public Link(Station<T> vertex, int colour)
        {
            AdjVertex = vertex;
            Colour = colour;
        }
    }

    //---------------------------------------------------------------------------------------------

    public class Station<T>
    {
        public T Name { get; set; }
        public List<Link<T>> E { get; set; }

        // constructor for Station
        public Station(T name)
        {
            Name = name;
            E = new List<Link<T>>();
        }

        // FindEdge
        // Returns the index of the given adjacent vertex in E; otherwise returns -1
        public int FindLink(T name)
        {
            int i;
            for (i = 0; i < E.Count; i++)
            {
                if (E[i].AdjVertex.Name.Equals(name))
                    return i;
            }
            return -1;
        }
    }

    //---------------------------------------------------------------------------------------------

    public interface IDirectedGraph<T>
    {
        void InsertStation(T name);
        void InsertLink(T name1, T name2, int colour);
        void DeleteLink(T name1, T name2, int colour);
    }

    //---------------------------------------------------------------------------------------------

    public class DirectedGraph<T> : IDirectedGraph<T> where T : IComparable
    {
        private List<Station<T>> V;

        public DirectedGraph()
        {
            V = new List<Station<T>>();
        }

        // FindStation
        // Returns the index of the given vertex (if found); otherwise returns -1
        private int FindStation(T name)
        {
            int i;
            for (i = 0; i < V.Count; i++)
            {
                if (V[i].Name.Equals(name))
                    return i;
            }
            return -1;
        }

        // FindStation that returns an int instead of T
        private int FindStation(int v)
        {
            int i;
            for (i = 0; i < V.Count; i++)
            {
                if (V[i].Name.Equals(v))
                    return i;
            }
            return -1;
        }

        // InsertStation
        // Adds the given statyion to the graph
        // Duplicate stations are not added
        public void InsertStation(T name)
        {
            if (FindStation(name) == -1)
            {
                Station<T> v = new Station<T>(name);
                V.Add(v); // adds v
            }
        }

        // InsertLink
        // Adds the given link (name1, name2, colour) to the graph
        // Notes: Duplicate edges are not added unless the colour is different
        public void InsertLink(T from, T to, int colour = 0)
        {
            int i, j;
            Link<T> e;
            // if the stations exist exist
            if ((i = FindStation(from)) > -1 && (j = FindStation(to)) > -1)
            {
                if (V[i].FindLink(to) != -1) // if the entry exists, check for colour differences
                {
                    if (V[i].E.Count < j) // color doesn't exist. can add the "duplicate" entry
                    {
                        e = new Link<T>(V[j], colour);
                        V[i].E.Add(e); // add e

                        e = new Link<T>(V[i], colour);
                        V[j].E.Add(e); // add e
                    }
                }

                else
                {
                    if (V[i].FindLink(to) == -1) // if it's a new entry
                    {
                        e = new Link<T>(V[j], colour);
                        V[i].E.Add(e); // add e

                        e = new Link<T>(V[i], colour);
                        V[j].E.Add(e); // add e
                    }
                }
            }
        }

        // DeleteLink
        // Removes the given link (name1, name2, colour) from the graph based on colour
        // Note: Nothing is done if the edge does not exist
        public void DeleteLink(T from, T to, int colour)
        {
            int i, j, k;
            // find from and to, match colour, if it matches, delete
            if ((i = FindStation(from)) > -1 && (j = V[i].FindLink(to)) > -1)
                for (k = 0; k < V[i].E.Count; k++) // cycle through edges
                {
                    if (V[i].E[k].Colour == colour) // if colors match
                    {
                        V[i].E.RemoveAt(k); // remove
                        break;
                    }
                }

            // find to and from (opposite direction), match colour, if it matches, delete
            if ((i = FindStation(to)) > -1 && (j = V[i].FindLink(from)) > -1)
                for (k = 0; k < V[i].E.Count; k++) // cycle through edges
                {
                    if (V[i].E[k].Colour == colour) // if colors match
                    {
                        V[i].E.RemoveAt(k); // remove
                        break;
                    }
                }
        }

        // displays string based on colour's int
        public string GetColour(int colour)
        {
            if (colour == 1)
                return "red";
            if (colour == 2)
                return "blue";
            if (colour == 3)
                return "green";
            if (colour == 4)
                return "orange";
            if (colour == 5)
                return "brown";
            if (colour == 6)
                return "purple";
            else
                return "[error]";
        }

        public bool FastestRoute(T from, T to)
        {
            Queue<T> Q = new Queue<T>();
            HashSet<T> S = new HashSet<T>();
            Q.Enqueue(from); // start Queue
            S.Add(from); // marked visited

            // lists and dictionary hold items to display fastest route
            List<T> list = new List<T>();
            List<T> list2 = new List<T>();
            Dictionary<T, T> dictionary = new Dictionary<T, T>();

            int i, j, currentcolour = 0, steps = 0, y, z;
            T b, working, curr, next;

            while (Q.Count > 0)
            {
                T v = Q.Dequeue(); // dequeue item
                list.Add(v); // add it to the list

                if (v.CompareTo(to) == 0) // if v and to are the same
                {
                    list.Reverse(); // reverse to find way back
                    Q.Clear(); // clear queue so we don't continue the loop

                    list2.Add(list[0]);

                    Console.WriteLine("Shortest distance from {0} to {1}: ", from, to);

                    // match items that belong
                    foreach (var item in list)
                    {
                        if (dictionary.TryGetValue(item, out working))
                        {
                            if (item.CompareTo(list2[list2.Count - 1]) == 0)
                            {
                                list2.Add(working);
                                steps++;
                            }
                        }
                    }

                    list2.Reverse(); // reverse to find way back

                    // display results
                    for (int i2 = 0; i2 < list2.Count; i2++)
                    {

                        if (i2 == list2.Count - 1)
                            Console.Write("{0}", list2[i2]);
                        else
                        {
                            Console.WriteLine("{0} -> ", list2[i2]);

                            curr = list2[i2];
                            next = list2[i2 + 1];

                            // below is used for matching color changes along the way
                            if ((z = FindStation(curr)) > -1 && (j = V[z].FindLink(next)) > -1)
                            {
                                y = V[z].E[j].Colour;

                                if (currentcolour == 0)
                                    currentcolour = V[z].E[j].Colour;
                                else
                                {
                                    if (y != currentcolour)
                                    {
                                        string colour1 = GetColour(currentcolour);
                                        string colour2 = GetColour(y);

                                        Console.WriteLine("Changed from {0} to {1} ->", colour1, colour2);
                                        currentcolour = V[z].E[j].Colour;
                                    }
                                }
                            }
                        }
                    }

                    Console.WriteLine("\n\n{0} steps total.", steps);
                    return true;
                }

                // continues to enqueue and add the popped items
                else
                {
                    if ((i = FindStation(v)) > -1)
                    {
                        for (j = 0; j < V[i].E.Count; j++)
                        {
                            b = V[i].E[j].AdjVertex.Name;
                            if (!S.Contains(b))
                            {
                                Q.Enqueue(b);
                                S.Add(b);

                                if (!dictionary.ContainsKey(b))
                                    dictionary.Add(b, v);
                            }
                        }
                    }
                }
            }
            return false;
        }

        // displays the number of edges total
        public void CountEdges()
        {
            int edges = 0;
            int i, j;
            T v;

            for (int index = 0; V.Count > index; index++) // while index is less than total number of Stations
            {
                v = V[index].Name;
                if ((i = FindStation(v)) > -1) // find station
                {
                    for (j = 0; j < V[i].E.Count; j++) // find all edges for that station
                        edges++; // add 1 when a edge is found
                }
            }
            Console.WriteLine("There are {0} edges total. ", edges);
        }

        // all counters and arrays necessary for the CriticalStations depth-first search
        List<int> critical = new List<int>();
        int[] low = new int[30];
        int[] num = new int[30];
        int[] parent = new int[30];
        bool[] visited = new bool[30];
        int counter = 0;

        public void CriticalStations()
        {
            // reset all counters and arrays back to 0
            counter = 0;
            Array.Clear(low, 0, low.Length);
            Array.Clear(num, 0, num.Length);
            Array.Clear(parent, 0, parent.Length);
            Array.Clear(visited, 0, visited.Length);
            critical.Clear();

            // starting point for depth-first search
            int v = Convert.ToInt32(V[13].Name);

            AssignNum(v); // assigns numbers based on traversal
            AssignLow(v); // finds articulatin points 
            FindArt(v); // finds articulatin points

            critical.Sort(); // sort list for output

            Console.WriteLine("Critial Stations/Articulation Points");

            // displays list of articulation points
            for (int i = 0; i < critical.Count; i++)
            {
                if (i == critical.Count - 1)
                    Console.Write("{0}", critical[i]);
                else
                    Console.Write("{0}, ", critical[i]);
            }

            Console.WriteLine();
        }

        // assigns numbers based on traversal
        private void AssignNum(int v)
        {
            num[v] = counter++; // give it a position in the traversal
            visited[v] = true; // marked as visited

            int i = FindStation(v); // get position of v

            for (int j = 0; j < V[i].E.Count; j++) // cycle through its edges
            {
                int w = Convert.ToInt32(V[i].E[j].AdjVertex.Name);

                if (!visited[w]) // if it hasn't been visited, recurse
                {
                    parent[w] = v; // set the edge's parent to v
                    AssignNum(w); // recurse with child
                }
            }
        }

        // finds articulatin points 
        private void AssignLow(int v)
        {
            low[v] = num[v];
            int i = FindStation(v);

            for (int j = 0; j < V[i].E.Count; j++) // cycle through its edges
            {
                int w = Convert.ToInt32(V[i].E[j].AdjVertex.Name); // set child

                if (num[w] > num[v]) // forward edge
                {
                    AssignLow(w); // recurse with child
                    if (low[w] >= num[v])
                    {
                        if (!critical.Contains(v)) // if the list doesn't contain it, add it
                            critical.Add(v);
                        low[v] = Math.Min(low[v], low[w]); // set low[v] to the lesser of the two values
                    }
                }

                else if (parent[v] != w) // back edge
                    low[v] = Math.Min(low[v], num[w]);
            }
        }

        // finds articulatin points 
        private void FindArt(int v)
        {
            visited[v] = true; // marked as visited
            low[v] = num[v] = counter++; // give it a position inthe traversal and set low to it

            int i = FindStation(v);

            for (int j = 0; j < V[i].E.Count; j++)
            {
                int w = Convert.ToInt32(V[i].E[j].AdjVertex.Name);

                if (!visited[w])
                {
                    parent[w] = v; // designate child's paremt
                    FindArt(w); // recurse with child

                    if (low[w] >= num[v]) // forward edge
                    {
                        if (!critical.Contains(v)) // if the list doesn't contain it, add it
                            critical.Add(v);
                        low[v] = Math.Min(low[v], low[w]);
                    }

                    else if (parent[v] != w) // back edge
                        low[v] = Math.Min(low[v], num[w]);
                }
            }
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            DirectedGraph<int> H = new DirectedGraph<int>();
            H.InsertStation(1);
            H.InsertStation(2);
            H.InsertStation(2); // tests for duplicate stations
            H.InsertStation(3);
            H.InsertStation(4);
            H.InsertStation(5);
            H.InsertStation(6);
            H.InsertStation(7);
            H.InsertStation(8);
            H.InsertStation(9);
            H.InsertStation(10);
            H.InsertStation(11);
            H.InsertStation(12);
            H.InsertStation(13);
            H.InsertStation(14);
            H.InsertStation(15);
            H.InsertStation(16);
            H.InsertStation(17);
            H.InsertStation(18);
            H.InsertStation(19);
            H.InsertStation(20);

            H.InsertLink(1, 3, 3);
            H.InsertLink(1, 3, 3); // duplicate links with same colors aren't added  
            H.InsertLink(3, 1, 3); // duplicate links with same colors aren't added (in reversed order)
            H.InsertLink(1, 14, 3);
            H.InsertLink(2, 11, 5);
            H.InsertLink(2, 12, 5);
            H.InsertLink(2, 17, 5);
            H.InsertLink(2, 17, 3); // duplicate links with different colors ARE added
            H.InsertLink(4, 6, 2);
            H.InsertLink(5, 6, 4);
            H.InsertLink(6, 7, 2);
            H.InsertLink(6, 14, 4);
            H.InsertLink(7, 12, 2);
            H.InsertLink(7, 15, 2);
            H.InsertLink(8, 10, 1);
            H.InsertLink(8, 12, 1);
            H.InsertLink(12, 13, 1);
            H.InsertLink(12, 13, 2); // duplicate links with different colors ARE added
            H.InsertLink(13, 14, 1);
            H.InsertLink(14, 16, 1);
            H.InsertLink(14, 17, 3);
            H.InsertLink(14, 19, 6);
            H.InsertLink(17, 18, 3);
            H.InsertLink(19, 20, 6);

            H.DeleteLink(12, 13, 2); // deleting 1 of 2 parallel lines (between 12 and 13) with different colours

            Console.WriteLine("--------------------");
            H.FastestRoute(1, 2); // outputs fastest route

            int x, y, z;
            string input;

            Console.WriteLine("--------------------");
            H.CriticalStations(); // initiates sequence of depth-first search to determine articulation points

            H.CountEdges(); // outputs number of edges total

            do
            {
                Console.Write("--------------------\n1: Fastest Route\n2: Critical Stations\n3: Delete Link\n4: Insert Link\nEnter choice: ");
                input = Console.ReadLine();

                if (input == "1") // outputs fastest route
                {
                    Console.Write("--------------------\nFastest Route\nFrom: ");
                    input = Console.ReadLine();
                    x = Convert.ToInt32(input);

                    Console.Write("To: ");
                    input = Console.ReadLine();
                    y = Convert.ToInt32(input);

                    H.FastestRoute(x, y); // outputs fastest route
                    input = "";
                }


                if (input == "2") // outputs critical station
                {
                    Console.Write("--------------------\n");

                    H.CriticalStations(); // initiates sequence of depth-first search to determine articulation points
                    input = "";
                }

                if (input == "3") // allows user to delete edges
                {
                    Console.Write("--------------------\nDelete Edges That Join...\nStation 1: ");
                    input = Console.ReadLine();
                    x = Convert.ToInt32(input);

                    Console.Write("Station 2: ");
                    input = Console.ReadLine();
                    y = Convert.ToInt32(input);

                    Console.Write("Colour: ");
                    input = Console.ReadLine();
                    z = Convert.ToInt32(input);

                    H.DeleteLink(x, y, z);
                    input = "";
                    H.CountEdges(); // outputs number of edges total
                }

                if (input == "4") // allows user to insert edges
                {
                    Console.Write("--------------------\nInsert Edges That Join...\nStation 1: ");
                    input = Console.ReadLine();
                    x = Convert.ToInt32(input);

                    Console.Write("Station 2: ");
                    input = Console.ReadLine();
                    y = Convert.ToInt32(input);

                    Console.Write("Colour: ");
                    input = Console.ReadLine();
                    z = Convert.ToInt32(input);

                    H.InsertLink(x, y, z);
                    input = "";
                    H.CountEdges(); // outputs number of edges total
                }
            } while (input != "0");

            Console.ReadLine();
        }
    }
}
