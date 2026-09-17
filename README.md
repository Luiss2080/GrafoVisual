# 🕸️ GrafoVisual

> Visualizador interactivo de BFS y DFS sobre un grafo no dirigido en C# y
> Windows Forms: pensado para estudiantes y docentes que quieren *ver*, paso a
> paso y con colores, cómo se comportan realmente estos dos algoritmos de
> recorrido, en vez de solo leerlos en un pizarrón.

## Características

- **Grafo no dirigido de 20 nodos**, con una estructura jerárquica fija
  definida en código (`Form1.InicializarGrafo`) y dibujada automáticamente al
  iniciar la app: nodos como botones circulares y aristas como líneas (con
  flecha en las conexiones "padre-hijo" del árbol base).
- **BFS y DFS (iterativo) ejecutables desde cualquier nodo**, elegido con un
  control numérico, con animación en vivo (cada nodo cambia de color al
  visitarse) y una bitácora de texto paso a paso del recorrido.
- **BFS encuentra el camino más corto medido en número de saltos**, no en
  "costo" o distancia ponderada: las aristas de este grafo no tienen peso.
  DFS explora en profundidad una rama completa antes de retroceder. Si
  buscas rutas óptimas en un grafo con pesos reales, este proyecto no lo
  implementa (sería un algoritmo distinto, como Dijkstra).
- **Validación de entradas en el motor del grafo** (`Grafo.cs`): un nodo
  inicial fuera de rango o un grafo vacío producen un error controlado en
  vez de un `IndexOutOfRangeException`; agregar la misma arista dos veces o
  un auto-bucle no corrompe la lista de adyacencia.
- **Núcleo de los algoritmos separado de la interfaz gráfica**
  (`BFSOrden`/`DFSOrden`/`DFSOrdenRecursivo`), lo que permite probarlo con
  pruebas unitarias reales y rápidas (sin abrir ningún formulario).

### Lo que este proyecto **no** hace (para ser honestos)

La estructura del grafo (20 nodos, sus aristas) está fija en el código: la
interfaz **no** permite agregar, eliminar o editar nodos/aristas de forma
visual, ni tiene detección de ciclos o análisis de conectividad. Si
necesitas eso, es una extensión pendiente, no una funcionalidad actual.

## Cómo usar

1. Abre `SimuladorGrafos.sln` en Visual Studio (con la carga de trabajo
   *.NET desktop development*) y presiona **F5**, o compílalo desde la
   línea de comandos (ver más abajo) y ejecuta el `.exe` generado.
2. Elige el **nodo inicial** con el control numérico (0 a 19).
3. Pulsa **"Ejecutar BFS"** o **"Ejecutar DFS"** y observa cómo se colorean
   los nodos en el orden en que el algoritmo los visita, mientras el cuadro
   de texto de la derecha muestra el detalle paso a paso.

## Instalación y uso local

Requiere Windows y el SDK de .NET (el proyecto compila sobre .NET Framework
4.7.2; Visual Studio o el SDK de .NET instalan las herramientas necesarias).

```bash
git clone https://github.com/Luiss2080/GrafoVisual.git
cd GrafoVisual

# Compilar la app y el proyecto de pruebas
dotnet build SimuladorGrafos.sln

# Ejecutar la app (WinForms; requiere Windows)
./SimuladorGrafos/bin/Debug/SimuladorGrafos.exe
```

## Tecnologías

- **C#** sobre **.NET Framework 4.7.2** (`SimuladorGrafos.csproj`)
- **Windows Forms** para la interfaz, con dibujo personalizado de nodos y
  aristas mediante `System.Drawing`/GDI+
- **xUnit** para las pruebas unitarias del motor del grafo
- **GitHub Actions** para build y pruebas automáticas en cada cambio
  (`.github/workflows/build-and-test.yml`)

## Tests

El núcleo de BFS/DFS (`Grafo.cs`) está cubierto por 21 pruebas unitarias que
no dependen de la interfaz gráfica: grafos conectados y desconectados,
grafos con ciclos (verificando que no haya bucles infinitos), auto-bucles,
aristas duplicadas y casos límite (nodo inicial inválido, grafo vacío, un
solo nodo).

```bash
dotnet test SimuladorGrafos.Tests/SimuladorGrafos.Tests.csproj
```

## Licencia

MIT. Consulta el archivo [`LICENSE`](LICENSE).
