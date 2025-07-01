document.addEventListener("DOMContentLoaded", function () {
  const productsGrid = document.getElementById("products-grid");
  const categoryTitle = document.getElementById("category-title");
  const loginLink = document.getElementById("login-link");
  const userInfo = document.getElementById("user-info");
  const userEmail = document.getElementById("user-email");
  const logoutButton = document.getElementById("logout-button");

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

    productsGrid.innerHTML = ""; // Limpa a área antes de adicionar os novos produtos
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
              <a href="produto.html?id=${
                product.id
              }" class="bg-sky-500 text-white font-bold py-2 px-4 rounded hover:bg-sky-600">Ver detalhes</a>
            </div>
          </div>
        </div>
      `;
      productsGrid.innerHTML += card;
    });
  }
});
