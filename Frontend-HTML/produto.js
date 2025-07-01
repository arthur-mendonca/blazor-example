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
  const addToCartButton = document.getElementById("add-to-cart-button");
  const cartCount = document.getElementById("cart-count");

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

  // Atualizar contador do carrinho
  updateCartCount();

  // --- Lógica para carregar detalhes do produto ---
  // Obter o ID do produto da URL
  const params = new URLSearchParams(window.location.search);
  const produtoId = params.get("id");

  if (!produtoId) {
    produtoContainer.innerHTML =
      '<p class="text-red-500 text-xl">ID do produto não fornecido.</p>';
    return;
  }

  let produtoAtual = null; // Armazenar dados do produto atual

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
      produtoAtual = produto; // Armazenar produto atual
      displayProdutoDetails(produto);

      // Adicionar evento ao botão de adicionar ao carrinho
      addToCartButton.addEventListener("click", () => {
        addToCart(produtoAtual);
        showAddToCartNotification(produtoAtual.nome);
      });
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

  // Função para adicionar produto ao carrinho (localStorage)
  function addToCart(product) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];

    // Verificar se o produto já está no carrinho
    const existingProductIndex = cart.findIndex(
      (item) => item.id === product.id
    );

    if (existingProductIndex >= 0) {
      // Se já existe, incrementa a quantidade
      cart[existingProductIndex].quantidade += 1;
    } else {
      // Se não existe, adiciona ao carrinho com quantidade 1
      cart.push({
        id: product.id,
        nome: product.nome,
        preco: product.preco,
        imagem: product.imagem,
        quantidade: 1,
      });
    }

    // Salvar no localStorage
    localStorage.setItem("cart", JSON.stringify(cart));

    // Atualizar o contador do carrinho
    updateCartCount();
  }

  // Função para mostrar notificação de "Produto Adicionado"
  function showAddToCartNotification(productName) {
    // Criar elemento de notificação
    const notification = document.createElement("div");
    notification.className =
      "fixed top-16 right-4 bg-green-500 text-white py-2 px-4 rounded shadow-lg z-50";
    notification.textContent = `${productName} adicionado ao carrinho!`;

    // Adicionar ao DOM
    document.body.appendChild(notification);

    // Remover após 3 segundos
    setTimeout(() => {
      notification.classList.add(
        "opacity-0",
        "transition-opacity",
        "duration-500"
      );
      setTimeout(() => {
        document.body.removeChild(notification);
      }, 500);
    }, 2500);
  }

  // Função para atualizar o contador do carrinho
  function updateCartCount() {
    const cart = JSON.parse(localStorage.getItem("cart")) || [];

    // Calcular quantidade total de itens
    const itemCount = cart.reduce((total, item) => total + item.quantidade, 0);

    if (itemCount > 0) {
      cartCount.textContent = itemCount;
      cartCount.classList.remove("hidden");
    } else {
      cartCount.classList.add("hidden");
    }
  }
});
