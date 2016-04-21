using UnityEngine;

public class player_manager : MonoBehaviour
{
	public Vector3 speed = new Vector3(10,10,10);
	public Vector3 rot = new Vector3(0,0,0);
	
	public Vector3 movement;
    public float inputX = 0f;
	public float inputZ = 0f;
	
	private Animator animator;
	private Rigidbody rigid;
 
    void Start()
    {
		animator = this.GetComponent<Animator>();
		rigid = this.GetComponent<Rigidbody>();
    }
	
	void Update()
	{
		animator.SetInteger("mov", 0);
		animator.SetInteger("atk", 0);
        inputX = 0f;
		inputZ = 0f;
        
        if (Input.GetKey(KeyCode.Escape)) {
            Debug.Log("Detected key code");
            Application.Quit();
        }
            
        if (Input.GetKey(KeyCode.W)) {
            inputX = 1f;
			animator.SetInteger("dir", 0);
			animator.SetInteger("mov", 1);
        }
        if (Input.GetKey(KeyCode.A)) {
            inputZ = 1f;
			animator.SetInteger("dir", 1);
			animator.SetInteger("mov", 1);
        }
        if (Input.GetKey(KeyCode.S)) {
            inputX = -1f;
			animator.SetInteger("dir", 2);
			animator.SetInteger("mov", 1);
        }
        if (Input.GetKey(KeyCode.D)) {
            inputZ = -1f;
			animator.SetInteger("dir", 3);
			animator.SetInteger("mov", 1);
        }
		if (Input.GetKey(KeyCode.Space)) {
			inputX = 0;
			inputZ = 0;
			animator.SetInteger("atk", 1);
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