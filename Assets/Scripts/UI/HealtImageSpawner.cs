using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Realyteam.Player
{
    public class HealtImageSpawner : MonoBehaviour
    {
        [Header("Settings")]
        public GameObject imagePrefab; // Prefab del Image
        public Transform spawnPoint;  // Punto donde se instanciará
        public float animationDuration = 2f; // Duración de la animación (en segundos)
        public float moveDistance = 50f; // Distancia que subirá la imagen
        public float fadeDuration = 1f; // Duración del desvanecimiento (en segundos)

        private Coroutine spawnCoroutine;

        private void Update()
        {
            // Verificar si el jugador está en la zona de curación a través del Singleton BubbleController
            if (BubbleController.Instance.OnHealthRestoring)
            {
                // Si está en la zona y no se está ejecutando el proceso, iniciar el ciclo
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

                // Animación de movimiento y desvanecimiento
                while (elapsedTime < animationDuration)
                {
                    elapsedTime += Time.deltaTime;
                    float progress = elapsedTime / animationDuration;

                    // Mover la imagen
                    rectTransform.anchoredPosition = Vector3.Lerp(startPosition, endPosition, progress);

                    // Hacer que la imagen mire hacia la cámara principal
                    if (Camera.main != null)
                    {
                        Vector3 cameraPosition = Camera.main.transform.position;
                        spawnedImage.transform.LookAt(cameraPosition);
                        // Ajustar la rotación para que la imagen no quede invertida
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

                // Destruir el objeto después de la animación
                Destroy(spawnedImage);

                // Esperar un pequeño intervalo antes de repetir
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
