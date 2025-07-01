document.addEventListener("DOMContentLoaded", function () {
  const productsGrid = document.getElementById("products-grid");
  const categoryTitle = document.getElementById("category-title");
  const loginLink = document.getElementById("login-link");
  const userInfo = document.getElementById("user-info");
  const userEmail = document.getElementById("user-email");
  const logoutButton = document.getElementById("logout-button");

  // --- Lógica de Autenticação da Navbar ---
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

  // --- Lógica para buscar e exibir produtos ---
  const params = new URLSearchParams(window.location.search);
  const category = params.get("categoria");

  if (category) {
    // Deixa a primeira letra maiúscula para o título
    categoryTitle.textContent =
      category.charAt(0).toUpperCase() + category.slice(1);
    fetchProducts(category);
  } else {
    categoryTitle.textContent = "Todos os Produtos";
    fetchProducts(); // Busca todos se nenhuma categoria for especificada
  }

  async function fetchProducts(categoryName) {
    // ATENÇÃO: Ajuste a URL base da API se for diferente.
    let url = "http://localhost:5284/api/produtos";
    if (categoryName) {
      // O endpoint da API espera o nome da categoria para filtrar
      url += `/categoria/${encodeURIComponent(categoryName)}`;
    }

    try {
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error("Erro ao buscar produtos: " + response.statusText);
      }
      const products = await response.json();
      displayProducts(products);
    } catch (error) {
      console.error("Falha na requisição:", error);
      productsGrid.innerHTML =
        '<p class="text-red-500">Não foi possível carregar os produtos. Verifique a conexão com a API.</p>';
    }
  }

  function displayProducts(products) {
    if (products.length === 0) {
      productsGrid.innerHTML =
        "<p>Nenhum produto encontrado nesta categoria.</p>";
      return;
    }

    productsGrid.innerHTML = "";
    products.forEach((product) => {
      const card = `
        <div class="bg-white rounded-lg shadow-md overflow-hidden transform hover:scale-105 transition-transform duration-300">
          <a href="produto.html?id=${product.id}">
            <img src="./${product.imagem}" alt="${
        product.nome
      }" class="w-full h-48 object-cover">
          </a>
          <div class="p-4">
            <a href="produto.html?id=${product.id}" class="hover:text-sky-500">
              <h3 class="text-lg font-semibold">${product.nome}</h3>
            </a>
            <p class="text-gray-600 mt-1">${product.descricao || ""}</p>
            <div class="mt-4 flex justify-between items-center">
              <span class="text-xl font-bold text-sky-500">R$ ${product.preco.toFixed(
                2
              )}</span>
              <div class="flex space-x-2">
                <button 
                  class="add-to-cart-btn bg-green-500 text-white font-bold py-2 px-3 rounded hover:bg-green-600" 
                  data-id="${product.id}" 
                  data-nome="${product.nome}" 
                  data-preco="${product.preco}" 
                  data-imagem="${product.imagem}">
                  <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 inline-block" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                  </svg>
                </button>
                <a href="produto.html?id=${
                  product.id
                }" class="bg-sky-500 text-white font-bold py-2 px-4 rounded hover:bg-sky-600">Ver detalhes</a>
              </div>
            </div>
          </div>
        </div>
      `;
      productsGrid.innerHTML += card;
    });

    // Adicionar event listeners para os botões de "Adicionar ao Carrinho"
    document.querySelectorAll(".add-to-cart-btn").forEach((button) => {
      button.addEventListener("click", function () {
        const productId = this.getAttribute("data-id");
        const productName = this.getAttribute("data-nome");
        const productPrice = parseFloat(this.getAttribute("data-preco"));
        const productImage = this.getAttribute("data-imagem");

        // Adicionar ao carrinho
        addToCart({
          id: productId,
          nome: productName,
          preco: productPrice,
          imagem: productImage,
          quantidade: 1,
        });

        // Exibir uma notificação visual
        showAddToCartNotification(productName);
      });
    });
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
      // Se não existe, adiciona ao carrinho
      cart.push(product);
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
    const cartCount = document.getElementById("cart-count");
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

  // Atualizar o contador do carrinho ao carregar a página
  updateCartCount();
});
