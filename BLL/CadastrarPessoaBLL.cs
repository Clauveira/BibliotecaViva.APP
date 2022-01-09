using System;

using BLL.Utils;
using BLL.Interface;
using BibliotecaViva.DTO;
using BibliotecaViva.SAL;
using BibliotecaViva.BLL.Utils;
using BibliotecaViva.SAL.Interface;

namespace BLL
{
    public class CadastrarPessoaBLL : ICadastrarPessoaBLL, IDisposable
    {
        private IRequisicao SAL { get; set; }
        private string URLCadastroPessoa { get; set; }
        public CadastrarPessoaBLL()
        {
            URLCadastroPessoa = Apontamentos.URLApi + "/Pessoa/Cadastrar";
            SAL = new Requisicao();
        }
        private void ValidarPreenchimento(string nome, string sobrenome, string genero, string latlong)
        {
            if (string.IsNullOrEmpty(nome))
            	throw new Exception("Por favor preencher o Nome.");
            if (string.IsNullOrEmpty(sobrenome))
            	throw new Exception("Por favor preencher o Sobrenome.");
            if (string.IsNullOrEmpty(genero))
            	throw new Exception("Por favor preencher o Genero.");
            if (!string.IsNullOrEmpty(latlong))
                try
                {
                    var coordenadas = TratadorUtil.ProcessarLatLong(latlong);
                }
                catch(Exception ex)
                {
                    throw new Exception("Por favor preencher um valor de Latitude e Longitude com valores válidos.");
                }
        }
        public PessoaDTO PopularPessoa(string nome, string sobrenome, string genero, string apelido, string latlong)
        {
            ValidarPreenchimento(nome, sobrenome, genero, latlong);
            var coordenadas = TratadorUtil.ProcessarLatLong(latlong);

            return new PessoaDTO()
            {
                Nome = nome,
                Sobrenome = sobrenome,
                Apelido = apelido,
                NomeSocial = string.Empty,
                Genero = genero,
                Latitude = coordenadas[0],
                Longitude = coordenadas[1]
            };
        }
        public string CadastrarPessoa(PessoaDTO pessoa)
        {    
            return SAL.ExecutarPost<PessoaDTO, string>(URLCadastroPessoa, pessoa);
        }
        public void Dispose()
        {
            URLCadastroPessoa = null;
            SAL.Dispose();
        }
    }
}