using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JSG.FortuneSpinWheel
{

    public class FortuneSpinWheel : MonoBehaviour
    {

        public RewardData[] m_RewardData;
        public Image m_CircleBase;
        public Image[] m_RewardPictures;
        public Text[] m_RewardCounts;
        public GameObject m_RewardPanel;
        public Text m_RewardFinalText;
        public Image m_RewardFinalImage;
        [HideInInspector]
        public bool m_IsSpinning = false;
        [HideInInspector]
        public float m_SpinSpeed = 0;
        [HideInInspector]
        public float m_Rotation = 0;

        public Image m_SpinButton;

        [HideInInspector]
        public int m_RewardNumber = -1;


        // Start is called before the first frame update
        void Start()
        {
            m_Rotation = 0;
            m_IsSpinning = false;
            m_RewardNumber = -1;

            for (int i = 0; i < m_RewardData.Length; i++)
            {

                m_RewardPictures[i].sprite = m_RewardData[i].m_Icon;
                if (m_RewardData[i].m_Count > 0)
                {
                    m_RewardCounts[i].text = "+" + m_RewardData[i].m_Count.ToString();
                }
                else
                {
                    m_RewardCounts[i].gameObject.SetActive(false);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (m_IsSpinning)
            {
                m_RewardPanel.gameObject.SetActive(false);
                if (m_SpinSpeed > 2)
                {
                    m_SpinSpeed -= 4 * Time.deltaTime;
                }
                else
                {
                    m_SpinSpeed -= .3f * Time.deltaTime;
                }
                m_Rotation += 100 * Time.deltaTime * m_SpinSpeed;
                m_CircleBase.transform.localRotation = Quaternion.Euler(0, 0, m_Rotation);
                for (int i = 0; i < 6; i++)
                {
                    m_RewardPictures[i].transform.rotation = Quaternion.identity;
                }
                if (m_SpinSpeed <= 0)
                {
                    m_SpinSpeed = 0;
                    m_IsSpinning = false;
                    m_RewardNumber = (int)((m_Rotation % 360) / 60);

                    StartCoroutine(ShowRewardMenu(1));
                    HandleReward();

                }

            }
            else
            {
                if (m_RewardNumber != -1)
                {
                    m_RewardPictures[m_RewardNumber].transform.localScale = (1 + 0.2f * Mathf.Sin(10 * Time.time)) * Vector3.one;
                }
            }
        }

        public void HandleReward()
        {
            RewardData reward = m_RewardData[m_RewardNumber];
            switch (reward.m_Type)
            {
                case "coin":
                    // add coin count to your inventory
                    break;

                case "gem":
                    //  add gem count to your inventory
                    break;

            }
        }
        IEnumerator ShowRewardMenu(int seconds)
        {
            RewardData reward = m_RewardData[m_RewardNumber];
            yield return new WaitForSeconds(seconds);
            if (reward.m_Type != "nothing")
            {
                m_RewardPanel.gameObject.SetActive(true);
                m_RewardFinalText.text = reward.m_Count.ToString();
                m_RewardFinalImage.sprite = reward.m_Icon;
                yield return new WaitForSeconds(2);
            }

            yield return new WaitForSeconds(.1f);
            Reset();
        }

        public void StartSpin()
        {
            if (!m_IsSpinning)
            {
                m_SpinSpeed = Random.Range(4f, 14f);
                m_IsSpinning = true;
                m_RewardNumber = -1;
                m_SpinButton.gameObject.SetActive(false);
            }
        }

        public void Reset()
        {
            m_Rotation = 0;
            m_CircleBase.transform.localRotation = Quaternion.identity;
            m_IsSpinning = false;
            m_RewardNumber = -1;
            m_SpinButton.gameObject.SetActive(true);
            m_RewardPanel.gameObject.SetActive(false);
        }
    }
}
