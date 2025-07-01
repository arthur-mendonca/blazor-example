document.addEventListener("DOMContentLoaded", function () {
  // Elementos da UI
  const loginLink = document.getElementById("login-link");
  const userInfo = document.getElementById("user-info");
  const userEmail = document.getElementById("user-email");
  const logoutButton = document.getElementById("logout-button");
  const cartItemsContainer = document.getElementById("cart-items");
  const cartSubtotal = document.getElementById("cart-subtotal");
  const cartShipping = document.getElementById("cart-shipping");
  const cartTotal = document.getElementById("cart-total");
  const emptyCartMessage = document.getElementById("empty-cart-message");
  const cartContainer = document.getElementById("cart-container");
  const checkoutButton = document.getElementById("checkout-button");

  // Constantes
  const SHIPPING_COST = 0; // Valor do frete

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

  // Exibir os itens do carrinho
  displayCart();

  // Adicionar evento ao botão de checkout
  checkoutButton.addEventListener("click", handleCheckout);

  // === FUNÇÕES ===

  // Função para exibir os itens do carrinho
  function displayCart() {
    const cart = JSON.parse(localStorage.getItem("cart")) || [];

    if (cart.length === 0) {
      // Mostrar mensagem de carrinho vazio
      emptyCartMessage.classList.remove("hidden");
      cartContainer.classList.add("hidden");
      return;
    }

    // Mostrar conteúdo do carrinho
    emptyCartMessage.classList.add("hidden");
    cartContainer.classList.remove("hidden");

    // Limpar itens anteriores
    cartItemsContainer.innerHTML = "";

    // Variáveis para calcular o total
    let subtotal = 0;

    // Adicionar cada item à tabela
    cart.forEach((item) => {
      const itemSubtotal = item.preco * item.quantidade;
      subtotal += itemSubtotal;

      const row = document.createElement("tr");
      row.innerHTML = `
        <td class="py-4 px-6 border-b">
          <div class="flex items-center">
            <img src="./${item.imagem}" alt="${
        item.nome
      }" class="w-16 h-16 object-cover mr-4">
            <div>
              <p class="font-medium">${item.nome}</p>
              <p class="text-sm text-gray-600">ID: ${item.id}</p>
            </div>
          </div>
        </td>
        <td class="py-4 px-6 border-b text-center">
          <div class="flex items-center justify-center">
            <button class="decrease-qty bg-gray-200 px-2 py-1 rounded-l" data-id="${
              item.id
            }">-</button>
            <span class="item-qty px-4 py-1 bg-gray-100">${
              item.quantidade
            }</span>
            <button class="increase-qty bg-gray-200 px-2 py-1 rounded-r" data-id="${
              item.id
            }">+</button>
          </div>
        </td>
        <td class="py-4 px-6 border-b text-right">R$ ${item.preco.toFixed(
          2
        )}</td>
        <td class="py-4 px-6 border-b text-right">R$ ${itemSubtotal.toFixed(
          2
        )}</td>
        <td class="py-4 px-6 border-b text-center">
          <button class="remove-item text-red-500 hover:text-red-700" data-id="${
            item.id
          }">
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
            </svg>
          </button>
        </td>
      `;

      cartItemsContainer.appendChild(row);
    });

    // Atualizar os totais
    updateTotals(subtotal);

    // Adicionar event listeners para os botões
    addCartEventListeners();
  }

  // Adicionar event listeners para os botões de quantidade e remoção
  function addCartEventListeners() {
    // Botões de diminuir quantidade
    document.querySelectorAll(".decrease-qty").forEach((button) => {
      button.addEventListener("click", function () {
        const productId = this.getAttribute("data-id");
        updateItemQuantity(productId, -1);
      });
    });

    // Botões de aumentar quantidade
    document.querySelectorAll(".increase-qty").forEach((button) => {
      button.addEventListener("click", function () {
        const productId = this.getAttribute("data-id");
        updateItemQuantity(productId, 1);
      });
    });

    // Botões de remover item
    document.querySelectorAll(".remove-item").forEach((button) => {
      button.addEventListener("click", function () {
        const productId = this.getAttribute("data-id");
        removeItemFromCart(productId);
      });
    });
  }

  // Atualizar a quantidade de um item
  function updateItemQuantity(productId, change) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    const index = cart.findIndex((item) => item.id === productId);

    if (index !== -1) {
      // Atualizar a quantidade (mínimo 1)
      cart[index].quantidade = Math.max(1, cart[index].quantidade + change);

      // Salvar no localStorage
      localStorage.setItem("cart", JSON.stringify(cart));

      // Atualizar a exibição
      displayCart();
      updateCartCount();
    }
  }

  // Remover um item do carrinho
  function removeItemFromCart(productId) {
    let cart = JSON.parse(localStorage.getItem("cart")) || [];
    cart = cart.filter((item) => item.id !== productId);

    // Salvar no localStorage
    localStorage.setItem("cart", JSON.stringify(cart));

    // Atualizar a exibição
    displayCart();
    updateCartCount();
  }

  // Atualizar os totais do carrinho
  function updateTotals(subtotal) {
    const total = subtotal + SHIPPING_COST;

    cartSubtotal.textContent = `R$ ${subtotal.toFixed(2)}`;
    cartShipping.textContent = `R$ ${SHIPPING_COST.toFixed(2)}`;
    cartTotal.textContent = `R$ ${total.toFixed(2)}`;
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

  // Função para processar o checkout
  function handleCheckout() {
    const cart = JSON.parse(localStorage.getItem("cart")) || [];

    if (cart.length === 0) {
      alert("Seu carrinho está vazio!");
      return;
    }

    // Verificar se o usuário está logado
    if (!user || !user.email) {
      alert("Você precisa fazer login para finalizar a compra!");
      window.location.href = "login.html";
      return;
    }

    // Aqui você implementaria a lógica de checkout real
    // Por enquanto, apenas simulamos com um alerta
    alert(
      "Pedido finalizado com sucesso! Em um sistema real, você seria redirecionado para a página de pagamento."
    );

    // Limpar o carrinho após o checkout
    localStorage.removeItem("cart");

    // Redirecionar para a página inicial ou de confirmação
    window.location.href = "index.html";
  }
});
