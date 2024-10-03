using System;

namespace DapperCrud
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=localhost;User Id=root;Password=root;Port=3309;Database=TesteDapper;";
            ConfiguracaoBancoDeDados.CriarBancoDeDados(connectionString);

            var usuarioRepositorio = new UsuarioRepositorio(connectionString);

            while (true)
            {
                Console.WriteLine("\n\nPressione qualquer tecla para continuar...\n");
                Console.ReadKey();

                Console.Clear();

                Console.WriteLine("\n\n<<< Sistema de Manutenção de Usuários (com Dapper) >>>\n");

                Console.WriteLine("\nMenu de Opções:\n");
                Console.WriteLine("1. Cadastrar Usuário");
                Console.WriteLine("2. Listar Todos os Usuários");
                Console.WriteLine("3. Buscar Usuário por ID");
                Console.WriteLine("4. Buscar Usuário por Filtro (Nome/Idade)");
                Console.WriteLine("5. Atualizar Usuário");
                Console.WriteLine("6. Remover Usuário");
                Console.WriteLine("7. Sair\n");
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        CadastrarUsuario(usuarioRepositorio);
                        break;
                    case "2":
                        ListarUsuarios(usuarioRepositorio);
                        break;
                    case "3":
                        BuscarUsuarioPorId(usuarioRepositorio);
                        break;
                    case "4":
                        BuscarUsuarioPorFiltro(usuarioRepositorio);
                        break;
                    case "5":
                        AtualizarUsuario(usuarioRepositorio);
                        break;
                    case "6":
                        RemoverUsuario(usuarioRepositorio);
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("\nOpção inválida. Tente novamente.\n");
                        break;
                }
            }
        }

        static void CadastrarUsuario(UsuarioRepositorio usuarioRepositorio)
        {
            Console.Write("\nDigite o nome do usuário: ");
            var nome = Console.ReadLine();

            Console.Write("Digite a idade do usuário: ");
            if (int.TryParse(Console.ReadLine(), out int idade))
            {
                var novoUsuario = new Usuario { Nome = nome, Idade = idade };
                usuarioRepositorio.Adicionar(novoUsuario);
                Console.WriteLine("\nUsuário cadastrado com sucesso!\n");
            }
            else
            {
                Console.WriteLine("\nIdade inválida. Tente novamente.\n");
            }
        }

        static void ListarUsuarios(UsuarioRepositorio usuarioRepositorio)
        {
            var usuarios = usuarioRepositorio.ObterTodos();
            if (usuarios.Count == 0)
            {
                Console.WriteLine("\nNenhum usuário cadastrado.\n");
            }
            else
            {
                Console.WriteLine("\nTodos os usuários:\n");
                foreach (var usuario in usuarios)
                {
                    Console.WriteLine($"Id: {usuario.Id}, Nome: {usuario.Nome}, Idade: {usuario.Idade}");
                }
            }
        }

        static void BuscarUsuarioPorId(UsuarioRepositorio usuarioRepositorio)
        {
            Console.Write("\nDigite o ID do usuário: \n");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var usuario = usuarioRepositorio.ObterPorId(id);
                if (usuario != null)
                {
                    Console.WriteLine($"Id: {usuario.Id}, Nome: {usuario.Nome}, Idade: {usuario.Idade}");
                }
                else
                {
                    Console.WriteLine("\nUsuário não encontrado.\n");
                }
            }
            else
            {
                Console.WriteLine("\nID inválido.\n");
            }
        }

        static void BuscarUsuarioPorFiltro(UsuarioRepositorio usuarioRepositorio)
        {
            Console.Write("\nDigite o nome do usuário (ou deixe em branco): ");
            var nome = Console.ReadLine();

            Console.Write("Digite a idade do usuário (ou deixe em branco): ");
            int? idade = null;
            if (int.TryParse(Console.ReadLine(), out int idadeInput))
            {
                idade = idadeInput;
            }

            var usuarios = usuarioRepositorio.ObterPorFiltro(nome, idade);
            if (usuarios.Count == 0)
            {
                Console.WriteLine("\nNenhum usuário encontrado com os filtros fornecidos.\n");
            }
            else
            {
                Console.WriteLine("\nUsuários encontrados:\n");
                foreach (var usuario in usuarios)
                {
                    Console.WriteLine($"Id: {usuario.Id}, Nome: {usuario.Nome}, Idade: {usuario.Idade}");
                }
            }
        }

        static void AtualizarUsuario(UsuarioRepositorio usuarioRepositorio)
        {
            Console.Write("\nDigite o ID do usuário a ser atualizado: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var usuario = usuarioRepositorio.ObterPorId(id);
                if (usuario != null)
                {
                    Console.Write($"Digite o novo nome do usuário (deixe em branco para manter '{usuario.Nome}'): ");
                    var novoNome = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoNome))
                    {
                        usuario.Nome = novoNome;
                    }

                    Console.Write($"Digite a nova idade do usuário (deixe em branco para manter '{usuario.Idade}'): ");
                    var novaIdadeInput = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novaIdadeInput) && int.TryParse(novaIdadeInput, out int novaIdade))
                    {
                        usuario.Idade = novaIdade;
                    }

                    usuarioRepositorio.Atualizar(usuario);
                    Console.WriteLine("\nUsuário atualizado com sucesso!\n");
                }
                else
                {
                    Console.WriteLine("\nUsuário não encontrado.\n");
                }
            }
            else
            {
                Console.WriteLine("\nID inválido.\n");
            }
        }

        static void RemoverUsuario(UsuarioRepositorio usuarioRepositorio)
        {
            Console.Write("\nDigite o ID do usuário a ser removido: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var usuario = usuarioRepositorio.ObterPorId(id);
                if (usuario != null)
                {
                    usuarioRepositorio.Remover(id);
                    Console.WriteLine("\nUsuário removido com sucesso!\n");
                }
                else
                {
                    Console.WriteLine("\nUsuário não encontrado.\n");
                }
            }
            else
            {
                Console.WriteLine("\nID inválido.\n");
            }
        }
    }
}
