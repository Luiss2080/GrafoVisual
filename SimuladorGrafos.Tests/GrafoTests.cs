using System;
using System.Collections.Generic;
using SimuladorGrafos;
using Xunit;

namespace SimuladorGrafos.Tests
{
    /// <summary>
    /// Pruebas del núcleo de BFS/DFS de <see cref="Grafo"/>, completamente
    /// desacopladas de la interfaz gráfica: se construye el grafo sin botones
    /// (null) y se usan los métodos *Orden(), que no colorean nada ni hacen
    /// Thread.Sleep, para que las pruebas sean rápidas y deterministas.
    /// </summary>
    public class GrafoTests
    {
        private static Grafo GrafoSinUI(int n) => new Grafo(n, null);

        // ---------------------------------------------------------------
        // BFS: corrección básica y "camino más corto" en número de saltos
        // ---------------------------------------------------------------

        [Fact]
        public void BFS_VisitaTodosLosNodosDeUnGrafoConectado()
        {
            // 0 - 1 - 2
            // |
            // 3
            var g = GrafoSinUI(4);
            g.AgregarArista(0, 1);
            g.AgregarArista(1, 2);
            g.AgregarArista(0, 3);

            var orden = g.BFSOrden(0);

            Assert.Equal(4, orden.Count);
            Assert.Equal(new HashSet<int> { 0, 1, 2, 3 }, new HashSet<int>(orden));
            Assert.Equal(0, orden[0]); // el primer nodo visitado es siempre el de inicio
        }

        [Fact]
        public void BFS_VisitaNodosEnOrdenDeSaltosCrecientes()
        {
            // Cadena lineal: 0 - 1 - 2 - 3 - 4
            // BFS debe visitar en orden estricto de distancia (hop count) desde el inicio.
            var g = GrafoSinUI(5);
            g.AgregarArista(0, 1);
            g.AgregarArista(1, 2);
            g.AgregarArista(2, 3);
            g.AgregarArista(3, 4);

            var orden = g.BFSOrden(0);

            Assert.Equal(new List<int> { 0, 1, 2, 3, 4 }, orden);
        }

        [Fact]
        public void BFS_EncuentraElMenorNumeroDeSaltosNoNecesariamenteElMenorPeso()
        {
            // Dos caminos de 0 a 3: uno directo de 1 salto (0-3) y otro de 3 saltos
            // (0-1-2-3). Un grafo "ponderado" honesto preferiría el camino de menor
            // costo total, pero este grafo no tiene pesos: BFS siempre preferirá el
            // camino con MENOS aristas, sin importar cuánto "costaría" en un modelo
            // ponderado. Esta prueba documenta explícitamente esa semántica (ver
            // README: BFS aquí es "saltos mínimos", no "ruta óptima ponderada").
            var g = GrafoSinUI(4);
            g.AgregarArista(0, 1);
            g.AgregarArista(1, 2);
            g.AgregarArista(2, 3);
            g.AgregarArista(0, 3); // atajo directo de un solo salto

            var orden = g.BFSOrden(0);

            // Con el atajo, tanto 1 como 3 están a 1 salto de 0 (primer nivel), y 2 está
            // a 2 saltos (solo alcanzable pasando por 1 o por 3). BFS debe visitar el
            // nodo 3 antes que el nodo 2, precisamente porque hay un camino de un solo
            // salto hasta 3 aunque el camino "largo" (0-1-2-3) tenga 3 aristas.
            Assert.Equal(0, orden[0]);
            Assert.True(orden.IndexOf(3) < orden.IndexOf(2),
                "El nodo 3 (a 1 salto) debe visitarse antes que el nodo 2 (a 2 saltos), " +
                "sin importar que exista un camino más largo entre ambos.");
        }

        // ---------------------------------------------------------------
        // DFS iterativo y recursivo
        // ---------------------------------------------------------------

        [Fact]
        public void DFSIterativo_VisitaTodosLosNodosDeUnGrafoConectado()
        {
            var g = GrafoSinUI(4);
            g.AgregarArista(0, 1);
            g.AgregarArista(1, 2);
            g.AgregarArista(0, 3);

            var orden = g.DFSOrden(0);

            Assert.Equal(4, orden.Count);
            Assert.Equal(new HashSet<int> { 0, 1, 2, 3 }, new HashSet<int>(orden));
            Assert.Equal(0, orden[0]);
        }

        [Fact]
        public void DFSRecursivo_VisitaTodosLosNodosDeUnGrafoConectado()
        {
            var g = GrafoSinUI(4);
            g.AgregarArista(0, 1);
            g.AgregarArista(1, 2);
            g.AgregarArista(0, 3);

            var orden = g.DFSOrdenRecursivo(0);

            Assert.Equal(4, orden.Count);
            Assert.Equal(new HashSet<int> { 0, 1, 2, 3 }, new HashSet<int>(orden));
            Assert.Equal(0, orden[0]);
        }

        [Fact]
        public void DFSIterativoYRecursivo_ProducenElMismoConjuntoDeNodos()
        {
            // Árbol con ramificaciones para que el orden de exploración importe.
            var g = GrafoSinUI(7);
            g.AgregarArista(0, 1);
            g.AgregarArista(0, 2);
            g.AgregarArista(1, 3);
            g.AgregarArista(1, 4);
            g.AgregarArista(2, 5);
            g.AgregarArista(2, 6);

            var ordenIterativo = g.DFSOrden(0);
            var ordenRecursivo = g.DFSOrdenRecursivo(0);

            // Ambas variantes de DFS son válidas y, para este grafo (sin ambigüedad de
            // orden entre vecinos), deben coincidir exactamente.
            Assert.Equal(ordenRecursivo, ordenIterativo);
        }

        // ---------------------------------------------------------------
        // Grafos desconectados: no debe haber crash ni ciclo infinito, y solo
        // se visita la componente alcanzable desde el nodo de inicio.
        // ---------------------------------------------------------------

        [Fact]
        public void BFS_EnGrafoDesconectado_SoloVisitaLaComponenteAlcanzable()
        {
            // Componente A: 0 - 1      Componente B: 2 - 3 (sin conexión entre ambas)
            var g = GrafoSinUI(4);
            g.AgregarArista(0, 1);
            g.AgregarArista(2, 3);

            var orden = g.BFSOrden(0);

            Assert.Equal(new HashSet<int> { 0, 1 }, new HashSet<int>(orden));
        }

        [Fact]
        public void DFS_EnGrafoDesconectado_SoloVisitaLaComponenteAlcanzable()
        {
            var g = GrafoSinUI(4);
            g.AgregarArista(0, 1);
            g.AgregarArista(2, 3);

            var orden = g.DFSOrden(0);

            Assert.Equal(new HashSet<int> { 0, 1 }, new HashSet<int>(orden));
        }

        // ---------------------------------------------------------------
        // Grafos cíclicos: el arreglo de visitados debe evitar bucles infinitos.
        // Si el algoritmo tuviera el bug de no comprobar "visitado" antes de
        // volver a encolar/apilar, estas pruebas colgarían indefinidamente en
        // vez de fallar limpiamente.
        // ---------------------------------------------------------------

        [Fact]
        public void BFS_EnGrafoConCiclo_TerminaYVisitaCadaNodoUnaSolaVez()
        {
            // Ciclo: 0 - 1 - 2 - 0
            var g = GrafoSinUI(3);
            g.AgregarArista(0, 1);
            g.AgregarArista(1, 2);
            g.AgregarArista(2, 0);

            var orden = g.BFSOrden(0);

            Assert.Equal(3, orden.Count); // cada nodo aparece exactamente una vez
            Assert.Equal(new HashSet<int> { 0, 1, 2 }, new HashSet<int>(orden));
        }

        [Fact]
        public void DFS_EnGrafoConCiclo_TerminaYVisitaCadaNodoUnaSolaVez()
        {
            var g = GrafoSinUI(3);
            g.AgregarArista(0, 1);
            g.AgregarArista(1, 2);
            g.AgregarArista(2, 0);

            var orden = g.DFSOrden(0);

            Assert.Equal(3, orden.Count);
            Assert.Equal(new HashSet<int> { 0, 1, 2 }, new HashSet<int>(orden));
        }

        [Fact]
        public void DFSRecursivo_EnGrafoConCiclo_TerminaYVisitaCadaNodoUnaSolaVez()
        {
            var g = GrafoSinUI(3);
            g.AgregarArista(0, 1);
            g.AgregarArista(1, 2);
            g.AgregarArista(2, 0);

            var orden = g.DFSOrdenRecursivo(0);

            Assert.Equal(3, orden.Count);
            Assert.Equal(new HashSet<int> { 0, 1, 2 }, new HashSet<int>(orden));
        }

        // ---------------------------------------------------------------
        // Auto-bucles y aristas duplicadas
        // ---------------------------------------------------------------

        [Fact]
        public void AgregarArista_ConAutoBucle_NoDuplicaLaEntradaYNoRompeElRecorrido()
        {
            var g = GrafoSinUI(2);
            g.AgregarArista(0, 0); // auto-bucle
            g.AgregarArista(0, 1);

            Assert.Single(g.ObtenerVecinos(0), v => v == 0); // una sola entrada del auto-bucle

            var orden = g.BFSOrden(0);
            Assert.Equal(new HashSet<int> { 0, 1 }, new HashSet<int>(orden));
        }

        [Fact]
        public void AgregarArista_LlamadaDosVecesConElMismoPar_EsIdempotente()
        {
            var g = GrafoSinUI(2);
            g.AgregarArista(0, 1);
            g.AgregarArista(0, 1); // duplicado intencional
            g.AgregarArista(1, 0); // duplicado intencional, orden invertido

            Assert.Single(g.ObtenerVecinos(0));
            Assert.Single(g.ObtenerVecinos(1));
        }

        // ---------------------------------------------------------------
        // Casos límite: nodo inicial inválido, grafo vacío, un solo nodo,
        // número de nodos negativo.
        // ---------------------------------------------------------------

        [Fact]
        public void BFS_ConNodoInicialQueNoExiste_LanzaExcepcionClara()
        {
            var g = GrafoSinUI(3);
            g.AgregarArista(0, 1);

            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => g.BFSOrden(5));
            Assert.Contains("5", ex.Message);
        }

        [Fact]
        public void DFS_ConNodoInicialNegativo_LanzaExcepcionClara()
        {
            var g = GrafoSinUI(3);

            Assert.Throws<ArgumentOutOfRangeException>(() => g.DFSOrden(-1));
        }

        [Fact]
        public void AgregarArista_ConNodoQueNoExiste_LanzaExcepcion()
        {
            var g = GrafoSinUI(2);

            Assert.Throws<ArgumentOutOfRangeException>(() => g.AgregarArista(0, 9));
        }

        [Fact]
        public void GrafoVacio_BFSOrdenYDFSOrden_DevuelvenListaVaciaSinLanzarExcepcion()
        {
            var g = GrafoSinUI(0);

            var ordenBfs = g.BFSOrden(0);
            var ordenDfs = g.DFSOrden(0);
            var ordenDfsRec = g.DFSOrdenRecursivo(0);

            Assert.Empty(ordenBfs);
            Assert.Empty(ordenDfs);
            Assert.Empty(ordenDfsRec);
        }

        [Fact]
        public void GrafoVacio_BFSyDFS_DevuelvenMensajeControladoSinLanzarExcepcion()
        {
            var g = GrafoSinUI(0);

            var mensajeBfs = g.BFS(0);
            var mensajeDfs = g.DFS(0);

            Assert.Contains("vacío", mensajeBfs, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("vacío", mensajeDfs, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void GrafoDeUnSoloNodo_BFSyDFS_VisitanSoloEseNodo()
        {
            var g = GrafoSinUI(1);

            Assert.Equal(new List<int> { 0 }, g.BFSOrden(0));
            Assert.Equal(new List<int> { 0 }, g.DFSOrden(0));
            Assert.Equal(new List<int> { 0 }, g.DFSOrdenRecursivo(0));
        }

        [Fact]
        public void Constructor_ConNumeroDeNodosNegativo_LanzaExcepcion()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Grafo(-1, null));
        }

        [Fact]
        public void Constructor_SinListaDeBotones_NoLanzaExcepcion()
        {
            // Un Grafo debe poder construirse sin UI real (botones == null), tal como lo
            // hacen todas las pruebas de esta clase.
            var g = new Grafo(3, null);
            g.AgregarArista(0, 1);

            Assert.Equal(new List<int> { 0, 1 }, g.BFSOrden(0));
        }
    }
}
