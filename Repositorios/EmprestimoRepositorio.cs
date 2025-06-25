using BibliotecaVirtual.Entidades;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BibliotecaVirtual.Repositorios
{
    public class EmprestimoRepositorio
    {
        private List<Emprestimo> emprestimos = new List<Emprestimo>();
        private int proximoId = 1;
        private readonly string caminhoArquivo = "emprestimos.json";

        public EmprestimoRepositorio()
        {
            CarregarDeArquivo();
        }

        public void Adicionar(Emprestimo emprestimo)
        {
            emprestimo.Id = proximoId++;
            emprestimos.Add(emprestimo);
            SalvarEmArquivo();
        }

        public List<Emprestimo> ListarTodos() => emprestimos;

        public List<Emprestimo> ListarAtivos() => emprestimos.Where(e => e.DataDevolucao == null).ToList();

        public Emprestimo BuscarPorId(int id) => emprestimos.FirstOrDefault(e => e.Id == id);

        private void SalvarEmArquivo()
        {
            var json = JsonConvert.SerializeObject(emprestimos, Formatting.Indented);
            File.WriteAllText(caminhoArquivo, json);
        }

        private void CarregarDeArquivo()
        {
            if (File.Exists(caminhoArquivo))
            {
                var json = File.ReadAllText(caminhoArquivo);
                var lista = JsonConvert.DeserializeObject<List<Emprestimo>>(json);
                if (lista != null)
                {
                    emprestimos = lista;
                    proximoId = emprestimos.Any() ? emprestimos.Max(e => e.Id) + 1 : 1;
                }
            }
        }
    }
}

