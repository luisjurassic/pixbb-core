## PIXBB Core

Biblioteca de acesso à api **V1** do serviço de pagamento instantâneo brasileiro (PIX) do Banco do Brasil(BB), baseada em .Net
Standard 2.0.

### Primeiros passos

- Criar conta no portal [Developer BB](https://app.developers.bb.com.br/) com cpf do administrador da conta.
- V2 obriga a criação de certificado digital para testes e produção.
- Criar 2 projetos, um para Teste e um para Prod, um vez que o projeto seja aprovado para produção, não funciona em
  homolog.

As implementações da biblioteca atendem a versão 1 da API. A versão 2 precisa ser ajustada na biblioteca. 

### Como usar?
Exemplo basico de uso. Para mais exemplos consulte o projeto de testes unitários
```csharp    
    var options = new PixOptions
    {
        Certificate = new X509Certificate2(@"cert-chain.pem"),
        ApiVersion = ApiVersion.V1,
        DeveloperApplicationKey = "...",
        ClientId = "...",
        ClientSecret = "..."
    };
    
    var client = new PixClient(options);
    
    var billing = new Billing()
    {
        Calendar = new Calendar(3600),
        Price = new Price(10),
        Key = "7f6844d0-de89-47e5-9ef7-e0a35a681615",
        PayerRequest = "Serviço prestado"
    };
    
    var cobranca = await client.ChargeAsync(billing);    
    Console.WriteLine($"Cobrança registrada. txid: {cobranca.TransactionId} link: {cobranca.ImageContentQRcode}"
    
    var consulta = await client.GetChargeAsync("fBMM12dETsQofwAVTmHM2lVKd6dbwW8vb9H");
    Console.WriteLine($"Cobrança encontrada. txid: {consulta.TransactionId}");    
```

### Exemplos
Você pode executar os exemplos contidos no projeto **PixBB.Test**

### Contributing

Relatórios de bugs e solicitações pull são bem-vindos no GitHub em https://github.com/luisjurassic/pixbb-core.


### Licença
A biblioteca está disponível como código aberto sob os termos da [Licença MIT](LICENSE).
