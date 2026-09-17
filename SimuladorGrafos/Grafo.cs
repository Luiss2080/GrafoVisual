using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimuladorGrafos
{
    public class Grafo
    {
        private int numNodos;
        private List<List<int>> listaAdj;
        private List<Button> nodosButtons;

        /// Constructor: Inicializa grafo con n nodos y los botones asociados
        public Grafo(int n, List<Button> botones)
        {
            if (n < 0)
                throw new ArgumentOutOfRangeException(nameof(n), n, "El número de nodos no puede ser negativo.");

            numNodos = n;
            listaAdj = new List<List<int>>();
            // Un grafo puede crearse sin botones asociados (p. ej. en pruebas unitarias);
            // en ese caso simplemente no se colorea ningún nodo.
            nodosButtons = botones ?? new List<Button>();

            // Inicializar la lista de adyacencia
            for (int i = 0; i < n; i++)
            {
                listaAdj.Add(new List<int>());
            }
        }

        /// Número de nodos del grafo.
        public int NumNodos => numNodos;

        /// Valida que el índice de nodo exista en el grafo; lanza una excepción clara si no.
        private void ValidarNodo(int nodo, string nombreParametro)
        {
            if (nodo < 0 || nodo >= numNodos)
            {
                throw new ArgumentOutOfRangeException(
                    nombreParametro,
                    nodo,
                    $"El nodo {nodo} no existe. El grafo tiene {numNodos} nodo(s) (índices válidos: 0 a {numNodos - 1}).");
            }
        }

        /// Agrega arista entre nodos u y v (grafo no dirigido).
        /// - Es idempotente: agregar la misma arista dos veces no duplica la conexión.
        /// - Soporta auto-bucles (u == v) sin duplicar la entrada en la lista de adyacencia.
        public void AgregarArista(int u, int v)
        {
            ValidarNodo(u, nameof(u));
            ValidarNodo(v, nameof(v));

            if (u == v)
            {
                // Auto-bucle: una sola entrada es suficiente para representarlo.
                // (Antes se agregaba dos veces al mismo tiempo: listaAdj[u].Add(v) y
                // listaAdj[v].Add(u) apuntan a la misma lista cuando u == v.)
                if (!listaAdj[u].Contains(v))
                    listaAdj[u].Add(v);
                return;
            }

            // Grafo no dirigido: agregamos en ambas direcciones, evitando duplicados
            if (!listaAdj[u].Contains(v))
                listaAdj[u].Add(v);
            if (!listaAdj[v].Contains(u))
                listaAdj[v].Add(u);
        }

        /// Retorna lista de vecinos de un nodo
        public List<int> ObtenerVecinos(int nodo)
        {
            ValidarNodo(nodo, nameof(nodo));
            return new List<int>(listaAdj[nodo]);
        }

        /// Núcleo del recorrido BFS, sin efectos de UI ni pausas: es la única fuente de
        /// verdad del algoritmo. Tanto BFS() (usado por la interfaz gráfica) como
        /// BFSOrden() (usado por las pruebas unitarias y por código que solo necesita el
        /// resultado) llaman a este método, así que no pueden divergir entre sí.
        /// - alVisitar(nodo): se invoca cuando un nodo se extrae de la cola (orden de visita).
        /// - alDescubrir(vecino, origen): se invoca cuando un vecino no visitado se encola.
        private void RecorrerBFS(int inicio, Action<int> alVisitar, Action<int, int> alDescubrir)
        {
            bool[] visitado = new bool[numNodos];
            Queue<int> cola = new Queue<int>();

            visitado[inicio] = true;
            cola.Enqueue(inicio);

            while (cola.Count > 0)
            {
                int nodoActual = cola.Dequeue();
                alVisitar?.Invoke(nodoActual);

                foreach (int vecino in listaAdj[nodoActual])
                {
                    if (!visitado[vecino])
                    {
                        visitado[vecino] = true;
                        cola.Enqueue(vecino);
                        alDescubrir?.Invoke(vecino, nodoActual);
                    }
                }
            }
        }

        /// Orden de visita BFS puro (sin UI, sin pausas): útil para pruebas unitarias y
        /// para cualquier consumidor que solo necesite el resultado del algoritmo.
        /// - En un grafo desconectado, solo devuelve los nodos alcanzables desde `inicio`.
        /// - En un grafo con ciclos, el arreglo de visitados evita bucles infinitos.
        public List<int> BFSOrden(int inicio)
        {
            var orden = new List<int>();
            if (numNodos == 0)
                return orden;

            ValidarNodo(inicio, nameof(inicio));
            RecorrerBFS(inicio, nodo => orden.Add(nodo), null);
            return orden;
        }

        /// BFS: Recorrido en anchura desde nodo inicial, con visualización en la UI.
        /// - Usa cola para procesar nodos en orden FIFO
        /// - Visita todos los vecinos inmediatos antes de avanzar al siguiente nivel
        /// - Encuentra el camino más corto medido en número de saltos (el grafo no tiene
        ///   pesos en las aristas), no el camino de menor "costo" en un grafo ponderado.
        public string BFS(int inicio)
        {
            StringBuilder resultado = new StringBuilder();

            if (numNodos == 0)
            {
                resultado.AppendLine("El grafo está vacío. No hay nodos para recorrer.");
                return resultado.ToString();
            }

            ValidarNodo(inicio, nameof(inicio));

            resultado.AppendLine($"Iniciando BFS desde el nodo {inicio}");
            resultado.Append("Orden de visita: ");

            int paso = 1;
            RecorrerBFS(
                inicio,
                alVisitar: nodoActual =>
                {
                    ColorearNodo(nodoActual, Color.LightGreen);
                    resultado.Append($"{nodoActual} ");
                    System.Threading.Thread.Sleep(500);
                },
                alDescubrir: (vecino, nodoActual) =>
                {
                    ColorearNodo(vecino, Color.Yellow);
                    resultado.AppendLine();
                    resultado.AppendLine($"Paso {paso++}: Visitando vecino {vecino} del nodo {nodoActual}");
                    System.Threading.Thread.Sleep(300);
                });

            resultado.AppendLine();
            resultado.AppendLine("\nRecorrido BFS completado!");

            return resultado.ToString();
        }

        /// Núcleo del recorrido DFS iterativo, sin efectos de UI ni pausas. DFS() y
        /// DFSOrden() llaman a este mismo método para que ambos no puedan divergir.
        /// - alVisitar(nodo): se invoca cuando un nodo se marca como visitado (al desapilarlo).
        /// - alApilar(vecino, origen): se invoca cuando un vecino no visitado se apila.
        private void RecorrerDFS(int inicio, Action<int> alVisitar, Action<int, int> alApilar)
        {
            bool[] visitado = new bool[numNodos];
            Stack<int> pila = new Stack<int>();

            pila.Push(inicio);

            while (pila.Count > 0)
            {
                int nodoActual = pila.Pop();

                // Un nodo puede haber sido apilado más de una vez (p. ej. si dos nodos ya
                // visitados comparten un vecino), por eso el chequeo de visitado se hace
                // aquí, al desapilar, y no al apilar. Esto es lo que evita bucles
                // infinitos en grafos cíclicos.
                if (!visitado[nodoActual])
                {
                    visitado[nodoActual] = true;
                    alVisitar?.Invoke(nodoActual);

                    // Añadir vecinos en orden inverso para que el recorrido sea similar al DFS recursivo
                    List<int> vecinos = new List<int>(listaAdj[nodoActual]);
                    vecinos.Reverse();

                    foreach (int vecino in vecinos)
                    {
                        if (!visitado[vecino])
                        {
                            pila.Push(vecino);
                            alApilar?.Invoke(vecino, nodoActual);
                        }
                    }
                }
            }
        }

        /// Orden de visita DFS iterativo puro (sin UI, sin pausas).
        public List<int> DFSOrden(int inicio)
        {
            var orden = new List<int>();
            if (numNodos == 0)
                return orden;

            ValidarNodo(inicio, nameof(inicio));
            RecorrerDFS(inicio, nodo => orden.Add(nodo), null);
            return orden;
        }

        /// DFS: Recorrido en profundidad iterativo, con visualización en la UI.
        /// - Usa pila para explorar caminos completos antes de retroceder
        /// - Visita cada rama hasta el final antes de explorar otras ramas
        public string DFS(int inicio)
        {
            StringBuilder resultado = new StringBuilder();

            if (numNodos == 0)
            {
                resultado.AppendLine("El grafo está vacío. No hay nodos para recorrer.");
                return resultado.ToString();
            }

            ValidarNodo(inicio, nameof(inicio));

            resultado.AppendLine($"Iniciando DFS desde el nodo {inicio}");
            resultado.Append("Orden de visita: ");

            int paso = 1;
            RecorrerDFS(
                inicio,
                alVisitar: nodoActual =>
                {
                    resultado.Append($"{nodoActual} ");
                    ColorearNodo(nodoActual, Color.LightCoral);
                    System.Threading.Thread.Sleep(500);
                },
                alApilar: (vecino, nodoActual) =>
                {
                    ColorearNodo(vecino, Color.LightBlue);
                    resultado.AppendLine();
                    resultado.AppendLine($"Paso {paso++}: Añadiendo vecino {vecino} del nodo {nodoActual} a la pila");
                    System.Threading.Thread.Sleep(300);
                });

            resultado.AppendLine();
            resultado.AppendLine("\nRecorrido DFS completado!");

            return resultado.ToString();
        }

        /// Núcleo del recorrido DFS recursivo, sin efectos de UI ni pausas. DFSRecursivo()
        /// y DFSOrdenRecursivo() llaman a este mismo método para que ambos no puedan
        /// divergir.
        private void RecorrerDFSRecursivo(int nodo, bool[] visitado, Action<int> alVisitar, Action<int, int> alDescubrir)
        {
            visitado[nodo] = true;
            alVisitar?.Invoke(nodo);

            foreach (int vecino in listaAdj[nodo])
            {
                if (!visitado[vecino])
                {
                    alDescubrir?.Invoke(vecino, nodo);
                    RecorrerDFSRecursivo(vecino, visitado, alVisitar, alDescubrir);
                }
            }
        }

        /// Orden de visita DFS recursivo puro (sin UI, sin pausas).
        public List<int> DFSOrdenRecursivo(int inicio)
        {
            var orden = new List<int>();
            if (numNodos == 0)
                return orden;

            ValidarNodo(inicio, nameof(inicio));
            bool[] visitado = new bool[numNodos];
            RecorrerDFSRecursivo(inicio, visitado, nodo => orden.Add(nodo), null);
            return orden;
        }

        /// DFS Recursivo: Implementación alternativa que usa la pila de llamadas, con
        /// visualización en la UI.
        /// - Más natural para mostrar la recursividad del algoritmo
        public string DFSRecursivo(int inicio)
        {
            StringBuilder resultado = new StringBuilder();

            if (numNodos == 0)
            {
                resultado.AppendLine("El grafo está vacío. No hay nodos para recorrer.");
                return resultado.ToString();
            }

            ValidarNodo(inicio, nameof(inicio));

            resultado.AppendLine($"Iniciando DFS Recursivo desde el nodo {inicio}");
            resultado.Append("Orden de visita: ");

            bool[] visitado = new bool[numNodos];
            int paso = 1;
            RecorrerDFSRecursivo(
                inicio,
                visitado,
                alVisitar: nodo =>
                {
                    resultado.Append($"{nodo} ");
                    ColorearNodo(nodo, Color.LightCoral);
                    System.Threading.Thread.Sleep(500);
                },
                alDescubrir: (vecino, nodo) =>
                {
                    ColorearNodo(vecino, Color.LightBlue);
                    resultado.AppendLine();
                    resultado.AppendLine($"Paso {paso++}: Explorando vecino {vecino} del nodo {nodo}");
                    System.Threading.Thread.Sleep(300);
                });

            resultado.AppendLine();
            resultado.AppendLine("\nRecorrido DFS Recursivo completado!");

            return resultado.ToString();
        }

        /// Actualiza el color del botón asociado al nodo para visualización
        private void ColorearNodo(int nodo, Color color)
        {
            if (nodo >= 0 && nodo < nodosButtons.Count)
            {
                // Asegurarse de que el cambio de color ocurra en el hilo de la UI
                if (nodosButtons[nodo].InvokeRequired)
                {
                    nodosButtons[nodo].Invoke(new Action(() => nodosButtons[nodo].BackColor = color));
                }
                else
                {
                    nodosButtons[nodo].BackColor = color;
                }

                // Aplicar cambios inmediatamente
                Application.DoEvents();
            }
        }
    }
}