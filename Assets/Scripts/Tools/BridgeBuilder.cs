using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBuilder : MonoBehaviour
{
    private GameObject Parent;
    public GameObject bridgeLink;
    public Vector2 spawnPoint;
    public GameObject bridgeAnchor;
    public int length;


    public void BuildObject()
    {
        GameObject[] bridgeLinks = new GameObject[length];
        // Step 0 - Create Empty Parent Object
        Parent = new GameObject("Bridge");

        // Step 1 - Spawn Left Anchor
        GameObject leftAnchor = Instantiate(bridgeAnchor, spawnPoint, Quaternion.identity);
        SetNewParent(leftAnchor);

        // Step 2 - Spawn Links
        float offset = 1;
        for(int i = 0; i < length; i++)
        {
            GameObject link = Instantiate(bridgeLink, spawnPoint + Vector2.right * offset, Quaternion.identity);
            SetNewParent(link);
            offset += 2;

            /// Add link to array
            bridgeLinks[i] = link;
        }

        // Step 3 - Spawn Right Anchor
        GameObject rightAnchor = Instantiate(bridgeAnchor, spawnPoint + Vector2.right * (offset - 1), Quaternion.identity);
        SetNewParent(rightAnchor);

        // Step 4 - Link Joints
        for(int i = 0; i < bridgeLinks.Length; i++)
        {
            HingeJoint2D hj = bridgeLinks[i].GetComponent<HingeJoint2D>();
            if (i == 0)
                hj.connectedBody = leftAnchor.GetComponent<Rigidbody2D>();
            else
                hj.connectedBody = bridgeLinks[i - 1].GetComponent<Rigidbody2D>();
        }
        rightAnchor.GetComponent<HingeJoint2D>().connectedBody = bridgeLinks[bridgeLinks.Length - 1].GetComponent<Rigidbody2D>();
    }

    // Set transform to the Bridge Empty Object
    private void SetNewParent(GameObject obj)
    {
        obj.transform.SetParent(Parent.transform);
    }
}
