using System.Diagnostics;
using System.Globalization;
using OrdenacaoTransacoes;

CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

// Quantidade de transações (pode passar outro valor como argumento: dotnet run -- 500000)
int quantidade = args.Length > 0 && int.TryParse(args[0], out int q) ? q : 100_000;

Console.WriteLine("==============================================================");
Console.WriteLine("   ORDENAÇÃO DE TRANSAÇÕES - Merge Sort x Quicksort x Heapsort");
Console.WriteLine("==============================================================");
Console.WriteLine($"Gerando {quantidade:N0} transações aleatórias...\n");

Transacao[] original = GerarTransacoes(quantidade, semente: 42);

// Cada algoritmo recebe um CLONE do mesmo array desordenado
var algoritmos = new (string Nome, Action<Transacao[]> Ordenar)[]
{
    ("Merge Sort", Ordenacao.MergeSort),
    ("Quicksort",  Ordenacao.QuickSort),
    ("Heapsort",   Ordenacao.HeapSort),
};

Transacao[]? ordenado = null;
Console.WriteLine($"{"Algoritmo",-12} | {"Tempo (ms)",12} | Ordenado?");
Console.WriteLine(new string('-', 40));

foreach (var (nome, ordenar) in algoritmos)
{
    Transacao[] copia = (Transacao[])original.Clone();

    Stopwatch sw = Stopwatch.StartNew();
    ordenar(copia);
    sw.Stop();

    bool ok = Ordenacao.EstaOrdenado(copia);
    Console.WriteLine($"{nome,-12} | {sw.Elapsed.TotalMilliseconds,12:N2} | {(ok ? "Sim" : "NÃO")}");
    ordenado ??= copia;
}

// 10 maiores transações: após ordenar em ordem crescente, são os 10 últimos
Console.WriteLine("\n==================== 10 MAIORES TRANSAÇÕES ====================");
int total = Math.Min(10, ordenado!.Length);
for (int i = 0; i < total; i++)
{
    Transacao t = ordenado[ordenado.Length - 1 - i]; // do maior para o menor
    Console.WriteLine($"{i + 1,2}º  {t}");
}

static Transacao[] GerarTransacoes(int n, int semente)
{
    string[] descricoes = { "Pix", "Boleto", "TED", "Cartão Crédito", "Cartão Débito",
                            "Saque", "Depósito", "Transferência", "Assinatura", "Compra Online" };
    Random rnd = new Random(semente);
    DateTime inicio = new DateTime(2026, 1, 1);
    var v = new Transacao[n];
    for (int i = 0; i < n; i++)
    {
        decimal valor = Math.Round((decimal)(rnd.NextDouble() * 50_000), 2);
        v[i] = new Transacao(i + 1, inicio.AddDays(rnd.Next(0, 365)),
                             descricoes[rnd.Next(descricoes.Length)], valor);
    }
    return v;
}
