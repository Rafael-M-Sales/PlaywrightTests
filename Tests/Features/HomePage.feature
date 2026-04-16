# language: pt
Funcionalidade: Página Inicial da SumUp
  Como um usuário da SumUp
  Eu quero acessar a página inicial
  Para que eu possa conhecer os produtos e serviços oferecidos

  Contexto:
    Dado que eu acesso a página inicial da SumUp

  @smoke @homepage @regressao
  Cenário: Validar carregamento completo da página inicial
    Então a página inicial deve carregar completamente
    E o logo da SumUp deve estar visível
    E o header deve estar visível
    E o footer deve estar visível

  @homepage @navegacao
  Cenário: Validar título da página inicial
    Então o título da página deve conter "SumUp"

  @homepage @menu
  Cenário: Validar menu de navegação está visível
    Então o menu de navegação deve estar visível

  @homepage @cta
  Cenário: Validar botão CTA principal está presente
    Então o botão CTA principal deve estar visível

  @homepage @visual
  Cenário: Validar elementos visuais críticos da homepage
    Então o logo da SumUp deve estar visível
    E o header deve estar visível
    E o título principal deve estar visível
