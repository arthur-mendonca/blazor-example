document.addEventListener("DOMContentLoaded", () => {
  const loginButton = document.getElementById("loginButton");
  const emailInput = document.getElementById("email");
  const senhaInput = document.getElementById("senha");
  const mensagemDiv = document.getElementById("mensagem");

  const API_URL = "http://localhost:5284/api/usuarios/login";

  loginButton.addEventListener("click", async () => {
    const email = emailInput.value;
    const senha = senhaInput.value;

    if (!email || !senha) {
      mensagemDiv.textContent = "Email e senha são obrigatórios.";
      return;
    }

    // Limpa a mensagem e mostra um estado de "carregando"
    mensagemDiv.textContent = "";
    loginButton.textContent = "Aguarde...";
    loginButton.disabled = true;

    try {
      const response = await fetch(API_URL, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ email, senha }),
      });

      const data = await response.json();

      if (!response.ok) {
        // Se a API retornar um erro (401, 500, etc.), mostra a mensagem de erro
        throw new Error(data.message || "Erro ao tentar fazer login.");
      }

      // Sucesso!
      console.log("Token recebido:", data.token);
      localStorage.setItem("token", data.token);
      localStorage.setItem("user", JSON.stringify(data.usuario));

      mensagemDiv.style.color = "green";
      mensagemDiv.textContent = "Login realizado com sucesso!";

      // Esconde o login e mostra a home
      document.getElementById("login-container").classList.add("hidden");
      document.getElementById("home-container").classList.remove("hidden");

      initializeCarousel();
    } catch (error) {
      mensagemDiv.style.color = "red";
      mensagemDiv.textContent = error.message;
    } finally {
      // Reativa o botão
      loginButton.textContent = "Entrar";
      loginButton.disabled = false;
    }
  });

  function initializeCarousel() {
    const images = [
      "/img/cel1.webp",
      "/img/cel2.webp",
      "/img/cel3.webp",
      "/img/cel4.webp",
      "/img/cel5.webp",
      "/img/cel6.webp",
    ];
    let currentIndex = 0;

    const carouselInner = document.getElementById("carousel-inner");
    const prevBtn = document.getElementById("prev-btn");
    const nextBtn = document.getElementById("next-btn");
    const indicatorsContainer = document.getElementById("carousel-indicators");

    // Cria os elementos de imagem e indicadores
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

    // Opcional: Auto-play
    setInterval(nextImage, 5000); // Muda a cada 5 segundos
  }

  // Verifica se o usuário já está logado ao carregar a página
  const token = localStorage.getItem("token");
  if (token) {
    document.getElementById("login-container").classList.add("hidden");
    document.getElementById("home-container").classList.remove("hidden");
    initializeCarousel();
  }
});
