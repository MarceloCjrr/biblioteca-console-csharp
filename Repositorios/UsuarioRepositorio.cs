using BibliotecaVirtual.Entidades;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BibliotecaVirtual.Repositorios
{
    public class UsuarioRepositorio
    {
        private List<Usuario> usuarios = new List<Usuario>();
        private int proximoId = 1;
        private readonly string caminhoArquivo = "usuarios.json";

        public UsuarioRepositorio()
        {
            CarregarDeArquivo();
        }

        public void Adicionar(Usuario usuario)
        {
            usuario.Id = proximoId++;
            usuarios.Add(usuario);
            SalvarEmArquivo();
        }

        public List<Usuario> ListarTodos() => usuarios;

        public Usuario BuscarPorId(int id) => usuarios.FirstOrDefault(u => u.Id == id);

        private void SalvarEmArquivo()
        {
            var json = JsonConvert.SerializeObject(usuarios, Formatting.Indented);
            File.WriteAllText(caminhoArquivo, json);
        }

        private void CarregarDeArquivo()
        {
            if (File.Exists(caminhoArquivo))
            {
                var json = File.ReadAllText(caminhoArquivo);
                var lista = JsonConvert.DeserializeObject<List<Usuario>>(json);
                if (lista != null)
                {
                    usuarios = lista;
                    proximoId = usuarios.Any() ? usuarios.Max(u => u.Id) + 1 : 1;
                }
            }
        }
    }
}

