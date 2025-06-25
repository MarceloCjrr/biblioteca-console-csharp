using BibliotecaVirtual.Entidades;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BibliotecaVirtual.Repositorios
{
    public class LivroRepositorio
    {
        private List<Livro> livros = new List<Livro>();
        private int proximoId = 1;
        private readonly string caminhoArquivo = "livros.json";

        public LivroRepositorio()
        {
            CarregarDeArquivo();
        }

        public void Adicionar(Livro livro)
        {
            livro.Id = proximoId++;
            livros.Add(livro);
            SalvarEmArquivo();
        }

        public List<Livro> ListarTodos() => livros;

        public List<Livro> ListarDisponiveis() => livros.Where(l => l.Disponivel).ToList();

        public Livro BuscarPorId(int id) => livros.FirstOrDefault(l => l.Id == id);

        private void SalvarEmArquivo()
        {
            var json = JsonConvert.SerializeObject(livros, Formatting.Indented);
            File.WriteAllText(caminhoArquivo, json);
        }

        private void CarregarDeArquivo()
        {
            if (File.Exists(caminhoArquivo))
            {
                var json = File.ReadAllText(caminhoArquivo);
                var lista = JsonConvert.DeserializeObject<List<Livro>>(json);
                if (lista != null)
                {
                    livros = lista;
                    proximoId = livros.Any() ? livros.Max(l => l.Id) + 1 : 1;
                }
            }
        }
    }
}
