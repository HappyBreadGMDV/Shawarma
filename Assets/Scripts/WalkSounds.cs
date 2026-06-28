using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSounds : MonoBehaviour
{
    public List<StairsData> Sounds = new List<StairsData>();
    public float rayDistance = .75f;
    public AudioSource audioSourse;          // оставил оригинальное название
    public float MaxtimeIdk;                // интервал ходьбы
    public float MaxtimeSpeed;              // интервал бега
    public float Maxtime;                   // текущий интервал
    private float time;

    [System.Serializable]
    public class StairsData
    {
        public string tag;
        public AudioClip[] sounds;
    }

    void Update()
    {
        // Выбор интервала в зависимости от бега
        Maxtime = Input.GetKey(KeyCode.LeftShift) ? MaxtimeSpeed : MaxtimeIdk;

        time += Time.deltaTime;

        // Если персонаж не двигается — сбрасываем накопленное время,
        // чтобы после остановки не было мгновенного звука при следующем движении
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            time = 0f;
            return;
        }

        // Начало луча: позиция audioSourse (вероятно, он у ног персонажа)
        Vector3 origin = audioSourse.transform.position;
        Vector3 direction = Vector3.down;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, rayDistance))
        {
            for (int i = 0; i < Sounds.Count; i++)
            {
                if (Sounds[i].tag == hit.collider.gameObject.tag)
                {
                    if (Sounds[i].sounds.Length > 0 && time >= Maxtime)
                    {
                        AudioClip clip = Sounds[i].sounds[Random.Range(0, Sounds[i].sounds.Length)];
                        audioSourse.PlayOneShot(clip);
                        time = 0f;
                    }
                    break;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (audioSourse == null) return;
        Vector3 origin = audioSourse.transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + Vector3.down * rayDistance);
    }
}