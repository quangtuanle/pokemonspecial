using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Rigidbody playerRigibody;
    int floorMask;
    float camRayLength = 100f;

    private void Awake()
    {
        // Tạo một layer mask cho floor layer
        floorMask = LayerMask.GetMask("Floor");

        // Thiết lập References (tham chiếu đến Component của Player)
        playerRigibody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Store the input axes
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Move the player around the scene
        Move(h, v);

        // Turn the player to face the mouse cursor
        Turning();
    }

    private void Turning()
    {
        // Create a ray from the mouse cursor on the screen in the direction of the camera
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer
        // Ta cần thêm thông số floorMask vì nếu không có mặc định nó sẽ hiểu là Terrain
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;

            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            playerRigibody.MoveRotation(newRotation);
        }
    }

    private void Move(float h, float v)
    {
        // Set the movement vector based on the axis input
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigibody.MovePosition(transform.position + movement);
    }

    // Xử lý khi gặp pokemon tùy theo tỉ lệ Catch Rate (Grass có thể xuyên qua nên dùng OnTrigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GrassR1")
        {
            float random = UnityEngine.Random.Range(0f, 1f);
            if (random < 0.1) // 10%
            {
                Debug.Log("Wild Bulbasaur appear!");
            }
            else if (random < 0.15) // 5%
            {
                Debug.Log("Wild Ivysaur appear!");
            }
            else // 85%
            {
                Debug.Log("Nothing!");
            }
        }
    }
}
