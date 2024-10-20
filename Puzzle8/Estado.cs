using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle8
{
    internal class Estado
    {
        public int[,] Tabuleiro { get; set; }
        public Estado Pai { get; set; }
        public int Custo { get; set; } // Número de movimentos (g)
        public int Heuristica { get; set; } // Distância de Manhattan (h)

        public Estado(int[,] tabuleiro, Estado pai = null)
        {
            Tabuleiro = (int[,])tabuleiro.Clone();
            Pai = pai;
            Custo = pai != null ? pai.Custo + 1 : 0;
            Heuristica = CalcularHeuristica();
        }

        // Função para calcular a distância de Manhattan
        private int CalcularHeuristica()
        {
            int dist = 0;
            int n = Tabuleiro.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int valor = Tabuleiro[i, j];
                    if (valor != 0) // Ignorar a peça vazia
                    {
                        int objetivoX = (valor - 1) / n;
                        int objetivoY = (valor - 1) % n;
                        dist += Math.Abs(i - objetivoX) + Math.Abs(j - objetivoY);
                    }
                }
            }
            return dist;
        }

        // Verifica se o estado atual é igual ao estado objetivo
        public bool EhObjetivo(int[,] estadoObjetivo)
        {
            for (int i = 0; i < Tabuleiro.GetLength(0); i++)
            {
                for (int j = 0; j < Tabuleiro.GetLength(1); j++)
                {
                    if (Tabuleiro[i, j] != estadoObjetivo[i, j])
                        return false;
                }
            }
            return true;
        }

        // Gera os filhos (estados possíveis a partir do estado atual)
        public List<Estado> GerarFilhos()
        {
            List<Estado> filhos = new List<Estado>();
            int n = Tabuleiro.GetLength(0);
            int linhaVazia = 0, colunaVazia = 0;

            // Encontrar a posição vazia (representada pelo 0)
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (Tabuleiro[i, j] == 0)
                    {
                        linhaVazia = i;
                        colunaVazia = j;
                    }
                }
            }

            // Movimentos possíveis: cima, baixo, esquerda, direita
            int[,] movimentos = new int[,]
            {
                { -1, 0 }, // Cima
                { 1, 0 },  // Baixo
                { 0, -1 }, // Esquerda
                { 0, 1 }   // Direita
            };

            for (int i = 0; i < movimentos.GetLength(0); i++)
            {
                int novaLinha = linhaVazia + movimentos[i, 0];
                int novaColuna = colunaVazia + movimentos[i, 1];

                if (novaLinha >= 0 && novaLinha < n && novaColuna >= 0 && novaColuna < n)
                {
                    // Clona o tabuleiro atual e faz o movimento
                    int[,] novoTabuleiro = (int[,])Tabuleiro.Clone();
                    novoTabuleiro[linhaVazia, colunaVazia] = novoTabuleiro[novaLinha, novaColuna];
                    novoTabuleiro[novaLinha, novaColuna] = 0;

                    // Cria um novo estado filho e adiciona à lista
                    filhos.Add(new Estado(novoTabuleiro, this));
                }
            }

            return filhos;
        }
    }
}
