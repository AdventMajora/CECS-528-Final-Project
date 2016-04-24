using UnityEngine;
using UnityEngine.UI;

public class player_manager : MonoBehaviour
{
	public Vector3 speed = new Vector3(10,10,10);
	public Vector3 rot = new Vector3(0,0,0);
	
	public Vector3 movement;
    public float inputX = 0f;
	public float inputZ = 0f;
	public float book_speed = 15;
	public float book_progress = 0f;
	public float book_max = 5f;
	public float books = 0f;
	private bool book_recharge = false;
	public GameObject book_prefab;
	
	private Animator animator;
	private Rigidbody rigid;
	private GameManager manager;
 
	public Slider avail_books;

    void Start()
    {
		animator = this.GetComponent<Animator>();
		rigid = this.GetComponent<Rigidbody>();
		manager = GameObject.Find("Camera").GetComponent<GameManager>();
    }
	
	void Update()
	{
		if (manager.isGamePlaying()) {
			avail_books.value = books;
			animator.SetInteger("mov", 0);
			animator.SetInteger("atk", 0);
			inputX = 0f;
			inputZ = 0f;

			if (books == 0f) {
				book_recharge = true;
			}
			books += 0.01f;
			if (books >= book_max) {
				books = book_max;
				book_recharge = false;
			}
				
			//xbox controller shit
			inputZ = Input.GetAxis("Horizontal_1")*-1;
			inputX = Input.GetAxis("Vertical_1")*-1;
			
			
			if (Input.GetKey(KeyCode.W)) {
				inputX = 1f;
			}
			if (Input.GetKey(KeyCode.A)) {
				inputZ = 1f;
			}
			if (Input.GetKey(KeyCode.S)) {
				inputX = -1f;		
			}
			if (Input.GetKey(KeyCode.D)) {
				inputZ = -1f;
			}
			
			if (inputZ !=0 || inputX != 0) {
				animator.SetInteger("mov", 1);
			}
			if (inputX > 0) {
				animator.SetInteger("dir", 0);
			}
			if (inputX < 0) {
				animator.SetInteger("dir", 2);
			}
			if (inputZ > 0) {
				animator.SetInteger("dir", 1);
			}
			if (inputZ < 0) {
				animator.SetInteger("dir", 3);
			}
			
			if (Input.GetKey (KeyCode.LeftShift) || Input.GetAxis("Jump_1") > 0) {
				animator.SetInteger("mov",0);
				if (Mathf.Floor(books) > 0) {
					inputX = 0;
					inputZ = 0;
					animator.SetInteger ("atk", 1);

					if (book_progress == 0f && !book_recharge) {
						book_progress++;
						books--;
						if (books < 0) {
							books = 0;
						}

						GameObject book_obj = (GameObject) Instantiate(
								book_prefab, 
								new Vector3(transform.position.x, 8, (transform.position.z-24)), 
								Random.rotation
						);	//Casting error here
						AudioSource audio = GetComponent<AudioSource>();
						audio.Play();
						Vector3 rVelocity = new Vector3(0, 120+(Random.value*100), -150-(Random.value*100));
						book_obj.GetComponent<Rigidbody>().velocity = rVelocity;	//set the velocity 
					} else {
						book_progress += 1f;
						if (book_progress >= book_speed) {
							book_progress = 0f;
						}
					}
				}
			} else {
				book_progress = 0f;
			}
			movement = new Vector3(
			inputX,
			0f,
			inputZ);
		}
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