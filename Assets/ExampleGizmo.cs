using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Node
{   
    public int num;
    public float cooldown;
    public Vector3 position;
    public Vector3 velocity;
    public List<Node> children;
    
}

public class ExampleGizmo : MonoBehaviour
{
    public List<Node> nodes;
    public float desireDistance = 1;
    public float connectedNodeForce = 1;
    public float disConnectedNodeForce = 1;
    void Start ()
    {
        nodes = new List<Node>();
        for (int i=0 ; i<10 ; i++)
        { 

            nodes.Add(new Node()
            {   
                num = i,
                cooldown = 1,
                children = nodes.Where(node => Random.value > 0.1f).ToList(),
                position = new Vector3(0.0f, 0.0f, 1.0f*i*20),
                velocity = Vector3.zero
            
            }); 
            
            
        }
        // foreach (var node in nodes)
        // {
        //     Debug.Log("nodeeeee"); 
        //     Debug.Log(node.num); 
        //     Debug.Log("children");
        //     foreach(var c in node.children){
        //         Debug.Log(c.num);
        //     }
        // }
        
    }
    //TODO ADD TEMPARATURE
    void Update()
    {   
        ApplyForce();
        foreach (var node in nodes){
            // Debug.Log(node.velocity); 
            node.position +=  node.velocity * Time.deltaTime;
        }
    }

    private void ApplyForce()
    {
        foreach(var node in nodes){
            var disconnectedNodes = nodes.Except(node.children);
            //attractive force
            node.cooldown *= 0.99f;
            foreach (var connectedNode in node.children)
            {

                var distance = (node.position - connectedNode.position).magnitude;
                
                var springForce =  distance * distance / 10;
                var repForce = 100 / distance;
                connectedNode.velocity = springForce * Time.deltaTime * (node.position - connectedNode.position).normalized+repForce * Time.deltaTime * (connectedNode.position - node.position).normalized;
                //connectedNode.velocity *= connectedNode.cooldown;
                Debug.Log(distance);
                //connectedNode.velocity -= repForce * Time.deltaTime * (connectedNode.position - node.position).normalized;
            }

            //repulsive force
            // foreach (var disconnectedNode in node.children)
            // {
            //     var distance = (node.position - disconnectedNode.position).magnitude;
            //     var applyforce = 2 / ( Mathf.Pow(distance, 2));
            //     disconnectedNode.velocity += applyforce * Time.deltaTime * (disconnectedNode.position - node.position).normalized;
            // }
        }
    }

    void OnDrawGizmos()
    {
        foreach (var node in nodes)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(node.position, 0.5f);
            Gizmos.color = Color.green;
            foreach(var child in node.children){
                Gizmos.DrawLine(node.position, child.position);
            }
        }
        
    }
}

