using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Realyteam.Player
{
    public class HealtImageSpawner : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject imagePrefab; // Prefab del Image
        public Transform spawnPoint;  // Punto donde se instanciar�
        public float animationDuration = 2f; // Duraci�n de la animaci�n (en segundos)
        public float moveDistance = 50f; // Distancia que subir� la imagen
        public float fadeDuration = 1f; // Duraci�n del desvanecimiento (en segundos)

        private Coroutine spawnCoroutine;

        private void Update()
        {
            // Verificar si el jugador est� en la zona de curaci�n a trav�s del Singleton BubbleController
            if (BubbleController.Instance.OnHealthRestoring)
            {
                // Si est� en la zona y no se est� ejecutando el proceso, iniciar el ciclo
                if (spawnCoroutine == null)
                {
                    spawnCoroutine = StartCoroutine(SpawnAndAnimate());
                }
            }
            else
            {
                // Si sale de la zona, detener el ciclo y destruir los objetos existentes
                if (spawnCoroutine != null)
                {
                    StopCoroutine(spawnCoroutine);
                    spawnCoroutine = null;
                }
            }
        }

        private IEnumerator SpawnAndAnimate()
        {
            while (true)
            {
                // Instanciar la imagen en el punto de spawn
                GameObject spawnedImage = Instantiate(imagePrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);

                // Obtener el componente RectTransform y CanvasGroup
                RectTransform rectTransform = spawnedImage.GetComponent<RectTransform>();
                CanvasGroup canvasGroup = spawnedImage.GetComponent<CanvasGroup>();

                if (canvasGroup == null)
                {
                    // Agregar CanvasGroup si no existe
                    canvasGroup = spawnedImage.AddComponent<CanvasGroup>();
                }

                // Inicializar valores
                canvasGroup.alpha = 1f;

                Vector3 startPosition = rectTransform.anchoredPosition;
                Vector3 endPosition = startPosition + new Vector3(0, moveDistance, 0);

                float elapsedTime = 0f;

                // Animaci�n de movimiento y desvanecimiento
                while (elapsedTime < animationDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float progress = elapsedTime / animationDuration;

                    // Mover la imagen
                    rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, progress);

                    // Hacer que la imagen mire hacia la c�mara principal
                    if (Camera.main != null)
                    {
                        Vector3 cameraPosition = Camera.main.transform.position;
                        spawnedImage.transform.LookAt(cameraPosition);
                        // Ajustar la rotaci�n para que la imagen no quede invertida
                        spawnedImage.transform.Rotate(0, 180, 0);
                    }

                    // Desvanecer si es el momento adecuado
                    if (elapsedTime > animationDuration - fadeDuration)
                    {
                        float fadeProgress = (elapsedTime - (animationDuration - fadeDuration)) / fadeDuration;
                        canvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeProgress);
                    }

                    yield return null;
                }

                // Destruir el objeto despu�s de la animaci�n
                Destroy(spawnedImage);

                // Esperar un peque�o intervalo antes de repetir
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
