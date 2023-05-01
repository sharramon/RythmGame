using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    [SerializeField] private Color m_firstColor;
    [SerializeField] private Color m_secondColor;
    [SerializeField] private Color m_thirdColor;
    [SerializeField] private Color m_baseColor;
    [SerializeField] private float m_changeColorSpeed;
    [SerializeField] private float m_baseColorDuration;
    [SerializeField] private GameObject m_pointLight;
    private Material m_material;
    private Color[] m_colors;
    private int m_currentColorIndex;
    private float m_colorTimer;
    private float m_baseColorTimer;
    private bool m_isOnBaseColor;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        m_material = renderer.material;

        // Create an array of colors in the order they should be cycled
        m_colors = new Color[] { m_firstColor, m_secondColor, m_thirdColor };
        m_currentColorIndex = 0;
        m_colorTimer = 0f;
        m_baseColorTimer = 0f;
        m_isOnBaseColor = false;

        // Set the initial color to the first color in the array
        m_material.color = m_colors[m_currentColorIndex];
        m_material.EnableKeyword("_EMISSION");
        m_material.SetColor("_EmissionColor", Color.black);
    }

    private void Update()
    {
        if (m_isOnBaseColor)
        {
            // If we're on the base color, increment the timer until we've waited for 2 seconds
            m_baseColorTimer += Time.deltaTime;
            if (m_baseColorTimer >= m_baseColorDuration)
            {
                // Once we've waited for 2 seconds, turn off emission and switch back to cycling colors
                m_isOnBaseColor = false;
                m_pointLight.SetActive(false);
                m_baseColorTimer = 0f;
                m_material.SetColor("_EmissionColor", Color.black);
            }
            else
            {
                if (m_pointLight.activeSelf == false)
                    m_pointLight.SetActive(true);

                m_material.SetColor("_EmissionColor", Color.white * 2f);
            }
        }
        else
        {
            // If we're cycling colors, increment the color timer
            m_colorTimer += Time.deltaTime;

            // If the timer has exceeded the change color speed, switch to the next color
            if (m_colorTimer >= m_changeColorSpeed)
            {
                // Reset the timer
                m_colorTimer = 0f;

                // Increment the color index and wrap around if necessary
                m_currentColorIndex = (m_currentColorIndex + 1) % m_colors.Length;

                // If we've cycled through all the colors, switch to the base color
                if (m_currentColorIndex == 0)
                {
                    m_isOnBaseColor = true;
                    m_material.color = m_colors[m_currentColorIndex];
                }
                else
                {
                    // Otherwise, set the material color to the new color
                    m_material.color = m_colors[m_currentColorIndex];
                }
            }
        }
    }
}
