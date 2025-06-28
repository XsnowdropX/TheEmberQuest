using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistanceX;
    [SerializeField] private float verticalOffsetY;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;
    //private float currentPosX;

    private void Update()
    {
        transform.position = new Vector3(player.position.x + lookAhead, 
            player.position.y+ verticalOffsetY, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistanceX *  player.localScale.x), 
            Time.deltaTime * cameraSpeed);
    }

    //public void MoveToNewRoom(Transform _newRoom)
    //{
    //    currentPosX = _newRoom.position.x;
    //}
}
