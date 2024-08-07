using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int speed;
    Rigidbody rigid;
    public int rotation;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }


    void Update()
    {
        float time = Time.deltaTime;
        float cal = 0.5f;

        float xMove = 0; // W 1 S-1
        float zMove = 0; // D 1 A-1

        int playerrot = 0;
        int playerforntrot = 0;

        switch (rotation)
        {
            case 0: // x-1 z-1
                playerrot = -135;

                if (Input.GetKey(KeyCode.A))
                {
                    xMove += cal;
                    zMove += -cal;
                    playerforntrot -= 90;
                }

                else if (Input.GetKey(KeyCode.D))
                {
                    xMove += -cal;
                    zMove += cal;
                    playerforntrot += 90;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    xMove += -cal;
                    zMove += -cal;
                    playerforntrot += 0;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    xMove += cal;
                    zMove += cal;
                    playerforntrot -= 180;
                }

                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) playerforntrot += 45;
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) playerforntrot += -45;
                if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) playerforntrot = -135; // -45
                if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) playerforntrot = 135; // 135

                Debug.Log(playerforntrot);

                xMove *= 0.5f;
                zMove *= 0.5f;

                this.transform.rotation = Quaternion.Euler(0, playerforntrot + playerrot,0);
                break;

            case 1: // x1 z-1
                playerrot = -225;

                if (Input.GetKey(KeyCode.A))
                {
                    xMove += cal;
                    zMove += cal;
                    playerforntrot -= 90;
                }

                else if (Input.GetKey(KeyCode.D))
                {
                    xMove += -cal;
                    zMove += -cal;
                    playerforntrot += 90;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    xMove += cal;
                    zMove += -cal;
                    playerforntrot += 0;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    xMove += -cal;
                    zMove += cal;
                    playerforntrot -= 180;
                }

                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) playerforntrot += 45;
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) playerforntrot += -45;
                if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) playerforntrot = -135; // -45
                if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) playerforntrot = 135; // 135

                xMove *= 0.5f;
                zMove *= 0.5f;

                this.transform.rotation = Quaternion.Euler(0, playerforntrot + playerrot, 0);

                break;

            case 2:
                playerrot = -315;

                if (Input.GetKey(KeyCode.A))
                {
                    xMove += -cal;
                    zMove += cal;
                    playerforntrot = -90;
                }

                else if (Input.GetKey(KeyCode.D))
                {
                    xMove += cal;
                    zMove += -cal;
                    playerforntrot = 90;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    xMove += cal;
                    zMove += cal;
                    playerforntrot = 0;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    xMove += -cal;
                    zMove += -cal;
                    playerforntrot = 180;
                }

                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) playerforntrot = -45;
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) playerforntrot = 45;
                if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) playerforntrot = -135; // -45
                if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) playerforntrot = 135; // 135

                xMove *= 0.5f;
                zMove *= 0.5f;

                this.transform.rotation = Quaternion.Euler(0, playerforntrot + playerrot, 0);
                break;

            case 3:
                playerrot = -45;

                if (Input.GetKey(KeyCode.A))
                {
                    xMove += -cal;
                    zMove += -cal;
                    playerforntrot = -90;
                }

                else if (Input.GetKey(KeyCode.D))
                {
                    xMove += cal;
                    zMove += cal;
                    playerforntrot = 90;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    xMove += -cal;
                    zMove += cal;
                    playerforntrot = 0;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    xMove += cal;
                    zMove += -cal;
                    playerforntrot = 180;
                }

                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) playerforntrot = -45;
                if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) playerforntrot = 45;
                if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) playerforntrot = -135; // -45
                if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) playerforntrot = 135; // 135

                xMove *= 0.5f;
                zMove *= 0.5f;

                this.transform.rotation = Quaternion.Euler(0, playerforntrot + playerrot, 0);
                break;

            default:
                break;
        }

        Vector3 getVel = new Vector3(xMove, 0, zMove) * time * speed;
        rigid.velocity = getVel;

        if (Input.GetKeyDown(KeyCode.R))
        {
            rotation++;
            if (rotation >= 4)
            {
                rotation = 0;
            }
        }

        //         if (Input.GetKey(KeyCode.C))
        //         {
        //             this.transform.eulerAngles += new Vector3(0, 1, 0);
        //         }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.transform.position += new Vector3(0, 2, 0);
        }
    }
}
