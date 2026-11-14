# TP2_Recuperatorio
---
# Prototipo 3D - Terrain y NavMesh 

* **Alumna:** Tactaca,Cecilia Yazmin 
* **Materia:** Programación de Videojuegos 1 
* **Carrera:** Tecnicatura Universitaria en Diseño Integral de Videojuegos (TUDIVJ) — UNJu 
* **Trabajo Práctico N° 2:** Integración de mecánicas 3D por Raycast, sistemas de navegación de enemigos con NavMesh, Terrenos, gestión de salud/puntuación y control centralizado de juego. Estación de Curación y versionamiento en Git/GitHub.
* **Equipo Docente:** Mg. Ing. Ariel Alejandro Vega | Tecn. Kevin Alexis Roman Llampa 
--- 
## 🎮 Descripción del Proyecto  

El Player (una capsula) debe recorrer el terrreno, recoger monedas y llegar a una meta, para ello debe hackear unas computadoras y abrir puertas. En el terreno hay trampas como un terreno lodoso que le resta velocidad y una zona toxica. 
También se encontrará con centinelas que lo atacaran, de los cuales se puede defender con un rayo paralizante.

--- 

## 🕹️ Controles del Jugador 
| Acción | Tecla / Botón | Descripción | 
| :--- | :--- | :--- | 
| **Movimiento** | `W`, `A`, `S`, `D` / Flechas | Mover al personaje por el escenario | 
| **Salto** | `Espacio` | Saltar entre plataformas | 
| **Interactuar / Recolectar** | `E` / Contacto con zona(`Trigger`) | Desactiva computadoras | 
| **Disparar** | `Mouse Botón Izquierdo` | Paraliza al enemigo por un tiempo | 

--- 
## 📸 Capturas de Pantalla  

![Vista General del Escenario]()
*Figura 1: Layout del escenario (Greyboxing) mostrando el punto de inicio.*
![Vista General del Escenario](docs/jueg)
*Figura 2: Zona de las plataformas móviles.*
![Vista General del Escenario](docs/jue)
*Figura 3: Zona de spawn y la zona de meta.*
--- 
## ⚙️ Mecánicas Implementadas

1. **Control de Personaje (3D Movement):** Movimiento fluido en tercera persona con detección de suelo y capacidad de salto.
2. **Disparo con Raycast:** Disparo que detiene a los centinelas que lo atacan.
3. **Raycast para interactuar con objetos:** Hackea computadoras para abrir puerta si las detecta con raycast y presiona E.
4. **Sistema de Meta:** Zona de meta (*Trigger*) que valida la condición de victoria.
5. **Zona lodosa:** Zona (*Trigger*) que le quita velocidad al Player.
6. **Zona Tóxica:** Zona (*Trigger*) que le quita HP al Player.
---
## 🛠️ Versión de Unity
* **Unity 6 LTS** (Versión utilizada: `6000.5.8f1`)
* **Pipeline de Renderizado:** Universal Render Pipeline (URP) *

## 🛠️ Polytope Studio
* **Asset de la Asset Store de Unity
* Se utilizó este asset para crear el terrain.
---
## 📝 Explicación Técnica de Codigo: 

### 1. Manejo de Estados en el juego

* **Script:** `GameManager.cs` 
* **Funcionamiento:** Se utilizó un enum para definir los estados del juego.
```csharp
      public enum GameState
{
    Start,
    Playing,
    Paused,
    Victory,
    GameOver
}
```
### 2. Manejo de la zona tóxica

* **Script:** `ToxicZone.cs`
* **Funcionamiento:** Se utilizó IEnumerator para que le restara salud al Player continuamente.
```csharp
    IEnumerator DamageRoutine()
    {

        if (zoneRenderer != null)
        {
            zoneRenderer.enabled = true;
        }

        Debug.Log("<color=orange>¡Entraste a una zona tóxica!</color>");

        while (currentPlayerHealth != null && !currentPlayerHealth.IsDead)
        {
            yield return new WaitForSeconds(damageInterval);

            if (currentPlayerHealth != null && !currentPlayerHealth.IsDead)
            {
                currentPlayerHealth.TakeDamage(damageAmount);
            }
        }

        StopDamage();
    }
```
### 3. Conteo de monedas

* **Script:** `ScoreManager.cs`
* **Funcionamiento:** Se utilizó un TextMeshPro para mostrar cuantas monedas recolecta el Player.
```csharp
     public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.NotifyPlayerStatusChanged();
        }
    }

    private void UpdateScoreUI()
    {
        if (pointsText != null)
        {
            pointsText.text = "Monedas " + score.ToString();
        }
    }
```
