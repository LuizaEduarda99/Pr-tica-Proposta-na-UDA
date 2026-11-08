namespace OrdenacaoTransacoes;

/// <summary>
/// Representa uma transação financeira. A propriedade Valor é a chave de ordenação.
/// </summary>
public class Transacao
{
    public int Id { get; set; }
    public DateTime Data { get; set; }
    public string Descricao { get; set; } = "";
    public decimal Valor { get; set; }

    public Transacao(int id, DateTime data, string descricao, decimal valor)
    {
        Id = id;
        Data = data;
        Descricao = descricao;
        Valor = valor;
    }

    public override string ToString() =>
        $"#{Id,-6} {Data:dd/MM/yyyy}  {Descricao,-15} R$ {Valor,12:N2}";
}
