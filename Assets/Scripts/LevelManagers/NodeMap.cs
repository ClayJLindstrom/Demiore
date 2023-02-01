using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*ISSUE: With the current programming, the dogs all make a map at the same time. This causes immense lag.
 * SOLUTIONS:
 * 1. Stop having the dogs do it all at once regularly.
 * 2. Have the NodeMap program avoid using While-loops and for-loops, and program it so that it can perform its operations
 * while allowing other things of equal importance to take effect as well.
 * 3. Instead of having each dog make their own map and trail to the player, have the mapping go in reverse; have the player node
 * draw paths to every node on the map (at least until reaching every dog or beast), then having each beast use the same map to copy
 * their own path to the player, and THEN clear the map (if needed). That way, we use one mapping instance for every dog! 
 * (This will be useful for future things anyway, so we might as well impleent it, such as when a dog howls for the rest of its' pack!)
 * 4. Alter the behavior of the dogs to avoid using the nodemap, or have them learn to navigate the maze on their own.
 * - 4.5. We can take inspiration from Lego Battles, where the minifigs first try going straight to the player, then when stuck, then
 * they trace a path using the nodemap. (It would still be smart to use the group programming where a group of dogs follow a leader.
 * Let's even have the Leader's position be added to the follower's current paths with each step so they can follow her.
 * 
 */

public class NodeMap : MonoBehaviour
{
    //Now for our Nodemap's variables!
    //our Nodes
    private Node[,] neighborhood;
    private List<Node> uncheckedNodes = new List<Node>();
    private List<Node> checkedNodes = new List<Node>();
    //just to be used with assigning neighbors.
    private RaycastHit2D neighborCheck;

    //for counting column/row length
    private int x_length, y_length;

    //our Nodes
    private class Node
    {
        private Vector2 location;
        private Neighbor[] neighbors = new Neighbor[8];//max of 9 neighbors
        //to keep track of parents
        private Node parent;
        //to keep track of our edges
        private int neighbor_count = 0;
        //and for the weight our node has.
        private float weight;
        //we'll add this for testing
        private string label;

        public Node(Vector2 loc)
        {
            location = loc;
            weight = 0;
            parent = null;
        }
        //edges
        public void AddEdge(Neighbor new_neighbor)
        {
            if (neighbor_count < 8) { 
                neighbors[neighbor_count] = new_neighbor;
                neighbor_count++;
            }
        }
        //for erasing the edges (for recalibrating them);
        public void DestroyEdges()
        {
            //delete(neighbors);
        }
        //for our node parent.
        public void SetParent(Node step_parent)
        {
            parent = step_parent;
        }
        public Node GetParent()
        {
            return parent;
        }
        //for our node weight
        public void SetWeight(float value) { weight = value; }
        public float GiveWeight() { return weight; }
        //return our neighbor nodes and their distances away.
        public Neighbor[] ReturnNeighbors()
        {
            return neighbors;
        }
        public int GiveNeighborCount() { return neighbor_count; }
        //return location.
        public Vector2 ReturnLocation()
        {
            return location;
        }
        //for resetting the node after a path is found
        public void ResetNode()
        {
            weight = 0;
            parent = null;
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
        public Node ReturnNode()
        {
            return end_node;
        }
    }

    //to check if a Neighbor needs to be added, and 
    private void AddEdge(Node one, Node two)
    {
        neighborCheck = Physics2D.Linecast(one.ReturnLocation(), two.ReturnLocation());
        if (!neighborCheck || neighborCheck.collider.gameObject.tag == "Player" || neighborCheck.collider.gameObject.tag == "BadGuy")
        {
            one.AddEdge(new Neighbor(two, Vector2.Distance(one.ReturnLocation(), two.ReturnLocation())));
            //In theory, we would want it going both ways, but with how the code works, it just duplicates in bridges.
            //two.AddEdge(new Neighbor(one, Vector2.Distance(one.ReturnLocation(), two.ReturnLocation())));
        }
    }



    //for Crafting our nodemap
    public NodeMap(Vector2 the_position, float x_length, float y_length, int x_count, int y_count)
    {
        x_length = x_count;
        y_length = y_count;
        float n_distance_x = x_length / x_count, n_distance_y = y_length / y_count;
        float x_start = -(x_length / 2), y_start = -y_length / 2;
        //make our appropriately-sized neighborhood map
        neighborhood = new Node[x_count, y_count];
        //for every column
        for (int i = 0; i < x_count; i++)
        {
            //and every row
            for (int j = 0; j < y_count; j++)
            {
                neighborhood[i, j] = new Node(the_position + new Vector2(x_start + n_distance_x * i, y_start + n_distance_y * j));
            }
        }
        //our nodes are spawned. Now we need to add edges
        //for every column,
        for (int i = 0; i < x_count; i++)
        {
            //and every row
            for (int j = 0; j < y_count; j++)
            {
                //if this isn't the left-most
                if (i != 0)
                {
                    //add the next ones to the left
                    AddEdge(neighborhood[i, j], neighborhood[i - 1, j]);
                    if (j != 0)
                    {
                        AddEdge(neighborhood[i, j], neighborhood[i - 1, j - 1]);
                    }
                    if (j != y_count - 1)
                    {
                        AddEdge(neighborhood[i, j], neighborhood[i - 1, j + 1]);
                    }
                }
                //if this isn't the rightmost
                if (i != x_count - 1)
                {
                    //add the next rightmost
                    AddEdge(neighborhood[i, j], neighborhood[i + 1, j]);
                    if (j != 0)
                    {
                        AddEdge(neighborhood[i, j], neighborhood[i + 1, j - 1]);
                    }
                    if (j != y_count - 1)
                    {
                        AddEdge(neighborhood[i, j], neighborhood[i + 1, j + 1]);
                    }
                }
                //now to add the ones above or below.
                if (j != 0)
                {
                    AddEdge(neighborhood[i, j], neighborhood[i, j - 1]);
                }
                if (j != y_count - 1)
                {
                    AddEdge(neighborhood[i, j], neighborhood[i, j + 1]);
                }
            }
        }
        /*/an error's going on with the neighbor nodes. 
        Debug.Log("Node on bottom left's neighbors");
        foreach (Neighbor nodes in neighborhood[0, 0].ReturnNeighbors())
        {
            if(nodes != null)//ckecking for this may be the issue, as the arrays are size 9, and not all of them are filled.
            {
                Debug.Log(nodes.ReturnNode().ReturnLocation());
            }
        }
        Debug.Log("Node in the middle");
        foreach (Neighbor nodes in neighborhood[15, 15].ReturnNeighbors())
        {
            if (nodes != null)//ckecking for this may be the issue, as the arrays are size 9, and not all of them are filled.
            {
                Debug.Log(nodes.ReturnNode().ReturnLocation());
            }
        }//*/
    }

    //should work in most cases, except for row/columns of size one.
    //FIX: Prevent this from not returning a neighborless node.
    private Node FindClosest(Vector3 targetLoc)
    {
        int x=0, y=0;
        //checking the x-axis.
        for(int i = 1; i < neighborhood.GetLength(0)-1; i++)
        {
            //if we find our closest x-value (If the current one is farther away from the
            if (Mathf.Abs(targetLoc.x - neighborhood[x, 0].ReturnLocation().x) < Mathf.Abs(targetLoc.x - neighborhood[i, 0].ReturnLocation().x))
            {
                break;
            }
            else { x = i; }
        }
        //we found our x! Now for our why.
        for (int j = 1; j < neighborhood.GetLength(1) - 1; j++)
        {
            //if we find our closest y-value (note: y = j-1)
            if (Mathf.Abs(targetLoc.y - neighborhood[x, y].ReturnLocation().y) < Mathf.Abs(targetLoc.y - neighborhood[x, j].ReturnLocation().y))
            {
                //if this is an actual node,
                if(neighborhood[x,y].GiveNeighborCount() > 0)
                {
                //we now have our x=y coordinates!
                    break;
                }
            }
            else { y = j; }
        }
        //Debug.Log("Closest Node: " + neighborhood[x, y].ReturnLocation().ToString());
        //we have our closest, but if it doesn't have any friends, we need to search again.
        if(neighborhood[x,y].GiveNeighborCount() > 0){
            //we see which diagonal node it is closest to
        }
        return neighborhood[x, y];
    }

    //reset our map!
    public void ResetMap()
    {
        //we only need to reset our checked and unchecked node lists. All the other nodes aren't touched.
        //for each of our checked nodes,
        foreach (Node node in checkedNodes)
        {
            //reset its weight and parent.
            node.ResetNode();
        }
        checkedNodes.Clear();
        foreach (Node node in uncheckedNodes)
        {
            //reset its weight and parent.
            node.ResetNode();
        }
        uncheckedNodes.Clear();
    }
    
    //for traversing our nodemap
    public List<Vector2> TracePath(Vector3 startingPos, Vector3 endingPos)
    {
        Debug.Log("Wrong Path Traced");
        //first, we get our starting and ending nodes
        Node current = FindClosest(startingPos);
        //Debug.Log(current.ReturnLocation());
        Node end = FindClosest(endingPos);
        //Debug.Log(end.ReturnLocation());
        uncheckedNodes.Add(current);
        while (current != end && uncheckedNodes.Count > 0)
        {
            checkedNodes.Add(current);
            if (uncheckedNodes.Contains(current)) { uncheckedNodes.Remove(current); }
            //add all of the nodes connected to it that are not on the closed list to the open list
            //also, set this node as the parent of all the nodes addd to the open list.
            foreach (Neighbor nodes in current.ReturnNeighbors())
            {
                //Debug.Log("going Through Neighbors!");
                //if we haven't checked it, or we have and it's a shorter path
                if (nodes != null)
                {
                    if (!checkedNodes.Contains(nodes.ReturnNode())
                        && nodes.ReturnNode().GiveWeight() < current.GiveWeight() + nodes.Weight())
                    {
                        //update the parent
                        nodes.ReturnNode().SetParent(current);
                        //update the weight
                        nodes.ReturnNode().SetWeight(current.GiveWeight() + nodes.Weight());
                        //add the node to the unchecked list.
                        uncheckedNodes.Add(nodes.ReturnNode());
                    }
                }
            }
            //then, we will calculate the next node to check, and repeat the cycle
            //find our next Node.
            if (uncheckedNodes.Count > 0)
            {
                current = uncheckedNodes[0];
                foreach (Node aNode in uncheckedNodes)
                {
                    if (aNode.GiveWeight() < current.GiveWeight()) { current = aNode; }
                }
            }
        }
        //we should now have our parents set up. Now it's time to onstruct the map.
        //we cannot return null, even if we don't find the right path!
        if(current != end) 
        {
            //then we'll get ourselves to the closest we can get!
            foreach(Node point in checkedNodes)
            {
                if(Vector2.Distance(point.ReturnLocation(), endingPos) < Vector2.Distance(current.ReturnLocation(), endingPos))
                {
                    current = point;
                }
            }
        }
        List<Vector2> path = new List<Vector2>();
        path.Add(current.ReturnLocation());
        while (current.GetParent() != null)
        {
            current = current.GetParent();
            path.Add(current.ReturnLocation());
        }
        Debug.Log(path);
        Debug.Log(path.Count);
        //now we have our path. Now it's time to reset our nodes.
        ResetMap();
        //now we can return our path!
        return path;
    }

    //To Fix: when using the Queen's path, the TraceQueensPath() returns only a 1-node path. parents aren't getting connected.

    //This is a modified version of Trace Path. Instead of tracing one path, we will trace all paths toward one destination at once.
    //first, we assign all of the node's parents according to which one is closer to the target.
    //second, we have creatures trace their paths based on a beginning node. (Essentially breaking down TracePath into two parts.)

    //first, for setting our parent values. (All ways are the "Queen's way" - Cheshire Cat)
    public void SetQueensPath(Vector3 startingPos)
    {
        Debug.Log("Queen's Path Set");
        //first, we get our starting node, the queen.
        Node current = FindClosest(startingPos);
        Debug.Log(current.GiveNeighborCount());//FIX: Returns Zero.  Why?!
        //Node end = FindClosest(endingPos); not needed.
        //first, we reset our checkedNodes list and uncheckedNodes list.
        ResetMap();
        uncheckedNodes.Add(current);
        //we do this for every node everywhere, even though we may not need to.
        do{
            checkedNodes.Add(current);
            if (uncheckedNodes.Contains(current)) { uncheckedNodes.Remove(current); }
            //add all of the nodes connected to it that are not on the closed list to the open list
            //also, set this node as the parent of all the nodes addd to the open list.
            foreach (Neighbor nodes in current.ReturnNeighbors())
            {
                //Debug.Log("going Through Neighbors!");
                //if we haven't checked it, or we have and it's a shorter path
                if (nodes != null)
                {
                    if (!checkedNodes.Contains(nodes.ReturnNode())
                        && nodes.ReturnNode().GiveWeight() < current.GiveWeight() + nodes.Weight())
                    {
                        //update the parent
                        nodes.ReturnNode().SetParent(current);
                        //update the weight
                        nodes.ReturnNode().SetWeight(current.GiveWeight() + nodes.Weight());
                        //add the node to the unchecked list.
                        uncheckedNodes.Add(nodes.ReturnNode());
                    }
                }
            }
            //then, we will calculate the next node to check, and repeat the cycle
            //find our next Node.
            if (uncheckedNodes.Count > 0)
            {
                current = uncheckedNodes[0];
                foreach (Node aNode in uncheckedNodes)
                {
                    if (aNode.GiveWeight() < current.GiveWeight()) { current = aNode; }
                }
            }
        }while (uncheckedNodes.Count > 0);
        //we should now have our parents set up. Now any creature can access the map and trace a path.
        //to see if we're setting parents or not.
        foreach(Node check in checkedNodes){
            if(check.GetParent() != null){Debug.Log("yes");}
            else{Debug.Log("No");}
        }
    }

    //part two: a monster tracing a path.
    public List<Vector2> TraceQueensPath(Vector2 startingPos){
        Debug.Log("Queen's Path Traced");
        //We get our closest Node.
        Node current = FindClosest(startingPos);
        //If this node doesn't have any parents (It would mean that it doesn't have any friends.)
        if(current.GetParent() != null) 
        {
            //then we'll get ourselves to the closest node we can get that has a parent!
            foreach(Node point in checkedNodes)
            {
                if(Vector2.Distance(point.ReturnLocation(), startingPos) < Vector2.Distance(current.ReturnLocation(), startingPos))
                {
                    current = point;
                }
            }
        }
        List<Vector2> path = new List<Vector2>();
        path.Add(current.ReturnLocation());
        while (current.GetParent() != null)
        {
            current = current.GetParent();
            path.Add(current.ReturnLocation());
        }
        Debug.Log(path);
        Debug.Log(path.Count);
        //we need to reverse the path, since the monsters travel the path from the bottom to the top.
        path.Reverse();
        //now we can return our path!
        return path;
    }
}
