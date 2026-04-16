# language: pt
Funcionalidade: Login na aplicação SumUp
  Como um cliente da SumUp
  Eu quero realizar login na minha conta
  Para que eu possa gerenciar minhas vendas e maquininhas

  Contexto:
    Dado que eu acesso a página de login da SumUp

  @smoke @login @regressao
  Cenário: Tentativa de login com credenciais inválidas
    Quando eu tento realizar login com email "usuario_fake@teste.com" e senha "SenhaInvalida123!"
    Então uma mensagem de erro deve ser exibida
    E eu devo permanecer na página de login

  @login @bloqueio
  Cenário: Validar presença de captcha no login
    Então o sistema deve processar o login com segurança
    E se houver captcha, ele deve ser identificado para tratamento

  @login @mfa
  Cenário: Validar fluxo de autenticação multifator
    Dado que eu tento realizar login com credenciais válidas
    Então o sistema pode solicitar um código de verificação para prosseguir
