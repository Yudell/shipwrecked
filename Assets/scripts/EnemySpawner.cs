using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Ссылка на префаб врага
    public float minSpawnInterval = 1f; // Минимальный интервал спавна (в секундах)
    public float maxSpawnInterval = 3f; // Максимальный интервал спавна (в секундах)
    private float[] railX; // Массив позиций рельсов по X
    private float topY; // Верхняя граница экрана

    void Start()
    {
        // Получаем главную камеру
        Camera camera = Camera.main;
        float size = camera.orthographicSize; // Половина высоты экрана
        float aspect = camera.aspect; // Соотношение сторон

        // Вычисляем левую и правую границы экрана
        float leftX = camera.transform.position.x - aspect * size;
        float rightX = camera.transform.position.x + aspect * size;

        // Верхняя граница экрана
        topY = camera.transform.position.y + size;

        // Инициализируем массив позиций рельсов
        railX = new float[5];
        for (int i = 0; i < 5; i++)
        {
            // Равномерно распределяем рельсы между leftX и rightX
            railX[i] = leftX + (rightX - leftX) * (i + 0.5f) / 5f;
        }

        // Запускаем корутину для спавна врагов
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Выбираем случайный рельс
            int railIndex = Random.Range(0, 5);
            float x = railX[railIndex];

            // Позиция спавна: X — позиция рельса, Y — верх экрана
            Vector3 spawnPosition = new Vector3(x, topY, 0);

            // Создаём врага
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Генерируем случайный интервал для следующего спавна
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);

            // Ждём случайное время перед следующим спавном
            yield return new WaitForSeconds(randomInterval);
        }
    }
}