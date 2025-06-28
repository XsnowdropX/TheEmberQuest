using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform prevprevRoom;
    [SerializeField] private Transform prevRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private Transform nextnextRoom;
    [SerializeField] private CameraController cam;

    private void Awake()
    {
        if(prevRoom)
            prevRoom.GetComponent<Room>().ActivateRoom(true);
        if(nextRoom)
            nextRoom.GetComponent<Room>().ActivateRoom(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
            { 
                if(nextnextRoom)
                nextnextRoom.GetComponent<Room>().ActivateRoom(true);
                //if(nextRoom)
                //nextRoom.GetComponent<Room>().ActivateRoom(true);
                //if(prevRoom)
                //prevRoom.GetComponent<Room>().ActivateRoom(true);
                if(prevprevRoom)
                prevprevRoom.GetComponent<Room>().ActivateRoom(false);
            }
            else
            { 
                if(prevprevRoom)
                prevprevRoom.GetComponent<Room>().ActivateRoom(true);
                //if(prevRoom)
                //prevRoom.GetComponent<Room>().ActivateRoom(true);
                //if(nextRoom)
                //nextRoom.GetComponent<Room>().ActivateRoom(true);
                if(nextnextRoom)
                nextnextRoom.GetComponent<Room>().ActivateRoom(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
            {
                if (nextnextRoom)
                    nextnextRoom.GetComponent<Room>().ActivateRoom(false);
                //if (nextRoom)
                //    nextRoom.GetComponent<Room>().ActivateRoom(true);
                //if (prevRoom)
                //    prevRoom.GetComponent<Room>().ActivateRoom(true);
                if (prevprevRoom)
                    prevprevRoom.GetComponent<Room>().ActivateRoom(true);
            }
            else
            {
                if (prevprevRoom)
                    prevprevRoom.GetComponent<Room>().ActivateRoom(false);
                //if (prevRoom)
                //    prevRoom.GetComponent<Room>().ActivateRoom(true);
                //if (nextRoom)
                //    nextRoom.GetComponent<Room>().ActivateRoom(true);
                if (nextnextRoom)
                    nextnextRoom.GetComponent<Room>().ActivateRoom(true);
            }
        }
    }
}
