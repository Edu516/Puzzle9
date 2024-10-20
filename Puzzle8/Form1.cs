using Timer = System.Threading.Timer;

namespace Puzzle8
{
    public partial class Form1 : Form
    {
        private IaBuscaMelhorEscolha ia = new IaBuscaMelhorEscolha();
        private List<Image> listaDeFragmentos = new List<Image>();
        private List<Estado> solucao; // Armazenar a solução
        private int movimentoAtual = 0; // Índice do movimento atual
        private Timer timer; // Timer para controle do tempo entre movimentos
        public Form1()
        {
            InitializeComponent();

            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Cria 9 botões e adiciona ao painel
            for (int i = 0; i < 9; i++)
            {
                Button btn = new Button
                {
                    Size = new Size(100, 100), // Tamanho do botão
                    Location = new Point((i % 3) * 100, (i / 3) * 100), // Localização do botão
                    Name = "button" + (i + 1),
                    Text = "",
                };

                panel1.Controls.Add(btn);
            }
            label2.Text = "A movimentação funciona de acordo com os slots" +
                "\n 1 | 2 |3" +
                "\n 4 | 5 |6" +
                "\n 7 | 8 |9" +
                "\n A movimentação citada no passo a passo se trata de trocar " +
                "\n os botões presente em cada slot" +
                "\n e não a numeração dos fragmentos presente em cada slot";
        }


        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Imagens|*.jpg;*.png;*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                DivideImage(img);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            int[,] estadoInicial = ObterEstadoAtual();

            // Definir o estado objetivo (Puzzle resolvido)
            int[,] estadoObjetivo = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 0 } // O 0 representa o espaço vazio
            };

            // Executar a busca pela melhor solução e capturar os resultados
            var (solucao, nosGerados, tempoMs) = ia.BuscaMelhorEscolha(estadoInicial, estadoObjetivo);

            // Limpar a lista de passos anterior no ListBox existente
            richTextBox1.Clear();

            if (solucao != null && solucao.Count > 0)
            {
                richTextBox1.AppendText($"Solução encontrada em {tempoMs} ms\n");
                richTextBox1.AppendText($"Nós gerados: {nosGerados}\n");
                richTextBox1.AppendText("Passos:\n");

                richTextBox1.AppendText(FormatarEstado(solucao[0].Tabuleiro, 0, 0));
                for (int i = 1; i < solucao.Count; i++)
                {
                    Estado estadoAtual = solucao[i];
                    Estado estadoAnterior = solucao[i - 1];
                    int slotMovido = 0;
                    int slotDestino = 0;

                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if (estadoAtual.Tabuleiro[x, y] == 0)
                            {
                                slotDestino = x * 3 + y + 1;
                            }
                            else if (estadoAtual.Tabuleiro[x, y] != estadoAnterior.Tabuleiro[x, y])
                            {
                                slotMovido = estadoAtual.Tabuleiro[x, y];
                            }
                        }
                    }

                    if (slotMovido != 0 && slotDestino != 0)
                    {
                        richTextBox1.AppendText(FormatarEstado(estadoAtual.Tabuleiro, slotMovido, slotDestino));
                    }
                }
            }
            else
            {
                MessageBox.Show("Nenhuma solução encontrada.");
            }

        }



        private string FormatarEstado(int[,] tabuleiro, int slotMovido, int slotDestino)
        {
            // Formata a representação do tabuleiro
            string estadoIdeal = "Estado Ideal:\n";
            estadoIdeal += "123\n456\n780\n\n";

            string estadoAtual = "Estado dos Fragmentos Após a troca:\n";
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    estadoAtual += tabuleiro[i, j] + " ";
                }
                estadoAtual += "\n";
            }
            string lin = "______________________________________";
            // Retorna a descrição do movimento
            return estadoAtual + $"Movimento: Slot {slotMovido} troca com Slot {slotDestino}\n{lin}\n";
        }



        private void button3_Click(object sender, EventArgs e)
        {
            // Lista de números representando o estado (valores 1 a 8 e o 0 para espaço vazio)
            List<int> valores = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 0 };

            // Embaralha a lista de valores usando a classe Random
            Random rnd = new Random();
            valores = valores.OrderBy(x => rnd.Next()).ToList();

            // Atualiza os botões com os valores embaralhados
            int index = 0;
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    int valor = valores[index];

                    // Define o texto do botão baseado no valor
                    btn.Text = valor == 0 ? "" : valor.ToString();

                    // Atualiza a imagem, se houver
                    if (valor > 0 && valor <= listaDeFragmentos.Count)
                    {
                        btn.BackgroundImage = listaDeFragmentos[valor - 1]; // Define o fragmento da imagem
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else
                    {
                        btn.BackgroundImage = null; // Limpa a imagem para o espaço vazio
                    }

                    index++;
                }
            }
        }


        private void DivideImage(Image img)
        {
            int btnSize = 100;
            Bitmap bmp = new Bitmap(img, new Size(btnSize * 3, btnSize * 3)); // Redimensiona a imagem

            listaDeFragmentos.Clear(); // Limpa a lista antes de armazenar novos fragmentos

            int index = 0;
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (index == 8) // Última parte (nona) fica em branco
                    {
                        btn.Text = ""; // Define o texto como vazio
                        btn.BackgroundImage = null; // Sem imagem no botão vazio
                    }
                    else
                    {
                        // Clona a parte correspondente da imagem
                        Bitmap part = bmp.Clone(new Rectangle((index % 3) * btnSize, (index / 3) * btnSize, btnSize, btnSize), bmp.PixelFormat);

                        // Armazena o fragmento na lista de fragmentos
                        listaDeFragmentos.Add(part);

                        btn.BackgroundImage = part; // Define a imagem do botão
                        btn.BackgroundImageLayout = ImageLayout.Stretch; // Estica a imagem para preencher o botão
                        btn.Text = (index + 1).ToString(); // Define o texto como o número correspondente
                    }

                    btn.Click += ButtonClick; // Adiciona o evento de clique
                    index++;
                }
            }
        }


        // Lógica para movimentar os botões (evento de clique)
        private void ButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button; // O botão que foi clicado
            Button emptyButton = panel1.Controls.OfType<Button>().FirstOrDefault(b => b.Text == ""); // Botão vazio

            // Verifica se o botão clicado está adjacente ao botão vazio
            if (IsAdjacent(clickedButton, emptyButton))
            {
                // Armazena a imagem e o texto do botão clicado temporariamente
                Image tempImage = clickedButton.BackgroundImage;
                string tempText = clickedButton.Text;

                // Move a imagem e o texto do botão clicado para o botão vazio
                emptyButton.BackgroundImage = tempImage;
                emptyButton.Text = tempText;

                // O botão clicado agora se torna o vazio
                clickedButton.BackgroundImage = null; // Limpa a imagem do botão clicado
                clickedButton.Text = ""; // Limpa o texto do botão clicado
            }
        }



        // Método para verificar se os botões são adjacentes
        private bool IsAdjacent(Button btn1, Button btn2)
        {
            // Obtém o índice de ambos os botões dentro do painel (assumindo grid 3x3)
            int btn1Index = panel1.Controls.GetChildIndex(btn1);
            int btn2Index = panel1.Controls.GetChildIndex(btn2);

            // Calcula a linha e a coluna de cada botão
            int rowBtn1 = btn1Index / 3;
            int colBtn1 = btn1Index % 3;
            int rowBtn2 = btn2Index / 3;
            int colBtn2 = btn2Index % 3;

            // Verifica se os botões estão na mesma linha ou coluna e são adjacentes
            return (Math.Abs(rowBtn1 - rowBtn2) == 1 && colBtn1 == colBtn2) ||  // Mesma coluna e linhas adjacentes
                   (Math.Abs(colBtn1 - colBtn2) == 1 && rowBtn1 == rowBtn2);    // Mesma linha e colunas adjacentes
        }


        // Método para obter o estado atual do tabuleiro (botões no painel)
        private int[,] ObterEstadoAtual()
        {
            int[,] estadoAtual = new int[3, 3];
            int index = 0;

            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    int valor = string.IsNullOrEmpty(btn.Text) ? 0 : int.Parse(btn.Text); // O botão vazio tem valor 0
                    estadoAtual[index / 3, index % 3] = valor;
                    index++;
                }
            }

            return estadoAtual;
        }

        // Método para atualizar o tabuleiro com base no estado atual
        private void AtualizarTabuleiro(int[,] tabuleiro)
        {
            int index = 0;

            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    int valor = tabuleiro[index / 3, index % 3];

                    // Atualiza o texto, o valor 0 representa o espaço vazio
                    btn.Text = valor == 0 ? "" : valor.ToString();

                    // Atualiza a imagem do botão se ele não for o vazio
                    if (valor != 0)
                    {
                        // Usa o valor do botão para buscar a imagem correspondente (supondo que as imagens estejam armazenadas)
                        btn.BackgroundImage = GetImagemFragmento(valor);
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else
                    {
                        // Se for o espaço vazio, remove a imagem
                        btn.BackgroundImage = null;
                    }

                    index++;
                }
            }
        }


        private Image GetImagemFragmento(int valor)
        {
            // Verifica se o valor é válido e se está dentro dos limites da lista de fragmentos
            if (valor > 0 && valor <= listaDeFragmentos.Count)
            {
                return listaDeFragmentos[valor - 1]; // Retorna o fragmento correspondente (1-8)
            }
            return null; // Caso contrário, retorna null (para o espaço vazio ou valor inválido)
        }

        private string EstadoParaString(int[,] tabuleiro)
        {
            string resultado = "";
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    resultado += tabuleiro[i, j] == 0 ? " " : tabuleiro[i, j].ToString(); // Usa espaço para o valor 0
                    resultado += " ";
                }
                resultado += Environment.NewLine; // Nova linha para cada linha do tabuleiro
            }
            return resultado;
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            IaBuscaHorizontal iaBuscaHorizontal = new IaBuscaHorizontal();

            int[,] estadoInicial = ObterEstadoAtual(); // Obtém o estado atual do tabuleiro

            // Definir o estado objetivo (puzzle resolvido)
            int[,] estadoObjetivo = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 0 } // O 0 representa o espaço vazio
            };

            // Executar a busca em largura
            var (solucao, nosGerados, tempo) = iaBuscaHorizontal.BuscaLargura(estadoInicial, estadoObjetivo);

            // Limpar o ListBox antes de exibir a nova solução
            // Clear the RichTextBox before displaying the new solution
            richTextBox1.Clear();

            // Check if a solution was found
            if (solucao != null && solucao.Count > 0)
            {
                richTextBox1.AppendText($"Solução encontrada em {tempo} ms\n");
                richTextBox1.AppendText($"Nós gerados: {nosGerados}\n");
                richTextBox1.AppendText("Passos:\n");

                for (int i = 1; i < solucao.Count; i++) // Start from 1 to skip the initial state
                {
                    Estado estadoAtual = solucao[i];
                    Estado estadoAnterior = solucao[i - 1];
                    int slotMovido = 0;
                    int slotDestino = 0;

                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            if (estadoAtual.Tabuleiro[x, y] == 0)
                            {
                                slotDestino = x * 3 + y + 1;
                            }
                            else if (estadoAtual.Tabuleiro[x, y] != estadoAnterior.Tabuleiro[x, y])
                            {
                                slotMovido = estadoAtual.Tabuleiro[x, y];
                            }
                        }
                    }

                    if (slotMovido != 0 && slotDestino != 0)
                    {
                        richTextBox1.AppendText(FormatarEstado(estadoAtual.Tabuleiro, slotMovido, slotDestino));
                    }
                }
            }
            else
            {
                MessageBox.Show($"Nenhuma solução encontrada.\nNós gerados: {nosGerados}\nTempo: {tempo} ms");
            }

        }



        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Exibe uma caixa de diálogo para o usuário inserir a sequência
            string input = Microsoft.VisualBasic.Interaction.InputBox("Digite uma sequência de 9 números de 1 a 9, sem repetição.",
                                                                      "Entrada de Sequência",
                                                                      "123456789");

            // Verifica se o valor não foi cancelado (string vazia significa que foi cancelado)
            if (!string.IsNullOrEmpty(input))
            {
                if (ValidarEntrada(input))
                {
                    AtualizarTabuleiroComEntrada(input); // Atualiza o tabuleiro com a nova sequência
                }
                else
                {
                    MessageBox.Show("Entrada inválida. Digite uma sequência de 9 números únicos entre 1 e 9.");
                }
            }
        }

        // Método para validar a entrada
        private bool ValidarEntrada(string input)
        {
            // Verifica se a entrada tem exatamente 9 caracteres
            if (input.Length != 9)
            {
                return false;
            }

            // Verifica se contém apenas números de 1 a 9 sem repetição
            return input.Distinct().Count() == 9 && input.All(c => char.IsDigit(c) && c != '0');
        }

        // Método para atualizar o tabuleiro com a entrada fornecida
        private void AtualizarTabuleiroComEntrada(string input)
        {
            // Converte a string de entrada em uma lista de inteiros
            List<int> valores = input.Select(c => int.Parse(c.ToString())).ToList();

            // Atualiza os botões com os valores fornecidos
            int index = 0;
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    int valor = valores[index];

                    // Define o texto do botão baseado no valor
                    btn.Text = valor == 9 ? "" : valor.ToString(); // O valor 9 será o espaço vazio

                    // Atualiza a imagem, se houver
                    if (valor > 0 && valor <= listaDeFragmentos.Count)
                    {
                        btn.BackgroundImage = listaDeFragmentos[valor - 1]; // Define o fragmento da imagem
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else
                    {
                        btn.BackgroundImage = null; // Limpa a imagem para o espaço vazio
                    }

                    index++;
                }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_2(object sender, EventArgs e)
        {

        }
    }



}
