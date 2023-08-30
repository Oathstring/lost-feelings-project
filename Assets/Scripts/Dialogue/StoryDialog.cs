using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor.Rendering;

public class StoryDialog : MonoBehaviour
{
    public TextMeshProUGUI textName; //nama peran dialog
    public TextMeshProUGUI textDialog; //dialog dari yang memerankan
    public GameObject[] chooseButtons;

    public DialogCast[] dialogCasts; //para pemeran dialog
    public bool speedTyping; //untuk ngecheck apakah ingin mempercepat ketikan
    public float slowSpeed; //kecepatan ketik saat pelan
    public float fastSpeed; //kecepatan ketik saat dipercepat
    public AudioSource soundButton; 

    private float currentTypingSpeed; //kecepatan ketik saat ini
    private int index; //nilai ini akan di gunakan dalam memilih dialog sesuai urutan
    private int dialog; //nilai ini akan di gunakan untuk mengkoreksi pilihan dialog
    private DialogBackground dialogBackground; //script dialog background

    // Start is called before the first frame update
    void Start()
    {
        dialogBackground = GetComponentInChildren<DialogBackground>(); //mengambil komponen dialog background

        textName.text = null; //menghapus semua text nama peran sebelum memulai dialog
        textDialog.text = null; //menghapus semua text dialog sebelum memulai dialog

        StartDialog(dialog); // mulai dialog pertama dan sesuai opsi choice (biasanya di mulai dari nilai 0)
    }

    // Update is called once per frame
    void Update()
    {
        //apakah ingin mempercepat kecepatan ketikan
        if(speedTyping) currentTypingSpeed = fastSpeed;
        else currentTypingSpeed = slowSpeed;

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if(!chooseButtons[0].activeSelf)
            {
                SelectDialog(dialog);
            }

            else
            {
                StopAllCoroutines();
                textDialog.text = dialogCasts[index].selectedDialog;
            }
        }

        if(dialogCasts[index].dialogChoiceTexts.Length != 0)
        {
            for(int i = 0; i < chooseButtons.Length; i++)
            {
                chooseButtons[i].SetActive(true);
                chooseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = dialogCasts[index].dialogChoiceTexts[i];
            }
        }

        else
        {
            for(int i = 0; i < chooseButtons.Length; i++)
            {
                chooseButtons[i].SetActive(false);
                chooseButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = null;
            }
        }
    }

    //memilih dialog
    public void SelectDialog(int choice)
    {
        soundButton.Play();

        if(textDialog.text == dialogCasts[index].selectedDialog)
        {
            NextDialog(choice);
        }

        else
        {
            StopAllCoroutines();
            textDialog.text = dialogCasts[index].selectedDialog;
        }
    }

    //memulai dialog pertama
    private void StartDialog(int choice)
    {
        index = 0;

        if(dialogCasts[index].dialogs.Length - 1 != 0)
        {
            dialog = choice;
            dialogCasts[index].selectedDialog = dialogCasts[index].dialogs[dialog];
            dialogBackground.UpdateBackground(dialogCasts[index].dialogBackrounds[dialog], dialogCasts[index].disableFadeAnimation);
        }

        else
        {
            choice = 0;
            dialogCasts[index].selectedDialog = dialogCasts[index].dialogs[choice];
            dialogBackground.UpdateBackground(dialogCasts[index].dialogBackrounds[choice], dialogCasts[index].disableFadeAnimation);
        }

        StartCoroutine(TypingDialog(choice));
    }

    //mulai mengetik
    IEnumerator TypingDialog(int choice)
    {
        foreach(char c in dialogCasts[index].dialogs[choice].ToCharArray())
        {
            textDialog.text += c;
            textName.text = dialogCasts[index].name;
            yield return new WaitForSeconds(currentTypingSpeed);
        }
    }

    //melanjutkan dialog
    private void NextDialog(int choice)
    {
        //apakah dialog cast mempunyai jumlah dialog melebihi nilai index
        if(index < dialogCasts.Length - 1)
        {
            index++;
            
            textName.text = null;
            textDialog.text = null;

            if(dialogCasts[index].dialogs[0] == "")
            {
                EndDialog(choice);   
            }
            
            //kembali melakukan check apakah opsinya lebih dari satu maka int choice bisa lebih dari satu juga
            if(dialogCasts[index].dialogs.Length - 1 != 0)
            {
                dialog = choice;
                dialogCasts[index].selectedDialog = dialogCasts[index].dialogs[dialog];
                if(dialogBackground.nextBackground != dialogCasts[index].dialogBackrounds[dialog])
                {
                    dialogBackground.UpdateBackground(dialogCasts[index].dialogBackrounds[dialog], dialogCasts[index].disableFadeAnimation);
                }
            }

            //jika tidak maka int choice akan kembali ke 0 untuk mengambil opsi pertama / 0
            else
            {
                choice = 0;
                dialogCasts[index].selectedDialog = dialogCasts[index].dialogs[choice];

                if(dialogBackground.nextBackground != dialogCasts[index].dialogBackrounds[choice])
                {
                    dialogBackground.UpdateBackground(dialogCasts[index].dialogBackrounds[choice], dialogCasts[index].disableFadeAnimation);
                }
            }

            //memulai mengetik dialog
            StartCoroutine(TypingDialog(choice));
        }
    }

    void EndDialog(int choice)
    {
        dialogBackground.UpdateBackground(dialogCasts[index].dialogBackrounds[choice], dialogCasts[index].disableFadeAnimation);

        textDialog.transform.parent.gameObject.SetActive(false);
        textName.transform.parent.gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

//berisi tentang sang peran, isi dialog dan background untuk dialognya
[Serializable]
public class DialogCast
{
    public string name;
    public bool disableFadeAnimation;
    [TextArea]
    public string selectedDialog;

    //variabel dialogs, dialogBackgrounds harus sama 
    [TextArea]
    public string[] dialogs;
    public Sprite[] dialogBackrounds;
    public string[] dialogChoiceTexts;
}
