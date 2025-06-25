using BibliotecaVirtual.Entidades;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BibliotecaVirtual.Repositorios
{
    public class AutorRepositorio
    {
        private List<Autor> autores = new List<Autor>();
        private int proximoId = 1;
        private readonly string caminhoArquivo = "autores.json";

        public AutorRepositorio()
        {
            CarregarDeArquivo();
        }

        public void Adicionar(Autor autor)
        {
            autor.Id = proximoId++;
            autores.Add(autor);
            SalvarEmArquivo();
        }

        public List<Autor> ListarTodos() => autores;

        public Autor BuscarPorId(int id) => autores.FirstOrDefault(a => a.Id == id);

        private void SalvarEmArquivo()
        {
            var json = JsonConvert.SerializeObject(autores, Formatting.Indented);
            File.WriteAllText(caminhoArquivo, json);
        }

        private void CarregarDeArquivo()
        {
            if (File.Exists(caminhoArquivo))
            {
                var json = File.ReadAllText(caminhoArquivo);
                var lista = JsonConvert.DeserializeObject<List<Autor>>(json);
                if (lista != null)
                {
                    autores = lista;
                    proximoId = autores.Any() ? autores.Max(a => a.Id) + 1 : 1;
                }
            }
        }
    }
}
