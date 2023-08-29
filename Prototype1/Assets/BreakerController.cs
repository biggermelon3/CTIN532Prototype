using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("levelup"))
        {
            Destroy(other.gameObject);
            switch (this.gameObject.tag)
            {
                case "Level1":
                    this.gameObject.tag = "Level2";
                    break;
                case "Level2":
                    this.gameObject.tag = "Level3";
                    break;
                case "Level3":
                    // 你可以决定在这里做什么，比如不做任何事情或输出信息等。
                    break;
            }
        }
    }
}

