using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Puzzle8
{
    class IaBuscaHorizontal
    {
        public (List<Estado>, int, long) BuscaLargura(int[,] estadoInicial, int[,] estadoObjetivo)
        {
            Queue<Estado> abertos = new Queue<Estado>();
            HashSet<string> fechados = new HashSet<string>();
            abertos.Enqueue(new Estado(estadoInicial));

            int nosGerados = 0; // Contador de nós
            Stopwatch stopwatch = new Stopwatch(); // Para medir o tempo de execução
            stopwatch.Start(); // Inicia o cronômetro

            while (abertos.Any())
            {
                Estado X = abertos.Dequeue();

                // Verifica se o estado atual é o objetivo
                if (X.EhObjetivo(estadoObjetivo))
                {
                    stopwatch.Stop(); // Para o cronômetro ao encontrar a solução
                    return (Caminho(X), nosGerados, stopwatch.ElapsedMilliseconds); // Retorna solução, nós gerados e tempo
                }

                fechados.Add(TabuleiroToString(X.Tabuleiro));

                // Gera os filhos do estado atual
                foreach (Estado filho in X.GerarFilhos())
                {
                    string filhoStr = TabuleiroToString(filho.Tabuleiro);
                    nosGerados++; // Conta cada nó gerado

                    // Adiciona o filho apenas se não estiver em fechados
                    if (!fechados.Contains(filhoStr))
                    {
                        abertos.Enqueue(filho);
                    }
                }
            }

            stopwatch.Stop(); // Para o cronômetro caso não encontre solução
            return (null, nosGerados, stopwatch.ElapsedMilliseconds); // Retorna os valores mesmo sem solução
        }

        // Converte o caminho da solução em uma lista de estados
        private List<Estado> Caminho(Estado estado)
        {
            List<Estado> caminho = new List<Estado>();
            while (estado != null)
            {
                caminho.Insert(0, estado);
                estado = estado.Pai;
            }
            return caminho;
        }

        // Converte o tabuleiro para uma string para fácil comparação e armazenamento
        private string TabuleiroToString(int[,] tabuleiro)
        {
            return string.Join(",", tabuleiro.Cast<int>());
        }
    }
}
