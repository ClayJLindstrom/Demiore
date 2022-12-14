using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeMap : MonoBehaviour
{
    //Now for our Nodemap's variables!
    //our Nodes
    private Node[,] neighborhood;

    //our Nodes
    private class Node
    {
        private Vector2 location;
        private Neighbor[] neighbors = new Neighbor[9];//max of 9 neighbors
        //to keep track of our edges
        private int neighbor_count = 0;
        //we'll add this for testing
        private string label;

        public Node(Vector2 loc)
        {
            location = loc;
            Debug.Log(location);
        }

        public void AddEdge(Neighbor new_neighbor)
        {
            neighbors[neighbor_count] = new_neighbor;
            neighbor_count++;
        }
        public Neighbor[] ReturnNeighbors()
        {
            return neighbors;
        }
        public Vector2 ReturnLocation()
        {
            return location;
        }
    }

    //Our edges
    private class Neighbor
    {
        private Node end_node;
        private float weight;

        public Neighbor(Node node, float w)
        {
            end_node = node;
            weight = w;
        }

        public float Weight()
        {
            //to return our weight
            return weight;
        }
        public Node ReturnNeighbor()
        {
            return end_node;
        }
    }

    //for Crafting our nodemap
    public NodeMap(Vector2 the_position, float x_length, float y_length, int x_count, int y_count)
    {
        float n_distance_x = x_length / x_count, n_distance_y = y_length/y_count;
        float x_start = -(x_length / 2), y_start = -y_length / 2;
        //make our appropriately-sized neighborhood map
        neighborhood = new Node[x_count, y_count];
        //for every column
        for(int i = 0; i < x_count; i++)
        {
            //and every row
            for(int j = 0; j < y_count; j++)
            {
                neighborhood[i, j] = new Node(the_position + new Vector2(x_start + n_distance_x * i, y_start + n_distance_y * j));
            }
        }
    }
    
    //for traversing our nodemap
    public List<Vector2> TracePath(Vector2 start, Vector2 end)
    {
        return null;
    }
}
