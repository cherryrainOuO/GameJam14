using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    
    public static T Instance{
        get{
            if(instance == null){
                instance = (T)FindObjectOfType(typeof(T));

                if(instance == null){ // 타입을 씬에서 찾을수 없다면 그 타입의 게임오브젝트를 생성
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake(){
        if(transform.parent != null && transform.root != null){
            DontDestroyOnLoad(this.transform.root.gameObject);
        }else{
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
