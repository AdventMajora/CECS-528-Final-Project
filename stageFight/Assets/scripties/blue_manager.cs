using UnityEngine;

/// <summary>
/// Player controller and behavior
/// </summary>
public class blue_manager : MonoBehaviour
{
	/// <summary>
	/// 1 - The speed of the ship
	/// </summary>
	public Vector3 speed = new Vector3(10,10,10);
	public Vector3 rot = new Vector3(0,0,0);
	
	public Vector3 movement;
    public float inputX = 0f;
	public float inputZ = 0f;
	
	private Animator animator;
 
    void Start()
    {
		animator = this.GetComponent<Animator>();
    }
	
	void Update()
	{
		animator.SetInteger("moving", 0);
        inputX = 0f;
		inputZ = 0f;
        
        if (Input.GetKey(KeyCode.Escape)) {
            Debug.Log("Detected key code");
            Application.Quit();
        }
        
        
            
        if (Input.GetKey(KeyCode.UpArrow)) {
            inputX = 1f;
			animator.SetInteger("dir", 0);
			animator.SetInteger("moving", 1);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            inputZ = 1f;
			animator.SetInteger("dir", 1);
			animator.SetInteger("moving", 1);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            inputX = -1f;
			animator.SetInteger("dir", 2);
			animator.SetInteger("moving", 1);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            inputZ = -1f;
			animator.SetInteger("dir", 3);
			animator.SetInteger("moving", 1);
        }
            
		// 4 - Movement per direction
		movement = new Vector3(
			inputX,
			0f,
            inputZ);
		
	}
    
    void OnCollisionEnter2D(Collision2D coll)
    {
    }
	
	void FixedUpdate()
	{
		
		transform.position += movement;
		transform.rotation = Quaternion.Euler(0,90,0);
        
	}
}