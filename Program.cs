using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaVirtual.Repositorios;
using BibliotecaVirtual.Entidades;
using Newtonsoft.Json;
using System.IO;

namespace ProjetoBiblioteca
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            var autorRepo = new AutorRepositorio();
            var usuarioRepo = new UsuarioRepositorio();
            var livroRepo = new LivroRepositorio();
            var emprestimoRepo = new EmprestimoRepositorio();

            bool rodando = true;
            while (rodando)
            {
                Console.Clear();
                Console.WriteLine("--------Menu Opções----------");
                Console.WriteLine("1. Cadastrar Usuário\n2. Cadastrar Autor\n3. Cadastrar Livro");
                Console.WriteLine("4. Realizar emprestimo\n5. Devolver Livro\n6. Listar Livros Disponíveis");
                Console.WriteLine("7. Listar Empréstimos\n0. Sair\n");
                Console.Write("Opção Escolhinda: ");
                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CadastrarUsuario(usuarioRepo);
                        break;

                    case "2":
                        CadastrarAutor(autorRepo);
                        break;

                    case "3":
                        CadastrarLivro(livroRepo, autorRepo);
                        break;

                    case "4":
                        RealizarEmprestimo(livroRepo, usuarioRepo, emprestimoRepo);
                        break;

                    case "5":
                        DevolverLivro(emprestimoRepo);
                        break;

                    case "6":
                        ListarLivrosDisponiveis(livroRepo);
                        break;

                    case "7":
                        ListarEmprestimos(emprestimoRepo);
                        break;

                    case "0":
                        rodando = false;
                        Console.Write("Saindo do programa.");
                        break;
                    default:
                        Console.Write("Opção inválida. Pressione qualquer tecla para tentar novamente");
                        Console.ReadKey();
                        break;

                }
            }
        }

        static void CadastrarAutor(AutorRepositorio autorRepo)
        {
            Console.Clear();
            Console.WriteLine("=== Cadastro de Autor ===");

            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("Nacionalidade: ");
            string nacionalidade = Console.ReadLine();

            Autor autor = new Autor
            {
                Nome = nome,
                Nacionalidade = nacionalidade
            };

            autorRepo.Adicionar(autor);
            Console.WriteLine("Autor cadastrado com sucesso!");
            Console.ReadKey();
        }


        static void CadastrarUsuario(UsuarioRepositorio usuarioRepo)
        {
            Console.Clear();
            Console.WriteLine("=== Cadastro de Usuário ===");

            Console.Write("Nome: ");
            string nome = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Usuario usuario = new Usuario
            {
                Nome = nome,
                Email = email
            };

            usuarioRepo.Adicionar(usuario);
            Console.WriteLine("Usuário cadastrado com sucesso!");
            Console.ReadKey();
        }


        static void CadastrarLivro(LivroRepositorio livroRepo, AutorRepositorio autorRepo)
        {
            Console.Clear();
            Console.WriteLine("=== Cadastro de Livro ===");

            var autores = autorRepo.ListarTodos();
            if (!autores.Any())
            {
                Console.WriteLine("Não há autores cadastrados. Cadastre um autor primeiro.");
                return;
            }

            Console.Write("Título: ");
            string titulo = Console.ReadLine();

            Console.Write("Ano de Publicação: ");
            
            int ano = int.Parse(Console.ReadLine());

            Console.WriteLine("Autores disponíveis:");
            foreach (var autor in autores)
            {
                Console.WriteLine($"{autor.Id} - {autor.Nome}");
            }

            Console.Write("Digite o ID do autor: ");
            int idAutor = int.Parse(Console.ReadLine());
            Autor autorSelecionado = autorRepo.BuscarPorId(idAutor);

            if (autorSelecionado == null)
            {
                Console.WriteLine("Autor não encontrado.");
                return;
            }

            Livro livro = new Livro
            {
                Titulo = titulo,
                AnoPublicado = ano,
                Autor = autorSelecionado,
                Disponivel = true
            };

            livroRepo.Adicionar(livro);
            Console.WriteLine("Livro cadastrado com sucesso!");
            Console.ReadKey();
        }


        static void RealizarEmprestimo(LivroRepositorio livroRepo, UsuarioRepositorio usuarioRepo, EmprestimoRepositorio emprestimoRepo)
        {
            Console.Clear();
            Console.WriteLine("=== Realizar Empréstimo ===");

            var livrosDisponiveis = livroRepo.ListarDisponiveis();
            if (!livrosDisponiveis.Any())
            {
                Console.WriteLine("Nenhum livro disponível.");
                return;
            }

            foreach (var livro in livrosDisponiveis)
                Console.WriteLine($"{livro.Id} - {livro.Titulo} ({livro.Autor.Nome})");

            Console.Write("Digite o ID do livro: ");
            int idLivro = int.Parse(Console.ReadLine());
            Livro livroSelecionado = livroRepo.BuscarPorId(idLivro);

            if (livroSelecionado == null || !livroSelecionado.Disponivel)
            {
                Console.WriteLine("Livro inválido.");
                return;
            }

            var usuarios = usuarioRepo.ListarTodos();
            foreach (var usuario in usuarios)
                Console.WriteLine($"{usuario.Id} - {usuario.Nome}");

            Console.Write("Digite o ID do usuário: ");
            int idUsuario = int.Parse(Console.ReadLine());
            Usuario usuarioSelecionado = usuarioRepo.BuscarPorId(idUsuario);

            if (usuarioSelecionado == null)
            {
                Console.WriteLine("Usuário não encontrado.");
                return;
            }

            Emprestimo emprestimo = new Emprestimo
            {
                Livro = livroSelecionado,
                Usuario = usuarioSelecionado,
                DataEmprestimo = DateTime.Now
            };

            livroSelecionado.Disponivel = false;
            emprestimoRepo.Adicionar(emprestimo);

            Console.WriteLine($"Livro '{livroSelecionado.Titulo}' emprestado para {usuarioSelecionado.Nome} com sucesso!");
            Console.ReadKey();
        }

        static void DevolverLivro(EmprestimoRepositorio emprestimoRepo)
        {
            Console.Clear();
            Console.WriteLine("=== Devolução de Livro ===");

            var emprestimosAtivos = emprestimoRepo.ListarAtivos();
            if (!emprestimosAtivos.Any())
            {
                Console.WriteLine("Nenhum empréstimo ativo.");
                return;
            }

            foreach (var e in emprestimosAtivos)
                Console.WriteLine($"{e.Id} - {e.Livro.Titulo} (Usuário: {e.Usuario.Nome})");

            Console.Write("Digite o ID do empréstimo: ");
            int id = int.Parse(Console.ReadLine());
            var emprestimo = emprestimoRepo.BuscarPorId(id);

            if (emprestimo == null || emprestimo.DataDevolucao != null)
            {
                Console.WriteLine("Empréstimo inválido.");
                return;
            }

            emprestimo.DataDevolucao = DateTime.Now;
            emprestimo.Livro.Disponivel = true;

            Console.WriteLine("Livro devolvido com sucesso!");
            Console.ReadKey();
        }

        static void ListarLivrosDisponiveis(LivroRepositorio livroRepo)
        {
            Console.Clear();
            Console.WriteLine("=== Livros Disponíveis ===");

            var livros = livroRepo.ListarDisponiveis();
            if (!livros.Any())
            {
                Console.WriteLine("Nenhum livro disponível no momento.");
                return;
            }

            foreach (var livro in livros)
            {
                Console.WriteLine($"ID: {livro.Id} | Título: {livro.Titulo} | Autor: {livro.Autor.Nome} | Ano: {livro.AnoPublicado}");
            }
            Console.Write("Press ENTER to go back!");
            Console.ReadKey();
        }

        static void ListarEmprestimos(EmprestimoRepositorio emprestimoRepo)
        {
            Console.Clear();
            Console.WriteLine("=== Lista de Empréstimos ===");

            var emprestimos = emprestimoRepo.ListarTodos();
            if (!emprestimos.Any())
            {
                Console.WriteLine("Nenhum empréstimo registrado.");
                return;
            }

            foreach (var e in emprestimos)
            {
                string status = e.DataDevolucao == null ? "Em andamento" : $"Devolvido em {e.DataDevolucao.Value.ToShortDateString()}";

                Console.WriteLine($"\nID: {e.Id}");
                Console.WriteLine($"Livro: {e.Livro.Titulo}");
                Console.WriteLine($"Usuário: {e.Usuario.Nome}");
                Console.WriteLine($"Data do Empréstimo: {e.DataEmprestimo.ToShortDateString()}");
                Console.WriteLine($"Status: {status}");
            }
            Console.ReadKey();
        }
    }
}
