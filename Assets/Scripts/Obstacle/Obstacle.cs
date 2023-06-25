using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Net;
using Unity.VisualScripting;
using Fusion;
using FacundoColomboMethods;
using UnityEngine.Events;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(Collider))]

public class Obstacle : NetworkBehaviour
{
    [SerializeField]Element myElement;
    [SerializeField] GameObject view;

   
    public Action<NetworkPlayer> PlayerEnter;
    public Action<NetworkPlayer> PlayerLeave;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        SetElement();
    }

    void SetElement()
    {
        Tuple<LayerMask, Material> x = LayerManager.instance.GetElementData(myElement);
        gameObject.layer = x.Item1.LayerMaskToLayerNumber();

        Renderer ren  = view!=null ? view.GetComponent<Renderer>() : GetComponent<Renderer>();
       
        ren.material = x.Item2;
      
    
    }
    // Update is called once per frame
    public void DestroyObstacle()
    {
        Runner.Despawn(this.Object);
    }

    private void OnValidate()
    {
        //ver porque no puedo pasar la layer mask y tengo q ponerlo a mano
        //if (Application.isPlaying) return;      
  
       
        
        //Tuple<Color,string,int> x = myElement == Element.Fire 
        //    ? Tuple.Create(new Color (1,0,0,0.5f),"[Fire]",6) 
        //    : Tuple.Create(new Color(0,0,1,0.5f),"[Water]", 7);

        ////if (TryGetComponent(out Renderer z))
        ////{
        ////    if (z != null)
        ////        z.sharedMaterial.color = x.Item1;
                    
        ////}

      
        //gameObject.layer = x.Item3;
       


    }

    NetworkPlayer aux;
    IEnumerator DamageCoroutine()
    {
        WaitForSeconds wait = new WaitForSeconds(4);
        while (aux!=null)
        {
            aux.lifeHandler.RPC_TakeDamage(20);
            yield return wait;
        }
     
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NetworkPlayer player))
        {
            PlayerEnter?.Invoke(player);
            if (player.gameObject.layer!=gameObject.layer)
            {
                aux = player;
                
                StartCoroutine(DamageCoroutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out NetworkPlayer player))
        {
            PlayerLeave?.Invoke(player);
            if (aux!=null&& aux ==player )
            {
              
                StopAllCoroutines();
                aux = null;
            }
        }
    }
}
