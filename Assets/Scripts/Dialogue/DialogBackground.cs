using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogBackground : MonoBehaviour
{
    public GameObject backgroundImageSpawner; //memunculkan gambar latar belakang

    [HideInInspector]public Sprite currentBackground; //latar belakang yang di pakai saat ini
    [HideInInspector]public Sprite nextBackground; //latar belakang berikutnya
    [HideInInspector]public Image image; //latar belakang saat ini berada di children gameobject pertama / 0
    [HideInInspector]public bool disableFadeAnimation;
    //private CanvasGroup canvasGroup; //digunakan untuk memperbarui warna alpha dari gambar jika ada

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponentsInChildren<Image>()[0]; //mengambil komponen dari children gameobject pertama untuk di gunakan
        //canvasGroup = image.GetComponent<CanvasGroup>(); //mengambil komponen grup kanvas dari image

        //mengecheck apa kah ada latar belakang yang sekarang, jika tidak maka akan di ganti warna hitam
        if(!currentBackground) image.color = Color.black; 
    }

    // Update is called once per frame
    void Update()
    {
        //mengecheck apa kah ada latar belakang yang sekarang, jika iya maka akan di ganti warna putih
        if(currentBackground)
        {
            image.color = Color.white;
        }
    }

    //memperbarui latar belakang 
    public void UpdateBackground(Sprite sprite, bool disableFade)
    {
        nextBackground = sprite;
        disableFadeAnimation = disableFade;
        Instantiate(backgroundImageSpawner, transform);
    }
}
