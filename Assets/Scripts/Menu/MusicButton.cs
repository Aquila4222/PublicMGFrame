using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class MusicButton : MonoBehaviour
{
    [SerializeField]private TMP_Text musicText;
    [SerializeField]private TMP_Text artistsText;
    
    [SerializeField]private Image buttonImage;


    
    
    void Awake()
    {
        buttonImage = GetComponent<Image>();
    }
    

    public void UpdateContent(TrackInfo trackInfo)
    {
        musicText.text = trackInfo.MusicTitle;
        artistsText.text = trackInfo.MusicArtist;
    }
    

    public void OnSelect()
    {
        StopAllCoroutines();
        StartCoroutine(OnSelectEnumerator());

        // buttonImage.color = new Color(1, 1, 1, buttonImage.color.a);
        // musicText.color = Color.black;
        // artistsText.color = Color.black;
        // transform.localPosition = new Vector3(10,transform.localPosition.y,transform.localPosition.z);
        // transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
    }


    IEnumerator OnSelectEnumerator()
    {
        for(int i = 0; i < 11; i++)
        {
            float f = (i/10f)*(2-i/10f);
            
            buttonImage.color = new Color(f, f, f, buttonImage.color.a);
            musicText.color = new Color(1-f, 1-f, 1-f, musicText.color.a);
            artistsText.color = new Color(1-f, 1-f, 1-f, artistsText.color.a);
            transform.localPosition = new Vector3(10*f,transform.localPosition.y,transform.localPosition.z);

            
            float s = (1.1f - 1f) * f + 1f;
            transform.localScale = new Vector3(s, s, s);
            
            yield return new WaitForSeconds(0.02f);
        }
    }
    

    public void OnUnselect()
    {
        StopAllCoroutines();
        
        buttonImage.color = new Color(0, 0, 0, buttonImage.color.a);
        musicText.color = Color.white;
        artistsText.color = Color.white;
        transform.localPosition = new Vector3(0,transform.localPosition.y,transform.localPosition.z);
        transform.localScale = new Vector3(1, 1, 1);
    }
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
