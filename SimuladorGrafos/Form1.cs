using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimuladorGrafos
{
    public partial class Form1 : Form
    {
        private Grafo grafo;
        private const int NumNodos = 20;
        private List<Button> nodosButtons = new List<Button>();
        private List<Control> aristasLabels = new List<Control>();

        // Tamaño de los nodos
        private const int TAMANO_NODO = 60;

        // Posiciones ajustadas para los nodos con más espacio entre ellos
        private readonly Point[] posicionesNodos = new Point[]
        {
            // Nivel 0 (raíz)
            new Point(550, 50),   // Nodo 0 (centro superior)
            
            // Nivel 1 (hijos directos de la raíz)
            new Point(350, 150),  // Nodo 1 (izquierda)
            new Point(750, 150),  // Nodo 2 (derecha)
            
            // Nivel 2 (hijos del nivel 1)
            new Point(250, 250),  // Nodo 3
            new Point(450, 250),  // Nodo 4
            new Point(650, 250),  // Nodo 5
            new Point(850, 250),  // Nodo 6
            
            // Nivel 3 (primera fila inferior)
            new Point(150, 350),  // Nodo 7
            new Point(250, 350),  // Nodo 8
            new Point(350, 350),  // Nodo 9
            new Point(450, 350),  // Nodo 10
            new Point(550, 350),  // Nodo 11
            new Point(650, 350),  // Nodo 12
            new Point(750, 350),  // Nodo 13
            new Point(850, 350),  // Nodo 14
            
            // Nivel 4 (fila final)
            new Point(200, 450),  // Nodo 15
            new Point(350, 450),  // Nodo 16
            new Point(500, 450),  // Nodo 17
            new Point(650, 450),  // Nodo 18
            new Point(800, 450)   // Nodo 19
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Configurar el formulario
                this.WindowState = FormWindowState.Maximized;

                // Centrar los controles en el panel izquierdo
                CentrarControles();

                // Actualizar el valor máximo del control numericNodoInicio
                numericNodoInicio.Maximum = NumNodos - 1;

                // Crear los botones de nodos en el panel
                CrearNodos();

                // Inicializar el grafo y sus conexiones
                InicializarGrafo();

                // Dibujar las aristas entre los nodos
                DibujarAristas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CentrarControles()
        {
            // Ajustar tamaños y posiciones de los controles en el panel izquierdo
            int panelWidth = panelGrafo.Width;

            // Centrar el control numericNodoInicio y su etiqueta
            lblNodoInicio.Location = new Point((panelWidth - lblNodoInicio.Width - numericNodoInicio.Width - 10) / 2, 20);
            numericNodoInicio.Location = new Point(lblNodoInicio.Right + 10, 20);

            // Centrar los botones
            int totalButtonWidth = btnBFS.Width + btnDFS.Width + 20; // 20 px de separación

            btnBFS.Location = new Point((panelWidth - totalButtonWidth) / 2, 60);
            btnDFS.Location = new Point(btnBFS.Right + 20, 60);

            // Ajustar la posición y tamaño del cuadro de resultados
            txtResultados.Width = panelWidth - 40; // 20px de margen a cada lado
            txtResultados.Height = 500; // Aumentar la altura
            txtResultados.Top = 170; // Posicionar el TextBox más arriba
            txtResultados.Left = 20;
        }

        private void CrearNodos()
        {
            // Limpiar nodos existentes si los hay
            foreach (Button btn in nodosButtons)
            {
                if (panelControles.Controls.Contains(btn))
                    panelControles.Controls.Remove(btn);
            }
            nodosButtons.Clear();

            // Crear los botones para representar los nodos
            for (int i = 0; i < NumNodos; i++)
            {
                Button btnNodo = new Button
                {
                    Text = i.ToString(),
                    Size = new Size(TAMANO_NODO, TAMANO_NODO),
                    Location = posicionesNodos[i],
                    Tag = i, // Guardar el índice del nodo
                    BackColor = Color.FromArgb(0, 200, 100), // Verde brillante
                    ForeColor = Color.White, // Texto blanco
                    Font = new Font("Arial", 16, FontStyle.Bold), // Fuente más grande
                    FlatStyle = FlatStyle.Flat
                };

                // Quitar el borde
                btnNodo.FlatAppearance.BorderSize = 0;

                // Crear círculo con el evento Paint
                btnNodo.Paint += (sender, e) => {
                    Button btn = (Button)sender;
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    // Dibujar el círculo completo
                    using (SolidBrush brush = new SolidBrush(btn.BackColor))
                    {
                        e.Graphics.FillEllipse(brush, 0, 0, btn.Width, btn.Height);
                    }

                    // Centrar el texto
                    StringFormat stringFormat = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };

                    using (SolidBrush textBrush = new SolidBrush(btn.ForeColor))
                    {
                        e.Graphics.DrawString(btn.Text, btn.Font, textBrush,
                            new RectangleF(0, 0, btn.Width, btn.Height), stringFormat);
                    }
                };

                panelControles.Controls.Add(btnNodo);
                nodosButtons.Add(btnNodo);
            }
        }

        private void InicializarGrafo()
        {
            grafo = new Grafo(NumNodos, nodosButtons);

            // Nivel 0 -> Nivel 1
            grafo.AgregarArista(0, 1);
            grafo.AgregarArista(0, 2);

            // Nivel 1 -> Nivel 2
            grafo.AgregarArista(1, 3);
            grafo.AgregarArista(1, 4);
            grafo.AgregarArista(2, 5);
            grafo.AgregarArista(2, 6);

            // Nivel 2 -> Nivel 3
            grafo.AgregarArista(3, 7);
            grafo.AgregarArista(3, 8);  // Conexión jerárquica para nodo 8
            grafo.AgregarArista(4, 9);
            grafo.AgregarArista(4, 10); // Conexión jerárquica para nodo 10
            grafo.AgregarArista(5, 11);
            grafo.AgregarArista(5, 12); // Conexión jerárquica para nodo 12
            grafo.AgregarArista(6, 13);
            grafo.AgregarArista(6, 14); // Conexión jerárquica para nodo 14

            // Nivel 3 -> Nivel 4
            grafo.AgregarArista(7, 15);
            grafo.AgregarArista(9, 16);
            grafo.AgregarArista(11, 17);
            grafo.AgregarArista(13, 18);
            grafo.AgregarArista(14, 19);

        }

        private void DibujarAristas()
        {
            // Limpiar aristas existentes si las hay
            foreach (Control ctrl in aristasLabels)
            {
                if (panelControles.Controls.Contains(ctrl))
                    panelControles.Controls.Remove(ctrl);
            }
            aristasLabels.Clear();

            // Primero dibujamos las aristas principales (estructura de árbol)
            for (int i = 0; i < NumNodos; i++)
            {
                foreach (int vecino in grafo.ObtenerVecinos(i))
                {
                    // Solo dibujamos la arista si i < vecino para evitar duplicados
                    if (i < vecino && EsConexionPadreHijo(i, vecino))
                    {
                        DibujarLinea(i, vecino, Color.Black, 2);
                    }
                }
            }

            // Luego dibujamos las conexiones adicionales
            for (int i = 0; i < NumNodos; i++)
            {
                foreach (int vecino in grafo.ObtenerVecinos(i))
                {
                    // Solo dibujamos la arista si i < vecino para evitar duplicados
                    if (i < vecino && !EsConexionPadreHijo(i, vecino))
                    {
                        DibujarLinea(i, vecino, Color.FromArgb(0, 190, 255), 1);
                    }
                }
            }
        }

        private void DibujarLinea(int nodoInicio, int nodoFin, Color color, float grosor)
        {
            try
            {
                // Obtener las posiciones de los nodos
                Button btnInicio = nodosButtons[nodoInicio];
                Button btnFin = nodosButtons[nodoFin];

                // Calcular los centros de los nodos
                Point centroInicio = new Point(
                    btnInicio.Left + btnInicio.Width / 2,
                    btnInicio.Top + btnInicio.Height / 2
                );

                Point centroFin = new Point(
                    btnFin.Left + btnFin.Width / 2,
                    btnFin.Top + btnFin.Height / 2
                );

                // Determinar si es una conexión padre-hijo (estructura de árbol)
                bool esPadreHijo = EsConexionPadreHijo(nodoInicio, nodoFin);

                // Crear un panel transparente que cubra todo el espacio entre los nodos
                int x = Math.Min(centroInicio.X, centroFin.X);
                int y = Math.Min(centroInicio.Y, centroFin.Y);
                int ancho = Math.Abs(centroFin.X - centroInicio.X);
                int alto = Math.Abs(centroFin.Y - centroInicio.Y);

                // Asegurarse de que el panel tiene al menos un tamaño mínimo
                ancho = Math.Max(ancho, 10);
                alto = Math.Max(alto, 10);

                Panel lineaPanel = new Panel
                {
                    BackColor = Color.Transparent,
                    Size = new Size(ancho + 20, alto + 20),
                    Location = new Point(x - 10, y - 10)
                };

                // Agregar el evento de pintura para dibujar la línea diagonal
                lineaPanel.Paint += (sender, e) => {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (Pen pen = new Pen(color, grosor))
                    {
                        // Si es una conexión padre-hijo, dibujar flecha
                        if (esPadreHijo)
                        {
                            // Ajustar los puntos para evitar solapar con los nodos
                            float radioNodo = TAMANO_NODO / 2.0f;

                            // Calcular el vector dirección normalizado
                            float dx = centroFin.X - centroInicio.X;
                            float dy = centroFin.Y - centroInicio.Y;
                            float longitud = (float)Math.Sqrt(dx * dx + dy * dy);
                            dx /= longitud;
                            dy /= longitud;

                            // Ajustar los puntos de inicio y fin
                            PointF inicioAjustado = new PointF(
                                centroInicio.X + dx * radioNodo,
                                centroInicio.Y + dy * radioNodo
                            );

                            PointF finAjustado = new PointF(
                                centroFin.X - dx * radioNodo,
                                centroFin.Y - dy * radioNodo
                            );

                            // Convertir a coordenadas del panel
                            inicioAjustado = new PointF(
                                inicioAjustado.X - lineaPanel.Left,
                                inicioAjustado.Y - lineaPanel.Top
                            );

                            finAjustado = new PointF(
                                finAjustado.X - lineaPanel.Left,
                                finAjustado.Y - lineaPanel.Top
                            );

                            // Dibujar línea
                            e.Graphics.DrawLine(pen, inicioAjustado, finAjustado);

                            // Dibujar flecha
                            DrawArrow(e.Graphics, pen, inicioAjustado, finAjustado);
                        }
                        else
                        {
                            // Para conexiones que no son padre-hijo, simplemente dibujar línea sin flecha
                            float radioNodo = TAMANO_NODO / 2.0f;

                            // Calcular el vector dirección normalizado
                            float dx = centroFin.X - centroInicio.X;
                            float dy = centroFin.Y - centroInicio.Y;
                            float longitud = (float)Math.Sqrt(dx * dx + dy * dy);
                            dx /= longitud;
                            dy /= longitud;

                            // Ajustar los puntos de inicio y fin
                            PointF inicioAjustado = new PointF(
                                centroInicio.X + dx * radioNodo,
                                centroInicio.Y + dy * radioNodo
                            );

                            PointF finAjustado = new PointF(
                                centroFin.X - dx * radioNodo,
                                centroFin.Y - dy * radioNodo
                            );

                            // Convertir a coordenadas del panel
                            inicioAjustado = new PointF(
                                inicioAjustado.X - lineaPanel.Left,
                                inicioAjustado.Y - lineaPanel.Top
                            );

                            finAjustado = new PointF(
                                finAjustado.X - lineaPanel.Left,
                                finAjustado.Y - lineaPanel.Top
                            );

                            // Dibujar línea
                            e.Graphics.DrawLine(pen, inicioAjustado, finAjustado);
                        }
                    }
                };

                // Añadir el panel al control
                panelControles.Controls.Add(lineaPanel);
                lineaPanel.SendToBack();

                // Guardar referencia
                aristasLabels.Add(lineaPanel);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al dibujar línea: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para dibujar una flecha en la línea
        private void DrawArrow(Graphics g, Pen pen, PointF start, PointF end)
        {
            // Tamaño de la flecha
            float arrowSize = 10.0f;

            // Calcular el ángulo de la flecha
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);

            // Crear los puntos de la flecha
            PointF[] arrowHead = new PointF[3];
            arrowHead[0] = end;

            arrowHead[1] = new PointF(
                end.X - arrowSize * (float)Math.Cos(angle - Math.PI / 6),
                end.Y - arrowSize * (float)Math.Sin(angle - Math.PI / 6)
            );

            arrowHead[2] = new PointF(
                end.X - arrowSize * (float)Math.Cos(angle + Math.PI / 6),
                end.Y - arrowSize * (float)Math.Sin(angle + Math.PI / 6)
            );

            // Dibujar la flecha
            g.FillPolygon(new SolidBrush(pen.Color), arrowHead);
        }

        // Método para determinar si una conexión es padre-hijo en la estructura de árbol
        private bool EsConexionPadreHijo(int nodoInicio, int nodoFin)
        {

            // Nivel 0 -> Nivel 1
            if (nodoInicio == 0 && (nodoFin == 1 || nodoFin == 2)) return true;

            // Nivel 1 -> Nivel 2
            if (nodoInicio == 1 && (nodoFin == 3 || nodoFin == 4)) return true;
            if (nodoInicio == 2 && (nodoFin == 5 || nodoFin == 6)) return true;

            // Nivel 2 -> Nivel 3 (INCLUIR TODOS LOS NODOS)
            if (nodoInicio == 3 && (nodoFin == 7 || nodoFin == 8)) return true; // Asegurar línea negra para nodo 8
            if (nodoInicio == 4 && (nodoFin == 9 || nodoFin == 10)) return true; // Asegurar línea negra para nodo 10
            if (nodoInicio == 5 && (nodoFin == 11 || nodoFin == 12)) return true; // Asegurar línea negra para nodo 12
            if (nodoInicio == 6 && (nodoFin == 13 || nodoFin == 14)) return true; // Asegurar línea negra para nodo 14

            // Nivel 3 -> Nivel 4
            if (nodoInicio == 7 && nodoFin == 15) return true;
            if (nodoInicio == 9 && nodoFin == 16) return true;
            if (nodoInicio == 11 && nodoFin == 17) return true;
            if (nodoInicio == 13 && nodoFin == 18) return true;
            if (nodoInicio == 14 && nodoFin == 19) return true;

            return false;
        }

        private void ResetearColorNodos()
        {
            // Restaurar el color por defecto de todos los nodos
            foreach (Button btn in nodosButtons)
            {
                btn.BackColor = Color.FromArgb(0, 200, 100); // Verde como color por defecto
                btn.ForeColor = Color.White; // Mantener texto blanco
                btn.Invalidate(); // Forzar redibujado
            }
        }

        private void ColorearNodo(int nodo, Color color)
        {
            if (nodo >= 0 && nodo < nodosButtons.Count)
            {
                // Asegurarse de que el cambio de color ocurra en el hilo de la UI
                if (nodosButtons[nodo].InvokeRequired)
                {
                    nodosButtons[nodo].Invoke(new Action(() => {
                        nodosButtons[nodo].BackColor = color;
                        // Conservar el texto en blanco
                        nodosButtons[nodo].ForeColor = Color.White;
                        // Forzar redibujado
                        nodosButtons[nodo].Invalidate();
                    }));
                }
                else
                {
                    nodosButtons[nodo].BackColor = color;
                    // Conservar el texto en blanco
                    nodosButtons[nodo].ForeColor = Color.White;
                    // Forzar redibujado
                    nodosButtons[nodo].Invalidate();
                }

                // Aplicar cambios inmediatamente
                Application.DoEvents();

                // Añadir una pequeña pausa para visualizar mejor la animación
                System.Threading.Thread.Sleep(300);
            }
        }

        private void btnBFS_Click(object sender, EventArgs e)
        {
            try
            {
                // Limpiar resultados anteriores
                txtResultados.Clear();
                txtResultados.AppendText("Ejecutando Búsqueda en Anchura (BFS)...\r\n\r\n");

                // Obtener nodo de inicio desde el selector
                int nodoInicio = (int)numericNodoInicio.Value;

                // Verificar que grafo esté inicializado
                if (grafo == null)
                {
                    txtResultados.AppendText("Error: El grafo no está inicializado.");
                    return;
                }

                // Resetear colores antes de comenzar
                ResetearColorNodos();

                // Ejecutar BFS
                string resultado = grafo.BFS(nodoInicio);
                txtResultados.AppendText(resultado);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar BFS: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDFS_Click(object sender, EventArgs e)
        {
            try
            {
                // Limpiar resultados anteriores
                txtResultados.Clear();
                txtResultados.AppendText("Ejecutando Búsqueda en Profundidad (DFS)...\r\n\r\n");

                // Obtener nodo de inicio desde el selector
                int nodoInicio = (int)numericNodoInicio.Value;

                // Verificar que grafo esté inicializado
                if (grafo == null)
                {
                    txtResultados.AppendText("Error: El grafo no está inicializado.");
                    return;
                }

                // Resetear colores antes de comenzar
                ResetearColorNodos();

                // Ejecutar DFS
                string resultado = grafo.DFS(nodoInicio);
                txtResultados.AppendText(resultado);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar DFS: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}