using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImageSpawner : MonoBehaviour
{
    private DialogBackground dialogBackground; //variabel dialog background
    private Image image; // komponen image jika tersedia juga di gameobject yang sama
    private CanvasGroup canvasGroup; //sama seperti variabel image

    // Start is called before the first frame update
    void Start()
    {
        dialogBackground = GetComponentInParent<DialogBackground>(); //mengambil komponennya dari parent
        image = GetComponent<Image>(); //mengambil komponen nya dari diri sendiri

        image.sprite = dialogBackground.nextBackground; //memasukan gambar sebagai gambar berikutnya agar di proses
        canvasGroup = GetComponent<CanvasGroup>(); //mengambil komponen nya dari diri sendiri
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogBackground.disableFadeAnimation)
        {
            canvasGroup.alpha = 1;
            enabled = false;
        }

        else
        {
            //jika alphanya sudah jadi 1 gameobjectnya akan di hancurkan
            if(canvasGroup.alpha >= 1)
            {
                enabled = false;
            }

            //jika belum akan di tambah perlahan sampai memiliki nilai 1
            else if(canvasGroup.alpha <= 1)
            {
                canvasGroup.alpha += Time.deltaTime / 1;
            }
        }

        if(image.sprite) image.color = Color.white;
        else image.color = Color.black;
    }

    //dipanggil sebelum gameobject ini hancur
    void OnDisable()
    {
        //apakah latar berikutnya sesuai dengan ini, jika iya maka latar berikutnya akan jadi latar sekarang
        if(dialogBackground.nextBackground == image.sprite) dialogBackground.currentBackground = image.sprite;

        //kalau latar belakang sekarang telah tersedia
        if(dialogBackground.currentBackground)
        {
            dialogBackground.image.sprite = dialogBackground.currentBackground;
        }

        else 
        {
            dialogBackground.image.sprite = null;
            dialogBackground.image.color = Color.black;
        }

        Destroy(gameObject);
    }
}
