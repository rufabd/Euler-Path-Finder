using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoreyNolur
{
    class Program
    {
        public class Graph
        {
            int v;
            Dictionary<int, LinkedList<int>> graph;
            int[,] edges = new int[7, 7];   // Representing graph in matrix form
            int[] noEuler = new int[20];   // 1st array till the specified vertices
            int[] euler = new int[20];   // Array to write the indexes of specified vertices
            int[] answer = new int[20];   // Array to show final answer
            int[] befanswer = new int[20];    //Array just before answer
            int indexOfEuler = 0; 
            public Graph(int[] vertices)
            {
                v = vertices.Count();
                graph = new Dictionary<int, LinkedList<int>>();
                foreach(int v in vertices)
                {
                    graph.Add(v, new LinkedList<int>());
                }
            }

            public void AddEdge(int src, int dest)  // Method for adding edges
            {
                if(graph.ContainsKey(src) && graph.ContainsKey(dest))
                {
                    if(!(graph[src]).Contains(dest))
                    {
                        (graph[src]).AddLast(dest);
                        edges[src, dest] = 1;
                    }
                    else
                    {
                        edges[src, dest] = 0;
                    }
                }
            }

            public void Print()   // Printing adjacency list
            {
                Console.WriteLine("Adjacency list for the given graph is shown below:\n");
                foreach(var i in graph)
                {
                    Console.Write("|{0}| ", i.Key);
                    foreach(var j in i.Value)
                    {
                        Console.Write(" {0} ", j);
                    }
                    Console.WriteLine();
                }
            }

            public void Matrix()  // Matrix form to represent graph
            {
                var row = edges.GetLength(0);
                var col = edges.GetLength(1);
                Console.WriteLine("\nThe Matrix form:\n");
                Console.WriteLine("-----------------------------------");
                for(int i = 1; i < row; i++)
                {
                    for(int j = 1; j < col; j++)
                    {
                        Console.Write("|{0}|" + "\t" , edges[i, j]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("-----------------------------------");
                Console.WriteLine();
            }

            public bool HasEulerianPath(int[,] array)  // Checking if the graph has eulerian path or not
            {
                bool t = true;
                int b = 0; // Her vertex ucun nece edge var
                int a = 0; //Tek sayda edge cixan verticeslerin sayi
                var row = array.GetLength(0);
                var col = array.GetLength(1);
                for(int i = 1; i < row; i++)
                {
                    for (int j = 1; j < col; j++)
                    {
                        if(edges[i,j] == 1)  //If [i,j] == 1 find number of edges for i-th vertex and increase b
                        {
                            b++;
                        }

                    }
                    if (b % 2 == 1)  //  If odd number of edges exist for the specified vertex increase a
                    {
                        a++;
                    }
                    b = 0;     
                }
                // Eulerian path rule
                if(a == 2)  
                {
                    Console.WriteLine("The shown graph has eulerian path\n");
                    t = true;
                }
                else if(a < 2 && a >= 0)
                {
                    Console.WriteLine("The shown graph does not have eaulerian path but cycle\n");
                    t = false;
                }
                else
                {
                    Console.WriteLine("There is neither eulerian path nor cycle\n");
                    t = false;
                }
                return t;
            }

            public int StartingVertex()  // Finding starting vertex
            {
                int start = 0;
                if(HasEulerianPath(edges))
                {
                    for(int i = 1; i < graph.Count; i++)
                    {
                        if(graph[i].Count % 2 == 1)
                        {
                            if (graph.ElementAt(i).Key < graph.ElementAt(i + 1).Key)
                            {
                                start = i;
                                break;
                            }
                        }
                    }
                }
                return start;
            }

            public int[] Reverse(int[] array)  // Method to reverse the array
            {
                int end = 0;
                for(int i = 0; i < array.Length; i++)
                {
                    if(array[i] == 0)
                    {
                        end = i - 1;
                        break;
                    }
                }
                int[] ak = new int[end + 1];
                for(int i = 0; i < ak.Length; i++)
                {
                    ak[i] = array[i];
                }
                Array.Reverse(ak);
                return ak;
            }


            public void Path()  // Finding euler path
            {
                var row = edges.GetLength(0);
                var col = edges.GetLength(1);
                int a = StartingVertex();
                int temp = a;
                int index1 = 0;   //specifiec index created for NOeuler array (for seeing on which index you stopped)
                int index2 = 0;   //specifiec index created for EULER array (for seeing on which index you stopped)
                int j = 1;
                int sum = 0;   // If there are no 1 values left => sum =0 => stop code
                bool first = true;
                //Console.Write(a + "\t");
                while (true) //will execute loop while condition is true until some condition will break it
                {
                    for (; j < col; j++)
                    {
                        if (edges[a, j] == 1)
                        {
                            if (first != true)
                            {
                                if (a != temp && index1 == 0 || index1 == 1)
                                {
                                    noEuler[index1++] = temp;
                                }
                                else if(a != temp && noEuler[index1 - 2] != temp)
                                {
                                    noEuler[index1++] = temp;
                                }
                            }
                            temp = a; //saves coordinate in tepmorary storage
                            edges[a, j] = 0;
                            edges[j, a] = 0;
                            a = j;
                            j = 0;
                            first = false;
                        }
                    }
                    sum = 0; //equalising to 0 cause before it used in while loop and may be other value instead of 0
                    for(int i = 1; i < row; i++)
                    {
                        for(int je = 1; je < col; je++)
                        {
                            sum += edges[i, je];
                        }
                    }
                    noEuler[index1++] = temp; //if you reached this code it meanss that you reached euler point and need to return back + save your steps to NOeuler
                    noEuler[index1++] = a;     //both steps
                    a = temp;
                    j = 0;
                    if(sum == 0)
                    {
                        break;
                    }
                    euler[index2++] = index1 - 1; //index-1 cause in 206th line there is index1++ which is required for future use | Euler array will store index of euler point
                }
                int index = 0;
                for(int h = 0; h < index2; h++, indexOfEuler++, index++)
                {
                    answer[index] = noEuler[euler[indexOfEuler]];
                }
                indexOfEuler = 0;
                for(int i = 0; i < noEuler.Length; i++)
                {
                    befanswer[i] = noEuler[i];     //rewriting existing array to new one for working on and reverse it in future
                }                
                while(index2>0)  //as index2 means how many time you need to take euler point(s) to the beginning
                {
                    for(int b = euler[indexOfEuler] + 1; b < noEuler.Length-1; b++)
                    {
                        befanswer[b - 1] = befanswer[b]; //shifts all values after eulerpoint to left (cause you remove euler point)
                    }
                    for(int l = 0; l < euler.Length; l++)
                    {
                        euler[l] = euler[l] - 1;  //index of euler point - 1 (cause of previous index+1)
                    }
                    indexOfEuler++;
                    index2--;
                }
                befanswer = Reverse(befanswer);
                for(int v = 0; v < answer.Length-1; v++)
                {
                    if(answer[v] == 0)
                    {
                        for(int f = 0; f < befanswer.Length; f++, v++)
                        {
                            answer[v] = befanswer[f];
                        }
                        break;
                    }
                }

                Console.WriteLine("The eulerian path for the given graph is:"); 
                Console.WriteLine();
                for(int i = 0; i < 9; i++)
                {
                    Console.Write(answer[i] + "   ");  // Final path
                }
                Console.WriteLine("\n");
                Console.WriteLine("The array of vertices before creating path:");
                for (int i = 0; i < 9; i++)
                {
                    Console.Write(noEuler[i] + "   ");  // Array of vertices before eulerian path
                }
                Console.WriteLine();
            }



            //public int NumberOfEdges()
            //{
            //    int number = 0; // Number of edges
            //    for (int i = 1; i < edges.GetLength(0); i++)
            //    {
            //        for(int j = 1; j < edges.GetLength(1); i++)
            //        {
            //            if(edges[i,j] == 1)
            //            {
            //                number = (number + 1) / 2;
            //            }
            //        }
            //    }
            //    return number;
            //}

            //public void Edges()
            //{
            //    if(NumberOfEdges() < graph.Count)
            //    {
            //        Console.WriteLine("It is not connected graph");
            //    }
            //}
        }

        

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Graph G = new Graph(new int[] { 1, 2, 3, 4, 5, 6 });
            int[,] edges = new int[7, 7];
            G = AddedEdges(G);
            Console.WriteLine("Welcome to the program for defining eulerian path!!!");
            G.Print();
            G.Matrix();
            Console.WriteLine("Starting vertex for the path is " + G.StartingVertex() + "\n");
            G.Path();

        }

        public static Graph AddedEdges(Graph G)  // Adding edges
        {
            G.AddEdge(1, 2);
            G.AddEdge(1, 3);
            G.AddEdge(1, 4);
            G.AddEdge(2, 1);
            G.AddEdge(2, 3);
            G.AddEdge(2, 4);
            G.AddEdge(3, 1);
            G.AddEdge(3, 2);
            G.AddEdge(3, 4);
            G.AddEdge(3, 5);
            G.AddEdge(4, 1);
            G.AddEdge(4, 2);
            G.AddEdge(4, 3);
            G.AddEdge(4, 5);
            G.AddEdge(5, 3);
            G.AddEdge(5, 4);

            //G.AddEdge(1, 3);
            //G.AddEdge(1, 4);
            //G.AddEdge(2, 5);
            //G.AddEdge(2, 6);
            //G.AddEdge(3, 1);
            //G.AddEdge(3, 4);
            //G.AddEdge(4, 1);
            //G.AddEdge(4, 3);
            //G.AddEdge(5, 2);
            //G.AddEdge(6, 2);
            return G;
        }
    }
}
