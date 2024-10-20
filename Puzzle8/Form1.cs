using Timer = System.Threading.Timer;

namespace Puzzle8
{
    public partial class Form1 : Form
    {
        private IaBuscaMelhorEscolha ia = new IaBuscaMelhorEscolha();
        private List<Image> listaDeFragmentos = new List<Image>();
        private List<Estado> solucao; // Armazenar a solu��o
        private int movimentoAtual = 0; // �ndice do movimento atual
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
            // Cria 9 bot�es e adiciona ao painel
            for (int i = 0; i < 9; i++)
            {
                Button btn = new Button
                {
                    Size = new Size(100, 100), // Tamanho do bot�o
                    Location = new Point((i % 3) * 100, (i / 3) * 100), // Localiza��o do bot�o
                    Name = "button" + (i + 1),
                    Text = "",
                };

                panel1.Controls.Add(btn);
            }
            label2.Text = "A movimenta��o funciona de acordo com os slots" +
                "\n 1 | 2 |3" +
                "\n 4 | 5 |6" +
                "\n 7 | 8 |9" +
                "\n A movimenta��o citada no passo a passo se trata de trocar " +
                "\n os bot�es presente em cada slot" +
                "\n e n�o a numera��o dos fragmentos presente em cada slot";
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
                { 7, 8, 0 } // O 0 representa o espa�o vazio
            };

            // Executar a busca pela melhor solu��o e capturar os resultados
            var (solucao, nosGerados, tempoMs) = ia.BuscaMelhorEscolha(estadoInicial, estadoObjetivo);

            // Limpar a lista de passos anterior no ListBox existente
            richTextBox1.Clear();

            if (solucao != null && solucao.Count > 0)
            {
                richTextBox1.AppendText($"Solu��o encontrada em {tempoMs} ms\n");
                richTextBox1.AppendText($"N�s gerados: {nosGerados}\n");
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
                MessageBox.Show("Nenhuma solu��o encontrada.");
            }

        }



        private string FormatarEstado(int[,] tabuleiro, int slotMovido, int slotDestino)
        {
            // Formata a representa��o do tabuleiro
            string estadoIdeal = "Estado Ideal:\n";
            estadoIdeal += "123\n456\n780\n\n";

            string estadoAtual = "Estado dos Fragmentos Ap�s a troca:\n";
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    estadoAtual += tabuleiro[i, j] + " ";
                }
                estadoAtual += "\n";
            }
            string lin = "______________________________________";
            // Retorna a descri��o do movimento
            return estadoAtual + $"Movimento: Slot {slotMovido} troca com Slot {slotDestino}\n{lin}\n";
        }



        private void button3_Click(object sender, EventArgs e)
        {
            // Lista de n�meros representando o estado (valores 1 a 8 e o 0 para espa�o vazio)
            List<int> valores = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 0 };

            // Embaralha a lista de valores usando a classe Random
            Random rnd = new Random();
            valores = valores.OrderBy(x => rnd.Next()).ToList();

            // Atualiza os bot�es com os valores embaralhados
            int index = 0;
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    int valor = valores[index];

                    // Define o texto do bot�o baseado no valor
                    btn.Text = valor == 0 ? "" : valor.ToString();

                    // Atualiza a imagem, se houver
                    if (valor > 0 && valor <= listaDeFragmentos.Count)
                    {
                        btn.BackgroundImage = listaDeFragmentos[valor - 1]; // Define o fragmento da imagem
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else
                    {
                        btn.BackgroundImage = null; // Limpa a imagem para o espa�o vazio
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
                    if (index == 8) // �ltima parte (nona) fica em branco
                    {
                        btn.Text = ""; // Define o texto como vazio
                        btn.BackgroundImage = null; // Sem imagem no bot�o vazio
                    }
                    else
                    {
                        // Clona a parte correspondente da imagem
                        Bitmap part = bmp.Clone(new Rectangle((index % 3) * btnSize, (index / 3) * btnSize, btnSize, btnSize), bmp.PixelFormat);

                        // Armazena o fragmento na lista de fragmentos
                        listaDeFragmentos.Add(part);

                        btn.BackgroundImage = part; // Define a imagem do bot�o
                        btn.BackgroundImageLayout = ImageLayout.Stretch; // Estica a imagem para preencher o bot�o
                        btn.Text = (index + 1).ToString(); // Define o texto como o n�mero correspondente
                    }

                    btn.Click += ButtonClick; // Adiciona o evento de clique
                    index++;
                }
            }
        }


        // L�gica para movimentar os bot�es (evento de clique)
        private void ButtonClick(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button; // O bot�o que foi clicado
            Button emptyButton = panel1.Controls.OfType<Button>().FirstOrDefault(b => b.Text == ""); // Bot�o vazio

            // Verifica se o bot�o clicado est� adjacente ao bot�o vazio
            if (IsAdjacent(clickedButton, emptyButton))
            {
                // Armazena a imagem e o texto do bot�o clicado temporariamente
                Image tempImage = clickedButton.BackgroundImage;
                string tempText = clickedButton.Text;

                // Move a imagem e o texto do bot�o clicado para o bot�o vazio
                emptyButton.BackgroundImage = tempImage;
                emptyButton.Text = tempText;

                // O bot�o clicado agora se torna o vazio
                clickedButton.BackgroundImage = null; // Limpa a imagem do bot�o clicado
                clickedButton.Text = ""; // Limpa o texto do bot�o clicado
            }
        }



        // M�todo para verificar se os bot�es s�o adjacentes
        private bool IsAdjacent(Button btn1, Button btn2)
        {
            // Obt�m o �ndice de ambos os bot�es dentro do painel (assumindo grid 3x3)
            int btn1Index = panel1.Controls.GetChildIndex(btn1);
            int btn2Index = panel1.Controls.GetChildIndex(btn2);

            // Calcula a linha e a coluna de cada bot�o
            int rowBtn1 = btn1Index / 3;
            int colBtn1 = btn1Index % 3;
            int rowBtn2 = btn2Index / 3;
            int colBtn2 = btn2Index % 3;

            // Verifica se os bot�es est�o na mesma linha ou coluna e s�o adjacentes
            return (Math.Abs(rowBtn1 - rowBtn2) == 1 && colBtn1 == colBtn2) ||  // Mesma coluna e linhas adjacentes
                   (Math.Abs(colBtn1 - colBtn2) == 1 && rowBtn1 == rowBtn2);    // Mesma linha e colunas adjacentes
        }


        // M�todo para obter o estado atual do tabuleiro (bot�es no painel)
        private int[,] ObterEstadoAtual()
        {
            int[,] estadoAtual = new int[3, 3];
            int index = 0;

            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    int valor = string.IsNullOrEmpty(btn.Text) ? 0 : int.Parse(btn.Text); // O bot�o vazio tem valor 0
                    estadoAtual[index / 3, index % 3] = valor;
                    index++;
                }
            }

            return estadoAtual;
        }

        // M�todo para atualizar o tabuleiro com base no estado atual
        private void AtualizarTabuleiro(int[,] tabuleiro)
        {
            int index = 0;

            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    int valor = tabuleiro[index / 3, index % 3];

                    // Atualiza o texto, o valor 0 representa o espa�o vazio
                    btn.Text = valor == 0 ? "" : valor.ToString();

                    // Atualiza a imagem do bot�o se ele n�o for o vazio
                    if (valor != 0)
                    {
                        // Usa o valor do bot�o para buscar a imagem correspondente (supondo que as imagens estejam armazenadas)
                        btn.BackgroundImage = GetImagemFragmento(valor);
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else
                    {
                        // Se for o espa�o vazio, remove a imagem
                        btn.BackgroundImage = null;
                    }

                    index++;
                }
            }
        }


        private Image GetImagemFragmento(int valor)
        {
            // Verifica se o valor � v�lido e se est� dentro dos limites da lista de fragmentos
            if (valor > 0 && valor <= listaDeFragmentos.Count)
            {
                return listaDeFragmentos[valor - 1]; // Retorna o fragmento correspondente (1-8)
            }
            return null; // Caso contr�rio, retorna null (para o espa�o vazio ou valor inv�lido)
        }

        private string EstadoParaString(int[,] tabuleiro)
        {
            string resultado = "";
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    resultado += tabuleiro[i, j] == 0 ? " " : tabuleiro[i, j].ToString(); // Usa espa�o para o valor 0
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

            int[,] estadoInicial = ObterEstadoAtual(); // Obt�m o estado atual do tabuleiro

            // Definir o estado objetivo (puzzle resolvido)
            int[,] estadoObjetivo = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 0 } // O 0 representa o espa�o vazio
            };

            // Executar a busca em largura
            var (solucao, nosGerados, tempo) = iaBuscaHorizontal.BuscaLargura(estadoInicial, estadoObjetivo);

            // Limpar o ListBox antes de exibir a nova solu��o
            // Clear the RichTextBox before displaying the new solution
            richTextBox1.Clear();

            // Check if a solution was found
            if (solucao != null && solucao.Count > 0)
            {
                richTextBox1.AppendText($"Solu��o encontrada em {tempo} ms\n");
                richTextBox1.AppendText($"N�s gerados: {nosGerados}\n");
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
                MessageBox.Show($"Nenhuma solu��o encontrada.\nN�s gerados: {nosGerados}\nTempo: {tempo} ms");
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
            // Exibe uma caixa de di�logo para o usu�rio inserir a sequ�ncia
            string input = Microsoft.VisualBasic.Interaction.InputBox("Digite uma sequ�ncia de 9 n�meros de 1 a 9, sem repeti��o.",
                                                                      "Entrada de Sequ�ncia",
                                                                      "123456789");

            // Verifica se o valor n�o foi cancelado (string vazia significa que foi cancelado)
            if (!string.IsNullOrEmpty(input))
            {
                if (ValidarEntrada(input))
                {
                    AtualizarTabuleiroComEntrada(input); // Atualiza o tabuleiro com a nova sequ�ncia
                }
                else
                {
                    MessageBox.Show("Entrada inv�lida. Digite uma sequ�ncia de 9 n�meros �nicos entre 1 e 9.");
                }
            }
        }

        // M�todo para validar a entrada
        private bool ValidarEntrada(string input)
        {
            // Verifica se a entrada tem exatamente 9 caracteres
            if (input.Length != 9)
            {
                return false;
            }

            // Verifica se cont�m apenas n�meros de 1 a 9 sem repeti��o
            return input.Distinct().Count() == 9 && input.All(c => char.IsDigit(c) && c != '0');
        }

        // M�todo para atualizar o tabuleiro com a entrada fornecida
        private void AtualizarTabuleiroComEntrada(string input)
        {
            // Converte a string de entrada em uma lista de inteiros
            List<int> valores = input.Select(c => int.Parse(c.ToString())).ToList();

            // Atualiza os bot�es com os valores fornecidos
            int index = 0;
            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Button btn)
                {
                    int valor = valores[index];

                    // Define o texto do bot�o baseado no valor
                    btn.Text = valor == 9 ? "" : valor.ToString(); // O valor 9 ser� o espa�o vazio

                    // Atualiza a imagem, se houver
                    if (valor > 0 && valor <= listaDeFragmentos.Count)
                    {
                        btn.BackgroundImage = listaDeFragmentos[valor - 1]; // Define o fragmento da imagem
                        btn.BackgroundImageLayout = ImageLayout.Stretch;
                    }
                    else
                    {
                        btn.BackgroundImage = null; // Limpa a imagem para o espa�o vazio
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
