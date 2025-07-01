document.addEventListener("DOMContentLoaded", () => {
  // 1. Verifica o estado do login e atualiza o cabeçalho
  updateHeader();

  // 2. Inicializa o carrossel de imagens
  initializeCarousel();

  // 3. Adiciona funcionalidade ao botão de logout
  const logoutButton = document.getElementById("logout-button");
  if (logoutButton) {
    logoutButton.addEventListener("click", () => {
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      window.location.reload(); // Recarrega a página para refletir o estado de logout
    });
  }

  // 4. Atualiza o contador do carrinho
  updateCartCount();
});

function updateHeader() {
  const token = localStorage.getItem("token");
  const userString = localStorage.getItem("user");

  const loginLink = document.getElementById("login-link");
  const userInfo = document.getElementById("user-info");
  const userEmail = document.getElementById("user-email");

  if (token && userString) {
    const user = JSON.parse(userString);
    // Usuário está logado
    if (loginLink) loginLink.classList.add("hidden");
    if (userInfo) userInfo.classList.remove("hidden");
    if (userEmail) userEmail.textContent = user.email; // Exibe o email do usuário
  } else {
    // Usuário não está logado
    if (loginLink) loginLink.classList.remove("hidden");
    if (userInfo) userInfo.classList.add("hidden");
  }
}

// Função para atualizar o contador do carrinho
function updateCartCount() {
  const cartCount = document.getElementById("cart-count");
  if (!cartCount) return;

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

function initializeCarousel() {
  const images = [
    "./img/cel1.webp",
    "./img/cel2.webp",
    "./img/cel3.webp",
    "./img/cel4.webp",
    "./img/cel5.webp",
    "./img/cel6.webp",
  ];
  let currentIndex = 0;

  const carouselInner = document.getElementById("carousel-inner");
  const prevBtn = document.getElementById("prev-btn");
  const nextBtn = document.getElementById("next-btn");
  const indicatorsContainer = document.getElementById("carousel-indicators");

  // Verifica se os elementos do carrossel existem antes de continuar
  if (!carouselInner || !prevBtn || !nextBtn || !indicatorsContainer) {
    return;
  }

  // Limpa o conteúdo anterior para evitar duplicatas
  carouselInner.innerHTML = "";
  indicatorsContainer.innerHTML = "";

  images.forEach((src, index) => {
    const img = document.createElement("img");
    img.src = src;
    img.className =
      "object-contain w-full h-full mx-auto absolute top-0 left-0 transition-opacity duration-500 ease-in-out";
    img.style.opacity = index === 0 ? "1" : "0";
    carouselInner.appendChild(img);

    const button = document.createElement("button");
    button.className = `w-3 h-3 rounded-full ${
      index === 0 ? "bg-white" : "bg-gray-400"
    }`;
    button.addEventListener("click", () => goToIndex(index));
    indicatorsContainer.appendChild(button);
  });

  const carouselImages = carouselInner.querySelectorAll("img");
  const indicatorButtons = indicatorsContainer.querySelectorAll("button");

  function updateCarousel() {
    carouselImages.forEach((img, index) => {
      img.style.opacity = index === currentIndex ? "1" : "0";
    });
    indicatorButtons.forEach((button, index) => {
      button.className = `w-3 h-3 rounded-full ${
        index === currentIndex ? "bg-white" : "bg-gray-400"
      }`;
    });
  }

  function nextImage() {
    currentIndex = (currentIndex + 1) % images.length;
    updateCarousel();
  }

  function prevImage() {
    currentIndex = (currentIndex - 1 + images.length) % images.length;
    updateCarousel();
  }

  function goToIndex(index) {
    currentIndex = index;
    updateCarousel();
  }

  nextBtn.addEventListener("click", nextImage);
  prevBtn.addEventListener("click", prevImage);

  setInterval(nextImage, 5000); // Auto-play
}
