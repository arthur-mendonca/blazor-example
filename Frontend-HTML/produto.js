document.addEventListener("DOMContentLoaded", function () {
  const loginLink = document.getElementById("login-link");
  const userInfo = document.getElementById("user-info");
  const userEmail = document.getElementById("user-email");
  const logoutButton = document.getElementById("logout-button");
  const produtoNome = document.getElementById("produto-nome");
  const produtoImagem = document.getElementById("produto-imagem");
  const produtoDescricao = document.getElementById("produto-descricao");
  const produtoPreco = document.getElementById("produto-preco");
  const produtoContainer = document.getElementById("produto-container");

  // --- Lógica de Autenticação da Navbar (similar ao home.js) ---
  const user = JSON.parse(localStorage.getItem("user"));
  if (user && user.email) {
    loginLink.classList.add("hidden");
    userInfo.classList.remove("hidden");
    userEmail.textContent = user.email;
  } else {
    loginLink.classList.remove("hidden");
    userInfo.classList.add("hidden");
  }

  logoutButton.addEventListener("click", () => {
    localStorage.removeItem("user");
    localStorage.removeItem("token");
    window.location.href = "index.html";
  });

  // --- Lógica para carregar detalhes do produto ---
  // Obter o ID do produto da URL
  const params = new URLSearchParams(window.location.search);
  const produtoId = params.get("id");

  if (!produtoId) {
    produtoContainer.innerHTML =
      '<p class="text-red-500 text-xl">ID do produto não fornecido.</p>';
    return;
  }

  fetchProdutoDetails(produtoId);

  async function fetchProdutoDetails(id) {
    // ATENÇÃO: Ajuste a URL base da API se for diferente.
    const url = `http://localhost:5284/api/produtos/${id}`;

    try {
      const response = await fetch(url);

      if (!response.ok) {
        if (response.status === 404) {
          produtoContainer.innerHTML =
            '<p class="text-red-500 text-xl">Produto não encontrado.</p>';
        } else {
          throw new Error(
            "Erro ao buscar detalhes do produto: " + response.statusText
          );
        }
        return;
      }

      const produto = await response.json();
      displayProdutoDetails(produto);
    } catch (error) {
      console.error("Falha na requisição:", error);
      produtoContainer.innerHTML =
        '<p class="text-red-500 text-xl">Não foi possível carregar os detalhes do produto. Verifique a conexão com a API.</p>';
    }
  }

  function displayProdutoDetails(produto) {
    // Atualizar o título da página
    document.title = `${produto.nome} - Shop+`;

    // Preencher os dados na página
    produtoNome.textContent = produto.nome;
    produtoImagem.src = `./${produto.imagem}`;
    produtoImagem.alt = produto.nome;
    produtoDescricao.textContent =
      produto.descricao || "Descrição não disponível";
    produtoPreco.textContent = `R$ ${produto.preco.toFixed(2)}`;
  }
});
