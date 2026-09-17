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

        /// Agrega arista entre nodos u y v (grafo no dirigido)
        public void AgregarArista(int u, int v)
        {
            ValidarNodo(u, nameof(u));
            ValidarNodo(v, nameof(v));

            // Grafo no dirigido: agregamos en ambas direcciones
            listaAdj[u].Add(v);
            listaAdj[v].Add(u);
        }

        /// Retorna lista de vecinos de un nodo
        public List<int> ObtenerVecinos(int nodo)
        {
            ValidarNodo(nodo, nameof(nodo));
            return new List<int>(listaAdj[nodo]);
        }

        /// BFS: Recorrido en anchura desde nodo inicial
        /// - Usa cola para procesar nodos en orden FIFO
        /// - Visita todos los vecinos inmediatos antes de avanzar al siguiente nivel
        public string BFS(int inicio)
        {
            StringBuilder resultado = new StringBuilder();

            if (numNodos == 0)
            {
                resultado.AppendLine("El grafo está vacío. No hay nodos para recorrer.");
                return resultado.ToString();
            }

            ValidarNodo(inicio, nameof(inicio));

            // Marcamos todos los nodos como no visitados
            bool[] visitado = new bool[numNodos];

            // Cola para BFS
            Queue<int> cola = new Queue<int>();

            // Marcamos el nodo inicial como visitado y lo añadimos a la cola
            visitado[inicio] = true;
            cola.Enqueue(inicio);

            resultado.AppendLine($"Iniciando BFS desde el nodo {inicio}");
            resultado.Append("Orden de visita: ");

            int paso = 1;
            while (cola.Count > 0)
            {
                // Extraer un nodo de la cola
                int nodoActual = cola.Dequeue();

                // Colorear el nodo actual
                ColorearNodo(nodoActual, Color.LightGreen);

                // Imprimir el nodo
                resultado.Append($"{nodoActual} ");

                // Esperar un poco para visualizar el proceso
                System.Threading.Thread.Sleep(500);

                // Buscar todos los nodos adyacentes no visitados
                foreach (int vecino in listaAdj[nodoActual])
                {
                    if (!visitado[vecino])
                    {
                        // Marcar como visitado y añadir a la cola
                        visitado[vecino] = true;
                        cola.Enqueue(vecino);

                        // Colorear el vecino
                        ColorearNodo(vecino, Color.Yellow);

                        // Mostrar paso actual
                        resultado.AppendLine();
                        resultado.AppendLine($"Paso {paso++}: Visitando vecino {vecino} del nodo {nodoActual}");

                        // Esperar para visualizar
                        System.Threading.Thread.Sleep(300);
                    }
                }
            }

            resultado.AppendLine();
            resultado.AppendLine("\nRecorrido BFS completado!");

            return resultado.ToString();
        }

        /// DFS: Recorrido en profundidad iterativo
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

            // Marcamos todos los nodos como no visitados
            bool[] visitado = new bool[numNodos];

            // Pila para DFS
            Stack<int> pila = new Stack<int>();

            // Añadimos el nodo inicial a la pila
            pila.Push(inicio);

            resultado.AppendLine($"Iniciando DFS desde el nodo {inicio}");
            resultado.Append("Orden de visita: ");

            int paso = 1;
            while (pila.Count > 0)
            {
                // Extraer un nodo de la pila
                int nodoActual = pila.Pop();

                // Si el nodo no ha sido visitado
                if (!visitado[nodoActual])
                {
                    // Marcar el nodo como visitado e imprimir
                    visitado[nodoActual] = true;
                    resultado.Append($"{nodoActual} ");

                    // Colorear el nodo actual
                    ColorearNodo(nodoActual, Color.LightCoral);

                    // Esperar un poco para visualizar el proceso
                    System.Threading.Thread.Sleep(500);

                    // Añadir vecinos en orden inverso
                    List<int> vecinos = new List<int>(listaAdj[nodoActual]);
                    vecinos.Reverse(); // Para que el recorrido sea similar al DFS recursivo

                    foreach (int vecino in vecinos)
                    {
                        if (!visitado[vecino])
                        {
                            pila.Push(vecino);

                            // Colorear el vecino
                            ColorearNodo(vecino, Color.LightBlue);

                            // Mostrar paso actual
                            resultado.AppendLine();
                            resultado.AppendLine($"Paso {paso++}: Añadiendo vecino {vecino} del nodo {nodoActual} a la pila");

                            // Esperar para visualizar
                            System.Threading.Thread.Sleep(300);
                        }
                    }
                }
            }

            resultado.AppendLine();
            resultado.AppendLine("\nRecorrido DFS completado!");

            return resultado.ToString();
        }

        /// DFS Recursivo: Implementación alternativa que usa la pila de llamadas
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

            bool[] visitado = new bool[numNodos];

            resultado.AppendLine($"Iniciando DFS Recursivo desde el nodo {inicio}");
            resultado.Append("Orden de visita: ");

            // Llamada al método auxiliar recursivo
            DFSRecursivoUtil(inicio, visitado, resultado, 1);

            resultado.AppendLine();
            resultado.AppendLine("\nRecorrido DFS Recursivo completado!");

            return resultado.ToString();
        }

        /// Función auxiliar que implementa la lógica recursiva del DFS
        /// - Marca el nodo actual y explora sus vecinos recursivamente
        private int DFSRecursivoUtil(int nodo, bool[] visitado, StringBuilder resultado, int paso)
        {
            // Marcar nodo como visitado
            visitado[nodo] = true;
            resultado.Append($"{nodo} ");

            // Colorear el nodo actual
            ColorearNodo(nodo, Color.LightCoral);

            // Pausa para visualización
            System.Threading.Thread.Sleep(500);

            // Explorar vecinos no visitados
            foreach (int vecino in listaAdj[nodo])
            {
                if (!visitado[vecino])
                {
                    // Colorear el vecino
                    ColorearNodo(vecino, Color.LightBlue);

                    // Mostrar paso
                    resultado.AppendLine();
                    resultado.AppendLine($"Paso {paso++}: Explorando vecino {vecino} del nodo {nodo}");

                    // Pausa para visualización
                    System.Threading.Thread.Sleep(300);

                    // Llamada recursiva
                    paso = DFSRecursivoUtil(vecino, visitado, resultado, paso);
                }
            }

            return paso;
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