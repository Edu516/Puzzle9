using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Puzzle8
{
    internal class IaBuscaMelhorEscolha
    {
        public (List<Estado>, int, long) BuscaMelhorEscolha(int[,] estadoInicial, int[,] estadoObjetivo)
        {
            List<Estado> abertos = new List<Estado> { new Estado(estadoInicial) };
            HashSet<string> fechados = new HashSet<string>();
            int nosGerados = 0; // Contador de nós
            Stopwatch stopwatch = new Stopwatch(); // Para medir o tempo de execução
            stopwatch.Start(); // Inicia o cronômetro

            while (abertos.Any())
            {
                // Ordenar os abertos pelo valor heurístico (h + g)
                abertos = abertos.OrderBy(e => e.Heuristica + e.Custo).ToList();

                Estado X = abertos.First();
                abertos.RemoveAt(0);

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
                    nosGerados++; // Conta cada nó gerado
                    string filhoStr = TabuleiroToString(filho.Tabuleiro);

                    if (!fechados.Contains(filhoStr) && !abertos.Any(e => TabuleiroToString(e.Tabuleiro) == filhoStr))
                    {
                        abertos.Add(filho);
                    }
                    else if (abertos.Any(e => TabuleiroToString(e.Tabuleiro) == filhoStr && filho.Custo < e.Custo))
                    {
                        Estado existente = abertos.First(e => TabuleiroToString(e.Tabuleiro) == filhoStr);
                        existente.Custo = filho.Custo;
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

