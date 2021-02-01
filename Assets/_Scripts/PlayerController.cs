using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    private Vector3 movement;
    private Animator _animator;
    private Rigidbody _rigidBody;
    [SerializeField]
    private float turnSpeed;
    //Se iguala a la identity para evitar que inicie antes la animación que el Quaternion y genere un conflicto
    private Quaternion rotation = Quaternion.identity;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    //Llamamos al FixedUpdate ya que es en el que se ejecuta el motor de físicas, y por tanto se ejecutará antes de llamar al OnAnimatorMove (En caso de usar Update, se ejecutaria antes el OnAnimatorMove)
    // Video 140 (La API de Unity: Update, FixedUpdate y OnAnimatorMove) minuto 10:00
    void FixedUpdate()
    {
        //La animación de desplazamiento ya lleva un desplazamiento implicito, por lo que no es necesario, por ahora, programar dicho desplazamiento
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movement.Set(horizontal, 0, vertical);
        movement.Normalize();

        //Solo necesitamos saber si se ha pulsado o no la tecla para desplazar, dado que la animación ya desplaza el GameObject
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        _animator.SetBool("isWalking", isWalking);

        if (isWalking){
            if(!_audioSource.isPlaying){
                _audioSource.Play();
            }
        }else{
            _audioSource.Stop();
        }

        //Con este método se puede interpolar la rotación para que sea más fluida
        //Video 139 (Coordinar física y animaciones para mover y rotar) minuto 29:00
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, movement, turnSpeed * Time.fixedDeltaTime, 0);

        rotation = Quaternion.LookRotation(desiredForward);
    }

    private void OnAnimatorMove() {
        
        //s = s0 + v*t
        //movement nos da la dirección y el deltaPosition el desplazamiento debido a la animación
        //Video 139 (Coordinar física y animaciones para mover y rotar) minuto 45:00
        _rigidBody.MovePosition(_rigidBody.position + movement * _animator.deltaPosition.magnitude);
        _rigidBody.MoveRotation(rotation);
    }
}
