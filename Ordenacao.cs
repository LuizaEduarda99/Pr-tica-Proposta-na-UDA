namespace OrdenacaoTransacoes;

/// <summary>
/// Algoritmos de ordenação adaptados para Transacao[], usando Valor como chave.
/// Todos ordenam em ordem CRESCENTE.
/// </summary>
public static class Ordenacao
{
    // =====================================================================
    // MERGE SORT  -  O(n log n) em todos os casos | espaço O(n) | ESTÁVEL
    // =====================================================================
    public static void MergeSort(Transacao[] v) => MergeSort(v, 0, v.Length - 1);

    private static void MergeSort(Transacao[] v, int ini, int fim)
    {
        if (ini < fim)
        {
            int meio = (ini + fim) / 2;
            MergeSort(v, ini, meio);
            MergeSort(v, meio + 1, fim);
            Merge(v, ini, meio, fim);
        }
    }

    private static void Merge(Transacao[] v, int ini, int meio, int fim)
    {
        int n1 = meio - ini + 1;
        int n2 = fim - meio;
        Transacao[] L = new Transacao[n1];
        Transacao[] R = new Transacao[n2];

        for (int i = 0; i < n1; i++) L[i] = v[ini + i];
        for (int j = 0; j < n2; j++) R[j] = v[meio + 1 + j];

        int iL = 0, iR = 0, k = ini;
        while (iL < n1 && iR < n2)
        {
            // Comparação adaptada: L[iL] <= R[iR]  ->  L[iL].Valor <= R[iR].Valor
            if (L[iL].Valor <= R[iR].Valor)
                v[k++] = L[iL++];
            else
                v[k++] = R[iR++];
        }
        while (iL < n1) v[k++] = L[iL++];
        while (iR < n2) v[k++] = R[iR++];
    }

    // =====================================================================
    // QUICKSORT  -  médio O(n log n) | pior O(n²) | espaço O(log n) | NÃO estável
    // Particionamento de Lomuto com pivô = mediana de três (evita o pior caso
    // em vetores já ordenados).
    // =====================================================================
    public static void QuickSort(Transacao[] v) => QuickSort(v, 0, v.Length - 1);

    private static void QuickSort(Transacao[] v, int ini, int fim)
    {
        while (ini < fim)
        {
            int p = Particionar(v, ini, fim);
            // Recursão no lado menor e laço no maior: pilha limitada a O(log n)
            if (p - ini < fim - p)
            {
                QuickSort(v, ini, p - 1);
                ini = p + 1;
            }
            else
            {
                QuickSort(v, p + 1, fim);
                fim = p - 1;
            }
        }
    }

    private static int Particionar(Transacao[] v, int ini, int fim)
    {
        // Mediana de três: coloca a mediana de v[ini], v[meio], v[fim] em v[fim]
        int meio = ini + (fim - ini) / 2;
        if (v[meio].Valor < v[ini].Valor) Trocar(v, meio, ini);
        if (v[fim].Valor < v[ini].Valor) Trocar(v, fim, ini);
        if (v[meio].Valor < v[fim].Valor) Trocar(v, meio, fim);

        decimal pivo = v[fim].Valor;
        int i = ini - 1;
        for (int j = ini; j < fim; j++)
        {
            if (v[j].Valor <= pivo)
            {
                i++;
                Trocar(v, i, j);
            }
        }
        Trocar(v, i + 1, fim);
        return i + 1; // posição final do pivô
    }

    // =====================================================================
    // HEAPSORT  -  O(n log n) em todos os casos | espaço O(1) | NÃO estável
    // =====================================================================
    public static void HeapSort(Transacao[] v)
    {
        int n = v.Length;

        // 1) Constrói o heap máximo (a partir do último nó que tem filhos)
        for (int i = n / 2 - 1; i >= 0; i--)
            Heapify(v, n, i);

        // 2) Move a raiz (maior) para o final e reorganiza o heap restante
        for (int fim = n - 1; fim > 0; fim--)
        {
            Trocar(v, 0, fim);
            Heapify(v, fim, 0);
        }
    }

    private static void Heapify(Transacao[] v, int tamanho, int i)
    {
        while (true)
        {
            int maior = i;
            int esq = 2 * i + 1;
            int dir = 2 * i + 2;

            if (esq < tamanho && v[esq].Valor > v[maior].Valor) maior = esq;
            if (dir < tamanho && v[dir].Valor > v[maior].Valor) maior = dir;

            if (maior == i) return;
            Trocar(v, i, maior);
            i = maior;
        }
    }

    // =====================================================================
    private static void Trocar(Transacao[] v, int a, int b) =>
        (v[a], v[b]) = (v[b], v[a]);

    public static bool EstaOrdenado(Transacao[] v)
    {
        for (int i = 1; i < v.Length; i++)
            if (v[i - 1].Valor > v[i].Valor) return false;
        return true;
    }
}
